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
    public partial class MenuPass : Form
    {
        private UserSettings m_SelectedSettings;
        private readonly string m_ErrorMsg = "Îøèáêà";
        //private string m_Password;

        /*public string Password
        {
            get { return m_Password; }
        }*/

        public UserSettings SelectedSettings
        {
            get { return m_SelectedSettings; }
        }
        private bool m_UserChecked = false;

        public bool UserChecked
        {
            get { return m_UserChecked; }
        }

        public MenuPass(IUserSettingsManager iUserSettingsManager)
        {
            InitializeComponent();
            Debug.Assert(iUserSettingsManager != null);
            foreach (UserSettings usTemp in iUserSettingsManager.GetAllItems())
            {
                comboBox1.Items.Add(usTemp);
            }
            Debug.Assert(comboBox1.Items.Count > 0);
            comboBox1.SelectedIndex = 0;
            foreach (Button btnTemp in new Button[]
                {
                    btn1,btn2,btn3,btn4,btn5,btn6,btn7,btn8,btn9,btn0
                })
            {
                btnTemp.Click += delegate(Object sender,EventArgs e)
                {
                    tbPassword.Text += ((Button)sender).Text;
                };
            }
            btnClear.Click += new EventHandler(btnClear_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            tbPassword.Clear();
        }


        void btnOK_Click(object sender, EventArgs e)
        {
            Debug.Assert((comboBox1.SelectedItem as UserSettings) != null);
            m_SelectedSettings = comboBox1.SelectedItem as UserSettings;
            Debug.Assert(m_SelectedSettings != null);
            if (m_SelectedSettings.UserPassword.Equals(tbPassword.Text,StringComparison.OrdinalIgnoreCase))
            {
                m_UserChecked = true;
            }
            else
            {
                MessageBox.Show(m_ErrorMsg);
            }
        }
    }
}