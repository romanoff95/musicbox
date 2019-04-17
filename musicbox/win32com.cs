using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Threading;

namespace musicbox
{
    internal class Win32Com  {  
        /// <summary>  
        /// Opening Testing and Closing the Port Handle.  
        /// </summary>  
        /// 
        [DllImport("kernel32.dll")]
        static extern bool BuildCommDCB(string lpDef, [In,Out] ref DCB lpDCB);
        [DllImport("kernel32.dll")]
        public static extern bool PurgeComm(IntPtr hFile, uint dwFlags);
        internal const UInt32 PURGE_TXABORT =   0x0001;  // Kill the pending/current writes to the comm port.
        internal const UInt32 PURGE_RXABORT   =     0x0002;  // Kill the pending/current reads to the comm port.
        internal const UInt32 PURGE_TXCLEAR     =   0x0004;  // Kill the transmit queue if there.
        internal const UInt32 PURGE_RXCLEAR       = 0x0008;  // Kill the typeahead buffer if there.
        [DllImport("kernel32.dll", SetLastError=true)]  
        internal static extern IntPtr CreateFile(String lpFileName, UInt32 dwDesiredAccess, 
            UInt32 dwShareMode,   IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, 
            EFileAttributes fileAttrs,   IntPtr hTemplateFile);  
        //Constants for errors:  
        internal const UInt32 ERROR_FILE_NOT_FOUND = 2;  
        internal const UInt32 ERROR_INVALID_NAME = 123;  
        internal const UInt32 ERROR_ACCESS_DENIED = 5;  
        internal const UInt32 ERROR_IO_PENDING = 997;  
        //Constants for return value:  
        internal const Int32 INVALID_HANDLE_VALUE = -1;  
        //Constants for dwFlagsAndAttributes:  
        internal const UInt32 FILE_FLAG_OVERLAPPED = 0x40000000;  
        //Constants for dwCreationDisposition:  
        internal const UInt32 OPEN_EXISTING = 3;  
        //Constants for dwDesiredAccess:  
        internal const UInt32 GENERIC_READ = 0x80000000;  
        internal const UInt32 GENERIC_WRITE = 0x40000000;
        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        [DllImport("kernel32.dll")]  
        internal static extern Boolean CloseHandle(IntPtr hObject);  
        /// <summary>  
        /// Manipulating the communications settings.  ///
        /// </summary>  
        /// 
        [DllImport("kernel32.dll")]  
        internal static extern Boolean GetCommState(IntPtr hFile, ref DCB lpDCB);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean GetCommTimeouts(IntPtr hFile, out COMMTIMEOUTS lpCommTimeouts);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean BuildCommDCBAndTimeouts(String lpDef, ref DCB lpDCB, ref COMMTIMEOUTS lpCommTimeouts);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean SetCommState(IntPtr hFile, [In] ref DCB lpDCB);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean SetCommTimeouts(IntPtr hFile, [In] ref COMMTIMEOUTS lpCommTimeouts);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean SetupComm(IntPtr hFile, UInt32 dwInQueue, UInt32 dwOutQueue);  
        [StructLayout( LayoutKind.Sequential )] 
        internal struct COMMTIMEOUTS   {   
            //JH1.1: Changed Int32 to UInt32 to allow setting to MAXDWORD   
            internal UInt32 ReadIntervalTimeout;   
            internal UInt32 ReadTotalTimeoutMultiplier;   
            internal UInt32 ReadTotalTimeoutConstant;   
            internal UInt32 WriteTotalTimeoutMultiplier;   
            internal UInt32 WriteTotalTimeoutConstant;  
        }  //JH1.1: Added to enable use of "return immediately" timeout.  
        internal const UInt32 MAXDWORD = 0xffffffff;
        public enum DtrControl : int
        {
            Disable = 0,
            Enable = 1,
            Handshake = 2
        };
        public enum RtsControl : int
        {
            Disable = 0,
            Enable = 1,
            Handshake = 2,
            Toggle = 3
        };
        internal struct DCB
        {

            internal uint DCBLength;
            internal uint BaudRate;
            private BitVector32 Flags;

            //I've missed some members...
            private uint wReserved;        // not currently used 
            internal uint XonLim;           // transmit XON threshold 
            internal uint XoffLim;          // transmit XOFF threshold             

            internal byte ByteSize;
            internal byte Parity;
            internal byte StopBits;

            //...and some more
            internal char XonChar;          // Tx and Rx XON character 
            internal char XoffChar;         // Tx and Rx XOFF character 
            internal char ErrorChar;        // error replacement character 
            internal char EofChar;          // end of input character 
            internal char EvtChar;          // received event character 
            private uint wReserved1;       // reserved; do not use     

