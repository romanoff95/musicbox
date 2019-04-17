using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace musicbox.Main
{
    public partial class MainWnd
    {
        class PlayList
        {
            private readonly Image imgSTopPress = Properties.Resources.Page1_3_Press;
            private readonly Image imgSBottomPress = Properties.Resources.Page1_8_Press;

            private Int32 m_nOffset = 0;

            private readonly Font m_Font = new Font("Arial", 18, FontStyle.Regular);
            private const Int32 m_ScrollableItemsCount = 8;

            private const Int32 nHeaderItemXOffset = 15;
            private const Int32 nHeaderItemYOffset = 15;
            private const Int32 nItemXOffset = 15;
            private const Int32 nItemYOffset = 90;
            private const Int32 nBetweenItemsDistance = 10;

            private readonly StringFormat m_ItemsStringFormat;

            private readonly Brush m_NormalItemBrush = Brushes.WhiteSmoke;
            private readonly Brush m_HeaderItemBrush = Brushes.Red;

            private MainWnd m_Parent;
            private readonly String m_DemoStringInTitle = "DEMO";

            private delegate void InvokeUIDel();

            public PlayList(MainWnd mainWnd)
            {
                m_Parent = mainWnd;

                m_ItemsStringFormat = new StringFormat(StringFormatFlags.NoWrap);
                m_ItemsStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                PlayManager.Instance.ItemsCountChanged += delegate
                {
                    //Index adjust
                    Int32 itemsWithoutTopLength = PlayManager.Instance.GetItemsWithoutTop().Length;
                    if (itemsWithoutTopLength < m_ScrollableItemsCount)
                    {
                        m_nOffset = 0;
                    }
                    else
                    {
                        if (m_nOffset + m_ScrollableItemsCount > itemsWithoutTopLength)
                        {
                            m_nOffset = itemsWithoutTopLength - m_ScrollableItemsCount;
                        }
                    }
                    m_Parent.pbPlayListItems.Invalidate();
                    m_Parent.pbPlayListScrollBar.Invalidate();
                    m_Parent.pbTitles.Invalidate();
                };

                PlayManager.Instance.PlayState += delegate(Object sender, Main.PlayManager.PlayStateEventArgs e)
                {
                    try
                    {
                        m_Parent.progressBarPlayList.BeginInvoke(new InvokeUIDel(
                            delegate()
                            {
                                m_Parent.progressBarPlayList.Value = e.State;
                            }));
                    }
                    catch (Exception em)
                    {
                        Debug.WriteLine("Progress Exception" + em.Message);
                    }
                };

                new ScrollWrapper(m_Parent.pbPlayListTopScrollBtn, m_Parent.pbPlayListBottomScrollBtn,
                    imgSTopPress,imgSBottomPress, 1, 60, new ScrollDel(
                    delegate(Int32 nPos)
                    {
                        if (PlayManager.Instance.GetItemsWithoutTop().Length > m_ScrollableItemsCount)
                        {
                            if (nPos + m_nOffset >= 0 && nPos + m_nOffset + m_ScrollableItemsCount <= PlayManager.Instance.GetItemsWithoutTop().Length)
                            {
                                m_nOffset += nPos;
                                m_Parent.pbPlayListItems.Invalidate();
                                m_Parent.pbPlayListScrollBar.Invalidate();
                            }
                        }
                    }));

                m_Parent.pbPlayListScrollBar.Paint += delegate(Object sender, PaintEventArgs pe)
                {
                    PictureBox pbTemp = (PictureBox)sender;
                    Int32 nScrollerHeight = 20;
                    Single nYScrollOffset = 0;
                    FileNameAndDemo[] itemsWithoutTop = PlayManager.Instance.GetItemsWithoutTop();
                    if (itemsWithoutTop.Length > m_ScrollableItemsCount)
                    {
                        Int32 nScrollArea = pbTemp.Height - nScrollerHeight;
                        Int32 nRestElements = (itemsWithoutTop.Length) - m_ScrollableItemsCount;
                        Single nScrollStep = (Single)nScrollArea / (Single)nRestElements;
                        nYScrollOffset = nScrollStep * m_nOffset;
                    }

                    pe.Graphics.FillRectangle(Brushes.Red, new RectangleF(pbTemp.Width / 2 - 4,
                        nYScrollOffset, 10, nScrollerHeight));
                };
                m_Parent.pbPlayListItems.Paint += new PaintEventHandler(pbItems_Paint);
                m_Parent.pbTitles.Paint += new PaintEventHandler(pbTitles_Paint);
            }

            void pbTitles_Paint(object sender, PaintEventArgs e)
            {
                FileNameAndDemo fileNameAndMediaAndDemo = PlayManager.Instance.GetTopItem();
                if (fileNameAndMediaAndDemo != null)
                {
                    if (fileNameAndMediaAndDemo.Demo)
                    {
                        Graphics g = e.Graphics;
                        using (Font fnt = new Font("Arial", 24, FontStyle.Regular))
                        {
                            SizeF textSize = g.MeasureString(m_DemoStringInTitle, fnt);
                            g.DrawString(m_DemoStringInTitle, fnt,
                                Brushes.Red, new RectangleF(500, 42, textSize.Width, textSize.Height));
                        }
                    }
                }
            }
            void pbItems_Paint(object sender, PaintEventArgs e)
            {
                Rectangle tempRect = new Rectangle(m_Parent.pbPlayListItems.Location, m_Parent.pbPlayListItems.Size);
                FileNameAndDemo[] fileNameAndDemoArr = PlayManager.Instance.GetItems();
                Graphics g = e.Graphics;
                if (fileNameAndDemoArr.Length > 0)
                {
                    FileNameAndDemo topItem = fileNameAndDemoArr[0];
                    TextRenderer.DrawText(g,Path.GetFileNameWithoutExtension(topItem.FileName),
                        m_Font, new Rectangle(new Point(nHeaderItemXOffset, nHeaderItemYOffset),
                        new Size(tempRect.Width - 15, m_Font.Height)), Color.Red,TextFormatFlags.WordEllipsis);
                    //PlayMedia[] playerItems = Player.Instance.GetItemsWithoutTop();
                    Int32 nMin = Math.Min(m_ScrollableItemsCount, fileNameAndDemoArr.Length - 1);
                    for (Int32 i = 1; i < nMin + 1; i++)
                    {
                        //i = i - 1;
                        TextRenderer.DrawText(g, Path.GetFileNameWithoutExtension(fileNameAndDemoArr[i + m_nOffset].FileName),
                            m_Font,
                            new Rectangle(new Point(nItemXOffset, nItemYOffset + (i - 1) * (m_Font.Height + nBetweenItemsDistance)),
                            new Size(tempRect.Width - 15, m_Font.Height + nBetweenItemsDistance)),Color.WhiteSmoke, TextFormatFlags.WordEllipsis);
                    }
                }
            }
        }
    }
}
