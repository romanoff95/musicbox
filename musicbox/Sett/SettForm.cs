using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace musicbox.Sett
{
    public partial class SettForm : Form
    {
        private readonly string m_DbConnectionString = Properties.Settings.Default.ConnectionString;
        private readonly string m_MediaSettingsDbSelectString =
            "SELECT Extension,Price,DemoVolume,OrderVolume,Used,Description,Image FROM MediaItems ORDER BY Id";
        private readonly OleDbDataAdapter m_DataAdapter;
        private readonly DataTable m_DataTable;

        public SettForm()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = Properties.Settings.Default;

            m_DataAdapter = new OleDbDataAdapter(m_MediaSettingsDbSelectString, m_DbConnectionString);
            m_DataTable = new DataTable();
            m_DataAdapter.Fill(m_DataTable);
            foreach (DataRow dr in m_DataTable.Rows)
            {
                int index = 0;
                Byte[] src = ((Byte[])dr["Image"]);
                /*Byte[] dst = new Byte[src.Length - nn];
                
                Array.Copy(src, nn, dst, 0, dst.Length);
                MemoryStream ms = new MemoryStream();
                ms.Write(src, nn,
                    src.Length - nn);
                
                Image img = Image.FromStream(ms);*/
                byte[] dst = null;
                for (int i = 0; i < src.Length; i++)
			    {
                    if (src[i] == 0xff && i + 4 < src.Length)
                    {
                        if ((src[i + 1] == 0xD8) && (src[i + 2] == 0xff) && (src[i + 3] == 0xe0))
                        {
                            index = i;
                            break;
                        }
                    }
                }
                if (index != 0)
                {
                    dst = new byte[src.Length - index];
                    Array.Copy(src, index, dst, 0, dst.Length);
                }
                dr["Image"] = dst;
            }
            dataGridView1.DataSource = m_DataTable;
        }

        /*private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_InteractNow != null)
            {
                m_InteractNow.Save();
            }
            m_InteractNow = null;
            dataGridView1.DataSource = null;
            dataGridView1.Update();
            if (String.Equals(e.Node.Name, treeView1.Nodes[0].Name, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("CommonSettings");
            }
            else if (String.Equals(e.Node.Name, treeView1.Nodes[1].Name, StringComparison.OrdinalIgnoreCase))
            {
                m_InteractNow = new MediaSettings(dataGridView1);
                m_InteractNow.Present();
            }
            else if (String.Equals(e.Node.Name, treeView1.Nodes[1].Name, StringComparison.OrdinalIgnoreCase))
            {

            }
        }*/
    }
    /*abstract class BaseSettings
    {
        abstract public void Present();
        abstract public void Save();
    }
    class MediaSettings : BaseSettings
    {
        private readonly string m_DbConnectionString = Properties.Settings.Default.ConnectionString;
        private readonly string m_MediaSettingsDbSelectString =
            "SELECT Id,Extension,Price,DemoVolume,OrderVolume,Used,Description,Image FROM MediaItems ORDER BY Id";
        private OleDbDataAdapter m_DataAdapter;
        private DataGridView m_DataGridView;
        private readonly DataTable m_DataTable = new DataTable();

        public MediaSettings(DataGridView dgv)
        {
            m_DataGridView = dgv;
            m_DataAdapter = new OleDbDataAdapter(m_MediaSettingsDbSelectString, m_DbConnectionString);
        }
        public override void Present()
        {
            m_DataAdapter.Fill(m_DataTable);
            m_DataGridView.DataSource = m_DataTable;
        }
        public override void Save()
        {
            if (m_DataTable.GetChanges() != null)
            {
                if (MessageBox.Show("Хотите сохранить изменения", "Info", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new OleDbCommandBuilder(m_DataAdapter);
                    m_DataAdapter.Update(m_DataTable);
                }
            }
        }
    }*/
}