            private static readonly int fBinary;
            private static readonly int fParity;
            private static readonly int fOutxCtsFlow;
            private static readonly int fOutxDsrFlow;
            private static readonly BitVector32.Section fDtrControl;
            private static readonly int fDsrSensitivity;
            private static readonly int fTXContinueOnXoff;
            private static readonly int fOutX;
            private static readonly int fInX;
            private static readonly int fErrorChar;
            private static readonly int fNull;
            private static readonly BitVector32.Section fRtsControl;
            private static readonly int fAbortOnError;

            static DCB()
            {
                // Create Boolean Mask
                int previousMask;
                fBinary = BitVector32.CreateMask();
                fParity = BitVector32.CreateMask(fBinary);
                fOutxCtsFlow = BitVector32.CreateMask(fParity);
                fOutxDsrFlow = BitVector32.CreateMask(fOutxCtsFlow);
                previousMask = BitVector32.CreateMask(fOutxDsrFlow);
                previousMask = BitVector32.CreateMask(previousMask);
                fDsrSensitivity = BitVector32.CreateMask(previousMask);
                fTXContinueOnXoff = BitVector32.CreateMask(fDsrSensitivity);
                fOutX = BitVector32.CreateMask(fTXContinueOnXoff);
                fInX = BitVector32.CreateMask(fOutX);
                fErrorChar = BitVector32.CreateMask(fInX);
                fNull = BitVector32.CreateMask(fErrorChar);
                previousMask = BitVector32.CreateMask(fNull);
                previousMask = BitVector32.CreateMask(previousMask);
                fAbortOnError = BitVector32.CreateMask(previousMask);

                // Create section Mask
                BitVector32.Section previousSection;
                previousSection = BitVector32.CreateSection(1);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                fDtrControl = BitVector32.CreateSection(2, previousSection);
                previousSection = BitVector32.CreateSection(1, fDtrControl);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                fRtsControl = BitVector32.CreateSection(3, previousSection);
                previousSection = BitVector32.CreateSection(1, fRtsControl);
            }

            public bool Binary
            {
                get { return Flags[fBinary]; }
                set { Flags[fBinary] = value; }
            }

            public bool CheckParity
            {
                get { return Flags[fParity]; }
                set { Flags[fParity] = value; }
            }

            public bool OutxCtsFlow
            {
                get { return Flags[fOutxCtsFlow]; }
                set { Flags[fOutxCtsFlow] = value; }
            }

            public bool OutxDsrFlow
            {
                get { return Flags[fOutxDsrFlow]; }
                set { Flags[fOutxDsrFlow] = value; }
            }

            public DtrControl DtrControl
            {
                get { return (DtrControl)Flags[fDtrControl]; }
                set { Flags[fDtrControl] = (int)value; }
            }

            public bool DsrSensitivity
            {
                get { return Flags[fDsrSensitivity]; }
                set { Flags[fDsrSensitivity] = value; }
            }

            public bool TxContinueOnXoff
            {
                get { return Flags[fTXContinueOnXoff]; }
                set { Flags[fTXContinueOnXoff] = value; }
            }

            public bool OutX
            {
                get { return Flags[fOutX]; }
                set { Flags[fOutX] = value; }
            }

            public bool InX
            {
                get { return Flags[fInX]; }
                set { Flags[fInX] = value; }
            }

            public bool ReplaceErrorChar
            {
                get { return Flags[fErrorChar]; }
                set { Flags[fErrorChar] = value; }
            }

            public bool Null
            {
                get { return Flags[fNull]; }
                set { Flags[fNull] = value; }
            }

            public RtsControl RtsControl
            {
                get { return (RtsControl)Flags[fRtsControl]; }
                set { Flags[fRtsControl] = (int)value; }
            }

