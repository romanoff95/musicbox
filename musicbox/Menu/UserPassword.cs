using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace musicbox.Menu
{
    public partial class UserPasswordForm : Form
    {
        private String m_Password = "";

        public String Password
        {
            get { return m_Password; }
            //set { m_StrUserPassword = value; }
        }
        public UserPasswordForm()
        {
            InitializeComponent();
            foreach (Button btn in new Button[] 
                {
                    btn1,btn2,btn3,btn4,btn5,btn6,btn7,btn8,
                    btn9,btn0
                })
            {
                btn.Click += delegate(Object sender, EventArgs e)
                {
                    tbPassword.Text += ((Button)sender).Text;
                };
            }
            btnClear.Click += delegate
            {
                tbPassword.Text = "";
            };
            btnOk.Click += delegate
            {
                m_Password = tbPassword.Text;
                //DialogResult = DialogResult.OK;
                //Close();
            };
        }
    }
}