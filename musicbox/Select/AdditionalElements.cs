using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace musicbox
{
    struct ItemRect
    {
        public ItemRect(String item, RectangleF itemrect)
        {
            this.item = item;
            this.itemrect = itemrect;
        }
        public String item;
        public RectangleF itemrect;
    }

    public delegate void ScrollDel(Int32 nScrollStep);
    /*abstract public class ScrollBase
    {
        private Timer tmScroll = new Timer();
        private Int32 nScrollInterval_mSec;
        private Int32 nPos;
        private ScrollDel scrollDel;

        public ScrollBase(Int32 nScrollInterval_mSec, Int32 nPos, ScrollDel scrollDel)
        {
            this.nPos = nPos;
            this.nScrollInterval_mSec = nScrollInterval_mSec;
            this.scrollDel = scrollDel;

            tmScroll.Interval = nScrollInterval_mSec;
            tmScroll.Tick += delegate
            {
                scrollDel(this.nPos);
            };
        }
        public void ScrollStart(Boolean isForward)
        {
            //Int32 nPosEx = isForward ? nPos : -nPos;
            //tmScroll.Tag = nPosEx;
            this.nPos = isForward ? nPos : -nPos;
            tmScroll.Start();
        }
        public void ScrollStop()
        {
            tmScroll.Stop();
        }
        public void OneScroll(Boolean isForward)
        {
            Int32 nPosEx = isForward ? nPos : -nPos;
            scrollDel(nPos);
        }

    }*/
   
    public class ScrollWrapper
    {
        //private delegate void StartDel(Int32 nPos, PictureBox tempPictureBox, Image tempImage);
        //private delegate void StopDel(PictureBox tempPictureBox, Image tempImage);

        private readonly Image m_TopPressed;
        private readonly Image m_BottomPressed;
        private readonly int m_ScrollPositionMovePerStep;
        private readonly ScrollDel m_ScrollDel;
        private readonly Timer m_ScrollTimer;

        public ScrollWrapper(PictureBox topScroll, PictureBox bottomScroll,
            Image topPressed, Image bottomPressed, Int32 scrollPositionMovePerStep,
            Int32 millisecondsBetweenScrollInterval,ScrollDel scrollDel)
        {
            m_TopPressed = topPressed;
            m_BottomPressed = bottomPressed;
            m_ScrollPositionMovePerStep = scrollPositionMovePerStep;
            m_ScrollDel = scrollDel;
            m_ScrollTimer = new Timer();

            m_ScrollTimer.Interval = millisecondsBetweenScrollInterval;
            m_ScrollTimer.Tick += new EventHandler(m_ScrollTimer_Tick);

            topScroll.MouseDown += new MouseEventHandler(topScroll_MouseDown);
            bottomScroll.MouseDown += new MouseEventHandler(bottomScroll_MouseDown);

            topScroll.MouseUp += new MouseEventHandler(Scroll_MouseUp);
            bottomScroll.MouseUp += new MouseEventHandler(Scroll_MouseUp);
        }

        void Scroll_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            Debug.Assert(pictureBox != null);
            Image image = pictureBox.Tag as Image;
            Debug.Assert(image != null);
            pictureBox.Image = image;
            pictureBox.Tag = null;
            
            Debug.Assert(m_ScrollTimer.Enabled == true);
            m_ScrollTimer.Stop();
        }

        void m_ScrollTimer_Tick(object sender, EventArgs e)
        {
            bool isIncrement = (bool)m_ScrollTimer.Tag;
            m_ScrollDel(isIncrement ? -m_ScrollPositionMovePerStep : m_ScrollPositionMovePerStep);
        }


        void bottomScroll_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownHelper(false, sender);
        }
        void topScroll_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownHelper(true, sender);
        }

        //private void MouseUpHelper(object sender)
        private void MouseDownHelper(bool IsTop,object sender)
        {
            PictureBox pictureBox = sender as PictureBox;
            Debug.Assert(pictureBox != null);
            pictureBox.Tag = pictureBox.Image;
            pictureBox.Image = IsTop ? m_TopPressed : m_BottomPressed;
            StartScroll(IsTop ? true : false);
        }
        private void StartScroll(bool IsIncrement)
        {
            Debug.Assert(m_ScrollTimer.Enabled == false);
            m_ScrollTimer.Tag = IsIncrement;
            m_ScrollTimer.Start();
        }

    }

    class ApplicationMessage
    {
        private static readonly Image imgMessageBackground = Properties.Resources.message2;
        private static readonly Int32 nOkLabelXOffset = 250;
        private static readonly Int32 nOkLabelYOffset = 330;

        private static readonly Int32 nOkWidth = 110;
        private static readonly Int32 nOkHeight = 50;
        private static readonly Font fntMsg = new Font("Arial", 20, FontStyle.Regular);

        public static void Show(Form parent,String strMessage)
        {
            Form frm = new Form();
            //frm.Owner = parent;
            Label lbl = new Label();
            Label lblMsg = new Label();
            lbl.Parent = frm;
            lbl.Location = new Point(nOkLabelXOffset, nOkLabelYOffset);
            lbl.Size = new Size(nOkWidth, nOkHeight);
            lbl.BackColor = Color.Transparent;
            lbl.Click += new EventHandler(lbl_Click);
            //lbl.Dock = DockStyle.Top;

            lblMsg.Parent = frm;
            lblMsg.Font = fntMsg;
            lblMsg.AutoSize = true;
            lblMsg.BackColor = Color.Transparent;
            lblMsg.Text = strMessage;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.Location = new Point(imgMessageBackground.Width / 2 - lblMsg.Width / 2, imgMessageBackground.Height / 2 - lblMsg.Height / 2);

            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Size = imgMessageBackground.Size;
            frm.BackgroundImage = imgMessageBackground;
            frm.BackgroundImageLayout = ImageLayout.Stretch;
            frm.ShowInTaskbar = false;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(parent);
        }

        static void lbl_Click(object sender, EventArgs e)
        {
            ((Form)((Label)sender).Parent).Close();
        }
    }
}
