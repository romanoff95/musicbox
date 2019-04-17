using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace musicbox.Sett
{
    class SettCtx : ApplicationContext
    {
        public SettCtx(Form mainForm) : base(mainForm)
        {
            SettForm settForm = new SettForm();
            if (settForm.ShowDialog() == DialogResult.Cancel)
            {
                this.MainForm.Close();
            }
        }
    }
}
