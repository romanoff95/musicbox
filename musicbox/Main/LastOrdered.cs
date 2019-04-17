using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace musicbox.Main
{
    sealed class LastOrdered
    {
        private static volatile LastOrdered instance;
        private static object syncRoot = new Object();
        private Queue<String> m_Items = new Queue<string>();
        private const Int32 m_Limit = 10;

        
        public Int32 Limit
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return m_Limit; }
        } 


        private LastOrdered() { }

        public static LastOrdered Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LastOrdered();
                    }
                }

                return instance;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddItems(String[] items)
        {
            Array.ForEach<String>(items, delegate(String value)
            {
                m_Items.Enqueue(value);
                if (m_Items.Count > m_Limit)
                    m_Items.Dequeue();
            });
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public String[] GetItems()
        {
            return m_Items.ToArray();
        }
    }
}
