using System;
using System.Collections.Generic;
using System.Text;

namespace musicbox
{
    internal interface IFileNameAndMeda
    {
        String FileName { get;}
        _Media Media { get;}
    } 
    internal class FileNameAndMedia : IFileNameAndMeda
    {
        private string m_FileName;
        private _Media m_Media;

        public _Media Media
        {
            get { return m_Media; }
            set { m_Media = value; }
        }

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        public FileNameAndMedia(string fileName, _Media media)
        {
            m_FileName = fileName;
            m_Media = media;
        }
    }

    internal interface IFileNameAndIMediaWithIsMoneyless
    {
        FileNameAndMedia FileNameAndMedia { get;}
        bool Moneyless { get;}
    }
    internal class FileNameAndMediaWithIsMoneyless : IFileNameAndIMediaWithIsMoneyless
    {
        private readonly bool m_Moneyless;
        private readonly FileNameAndMedia m_FileNameAndMedia;

        public FileNameAndMedia FileNameAndMedia
        {
            get { return m_FileNameAndMedia; }
        } 
        public bool Moneyless
        {
            get { return m_Moneyless; }
        }
        public FileNameAndMediaWithIsMoneyless(FileNameAndMedia fileNameAndMedia, bool moneyless)
        {
            m_FileNameAndMedia = fileNameAndMedia;
            m_Moneyless = moneyless;
        }
    }

    internal class FileNameAndDemo
    {
        private readonly bool m_Demo;
        //private readonly FileNameAndMedia m_FileNameAndMedia;
        private readonly string m_FileName;

        public bool Demo
        {
            get { return m_Demo; }
        }
        internal string FileName
        {
            get { return m_FileName; }
        } 
        public FileNameAndDemo(string fileName,bool demo)
        {
            m_Demo = demo;
            m_FileName = fileName;
        }
    }
}
