using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Data.OleDb;

namespace musicbox.Main
{
    class PlayManager
    {
        private readonly Queue<FileNameAndDemo> m_MediaQueue = new Queue<FileNameAndDemo>();
        private IPlayItem m_PlayItem;
        private readonly Thread playThread;
        public event EventHandler ItemsCountChanged;
        public event EventHandler<PlayStateEventArgs> PlayState;
        private readonly AutoResetEvent playStateEvent = new AutoResetEvent(false);
        //private bool m_Disposed = false;
        private bool m_Started = false;
        /*private readonly byte ThreadStopTimeoutSec = 1;

        private readonly string m_PlayListTableName = "PlayList";
        private readonly string m_DbConnectionString = Properties.Settings.Default.ConnectionString;
        private readonly string m_PlayListFileColName = "FileName";
        private readonly string m_PlayListIdColName = "Id";

        private readonly OleDbConnection m_DbConnection;*/
        private volatile static PlayManager m_Instance;
        private static object m_SyncRoot = new object();
        private readonly IMediaManager m_IMediaManager;

        public static PlayManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_SyncRoot)
                    {
                        if (m_Instance == null)
                            m_Instance = new PlayManager(MediaManager.Instance);
                        
                    }
                }
                return m_Instance;
            }
        }

        public class PlayStateEventArgs : EventArgs
        {
            private Int32 m_State;

            public Int32 State
            {
                get { return m_State; }
                set { m_State = value; }
            }
            public PlayStateEventArgs(Int32 state)
            {
                m_State = state;
            }
        }
 
        private PlayManager(IMediaManager iMediaManager) 
        {
            m_IMediaManager = iMediaManager;
            playThread = new Thread(new ThreadStart(PlayProc));

            //LoadPlayListFromDb();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start()
        {
            /*if (m_Disposed)
            {
                throw new ObjectDisposedException("Player.Start() : Object is already disposed");
            }*/
            Debug.Assert(m_Started == false);
            if (!m_Started)
            {
                Debug.Assert(!playThread.IsAlive);
                m_Started = true;
                playThread.Start();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            /*if (m_Disposed)
            {
                throw new ObjectDisposedException("Player.Stop() : Object is already disposed");
            }*/
            Debug.Assert(m_Started == true);
            _StopLoop();
        }
        private void _StopLoop()
        {
            if (m_Started)
            {
                Debug.Assert(playThread.IsAlive);
                playStateEvent.Set();
                playThread.Join();
                m_Started = false;
            }
        }
        private void LoadPlayListFromDb()
        {
            /*m_DbConnection = new OleDbConnection(m_DbConnectionString);
            m_DbConnection.Open();
            using (OleDbCommand command = m_DbConnection.CreateCommand())
            {
                command.CommandText = String.Format(
                    "SELECT {0},{1} FROM {2}", m_PlayListIdColName, m_PlayListFileColName, m_PlayListTableName);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    m_MediaQueue.Enqueue(new PlayMedia(reader[m_PlayListFileColName].ToString(),
                }

            }*/
        }
        private void SavePlayListInDb()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        ~PlayManager()
        {
            Cleanup();
        }
        /*[MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            if (m_Disposed == false)
            {
                Cleanup();
                m_Disposed = true;
                GC.SuppressFinalize(this);
            }
        }*/

        private void Cleanup()
        {
            if (m_Started)
            {
                _StopLoop();
            }
            ItemPlayStop();
            /*if(m_PlayItem != null)
                m_PlayItem.Dispose();*/
            //SavePlayListInDb();
        }


        /*void m_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw e.Error;
            playStateEvent.Set();
        }*/
        void Progress(int percent)
        {
            if (percent >= 0)
            {
                if (PlayState != null)
                    PlayState(this, new PlayStateEventArgs(percent));
            }
        }
        void PlayProc()
        {
            do
            {
                lock (this)
                {
                    if (m_PlayItem == null)
                    {
                        if (m_MediaQueue.Count > 0)
                        {
                            Progress(0);
                            ItemPlayStart();
                        }
                        else
                        {
                            AddDemo();
                        }

                    }
                    else
                    {
                        //Play ending
                        if (m_PlayItem.CurrentPosition >= m_PlayItem.Duration)
                        {
                            Progress(100);
                            ItemPlayStop();
                        }
                        else
                        {
                            //If now playing demo and queue has no demo media smooth stop demo
                            if (m_PlayItem.IsDemo)
                            {
                                if (m_MediaQueue.Count > 0)
                                {
                                    if (!m_MediaQueue.Peek().Demo)
                                    {
                                        SmoothStopPlay();
                                        OnItemsCountChanged();
                                    }
                                }
                            }
                            //--------------------------------------
                            /*MediaItemInfo tempItemInfo;
                            Program.TryGetPlayItemInfoByExtension(
                                Path.GetExtension(m_PlayItem.Path).ToLower(), out tempItemInfo);*/
                            /*if (m_PlayItem.Volume != (m_PlayItem.IsDemo ? m_PlayItem.Media.DemoVolume : m_PlayItem.Media.OrderVolume))
                            {
                                m_PlayItem.Volume = m_PlayItem.IsDemo ? m_PlayItem.Media.DemoVolume : m_PlayItem.Media.OrderVolume;
                            }*/
                            //m_PlayItem.Volume 
                            //m_PlayItem.Volume = m_PlayItem.IsDemo ? m_PlayItem.Media.DemoVolume : m_PlayItem.Media.OrderVolume;
                            Progress((Int32)(m_PlayItem.CurrentPosition / (m_PlayItem.Duration / 100)));
                        }
                    }
                }
            } while (!playStateEvent.WaitOne(20, false));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateVolume()
        {
            _Media media = m_IMediaManager.GetMedia(Path.GetExtension(m_PlayItem.Path));
            m_PlayItem.Volume = m_PlayItem.IsDemo ? media.DemoVolume : media.OrderVolume;
        }
        //m_PlayQueue locked && m_playQueue
        //m_PlayQueue have 
        private void SmoothStopPlay()
        {
            for (int i = 0; i < 5; i++)
            {
                m_PlayItem.Volume -= 5;
                Thread.Sleep(200);
            }
            m_PlayItem.CurrentPosition = m_PlayItem.Duration;
        }

        //locked
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void ItemPlayStart()
        {
            //IPlayItem playItem;
            FileNameAndDemo playMedia = m_MediaQueue.Peek();
            string extension = Path.GetExtension(playMedia.FileName);
            Debug.Assert(extension.Length > 0);
            _Media media = m_IMediaManager.GetMedia(extension);
            //IMPORTANT DirectXException Handling
            switch (media.MediaType)
            {
                case MediaTypes.Audio:
                    m_PlayItem = new AudioPlayItem(playMedia.FileName, playMedia.Demo);
                    break;
                case MediaTypes.Video:
                    m_PlayItem = new VideoPlayItem(playMedia.FileName, playMedia.Demo);
                    break;
                case MediaTypes.Karaoke:
                    m_PlayItem = new KarPlayItem(playMedia.FileName, playMedia.Demo);
                    break;
            }
            
            /*MediaItemInfo tempItemInfo;
            Program.TryGetPlayItemInfoByExtension(
                Path.GetExtension(playMedia.FullName).ToLower(), out tempItemInfo);*/
            //_Media media;
            //MediaManager.Instance.TryGetMedia(playMedia.FileName, out media);
            m_PlayItem.Volume = m_PlayItem.IsDemo ? media.DemoVolume : media.OrderVolume;
            m_PlayItem.Play();
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void ItemPlayStop()
        {
            if (m_PlayItem != null)
            {
                m_PlayItem.Dispose();
                m_PlayItem = null;
            }
            if(m_MediaQueue.Count > 0)
                m_MediaQueue.Dequeue();
            OnItemsCountChanged();
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void AddDemo()
        {
            String mediaPath;

            if (Menu.DemoSelect.Instance.GetRandomDemoFileName(out mediaPath))
            {
                _Media media;
                if (musicbox.MediaManager.Instance.TryGetMedia(Path.GetExtension(mediaPath),out media))
                {
                        m_MediaQueue.Enqueue(new FileNameAndDemo(mediaPath,true));
                        OnItemsCountChanged();
                }
            }
        }
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void OnItemsCountChanged()
        {
            if (ItemsCountChanged != null)
                ItemsCountChanged(this, EventArgs.Empty);
        }
        /*[MethodImpl(MethodImplOptions.Synchronized)]
        public void Play()
        {
            Debug.Assert(!playThread.IsAlive);
            playThread.Start();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            if(m_PlayItem != null)
                m_PlayItem.Dispose();
        }*/
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(FileNameAndDemo fileNameAndMedia)
        {
            m_MediaQueue.Enqueue(fileNameAndMedia);

            //StopPlayIfDemo();
            //OnItemsCountChanged();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public musicbox.FileNameAndDemo[] GetItems()
        {
            return m_MediaQueue.ToArray();
        }
        /*[MethodImpl(MethodImplOptions.Synchronized)]
        public void AddRange(musicbox.FileNameAndMediaAndDemo[] playMediaArr)
        {
            foreach (PlayMedia playMedia in playMediaArr)
            {
                m_MediaQueue.Enqueue(playMedia);
            }
            StopPlayIfDemo();
            OnItemsCountChanged();
        }*/
        //[MethodImpl(MethodImplOptions.Synchronized)]
        /*private void SmoothStopPlaying()
        {
            for (int i = 0; i < 5; i++)
            {
                m_PlayItem.Volume -= 5;
                Thread.Sleep(200);
            }
            m_PlayItem.CurrentPosition = m_PlayItem.Duration;
        }*/
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ClearPlaylist()
        {
            if (m_MediaQueue.Count > 0)
            {
                FileNameAndDemo fileNameAndMediaAndDemo = m_MediaQueue.Peek();
                m_MediaQueue.Clear();
                m_MediaQueue.Enqueue(fileNameAndMediaAndDemo);
                OnItemsCountChanged();
            }
            /*if (m_PlayItem != null)
            {
                m_PlayItem.CurrentPosition = m_PlayItem.Duration;
            }*/
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CancelPlayed()
        {
            if (m_PlayItem != null)
            {
                m_PlayItem.CurrentPosition = m_PlayItem.Duration;
            }
        }
        /*[MethodImpl(MethodImplOptions.Synchronized)]
        public void Serialize()
        {
            Program.SerializeToBinary(m_SerializeFilePath, m_MediaQueue);
        }*/
        [MethodImpl(MethodImplOptions.Synchronized)]
        public musicbox.FileNameAndDemo GetTopItem()
        {
            FileNameAndDemo retFileNameAndMedia = null;
            if(m_MediaQueue.Count > 0)
                retFileNameAndMedia = m_MediaQueue.Peek();
            return retFileNameAndMedia;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public musicbox.FileNameAndDemo[] GetItemsWithoutTop()
        {
            FileNameAndDemo[] returnArr;
            FileNameAndDemo[] playMediaArr = m_MediaQueue.ToArray();
            if (playMediaArr.Length > 1)
            {
                List<FileNameAndDemo> tempList = new List<FileNameAndDemo>(playMediaArr);
                tempList.RemoveAt(0);
                returnArr = tempList.ToArray();
            }
            else
                returnArr = new FileNameAndDemo[0];
            return returnArr;
        }

    }

    class PlayManagerEx
    {

    }
}
