using System;
using System.Collections.Generic;
using System.Text;
using DirectShowLib;
using System.Runtime.InteropServices;
using Sanford.Multimedia.Midi;

namespace musicbox
{
    class MediaInspector
    {
        public static Boolean TryMediaOpen(MediaTypes mediaType, String path)
        {
            Boolean ret = false;
            switch (mediaType)
            {
                case MediaTypes.Audio:
                    {
                        DirectShowLib.IGraphBuilder gb = (DirectShowLib.IGraphBuilder)new DirectShowLib.FilterGraph();
                        //Int32 hr = gb.RenderFile(path, null);
                        if (gb.RenderFile(path, null) == 0)
                            ret = true;
                        Marshal.ReleaseComObject(gb);
                        gb = null;
                        break;
                    }
                case MediaTypes.Video:
                    {
                        DirectShowLib.IGraphBuilder gb = (DirectShowLib.IGraphBuilder)new DirectShowLib.FilterGraph();
                        Int32 hr = gb.RenderFile(path, null);
                        if (hr == 0)
                        {
                            DirectShowLib.IVideoWindow videoWindow = (DirectShowLib.IVideoWindow)gb;
                            OABool lVisible;
                            if (videoWindow.get_Visible(out lVisible) == 0)
                                ret = true;
                            videoWindow = null;
                        }
                        Marshal.ReleaseComObject(gb);
                        gb = null;
                        break;
                    }
                case MediaTypes.Karaoke:
                    {
                        ret = true;
                        try
                        {
                            Sequence sequence = new Sequence(path);
                            sequence.Dispose();
                        }
                        catch (Exception)
                        {
                            ret = false;
                        }
                        break;
                    }
                default:
                    break;
            }
            return ret;
        }


    }
}
