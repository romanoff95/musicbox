using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace musicbox
{
    /*interface IItemInfo : IDisposable
    {
        String Name {get;set;}
        Int32 Volume { get;set;}
        MediaTypes MediaType { get;set;}
        Int32 Price { get;set;}
        Bitmap IconPicture { get;set;}
        Boolean TryOpen(String path);
        String ToString();
        event EventHandler PriceChanged;
    }*/
    [Serializable]
    public class MediaItemInfo 
    {
        private Bitmap imgIcon;
        private Int32 m_Price;
        private MediaTypes songType;
        private Int32 m_Volume;
        private String m_ItemName;
        private Boolean m_Used;
        [OptionalField] //1.3
        private Int32 m_DemoVolume;

        [field :NonSerialized]
        public event EventHandler PriceChanged;

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            m_DemoVolume = 100;
        }

        public String Name
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }

        public Int32 DemoVolume
        {
            get { return m_DemoVolume; }
            set { m_DemoVolume = value; }
        }

        public Int32 Volume
        {
            get { return m_Volume; }
            set { m_Volume = value; }
        }

        public MediaTypes MediaType
        {
            get { return songType; }
            set { songType = value; }
        }

        public Int32 Price
        {
            get { return m_Price; }
            set 
            { 
                m_Price = value;
                if (PriceChanged != null)
                    PriceChanged(this, EventArgs.Empty);
            }
        }
        public Bitmap ImgIcon
        {
            get { return imgIcon; }
            set { imgIcon = value; }
        }
        public Boolean Used
        {
            get { return m_Used; }
            set { m_Used = value; }
        }
        public MediaItemInfo( Bitmap img, Int32 nPrice, MediaTypes mediaType, Int32 nVolume,String ItemName,Boolean Used,Int32 demoVolume)
        {
            imgIcon = img;
            m_Price = nPrice;
            songType = mediaType;
            m_Volume = nVolume;
            m_ItemName = ItemName;
            m_Used = Used;
            m_DemoVolume = demoVolume;
        }
        public override string ToString()
        {
            return m_ItemName;
        }
    }
}
