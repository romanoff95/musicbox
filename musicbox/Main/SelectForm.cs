using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace musicbox.Main
{
    public partial class SelectForm : Form
    {
        private readonly String m_strCorruptFileInfo = "Данная композиция недоступна," + Environment.NewLine +
            "пожалуйста выберите другую.";
        private readonly String m_strMoneyNotEnough = "У вас не достаточно средств";
        private readonly String m_MediaYetOrder = "Данная композиция уже выбрана";
        private readonly String m_MediaExistInPlaylist = "Композиция была заказана";
        
        private FileNameAndMediaWithIsMoneyless[] m_ResultMedia = new FileNameAndMediaWithIsMoneyless[0];

        private SelectedMedia m_SelectedMedia;
        private SelectingMedia m_SelectingMedia;
        private Artist m_Artist;
        private IMediaManager m_IMediaManager;


        internal SelectForm(IMediaManager iMediaManager)
        {
            InitializeComponent();
            m_IMediaManager = iMediaManager;
#if FULL
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoScroll = false;
#endif

            m_SelectedMedia = new SelectedMedia(this);
            m_SelectingMedia = new SelectingMedia(this);
            m_Artist = new Artist(this);

            headerCtrl1.StyleItems =
                new List<DirectoryInfo>(new DirectoryInfo(Properties.Settings.Default.RootPath).GetDirectories()).ConvertAll<String>(
                delegate(DirectoryInfo di)
                {
                    return di.Name;
                }).ToArray();
            headerCtrl1.Mode = HeaderCtrl.Modes.Styles;
            headerCtrl1.ItemClick += delegate(Object sender, EventArgs e)
            {
                HeaderCtrl hc = (HeaderCtrl)sender;
                HeaderCtrl.ItemClickEventArgs itemClickEventArgs = e as HeaderCtrl.ItemClickEventArgs;
                m_Artist.ClearItems();
                if (hc.Mode == HeaderCtrl.Modes.Styles)
                {
                    foreach (DirectoryInfo tempDirectoryInfo in
                    new DirectoryInfo(Path.Combine(Properties.Settings.Default.RootPath, itemClickEventArgs.Item)).GetDirectories())
                    {
                        m_Artist.AddItem(tempDirectoryInfo.FullName);
                    }
                }
                else if (hc.Mode == HeaderCtrl.Modes.AYa || hc.Mode == HeaderCtrl.Modes.AZ ||
                    hc.Mode == HeaderCtrl.Modes.ZeroNine)
                {
                    List<DirectoryInfo> diList = new List<DirectoryInfo>();
                    new List<DirectoryInfo>(new DirectoryInfo(Properties.Settings.Default.RootPath).GetDirectories()).ForEach(
                        delegate(DirectoryInfo di)
                        {
                            diList.AddRange(di.GetDirectories(itemClickEventArgs.Item + "*"));
                        });
                    foreach (DirectoryInfo tempDirInfo in
                        FiltrateDirs(diList.ToArray(), true, itemClickEventArgs.Item))
                    {
                        m_Artist.AddItem(tempDirInfo.FullName);
                    }
                }
            };
            headerCtrl1.SearchClick += delegate
            {
                using (Search.SearchForm search = new musicbox.Search.SearchForm(m_IMediaManager))
                {
                    DialogResult dlgResult = search.ShowDialog(this);
                    if (dlgResult == DialogResult.OK)
                    {
                        m_Artist.ClearItems();
                        m_SelectingMedia.ClearItems();
                        foreach (DirectoryInfo directoryInfo in search.Results.Directories)
                        {
                            m_Artist.AddItem(directoryInfo.FullName);
                        }
                        foreach(FileInfo fileInfo in search.Results.Files)
                        {
                            string fileExtension = Path.GetExtension(fileInfo.FullName);
                            _Media media;
                            if(m_IMediaManager.TryGetMedia(fileExtension,out media))
                            {
                                m_SelectingMedia.AddItem(new FileNameAndMedia(fileInfo.FullName, media));
                            }
                        }
                    }
                }
            };
            // new EventHandler(stylesCtrl1_SearchClick);

            m_Artist.ItemClick += delegate(Object sender, Artist.ArtistClickEventArgs e)
            {
                Artist ai = (Artist)sender;
                if (e.Item.Length > 0)
                {
                    m_SelectingMedia.ClearItems();

                    foreach (_Media media in m_IMediaManager.GetAllMedia())
                    {
                        foreach (FileInfo fileInfo in new DirectoryInfo(e.Item).GetFiles("*" + media.Extension))
                        {
                            m_SelectingMedia.AddItem(new FileNameAndMedia(fileInfo.FullName, media));
                        }
                    }
                }
            };
            m_SelectingMedia.ItemClick += new EventHandler<SelectingMedia.SelectingMediaEventArgs>(m_SelectingMedia_ItemClick);
            //{
               /* if (!m_SelectedMedia.Items.IsNameExist(e.Item.FileName))
                {
                    String[] lastOrdered = LastOrdered.Instance.GetItems();
                    if (!Array.Exists<String>(lastOrdered,
                        delegate(String value)
                        {
                            return value == e.Item.FileName;
                        }))
                    {
                        if (!Properties.Settings.Default.MoneylessModeEnabled)
                        {
                            if (m_SelectedMedia.RestSum >= e.Item.IMedia.Price)
                            {
                                m_SelectedMedia.Items.Add(new FileNameAndIMediaWithIsMoneyless(e.Item.IMedia, false));
                            }
                        }
                        else
                        {
                            m_SelectedMedia.Items.Add(new FileNameAndIMediaWithIsMoneyless(e.Item.IMedia, true));
                        }
                    }
                    else
                        ApplicationMessage.Show(this, m_MediaExistInPlaylist);

                }
                else
                    ApplicationMessage.Show(this, m_MediaYetOrder);*/
                /* }
                 else
                     ApplicationMessage.Show(this,m_strMoneyNotEnough);*/
                //-------------------------------------
            //};
            m_SelectedMedia.GoClicked +=new EventHandler(m_SelectedMedia_GoClicked);
        }

        void m_SelectingMedia_ItemClick(object sender, SelectForm.SelectingMedia.SelectingMediaEventArgs e)
        {
            SelectedMedia.AddResult result = m_SelectedMedia.AddMedia(e.Item);
            switch (result)
            {
                case SelectedMedia.AddResult.MediaExistInPlaylist:
                    {
                        ApplicationMessage.Show(this, m_MediaExistInPlaylist);
                        break;
                    }
                case SelectedMedia.AddResult.MoneyNotEnought:
                    {
                        ApplicationMessage.Show(this, m_strMoneyNotEnough);
                        break;
                    }
                case SelectedMedia.AddResult.OK:
                    {
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }
        }

        void m_SelectedMedia_GoClicked(object sender, EventArgs e)
        {
            //if (m_SelectedMedia.RestSum != 0)
            //{
                /*AppMessageYesNo appMessageYesNo = new AppMessageYesNo();
                appMessageYesNo.MessageText = String.Format(m_RestSumText, m_SelectedMedia.RestSum) ;
                DialogResult dialogResult = appMessageYesNo.ShowDialog(this);
                if ( dialogResult == DialogResult.Cancel)
                {
                    return;
                }*/
            //}
            //CBInfo.Instance.BillsCountNow.Exchange(m_SelectedMedia.RestSum);


            m_ResultMedia = m_SelectedMedia.GetItems();
            m_SelectingMedia.ClearItems();
            m_Artist.ClearItems();
            m_SelectedMedia.ClearItems();
            this.DialogResult = DialogResult.OK;
            Close();

            /*else
            {
                m_ResultMedia = m_SelectedMedia.Items.ToArray();
                m_SelectingMedia.Items.Clear();
                m_Artist.Items.Clear();
                m_SelectedMedia.Items.Clear();
                this.DialogResult = DialogResult.OK;
                Close();
            }*/
        }
        /*private List<IAsyncResult> m_asyncResultList = new List<IAsyncResult>();
        private delegate Boolean FooDel(MediaTypes mediaType,String fileName);
        private delegate void GoClickedDel();*/


        private DirectoryInfo[] FiltrateDirs(DirectoryInfo[] dia, Boolean isFirst,String compareStr)
        {
            List<DirectoryInfo> diList = new List<DirectoryInfo>();
            foreach (DirectoryInfo di in dia)
            {
                if (isFirst)
                {
                    if (di.Name.StartsWith(compareStr))
                    {
                        diList.Add(di);
                    }
                }
                else
                {
                    if (di.Name.Contains(compareStr))
                    {
                        diList.Add(di);
                    }
                }
            }
            return diList.ToArray();
        }
        internal FileNameAndMediaWithIsMoneyless[] Result
        {
            get
            {
                return m_ResultMedia;
            }
        }
    }
}