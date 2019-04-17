using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace musicbox.Menu
{
    public partial class Menu : Form
    {
        private void InitUsersTab()
        {
            foreach (UserSettings userSettings in m_IUserSettingsManager.GetAllItems())
            {
                cbUser.Items.Add(userSettings);
            }
            Debug.Assert(cbUser.Items.Count > 0);
            cbUser.SelectedIndex = 0;

            cbUser.SelectionChangeCommitted += new EventHandler(cbUser_SelectionChangeCommitted);
            btnSetPassword.Click += new EventHandler(btnSetPassword_Click);
            btnSetPermissions.Click += new EventHandler(btnSetPermissions_Click);
            //AdjustPermissionButton();
        }
        private void AdjustPermissionButton()
        {
            UserSettings userSettings = cbUser.SelectedItem as UserSettings;
            Debug.Assert(userSettings != null);
            btnSetPermissions.Enabled = userSettings.IsPermissionsUsed;
        }
        void cbUser_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AdjustPermissionButton();
        }

        void btnSetPermissions_Click(object sender, EventArgs e)
        {
            UserSettings userSettings = cbUser.SelectedItem as UserSettings;
            Debug.Assert(userSettings != null);
            UserPermissonForm userPermissionForm = new UserPermissonForm(userSettings);
            userPermissionForm.ShowDialog(this);
        }

        void btnSetPassword_Click(object sender, EventArgs e)
        {
            UserPasswordForm userPasswordForm = new UserPasswordForm();
            if (userPasswordForm.ShowDialog(this) == DialogResult.OK)
            {
                UserSettings userSettings = cbUser.SelectedItem as UserSettings;
                Debug.Assert(userSettings != null);
                userSettings.UserPassword = userPasswordForm.Password;
            }
        }
    }
}
