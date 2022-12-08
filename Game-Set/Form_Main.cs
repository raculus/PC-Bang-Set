﻿using System;
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

namespace Game_Set
{
    public partial class Form_Main : Form
    {
        int freq = new Get().DisplayFreq();
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
            if(!(new FileInfo("Everything.exe").Exists))
            {
                File.WriteAllBytes("Everything.exe", PC_Bang_Set.Properties.Resources.Everything);
            }
            if(!(new FileInfo("Everything64.dll").Exists))
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
            checkedListBox1.Items.Add("서든 설정");
            checkedListBox1.Items.Add("서비스 중지");
            checkedListBox1.Items.Add("SAA 다운로드");

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            checkedListBox1.SetItemChecked(checkedListBox1.Items.Count-1, false);
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
            
                Get get = new Get();
                var gpuInfo = get.GpuInfo();

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

            FileInfo fi = new FileInfo(userPath);
            if (fi.Exists)
            {
                fi.IsReadOnly = false;
            }
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
        private void download_saauto()
        {
            Downloader(krokr("sa"), Application.StartupPath + @"\notepad.exe");
        }
        private void download_apexRecoil()
        {
            Downloader(krokr("apex-recoil"), Application.StartupPath + @"\NRS.exe");
        }
        private void sa_set()
        {
            string saPath = "";
            var list = everything.Search(@"SuddenAttack\suddenattack.exe");
            if(list.Count> 0)
            {
                saPath = list[0].Replace(@"\suddenattack.exe", "");
            }
            if(saPath == "")
            {
                string msg = "서든어택 폴더를 선택해 주세요";
                MessageBox.Show(msg);
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;
                    if (path.Contains("SuddenAttack"))
                    {
                        saPath = path;
                    }
                    else
                    {
                        MessageBox.Show(msg);
                        return;
                    }
                    Debug.WriteLine("Selected path");
                }
                else
                {
                    return;
                }
            }
            if (saPath == "")
            {
                return;
            }
            Debug.WriteLine(saPath);
            var sa_world_snd = saPath + @"\game\sa_worlds\snd";
            var di = new DirectoryInfo(sa_world_snd);
            if(di.Exists)
            {
                Directory.Delete(sa_world_snd, true);
                Directory.CreateDirectory(sa_world_snd);
            }

            //캐릭터 호흡 효과음 삭제
            foreach(var dir in Directory.GetDirectories(saPath+ @"\game\sa_characters\customvoice\", "*"))
            {
                var path = dir + @"\breath";
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    FileInfo[] fi = directoryInfo.GetFiles();
                    foreach (var file in fi)
                    {
                        file.Delete();
                    }
                }
            }

            //display.cfg 대체(직접입력, 해상도, 수직동기화)
            string display_path = saPath + @"\display.cfg";
            Downloader(krokr("sa-display"), display_path);

            //profile\player.txt 대체(사운드, 해상도, 수직동기화, 감도)
            string player_path = saPath + @"\profiles\player.txt";
            Downloader(krokr("sa-player"), player_path);
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
                else if (index == 9)
                {
                    threads.Add(new Thread(sa_set));
                }
                else if (checkedItem == "서비스 중지")
                {
                    threads.Add(new Thread(stopServices));
                }
                else if(checkedItem == "NRS 다운로드")
                {
                    new Thread(download_apexRecoil).Start();
                }
                else
                {
                    threads.Add(new Thread(download_saauto));
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
            if (checkBox_checkAll.Checked)
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
    }
}
