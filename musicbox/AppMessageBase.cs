using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace musicbox
{
    public partial class AppMessageBase : Form
    {
        private readonly Rectangle m_TextRectangle;
        private readonly StringFormat m_TextFormant;
        private string m_MessageText = string.Empty;

        public string MessageText
        {
            get { return m_MessageText; }
            set { m_MessageText = value; }
        }

        public AppMessageBase()
        {
            InitializeComponent();
            m_TextFormant = new StringFormat();
            m_TextFormant.Alignment = StringAlignment.Center;
            m_TextFormant.LineAlignment = StringAlignment.Center;

            m_TextRectangle = Rectangle.Inflate(ClientRectangle,-20, -20);
            Paint += new PaintEventHandler(pictureBox1_Paint);
        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;
            e.Graphics.DrawString(m_MessageText, this.Font, Brushes.Black, m_TextRectangle, m_TextFormant);
            /*TextRenderer.DrawText(grfx, m_MessageText, this.Font, m_TextRectangle, Color.Black,
                TextFormatFlags.HorizontalCenter| TextFormatFlags.VerticalCenter);*/
        }

    }
}