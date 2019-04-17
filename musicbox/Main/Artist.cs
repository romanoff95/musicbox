using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace musicbox.Main
{
    public partial class SelectForm
    {
        class Artist
        {
            private ArtistItemsCollection m_Items;

            private Int32 m_nScrollOffset = 0;
            public event EventHandler<ArtistClickEventArgs> ItemClick;
            private const Int32 m_nScrollStep = 1;
            private const Int32 m_nScrollIntervalmSec = 60;

            private readonly Image imgTopScrollPress = Properties.Resources.Page2_11_Press;
            private readonly Image imgBottomScrollPress = Properties.Resources.Page2_29_Press;

            private PictureBox[] m_Titles;
            private Font m_ItemsFont = new Font("Arial", 18, FontStyle.Regular);
            private Brush m_ItemsBrush = Brushes.WhiteSmoke;
            private StringFormat m_ItemsStringFormat;

            public class ArtistClickEventArgs : EventArgs
            {
                private string m_Item;

                public string Item
                {
                    get { return m_Item; }
                    set { m_Item = value; }
                }
                public ArtistClickEventArgs(string item)
                {
                    this.m_Item = item;
                }
            }
            private SelectForm m_Parent;
            public Artist(SelectForm parent)
            {
                m_Parent = parent;
                m_Items = new ArtistItemsCollection(this);
                m_Titles = new PictureBox[] { m_Parent.pbArtistItem1, m_Parent.pbArtistItem2, 
                    m_Parent.pbArtistItem3, m_Parent.pbArtistItem4, m_Parent.pbArtistItem5 };

                musicbox.ScrollDel scrollDel = delegate(Int32 nPos)
                {
                    if (m_Items.Count > m_Titles.Length)
                    {
                        if (m_nScrollOffset + nPos >= 0 && m_nScrollOffset + nPos <= m_Items.Count - m_Titles.Length)
                        {
                            m_nScrollOffset += nPos;
                            DrawItems();
                            m_Parent.pbArtistScrollBar.Invalidate();
                        }
                    }
                };

                new ScrollWrapper(m_Parent.pbArtistTopScroll, m_Parent.pbArtistBottomScroll,
                    imgTopScrollPress, imgBottomScrollPress,
                    m_nScrollStep, m_nScrollIntervalmSec, new musicbox.ScrollDel(scrollDel));/*.OneScrollable += delegate(Object sender, EventArgs e)
                {
                    //MessageBox.Show("");
                };*/

                m_ItemsStringFormat = new StringFormat(StringFormatFlags.NoWrap);
                m_ItemsStringFormat.Alignment = StringAlignment.Center;
                m_ItemsStringFormat.LineAlignment = StringAlignment.Center;
                m_ItemsStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                foreach (PictureBox tempPb in m_Titles)
                {
                    tempPb.Paint += new PaintEventHandler(tempPb_Paint);
                    tempPb.Click += new EventHandler(tempPb_Click);
                }
                m_Parent.pbArtistScrollBar.Paint += delegate(Object sender, PaintEventArgs pe)
                {
                    PictureBox pbTemp = (PictureBox)sender;
                    Int32 nScrollerHeight = 20;
                    Single nYScrollOffset = 0;
                    if (m_Items.Count > m_Titles.Length)
                    {
                        Int32 nScrollArea = pbTemp.Height - nScrollerHeight;
                        Int32 nRestElements = m_Items.Count - m_Titles.Length;
                        Single nScrollStep = (Single)nScrollArea / (Single)nRestElements;
                        nYScrollOffset = nScrollStep * m_nScrollOffset;
                    }

                    pe.Graphics.FillRectangle(Brushes.Red, new RectangleF(0,
                        nYScrollOffset, pbTemp.Width, nScrollerHeight));

                };
            }
            public void AddItem(string directoryName)
            {
                m_Items.Add(directoryName);
            }
            public void ClearItems()
            {
                m_Items.Clear();
            }
            void tempPb_Click(object sender, EventArgs e)
            {
                PictureBox tempPb = (PictureBox)sender;
                if (tempPb.Tag != null)
                {
                    if (ItemClick != null)
                    {
                        Debug.Assert(tempPb.Tag != null);
                        ItemClick(this, new ArtistClickEventArgs((string)tempPb.Tag));
                    }
                }

            }

            void tempPb_Paint(object sender, PaintEventArgs e)
            {
                PictureBox tempPb = (PictureBox)sender;
                if (tempPb.Tag != null)
                {
                    e.Graphics.DrawString(Path.GetFileNameWithoutExtension((String)tempPb.Tag),
                        m_ItemsFont, m_ItemsBrush, new Rectangle(new Point(0, 0),
                        tempPb.Size), m_ItemsStringFormat);
                }
            }

            private void DrawItems()
            {
                Int32 nMin = Math.Min(m_Titles.Length, m_Items.Count);
                for (Int32 i = 0; i < nMin; i++)
                {
                    m_Titles[i].Tag = m_Items[i + m_nScrollOffset];
                }
                for (int i = nMin; i < m_Titles.Length; i++)
                {
                    m_Titles[i].Tag = null;
                }
                foreach (PictureBox temppb in m_Titles)
                {
                    temppb.Invalidate();
                }
                m_Parent.pbArtistScrollBar.Invalidate();
            }
            class ArtistItemsCollection : CollectionBase
            {
                private Artist m_Parent;
                public ArtistItemsCollection(Artist artist)
                {
                    m_Parent = artist;
                }
                public String this[int index]
                {
                    set
                    {
                        List[index] = value;
                    }
                    get
                    {
                        return (string)List[index];
                    }
                }
                public int Add(string value)
                {
                    return List.Add(value);
                }
                /*public void AddRange(T[] value)
                {
                    foreach (String tempval in value)
                    {
                        List.Add(tempval);
                    }
                }*/
                protected override void OnClear()
                {
                    m_Parent.m_nScrollOffset = 0;
                }
                protected override void OnInsertComplete(int index, object value)
                {
                    m_Parent.DrawItems();
                }
                protected override void OnClearComplete()
                {
                    m_Parent.DrawItems();
                }
            }
        }
    }
}
