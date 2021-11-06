using System;
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
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoGet(uint action, uint param, IntPtr vparam, SPIF fWinIni);
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, SPIF fWinIni);

        public const UInt32 SPI_GETMOUSE = 0x0003;
        public const UInt32 SPI_SETMOUSE = 0x0004;

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

        private void apply_ow_setting()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Overwatch\Settings\Settings_v0.ini";
            Downloader(krokr("ow-setting"), path);
        }

        private void Downloader(string url, string path)
        {
            using (var client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DownloadFile(url, path);
            }
        }
        private string readToURL(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
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
            DirectoryInfo di = new DirectoryInfo(userPath);
            if(!(di.Exists))
            {
                di.Create();
            }
            userPath += "videoconfig.txt";

            Downloader(krokr("apex-setting"), userPath);
            FileInfo fi = new FileInfo(userPath);
            fi.IsReadOnly = true;


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
            }
        }
    }
}
