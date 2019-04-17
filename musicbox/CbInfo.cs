using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;

namespace musicbox
{
    [Serializable]
    public class CBInfo
    {
        [Serializable]
        public class CBItem
        {
            private Int32 m_Value = 0;
            [field:NonSerialized]
            public event EventHandler ValueChanged;

            public Int32 Value
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return m_Value;
                }
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Add(Int32 nAdd)
            {
                Interlocked.Add(ref m_Value, nAdd);
                OnValueChanged();
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Exchange(Int32 nVal)
            {
                Interlocked.Exchange(ref m_Value, nVal);
                OnValueChanged();
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Increment()
            {
                Interlocked.Increment(ref m_Value);
                OnValueChanged();
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            private void OnValueChanged()
            {
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
        }

        private CBItem m_SumNow = new CBItem();
        private CBItem m_BillsCountNow = new CBItem();
        private CBItem m_Sum = new CBItem();
        private CBItem m_BillsCount = new CBItem();
        private CBItem m_BillsCountCommon = new CBItem();
        private CBItem m_SumCommon = new CBItem();
        public static readonly String FilePath = "cbinfo.bin";

        private static CBInfo instance;
        private static object syncRoot = new Object();

        private CBInfo() 
        {
            
        }

        public static CBInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            if (!Program.DeserializeFromBinary<CBInfo>(FilePath, out instance))
                            {
                                instance = new CBInfo();
                            }

                        }
                    }
                }

                return instance;
            }
        }
        public CBItem SumNow
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_SumNow; }
        }
        
        public CBItem BillsCountNow
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_BillsCountNow; }
        }
        
        public CBItem Sum
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_Sum; }
        }
        
        public CBItem BillsCount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_BillsCount; }
        }
        
        public CBItem BillsCountCommon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_BillsCountCommon; }
        }
        
        public CBItem SumCommon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_SumCommon; }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Serialize()
        {
            Program.SerializeToBinary(FilePath, instance);
        }

    }
}
