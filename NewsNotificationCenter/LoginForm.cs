using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            NotificationForm notifyForm = new NotificationForm();
            notifyForm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - notifyForm.Width,
                                             Screen.PrimaryScreen.WorkingArea.Height - notifyForm.Height);
            string title = "子牙循环经济技术开发区加快项目建设速写";
            notifyForm.linkTitle.Text = title;
            notifyForm.linkTitle.Links.Add(0, title.Length, "http://www.ziya.gov.cn/zhengwu/yuanquxinwen/1759-zi-ya-xun-huan-jing-ji-ji-zhu-kai-fa-qu-jia-kuai");
            notifyForm.linkTitle.Left = (this.ClientSize.Width - notifyForm.linkTitle.Width) / 2;
            notifyForm.rtbDescription.Text = "随着3月3日至7日静海县重点工作检查推动活动的开展，连日来，全县各部门和单位以只争朝夕的精神大干快 上，跑资金、谈项目，重点工程建设如火如荼，静海…";
            notifyForm.ShowForm();
        }
    }
}
