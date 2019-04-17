using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace musicbox.Search
{
    class Search
    {
        //public static event EventHandler<SearchProgressEvenetArgs> SearchProgress;

        /*public class SearchProgressEvenetArgs :EventArgs
        {
            private int m_Percent;

            public int Percent
            {
                get { return m_Percent; }
                set { m_Percent = value; }
            }
            public SearchProgressEvenetArgs(int percent)
            {
                m_Percent = percent;
            }

        }*/
        internal class SearchResults
        {
            private List<FileInfo> m_Files = new List<FileInfo>();
            private List<DirectoryInfo> m_Directories = new List<DirectoryInfo>();

            public List<FileInfo> Files
            {
                get { return m_Files; }
                set { m_Files = value; }
            }
            public List<DirectoryInfo> Directories
            {
                get { return m_Directories; }
                set { m_Directories = value; }
            }
        }

        static internal void SearchFilesInPath(string path, string searchStr, string[] extensions, out SearchResults results)
        {
            int percentCounter = 0;
            DirectoryInfo[] rootDirs = new DirectoryInfo(path).GetDirectories();
            results = new SearchResults();
            if (rootDirs.Length > 0)
            {
                int percentBlock = 100 / rootDirs.Length;
                foreach (DirectoryInfo subdirLevel0 in rootDirs)
                {
                    results.Directories.AddRange(subdirLevel0.GetDirectories("*" + (String)searchStr + "*", SearchOption.TopDirectoryOnly));
                    foreach (DirectoryInfo subdirLevel1 in subdirLevel0.GetDirectories())
                    {
                        foreach (string extension in extensions)
                        {
                            results.Files.AddRange(subdirLevel1.GetFiles("*" + searchStr + "*" + extension, SearchOption.TopDirectoryOnly));
                        }
                    }
                    if (percentCounter + percentBlock <= 100)
                        percentCounter += percentBlock;
                    /*if (SearchProgress != null)
                    {
                        SearchProgress(this,new SearchProgressEvenetArgs(percentCounter));
                    }*/

                }
                //if (SearchProgress != null)
                //    SearchProgress(this, new SearchProgressEvenetArgs(100));
            }
        }
    }
}
