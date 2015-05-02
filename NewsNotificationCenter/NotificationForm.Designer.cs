namespace NewsNotificationCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
            this.linkTitle = new System.Windows.Forms.LinkLabel();
            this.labelNew = new System.Windows.Forms.Label();
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
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(175)))), ((int)(((byte)(88)))));
            this.labelNew.Location = new System.Drawing.Point(75, 36);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(79, 13);
            this.labelNew.TabIndex = 2;
            this.labelNew.Text = "1条新通知：";
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 141);
            this.Controls.Add(this.labelNew);
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
        private System.Windows.Forms.Label labelNew;

    }
}