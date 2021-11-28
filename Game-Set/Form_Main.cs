using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Set
{
    public partial class Form_Main : Form
    {
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
                MoveWindow(handle, 10, 10, width, height, true);
            }
        }

        private string GetComponent(string hwclass, string syntax)
        {
            string result = null;
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + hwclass);
            foreach(ManagementObject mj in mos.Get())
            {
                result = Convert.ToString(mj[syntax]);
            }
            return result;
        }
        private (string Name, string VenderID, string DeviceID) GetGpuInfo()
        {
            string pnpId = GetComponent("Win32_VideoController", "PNPDeviceID");
            string[] idArr = pnpId.Split('&');
            string venderID = idArr[0].Split('_')[1];
            string deviceID = idArr[1].Split('_')[1];
            deviceID = Convert.ToInt32(deviceID, 16).ToString();
            venderID = Convert.ToInt32(venderID, 16).ToString();

            string name = GetComponent("Win32_VideoController", "Name");
            (string Name,string VenderID, string DeviceID) gpuInfo = (name, venderID, deviceID);
            return gpuInfo;
        }

        public Form_Main()
        {
            InitializeComponent();
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add("오버워치 그래픽 설정");
            checkedListBox1.Items.Add("포인터 정확도 끄기");
            checkedListBox1.Items.Add("지포스 드라이버 다운로드");
            checkedListBox1.Items.Add("불필요 프로세스 종료");
            checkedListBox1.Items.Add("배틀넷 켜기");
            checkedListBox1.Items.Add("에이펙스 설정");
            checkedListBox1.Items.Add("오버워치 더 작은창모드");

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
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

        private void apply_ow_setting()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Overwatch\Settings\";
            ifNotExistDir(path);
            path += @"Settings_v0.ini";
            Downloader(krokr("ow-setting"), path);
            
            var gpuInfo = GetGpuInfo();

            IniFile ini = new IniFile();
            ini.Load(path);
            ini["GPU.6"]["GPUDeviceID"] = "\"" + gpuInfo.DeviceID + "\"";
            ini["GPU.6"]["GPUName"] = "\"" + gpuInfo.Name + "\"";
            ini["GPU.6"]["GPUVenderID"] = "\"" + gpuInfo.VenderID + "\"";
            ini.Save(path);
        }

        private void Downloader(string url, string path)
        {
            using (var client = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DownloadFile(url, path);
            }
        }
        private string readToURL(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string str = wc.DownloadString(url);
            return str;
        }
        private void kill_useless_process()
        {
            string useless = readToURL(krokr("process"));
            foreach(var name in useless.Split('\n'))
            {
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
            string userPath = System.Environment.GetEnvironmentVariable("USERPROFILE");
            userPath += @"\Saved Games\Respawn\Apex\local\";
            ifNotExistDir(userPath);
            userPath += "videoconfig.txt";

            FileInfo fi = new FileInfo(userPath);
            if (fi.Exists)
            {
                fi.IsReadOnly = false;
            }
            else
            {
                Downloader(krokr("apex-setting"), userPath);
            }
            fi.IsReadOnly = true;
        }
        private void small_overwatch()
        {
            ChangeWindowSize("overwatch", 640, 480);
            
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            foreach(string checkedItem in checkedListBox1.CheckedItems)
            {
                int index = checkedListBox1.Items.IndexOf(checkedItem);
                if (index == 0)
                {
                    //오버워치 그래픽 설정
                    apply_ow_setting();
                }
                else if (index == 1)
                {
                    //포인터 정확도 끄기
                    PointerAccel(false);
                }
                else if (index == 2)
                {
                    //지포스 드라이버 다운로드
                    gpu_driver();
                }
                else if (index == 3)
                {
                    //불필요 프로세스 끄기
                    kill_useless_process();
                }
                else if (index == 4)
                {
                    string battlenet = @"C:\Program Files (x86)\Battle.net\Battle.net Launcher.exe";
                    if (File.Exists(battlenet))
                        Process.Start(battlenet);
                    else
                    {
                    }
                }
                else if(index == 5)
                {
                    apex_settings();
                    Clipboard.SetText("+exec autoexec.cfg -dev");
                }
                else if(index == 6)
                {
                    small_overwatch();
                }
            }
        }
    }
}
