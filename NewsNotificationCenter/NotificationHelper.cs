using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;

namespace NewsNotificationCenter
{
    public class NotificationHelper
    {
        private static string _baseURL = ConfigSettings.GetInstance().NotificationURL;

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetSign(LoginUser loginUser, string timeStamp)
        {
            byte[] result = Encoding.Default.GetBytes(loginUser.Token + timeStamp);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        public static string GetParameters(LoginUser loginUser)
        {
            string timestamp = GetTimeStamp();
            return String.Format("user_id={0}&sign={1}&timestamp={2}", loginUser.ID, GetSign(loginUser, timestamp), timestamp);
        }

        public static void GetNotifications(LoginUser loginUser, AsyncCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseURL + "?" + GetParameters(loginUser));
            request.KeepAlive = false;
            request.Method = "GET";
            request.UserAgent = "Ziya Windows Client";
            request.ContentType = "application/x-www-form-urlencoded";

            request.BeginGetResponse(callback, request);
        }

        public static void SetNotified(LoginUser loginUser, Notification notification)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseURL + "/" + notification.ID + "/notified");
            request.KeepAlive = false;
            request.Method = "POST";
            request.UserAgent = "Ziya Windows Client";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                var postData = GetParameters(loginUser);
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(postData);
                }

                request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            }
            catch (WebException)
            {
                
            }
            catch
            {
                
            }
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            var request = (HttpWebRequest)ar.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(ar);
            response.Close();
        }
    }
}
