using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    public partial class LoginForm : Form
    {
        private enum AppStatus
        {
            LoggedOut,
            LoggingIn,
            LoggedIn
        }

        private AppStatus _currentStatus;
        private LoginUser _loginUser;
        private NewsNotifier _newsNotifier;
        private const int _balloonTipTimeOut = 2000;

        public LoginForm()
        {
            InitializeComponent();

            _loginUser = new LoginUser();
            _loginUser.LoginStatusChanged += OnLoginStatusChanged;
            _newsNotifier = new NewsNotifier(_loginUser);
            _newsNotifier.NewsArrived += OnNewsArrivied;

            _currentStatus = AppStatus.LoggedOut;
        }

        private void OnNewsArrivied(object sender, NewsNotifier.NewsNotifierEventArgs e)
        {
            foreach (var message in e.Messages)
            {
                this.Invoke(new Action<NewsNotifier.Message>(Notify), message);
            }
            foreach (var post in e.Posts)
            {
                this.Invoke(new Action<NewsNotifier.Post>(Notify), post);
            }
        }

        private void OnLoginStatusChanged(object sender, EventArgs e)
        {
            if (_loginUser.IsLoggedIn)
            {
                SwitchStatus(AppStatus.LoggedIn);
            }
            else
            {
                if (!String.IsNullOrEmpty(_loginUser.Error))
                {
                    MessageBox.Show(_loginUser.Error);
                }
                else
                {
                    MessageBox.Show("登录失败，请检查用户名和密码是否正确");
                }
                SwitchStatus(AppStatus.LoggedOut);
            }
        }

        private void SwitchStatus(AppStatus status)
        {
            if (status == AppStatus.LoggingIn)
            {
                this.lblStatus.Text = "正在登录";
                this.lblStatus.Update();

                this.txtUsername.Enabled = false;
                this.txtPassword.Enabled = false;
                this.btnLogin.Enabled = false;

                this.loginMenuItem.Visible = false;
                this.logoutMenuItem.Visible = false;
            }
            else if(status == AppStatus.LoggedIn)
            {
                this.txtUsername.Enabled = false;
                this.txtPassword.Enabled = false;
                this.btnLogin.Enabled = false;
                Hide();

                this.notifyIcon.BalloonTipText = "登录成功！";
                this.notifyIcon.ShowBalloonTip(_balloonTipTimeOut);

                this.loginMenuItem.Visible = false;
                this.logoutMenuItem.Visible = true;

                _newsNotifier.Start();
            }
            else
            {
                // Log out
                _newsNotifier.Stop();
                this.lblStatus.Text = "请先登录";
                this.txtPassword.Text = String.Empty;
                this.txtUsername.Enabled = true;
                this.txtPassword.Enabled = true;
                this.btnLogin.Enabled = true;
                this.Enabled = true;
                Show();

                if (_currentStatus == AppStatus.LoggedIn)
                {
                    this.notifyIcon.BalloonTipText = "您已退出！";
                    this.notifyIcon.ShowBalloonTip(_balloonTipTimeOut);
                }

                this.loginMenuItem.Visible = true;
                this.logoutMenuItem.Visible = false;
            }

            _currentStatus = status;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (_currentStatus == AppStatus.LoggedIn)
                return;

            Show();
            WindowState = FormWindowState.Normal;
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void loginMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void logoutMenuItem_Click(object sender, EventArgs e)
        {
            SwitchStatus(AppStatus.LoggedOut);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("请输入密码");
                return;
            }

            SwitchStatus(AppStatus.LoggingIn);

            _loginUser.Name = txtUsername.Text;
            _loginUser.Password = txtPassword.Text;
            _loginUser.Login();
        }

        private void Notify(NewsNotifier.Message message)
        {
            NotificationForm notifyForm = new NotificationForm();
            notifyForm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - notifyForm.Width,
                                             Screen.PrimaryScreen.WorkingArea.Height - notifyForm.Height);
            notifyForm.linkTitle.Text = "新的消息";
            notifyForm.rtbDescription.Text = message.Content;
            notifyForm.ShowForm();
        }

        // Sample post:
        // 子牙循环经济技术开发区加快项目建设速写
        // 随着3月3日至7日静海县重点工作检查推动活动的开展，连日来，全县各部门和单位以只争朝夕的精神大干快 上，跑资金、谈项目，重点工程建设如火如荼，静海…
        // http://www.ziya.gov.cn/zhengwu/yuanquxinwen/1759-zi-ya-xun-huan-jing-ji-ji-zhu-kai-fa-qu-jia-kuai
        private void Notify(NewsNotifier.Post post)
        {
            NotificationForm notifyForm = new NotificationForm();
            notifyForm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - notifyForm.Width,
                                             Screen.PrimaryScreen.WorkingArea.Height - notifyForm.Height);

            notifyForm.linkTitle.Text = post.Title;
            notifyForm.linkTitle.Links.Add(0, post.Title.Length, post.URL);
            notifyForm.linkTitle.Left = (notifyForm.ClientSize.Width - notifyForm.linkTitle.Width) / 2;
            notifyForm.rtbDescription.Text = post.Content;
            notifyForm.ShowForm();
        }
    }
}
