using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace musicbox
{
    internal interface IMediaManager
    {
        bool TryGetMedia(string extension, out _Media media);
        _Media GetMedia(string extension);
        IEnumerable<_Media> GetAllMedia();
        void SetMedia(string extension, _Media media);
    }
    class MediaManager : IMediaManager
    {
        private readonly string mp3extension = ".mp3";
        private readonly string aviExtension = ".avi";
        private readonly string karExtension = ".kar";

        private readonly Dictionary<string, _Media> m_Items = new Dictionary<string, _Media>();
        private readonly object m_Locker = new object();
        //Singleton
        private static volatile MediaManager m_Instance;
        private static object m_SyncRoot = new object();

        public static MediaManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_SyncRoot)
                    {
                        if (m_Instance == null)
                            m_Instance = new MediaManager();
                    }
                }
                return m_Instance;
            }
        }
        private MediaManager()
        {
            m_Items.Add(mp3extension.ToUpperInvariant(), new _Media(mp3extension.ToUpperInvariant(),
                10, Properties.Resources.audio, "Аудио", MediaTypes.Audio,100,100));
            m_Items.Add(aviExtension.ToUpperInvariant(), new _Media(aviExtension.ToUpperInvariant(),
                20, Properties.Resources.video, "Видео", MediaTypes.Video, 100, 100));
            m_Items.Add(karExtension.ToUpperInvariant(), new _Media(karExtension.ToUpperInvariant(),
                50, Properties.Resources.karaoke, "Караоке", MediaTypes.Karaoke, 100, 100));
        }
        //-------------
        public bool TryGetMedia(string extension, out _Media media)
        {
            lock (m_Locker)
            {
                return m_Items.TryGetValue(extension.ToUpperInvariant(), out media);
            }
        }
        public IEnumerable<_Media> GetAllMedia()
        {
            lock (m_Locker)
            {
                foreach (KeyValuePair<string, _Media> kvp in m_Items)
                {
                    yield return kvp.Value;
                }
            }
        }
        public void SetMedia(string extension, _Media media)
        {
            lock (m_Locker)
            {
                m_Items[extension.ToUpperInvariant()] = media;
            }
        }
        public _Media GetMedia(string extension)
        {
            lock (m_Locker)
            {
                return m_Items[extension.ToUpperInvariant()];
            }
        }
    }
    interface IMedia
    {
        string Description { get; }
        string Extension { get; }
        System.Drawing.Bitmap Img { get; }
        MediaTypes MediaType { get; }
        int Price { get; set; }
        int DemoVolume { get; set;}
        int OrderVolume { get; set;}
    }

    class _Media : musicbox.IMedia
    {
        private readonly string m_Extension;
        private int m_Price;
        private readonly Bitmap m_Img;
        private readonly string m_Description;
        private readonly MediaTypes m_MediaType;
        private int m_OrderVolume;
        private int m_DemoVolume;

        public int OrderVolume
        {
            get { return m_OrderVolume; }
            set { m_OrderVolume = value; }
        } 

        public int DemoVolume
        {
            get { return m_DemoVolume; }
            set { m_DemoVolume = value; }
        } 

        public MediaTypes MediaType
        {
            get { return m_MediaType; }
        } 
        public string Extension
        {
            get { return m_Extension; }
        }
        public int Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }
        public Bitmap Img
        {
            get { return m_Img; }
        }
        public string Description
        {
            get { return m_Description; }
        }
        public override string ToString()
        {
            return m_Description;
        }
        public _Media(string extension, int price, Bitmap image, string description,MediaTypes mediaType,
            int orderVolume,int demoVolume)
        {
            m_Extension = extension;
            m_Price = price;
            m_Img = image;
            m_Description = description;
            m_MediaType = mediaType;
            m_OrderVolume = orderVolume;
            m_DemoVolume = demoVolume;
        }

    }

    public interface IUserSettingsManager
    {
        IEnumerable<UserSettings> GetAllItems();
    }
    class UserSettingsManager : IUserSettingsManager
    {
        private static UserSettingsManager m_Instance;
        private List<UserSettings> m_UserSettingsList = new List<UserSettings>();

        public static UserSettingsManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new UserSettingsManager();
                return m_Instance;
            }
        }

        private UserSettingsManager()
        {
            m_UserSettingsList.Add(new UserSettings("Администратор", "",false,
                new UserSettings.MenuTabsAccessPermissions(true, true, true, true, true, true, true)));
            m_UserSettingsList.Add(new UserSettings("Пользователь", "",true,
                new UserSettings.MenuTabsAccessPermissions(true, false, false, false, false, false, false)));
        }

        public IEnumerable<UserSettings> GetAllItems()
        {
            foreach (UserSettings us in m_UserSettingsList)
            {
                yield return us;
            }
        }
    }
    public interface IUserSettings
    {
        string UserName { get; set;}
        string UserPassword { get; set;}
        bool IsPermissionsUsed { get;}
        UserSettings.MenuTabsAccessPermissions _MenuTabsAccessPermissions { get;}
    }

    public class UserSettings : IUserSettings
    {
        private String m_UserName;
        private String m_UserPassword;
        private bool m_IsPermissionsUsed;
        private readonly MenuTabsAccessPermissions m_MenuTabsAccessPermissions;

        public struct MenuTabsAccessPermissions
        {
            private Boolean m_CommonTab;
            private Boolean m_CountersTab;
            private Boolean m_MoneylessModeTab;
            private Boolean m_VolumeTab;
            private Boolean m_PriceTab;
            private Boolean m_ManageUsersTab;
            private Boolean m_TimersTab;

            public MenuTabsAccessPermissions(
            Boolean AllowCommon, Boolean AllowCounters, Boolean AllowMoneylessMode,
                Boolean AllowVolume, Boolean AllowPrice, Boolean AllowManageUsers, Boolean allowTimers)
            {
                m_CommonTab = AllowCommon;
                m_CountersTab = AllowCounters;
                m_MoneylessModeTab = AllowMoneylessMode;
                m_VolumeTab = AllowVolume;
                m_PriceTab = AllowPrice;
                m_ManageUsersTab = AllowManageUsers;
                m_TimersTab = allowTimers;
            }

            public Boolean ManageUsersTab
            {
                get { return m_ManageUsersTab; }
                set { m_ManageUsersTab = value; }
            }

            public Boolean TimersTab
            {
                get { return m_TimersTab; }
                set { m_TimersTab = value; }
            }

            public Boolean PriceTab
            {
                get { return m_PriceTab; }
                set { m_PriceTab = value; }
            }

            public Boolean VolumeTab
            {
                get { return m_VolumeTab; }
                set { m_VolumeTab = value; }
            }

            public Boolean MoneylessModeTab
            {
                get { return m_MoneylessModeTab; }
                set { m_MoneylessModeTab = value; }
            }

            public Boolean CountersTab
            {
                get { return m_CountersTab; }
                set { m_CountersTab = value; }
            }

            public Boolean CommonTab
            {
                get { return m_CommonTab; }
                set { m_CommonTab = value; }
            }
        }

        public UserSettings(String UserName, String UserPassword,bool usePermissions,MenuTabsAccessPermissions menuTabsAccessPermissions)
        {
            m_UserName = UserName;
            m_UserPassword = UserPassword;
            m_MenuTabsAccessPermissions = menuTabsAccessPermissions;
            m_IsPermissionsUsed = usePermissions;
        }

        public String UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }
        public String UserPassword
        {
            get { return m_UserPassword; }
            set { m_UserPassword = value; }
        }
        public MenuTabsAccessPermissions _MenuTabsAccessPermissions
        {
            get { return m_MenuTabsAccessPermissions; }
        }
        public bool IsPermissionsUsed
        {
            get { return m_IsPermissionsUsed; }
        }
        public override string ToString()
        {
            return UserName;
        }

    }

    /*class FileNameAndMediaAndDemo
    {

    }*/
}
