using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace musicbox.Main
{
    public partial class MainWnd : Form
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr SetUnhandledExceptionFilter(IntPtr lpFilter);

        //private readonly PlayManager m_Player;
        private SongInfoSender m_SongInfoSender;

        private static VideoWnd s_VideoWnd;

        private Object SelectWndLocker = new object();
        private TopTen m_TopTen;
        private PlayList m_PlayList;
        private PriceList m_PriceList;
        private delegate void SelectFormInvokeDel();
        private readonly bool m_UseBillValidator = Properties.Settings.Default.UseBillValidator;
        private readonly bool m_SendDataToInet = Properties.Settings.Default.SendDataToInternet;

        private bool m_SelectFormNowOpened = false;

        public static VideoWnd VideoWnd
        {
            get
            {
                return MainWnd.s_VideoWnd;
            }
        }
        public MainWnd()
        {

            //IntPtr SaveFilter = SetUnhandledExceptionFilter(IntPtr.Zero);
            InitializeComponent();
            //SetUnhandledExceptionFilter(SaveFilter);

            //m_Player = new PlayManager();

            m_PlayList = new PlayList(this);
            m_PriceList = new PriceList(this);
            m_TopTen = new TopTen(this,Properties.Settings.Default.MenuShowDelaySec);
            s_VideoWnd = new VideoWnd();


            if (m_UseBillValidator)
            {
                BillValidatorManager.Instance.Start();
            }
            

            //===================================
#if FULL
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.AutoScroll = false;
            
#endif
#if !DEBUG
            Cursor.Hide();
#endif
            //======================

            this.Load += delegate
            {
#if WW
                s_VideoWnd.Show();
#endif
            };
            this.Shown += new EventHandler(MainWnd_Shown);
            this.FormClosing += new FormClosingEventHandler(MainWnd_FormClosing);
            
            axShockwaveFlash1.LoadMovie(0, Path.Combine(Directory.GetCurrentDirectory(), "top.swf"));

            /*MenuSettings.Instance.MenuInitDelayChanged += delegate
            {
                m_TopTen.WaitDelaySec = MenuSettings.Instance.MenuInitDelay;
            };*/
            //m_TopTen.SetItems(songInfoManager.GetTopTen());
            m_TopTen.Delay += new EventHandler(m_TopTen_Delay);
            pbOrder.Click += new EventHandler(pbOrder_Click);

        }

        void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            BillValidatorManager.Instance.BillIn -= new EventHandler<BillValidatorManager.BillInEventArgs>(m_BillValidatorManager_BillIn);
            BillValidatorManager.Instance.Incas -= new EventHandler(m_BillValidatorManager_Incas);
            PlayManager.Instance.Stop();
            BillValidatorManager.Instance.Stop();   //????
            /*if (m_SendDataToInet)
            {
                Debug.Assert(m_SongInfoSender != null);
                m_SongInfoSender.Dispose();
            }*/
        }
        void MainWnd_Shown(object sender, EventArgs e)
        {
            PlayManager.Instance.Start();
            BillValidatorManager.Instance.BillIn += new EventHandler<BillValidatorManager.BillInEventArgs>(m_BillValidatorManager_BillIn);
            BillValidatorManager.Instance.Incas += new EventHandler(m_BillValidatorManager_Incas);

            /*if (m_SendDataToInet)
            {
                m_SongInfoSender = SongInfoSender.Create();
            }*/
        }
        void m_BillValidatorManager_Incas(object sender, EventArgs e)
        {
            /*sendInfoManager.IncasInfoSendManager.Add(new IncasInfoSendManager.IncasInfo[] { new
                        IncasInfoSendManager.IncasInfo(
                        CBInfo.Instance.SumCommon.Value.ToString(),
                        CBInfo.Instance.Sum.Value.ToString(), CBInfo.Instance.BillsCount.Value.ToString(),
                        DateTime.Now) });*/
            CBInfo.Instance.BillsCount.Exchange(0);
            CBInfo.Instance.Sum.Exchange(0);
            CBInfo.Instance.Serialize();
        }
        void m_TopTen_Delay(object sender, EventArgs e)
        {
            using (Menu.MenuPass mp = new musicbox.Menu.MenuPass(UserSettingsManager.Instance))
            {
                if (mp.ShowDialog(this) == DialogResult.OK)
                {
                    if (mp.UserChecked)
                    {
                        using (musicbox.Menu.Menu menu = new musicbox.Menu.Menu(this, mp.SelectedSettings, MediaManager.Instance,
                            UserSettingsManager.Instance))
                        {
                            menu.VolumeChanged += new EventHandler(menu_VolumeChanged);
                            /*menu.mediaCancelPlay += delegate
                            {
                                PlayManager.Instance.CancelPlayed();
                            };
                            menu.PlayQueueClearBtnClick += delegate
                            {
                                PlayManager.Instance.ClearPlaylist();
                            };*/
                            menu.ShowDialog(this);
                        }
                    }
                }
            }
        }

        void menu_VolumeChanged(object sender, EventArgs e)
        {
            PlayManager.Instance.UpdateVolume();
        }
        void m_BillValidatorManager_BillIn(object sender, BillValidatorManager.BillInEventArgs e)
        {
            CBInfo.Instance.BillsCount.Increment();
            CBInfo.Instance.Sum.Add((Int32)e.Bill);

            //CBInfo.Instance.BillsCountNow.Increment();
            CBInfo.Instance.SumNow.Add((Int32)e.Bill);

            CBInfo.Instance.BillsCountCommon.Increment();
            CBInfo.Instance.SumCommon.Add((Int32)e.Bill);

            CBInfo.Instance.Serialize();

            Debug.Assert(InvokeRequired);
            BeginInvoke(new SelectFormInvokeDel(SelectDialogInvoke));
        }

        void pbOrder_Click(object sender, EventArgs e)
        {
            Debug.Assert(!InvokeRequired);
            SelectDialogInvoke();
        }
        private void SelectDialogInvoke()
        {
            if (m_SelectFormNowOpened)
            {
                return;
            }
            using (SelectForm selectForm = new SelectForm(MediaManager.Instance))
            {
                if (selectForm.ShowDialog(this) == DialogResult.OK)
                {
                    foo(selectForm.Result);
                }
            }
            Debug.Assert(m_SelectFormNowOpened == true);
        }

        private void foo(FileNameAndMediaWithIsMoneyless[] fileNameAndIMediaWithIsMoneylessArr)
        {
            /*foreach (FileNameAndMediaWithIsMoneyless _f in fileNameAndIMediaWithIsMoneyless)
            {
                m_Player.Add(_f.FileNameAndMedia);
            }*/
            foreach (FileNameAndMediaWithIsMoneyless fileNameAndMediaWithMoneyless in fileNameAndIMediaWithIsMoneylessArr)
            {
                PlayManager.Instance.Add(new FileNameAndDemo(fileNameAndMediaWithMoneyless.FileNameAndMedia.FileName,false));
            }
            //m_SongInfoSender.AddIMedia(fileNameAndIMediaWithIsMoneyless);
        }
    }
}