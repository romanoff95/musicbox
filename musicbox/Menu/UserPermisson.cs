using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace musicbox.Menu
{
    public partial class UserPermissonForm : Form
    {
        //private readonly UserSettings.MenuTabsAccessPermissions m_Permssions;

        private void _binding(Control bindCtrl,object srcObj,string dataMember)
        {
            bindCtrl.DataBindings.Add("Checked", srcObj,dataMember );
            bindCtrl.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            bindCtrl.DataBindings[0].ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
        }

        public UserPermissonForm(UserSettings us)
        {
            InitializeComponent();
            _binding(checkBoxPermViewCounters, us._MenuTabsAccessPermissions, "CountersTab");
            _binding(checkBoxPermMoneylessModeUse, us._MenuTabsAccessPermissions, "MoneylessModeTab");
            _binding(checkBoxPermPriceChange, us._MenuTabsAccessPermissions, "PriceTab");
            _binding(checkBoxPermVolumeChange, us._MenuTabsAccessPermissions, "VolumeTab");
            _binding(checkBoxPermTimeChange, us._MenuTabsAccessPermissions, "TimersTab");
        }
    }
}