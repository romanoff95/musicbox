using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace musicbox.Menu
{
    public partial class DemoSelect : Form
    {
        //private String m_RootPath;

        private static DemoSelect instance;
        private delegate void UpdateNodesDel();
        private readonly Int32 m_IcoSize = 32;
        private readonly Bitmap m_FolderIcon;
        private readonly Icon m_FolderRes = Properties.Resources.folderopen;
        private readonly String m_FolderIconKeyName = "folder";
        private readonly String m_SerializeFileName = "demo.bin";

        private List<String> m_DemoList = new List<string>(); // for demo matters
        private Object m_DemoListLocker = new object();
        private delegate void UpdateDemoListDel();

        private DemoSelect()
        {
            
            
            InitializeComponent();

            m_FolderIcon = m_FolderRes.ToBitmap();
            m_FolderIcon.MakeTransparent();

            InitTreeImageList(treeView1);
            InitTreeImageList(treeView2);

            TreeNode[] treeNodesArr;
            if (Program.DeserializeFromBinary<TreeNode[]>(m_SerializeFileName, out treeNodesArr))
            {
                //TreeNode[] treeNodesArr = treeNodesArrObj as TreeNode[];
                treeView2.Nodes.AddRange(treeNodesArr);
            }
            else
            {
                Debug.Assert(false);
                throw new Exception("Demo");
                
            }

            
            // demo fileList init;
            UpdateDemoList();

            Load += new EventHandler(DemoSelect_Load);

            btnFinish.Click += delegate
            {
                TreeNode[] treeNodesArrEx = new TreeNode[treeView2.Nodes.Count];
                treeView2.Nodes.CopyTo(treeNodesArrEx,0);
                //DbManipulation(treeNodesArr);
                Program.SerializeToBinary(m_SerializeFileName, treeNodesArrEx);
                UpdateDemoList();
            };
            btnCancel.Click += delegate
            {
                this.Close();
            };
            btnDelete.Click += delegate
            {
                if (treeView2.SelectedNode == null)
                {
                    MessageBox.Show("Ничего не выбрано");
                }
                else
                {
                    treeView2.SelectedNode.Remove();
                }
            };

        }

        private void DbManipulation(TreeNode[] treeNodesArr)
        {
            string table = "Demo_Style";
            //OleDbConnection connection = new OleDbConnection(Properties.Settings.Default.ConnectionString);
            OleDbDataAdapter da = new OleDbDataAdapter(
                String.Format("SELECT * FROM {0} ",table),Properties.Settings.Default.ConnectionString);
            da.RowUpdated += new OleDbRowUpdatedEventHandler(da_RowUpdated);
            new OleDbCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataColumn col = dt.Columns[0];
            DataRow row = dt.NewRow();
            row["Name"] = "31337" ;
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Name"] = "31337";
            dt.Rows.Add(row);
            da.Update(dt);
        }

        void da_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            MessageBox.Show(e.Row["Id"].ToString());
            if (e.StatementType == StatementType.Insert)
            {
                OleDbCommand command = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                int ret = (int)command.ExecuteScalar();
                Debug.WriteLine(ret.ToString());
            }
        }

        void DemoSelect_Load(object sender, EventArgs e)
        {
            DisableControls(false);
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += InitSelectTree;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += delegate(Object sender1, ProgressChangedEventArgs e1)
            {
                if (e1.ProgressPercentage > 0)
                {
                    toolStripProgressBar1.Value = e1.ProgressPercentage;
                }
            };
            bw.RunWorkerCompleted += delegate(Object sender1, RunWorkerCompletedEventArgs e1)
            {
                DisableControls(true);
            };
            bw.RunWorkerAsync(treeView1);
            Load -= new EventHandler(DemoSelect_Load);
            
        }
        private void DisableControls(Boolean value)
        {
            foreach (Control ctrl in Controls)
            {
                ctrl.Enabled = value;
            }
        }

        public static DemoSelect Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DemoSelect();
                }
                return instance;
            }
        }
        private void InitTreeImageList(TreeView tree)
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(m_IcoSize, m_IcoSize);
            imageList.Images.Add(m_FolderIconKeyName, m_FolderIcon);

            foreach (_Media media in musicbox.MediaManager.Instance.GetAllMedia())
            {
                imageList.Images.Add(media.Extension, media.Img);
            }
            tree.ImageList = imageList;
        }
        
        private void InitSelectTree(Object sender,DoWorkEventArgs e)
        {
            DirectoryInfo diRootPath = new DirectoryInfo(Properties.Settings.Default.RootPath);
            DirectoryInfo[] diaRootPath = diRootPath.GetDirectories();
            Int32 counter = 0;
            foreach (DirectoryInfo di in diaRootPath)
            {
                DirectoryInfo[] diaArtists = di.GetDirectories();
                TreeNode styleNode = CreateTreeNode(di.Name,di.Name, false);
                foreach (DirectoryInfo di1 in diaArtists)
                {
                    TreeNode artistNode = CreateTreeNode(di1.Name,di1.Name, false); // true - file , false - dir;
                    FileInfo[] fia = di1.GetFiles();
                    foreach (FileInfo fi in fia)
                    {
                        TreeNode fileNode = CreateTreeNode(fi.Name,fi.FullName, true);
                        fileNode.ImageIndex = 1;
                        fileNode.SelectedImageIndex = 1; // todo pic by ext
                        artistNode.Nodes.Add(fileNode);
                    }
                    styleNode.Nodes.Add(artistNode);
                }
                counter++;
                ((BackgroundWorker)sender).ReportProgress(100 / diaRootPath.Length  * counter);
                TreeView tree = (TreeView)e.Argument;
                tree.BeginInvoke(new UpdateNodesDel(delegate
                {
                    tree.Nodes.Add(styleNode);
                }));
            }
            ((BackgroundWorker)sender).ReportProgress(100);
        }

        private TreeNode CreateTreeNode(String strText, String strName, Object bIsFile)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Text = strText;
            treeNode.Name = strName;
            treeNode.Tag = bIsFile;
            return treeNode;
        }

        private void TreeNodeCopy(TreeNode srcNode, TreeNode dstNode)
        {
            if (srcNode.Nodes.Count > 0)
            {
                foreach (TreeNode node in srcNode.Nodes)
                {
                    TreeNode childNode = CreateTreeNode( node.Text,node.Name, node.Tag);
                    TreeNodeCopy(node, childNode);
                    dstNode.Nodes.Add(childNode);
                }
            }
        }
        private Boolean IsContains(TreeNode node, String strNode)
        {
            return node.Nodes.ContainsKey(strNode) ? true : false;
        }
        
        private Boolean GetLower(TreeNode rootNode ,String[] straPath,out TreeNode outNode)
        {
            Boolean retFlag = false;
            TreeNode node = rootNode;
            for (Int32 i = 1; i < straPath.Length; i++)
            {
                if (!IsContains(node, straPath[i]))
                {
                    retFlag = true;
                    break;
                }
                node = node.Nodes[straPath[i]];
            }
            outNode = node;
            return retFlag;
        }
        /*private TreeNode TreeNodeFromStrPath(TreeNode rootNode,String[] straPath)
        {
            TreeNode LowerNode = rootNode;
            for (Int32 i = 1; i < straPath.Length; i++)
            {
                TreeNode trNode = CreateTreeNode(straPath[i]);
                trNode.Name = straPath[i];
                rootNode.Nodes.Add(trNode);
                LowerNode = trNode;
            }
            return LowerNode;
        }*/
        struct nodeStruct
        {
            public nodeStruct(String strName, String strText, Object Tag)
            {
                this.strName = strName;
                this.strText = strText;
                this.Tag = Tag;
            }
            public String strName;
            public String strText;
            public Object Tag;
        }
        private void NodeListAboveTarget(TreeNode targetNode,List<nodeStruct> treeNodeList)
        {
            TreeNode _srcNode = targetNode;
            while (_srcNode != null)
            {
                treeNodeList.Add(new nodeStruct(_srcNode.Name,_srcNode.Text,_srcNode.Tag));
                _srcNode = _srcNode.Parent;
            }
        }
        private TreeNode NodeListToBottom(List<nodeStruct> nodeList,out TreeNode outNode)
        {
            TreeNode header = CreateTreeNode(nodeList[0].strText, nodeList[0].strText,nodeList[0].Tag);
            TreeNode nd = header;
            for(Int32 i = 1 ; i < nodeList.Count ; i++)
            {
                nd.Nodes.Add(CreateTreeNode(nodeList[i].strText, nodeList[i].strText,nodeList[i].Tag));
                nd = nd.Nodes[0];
            }
            outNode = nd;
            return header;
        }
        private TreeNode RootNode(TreeNode targetNode)
        {
            TreeNode rootNode = targetNode;
            while (rootNode != null)
            {
                rootNode = rootNode.Parent;
            }
            return rootNode;
        }
        
        private void btnMove_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                List<nodeStruct> ndStrList = new List<nodeStruct>();
                NodeListAboveTarget(treeView1.SelectedNode, ndStrList);
                ndStrList.Reverse();
                Debug.Assert(ndStrList.Count > 0);

                if (treeView2.Nodes.ContainsKey(ndStrList[0].strName))
                {
                    TreeNode ndCycler = treeView2.Nodes[ndStrList[0].strName];
                    Int32 i = 0;
                    for (i = 1; i < ndStrList.Count; i++)
                    {
                        if (ndCycler.Nodes.ContainsKey(ndStrList[i].strName))
                        {
                            ndCycler = ndCycler.Nodes[ndStrList[i].strName];
                        }
                        else
                            break;
                    }
                    for (Int32 j = i; j < ndStrList.Count; j++)
                    {
                        ndCycler.Nodes.Add(CreateTreeNode(ndStrList[j].strText,ndStrList[j].strName, ndStrList[j].Tag));
                        ndCycler = ndCycler.Nodes[ndStrList[j].strName];
                    }
                    TreeNodeCopy(treeView1.SelectedNode, ndCycler);

                }
                else
                {
                    TreeNode lowNode;
                    TreeNode RootNode = NodeListToBottom(ndStrList,out lowNode);
                    TreeNodeCopy(treeView1.SelectedNode, lowNode);
                    treeView2.Nodes.Add(RootNode);

                }
                treeView2.Sort();
            }
        }
        // Demo
        private void _recursive(TreeNode treeNode, ref List<String> demoList)
        {
            if (treeNode.Nodes.Count > 0)
            {
                foreach (TreeNode tnChild in treeNode.Nodes)
                {
                    if ((Boolean)tnChild.Tag == true)
                        demoList.Add(tnChild.Name);
                    _recursive(tnChild, ref demoList);
                }
            }
        }
        private void UpdateDemoList()
        {
            BackgroundWorker bw1 = new BackgroundWorker();
            bw1.WorkerReportsProgress = true;
            bw1.DoWork += delegate(Object sender, DoWorkEventArgs e)
            {
                List<String> demoList = new List<string>();
                foreach (TreeNode treeNode in treeView2.Nodes)
                {
                    _recursive(treeNode, ref demoList);
                }
                lock (m_DemoListLocker)
                    m_DemoList = demoList;
                if (!this.IsHandleCreated)
                    DisableControls(true);
                else
                    this.BeginInvoke(new UpdateDemoListDel(delegate
                {
                    DisableControls(true);
                }));
            };
            DisableControls(false);
            bw1.RunWorkerAsync();
        }
        public Boolean GetRandomDemoFileName(out String Path)
        {
            Path = "";
            Boolean ret = false;
            lock (m_DemoListLocker)
            {
                if (m_DemoList.Count > 0)
                {
                    Random random = new Random();
                    Path = m_DemoList[random.Next(m_DemoList.Count)];
                    ret = true;
                }
            }
            return ret;
        }
    }
}