using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace musicbox
{
    public partial class AppMessageYesNo : musicbox.AppMessageBase
    {
        public AppMessageYesNo()
        {
            InitializeComponent();
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(button1.ClientRectangle);
            button1.Region = new Region(gp);

            GraphicsPath gp1 = new GraphicsPath();
            gp1.AddEllipse(button2.ClientRectangle);
            button2.Region = new Region(gp1);

            button1.Paint += new PaintEventHandler(button_Paint);
            button2.Paint +=new PaintEventHandler(button_Paint);


        }

        void button_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Brushes.Silver))
            {
                StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                pen.Alignment = PenAlignment.Center;
                pen.Width = 1;
                LinearGradientBrush linear = new LinearGradientBrush(button1.ClientRectangle, Color.DimGray, Color.White, 270);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(linear, button1.ClientRectangle);
                e.Graphics.DrawEllipse(pen, button1.ClientRectangle);
                e.Graphics.DrawString(((Button)sender).Name == "button1" ? "Играть" : "Дозаказать"
                    , ((Button)sender).Font, Brushes.Black, button1.ClientRectangle, sf);
            }
        }
    }
}

