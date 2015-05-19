using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    public partial class NotificationForm : Form
    {
        private int _currentState, _currentTop;
        private Notification _notification;
        private string MessageTitle = ConfigSettings.GetInstance().MessageTitle;
        private string MessageTargetURL = ConfigSettings.GetInstance().MessageTargetURL;

        public bool ShouldSetNotified
        {
            get;
            set;
        }

        public delegate void InvokeDelegate(Notification notification);

        public NotificationForm(Notification notification)
        {
            InitializeComponent();

            _notification = notification;
            ShouldSetNotified = true;
        }

        public void ShowForm()
        {
            // Sample Post:
            // 子牙循环经济技术开发区加快项目建设速写
            // 随着3月3日至7日静海县重点工作检查推动活动的开展，连日来，全县各部门和单位以只争朝夕的精神大干快 上，跑资金、谈项目，重点工程建设如火如荼，静海…
            // http://www.ziya.gov.cn/zhengwu/yuanquxinwen/1759-zi-ya-xun-huan-jing-ji-ji-zhu-kai-fa-qu-jia-kuai
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            if (_notification is MyMessage)
            {
                MyMessage message = _notification as MyMessage;
                this.linkTitle.Text = MessageTitle;
                this.linkTitle.Links.Add(0, MessageTitle.Length, MessageTargetURL);
            }
            else if (_notification is Post)
            {
                Post post = _notification as Post;
                this.linkTitle.Text = post.Title;
                if (post.Title != null)
                {
                    this.linkTitle.Links.Add(0, post.Title.Length, post.URL ?? ConfigSettings.GetInstance().MessageTargetURL);
                }
                this.linkLabelNew.Links.Add(0, this.linkLabelNew.Text.Length, post.URL ?? ConfigSettings.GetInstance().MessageTargetURL);
            }
            this.linkTitle.Left = (this.ClientSize.Width - this.linkTitle.Width) / 2;

            _popupTimer.Enabled = false;
            _closeTimer.Enabled = false;
            _popupTimer.Interval = 10;
            _closeTimer.Interval = ConfigSettings.GetInstance().AutoCloseTimeInSeconds * 1000;
            _popupTimer.Tick += _popupTimer_Tick;
            _closeTimer.Tick += _closeTimer_Tick;

            Rectangle WorkAreaRectangle = System.Windows.Forms.Screen.GetWorkingArea(this);
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            
            WindowState = FormWindowState.Normal;
            this.SetBounds(WorkAreaRectangle.Width - this.Width,
            WorkAreaRectangle.Height - _currentTop, this.Width, this.Height);
            _currentState = 1;

            _popupTimer.Enabled = true;
            Show();
        }

        private void _popupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.IsDisposed)
                {
                    _popupTimer.Enabled = false;
                    return;
                }

                Rectangle WorkAreaRectangle = System.Windows.Forms.Screen.GetWorkingArea(this);
                if (_currentState == 1)
                {
                    if (_currentTop < this.Height)
                    {
                        _currentTop += Convert.ToInt32(this.Height / 10);
                        if (_currentTop > this.Height)
                        {
                            _currentTop = this.Height;
                        }
                        this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - _currentTop, this.Width, this.Height);
                    }
                    else
                    {
                        _currentState = 2;
                        _popupTimer.Enabled = false;
                        _closeTimer.Enabled = true;
                    }
                }
                if (_currentState == 3)
                {
                    if (_currentTop > 0)
                    {
                        _currentTop -= Convert.ToInt32(this.Height / 10);
                        this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - _currentTop, this.Width, this.Height);
                    }
                    else
                    {
                        _popupTimer.Enabled = false;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // In rare cases, this Tick callback will be called even after NotificationForm is closed (disposed).
                Debug.WriteLine("NotificationForm::_popupTimer_Tick, Exception, " + ex.Message, "Error");
            }
        }

        private void _closeTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed)
            {
                _closeTimer.Enabled = false;
                return;
            }

            _currentState = 3;
            _popupTimer.Enabled = true;
            _closeTimer.Enabled = false;
        }

        private void linkTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData != null)
                OpenLinkAndClose(e.Link.LinkData.ToString());
        }

        private void linkLabelNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData != null)
                OpenLinkAndClose(e.Link.LinkData.ToString());
        }

        private void OpenLinkAndClose(string url)
        {
            _popupTimer.Enabled = false;
            _closeTimer.Enabled = false;
            System.Diagnostics.Process.Start(url);
            this.Close();
        }

        private void NotificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ShouldSetNotified)
            {
                LoginForm.GetInstance().BeginInvoke(new InvokeDelegate(NewsNotifier.SetNotified), _notification);
            }
        }
    }
}
