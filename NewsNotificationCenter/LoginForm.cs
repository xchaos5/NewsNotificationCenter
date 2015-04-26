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
                this.Invoke(new Action<Message>(Notify), message);
            }
            foreach (var post in e.Posts)
            {
                this.Invoke(new Action<Post>(Notify), post);
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

        private void Notify(Notification notification)
        {
            NotificationForm notifyForm = new NotificationForm(notification);
            notifyForm.ShowForm();
        }
    }
}
