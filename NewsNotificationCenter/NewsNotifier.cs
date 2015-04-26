using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NewsNotificationCenter
{
    public class Notification
    {
        public int ID;
        public string Content;
        public bool Notified;
    }

    public class Message : Notification
    {
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

    public class Post : Notification
    {
        public string Title;
        public string URL;

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

    public class NewsNotifier
    {
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

        private const int _intervalInSeconds = 1;
        private static LoginUser _loginUser;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            // TODO: remove
            _timer.Stop();

            try
            {
                NotificationHelper.GetNotifications(_loginUser, new AsyncCallback(ReadCallback));
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
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            try
            {
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
                response.Close();
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
                    Message message = new Message(messageStr);
                    messages.Add(message);
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
                    Post post = new Post(postStr);
                    posts.Add(post);
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

        public static void SetNotified(Notification notification)
        {
            NotificationHelper.SetNotified(_loginUser, notification);
        }
    }
}
