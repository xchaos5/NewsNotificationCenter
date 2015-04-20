using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NewsNotificationCenter
{
    public class NewsNotifier
    {
        public class Message
        {
            public int ID;
            public string Content;
            public bool Notified;

            public Message(string msg)
            {
                using (JsonTextReader reader = new JsonTextReader(new StringReader(msg)))
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "notification_id")
                        {
                            ID = reader.ReadAsInt32() ?? 0;
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "content")
                        {
                            Content = reader.ReadAsString();
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "notified")
                        {
                            bool.TryParse(reader.ReadAsString(), out Notified);
                        }
                    }
                }
            }
        }

        public class Post
        {
            public int ID;
            public string Title;
            public string Content;
            public string URL;
            public bool Notified;

            public Post(string post)
            {
                using (JsonTextReader reader = new JsonTextReader(new StringReader(post)))
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "notification_id")
                        {
                            ID = reader.ReadAsInt32() ?? 0;
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "title")
                        {
                            Title = reader.ReadAsString();
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "content")
                        {
                            Content = reader.ReadAsString();
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "url")
                        {
                            URL = reader.ReadAsString();
                        }
                        if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "notified")
                        {
                            bool.TryParse(reader.ReadAsString(), out Notified);
                        }
                    }
                }
            }
        }

        public class NewsNotifierEventArgs: EventArgs
        {
            public List<Message> Messages
            {
                get;
                private set;
            }
            public List<Post> Posts
            {
                get;
                private set;
            }

            public NewsNotifierEventArgs(List<Message> messages, List<Post> posts)
            {
                Messages = messages;
                Posts = posts;
            }
        }

        public EventHandler<NewsNotifierEventArgs> NewsArrived;

        private const string _baseURL = "http://db.ziya.gov.cn/api/notifications";
        private const int _intervalInSeconds = 1;
        private LoginUser _loginUser;
        private Timer _timer;
        Regex _messagesReg = new Regex("messages\"\\s*:\\s*\\[(?<message>.*?)\\]");
        Regex _postsReg = new Regex("posts\"\\s*:\\s*\\[(?<post>.*?)\\]");

        public NewsNotifier(LoginUser loginuser)
        {
            _loginUser = loginuser;
            _timer = new Timer();
            _timer.Interval = _intervalInSeconds * 1000;
            _timer.Tick += Timer_Tick;
        }

        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        private string GetQueryString()
        {
            string queryString = "?";
            string timestamp = GetTimeStamp();
            queryString += "user_id=" + _loginUser.ID;

            byte[] result = Encoding.Default.GetBytes(_loginUser.Token + timestamp);
            MD5 md5 = new MD5CryptoServiceProvider();  
            byte[] output = md5.ComputeHash(result);
			string md5Str = BitConverter.ToString(output).Replace("-","").ToLower();
            queryString += "&sign=" + md5Str;
            queryString += "&timestamp=" + timestamp;

            return queryString;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // TODO: remove
            _timer.Stop();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseURL + GetQueryString());
            request.Method = "GET";
            request.UserAgent = "Ziya Windows Client";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            }
            catch (WebException)
            {
                
            }
            catch
            {
                
            }
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest)asynchronousResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var resultString = streamReader.ReadToEnd();

                    List<Message> messages = GetMessages(resultString);
                    List<Post> posts = GetPosts(resultString);

                    NewsNotifierEventArgs e = new NewsNotifierEventArgs(messages, posts);
                    NewsArrived(this, e);
                }
            }
            catch
            {
                
            }
            finally
            {
                
            }
        }

        private List<Message> GetMessages(string result)
        {
            List<Message> messages = new List<Message>();
            MatchCollection matches = _messagesReg.Matches(result);
            if (matches.Count > 0)
            {
                string[] messageArr = matches[0].Groups["message"].Value.Split(new string[]{"},{"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string m in messageArr)
                {
                    // iterate each message string
                    string messageStr = "{" + m.Replace("{", "").Replace("}", "") + "}";
                    messages.Add(new Message(messageStr));
                }
            }
            return messages;
        }

        private List<Post> GetPosts(string result)
        {
            List<Post> posts = new List<Post>();
            MatchCollection matches = _postsReg.Matches(result);
            if (matches.Count > 0)
            {
                string[] postArr = matches[0].Groups["post"].Value.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string p in postArr)
                {
                    // iterate each post string
                    string postStr = "{" + p.Replace("{", "").Replace("}", "") + "}";
                    posts.Add(new Post(postStr));
                }
            }
            return posts;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
