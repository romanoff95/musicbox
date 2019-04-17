using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;


namespace musicbox
{
    interface IPlayItem : IDisposable
    {
        void Play();
        Double Duration { get;}
        Double CurrentPosition { get;set;}
        Boolean IsDemo { get; set;}
        int Volume { get;set;}
        String Path { get;}

    }
    class PlayItemBase
    {
        private Boolean m_IsDemo;
        private Boolean m_FallComplete = false;
        private String m_Path;

        public String Path
        {
            get { return m_Path; }
        }

        public Boolean FallComplete
        {
            get { return m_FallComplete; }
            set { m_FallComplete = value; }
        }

        public Boolean IsDemo
        {
            get { return m_IsDemo; }
            set { m_IsDemo = value; }
        }
        public PlayItemBase(Boolean isDemo,String path)
        {
            m_IsDemo = isDemo;
            m_Path = path;
        }
    }
    class AudioVideoBase : PlayItemBase,IPlayItem
    {
        protected DirectShowLib.IGraphBuilder m_GraphBuilder = null;
        protected DirectShowLib.IMediaPosition m_MediaPosition = null;
        protected DirectShowLib.IMediaControl m_MediaControl = null;
        private DirectShowLib.IBasicAudio m_BasicAudio = null;

        protected Int32 m_hr;

        public AudioVideoBase(String path, Boolean isDemo)
            : base(isDemo, path)
        {
            m_GraphBuilder = (DirectShowLib.IGraphBuilder)new FilterGraph();
            m_hr = m_GraphBuilder.RenderFile(path, null);
            DsError.ThrowExceptionForHR(m_hr);

            m_MediaControl = (DirectShowLib.IMediaControl)m_GraphBuilder;
            m_MediaPosition = (DirectShowLib.IMediaPosition)m_GraphBuilder;
            m_BasicAudio = (DirectShowLib.IBasicAudio)m_GraphBuilder;
        }

        public virtual void Dispose()
        {
            m_MediaControl = null;
            m_MediaPosition = null;
            Marshal.ReleaseComObject(m_GraphBuilder);
            m_GraphBuilder = null;
            //GC.Collect();
        }
        public virtual void Play()
        {
            m_hr = m_MediaControl.Run();
            DsError.ThrowExceptionForHR(m_hr);
        }
        public virtual Double Duration
        {
            get
            {
                Double plLength;
                m_hr = m_MediaPosition.get_Duration(out plLength);
                DsError.ThrowExceptionForHR(m_hr);
                return plLength;
            }
        }
        public virtual Double CurrentPosition
        {
            get
            {
                Double plTime;
                m_hr = m_MediaPosition.get_CurrentPosition(out plTime);
                DsError.ThrowExceptionForHR(m_hr);
                return plTime;
            }
            set
            {
                m_hr = m_MediaPosition.put_CurrentPosition(value);
                DsError.ThrowExceptionForHR(m_hr);
            }
        }
        public virtual int Volume
        {
            get
            {
                int plVolume;
                m_hr = m_BasicAudio.get_Volume(out plVolume);
                DsError.ThrowExceptionForHR(m_hr);
                return -(-(plVolume / 100) - 100);
            }
            set
            {
                Int32 plVolume = (value * 100) - 10000;
                m_hr = m_BasicAudio.put_Volume(plVolume);
                DsError.ThrowExceptionForHR(m_hr);
            }
        }
    }
    class AudioPlayItem : AudioVideoBase
    {
        public AudioPlayItem(String path, Boolean isDemo)
            : base(path,isDemo)
        {

        }

        public override void Dispose()
        {
            m_MediaControl.Stop();
            base.Dispose();
        }

    }
    class VideoPlayItem : AudioVideoBase
    {
        private DirectShowLib.IVideoWindow m_VideoWindow = null;
        private DirectShowLib.IBasicVideo m_BasicVideo = null;
        private delegate void VideoWndInvokeDel();

