using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace musicbox
{
    abstract class SendBase : IDisposable
    {
        protected readonly int m_DevId = Properties.Settings.Default.DeviceNum;
        protected readonly string m_ServerAddress = Properties.Settings.Default.ServerAddr;
        protected readonly string m_DeviceKey = Properties.Settings.Default.DeviceKey;
        private bool m_Disposed = false;

        protected bool Disposed
        {
            get 
            {
                lock (this)
                {
                    return m_Disposed;
                }
            }
        }

        protected virtual void Cleanup()
        {
        }
        ~SendBase()
        {
            Debug.WriteLine("Base Cleanup");
            Cleanup();
        }
        #region IDisposable Members
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            if (m_Disposed == false)
            {
                Cleanup();
                m_Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
    abstract class StoredDataSenderBase : SendBase
    {
        protected OleDbConnection m_Connection;
        private readonly string m_ConnectString = Properties.Settings.Default.ConnectionString;

        public StoredDataSenderBase()
        {
            m_Connection = new OleDbConnection(m_ConnectString);
            m_Connection.Open();
        }
        protected override void Cleanup()
        {
            try
            {
                m_Connection.Close();
                m_Connection.Dispose();
            }
            catch (InvalidOperationException)
            {
                Debug.Assert(false, "Connection already unloaded");
            }
            finally
            {
                base.Cleanup();
            }
        }

        /*protected virtual void Add();
        protected virtual void Send();*/
    }
    class SongInfoSendManager : StoredDataSenderBase
    {
        private readonly string tableName = "SongInfo";
        private readonly string idColName = "Id";
        private readonly string songNameColName = "SongName";
        private readonly string songPriceColName = "SongPrice";
        private readonly string orderDateColName = "OrderDate";
        private readonly string songTypeColName = "SongType";

        private readonly String paramFormat = "devid={0}&song_name={1}&" +
            "song_price={2}&d={3}&m={4}&y={5}&h={6}&min={7}&s={8}&devkey={9}&order_id={10}";
        private readonly String m_Page = Properties.Settings.Default.OrderedSongPage;


        private const int songNameColCharLength = 100;

        public struct SongInfoSendData
        {
            private String m_SongName;
            private Int32 m_SongPrice;
            private DateTime m_OrderDate;
            private MediaTypes m_SongType;

            public SongInfoSendData(String SongName, Int32 SongPrice, DateTime OrderDate, MediaTypes songType)
            {
                this.m_SongName = SongName;
                this.m_SongPrice = SongPrice;
                this.m_OrderDate = OrderDate;
                this.m_SongType = songType;
            }

            public String SongName
            {
                get { return m_SongName; }
                set { m_SongName = value; }
            }
            public Int32 SongPrice
            {
                get { return m_SongPrice; }
                set { m_SongPrice = value; }
            }
            public DateTime OrderDate
            {
                get { return m_OrderDate; }
                set { m_OrderDate = value; }
            }
            public MediaTypes SongType
            {
                get { return m_SongType; }
                set { m_SongType = value; }
            }
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void Send()
        {
            OleDbCommand command = null;
            string selectId = null;
            command = m_Connection.CreateCommand();
            command.CommandText = String.Format(
                "SELECT TOP 1 * FROM {0} ORDER BY {1}",
                tableName, idColName);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                selectId = reader[idColName].ToString();
                DateTime orderDate = DateTime.Parse(reader[orderDateColName].ToString());
                if (Program.SendDataToInet(String.Concat(
                    m_ServerAddress, m_Page),
                    String.Format(paramFormat, m_DevId, Path.GetFileName(reader[songNameColName].ToString()), reader[songPriceColName],
                    orderDate.Day, orderDate.Month, orderDate.Year, orderDate.Hour, orderDate.Minute, orderDate.Second,
                    m_DeviceKey, reader[idColName])))
                {
                    reader.Close();
                    command.CommandText = String.Format(
                        "DELETE FROM {0} WHERE {1} = {2}", tableName, idColName, selectId);
                    command.ExecuteNonQuery();
                }

            }
            else
                reader.Close();

        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(SongInfoSendData[] songInfoArr)
        {
            if (songInfoArr.Length > 0)
            {
                OleDbCommand command = null;
                command = m_Connection.CreateCommand();
                command.CommandText = String.Format(
                    "INSERT INTO {0}({1},{2},{3},{4}) VALUES (@{1},@{2},@{3},@{4})",
                    tableName, songNameColName, songPriceColName, orderDateColName,
                    songTypeColName);

                command.Parameters.Add("@" + songNameColName, OleDbType.VarChar, songNameColCharLength);
                command.Parameters.Add("@" + songPriceColName, OleDbType.Currency);
                command.Parameters.Add("@" + orderDateColName, OleDbType.Date);
                command.Parameters.Add("@" + songTypeColName, OleDbType.SmallInt);
                foreach (SongInfoSendData songInfo in songInfoArr)
                {
                    command.Parameters["@" + songNameColName].Value = songInfo.SongName;
                    command.Parameters["@" + songPriceColName].Value = songInfo.SongPrice;
                    command.Parameters["@" + orderDateColName].Value = songInfo.OrderDate;
                    command.Parameters["@" + songTypeColName].Value = songInfo.SongType;
                    command.ExecuteNonQuery();
                }
                
            }
        }
    }
    abstract class InfoSender<T> : SendBase
    {
        private Thread m_SendThread;
        private readonly AutoResetEvent m_Event = new AutoResetEvent(false);
        private readonly int m_SendIntervalSec = 1;
        protected readonly Queue<T> m_DataQueue;
        private readonly string m_SerializedFileName;

        /*protected static InfoSender<T> _Create(string filename)
        {
            InfoSender<T> infoSender;

            if (!Program.TryDeserializeObjectFromBinary(m_SerializedFileName, out tInfoSender))
            {
                infoSender = new InfoSender<T>();
            }
            return infoSender;
        }*/

        public virtual void AddRange(T[] dataArr)
        {
            lock (m_DataQueue)
            {
                foreach (T data in dataArr)
                {
                    m_DataQueue.Enqueue(data);
                }
            }
        }
        protected override void Cleanup()
        {
            try
            {
                m_Event.Set();
                m_SendThread.Join();
                _Serialize();
            }
            finally
            {
                base.Cleanup();
            }
        }
        protected InfoSender(string serializedFileName)
        {
            m_SerializedFileName = serializedFileName;
            if (!Program.DeserializeFromBinary<Queue<T>>(serializedFileName, out m_DataQueue))
            {
                m_DataQueue = new Queue<T>();
            }
            m_SendThread = new Thread(new ThreadStart(SendLoop));
            m_SendThread.Start();
        }
        private void SendLoop()
        {
            do
            {
                SendFunc();
            } while (!m_Event.WaitOne(TimeSpan.FromSeconds(m_SendIntervalSec),false));
        }
        abstract protected void SendFunc();
        protected virtual void _Serialize()
        {
            Program.SerializeToBinary(m_SerializedFileName, m_DataQueue);
        }
    }
    [Serializable]
    class SongInfoSender : InfoSender<musicbox.SongInfoSender.SongInfoData>
    {
        [Serializable]
        internal struct SongInfoData
        {
            private String m_SongName;
            private Int32 m_SongPrice;
            private DateTime m_OrderDate;
            private MediaTypes m_SongType;

            public SongInfoData(String SongName, Int32 SongPrice, DateTime OrderDate, MediaTypes songType)
            {
                this.m_SongName = SongName;
                this.m_SongPrice = SongPrice;
                this.m_OrderDate = OrderDate;
                this.m_SongType = songType;
            }

            public String SongName
            {
                get { return m_SongName; }
                set { m_SongName = value; }
            }
            public Int32 SongPrice
            {
                get { return m_SongPrice; }
                set { m_SongPrice = value; }
            }
            public DateTime OrderDate
            {
                get { return m_OrderDate; }
                set { m_OrderDate = value; }
            }
            public MediaTypes SongType
            {
                get { return m_SongType; }
                set { m_SongType = value; }
            }
        }
        private static readonly string m_SerializedFileName = "songSender.bin";
        private readonly string m_SendPattern = "devid={0}&song_name={1}&" +
            "song_price={2}&d={3}&m={4}&y={5}&h={6}&min={7}&s={8}&devkey={9}&order_id={10}";
        private readonly String m_Page = Properties.Settings.Default.OrderedSongPage;

        private SongInfoSender(string serializedFileName)
            : base(serializedFileName)
        {
        }
        public static SongInfoSender Create()
        {
            return new SongInfoSender(m_SerializedFileName);
        }
        public virtual void AddIMedia(musicbox.FileNameAndMediaWithIsMoneyless[] _f)
        {
            lock (m_DataQueue)
            {
                foreach (musicbox.FileNameAndMediaWithIsMoneyless _f1 in _f)
                {
                    m_DataQueue.Enqueue(new SongInfoData(_f1.FileNameAndMedia.FileName,
                        _f1.Moneyless ? 0 : _f1.FileNameAndMedia.Media.Price, DateTime.Now,
                        _f1.FileNameAndMedia.Media.MediaType));
                }
            }
        }
        protected override void SendFunc()
        {
            bool flag = false;
            SongInfoData songInfoData = new SongInfoData();
            lock (m_DataQueue)
            {
                if (m_DataQueue.Count > 0)
                {
                    songInfoData = m_DataQueue.Peek();
                    flag = true;
                }
            }
            if (flag == true)
            {
                string sendParams = String.Format(m_SendPattern, m_DevId, songInfoData.SongName, songInfoData.SongPrice,
                    songInfoData.OrderDate.Day, songInfoData.OrderDate.Month, songInfoData.OrderDate.Year,
                    songInfoData.OrderDate.Hour, songInfoData.OrderDate.Minute, songInfoData.OrderDate.Second,
                    m_DeviceKey,0);
                if (Program.SendDataToInet(m_ServerAddress, sendParams))
                {
                    lock (m_DataQueue)
                    {
                        m_DataQueue.Dequeue();
                    }
                }   
            }
        }
    }
    class IncasInfoSendManager : StoredDataSenderBase
    {
        private readonly string m_TableName = "IncasInfo";
        private readonly string m_IdColName = "Id";
        private readonly string m_CommonSumColName = "mCommonSum";
        private readonly string m_DeltaSumColName = "mDeltaSum";
        private readonly string m_BillsInStackerColName = "nBillsInStacker";
        private readonly string m_IncasDateColName = "dIncasDate";

        private readonly String paramFormat = "devid={0}&devkey={1}&delta_sum={2}&common_sum={3}" +
                    "&bill_status={4}&d={5}&m={6}&y={7}&h={8}&min={9}&s={10}&incas_id={11}";
        private readonly String m_Page = Properties.Settings.Default.IncasPage;

        public struct IncasInfo
        {
            private String m_CommonSum;
            private String m_DeltaSum;
            private String m_BillsInStacker;
            private DateTime m_IncasDate;

            public IncasInfo(String strCommonSum, String strDeltaSum, String strBillsInStacker, DateTime dtIncas)
            {
                this.m_CommonSum = strCommonSum;
                this.m_DeltaSum = strDeltaSum;
                this.m_BillsInStacker = strBillsInStacker;
                this.m_IncasDate = dtIncas;
            }
            public String CommonSum
            {
                get { return m_CommonSum; }
                set { m_CommonSum = value; }
            }
            public String DeltaSum
            {
                get { return m_DeltaSum; }
                set { m_DeltaSum = value; }
            }
            public String BillsInStacker
            {
                get { return m_BillsInStacker; }
                set { m_BillsInStacker = value; }
            }
            public DateTime IncasDate
            {
                get { return m_IncasDate; }
                set { m_IncasDate = value; }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(IncasInfo[] incasInfoArr)
        {
            if (incasInfoArr.Length > 0)
            {
                OleDbCommand command = null;
                command = m_Connection.CreateCommand();
                command.CommandText = String.Format(
                    "INSERT INTO {0}({1},{2},{3},{4}) VALUES (@{1},@{2},@{3},@{4})",
                    m_TableName, m_CommonSumColName, m_DeltaSumColName, m_BillsInStackerColName,
                    m_IncasDateColName);
                command.Parameters.Add("@" + m_CommonSumColName, OleDbType.Currency);
                command.Parameters.Add("@" + m_DeltaSumColName, OleDbType.Currency);
                command.Parameters.Add("@" + m_BillsInStackerColName, OleDbType.Integer);
                command.Parameters.Add("@" + m_IncasDateColName,OleDbType.Date);
                foreach (IncasInfo incasInfo in incasInfoArr)
                {
                    command.Parameters["@" + m_CommonSumColName].Value = incasInfo.CommonSum;
                    command.Parameters["@" + m_DeltaSumColName].Value = incasInfo.DeltaSum;
                    command.Parameters["@" + m_BillsInStackerColName].Value = incasInfo.BillsInStacker;
                    command.Parameters["@" + m_IncasDateColName].Value = incasInfo.IncasDate;
                    command.ExecuteNonQuery();
                }

            }
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void Send()
        {
            OleDbCommand command = null;
            string selectId = null;
            command = m_Connection.CreateCommand();
            command.CommandText = String.Format(
                "SELECT TOP 1 * FROM {0} ORDER BY {1}",
                m_TableName, m_IdColName);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                selectId = reader[m_IdColName].ToString();
                DateTime incasDate = DateTime.Parse(reader[m_IncasDateColName].ToString());
                if (Program.SendDataToInet(String.Concat(
                    m_ServerAddress, m_Page),
                    String.Format(paramFormat,m_DevId,m_DeviceKey,reader[m_DeltaSumColName],
                    reader[m_CommonSumColName],reader[m_BillsInStackerColName],
                    incasDate.Day,incasDate.Month,incasDate.Year,incasDate.Hour,incasDate.Minute,incasDate.Second,
                    reader[m_IdColName])))
                {
                    reader.Close();
                    command.CommandText = String.Format(
                        "DELETE FROM {0} WHERE {1} = {2}", m_TableName, m_IdColName, selectId);
                    command.ExecuteNonQuery();
                }

            }
            else
                reader.Close();

        }
    }

    class SongInfoManager : StoredDataSenderBase
    {
        private readonly string m_TableName = "Ordered_Songs";
        private readonly string m_SongNameColName = "vcSongName";
        private readonly string m_SongOrderColName = "dtSongOrder";

        private const int m_SongNameColNameLength = 100;

        public struct SongInfo
        {
            private string m_SongName;

            public string SongName
            {
                get { return m_SongName; }
                set { m_SongName = value; }
            }
            private DateTime m_OrderDate;

            public DateTime OrderDate
            {
                get { return m_OrderDate; }
                set { m_OrderDate = value; }
            }

            public SongInfo(string songName,DateTime orderDate)
            {
                m_SongName = songName;
                m_OrderDate = orderDate;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(SongInfo[] songInfoArr)
        {
            OleDbCommand command = null;
            command = m_Connection.CreateCommand();
            command.CommandText = String.Format(
                "INSERT INTO {0}({1},{2}) VALUES(@{1},@{2})",
                m_TableName, m_SongNameColName, m_SongOrderColName);
            command.Parameters.Add("@" + m_SongNameColName, OleDbType.WChar, m_SongNameColNameLength);
            command.Parameters.Add("@" + m_SongOrderColName, OleDbType.Date);

            foreach (SongInfo songInfo in songInfoArr)
            {
                command.Parameters["@" + m_SongNameColName].Value = songInfo.SongName;
                command.Parameters["@" + m_SongOrderColName].Value = songInfo.OrderDate;
                command.ExecuteNonQuery();
            }

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] GetTopTen()
        {
            OleDbCommand cmd;
            OleDbDataReader rdr;
            List<String> retList = new List<String>();

            cmd = base.m_Connection.CreateCommand();
            cmd.CommandText =
                String.Format("SELECT TOP 10 {0} FROM {1} GROUP BY {0} ORDER BY COUNT(*) DESC",
                m_SongNameColName, m_TableName);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                retList.Add(Path.GetFileNameWithoutExtension(rdr[0].ToString()));
            }
            //Return more than 10 result strings
            return retList.ToArray();
        }
    }

    class AppInfoSendManager : SendBase
    {
        public struct AppInfo
        {
            private String m_DeltaSum;
            private String m_CommonSum;
            private String m_BillsInStacker;
            private String m_BillStatus;

            public String DeltaSum
            {
                get { return m_DeltaSum; }
                set { m_DeltaSum = value; }
            }
            public String CommonSum
            {
                get { return m_CommonSum; }
                set { m_CommonSum = value; }
            }
            public String BillsInStacker
            {
                get { return m_BillsInStacker; }
                set { m_BillsInStacker = value; }
            }
            public String BillStatus
            {
                get { return m_BillStatus; }
                set { m_BillStatus = value; }
            }
            
            public AppInfo(String deltaSum, String commonSum, String billsInStacker, String billStatus)
            {
                this.m_DeltaSum = deltaSum;
                this.m_CommonSum = commonSum;
                this.m_BillsInStacker = billsInStacker;
                this.m_BillStatus = billStatus;
            }
        }

        private readonly String m_SendFormat = "devid={0}&devkey={1}&delta_sum={2}&common_sum={3}&bill_count={4}&bill_status={5}";
        private readonly String m_Page = Properties.Settings.Default.LinkPage;

        public void Send(AppInfo appInfo)
        {
            Program.SendDataToInet(
                String.Concat(base.m_ServerAddress, m_Page),
                String.Format(m_SendFormat, m_DevId, m_DeviceKey,
                appInfo.DeltaSum, appInfo.CommonSum, appInfo.BillsInStacker, appInfo.BillStatus));
        }
    }
    class SendInfoManager : IDisposable
    {
        private Thread m_SendThread;
        private AutoResetEvent m_SendEvent = new AutoResetEvent(false);
        private bool m_Disposed = false;
        private readonly bool m_SendDataToInet = Properties.Settings.Default.SendDataToInternet;
        private readonly int m_SendDataIntervalSec = Properties.Settings.Default.SendDataIntervalSec;

        private SongInfoSendManager m_SongInfoSendManager;
        private IncasInfoSendManager m_IncasInfoSendManager;

        internal SongInfoSendManager SongInfoSendManager
        {
            get { return m_SongInfoSendManager; }
        }
        internal IncasInfoSendManager IncasInfoSendManager
        {
            get { return m_IncasInfoSendManager; }
        }


        public SendInfoManager()
        {
            m_SendThread = new Thread(new ThreadStart(SendProc));
            m_SongInfoSendManager = new SongInfoSendManager();
            m_IncasInfoSendManager = new IncasInfoSendManager();
            if(m_SendDataToInet)
                m_SendThread.Start();
        }
        private void SendProc()
        {
            do
            {
                m_SongInfoSendManager.Send();
                m_IncasInfoSendManager.Send();
                /*new AppInfoSendManager().Send(new AppInfoSendManager.AppInfo(
                    CBInfo.Instance.Sum.Value.ToString(), CBInfo.Instance.SumCommon.Value.ToString(),
                    CBInfo.Instance.BillsCount.Value.ToString(), Main.MainWnd.billValidatorManager.State.ToString()));*/
            } while (!m_SendEvent.WaitOne(TimeSpan.FromSeconds(m_SendDataIntervalSec), false));
        }

        private void Cleanup()
        {
            if (m_SendDataToInet)
            {
                Debug.Assert(m_SendThread.IsAlive == true);
                m_SendEvent.Set();
                m_SendThread.Join();               
            }
            m_SongInfoSendManager.Dispose();
            m_IncasInfoSendManager.Dispose();
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
        ~SendInfoManager()
        {
            lock (this)
            {
                Cleanup();
            }
        }
    }
}
