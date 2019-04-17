using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace musicbox.Main
{
    public partial class MainWnd
    {
        class PriceList
        {
            private readonly MainWnd m_Parent;
            private const Int32 m_x = 150;
            private const Int32 m_y = 20;
            private const Int32 m_xDistanceBetweenElements = 10;
            private const Int32 m_yDistanceBetweenElements = 10;
            private const Int32 m_ElementHeight = 50;
            private const Int32 m_ElementWidth = 50;

            private readonly String m_RubText = "руб.";

            private readonly String m_HelpText = "Выберите стиль музыки(кнопки вверху) " +
                            "можно воспользоваться кнопкой ПОИСК " +
                            "потом выберите исполнителя затем " +
                            "уж можно и композицию выбрать";

            private Boolean m_Flag = false;


            private readonly Font m_Font = new Font("Arial", 18, FontStyle.Regular);

            public PriceList(MainWnd mainWnd)
            {
                m_Parent = mainWnd;
                m_Parent.pbPriceList.Paint += new System.Windows.Forms.PaintEventHandler(pbPriceList_Paint);
                m_Parent.pbPriceList.MouseDown += new MouseEventHandler(pbPriceList_MouseDown);

            }

            void pbPriceList_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Location.X < 120 && e.Location.Y < 120)
                {
                    Timer timer = new Timer();
                    timer.Interval = (Int32)TimeSpan.FromSeconds(25).TotalMilliseconds;
                    timer.Tick += new EventHandler(timer_Tick);
                    m_Flag = true;
                    m_Parent.pbPriceList.Invalidate();
                    timer.Start();
                }
            }

            void timer_Tick(object sender, EventArgs e)
            {
                m_Flag = false;
                m_Parent.pbPriceList.Invalidate();
                Timer tm = sender as Timer;
                tm.Stop();

            }
            void foofunc(Graphics g)
            {
                Int32 nextY = m_y;

                SizeF rubSize = g.MeasureString(m_RubText, m_Font);

                foreach (_Media media in MediaManager.Instance.GetAllMedia())
                {
                    Image targetImage = media.Img;
                    Int32 nextX = m_x;
                    g.DrawImageUnscaled(targetImage, m_x, nextY, targetImage.Width, m_ElementHeight);

                    nextX += targetImage.Width + m_xDistanceBetweenElements;

                    StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
                    //stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    SizeF textSize = g.MeasureString(media.Description, m_Font);
                    g.DrawString(media.Description, m_Font, Brushes.WhiteSmoke, new RectangleF(nextX,
                        nextY, textSize.Width, targetImage.Height)
                        , stringFormat);

                    nextX += (Int32)textSize.Width + m_xDistanceBetweenElements;

                    textSize = g.MeasureString(media.Price.ToString(), m_Font);
                    g.DrawString(media.Price.ToString(), m_Font, Brushes.WhiteSmoke,
                        new RectangleF(nextX, nextY, textSize.Width, targetImage.Height), stringFormat);

                    nextX += (Int32)textSize.Width + m_xDistanceBetweenElements;

                    g.DrawString(m_RubText, m_Font, Brushes.WhiteSmoke,
                        new RectangleF(nextX, nextY, rubSize.Width, targetImage.Height), stringFormat);
                    nextY += m_ElementHeight;
                }
            }
            void pbPriceList_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
            {
                if (!m_Flag)
                    foofunc(e.Graphics);
                else
                {
                    //StringFormat stringFormat = new StringFormat(StringFormatFlags.);
                    e.Graphics.DrawString(m_HelpText, m_Font, Brushes.WhiteSmoke, new Rectangle
                        (new Point(m_x, m_y + 20), new Size(500, 500)));
                }
                
            }
        }
    }
}
