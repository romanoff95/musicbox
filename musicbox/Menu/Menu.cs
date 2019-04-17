using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace musicbox.Menu
{
    public partial class Menu : Form
    {
        /*private delegate void DataBindingDel(Control ctrl,String strCtrlProp,String strBindObjectProp,Object bindObject);
        private delegate void NumDel(NumericUpDown num, Boolean isUp);
        private delegate void DisableControlsOnTabPageDel(TabPage tp,Boolean bIsTrue);

        public event EventHandler mediaCancelPlay;
        public event EventHandler PlayQueueClearBtnClick;

        private delegate void SomeDel();
        private delegate void UpdateLabelTextDel(Label lbl,String text);*/
        private readonly IUserSettings m_IUserSettings;
        private readonly IMediaManager m_IMediaManager;
        private readonly IUserSettingsManager m_IUserSettingsManager;

        public event EventHandler PlayCancelButtonClick;
        public event EventHandler ClearQueueButtonClick;

        /*private struct SomeStruct
        {
            private CBInfo.CBItem m_CbItem;
            private EventHandler m_EventHandler;

            public EventHandler _EventHandler
            {
                get { return m_EventHandler; }
                set { m_EventHandler = value; }
            }

            public CBInfo.CBItem CbItem
            {
                get { return m_CbItem; }
                set { m_CbItem = value; }
            }

            public SomeStruct(CBInfo.CBItem cbItem, EventHandler del)
            {
                m_CbItem = cbItem;
                m_EventHandler = del;
            }
        }*/

        internal Menu(Main.MainWnd mainWnd, IUserSettings iUserSettings, IMediaManager iMediaManager,IUserSettingsManager iUserSettingsManager)
        {
            InitializeComponent();
            m_IUserSettings = iUserSettings;
            m_IMediaManager = iMediaManager;
            m_IUserSettingsManager = iUserSettingsManager;

            InitVolumeTab();
            InitPriceTab();
            InitUsersTab();
        }
        

        private void btnCancelPlayingNow_Click(object sender, EventArgs e)
        {
            if (PlayCancelButtonClick != null)
                PlayCancelButtonClick(this, EventArgs.Empty);
        }

        private void btnPlayQueueClear_Click(object sender, EventArgs e)
        {
            if (PlayCancelButtonClick != null)
                PlayCancelButtonClick(this, EventArgs.Empty);
        }

        private void btnDemoSelect_Click(object sender, EventArgs e)
        {
            Debug.Assert(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}