using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace kinect_sdk_example
{
    static class KeyPressEmulator
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        //const int VK_F5 = 0x27;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [STAThread]
        public static void setKeyPressed(int key, Boolean pressed)
        {
            Process[] processes = Process.GetProcessesByName("googleearth");

            foreach (Process proc in processes)
            {
                SetForegroundWindow(proc.MainWindowHandle);
                PostMessage(proc.MainWindowHandle, pressed ? WM_KEYDOWN : WM_KEYUP, key, 0);
            }


        }
    }
}
