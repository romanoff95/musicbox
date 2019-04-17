using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace musicbox.Menu
{
    public partial class DemoManageForm : Form
    {
        private delegate void UIUpdateDel();
        private readonly OleDbConnection m_Connection;
        private readonly string m_ConnectString = Properties.Settings.Default.ConnectionString;
        private readonly MediaNode m_RootMediaNode = new MediaNode("root");
        //[StructLayout(LayoutKind.Auto)]
        private class MediaNode
        {
            private string m_Name;
            private readonly Dictionary<string, MediaNode> m_MediaNode = new Dictionary<string, MediaNode>();

            public string Name
            {
                get { return m_Name; }
                set { m_Name = value; }
            }

            public Dictionary<string, MediaNode> _MediaNodes
            {
                get { return m_MediaNode; }
            }
            public MediaNode(string name)
            {
                m_Name = name;
            }
        }

        public DemoManageForm()
        {
            InitializeComponent();
            m_Connection = new OleDbConnection(m_ConnectString);
            m_Connection.Open();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += InitSelectTree;
            bw.RunWorkerAsync(treeView1);
        }

        
        private void InitSelectTree(Object sender, DoWorkEventArgs e)
        {
            DirectoryInfo diRootPath = new DirectoryInfo(Properties.Settings.Default.RootPath);
            DirectoryInfo[] diaRootPath = diRootPath.GetDirectories();
            //Int32 counter = 0;
            foreach (DirectoryInfo di in diaRootPath)
            {
                DirectoryInfo[] diaArtists = di.GetDirectories();
                TreeNode styleNode = new TreeNode(di.Name);
                foreach (DirectoryInfo di1 in diaArtists)
                {
                    TreeNode artistNode = new TreeNode(di1.Name);
                    FileInfo[] fia = di1.GetFiles();
                    foreach (FileInfo fi in fia)
                    {
                        TreeNode fileNode = new TreeNode(fi.Name);
                        artistNode.Nodes.Add(fileNode);
                    }
                    styleNode.Nodes.Add(artistNode);
                }
                //counter++;
                TreeView tree = (TreeView)e.Argument;
                tree.BeginInvoke(new UIUpdateDel(delegate
                {
                    tree.Nodes.Add(styleNode);
                }));
            }
            
        }
        //private object FieldExist(string tableName, string expressionCol, string expressionVal,string selectCol)
        //{
            /*OleDbCommand command = m_Connection.CreateCommand();
            command.CommandText = String.Format(
                "SELECT {0} FROM {1} WHERE {3} = {4}",
                selectCol, tableName, expressionCol, expressionVal);
            return command.ExecuteScalar();*/
            //object selectObj = command.ExecuteScalar();
            //return selectObj == null ? false : true;
        //}
        /*private object AddField(string tableName,string colName,string Name)
        {
            
            OleDbCommand command = m_Connection.CreateCommand();
            command.
            command.CommandText = String.Format(
                "INSERT INTO {0}({1}) VALUES({2})",
                tableName, colName, Name);
            command.ExecuteNonQuery(
        }*/
        /*private void InsertAndGetId(string name)
        {
            string styleTableName = "Demo_Style";
            string styleNameColName = "Name";
            int styleNameColLength = 200;

            OleDbCommand command = m_Connection.CreateCommand();
            command.CommandText = String.Format("INSERT INTO {0}({1}) VALUES(@{1})",styleTableName,styleNameColName);
            command.Parameters.Add(new OleDbParameter("@" + styleNameColName,OleDbType.WChar,styleNameColLength));
            command.Parameters["@" + styleNameColName].Value = name;
            command.ExecuteNonQuery();
            command.CommandText = "SELECT {0} FROM {1} WHERE {2} = {3}";
        }*/
        /*private MediaNode InsertZeroLevel(string text)
        {
            MediaNode mediaNode;
            if (!m_RootMediaNode._MediaNodes.TryGetValue(text, out mediaNode))
            {
                mediaNode = new MediaNode();
                m_RootMediaNode._MediaNodes.Add(text, mediaNode);
            }
            return mediaNode;
        }
        private MediaNode InsertChildNodes(MediaNode parentMediaNode,TreeNode parentTreeNode)
        {
            foreach (TreeNode node in parentTreeNode)
            {
                MediaNode childMediaNode = new MediaNode();
                parentMediaNode._MediaNodes.Add(node.Text);
                InsertChildNodes(childMediaNode, node);
            }
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
           /* string styleTableName = "Demo_Style";
            string artistTableName = "Demo_Artist";
            string mediaTableName = "Demo_Media";

            TreeNode node = treeView1.SelectedNode;
            if (node == null)
            {
                MessageBox.Show("Nothing select");
                return;
            }
            if (node.Level > 2)
            {
                return;
            }
            TreeNode rootNode = node;
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }
            Debug.Assert(rootNode.Level == 0);
            switch (node.Level)
            {
                case 0:
                    MediaNode mediaNode = InsertZeroLevel(node.Text);
                    InsertChildNodes(mediaNode, rootNode);
                case 1:
            }*/
        }
    }
}