        public VideoPlayItem(String path, Boolean isDemo)
            : base(path,isDemo)
        {
            m_VideoWindow = (DirectShowLib.IVideoWindow)m_GraphBuilder;
            m_BasicVideo = (DirectShowLib.IBasicVideo)m_GraphBuilder;        
            
        }
        #region IPlayItem Members

        public override void Play()
        {
            
            //MainWnd.VideoWnd.panel1.BringToFront();
            //m_hr = m_VideoWindow.put_Owner(Main.MainWnd.s_MovieWnd.Handle);
            //DsError.ThrowExceptionForHR(m_hr);
            //m_hr = m_VideoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren);

            m_hr = m_VideoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(m_hr);
            /*m_hr = m_VideoWindow.put_FullScreenMode(OABool.True);
            m_VideoWindow.put_MessageDrain(Main.MainWnd.s_VideoWnd.Handle);*/
            
            if (Screen.AllScreens.Length > 1)
            {
                //Now
                Rectangle secondScreenRect = Screen.AllScreens[1].Bounds;
                m_VideoWindow.SetWindowPosition(secondScreenRect.Left, secondScreenRect.Top,
                    secondScreenRect.Width, secondScreenRect.Height);
                //10, 10);
                //m_hr = m_VideoWindow.put_FullScreenMode(OABool.True);
                //DesktopLocation = tempRect.Location;
                //Size = tempRect.Size;
            }
            
            /*m_VideoWindow.SetWindowPosition(0, 0,
                MainWnd.VideoWnd.ClientSize.Width, MainWnd.VideoWnd.ClientSize.Height);*/
            m_VideoWindow.SetWindowForeground(OABool.False);
            VideoWndSwitch(true);
            base.Play();
        }
        private void VideoWndSwitch(bool enable)
        {
            /*if (Main.MainWnd.VideoWnd.IsHandleCreated)
            {
                Main.MainWnd.VideoWnd.Invoke(new VideoWndInvokeDel(
                    delegate
                    {
                        if (enable ? !Main.MainWnd.VideoWnd.Visible : Main.MainWnd.VideoWnd.Visible)
                            Main.MainWnd.VideoWnd.Visible = enable;
                    }));
            }*/
        }
        public override void Dispose()
        {

            VideoWndSwitch(false);
            m_MediaControl.Stop();
            //IntPtr handle;

            /*m_hr = m_VideoWindow.get_Owner(out handle);
            DsError.ThrowExceptionForHR(m_hr);
            System.Windows.Forms.Form.FromHandle(handle).Dispose();*/
            //m_VideoWindow.put_Owner(IntPtr.Zero);

            DsError.ThrowExceptionForHR(m_hr);
            m_VideoWindow = null;
            m_BasicVideo = null;
            base.Dispose();
        }

        #endregion
    }
    class KarPlayItem : PlayItemBase, IPlayItem
    {
        private Kar m_Kar;
        public KarPlayItem(String path, Boolean isDemo)
            : base(isDemo,path)
        {
            m_Kar = new Kar(path);
        }
        #region IPlayItem Members

        public void Play()
        {
            m_Kar._Owner = Main.MainWnd.VideoWnd;
            m_Kar.Play();
        }

        public void Stop()
        {
            m_Kar.Stop();
        }

        public void Dispose()
        {
            m_Kar.Dispose();
        }

        /*public bool Stopped
        {
            get
            {
                return m_Kar.Stopped;
            }
        }

        public bool Disposed
        {
            get { return m_Kar.Disposed; }
        }*/

        public double Duration
        {
            get { return (Double)m_Kar.Duration; }
        }

        public double CurrentPosition
        {
            get { return (Double)m_Kar.PlayPosition; }
            set { m_Kar.PlayPosition = (Int32)value; }

        }
        public int Volume
        {
            get
            {
                return (int)(m_Kar.GetVolume() / 100);
            }
            set
            {
                int val = 655 * value;
                m_Kar.SetVolume((uint)val);
            }
        }
        #endregion
    }
}
