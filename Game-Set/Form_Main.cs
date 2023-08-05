using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Policy;

namespace Game_Set
{
    public partial class Form_Main : Form
    {
        int freq = Gpu.DisplayFreq();
        readonly Everything everything = new Everything();

        [Flags]
        public enum SPIF
        {
            None = 0x00,
            /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
            SPIF_UPDATEINIFILE = 0x01,
            /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
            SPIF_SENDCHANGE = 0x02,
            /// <summary>Same as SPIF_SENDCHANGE.</summary>
            SPIF_SENDWININICHANGE = 0x02
        }

        // http://stackoverflow.com/questions/24737775/toggle-enhance-pointer-precision
        // 포인터 정확도 향상 끄기용
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoGet(uint action, uint param, IntPtr vparam, SPIF fWinIni);
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

        public const UInt32 SPI_GETMOUSE = 0x0003;
        public const UInt32 SPI_SETMOUSE = 0x0004;
        // 창 사이즈 조절용
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static bool PointerAccel(bool b)
        {
            int[] mouseParams = new int[3];
            // Get the current values.
            SystemParametersInfoGet(SPI_GETMOUSE, 0, GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), 0);
            // Modify the acceleration value as directed.
            mouseParams[2] = b ? 1 : 0;
            // Update the system setting.
            return SystemParametersInfoSet(SPI_SETMOUSE, 0, GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), SPIF.SPIF_SENDCHANGE);
        }


        private void ChangeWindowSize(string title, int width, int height)
        {
            Process[] proc = Process.GetProcessesByName(title);
            foreach (Process p in proc)
            {
                IntPtr handle = p.MainWindowHandle;
                MoveWindow(handle, 3, 3, width, height, true);
            }
        }

        public Form_Main()
        {
            InitializeComponent();
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (!(new FileInfo("Everything.exe").Exists))
            {
                File.WriteAllBytes("Everything.exe", PC_Bang_Set.Properties.Resources.Everything);
            }
            if (!(new FileInfo("Everything64.dll").Exists))
            {
                File.WriteAllBytes("Everything64.dll", PC_Bang_Set.Properties.Resources.Everything64);
            }

            checkedListBox1.Items.Add("오버워치 그래픽 설정");
            checkedListBox1.Items.Add("포인터 정확도 끄기");
            checkedListBox1.Items.Add("지포스 드라이버 다운로드");
            checkedListBox1.Items.Add("불필요 프로세스 종료");
            checkedListBox1.Items.Add("배틀넷 켜기");
            checkedListBox1.Items.Add("에이펙스 설정");
            checkedListBox1.Items.Add("오버워치 더 작은창모드");
            checkedListBox1.Items.Add("배틀그라운드 인트로 제거");
            checkedListBox1.Items.Add("로지텍OMM 다운로드");
            checkedListBox1.Items.Add("서비스 중지");
            checkedListBox1.Items.Add("크롬 확장프로그램");

            for(int i=0; i<checkedListBox1.Items.Count; i++)
            {
                bool isCheck = true;
                string itemName = checkedListBox1.Items[i].ToString();
                switch (itemName)
                {
                    case "지포스 드라이버 다운로드":
                        isCheck = false; break;
                }
                checkedListBox1.SetItemChecked(i, isCheck);
            }

            trackbar_Brightness.Value = Monitor.Get();
        }
        private string krokr(string sub)
        {
            string url = @"http://";
            url += sub + ".kotlin.kro.kr";
            return url;
        }
        private void ifNotExistDir(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (!(di.Exists))
            {
                di.Create();
            }
        }
        private void stopServices()
        {
            string[] services = { "WSearch", "SysMain", "LanmanWorkstation" };
            foreach(string service in services)
            {
                ServiceController sc = new ServiceController(service);
                if(sc.CanStop)
                {
                    sc.Stop();
                }
            }
        }

        private void apply_ow_setting()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Overwatch\Settings\";
            ifNotExistDir(path);
            path += @"Settings_v0.ini";
            Downloader(krokr("ow-setting"), path);
            
            var gpuInfo = Gpu.GpuInfo();

            IniFile ini = new IniFile();
            ini.Load(path);
            ini["GPU.6"]["GPUDeviceID"] = "\"" + gpuInfo.DeviceID + "\"";
            ini["GPU.6"]["GPUName"] = "\"" + gpuInfo.Name + "\"";
            ini["GPU.6"]["GPUVenderID"] = "\"" + gpuInfo.VenderID + "\"";

            ini["Render.13"]["FrameRateCap"] = freq - 2;
            ini["Render.13"]["FullScreenRefresh"] = freq;
            ini.Save(path);
        }

        private void Downloader(string url, string path)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                try
                {
                    client.DownloadFile(new Uri(url), path);
                }
                catch(WebException e)
                {
                    MessageBox.Show(e.InnerException.Message);
                    Debug.WriteLine("Downloader Error!!");
                    Debug.WriteLine("URL: "+url+" / Path: " + path);
                }
            }
        }
        private string readToURL(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string str = wc.DownloadString(url);
            return str;
        }
        private void kill_useless_process()
        {
            string useless = readToURL(krokr("process"));
            foreach(string name in useless.Split('\n'))
            {
                if (name == "")
                    continue;
                Console.WriteLine("Process Kill: "+name);
                Process[] processList = Process.GetProcessesByName(name);
                if(processList.Length > 0)
                    processList[0].Kill();
            }
        }
        private void gpu_driver()
        {
            string url = krokr("gpu");
            var startInfo = new ProcessStartInfo("chrome.exe");
            startInfo.Arguments = url;
            Process.Start(startInfo);
        }
        private void get_chrome_extension()
        {
            List<string> extensions = new List<string>();
            extensions.Add("https://chrome.google.com/webstore/detail/dark-reader/eimadpbcbfnmbkopoojfekhnkhdbieeh");
            extensions.Add("https://chrome.google.com/webstore/detail/ublock-origin/cjpalhdlnbpafiamejdnhcphjbkeiagm");
            extensions.Add("https://chrome.google.com/webstore/detail/better-youtube-shorts/icnidlkdlledahfgejnagmhgaeijokcp");
            foreach(string extension in extensions)
            {
                var startInfo = new ProcessStartInfo("chrome.exe");
                startInfo.Arguments = extension;
                Process.Start(startInfo);
            }
        }
        private void apex_settings()
        {
            // 시작옵션 클립보드에 복사
            string str = readToURL(krokr("apex-startopt"));
            str += " +fps_max \"";
            str += freq - 2 + "\"";

            Clipboard.SetText(str);


            var autoexecPath = @"\cfg\autoexec.cfg";
            var superglidePath = @"\cfg\superglide.cfg";
            string superglide = "bind \"mouse1\" \"+jump; fps_max 30\" 0\r\nbind \"mouse2\" \"+duck; fps_max 189; exec autoexec.cfg\" 0";

            // cfg 파일 설정
            var list = everything.Search(@"Apex\r5apex.exe");
            if(list.Count > 0)
            {
                string apex_path = list[0].Replace(@"\r5apex.exe", "");

                Downloader(krokr("apex-autoexec"), apex_path + autoexecPath);
                if(new FileInfo(apex_path + superglidePath).Exists)
                    File.WriteAllText(apex_path + superglidePath, superglide);
            }
            list = everything.Search(@"Apex Legends\r5apex.exe");
            if (list.Count > 0)
            {
                string apex_path = list[0].Replace(@"\r5apex.exe", "");
                Downloader(krokr("apex-autoexec"), apex_path + autoexecPath);
                if (new FileInfo(apex_path + superglidePath).Exists)
                    File.WriteAllText(apex_path + superglidePath, superglide);
            }

            //videoconfig.txt 설정 (그래픽 옵션)
            string userPath = System.Environment.GetEnvironmentVariable("USERPROFILE");
            userPath += @"\Saved Games\Respawn\Apex\local\";
            ifNotExistDir(userPath);
            userPath += "videoconfig.txt";
            Debug.WriteLine(userPath);

            FileReadOnly(userPath, false);
            Downloader(krokr("apex-setting"), userPath);

        }
        private void small_overwatch()
        {
            ChangeWindowSize("overwatch", 640, 480);
            
        }
        private void remove_pubg_intro()
        {
            var list = everything.Search(@"\TslGame\Content\Movies");
            if(list.Count > 0)
            {
                string path = list[0];

                var di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    di.Delete(true);
                    di.Create();
                }
            }
        }
        private void download_omm()
        {
            string url = krokr("omm");
            Downloader(url, "OMM.exe");
            Process.Start("OMM.exe");
        }
        private void FileReadOnly(string path, bool isReadOnly)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.IsReadOnly = isReadOnly;
            }
        }
        private void RunBattlenet()
        {
            string battlenet = @"C:\Program Files (x86)\Battle.net\Battle.net Launcher.exe";
            if (File.Exists(battlenet))
                Process.Start(battlenet);
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = checkedListBox1.CheckedItems.Count;
            List<Thread> threads = new List<Thread>();

            foreach(string checkedItem in checkedListBox1.CheckedItems)
            {
                progressBar1.PerformStep();
                int index = checkedListBox1.Items.IndexOf(checkedItem);
                if (index == 0)
                {
                    //오버워치 그래픽 설정
                    threads.Add(new Thread(apply_ow_setting));
                }
                else if (index == 1)
                {
                    //포인터 정확도 끄기
                    PointerAccel(false);
                }
                else if (index == 2)
                {
                    //지포스 드라이버 다운로드
                    threads.Add(new Thread(gpu_driver));
                }
                else if (index == 3)
                {
                    //불필요 프로세스 끄기
                    threads.Add(new Thread(kill_useless_process));
                }
                else if (index == 4)
                {
                    threads.Add(new Thread(RunBattlenet));
                }
                else if (index == 5)
                {
                    threads.Add(new Thread(apex_settings));
                }
                else if (index == 6)
                {
                    threads.Add(new Thread(small_overwatch));
                }
                else if (index == 7)
                {
                    threads.Add(new Thread(remove_pubg_intro));
                }
                else if (checkedItem == "로지텍OMM 다운로드")
                {
                    threads.Add(new Thread(download_omm));
                }
                else if (checkedItem == "서비스 중지")
                {
                    threads.Add(new Thread(stopServices));
                }
                else if (checkedItem == "크롬 확장프로그램")
                {
                    threads.Add(new Thread(get_chrome_extension));
                }
            }
            foreach(var thread in threads)
            {
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            Task.WaitAll();
            Debug.WriteLine("Done!");
        }

        private void checkBox_checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_CheckAll.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void trackbar_Brightness_MouseUp(object sender, MouseEventArgs e)
        {
            Monitor.SetBrightness(trackbar_Brightness.Value);
        }

        private void trackbar_Brightness_KeyUp(object sender, KeyEventArgs e)
        {
            Monitor.SetBrightness(trackbar_Brightness.Value);
        }

        private void trackbar_Brightness_ValueChanged(object sender, EventArgs e)
        {
            label_MonitorBright.Text = "모니터 밝기: " + trackbar_Brightness.Value.ToString() + "%";
        }
    }
}
