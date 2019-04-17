using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace musicbox.Main
{
    public partial class SelectForm
    {
        class SelectedMedia
        {
            private readonly Image imgTopScrollPressed = Properties.Resources.Page2_35_Press;

            private readonly Image imgBottomScrollPressed = Properties.Resources.Page2_37_Press;

            private Font fnt = new Font("Verdana", 13, FontStyle.Regular);
            private Brush m_WhiteBrush = Brushes.WhiteSmoke;

            private Int32 nStartX = 30, nStartY = 70;
            private readonly MediaCollection m_Items;

            private readonly Int32 m_ItemsInWnd = 5;
            private Int32 m_ItemOffset = 0;
            private Int32 m_RestSum = 0;

            public event EventHandler GoClicked;

            private readonly StringFormat m_ItemsStringFormat;

            private readonly SelectForm m_Parent;
            private delegate void UpdateSumDel();

            private const int SUM_LOW_LIMIT = 0;

            private readonly string m_RestSumText = "У вас осталось : {0} рублей.";


            public SelectedMedia(SelectForm selectForm)
            {
                m_Parent = selectForm;

                m_ItemsStringFormat = new StringFormat(StringFormatFlags.NoWrap);
                m_ItemsStringFormat.Trimming = StringTrimming.EllipsisCharacter;
                m_Items = new MediaCollection(this);

                m_Parent.pbSelectedMediaItems.Paint += new PaintEventHandler(pbChoosedView_Paint);

                m_Parent.pbCancelBtn.Tag = Properties.Resources.Page2_33_Press;


                new ScrollWrapper(m_Parent.pbSelectedMediaTopScroll,
                    m_Parent.pbSelectedMediaBottomScroll, imgTopScrollPressed,
                    imgBottomScrollPressed, 1, 60, new ScrollDel(
                    delegate(Int32 nPos)
                    {
                        if (m_Items.Count > m_ItemsInWnd)
                        {
                            if (nPos + m_ItemOffset >= 0 && nPos + m_ItemOffset + m_ItemsInWnd <= m_Items.Count)
                            {
                                m_ItemOffset += nPos;
                                OnItemsStateChanged();
                            }
                        }
                    }));

                m_Parent.pbCancelBtn.Click += new EventHandler(pbCancelBtn_Click);
                m_Parent.pbGo.Click += new EventHandler(pbGo_Click);

                //Init
                m_RestSum = CBInfo.Instance.SumNow.Value;
                m_Parent.lblSum.Text = m_RestSum.ToString();

                m_Parent.Shown += new EventHandler(m_Parent_Shown);
                m_Parent.FormClosing += new FormClosingEventHandler(m_Parent_FormClosing);
            }

            void pbGo_Click(object sender, EventArgs e)
            {
                Debug.Assert(m_RestSum >= 0);
                if (m_RestSum != SUM_LOW_LIMIT)
                {
                    AppMessageYesNo appMessageYesNo = new AppMessageYesNo();
                    appMessageYesNo.MessageText = String.Format(m_RestSumText, m_RestSum);
                    DialogResult dialogResult = appMessageYesNo.ShowDialog(m_Parent);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                CBInfo.Instance.BillsCountNow.Exchange(0);
                CBInfo.Instance.SumNow.Exchange(m_RestSum);
                CBInfo.Instance.Serialize();

                if (GoClicked != null)
                    GoClicked(this, EventArgs.Empty);
            }

            void m_Parent_FormClosing(object sender, FormClosingEventArgs e)
            {
                CBInfo.Instance.SumNow.ValueChanged -= new EventHandler(SumNow_ValueChanged);
            }

            void m_Parent_Shown(object sender, EventArgs e)
            {
                CBInfo.Instance.SumNow.ValueChanged += new EventHandler(SumNow_ValueChanged);
            }

            void SumNow_ValueChanged(object sender, EventArgs e)
            {
                RecalculateSum();
            }
            void pbCancelBtn_Click(object sender, EventArgs e)
            {
                if (m_Items.Count > 0)
                {
                    m_Items.RemoveAt(m_Items.Count - 1);
                }
            }
            public enum AddResult
            {
                MediaExistInPlaylist,
                MoneyNotEnought,
                OK
            }
            public AddResult AddMedia(FileNameAndMedia item)
            {
                //AddResult result;
                foreach (FileNameAndMediaWithIsMoneyless tmp in m_Items)
                {
                    if (tmp.FileNameAndMedia.FileName.Equals(item.FileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return AddResult.MediaExistInPlaylist;
                    }
                }
                if (Properties.Settings.Default.MoneylessModeEnabled)
                {
                    m_Items.Add(new FileNameAndMediaWithIsMoneyless(item,true));
                    return AddResult.OK;
                }
                else
                {
                    if(m_RestSum - item.Media.Price >= SUM_LOW_LIMIT)
                    {
                        m_Items.Add(new FileNameAndMediaWithIsMoneyless(item,false));
                        RecalculateSum();
                        return AddResult.OK;
                    }
                    else
                        return AddResult.MoneyNotEnought;
                }
            }
            public FileNameAndMediaWithIsMoneyless[] GetItems()
            {
                List<FileNameAndMediaWithIsMoneyless> tmp = new List<FileNameAndMediaWithIsMoneyless>();
                foreach (FileNameAndMediaWithIsMoneyless tmp1 in tmp)
                {
                    tmp.Add(tmp1);
                }
                return tmp.ToArray();
            }
            public void ClearItems()
            {
                m_Items.Clear();
            }
            private void pbChoosedView_Paint(object sender, PaintEventArgs e)
            {
                Control senderControl = sender as Control;
                Int32 nMin = Math.Min(m_ItemsInWnd, m_Items.Count);
                for (Int32 i = 0; i < nMin; i++)
                {
                    if (m_Items.Count >= i + m_ItemOffset)
                    {
                        e.Graphics.DrawString(Path.GetFileNameWithoutExtension(m_Items[i + m_ItemOffset].FileNameAndMedia.FileName),
                            fnt, m_WhiteBrush,
                            new Rectangle(new Point(nStartX, nStartY + i * fnt.Height),
                            new Size(senderControl.Width - nStartX, fnt.Height)), m_ItemsStringFormat);
                    }
                }

            }
            private int GetMediaSum()
            {
                int restSum = 0;
                foreach (FileNameAndMediaWithIsMoneyless item in m_Items)
                {
                    if(!item.Moneyless)
                    {
                        restSum += item.FileNameAndMedia.Media.Price;
                    }
                }
                return restSum;
            }
            private void ImageExchager(Image from, Image to)
            {
                Image tmpImage = to;
                to = from;
                from = tmpImage;
            }
            private void pbCancelBtn_MouseDown(object sender, MouseEventArgs e)
            {
                Debug.Assert(m_Parent.pbCancelBtn.Tag != null);
                ImageExchager((Image)m_Parent.pbCancelBtn.Tag, m_Parent.pbCancelBtn.Image);
            }

            private void pbCancelBtn_MouseUp(object sender, MouseEventArgs e)
            {
                Debug.Assert(m_Parent.pbCancelBtn.Tag != null);
                ImageExchager((Image)m_Parent.pbCancelBtn.Tag, m_Parent.pbCancelBtn.Image);
            }
            /*public Int32 RestSum
            {
                get { return m_RestSum; }
            }*/
            private void RecalculateSum()
            {
                Debug.Assert(m_Parent.lblSum.IsHandleCreated);
                if (m_Parent.lblSum.InvokeRequired)
                {
                    m_Parent.lblSum.BeginInvoke(new UpdateSumDel(UpdateSum));
                }
                else
                {
                    UpdateSum();
                }
            }
            /*private void UpdateSum()
            {
                lock (m_SumLocker)
                {
                    m_RestSum = CBInfo.Instance.SumNow.Value - GetMediaSum();
                    Debug.Assert(!m_Parent.lblSum.InvokeRequired);
                    m_Parent.lblSum.Text = m_RestSum.ToString();
                    
                }
            }*/
            private void OnItemsStateChanged()
            {
                m_Parent.pbSelectedMediaItems.Invalidate();
            }
            private void UpdateSum()
            {
                m_RestSum = (CBInfo.Instance.SumNow.Value - GetMediaSum());
                Debug.Assert(m_Parent.lblSum.IsHandleCreated);
                Debug.Assert(!m_Parent.lblSum.InvokeRequired);
                m_Parent.lblSum.Text = m_RestSum.ToString();
            }
            //-----------------------------------------------
            class MediaCollection : CollectionBase 
            {
                private SelectedMedia m_Parent;
                public MediaCollection(SelectedMedia selectedMedia)
                {
                    m_Parent = selectedMedia;
                }
                public FileNameAndMediaWithIsMoneyless this[int index]
                {
                    get
                    {
                        return ((FileNameAndMediaWithIsMoneyless)List[index]);
                    }
                    set
                    {
                        List[index] = value;
                    }
                }

                public int Add(FileNameAndMediaWithIsMoneyless value)
                {
                    return (List.Add(value));
                }
                /*public T[] ToArray()
                {
                    T[] mediaArr = new T[List.Count];
                    List.CopyTo(mediaArr, 0);
                    return mediaArr;
                }
                public bool Contains(T value)
                {
                    return (List.Contains(value));
                }*/
                /*public bool IsNameExist(String fullPath)
                {
                    Boolean ret = false;
                    foreach (T media in List)
                    {
                        if (media.FileNameAndIMedia.FileName.Equals(fullPath,StringComparison.)
                        {
                            ret = true;
                            break;
                        }
                    }
                    return ret;
                }*/
                public void Remove(FileNameAndMediaWithIsMoneyless value)
                {
                    List.Remove(value);
                }
                protected override void OnClear()
                {
                    m_Parent.OnItemsStateChanged();
                }
                protected override void OnClearComplete()
                {
                    m_Parent.RecalculateSum();
                }
                protected override void OnInsertComplete(int index, object value)
                {
                    if (List.Count > m_Parent.m_ItemsInWnd - 1)
                        m_Parent.m_ItemOffset = List.Count - m_Parent.m_ItemsInWnd;
                    m_Parent.OnItemsStateChanged();
                    m_Parent.RecalculateSum();
                }
                protected override void OnRemoveComplete(int index, object value)
                {
                    if (List.Count >= m_Parent.m_ItemsInWnd)
                    {
                        if (m_Parent.m_ItemOffset + m_Parent.m_ItemsInWnd == List.Count + 1)
                        {
                            m_Parent.m_ItemOffset--;
                        }
                    }
                    m_Parent.OnItemsStateChanged();
                    m_Parent.RecalculateSum();

                    /*if (m_ChoosedCtrl.m_Items.Count > m_ChoosedCtrl.m_ItemsInWnd)
                    {
                        m_ChoosedCtrl.m_ItemOffset--;
                        if (m_ChoosedCtrl.Items.Count > m_ChoosedCtrl.m_ItemsInWnd - 1)
                            m_ChoosedCtrl.m_ItemOffset = m_ChoosedCtrl.Items.Count - m_ChoosedCtrl.m_ItemsInWnd;
                    }*/
                    //m_ChoosedCtrl.OnItemsStateChanged();
                    //m_ChoosedCtrl.RecalculateSum();
                }
            }
        }
    }
}
