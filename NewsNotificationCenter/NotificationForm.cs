using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    public partial class NotificationForm : Form
    {
        Timer _timer = new Timer();
        int _currentState, _currentTop;

        public NotificationForm()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            _timer.Enabled = false;
            _timer.Tick += new EventHandler(_timer_Tick);

            Rectangle WorkAreaRectangle = System.Windows.Forms.Screen.GetWorkingArea(this);
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            
            WindowState = FormWindowState.Normal;
            this.SetBounds(WorkAreaRectangle.Width - this.Width,
            WorkAreaRectangle.Height - _currentTop, this.Width, this.Height);
            _currentState = 1;

            _timer.Enabled = true;
            Show();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Interval = 10;

            Rectangle WorkAreaRectangle = System.Windows.Forms.Screen.GetWorkingArea(this);
            if (_currentState == 1)
            {
                if (_currentTop < this.Height)
                {
                    _currentTop = _currentTop + Convert.ToInt16(this.Height / 10);
                    if (_currentTop > this.Height)
                    {
                        _currentTop = this.Height;
                    }
                    this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - _currentTop, this.Width, this.Height);

                }
                else
                {
                    _currentState = 2;
                    _timer.Enabled = false;
                }
            }
        }

        private void linkMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ziya.gov.cn");
        }

        private void linkTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
