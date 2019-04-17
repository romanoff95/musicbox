using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace musicbox.Search
{
    public partial class SearchForm : Form
    {
        private Font fnt = new Font("Arial", 20, FontStyle.Bold);
        private String m_strSearch = "";
        private readonly String m_NothingFoundMsg = "Ничего не найдено";
        private readonly String m_SearchStringNotPassedMsg = "Введите строку для поиска";
        private delegate void EnableControlsDel(Boolean enable);
        private Search.SearchResults m_Results;
        private BackgroundWorker searchWorker;
        private readonly string m_RootPath = Properties.Settings.Default.RootPath;
        private readonly IMediaManager m_IMediaManager;


        internal SearchForm(IMediaManager iMediaManager)
        {
            InitializeComponent();
#if FULL
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.AutoScroll = false;
#endif
            pictureBox2.Paint += new PaintEventHandler(pictureBox2_Paint);
            m_IMediaManager = iMediaManager;
        }

        void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(m_strSearch, fnt, Brushes.Black, new PointF(0, ((PictureBox)sender).Height / 2));
        }

        void ClickChar(Char ch)
        {
            m_strSearch += ch.ToString();
            pictureBox2.Invalidate();
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            ClickChar('А');
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ClickChar('Б');
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ClickChar('В');
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ClickChar('Г');
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ClickChar('Д');
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ClickChar('Е');
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            ClickChar('Ё');
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            ClickChar('Ж');
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            ClickChar('З');
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            ClickChar('И');
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            ClickChar('Й');
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            ClickChar('К');
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            ClickChar('Л');
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            ClickChar('М');
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            ClickChar('Н');
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            ClickChar('О');
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            ClickChar('П');
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            ClickChar('Р');
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            ClickChar('С');
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            ClickChar('Т');
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            ClickChar('У');
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            ClickChar('Ф');
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            ClickChar('Х');
        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {
            ClickChar('Ц');
        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            ClickChar('Ч');
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {
            ClickChar('Ш');
        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {
            ClickChar('Щ');
        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {
            ClickChar('Ъ');
        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {
            ClickChar('Ы');
        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {
            ClickChar('Ь');
        }

        private void pictureBox38_Click(object sender, EventArgs e)
        {
            ClickChar('Э');
        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {
            ClickChar('Ю');
        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {
            ClickChar('Я');
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            ClickChar('A');
        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {
            ClickChar('B');
        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {
            ClickChar('C');
        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {
            ClickChar('D');
        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {
            ClickChar('E');
        }

        private void pictureBox44_Click(object sender, EventArgs e)
        {
            ClickChar('F');
        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {
            ClickChar('G');
        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {
            ClickChar('H');
        }

        private void pictureBox47_Click(object sender, EventArgs e)
        {
            ClickChar('I');
        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {
            ClickChar('J');
        }

        private void pictureBox49_Click(object sender, EventArgs e)
        {
            ClickChar('K');
        }

        private void pictureBox50_Click(object sender, EventArgs e)
        {
            ClickChar('L');
        }

        private void pictureBox51_Click(object sender, EventArgs e)
        {
            ClickChar('M');
        }

        private void pictureBox52_Click(object sender, EventArgs e)
        {
            ClickChar('N');
        }

        private void pictureBox53_Click(object sender, EventArgs e)
        {
            ClickChar('O');
        }

        private void pictureBox54_Click(object sender, EventArgs e)
        {
            ClickChar('P');
        }

        private void pictureBox55_Click(object sender, EventArgs e)
        {
            ClickChar('Q');
        }

        private void pictureBox56_Click(object sender, EventArgs e)
        {
            ClickChar('R');
        }

        private void pictureBox57_Click(object sender, EventArgs e)
        {
            ClickChar('S');
        }

        private void pictureBox59_Click(object sender, EventArgs e)
        {
            ClickChar('T');
        }

        private void pictureBox60_Click(object sender, EventArgs e)
        {
            ClickChar('U');
        }

        private void pictureBox61_Click(object sender, EventArgs e)
        {
            ClickChar('V');
        }

        private void pictureBox62_Click(object sender, EventArgs e)
        {
            ClickChar('W');
        }

        private void pictureBox63_Click(object sender, EventArgs e)
        {
            ClickChar('X');
        }

        private void pictureBox64_Click(object sender, EventArgs e)
        {
            ClickChar('Y');
        }

        private void pictureBox65_Click(object sender, EventArgs e)
        {
            ClickChar('Z');
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            m_strSearch = "";
            pictureBox2.Invalidate();
        }
        //Exit 
        private void pictureBox58_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        internal Search.SearchResults Results
        {
            get { return m_Results; }
            //set { m_Results = value; }
        }
        
        //Start
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (searchWorker.IsBusy)
                return;
            Debug.Assert(m_strSearch.Length >= 0);
            if (m_strSearch.Length == 0)
            {
                ApplicationMessage.Show(this, m_SearchStringNotPassedMsg);
                return;
            }
            EnableControlsDel enableControlsDel = delegate(Boolean enable)
            {
                foreach (Control ctrl in Controls)
                {
                    if (ctrl != pbSearchStatus)
                        ctrl.Enabled = enable;
                }    
            };
            enableControlsDel(false);

            searchWorker = new BackgroundWorker();
            searchWorker.DoWork += new DoWorkEventHandler(StartSearch);
            searchWorker.WorkerReportsProgress = true;
            searchWorker.ProgressChanged += delegate(object sender1, ProgressChangedEventArgs e1)
            {
                pbSearchStatus.Value = e1.ProgressPercentage;
            };
            searchWorker.RunWorkerCompleted += delegate(object sender2, RunWorkerCompletedEventArgs e2)
            {
                if(e2.Error != null)
                    throw e2.Error;
                enableControlsDel(true);
                Search.SearchResults searchResults = (Search.SearchResults)e2.Result;
                if (searchResults.Directories.Count > 0 || searchResults.Files.Count > 0)
                {
                    m_Results = searchResults;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    ApplicationMessage.Show(this, m_NothingFoundMsg);
                }
            };
            searchWorker.RunWorkerAsync(m_strSearch);
            
        }

        private void StartSearch(Object sender, DoWorkEventArgs e)
        {
            List<string> extensions = new List<string>();
            foreach(_Media media in m_IMediaManager.GetAllMedia())
            {
                extensions.Add(media.Extension);
            }
            Search.SearchResults searchResults;
            Search.SearchFilesInPath(m_RootPath, (string)e.Argument, extensions.ToArray(),out searchResults);
            e.Result = searchResults;
        }

        /*void Search_SearchProgress(object sender, int e)
        {
            throw new Exception("The method or operation is not implemented.");
        }*/
    }
}