            public bool AbortOnError
            {
                get { return Flags[fAbortOnError]; }
                set { Flags[fAbortOnError] = value; }
            }

        }
        /// <summary>  
        /// Reading and writing.  
        /// </summary>
        /// 
        [DllImport("kernel32.dll", SetLastError=true)]  
        internal static extern Boolean WriteFile(IntPtr fFile, Byte[] lpBuffer,
            UInt32 nNumberOfBytesToWrite,   out UInt32 lpNumberOfBytesWritten,
            IntPtr lpOverlapped);  
        [StructLayout( LayoutKind.Sequential )] 
        internal struct OVERLAPPED   
        {   
            internal UIntPtr Internal;   
            internal UIntPtr InternalHigh;   
            internal UInt32 Offset;   
            internal UInt32 OffsetHigh;   
            internal IntPtr hEvent;  
        }  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean SetCommMask(IntPtr hFile, UInt32 dwEvtMask);  
        // Constants for dwEvtMask:  
        internal const UInt32 EV_RXCHAR = 0x0001;  
        internal const UInt32 EV_RXFLAG = 0x0002;  
        internal const UInt32 EV_TXEMPTY = 0x0004;  
        internal const UInt32 EV_CTS = 0x0008;  
        internal const UInt32 EV_DSR = 0x0010;  
        internal const UInt32 EV_RLSD = 0x0020;  
        internal const UInt32 EV_BREAK = 0x0040;  
        internal const UInt32 EV_ERR = 0x0080;  
        internal const UInt32 EV_RING = 0x0100;  
        internal const UInt32 EV_PERR = 0x0200;  
        internal const UInt32 EV_RX80FULL = 0x0400;  
        internal const UInt32 EV_EVENT1 = 0x0800;  
        internal const UInt32 EV_EVENT2 = 0x1000;  
        [DllImport("kernel32.dll", SetLastError=true)]  
        internal static extern Boolean WaitCommEvent(IntPtr hFile, IntPtr lpEvtMask, IntPtr lpOverlapped);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean CancelIo(IntPtr hFile);    
        [DllImport("kernel32.dll", SetLastError=true)]  
        internal static extern Boolean ReadFile(IntPtr hFile, [Out] Byte[] lpBuffer, 
            UInt32 nNumberOfBytesToRead,   out UInt32 nNumberOfBytesRead, 
            IntPtr lpOverlapped);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean TransmitCommChar(IntPtr hFile, Byte cChar);  
        /// <summary>  
        /// Control port functions.  /// 
        /// </summary>  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean EscapeCommFunction(IntPtr hFile, UInt32 dwFunc);  
        // Constants for dwFunc:  
        internal const UInt32 SETXOFF = 1;  
        internal const UInt32 SETXON = 2;  
        internal const UInt32 SETRTS = 3;  
        internal const UInt32 CLRRTS = 4;  
        internal const UInt32 SETDTR = 5;  
        internal const UInt32 CLRDTR = 6;  
        internal const UInt32 RESETDEV = 7;  
        internal const UInt32 SETBREAK = 8;  
        internal const UInt32 CLRBREAK = 9;    
        [DllImport("kernel32.dll")]
        internal static extern Boolean GetCommModemStatus(IntPtr hFile, out UInt32 lpModemStat);  
        // Constants for lpModemStat:  
        internal const UInt32 MS_CTS_ON = 0x0010;  
        internal const UInt32 MS_DSR_ON = 0x0020;  
        internal const UInt32 MS_RING_ON = 0x0040;  
        internal const UInt32 MS_RLSD_ON = 0x0080;  
        /// <summary>  
        /// Status Functions.  
        /// </summary>  
        /// 
        [DllImport("kernel32.dll", SetLastError=true)]  
        internal static extern Boolean GetOverlappedResult(IntPtr hFile, IntPtr lpOverlapped,   
            out UInt32 nNumberOfBytesTransferred, Boolean bWait);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean ClearCommError(IntPtr hFile, out UInt32 lpErrors, 
            IntPtr lpStat);  
        [DllImport("kernel32.dll")]  
        internal static extern Boolean ClearCommError(IntPtr hFile, 
            out UInt32 lpErrors, out COMSTAT cs);  
        //Constants for lpErrors:  
        internal const UInt32 CE_RXOVER = 0x0001;  
        internal const UInt32 CE_OVERRUN = 0x0002;  
        internal const UInt32 CE_RXPARITY = 0x0004;  
        internal const UInt32 CE_FRAME = 0x0008;  
        internal const UInt32 CE_BREAK = 0x0010;  
        internal const UInt32 CE_TXFULL = 0x0100;  
        internal const UInt32 CE_PTO = 0x0200;  
        internal const UInt32 CE_IOE = 0x0400;  
        internal const UInt32 CE_DNS = 0x0800;  
        internal const UInt32 CE_OOP = 0x1000;  
        internal const UInt32 CE_MODE = 0x8000;  
        [StructLayout( LayoutKind.Sequential )] 
        internal struct COMSTAT   
        {   
            internal const uint fCtsHold = 0x1;   
            internal const uint fDsrHold = 0x2;   
            internal const uint fRlsdHold = 0x4;   
            internal const uint fXoffHold = 0x8;   
            internal const uint fXoffSent = 0x10;   
            internal const uint fEof = 0x20;   
            internal const uint fTxim = 0x40;   
            internal UInt32 Flags;   
            internal UInt32 cbInQue;   
            internal UInt32 cbOutQue;  
        }  
        [DllImport("kernel32.dll")]
        internal static extern Boolean GetCommProperties(IntPtr hFile, out COMMPROP cp);
        [StructLayout( LayoutKind.Sequential )] 
        internal struct COMMPROP  
        {   
            internal UInt16 wPacketLength;    
            internal UInt16 wPacketVersion;    
            internal UInt32 dwServiceMask;    
            internal UInt32 dwReserved1;    
            internal UInt32 dwMaxTxQueue;    
            internal UInt32 dwMaxRxQueue;    
            internal UInt32 dwMaxBaud;    
            internal UInt32 dwProvSubType;    
            internal UInt32 dwProvCapabilities;    
            internal UInt32 dwSettableParams;    
            internal UInt32 dwSettableBaud;    
            internal UInt16 wSettableData;    
            internal UInt16 wSettableStopParity;    
            internal UInt32 dwCurrentTxQueue;    
            internal UInt32 dwCurrentRxQueue;    
            internal UInt32 dwProvSpec1;    
            internal UInt32 dwProvSpec2;    
            internal Byte wcProvChar;   
        }

