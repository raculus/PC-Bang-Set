using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace Game_Set
{
    public partial class Form_Main : Form
    {

        public Form_Main()
        {
            InitializeComponent();
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add("오버워치 설정");
            checkedListBox1.Items.Add("더파이널스 설정");
            checkedListBox1.Items.Add("에이펙스 설정");
            checkedListBox1.Items.Add("포인터 정확도 끄기");
            checkedListBox1.Items.Add("불필요 프로세스 종료");
            checkedListBox1.Items.Add("배틀넷 켜기");
            checkedListBox1.Items.Add("배틀그라운드 인트로 제거");
            checkedListBox1.Items.Add("로지텍OMM 다운로드");
            checkedListBox1.Items.Add("서비스 중지");
            checkedListBox1.Items.Add("크롬 확장프로그램");

            for (int i=0; i<checkedListBox1.Items.Count; i++)
            { 
                checkedListBox1.SetItemChecked(i, true);
            }

            try
            {
                trackbar_Brightness.Value = Monitor.Get();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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

        private async Task settings_overwatchAsync()
        {
            const string URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/Overwatch/Settings_v0.ini";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\Overwatch\Settings\";
            ifNotExistDir(path);
            path += @"Settings_v0.ini";
            await Downloader(URL, path);
        }

        private async Task settings_the_finals()
        {
            const string URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/TheFinals/GameUserSettings.ini";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += @"\Discovery\Saved\Config\WindowsClient";
            ifNotExistDir(path);
            path += @"\GameUserSettings.ini";
            await Downloader(URL, path);
        }
        static async Task Downloader(string url, string path)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    try
                    {
                        using (var fileStream = File.Create(path))
                        using (var httpStream = await response.Content.ReadAsStreamAsync())
                        {
                            await httpStream.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                        Debug.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
        static async Task<string> GetUrlContentAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
        }
        private async Task kill_processesAsync()
        {
            const string URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/kill_process.txt";
            string useless = await GetUrlContentAsync(URL);
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
        private void get_chrome_extension()
        {
            List<string> extensions = new List<string>();
            extensions.Add("https://chrome.google.com/webstore/detail/dark-reader/eimadpbcbfnmbkopoojfekhnkhdbieeh");
            extensions.Add("https://chrome.google.com/webstore/detail/ublock-origin/cjpalhdlnbpafiamejdnhcphjbkeiagm");
            extensions.Add("https://chromewebstore.google.com/detail/better-youtube-shorts/pehohlhkhbcfdneocgnfbnilppmfncdg?pli=1");
            foreach(string extension in extensions)
            {
                var startInfo = new ProcessStartInfo("chrome.exe");
                startInfo.Arguments = extension;
                Process.Start(startInfo);
            }
        }
        private async Task apex_settingsAsync()
        {
            var autoexecPath = @"\cfg\autoexec.cfg";
            var superglidePath = @"\cfg\superglide.cfg";
            string superglide = "bind \"mouse1\" \"+jump; fps_max 30\" 0\r\nbind \"mouse2\" \"+duck; fps_max 189; exec autoexec.cfg\" 0";

            // cfg 파일 설정
            const string URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/Apex/autoexec.cfg";
            //var list = everything.Search(@"Apex\r5apex.exe");
            var list = new List<string>();
            if(list.Count > 0)
            {
                string apex_path = list[0].Replace(@"\r5apex.exe", "");
                await Downloader(URL, apex_path + autoexecPath);
                if(new FileInfo(apex_path + superglidePath).Exists)
                    File.WriteAllText(apex_path + superglidePath, superglide);
            }
            //list = everything.Search(@"Apex Legends\r5apex.exe");
            if (list.Count > 0)
            {
                string apex_path = list[0].Replace(@"\r5apex.exe", "");
                await Downloader(URL, apex_path + autoexecPath);
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

            const string VIDEOCONFIG_URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/Apex/videoconfig.txt";
            await Downloader(VIDEOCONFIG_URL, userPath);

        }
        private void remove_pubg_intro()
        {
            Debug.WriteLine($"remove_pubg_intro");
        }
        private static async Task download_omm()
        {
            const string URL = "https://github.com/raculus/PC-Bang-Set/raw/master/cfg/OnboardMemoryManager.exe";
            //Downloader(URL, "OMM.exe");
            await Downloader(URL, "OMM.exe");
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

        private async void button_Apply_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = checkedListBox1.CheckedItems.Count;
            List<Thread> threads = new List<Thread>();

            checkedListBox1.SelectedItem = null;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if(!checkedListBox1.GetItemChecked(i))
                {
                    continue;
                }
                string checkedItem = checkedListBox1.Items[i].ToString();
                checkedListBox1.SelectedItem = checkedItem;

                progressBar1.PerformStep();
                switch (checkedItem)
                {
                    case "오버워치 설정":
                        await settings_overwatchAsync();
                        break;
                    case "포인터 정확도 끄기":
                        PointerAccel.Set(false);
                        break;
                    case "더파이널스 설정":
                        await settings_the_finals();
                        break;
                    case "불필요 프로세스 종료":
                        await kill_processesAsync();
                        break;
                    case "배틀넷 켜기":
                        threads.Add(new Thread(RunBattlenet));
                        break;
                    case "에이펙스 설정":
                        await apex_settingsAsync();
                        break;
                    case "배틀그라운드 인트로 제거":
                        threads.Add(new Thread(remove_pubg_intro));
                        break;
                    case "로지텍OMM 다운로드":
                        await download_omm();
                        break;
                    case "서비스 중지":
                        threads.Add(new Thread(stopServices));
                        break;
                    case "크롬 확장프로그램":
                        threads.Add(new Thread(get_chrome_extension));
                        break;
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
            checkedListBox1.SelectedItem = null;
        }

        private void checkBox_checkAll_CheckedChanged(object sender, EventArgs e)
        {
            for(int i=0; i< checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkbox_CheckAll.Checked);
            }
        }

        private void trackbar_Brightness_ValueChanged(object sender, EventArgs e)
        {
            label_MonitorBright.Text = "모니터 밝기: " + trackbar_Brightness.Value.ToString() + "%";
            Monitor.SetBrightness(trackbar_Brightness.Value);
        }
    }
}
