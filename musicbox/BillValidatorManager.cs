using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Runtime.CompilerServices;


namespace musicbox
{
    //Singleton
    internal class BillValidatorManager
    {
        public const byte VALIDATOR_UNITIALIZED = Byte.MaxValue;

        private IntPtr m_SerialPort;
        private readonly String S_COM9600 = "9600,n,8,1";	//!<Comunnication format string setting 9600 bps, no parity, 8 databits, 1 stop bit
        private CCCRSProtocol m_Protocol;
        private readonly Byte m_PeriferalAddr = 3;
        private readonly AutoResetEvent m_PollFinishEvent = new AutoResetEvent(false);
        public event EventHandler Incas;
        public event EventHandler<BillInEventArgs> BillIn;
        //public event EventHandler<ValidatorStateChangedEventArgs> ValidatorStateChanged;
        private byte m_PreviousState = VALIDATOR_UNITIALIZED;
        private readonly Thread m_PollThread;
        private readonly int m_PollIntervalMilliseconds = 20;
        private readonly byte m_SerialPortNumber = Properties.Settings.Default.SeralPortNumber;

        //private readonly int m_StopPollTimeoutSec = 3;
        private bool m_Started = false;
        private byte m_ValidatorCurrentState = VALIDATOR_UNITIALIZED;

        public byte CurrentState
        {
            get { return m_ValidatorCurrentState; }
        }
        public bool Started
        {
            get 
            {
                lock (this)
                {
                    return m_Started;
                }
            }
        }

        public class BillInEventArgs : EventArgs
        {
            private float m_Bill;

            public float Bill
            {
                get { return m_Bill; }
                set { m_Bill = value; }
            }
            public BillInEventArgs(float bill)
            {
                m_Bill = bill;
            }
        }
        public static BillValidatorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillValidatorManager();
                }
                return instance;
            }
        }

        private static BillValidatorManager instance;

        private BillValidatorManager()
        {
            m_PollThread = new Thread(new ThreadStart(PollFunc));
            SetThreadBackground(m_PollThread);
        }
        [Conditional("DEBUG")]
        private void SetThreadBackground(Thread thread)
        {
            thread.IsBackground = true;
        }

        private void PollFunc()
        {
            do
            {
                bool ret = m_Protocol.CmdPoll(m_PeriferalAddr);

                if (m_Protocol.PollResults.Z1 != m_PreviousState)
                {
                    /*if (ValidatorStateChanged != null)
                    {
                        ValidatorStateChanged(this, new ValidatorStateChangedEventArgs(m_Protocol.PollResults.Z1));
                    }*/
                    m_ValidatorCurrentState = m_Protocol.PollResults.Z1;
                }
                //Debug.WriteLine("Poll : " + ret.ToString() + " " +DateTime.Now.ToString() +
                //    " Z1 " + m_Protocol.PollResults.Z1.ToString() + " Z2 " + m_Protocol.PollResults.Z2.ToString() );

                if (m_Protocol.PollResults.Z1 == CCCRSProtocol.ST_DISABLED)
                {
                    //if (!m_BillEnable)
                    //{
                    //m_BillEnable = true;
                    m_Protocol.CmdBillType(0xFFFFFFFF, 0, m_PeriferalAddr);
                    //}
                }
                else if (m_Protocol.PollResults.Z1 == CCCRSProtocol.ST_PACKED)
                {
                    //Debug.WriteLine("Money " + m_Protocol.PollResults.Z2.ToString());
                    //Debug.WriteLine("MoneyEx " + m_Protocol.BillRecordList[m_Protocol.PollResults.Z2].Denomination);
                    if (BillIn != null)
                        BillIn(this, new BillInEventArgs(m_Protocol.BillRecordList[m_Protocol.PollResults.Z2].Denomination));

                }
                else if (m_Protocol.PollResults.Z1 == CCCRSProtocol.ST_BOX)
                {
                    if (m_PreviousState != CCCRSProtocol.ST_BOX)
                    {
                        if (Incas != null)
                            Incas(this, EventArgs.Empty);
                    }
                }
                m_PreviousState = m_Protocol.PollResults.Z1;
            } while (!m_PollFinishEvent.WaitOne(TimeSpan.FromMilliseconds(m_PollIntervalMilliseconds),false));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start()
        {
            Debug.Assert(m_Started == false);
            if (!m_Started)
            {
                using (SerialPort serialPort = new SerialPort("COM" + m_SerialPortNumber.ToString()))
                {
                    serialPort.Open();
                    serialPort.Close();
                }
                m_SerialPort = Win32Com.OpenCOM(m_SerialPortNumber);
                if (m_SerialPort == (IntPtr)Win32Com.INVALID_HANDLE_VALUE)
                    throw new Exception("BillValidator : Не могу открыть порт");
                if (!Win32Com.InitCOM(m_SerialPort, S_COM9600, 300))
                    throw new Exception("BillValidator : Не могу инициализировать порт");
                m_Protocol = new CCCRSProtocol(m_SerialPort);
                bool ret = m_Protocol.CmdPoll(m_PeriferalAddr);
                ret = m_Protocol.CmdReset(m_PeriferalAddr);
                ret = m_Protocol.CmdGetBillTable(m_PeriferalAddr);

                Debug.Assert(m_PollThread.IsAlive == false);
                m_PollThread.Start();
                m_Started = true;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            //Debug.Assert(m_Started == true);
            _StopThread();
        }
        private void _StopThread()
        {
            if (m_Started)
            {
                Debug.Assert(m_PollThread.IsAlive == true);
                m_PollFinishEvent.Set();
                m_PollThread.Join();
                m_Started = false;
            }
        }
        private void Cleanup()
        {
            _StopThread();
            Debug.Assert(m_SerialPort != null);
            Win32Com.CloseCOM(m_SerialPort);
        }
        ~BillValidatorManager()
        {
            Cleanup();
        }
    }
}
