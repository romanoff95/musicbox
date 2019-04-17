using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace musicbox.Main
{
    public partial class HeaderCtrl : UserControl
    {
        private readonly Image imgLeftScrollPressed = Properties.Resources.Page2_1_Press;
        private readonly Image imgRightScrollPressed = Properties.Resources.Page2_9_Press;

        private readonly Image imgStylesPressed = Properties.Resources.Page2_3_Press;

        private readonly Image imgAYaPressed = Properties.Resources.Page2_4_Press;

        private readonly Image imgAZPressed = Properties.Resources.Page2_5_Press;

        private readonly Image imgZeroNinePressed = Properties.Resources.Page2_6_Press;

        private readonly Image imgSearchPressed = Properties.Resources.Page2_7_Press;

        private delegate void ModeClickDel(Modes mode);

        public enum Modes
        {
            Styles,
            AYa,
            AZ,
            ZeroNine,
        }

        public class ModeElements : IEnumerable<String>
        {
            private Char first;
            private Char last;
            public ModeElements(Char first,Char last)
            {
                this.first = first;
                this.last = last;
            }
            public IEnumerator<String> GetEnumerator()
            {
                for (Char ch = first; ch < last; ch++)
                {
                    yield return (ch.ToString());
                } 
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public event EventHandler ItemClick;
        public event EventHandler ModeChanged;
        public event EventHandler SearchClick;

        private readonly Font m_ItemsFont = new Font("Arial", 20, FontStyle.Regular);
        private readonly Brush m_ItemsBrush = Brushes.DimGray;
        private ItemRect[] items;
        private Int32 nXDelta = 20;
        private Int32 nYOffset = 20;
        private Int32 nRightDeltaOffset = 30, nLeftDeltaOffset = 30;
        private Int32 nScrollOffset = 0;
        private Single nItemsWidth;
        private List<String> styleItems = new List<string>();

        private readonly List<String> aYaItems = new List<string>(new ModeElements('À','ß'));
        private readonly List<String> aZItems = new List<string>(new ModeElements('A', 'Z'));
        private readonly List<String> zeroNineItems = new List<string>(new ModeElements('0', '9'));

        private Modes m_Mode = Modes.Styles;


        public class ItemClickEventArgs : EventArgs
        {
            private String m_Item;

            public String Item
            {
                get { return m_Item; }
                set { m_Item = value; }
            }
            public ItemClickEventArgs(String item)
            {
                this.m_Item = item;
            }
        }
        
        public HeaderCtrl()
        {
            InitializeComponent();
            new ScrollWrapper(pictureBox10, pictureBox1, imgRightScrollPressed,
                imgLeftScrollPressed, 20, 1,new ScrollDel(ScrollItems));

            pbStylesMode.Tag = imgStylesPressed;
            pbAYaMode.Tag = imgAYaPressed;
            pbAZMode.Tag = imgAZPressed;
            pbZeroNineMode.Tag = imgZeroNinePressed;
            pbSearchMode.Tag = imgSearchPressed;

            foreach (PictureBox pictureBox in
                new PictureBox[] {pbStylesMode,pbAYaMode,pbAZMode,
                    pbZeroNineMode,pbSearchMode})
            {
                pictureBox.MouseDown += new MouseEventHandler(pictureBox_Mouse);
                pictureBox.MouseUp +=new MouseEventHandler(pictureBox_Mouse);
            }
            ModeClickDel modeClickDel = delegate(Modes mode)
            {
                Mode = mode;
                if (ModeChanged != null)
                    ModeChanged(this, EventArgs.Empty);
            };
            pbStylesMode.Click += delegate(Object sender, EventArgs e)
            {
                modeClickDel(Modes.Styles);
            };
            pbAYaMode.Click += delegate(Object sender, EventArgs e)
            {
                modeClickDel(Modes.AYa);
            };
            pbAZMode.Click += delegate(Object sender, EventArgs e)
            {
                modeClickDel(Modes.AZ);
            };
            pbZeroNineMode.Click += delegate(Object sender, EventArgs e)
            {
                modeClickDel(Modes.ZeroNine);
            };
            pbSearchMode.Click += delegate
            {
                if (SearchClick != null)
                    SearchClick(this, EventArgs.Empty);
            };
        }

        void pictureBox_Mouse(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            Debug.Assert(pictureBox != null);
            Image unpressedImage = pictureBox.Image;
            pictureBox.Image = pictureBox.Tag as Image;
            pictureBox.Tag = unpressedImage;
        }

        public Modes Mode
        {
            get { return m_Mode; }
            set 
            { 
                m_Mode = value;
                switch (value)
                {
                    case Modes.Styles:
                        {
                            DrawItems(styleItems.ToArray());
                            break;
                        }
                    case Modes.AYa:
                        {
                            DrawItems(aYaItems.ToArray());
                            break;
                        }
                    case Modes.AZ:
                        {
                            DrawItems(aZItems.ToArray());
                            break;
                        }
                    case Modes.ZeroNine:
                        {
                            DrawItems(zeroNineItems.ToArray());
                            break;
                        }
                }
            }
        }
        public String[] StyleItems
        {
            set
            {
                if(value != null)
                {
                    styleItems.Clear();
                    styleItems.AddRange(value);
                }
            }
            get
            {
                return styleItems.ToArray();
            }
        }
        private void DrawItems(String[] items)
        {
            Int32 nOffset = 0;
            nScrollOffset = 0;
            List<ItemRect> itemRectList = new List<ItemRect>();
            using (Graphics grfx = pbItems.CreateGraphics())
            {
                for (Int32 i = 0; i < items.Length; i++)
                {
                    SizeF itemSize = grfx.MeasureString(items[i], m_ItemsFont);
                    itemRectList.Add(new ItemRect(items[i], new RectangleF(nOffset, nYOffset,
                        itemSize.Width + nRightDeltaOffset + nLeftDeltaOffset, itemSize.Height)));
                    nOffset += Size.Round(itemSize).Width + nXDelta + nRightDeltaOffset + nLeftDeltaOffset;
                }
            }
            if(items.Length > 0)
                nItemsWidth = itemRectList[itemRectList.Count - 1].itemrect.X +
                    itemRectList[itemRectList.Count - 1].itemrect.Width;
            this.items = itemRectList.ToArray();
            pbItems.Invalidate();
        }

        public void ScrollItems(Int32 nPos)
        {
            if (items != null)
            {
                if (items.Length > 0)
                {
                    if (nItemsWidth > pbItems.ClientRectangle.Width)
                    {
                        if (nScrollOffset + nPos >= -nItemsWidth + pbItems.ClientRectangle.Width &&
                            nScrollOffset + nPos <= 0)
                        {
                            nScrollOffset += nPos;
                            Int32 nItemsLen = items.Length;
                            for (Int32 i = 0; i < nItemsLen; i++)
                            {
                                PointF itemLoc = new PointF(items[i].itemrect.Location.X + nPos, nYOffset);
                                SizeF sz = items[i].itemrect.Size;
                                items[i].itemrect = new RectangleF(itemLoc, sz);
                            }
                            pbItems.Invalidate();
                        }
                    }
                }
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (items != null)
            {
                Int32 nItemsLen = items.Length;
                for (Int32 i = 0; i < nItemsLen; i++)
                {
                    e.Graphics.DrawString(items[i].item, m_ItemsFont, m_ItemsBrush,
                        new PointF(items[i].itemrect.Location.X + nLeftDeltaOffset, items[i].itemrect.Location.Y));
                    if (i > 0)
                        e.Graphics.DrawLine(Pens.Gray, items[i].itemrect.Location.X, items[i].itemrect.Top,
                            items[i].itemrect.Location.X, items[i].itemrect.Bottom);
                }
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (items != null)
            {
                for (Int32 i = 0; i < items.Length; i++)
                {
                    if (e.Location.X >= items[i].itemrect.Location.X &&
                        e.Location.Y >= items[i].itemrect.Location.Y &&
                        e.Location.X <= items[i].itemrect.Location.X + items[i].itemrect.Width &&
                        e.Location.Y <= items[i].itemrect.Location.Y + items[i].itemrect.Height)
                    {
                        if (ItemClick != null)
                            ItemClick(this, new ItemClickEventArgs(items[i].item));
                        break;
                    }
                }
            }
        }
        public override bool Focused
        {
            get
            {
                return base.Focused;
            }
        }
    }
}
