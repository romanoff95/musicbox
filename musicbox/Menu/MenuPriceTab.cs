using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace musicbox.Menu
{
    public partial class Menu : Form
    {

        public EventHandler PriceChanged;

        private void InitPriceTab()
        {
            foreach (_Media media in m_IMediaManager.GetAllMedia())
            {
                cbPrice.Items.Add(media);
            }
            cbPrice.SelectionChangeCommitted += new EventHandler(cbPrice_SelectionChangeCommitted);

            numericUpDownPrice.ValueChanged += new EventHandler(numericUpDownPrice_ValueChanged);

            Debug.Assert(cbPrice.Items.Count > 0);
            cbPrice.SelectedIndex = 0;
            ApplyMediaPriceToNumericCtrl();

            btnDecPrice.Click += new EventHandler(btnDecPrice_Click);
            btnIncPrice.Click += new EventHandler(btnIncPrice_Click);
        }

        void btnIncPrice_Click(object sender, EventArgs e)
        {
            numericUpDownPrice.UpButton();
        }

        void btnDecPrice_Click(object sender, EventArgs e)
        {
            numericUpDownPrice.DownButton();
        }

        void numericUpDownPrice_ValueChanged(object sender, EventArgs e)
        {
            _Media media = cbPrice.SelectedItem as _Media;
            Debug.Assert(media != null);
            media.Price = Convert.ToInt32(numericUpDownPrice.Value);
            if (PriceChanged != null)
                PriceChanged(this, EventArgs.Empty);
        }

        private void ApplyMediaPriceToNumericCtrl()
        {
            _Media media = cbPrice.SelectedItem as _Media;
            Debug.Assert(media != null);
            numericUpDownPrice.Value = media.Price;
        }
        void cbPrice_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ApplyMediaPriceToNumericCtrl();
        }
    }
}
