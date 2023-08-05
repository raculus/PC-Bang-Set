using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

    class Gpu
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
     string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;

        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {

            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;

        }
        public static int DisplayFreq()
        {
            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                i++;
            }
            int freq = vDevMode.dmDisplayFrequency;
            Console.WriteLine("Freq: " + freq);
            return freq;
        }

    private static string GetComponent(string hwclass, string syntax)
    {
        string result = null;
        ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + hwclass);
        foreach (ManagementObject mj in mos.Get())
        {
            result = Convert.ToString(mj[syntax]);
        }
        return result;
    }
    public static (string Name, string VenderID, string DeviceID) GpuInfo()
    {
        string pnpId = GetComponent("Win32_VideoController", "PNPDeviceID");
        string[] idArr = pnpId.Split('&');
        string venderID = idArr[0].Split('_')[1];
        string deviceID = idArr[1].Split('_')[1];
        deviceID = Convert.ToInt32(deviceID, 16).ToString();
        venderID = Convert.ToInt32(venderID, 16).ToString();

        string name = GetComponent("Win32_VideoController", "Name");
        (string Name, string VenderID, string DeviceID) gpuInfo = (name, venderID, deviceID);
        return gpuInfo;
    }
}
