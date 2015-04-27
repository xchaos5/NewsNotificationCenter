using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace NewsNotificationCenter
{
    public class LoginUser
    {
        private string _loginURL = ConfigSettings.GetInstance().LoginURL;

        public int ID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool IsLoggedIn
        {
            get;
            private set;
        }

        public string Token
        {
            get;
            private set;
        }

        public string Error
        {
            get;
            private set;
        }

        public EventHandler LoginStatusChanged;

        public void Login()
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Password))
                return;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_loginURL);
            request.KeepAlive = false;
            request.Method = "POST";
            request.UserAgent = "Ziya Windows Client";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                var postData = string.Format("username={0}&password={1}", Name, Password);
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(postData);
                }

                request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            }
            catch (WebException)
            {
                Error = "网络错误，请检查网络是否已连接";
                LoginStatusChanged(this, null);
            }
            catch
            {
                Error = "发送请求时发生了异常";
                LoginStatusChanged(this, null);
            }
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var resultString = streamReader.ReadToEnd();
                    using (JsonTextReader reader = new JsonTextReader(new StringReader(resultString)))
                    {
                        bool isLoggedIn = false;
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "id")
                            {
                                ID = reader.ReadAsInt32() ?? 0;
                            }

                            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "token")
                            {
                                Token = reader.ReadAsString();
                                isLoggedIn = true;
                            }
                        }
                        IsLoggedIn = isLoggedIn;
                        Error = String.Empty;
                    }
                }
            }
            catch
            {
                Error = "获取服务器反馈时发生了异常";
            }
            finally
            {
                response.Close();
                LoginStatusChanged(this, null);
            }
        }
    }
}