        //private int iTimeOut = 1500;


        public static IntPtr OpenCOM(byte portNumber)
        {
            Int32 dwError;
            String strCOM = "COM" + portNumber.ToString();
            IntPtr hCOMPort = CreateFile(strCOM, GENERIC_READ | GENERIC_WRITE,
                        0, IntPtr.Zero, OPEN_EXISTING,
                        0,
                        IntPtr.Zero);
            if (hCOMPort == (IntPtr)INVALID_HANDLE_VALUE)
                dwError = Marshal.GetLastWin32Error();
            else dwError = 0;
            return hCOMPort;
        }
        public static void CloseCOM(IntPtr hCOMPort)
        {
            if (hCOMPort != (IntPtr)INVALID_HANDLE_VALUE)
                PurgeComm(hCOMPort, 0xffffffff);
            CloseHandle(hCOMPort);
            //hCOMPort = (IntPtr)INVALID_HANDLE_VALUE;
        }

        public static bool InitCOM(IntPtr hCOMPort, String Str, int iTimeOut)
        {
            DCB dcb = new DCB(), dcb1 = new DCB();
            COMMTIMEOUTS CommTimeOuts = new COMMTIMEOUTS();
            //if ( < 0) return FALSE;
            //PurgeComm(hCOMPort, -1);
            //CloseCOM();
            SetupComm(hCOMPort, 65535, 0xffff);
            GetCommState(hCOMPort, ref dcb);
            if (!BuildCommDCB(Str, ref dcb1)) return false;
            // Filling in the DCB
            dcb.BaudRate = dcb1.BaudRate;
            dcb.ByteSize = dcb1.ByteSize;
            dcb.Parity = dcb1.Parity;
            dcb.StopBits = dcb1.StopBits;
            dcb.Binary = true;          // binary mode, no EOF check
            dcb.Parity = 0;          // enable parity checking
            dcb.AbortOnError = false; // abort reads/writes on error
            dcb.DtrControl = DtrControl.Disable;
            dcb.RtsControl = RtsControl.Disable;
            dcb.OutxCtsFlow = false;
            dcb.OutxDsrFlow = false ;
            dcb.DsrSensitivity = false;
            dcb.OutX = false;
            //---------------
            if (!SetCommState(hCOMPort, ref dcb)) return false;
            CommTimeOuts.ReadTotalTimeoutConstant = 1500;
            CommTimeOuts.ReadTotalTimeoutMultiplier = 11;
            CommTimeOuts.WriteTotalTimeoutConstant = 200;
            CommTimeOuts.WriteTotalTimeoutMultiplier = 11;
            return SetCommTimeouts(hCOMPort, ref CommTimeOuts);

            //------------------------
        }
        public static bool Send(IntPtr hCOMPort,Byte[] Data, int Number)
        {
            bool bError;
            UInt32 wBytes;
            bError = WriteFile(hCOMPort, Data, (UInt32)Number, out wBytes, IntPtr.Zero);

            bError = bError && ((UInt32)Number == wBytes);
            return bError;
        }

        public static bool Recieve(IntPtr hCOMPort,out Byte[] Buffer, int Length)
        {
            bool res;
            UInt32 dwBytes;
            byte[] buff = new byte[Length];
            res = ReadFile(hCOMPort, buff, (UInt32)Length, out dwBytes, IntPtr.Zero);
            res = res & (dwBytes == (UInt32)Length);
            Buffer = buff;
            return res;
        }
        public static void DTR(IntPtr hCOMPort ,bool bDTR)
        {
            EscapeCommFunction(hCOMPort, (bDTR) ? SETDTR : CLRDTR);
            Thread.Sleep(1);
        }
        public static void RTS(IntPtr hCOMPort,bool bRTS)
        {
            EscapeCommFunction(hCOMPort, (bRTS) ? SETRTS : CLRRTS);
            Thread.Sleep(1);
        }

    }
}
