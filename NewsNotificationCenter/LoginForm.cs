using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
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

        private static LoginForm _instance;

        private AppStatus _currentStatus;
        private LoginUser _loginUser;
        private NewsNotifier _newsNotifier;
        private List<int> _notifiedList = new List<int>();

        private const int BalloonTipTimeOut = 2000;
        private const string UsernameDisplayText = "用户名";
        private const string PasswordDisplayText = "密码";
        private Color DisplayTextColor = Color.FromArgb(224, 224, 224);

        private LoginForm()
        {
            InitializeComponent();

            _loginUser = new LoginUser();
            _loginUser.LoginStatusChanged += OnLoginStatusChanged;
            _newsNotifier = new NewsNotifier(_loginUser);
            _newsNotifier.NewsArrived += OnNewsArrivied;

            btnLogin.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 255, 255);
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 255, 255, 255);

            _currentStatus = AppStatus.LoggedOut;
        }

        public static LoginForm GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoginForm();
            }
            return _instance;
        }

        private void OnNewsArrivied(object sender, NewsNotifier.NewsNotifierEventArgs e)
        {
            foreach (var message in e.Messages)
            {
                if (!_notifiedList.Contains(message.ID))
                {
                    _notifiedList.Add(message.ID);
                    this.Invoke(new Action<MyMessage>(Notify), message);
                }
            }
            foreach (var post in e.Posts)
            {
                if (!_notifiedList.Contains(post.ID))
                {
                    _notifiedList.Add(post.ID);
                    this.Invoke(new Action<Post>(Notify), post);
                }
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
                this.txtUsername.Enabled = false;
                this.txtPassword.Enabled = false;
                this.btnLogin.Enabled = false;

                this.loginMenuItem.Visible = false;
                this.logoutMenuItem.Visible = false;
            }
            else if (status == AppStatus.LoggedIn)
            {
                this.txtUsername.Enabled = false;
                this.txtPassword.Enabled = false;
                this.btnLogin.Enabled = false;
                Hide();

                this.notifyIcon.BalloonTipText = "登录成功！";
                this.notifyIcon.ShowBalloonTip(BalloonTipTimeOut);

                this.loginMenuItem.Visible = false;
                this.logoutMenuItem.Visible = true;

                _newsNotifier.Start();
            }
            else
            {
                // Log out
                _newsNotifier.Stop();
                this.txtPassword.Text = "密码";
                this.txtPassword.ForeColor = DisplayTextColor;
                this.txtPassword.PasswordChar = '\0';
                this.txtUsername.Enabled = true;
                this.txtPassword.Enabled = true;
                this.btnLogin.Enabled = true;
                this.Enabled = true;
                Show();

                if (_currentStatus == AppStatus.LoggedIn)
                {
                    this.notifyIcon.BalloonTipText = "您已退出！";
                    this.notifyIcon.ShowBalloonTip(BalloonTipTimeOut);
                }

                this.loginMenuItem.Visible = true;
                this.logoutMenuItem.Visible = false;

                List<Form> openForms = new List<Form>();
                foreach (Form form in Application.OpenForms)
                    openForms.Add(form);

                foreach (Form form in openForms)
                {
                    if (form is NotificationForm)
                    {
                        NotificationForm notificationForm = form as NotificationForm;
                        notificationForm.ShouldSetNotified = false;
                        notificationForm.Close();
                    }
                }
            }

            _currentStatus = status;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (_currentStatus == AppStatus.LoggedIn)
            {
                System.Diagnostics.Process.Start("http://www.ziya.gov.cn");
                return;
            }

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
            if (String.IsNullOrEmpty(txtUsername.Text) || txtUsername.Text == UsernameDisplayText)
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            if (String.IsNullOrEmpty(txtPassword.Text) || txtPassword.Text == PasswordDisplayText)
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

        private void LoginForm_Activated(object sender, EventArgs e)
        {
            lblTitle.Focus();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == UsernameDisplayText)
            {
                txtUsername.Text = String.Empty;
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.Text = UsernameDisplayText;
                txtUsername.ForeColor = DisplayTextColor;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == PasswordDisplayText)
            {
                txtPassword.Text = String.Empty;
                txtPassword.ForeColor = Color.Black;
                txtPassword.PasswordChar = '*';
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Text = PasswordDisplayText;
                txtPassword.ForeColor = DisplayTextColor;
                txtPassword.PasswordChar = '\0';
            }
        }
    }
}
