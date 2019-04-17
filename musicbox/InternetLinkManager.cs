using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace musicbox
{
    class InternetLinkManager : IDisposable
    {
        private readonly string phoneBookEntry = Properties.Settings.Default.ConnectionName;
        private readonly int checkConnectionIntervalSec = Properties.Settings.Default.CheckInternetConnectionIntervalSec;
        private bool m_Disposed = false;
        private bool m_Started = false;

        private Thread m_LinkThread;
        private AutoResetEvent m_LinkEvent = new AutoResetEvent(false);
        //private readonly int m_LinkThreadEndIntervalSec = 2;

        public InternetLinkManager()
        {
            m_LinkThread = new Thread(new ThreadStart(LinkProc));
        }
        public void Start()
        {
            lock (this)
            {
                if (m_Disposed)
                {
                    throw new ObjectDisposedException("InternetLinkManager");
                }
                if (m_Started)
                {
                    throw new InvalidOperationException("InternetLinkManager : Already started");
                }
                Debug.Assert(m_LinkThread.IsAlive == false);
                m_Started = true;
                m_LinkThread.Start();
            }
        }
        public void Stop()
        {
            lock (this)
            {
                if (m_Disposed)
                {
                    throw new ObjectDisposedException("InternetLinkManager");
                }
                if (!m_Started)
                {
                    throw new InvalidOperationException("InternetLinkManager : Already stopped");
                }
                Debug.Assert(m_LinkThread.IsAlive == true);
                m_Started = false;
                m_LinkEvent.Set();
                m_LinkThread.Join();
            }
        }
        public bool Disposed
        {
            get 
            {
                lock (this)
                {
                    return m_Disposed;
                }
            }
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
        void LinkProc()
        {
            do
            {
                if (!RasManager.CheckInternetConnection())
                {
                    uint result = RasManager.UpLink(phoneBookEntry);
                    if (result != 0)
                    {
                        StringBuilder sb = new StringBuilder(512);
                        RasManager.RasGetErrorString(result, sb, sb.Capacity);
                        Debug.WriteLine(sb.ToString());
                    }
                }
            } while (!m_LinkEvent.WaitOne(TimeSpan.FromSeconds(checkConnectionIntervalSec),false));
        }

        private void Cleanup()
        {
            if (m_Started)
            {
                m_LinkEvent.Set();
            }
        }
        ~InternetLinkManager()
        {
            lock (this)
            {
                Cleanup();
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            lock (this)
            {
                Cleanup();
                m_Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
