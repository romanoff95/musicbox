using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;

namespace musicbox
{
    internal struct _MediaInfo
    {
        private int m_DbId;
        private readonly string m_Extension;
        private int m_Price;
        private int m_DemoVolume;
        private int m_OrderVolume;
        private string m_Description;

        public string Extension
        {
            get { return m_Extension; }
        }

        public _MediaInfo(int dbId, string extension, int price,
            int demoVolume, int orderVolume, string description)
        {
            m_DbId = dbId;
            m_Extension = extension;
            m_Price = price;
            m_DemoVolume = demoVolume;
            m_OrderVolume = orderVolume;
            m_Description = description;
        }
    }

    class MediaItemsSettings
    {


        private readonly List<_MediaInfo> m_Items = new List<_MediaInfo>();
        private readonly string m_DbConnString = Properties.Settings.Default.ConnectionString;
        //private readonly string m_TableName = "MediaItems";
        private readonly DataTable dtMediaItems = new DataTable();
        private readonly OleDbDataAdapter da;

        public MediaItemsSettings()
        {
            da = new OleDbDataAdapter(
                "SELECT * FROM MediaItems", m_DbConnString);
            da.Fill(dtMediaItems);

            foreach(DataRow row in dtMediaItems.Rows)
            {
                if (Boolean.Parse(row["Used"].ToString()))
                {
                    m_Items.Add(
                        new _MediaInfo(Int32.Parse(row["Id"].ToString()),
                        row["Extension"].ToString(), Int32.Parse(row["Price"].ToString()),
                        Int32.Parse(row["DemoVolume"].ToString()), Int32.Parse(row["OrderVolume"].ToString()),
                        row["Description"].ToString()));
                }
            }
        }

        public List<_MediaInfo> Items
        {
            get { return m_Items; }
        }
        ~MediaItemsSettings()
        {
            Debug.WriteLine("MediaItemsSettings.Finalize()");
            dtMediaItems.Dispose();
            da.Dispose();
        }
    }
}
