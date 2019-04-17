using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace musicbox
{
    public class RasManager
    {
        [DllImport("rasapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int RasEnumConnections(
            [In, Out] RASCONN[] rasconn,
            [In, Out] ref int cb,
            [Out] out int connections);

        const int RAS_MaxEntryName = 256;
        const int RAS_MaxDeviceType = 16;
        const int RAS_MaxDeviceName = 128;
        const int MAX_PATH = 260;


        private const int SUCCESS = 0;
        private const int ERROR_NOT_ENOUGH_MEMORY = 8;
        private const int RASBASE = 600;
        private const int ERROR_BUFFER_TOO_SMALL = RASBASE + 3;
        private const int ERROR_INVALID_SIZE = RASBASE + 32;


        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct RASCONN
        {
            public int dwSize;
            public IntPtr hrasconn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName)]
            public string szDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPhonebook;
            public int dwSubEntry;
            public Guid guidEntry;
            public int dwFlags;
            public Guid luid;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RASDIALPARAMS
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 + 1)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 + 1)]
            public string szPhoneNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 + 1)]
            public string szCallbackNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 + 1)]
            public string szUserName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 + 1)]
            public string szPassword;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15 + 1)]
            public string szDomain;
            public uint dwSubEntry;
            public IntPtr dwCallbackId;
        }
        public delegate void Callback(uint unMsg, int rasconnstate, int dwError);
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasDial(int lpRasDialExtensions, string lpszPhonebook, ref RASDIALPARAMS lprasdialparams, int dwNotifierType,
        Callback lpvNotifier, ref IntPtr lphRasConn);
        [DllImport("rasapi32.dll", SetLastError = true)]
        static extern uint RasHangUp(IntPtr hRasConn);
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasGetErrorString(
        uint uErrorValue,    // error to get string for
        StringBuilder lpszErrorString,  // buffer to hold error string
        [In]int cBufSize       // size, in characters, of buffer
        );



        private static RASCONN[] GetAllConnections()
        {
            RASCONN[] tempConn = new RASCONN[1];
            RASCONN[] allConnections = tempConn;

            tempConn[0].dwSize = Marshal.SizeOf(typeof(RASCONN));
            int lpcb = tempConn[0].dwSize;
            int lpcConnections = 0;
            int ret = RasEnumConnections(tempConn, ref lpcb, out lpcConnections);
            if (ret == ERROR_INVALID_SIZE)
            {
                throw new Exception("RAS: RASCONN data structure has invalid format");
            }
            else if (ret == ERROR_BUFFER_TOO_SMALL && lpcb != 0)
            {
                // first call returned that there are more than one connections
                // and more memory is required
                allConnections = new RASCONN[lpcb / Marshal.SizeOf(typeof(RASCONN))];
                allConnections[0] = tempConn[0];
                ret = RasEnumConnections(allConnections, ref lpcb, out lpcConnections);
            }

            // Check errors
            if (ret != SUCCESS)
            {
                throw new Exception("RAS returns error: " + ret);
            }
            if (lpcConnections > allConnections.Length)
            {
                throw new Exception("RAS: error retrieving correct connection count");
            }
            else if (lpcConnections == 0)
            {
                allConnections = new RASCONN[0];
            }

            return allConnections;
        }

        public static bool CheckConnectionExist()
        {
            return GetAllConnections().Length > 0 ? true : false;
        }
        public static bool CheckInternetConnection()
        {
            try
            {
                System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false; // host not reachable. 
            }
        }
        public static uint UpLink(string phoneBookEntry)
        {
            if (phoneBookEntry == String.Empty)
                throw new ArgumentNullException("phoneBookEntry");
            RASCONN[] rasConnArr = GetAllConnections();
            RASCONN rConn = new RASCONN();
            rConn = 
                Array.Find<RASCONN>(rasConnArr,
                delegate(RASCONN rasConn)
                {
                    return rasConn.szEntryName == phoneBookEntry ? true : false;
                });
            if (rConn.hrasconn != IntPtr.Zero)
            {
                RasHangUp(rConn.hrasconn);
                Thread.Sleep(3000);
            }

            IntPtr hConnection = IntPtr.Zero ;
            RASDIALPARAMS rasDialParams = new RASDIALPARAMS();
            rasDialParams.dwSize = Marshal.SizeOf(rasDialParams);
            rasDialParams.szEntryName += phoneBookEntry;
            rasDialParams.szUserName += "\0";
            rasDialParams.szPassword += "\0";
            UInt32 result = RasDial(0, null, ref rasDialParams, 0, null, ref hConnection);
            return result;

        }

    }
}
