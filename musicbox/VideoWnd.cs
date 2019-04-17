using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace musicbox
{
    public partial class VideoWnd : Form
    {
        public VideoWnd()
        {
            InitializeComponent();
        }

        private void VideoWnd_Load(object sender, EventArgs e)
        {
            Rectangle tempRect =
                Screen.AllScreens[Screen.AllScreens.Length > 1 ? 1 : 0].Bounds;
            DesktopLocation = tempRect.Location;
            Size = tempRect.Size;
        }
    }
}