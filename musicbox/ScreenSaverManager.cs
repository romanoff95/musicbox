using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace musicbox
{
    static partial class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(UInt32 uiAction, uint uiParam, IntPtr pvParam, UInt32 fWinIni);

        public static void DisableScreenSaver()
        {
            SystemParametersInfo(0x0011, 0, IntPtr.Zero, 0);
        }
    }
}
