using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace musicbox.Menu
{
    public partial class Menu : Form
    {
        public event EventHandler VolumeChanged;

        private void InitVolumeTab()
        {
            //Init Volume Tab --------------------------
            foreach (_Media media in m_IMediaManager.GetAllMedia())
            {
                cbVolume.Items.Add(media);
            }
            cbVolume.SelectionChangeCommitted += new EventHandler(cbVolume_SelectionChangeCommitted);

            trackBarOrderVolume.ValueChanged += new EventHandler(trackBarOrderVolume_ValueChanged);
            trackBarDemoMode.ValueChanged += new EventHandler(trackBarDemoMode_ValueChanged);

            Debug.Assert(cbVolume.Items.Count > 0);
            cbVolume.SelectedIndex = 0;
            ApplyMediaVolumeToTackbar();
            // ---------------------------
        }
        //Volume
        private void VolumeChange(object sender, bool isDemoValue)
        {
            Debug.Assert(cbVolume.SelectedItem != null);
            _Media media = cbVolume.SelectedItem as _Media;
            Debug.Assert(media != null);
            Debug.Assert((sender as TrackBar) != null);
            if (isDemoValue)
                media.DemoVolume = ((TrackBar)sender).Value;
            else
                media.OrderVolume = ((TrackBar)sender).Value;
            if (VolumeChanged != null)
                VolumeChanged(this, EventArgs.Empty);
        }
        void trackBarDemoMode_ValueChanged(object sender, EventArgs e)
        {
            VolumeChange(sender, true);
        }

        void trackBarOrderVolume_ValueChanged(object sender, EventArgs e)
        {
            VolumeChange(sender, false);
        }

        //Accept 
        private void ApplyMediaVolumeToTackbar()
        {
            //Debug.Assert((sender as ComboBox) != null);
            _Media media = cbVolume.SelectedItem as _Media;
            Debug.Assert(media != null);
            trackBarOrderVolume.Value = media.OrderVolume;
            trackBarDemoMode.Value = media.DemoVolume;
        }
        void cbVolume_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ApplyMediaVolumeToTackbar();
        }
        //---------------------
    }
}
