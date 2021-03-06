﻿namespace NewsNotificationCenter
{
    partial class NotificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
            this.linkTitle = new System.Windows.Forms.LinkLabel();
            this.linkLabelNew = new System.Windows.Forms.LinkLabel();
            this._popupTimer = new System.Windows.Forms.Timer(this.components);
            this._closeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // linkTitle
            // 
            this.linkTitle.AutoSize = true;
            this.linkTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkTitle.LinkColor = System.Drawing.SystemColors.ControlText;
            this.linkTitle.Location = new System.Drawing.Point(24, 62);
            this.linkTitle.MaximumSize = new System.Drawing.Size(200, 0);
            this.linkTitle.Name = "linkTitle";
            this.linkTitle.Size = new System.Drawing.Size(176, 17);
            this.linkTitle.TabIndex = 1;
            this.linkTitle.TabStop = true;
            this.linkTitle.Text = "标题标题标题标题标题标题";
            this.linkTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTitle_LinkClicked);
            // 
            // linkLabelNew
            // 
            this.linkLabelNew.AutoSize = true;
            this.linkLabelNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabelNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(175)))), ((int)(((byte)(88)))));
            this.linkLabelNew.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelNew.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(175)))), ((int)(((byte)(88)))));
            this.linkLabelNew.Location = new System.Drawing.Point(79, 38);
            this.linkLabelNew.Name = "linkLabelNew";
            this.linkLabelNew.Size = new System.Drawing.Size(79, 13);
            this.linkLabelNew.TabIndex = 3;
            this.linkLabelNew.TabStop = true;
            this.linkLabelNew.Text = "1条新通知：";
            this.linkLabelNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNew_LinkClicked);
            // 
            // _popupTimer
            // 
            this._popupTimer.Tick += new System.EventHandler(this._popupTimer_Tick);
            // 
            // _closeTimer
            // 
            this._closeTimer.Tick += new System.EventHandler(this._closeTimer_Tick);
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 141);
            this.Controls.Add(this.linkLabelNew);
            this.Controls.Add(this.linkTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationForm";
            this.ShowInTaskbar = false;
            this.Text = "通知:";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NotificationForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.LinkLabel linkTitle;
        private System.Windows.Forms.LinkLabel linkLabelNew;
        private System.Windows.Forms.Timer _popupTimer;
        private System.Windows.Forms.Timer _closeTimer;

    }
}