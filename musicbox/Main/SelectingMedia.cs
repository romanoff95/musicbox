using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace musicbox.Main
{
    public partial class SelectForm
    {
        internal class SelectingMedia
        {
            private Image imgScrollTopPress = Properties.Resources.Page2_13_Press;

            private Image imgScrollBottomPress = Properties.Resources.Page2_30_Press;

            private FileCtrlItemCollection m_Items;

            private Font m_ItemsFont = new Font("Arial", 15, FontStyle.Regular);
            private Brush m_ItemsBrush = Brushes.WhiteSmoke;
            private Int32 m_nScrollOffset = 0;
            private Int32 m_nItemsWnd = 5;

            private const Int32 m_nScrollInterval_mSec = 60;
            private const Int32 m_nScrollStep = 1;
            private readonly StringFormat m_ItemsStringFormat;

            public event EventHandler<SelectingMediaEventArgs > ItemClick;

            public class SelectingMediaEventArgs : EventArgs
            {
                private FileNameAndMedia m_Item;

                public FileNameAndMedia Item
                {
                    get { return m_Item; }
                    set { m_Item = value; }
                }
                public SelectingMediaEventArgs(FileNameAndMedia item)
                {
                    this.m_Item = item;
                }
            }

            private SelectForm m_Parent;

            public SelectingMedia(SelectForm parent)
            {
                m_Parent = parent;

                m_ItemsStringFormat = new StringFormat(StringFormatFlags.NoWrap);
                m_ItemsStringFormat.LineAlignment = StringAlignment.Center;
                m_ItemsStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                new ScrollWrapper(m_Parent.pbSelectingMediaTopScroll, m_Parent.pbSelectingMediaBottomScroll,
                    imgScrollTopPress,
                    imgScrollBottomPress, m_nScrollStep, m_nScrollInterval_mSec, new ScrollDel(
                    delegate(Int32 nPos)
                    {
                        if (m_nScrollOffset + nPos >= 0 && m_nScrollOffset + m_nItemsWnd + nPos <= m_Items.Count)
                        {
                            m_nScrollOffset += nPos;
                            m_Parent.pbSelectingMediaItems.Invalidate();
                            m_Parent.pbSelectingMediaScrollBar.Invalidate();
                        }
                    }));

                m_Parent.pbSelectingMediaItems.Paint += new System.Windows.Forms.PaintEventHandler(pbSelectingMediaItems_Paint);

                m_Parent.pbSelectingMediaItems.MouseDown += delegate(Object sender, MouseEventArgs e)
                {
                    Int32 nMin = Math.Min(m_Items.Count, m_nItemsWnd);
                    for (Int32 i = 0; i < nMin; i++)
                    {
                        Rectangle rect = new Rectangle(0, m_Parent.pbSelectingMediaItems.Height / m_nItemsWnd * i,
                            m_Parent.pbSelectingMediaItems.Width, m_Parent.pbSelectingMediaItems.Height / m_nItemsWnd);
                        if (e.Location.X >= rect.Location.X &&
                            e.Location.Y >= rect.Location.Y &&
                            e.Location.X <= rect.Location.X + rect.Width &&
                            e.Location.Y <= rect.Location.Y + rect.Height)
                        {
                            if (ItemClick != null)
                                ItemClick(this, new SelectingMediaEventArgs(m_Items[i + m_nScrollOffset]));
                            break;
                        }
                    }
                };
                
                m_Parent.pbSelectingMediaScrollBar.Paint += delegate(Object sender, PaintEventArgs pe)
                {
                    PictureBox pbTemp = (PictureBox)sender;
                    Int32 nScrollerHeight = 20;
                    Single nYScrollOffset = 0;
                    if (m_Items != null)
                    {
                        if (m_Items.Count > m_nItemsWnd)
                        {
                            Int32 nScrollArea = pbTemp.Height - nScrollerHeight;
                            Int32 nRestElements = m_Items.Count - m_nItemsWnd;
                            Single nScrollStep = (Single)nScrollArea / (Single)nRestElements;
                            nYScrollOffset = nScrollStep * m_nScrollOffset;
                        }
                    }

                    pe.Graphics.FillRectangle(Brushes.Red, new RectangleF(0,
                        nYScrollOffset, pbTemp.Width, nScrollerHeight));
                };
                m_Items = new FileCtrlItemCollection(this);
            }
            public void AddItem(FileNameAndMedia item)
            {
                m_Items.Add(item);
            }
            public void ClearItems()
            {
                m_Items.Clear();
            }
            void pbSelectingMediaItems_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
            {
                Control senderCtrl = (Control)sender;
                Int32 nMin = Math.Min(m_nItemsWnd, m_Items.Count);
                Int32 nYOffset = 0;
                for (Int32 i = 0; i < nMin; i++)
                {
                    Bitmap transparent = m_Items[i + m_nScrollOffset].Media.Img;
                    //transparent.MakeTransparent(Color.Black);
                    e.Graphics.DrawImageUnscaledAndClipped
                            (transparent, new Rectangle(new Point(0, m_Parent.pbSelectingMediaItems.Height / m_nItemsWnd * i), transparent.Size));
                    e.Graphics.DrawString(Path.GetFileNameWithoutExtension(m_Items[i + m_nScrollOffset].FileName),
                            m_ItemsFont, m_ItemsBrush, new Rectangle(new Point(transparent.Width, m_Parent.pbSelectingMediaItems.Height / m_nItemsWnd * i),
                            new Size(senderCtrl.Width - transparent.Width, transparent.Height)), m_ItemsStringFormat);
                    nYOffset += transparent.Height;
                }
            }
            private void _Invalidate()
            {
                m_Parent.pbSelectingMediaItems.Invalidate();
                m_Parent.pbSelectingMediaScrollBar.Invalidate();
            }
            public class FileCtrlItemCollection : CollectionBase 
            {
                private SelectingMedia m_Parent;
                public FileCtrlItemCollection(SelectingMedia selectingMedia)
                {
                    m_Parent = selectingMedia;
                }
                public FileNameAndMedia this[int index]
                {
                    get
                    {
                        return ((FileNameAndMedia)List[index]);
                    }
                    set
                    {
                        List[index] = value;
                    }
                }
                public int Add(FileNameAndMedia value)
                {
                    return List.Add(value);
                }
                protected override void OnClear()
                {
                    m_Parent.m_nScrollOffset = 0;
                    m_Parent._Invalidate();
                }
                protected override void OnInsert(int index, object value)
                {
                    m_Parent._Invalidate();
                }
                protected override void OnRemove(int index, object value)
                {
                    m_Parent._Invalidate();
                }
                protected override void OnSet(int index, object oldValue, object newValue)
                {
                    m_Parent._Invalidate();
                }
            }
        }
    }
}
