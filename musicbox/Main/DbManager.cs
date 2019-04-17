using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Diagnostics;
using System.Data;
using System.IO;

namespace musicbox.DbManager
{
    abstract class DbBase<T> where T : new()
    {
        private OleDbConnection conn;
        private static T instance;
        private readonly String m_ConnectString = Properties.Settings.Default.ConnectionString;

        protected struct ParameterElement
        {
            private String m_ColumnName;
            private Object m_Param;
            private OleDbType m_Type;
            private Int32 m_Size;

            public ParameterElement(String columnName, Object param, OleDbType type, Int32 size)
            {
                this.m_ColumnName = columnName;
                this.m_Param = param;
                this.m_Type = type;
                this.m_Size = size;
            }
            public Object Param
            {
                get { return m_Param; }
                set { m_Param = value; }
            }
            public String ColumnName
            {
                get { return m_ColumnName; }
                set { m_ColumnName = value; }
            }
            public OleDbType Type
            {
                get { return m_Type; }
                set { m_Type = value; }
            }
            public Int32 Size
            {
                get { return m_Size; }
                set { m_Size = value; }
            }
        }

        protected DbBase()
        {
            this.conn = new OleDbConnection(m_ConnectString);
            conn.Open();
        }
        public virtual void Dispose()
        {
            if (instance != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        protected OleDbConnection Connection
        {
            get { return conn; }
            set { conn = value; }
        }

        protected void Add(String tableName, ParameterElement[] elements)
        {
            Debug.Assert(conn.State == System.Data.ConnectionState.Open);
            Debug.Assert(elements.Length > 0);
            OleDbCommand cmd = conn.CreateCommand();
            List<ParameterElement> elementsList = new List<ParameterElement>(elements);
            List<String> colNameList = elementsList.ConvertAll<String>(delegate(ParameterElement pe)
            {
                return pe.ColumnName;
            });
            elementsList.ForEach(delegate(ParameterElement pe)
            {
                if (pe.Size != 0)
                {
                    cmd.Parameters.Add(new OleDbParameter(String.Concat("@", pe.ColumnName), pe.Type, pe.Size));
                }
                else
                    cmd.Parameters.Add(new OleDbParameter(String.Concat("@", pe.ColumnName), pe.Type));
                cmd.Parameters[String.Concat("@", pe.ColumnName)].Value = pe.Param;
            });
            cmd.CommandText = String.Format(
                "INSERT INTO {0}({1}) VALUES({2})",
                tableName, String.Join(",",colNameList.ToArray()), String.Concat("@",String.Join(",@",colNameList.ToArray())));
            cmd.ExecuteNonQuery();
        }
       
    }
 
    sealed class DbSong : DbBase<DbSong>
    {
        private readonly String m_TableName = "Ordered_Songs";
        private String m_SongOrderedColName = "dtSongOrder";
        private String m_SongNameColName = "vcSongName";
        private const Int32 nSongNameColSize = 100;

        public struct SongInfo
        {
            private String m_SongName;
            private DateTime m_SongOrdered;
            public SongInfo(String songName, DateTime songOrdered)
            {
                m_SongName = songName;
                m_SongOrdered = songOrdered;
            }
            public DateTime SongOrdered
            {
                get { return m_SongOrdered; }
                set { m_SongOrdered = value; }
            }
            public String SongName
            {
                get { return m_SongName; }
                set { m_SongName = value; }
            }
        }
        public void AddRange(SongInfo[] songInfoArr)
        {
            /*base.Add(strTableName, new ParameterElement[] {
                new ParameterElement(strSongNameColName,songName,OleDbType.VarChar,nSongNameColSize),
                new ParameterElement(strDateSongOrderColName,dtSongOrder,OleDbType.Date,0)});*/
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            foreach (SongInfo songInfo in songInfoArr)
            {
                OleDbCommand command = Connection.CreateCommand();
                String songNameParamName = String.Concat("@",m_SongNameColName);
                String songOrderedParamName = String.Concat("@",m_SongOrderedColName);
                command.CommandText = String.Format("INSERT INTO {0}({1},{2}) VALUES({3},{4})",
                    m_TableName, m_SongNameColName,m_SongOrderedColName,songNameParamName,songOrderedParamName);
                command.Parameters.Add(new OleDbParameter(songNameParamName, OleDbType.VarChar, nSongNameColSize));
                command.Parameters.Add(new OleDbParameter(songOrderedParamName,OleDbType.Date));

                command.Parameters[songNameParamName].Value = songInfo.SongName;
                command.Parameters[songOrderedParamName].Value = songInfo.SongOrdered;

                command.ExecuteNonQuery();
            }
            
        }
        public String[] GetTopTen()
        {
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            OleDbCommand cmd;
            OleDbDataReader rdr;
            List<String> retList = new List<String>();

            cmd = Connection.CreateCommand();
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
    sealed class Bills : DbBase<Bills>
    {
        private String strTableName = "Bills";
        private String strDateColName = "dtBillIn";
        private String strBillInColName = "nBillNominal";

        public void AddBill(Int32 nBillNominal,DateTime dtBillIn)
        {
            Add(strTableName, new ParameterElement[] {
                new ParameterElement(strDateColName,dtBillIn,OleDbType.Date,0),
                new ParameterElement(strBillInColName,nBillNominal,OleDbType.Integer,0)});
        }
        public Boolean GetBillsQuantity(out Int32 nBillsCount)
        {
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            Boolean bRet = false;
            nBillsCount = 0;
            OleDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(
                "SELECT COUNT(*) FROM {0}", strTableName);
            Object oResult = cmd.ExecuteScalar();
            if (oResult != null)
            {
                nBillsCount = (Int32)oResult;
                bRet = true;
            }
            return bRet;
        }
        public Boolean GetBillsQuantity(DateTime DateOf,out Int32 nBillsCount)
        {
            String strDateParamName = "@dtDateOf";
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            Boolean bRet = false;
            nBillsCount = 0;
            OleDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(
                "SELECT COUNT(*) FROM {0} WHERE {1} > {2}", strTableName, strDateColName, strDateParamName);
            cmd.Parameters.Add(new OleDbParameter(strDateParamName, OleDbType.Date));
            cmd.Parameters[strDateParamName].Value = DateOf;
            Object oResult = cmd.ExecuteScalar();
            if (oResult != null)
            {
                nBillsCount = (Int32)oResult;
                bRet = true;
            }
            return bRet;
        }
        public Boolean GetSum(out Decimal nSum)
        {
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            Boolean bRet = false;
            nSum = 0;
            OleDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(
                "SELECT SUM({0}) FROM {1}", strBillInColName, strTableName);
            Object oResult = cmd.ExecuteScalar();
            if (oResult != null)
            {
                nSum = (Decimal)oResult;
                bRet = true;
            }
            return bRet;
        }
        public Boolean GetSum(DateTime DateOf,out Decimal nSum)
        {
            String strDateParamName = "@dtDateOf";
            Debug.Assert(Connection.State == System.Data.ConnectionState.Open);
            Boolean bRet = false;
            nSum = 0;
            OleDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(
                "SELECT SUM({0}) FROM {1} WHERE {2} > {3}", strBillInColName, strTableName, strDateColName, strDateParamName);
            Object oResult = cmd.ExecuteScalar();
            if (oResult != null)
            {
                nSum = (Decimal)oResult;
                bRet = true;
            }
            return bRet;
        }
    }

    sealed class Incas : DbBase<Incas>
    {
        private String strTableName = "Incas";
        private String strDateColName = "dtIncas";
        private String strBillsQuantityColName = "nBillsQuantity";
        private String strIncasSumColName = "nIncasSum";

        public void AddIncas(DateTime dtIncasData, Int32 nBillsQuantity,
            Int32 nIncasSum)
        {
            Add(strTableName, new ParameterElement[] {
                new ParameterElement(strDateColName,dtIncasData,OleDbType.Date,0),
                new ParameterElement(strIncasSumColName,nIncasSum,OleDbType.Integer,0),
                new ParameterElement(strBillsQuantityColName,nBillsQuantity,OleDbType.Integer,0)});
        }
        public Boolean GetLastIncas(out DateTime dtIncas)
        {
            Boolean bRet = false;
            dtIncas = DateTime.Now;
            OleDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(
                "SELECT TOP 1 {0} FROM {1} ORDER BY {0} DESC",
                strDateColName, strTableName);
            Object oResult = cmd.ExecuteScalar();
            if (oResult != null)
            {
                dtIncas = (DateTime)oResult;
                bRet = true;
            }
            return bRet;
        }

    }
}
