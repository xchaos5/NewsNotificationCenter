using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NewsNotificationCenter
{
    public class NewsNotifier
    {
        private const string _baseURL = "http://db.ziya.gov.cn/api/notifications";
        private const int _intervalInSeconds = 5;
        private LoginUser _loginUser;
        private Timer _timer;

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
			string md5Str = BitConverter.ToString(output).Replace("-","");
            queryString += "&sign=" + md5Str;
            queryString += "&timestamp=" + timestamp;

            return queryString;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
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

            // TODO: remove
            _timer.Stop();
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
                    JsonTextReader reader = new JsonTextReader(new StringReader(resultString));
                    while (reader.Read())
                    {
                        //if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "id")
                        //{
                        //    ID = reader.ReadAsInt32() ?? 0;
                        //}


                    }
                }
            }
            catch
            {
                
            }
            finally
            {
                
            }
        }

        private void Notify(string title, string content)
        {
            NotificationForm notifyForm = new NotificationForm();
            notifyForm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - notifyForm.Width,
                                             Screen.PrimaryScreen.WorkingArea.Height - notifyForm.Height);
            //string title = "子牙循环经济技术开发区加快项目建设速写";
            notifyForm.linkTitle.Text = title;
            notifyForm.linkTitle.Links.Add(0, title.Length, "http://www.ziya.gov.cn/zhengwu/yuanquxinwen/1759-zi-ya-xun-huan-jing-ji-ji-zhu-kai-fa-qu-jia-kuai");
            notifyForm.linkTitle.Left = (notifyForm.ClientSize.Width - notifyForm.linkTitle.Width) / 2;
            notifyForm.rtbDescription.Text = content; //"随着3月3日至7日静海县重点工作检查推动活动的开展，连日来，全县各部门和单位以只争朝夕的精神大干快 上，跑资金、谈项目，重点工程建设如火如荼，静海…";
            notifyForm.ShowForm();
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
