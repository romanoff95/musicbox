using System;
using System.Collections.Generic;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace musicbox
{
    class Kar : IDisposable
    {
        private Sequence m_Sequence;
        private Sequencer m_Sequencer;
        private OutputDevice m_OutputDevice;
        private Control m_Owner;
        private const Int32 m_OutDeviceId = 0;
        private StringFormat m_StringFormat;

        private const int MMSYSERR_NOERROR = 0;

        static bool IsSupport()
        {
            Boolean bRetFlag = true;
            if (OutputDevice.DeviceCount == 0)
                bRetFlag = false;
            return bRetFlag;
        }
        public Control _Owner
        {
            set
            {
                m_Owner = value;
                m_Owner.Paint += new PaintEventHandler(m_Owner_Paint);
            }
        }

        [DllImport("winmm.dll")]
        static extern uint midiOutGetVolume(int hMidiOut,out uint Volume);
        [DllImport("winmm.dll")]
        static extern uint midiOutSetVolume(int hMidiOut, uint Volume);
        [DllImport("winmm.dll", SetLastError = true)]
        static extern uint midiOutGetDevCaps(int uDeviceID, out MIDIOUTCAPS caps, uint cbMidiOutCaps);
        [StructLayout(LayoutKind.Sequential)]
        struct MIDIOUTCAPS
        {
            public ushort wMid;
            public ushort wPid;
            public ulong vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public ushort wTechnology;
            public ushort wVoices;
            public ushort wNotes;
            public ushort wChannelMask;
            public uint dwSupport;
        }

        public Kar(string fileName)
        {
            m_Sequence = new Sequence();
            m_Sequencer = new Sequencer();
            m_StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            //m_StringFormat.Alignment = StringAlignment.Center;
            m_StringFormat.LineAlignment = StringAlignment.Center;
            m_StringFormat.Trimming = StringTrimming.EllipsisCharacter;
            //adjust
            m_Sequence.Format = 1;
            m_Sequencer.Position = 0;

            m_Sequencer.Sequence = m_Sequence;
            //------------
            m_OutputDevice = new OutputDevice(m_OutDeviceId);

            m_Sequence.Load(fileName);
            

            m_Sequencer.MetaMessagePlayed += new EventHandler<MetaMessageEventArgs>(m_Sequencer_MetaMessagePlayed);
            m_Sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(m_Sequencer_ChannelMessagePlayed);
            m_Sequencer.Chased += new EventHandler<ChasedEventArgs>(m_Sequencer_Chased);
            m_Sequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(m_Sequencer_SysExMessagePlayed);

            karTextBit = GetRawKarText(m_Sequence);
            bool startAdd = false;
            string queuedstr = "";
            int curPos = 0;
            foreach (KeyValuePair<int, string> var in karTextBit)
            {
                if (startAdd == false)
                {
                    if (var.Value.StartsWith("/") || var.Value.StartsWith("\\"))
                    {
                        startAdd = true;
                        queuedstr += var.Value;
                        curPos = var.Key;
                    }
                }
                else
                {
                    if (var.Value.StartsWith("/") || var.Value.StartsWith("\\"))
                    {
                        karTextByPiece.Add(curPos,queuedstr.Remove(0, 1));
                        queuedstr = String.Empty;
                        queuedstr += var.Value;
                        curPos = var.Key;
                    }
                    else
                    {
                        queuedstr += var.Value;
                    }
                }
            }
            if (queuedstr != String.Empty)
                karTextByPiece.Add(curPos, queuedstr.Remove(0, 1));
            if (karTextByPiece.Count > 0)
                filled = karTextByPiece.Values[0];
            if (karTextByPiece.Count > 1)
            {
                filled1 = karTextByPiece.Values[1];
                //m_Owner.Invalidate();
            }
        }

        void m_Sequencer_SysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
        }

        void m_Sequencer_Chased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                m_OutputDevice.Send(message);
            }
        }

        void m_Sequencer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            /*if (closing)
            {
                return;
            }*/
            
            m_OutputDevice.Send(e.Message);
        }
        void m_Owner_Paint(object sender, PaintEventArgs e)
        {
            //filled.Length /
            bool split = false, split1 = false;
            int splitidx = 0;
            string tmpfilled = string.Empty, tmpfilling = string.Empty;

            bool splitex = false, splitex1 = false;
            int splitidxex = 0;
            string tmpfilledex = string.Empty, tmpfillingex = string.Empty;
            
            SizeF sizeF = TextRenderer.MeasureText(filled, m_Owner.Font);
            Size size = Size.Round(sizeF);
            if (size.Width > m_Owner.ClientRectangle.Width)
            {
                split = true;
                //int prevlen = filled.Length;
                splitidx = filled.Length / 2;
                tmpfilled = filled.Insert(splitidx, Environment.NewLine);
                if (filling.Length > splitidx)
                {
                    split1 = true;
                    tmpfilling = filling.Insert(splitidx, Environment.NewLine);
                }
            }
            sizeF = TextRenderer.MeasureText(split ? tmpfilled : filled , m_Owner.Font);
            size = Size.Round(sizeF);

            Rectangle tempRect = new Rectangle(
                new Point((m_Owner.Width / 2) - (size.Width / 2), m_Owner.Height - size.Height*2 - 160),
                new Size(size.Width + 10, size.Height + 10));

            /*SizeF sizeF1 = TextRenderer.MeasureText(filled1, m_Owner.Font);
            Size size1 = Size.Round(sizeF1);
            Rectangle tempRect1 = new Rectangle(
                new Point((m_Owner.Width / 2) - (size1.Width / 2), m_Owner.Height - size1.Height - 90),
                new Size(size1.Width + 10, size1.Height + 10));*/
            SizeF sizeF1 = TextRenderer.MeasureText(filled1, m_Owner.Font);
            Size size1 = Size.Round(sizeF1);
            if (size1.Width > m_Owner.ClientRectangle.Width)
            {
                splitex = true;
                //int prevlen = filled.Length;
                splitidxex = filled1.Length / 2;
                tmpfilledex = filled1.Insert(splitidxex, Environment.NewLine);
                if (filling1.Length > splitidxex)
                {
                    splitex1 = true;
                    tmpfillingex = filling1.Insert(splitidxex, Environment.NewLine);
                }
            }
            sizeF1 = TextRenderer.MeasureText(splitex ? tmpfilledex : filled1, m_Owner.Font);
            size1 = Size.Round(sizeF1);

            Rectangle tempRect1 = new Rectangle(
                new Point((m_Owner.Width / 2) - (size1.Width / 2), m_Owner.Height - size1.Height - 90),
                new Size(size1.Width + 10, size1.Height + 10));


            TextRenderer.DrawText(e.Graphics, splitex ? tmpfilledex : filled1, m_Owner.Font, tempRect1, Color.White, TextFormatFlags.Default);
            TextRenderer.DrawText(e.Graphics, splitex1 ? tmpfillingex : filling1, m_Owner.Font, tempRect1, Color.Red, TextFormatFlags.Default);

            TextRenderer.DrawText(e.Graphics, split ? tmpfilled : filled , m_Owner.Font, tempRect, Color.White, TextFormatFlags.Default);
            TextRenderer.DrawText(e.Graphics, split1 ? tmpfilling : filling , m_Owner.Font, tempRect, Color.Red, TextFormatFlags.Default);
        }
        string filled = String.Empty;
        string filling = String.Empty;
        string filled1 = String.Empty;
        string filling1 = String.Empty;
        bool bFlag = false;
        int prevPos = 0;
        SortedList<int, string> karTextBit = new SortedList<int, string>();
        SortedList<int, string> karTextByPiece = new SortedList<int, string>();

        private delegate void UIInvokeDel();
        void m_Sequencer_MetaMessagePlayed(object sender, MetaMessageEventArgs e)
        {
            if (e.Message.MetaType == MetaType.Text)
            {
                if (prevPos == m_Sequencer.Position)
                    return;

                string tmpFilled;
                
                int filledIdx;
                if ((filledIdx = karTextByPiece.IndexOfKey(m_Sequencer.Position - 1)) != -1)
                {
                    string tmpNextFilled = String.Empty ;
                    if (karTextByPiece.Count >= (filledIdx + 1 + 1))
                        tmpNextFilled = karTextByPiece.Values[filledIdx + 1];

                    
                    if (karTextByPiece.TryGetValue(m_Sequencer.Position - 1, out tmpFilled))
                    {
                        bFlag = !bFlag;
                        if (bFlag)
                        {
                            filled = tmpFilled;
                            filling = String.Empty;
                            filled1 = tmpNextFilled;
                            filling1 = String.Empty;
                        }
                        else
                        {
                            filled1 = tmpFilled;
                            filling1 = String.Empty;
                            filled = tmpNextFilled;
                            filling = String.Empty;
                        }
                    }
                }
                string tmpFilling;
                if (karTextBit.TryGetValue(m_Sequencer.Position - 1, out tmpFilling))
                {
                    if (!tmpFilling.StartsWith("@"))
                    {
                        if (tmpFilling.StartsWith("/") || tmpFilling.StartsWith("\\"))
                            tmpFilling = tmpFilling.Remove(0, 1);
                        if (bFlag)
                            filling += tmpFilling;
                        else
                            filling1 += tmpFilling;
                    }
                }
                m_Owner.Invalidate();
                prevPos = m_Sequencer.Position;
            }
        }
        
        private SortedList<int,string> GetRawKarText(Sequence sequence)
        {
            IEnumerator<MidiEvent> mdiEventEnum;
            //string rawKarText = "";
            SortedList<int, string> ret = new SortedList<int,string>();
            foreach (Track track in sequence)
            {
                mdiEventEnum = track.Iterator().GetEnumerator();
                while (mdiEventEnum.MoveNext())
                {
                    if (mdiEventEnum.Current.MidiMessage.MessageType == MessageType.Meta)
                    {
                        if (((MetaMessage)mdiEventEnum.Current.MidiMessage).MetaType == MetaType.Text)
                        {
                            if (!ret.ContainsKey(mdiEventEnum.Current.AbsoluteTicks))
                            {
                                byte[] messageData = mdiEventEnum.Current.MidiMessage.GetBytes();
                                int nullByteIdx = Array.IndexOf<byte>(messageData, 0x00);
                                if (nullByteIdx != -1)
                                    Array.Resize<byte>( ref messageData, nullByteIdx);
                                string messageTextPiece = System.Text.Encoding.GetEncoding(1251).GetString(messageData);
                                ret.Add(mdiEventEnum.Current.AbsoluteTicks, messageTextPiece);
                            }
                        }
                    }
                }
            }
            return ret;
        }
        public void Play()
        {
            m_Sequencer.Start();
        }
        public Int32 PlayPosition
        {
            set { m_Sequencer.Position = value; }
            get { return m_Sequencer.Position; }
        }
        public Int32 Duration
        {
            get { return m_Sequence.GetLength(); }
        }
        public void Stop()
        {
            m_Sequencer.Stop();
        }
        public uint GetVolume()
        {
            //uint volume = -1;
            //MIDIOUTCAPS caps = new MIDIOUTCAPS();
            //midiOutGetDevCaps(m_OutputDevice.Handle, out caps, (uint)Marshal.SizeOf(typeof(MIDIOUTCAPS)));
            uint volume;
            
            uint ret = midiOutGetVolume((int)m_OutputDevice.Handle, out volume);
            return ret == MMSYSERR_NOERROR ? volume : 0;
        }
        public void SetVolume(uint volume)
        {
            midiOutSetVolume(m_OutputDevice.Handle, volume);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_Sequencer.Stop();
            filled = String.Empty;
            filling = String.Empty;
            filled1 = String.Empty;
            filling1 = String.Empty;
            //karqueue.Clear();
            m_Owner.Invalidate();
            if (m_OutputDevice != null)
                m_OutputDevice.Dispose();
            m_Sequence.Dispose();
            m_Sequencer.Dispose();
        }
        public Int32 Volume
        {
            set
            {
            }
            get
            {
                return 0;
            }
        }

        #endregion
    }
}
