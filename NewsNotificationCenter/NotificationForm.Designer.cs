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
            this.linkTitle = new System.Windows.Forms.LinkLabel();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.linkMore = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // linkTitle
            // 
            this.linkTitle.AutoSize = true;
            this.linkTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkTitle.Location = new System.Drawing.Point(125, 21);
            this.linkTitle.Name = "linkTitle";
            this.linkTitle.Size = new System.Drawing.Size(35, 17);
            this.linkTitle.TabIndex = 1;
            this.linkTitle.TabStop = true;
            this.linkTitle.Text = "Title";
            this.linkTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTitle_LinkClicked);
            // 
            // rtbDescription
            // 
            this.rtbDescription.BackColor = System.Drawing.SystemColors.Control;
            this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbDescription.Location = new System.Drawing.Point(12, 50);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(260, 115);
            this.rtbDescription.TabIndex = 2;
            this.rtbDescription.Text = "";
            // 
            // linkMore
            // 
            this.linkMore.AutoSize = true;
            this.linkMore.Location = new System.Drawing.Point(217, 179);
            this.linkMore.Name = "linkMore";
            this.linkMore.Size = new System.Drawing.Size(55, 13);
            this.linkMore.TabIndex = 3;
            this.linkMore.TabStop = true;
            this.linkMore.Text = "查看更多";
            this.linkMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMore_LinkClicked);
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 201);
            this.Controls.Add(this.linkMore);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.linkTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationForm";
            this.Text = "通知";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.LinkLabel linkTitle;
        public System.Windows.Forms.RichTextBox rtbDescription;
        private System.Windows.Forms.LinkLabel linkMore;

    }
}