using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace musicbox.Main
{
    public partial class MainWnd
    {
        class TopTen
        {
            private const Int32 nElementsYOffset = 20;
            private const Int32 nElementsXOffset = 20;
            private const Int32 nDistanceBetweenElems = 10;
            private const Int32 m_ItemsLimit = 10;
            private Font m_ItemsFont = new Font("Arial", 18, FontStyle.Regular);
            //private PictureBox m_pbTopTen;
            //private TopTenCollection m_Items;
            private String[] m_Items = new string[0];
            private StringFormat m_ItemsStringFormat;
            private MainWnd m_MainWnd;

            public event EventHandler Delay;
            private Int32 m_WaitDelaySec;
            private Timer m_Delay = new Timer();

            public TopTen(MainWnd mainWnd,Int32 delayInterval)
            {
                m_WaitDelaySec = delayInterval;
                //m_Items = new TopTenCollection(this);
                m_MainWnd = mainWnd;
                m_ItemsStringFormat = new StringFormat(StringFormatFlags.NoWrap);
                m_ItemsStringFormat.Alignment = StringAlignment.Near;
                m_ItemsStringFormat.LineAlignment = StringAlignment.Near;
                m_ItemsStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                mainWnd.pbTopTen.Paint += new PaintEventHandler(pbTopTen_Paint);
                mainWnd.pbTopTen.MouseDown += delegate
                {
                    m_Delay.Start();
                };
                mainWnd.pbTopTen.MouseUp += delegate
                {
                    if (m_Delay.Enabled)
                        m_Delay.Stop();
                };

                m_Delay.Interval = Convert.ToInt32(TimeSpan.FromSeconds(m_WaitDelaySec).TotalMilliseconds);
                m_Delay.Tick += delegate
                {
                    m_Delay.Stop();
                    if (Delay != null)
                        Delay(this, EventArgs.Empty);
                };
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void SetItems(String[] value)
            {
                m_Items = value;
                m_MainWnd.pbTopTen.Invalidate();
            }

            void pbTopTen_Paint(object sender, PaintEventArgs e)
            {
                Int32 min = Math.Min(m_ItemsLimit, m_Items.Length);
                for (Int32 i = 0; i < min; i++)
                {
                    TextRenderer.DrawText(e.Graphics,((i + 1).ToString() + ". " + m_Items[i]), m_ItemsFont,
                        new Rectangle(new Point(nElementsXOffset, (nDistanceBetweenElems + m_ItemsFont.Height) * i + nElementsYOffset),
                        new Size(m_MainWnd.pbTopTen.Width - 30, nDistanceBetweenElems + m_ItemsFont.Height)),Color.WhiteSmoke,
                           TextFormatFlags.WordEllipsis);
                }
            }

            public Int32 WaitDelaySec
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get { return m_WaitDelaySec; }
                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    m_WaitDelaySec = value;
                    m_Delay.Interval = Convert.ToInt32(TimeSpan.FromSeconds(m_WaitDelaySec).TotalMilliseconds);
                }
            }

            /*class TopTenCollection : CollectionBase
            {
                private TopTen m_TopTen;
                public TopTenCollection(TopTen topTen)
                {
                    m_TopTen = topTen;
                }
                public Int32 Add(String value)
                {
                    return List.Add(value);
                }
                public String this[Int32 index]
                {
                    get
                    {
                        return (String)List[index];
                    }
                    set
                    {
                        List[index] = value;
                    }
                }
            }*/

        }
    }
}
