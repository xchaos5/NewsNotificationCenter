using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace NewsNotificationCenter
{
    public class ConfigSettings
    {
        private static ConfigSettings _instance;

        private ConfigSettings() { }

        public static ConfigSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigSettings();
                _instance._messageTitle = ConfigurationManager.AppSettings["MessageTitle"] ?? "「您有一条短消息」";
                _instance._messageTargetURL = ConfigurationManager.AppSettings["MessageTargetURL"] ?? "http://news.ziya.gov.cn/";
                _instance._loginURL = ConfigurationManager.AppSettings["LoginURL"] ?? "http://db.ziya.gov.cn/api/login";
                _instance._notificationURL = ConfigurationManager.AppSettings["NotificationURL"] ?? "http://db.ziya.gov.cn/api/notifications";

                int syncIntervalInSeconds = 120;
                if (ConfigurationManager.AppSettings["SyncIntervalInSeconds"] != null)
                {
                    Int32.TryParse(ConfigurationManager.AppSettings["SyncIntervalInSeconds"], out syncIntervalInSeconds);
                }
                _instance._syncIntervalInSeconds = syncIntervalInSeconds;

                int autoCloseTimeInSeconds = 60;
                if (ConfigurationManager.AppSettings["AutoCloseTimeInSeconds"] != null)
                {
                    Int32.TryParse(ConfigurationManager.AppSettings["AutoCloseTimeInSeconds"], out autoCloseTimeInSeconds);
                }
                _instance._autoCloseTimeInSeconds = autoCloseTimeInSeconds;
            }
            return _instance;
        }

        private string _messageTitle;
        public string MessageTitle
        {
            get { return _messageTitle; }
        }

        private string _messageTargetURL;
        public string MessageTargetURL
        {
            get { return _messageTargetURL; }
        }

        private string _loginURL;
        public string LoginURL
        {
            get { return _loginURL; }
        }

        private string _notificationURL;
        public string NotificationURL
        {
            get { return _notificationURL; }
        }

        private int _syncIntervalInSeconds;
        public int SyncIntervalInSeconds
        {
            get { return _syncIntervalInSeconds; }
        }

        private int _autoCloseTimeInSeconds;
        public int AutoCloseTimeInSeconds
        {
            get { return _autoCloseTimeInSeconds; }
        }
    }
}
