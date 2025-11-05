using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana
{
    public class notifyForm : Form
    {
        //Timer t = new Timer();
        Queue<string> messageQueue = new Queue<string>(10);
        Array messageList = new string[10];
        private static notifyForm _notifyFormSingle;
        Size monitorSize;
        private notifyForm()
        {
            InitializeComponent();
            //t.Tick += new EventHandler(t_Tick);
            monitorSize = new Size(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
            this.DesktopBounds = new Rectangle(monitorSize.Width - 300, monitorSize.Height - 88, 300, 60);
            Hide();
            notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon1_BalloonTipClicked);
        }

        void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(this, null);
        }

        public static notifyForm GetInstance()
        {
            if (_notifyFormSingle == null)
            {
                _notifyFormSingle = new notifyForm();
            }
            return _notifyFormSingle;
        }

        //void t_Tick(object sender, EventArgs e)
        //{

        //}

        private void notifyForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        public void DisplayMessage(string s)
        {
            if (messageQueue.Count >= 10)
            {
                messageQueue.Dequeue();
            }
            messageQueue.Enqueue(s);
            //t.Interval = 4000;
            //t.Start();
            textBox1.Text = s;
            notifyIcon1.ShowBalloonTip(4000, "Message", s, ToolTipIcon.Info);
        }



        private void notifyForm_Load(object sender, EventArgs e)
        {

        }

        private void notifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            //if (FormWindowState.Normal == WindowState)
            Hide();
        }

        #region toolstrip item click
        private void displayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //textBox1.Text = messageQueue.p
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void showMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show();
            //WindowState = FormWindowState.Normal;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            messageList = messageQueue.ToArray();
            StringBuilder s5 = new StringBuilder();

            if (messageList.Length > 5)
            {
                for (int i = messageList.Length - 1; i >= 5; i--)
                {
                    s5.AppendLine(messageList.GetValue(i).ToString());
                }
            }
            else
            {
                for (int i = messageList.Length - 1; i >= 0; i--)
                {
                    s5.AppendLine(messageList.GetValue(i).ToString());
                }
            }
            textBox1.Text = s5.ToString();
            Show();
            WindowState = FormWindowState.Maximized;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            messageList = messageQueue.ToArray();
            StringBuilder s5 = new StringBuilder();

            for (int i = messageList.Length - 1; i >= 0; i--)
            {
                s5.AppendLine(messageList.GetValue(i).ToString());
            }
            textBox1.Text = s5.ToString();
            Show();
            WindowState = FormWindowState.Maximized;
        }
        #endregion

        #region
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (textBox1 != null)
                {
                    textBox1.Dispose();
                }
                if (notifyIcon1 != null)
                {
                    notifyIcon1.Dispose();
                }
                if (contextMenuStrip1 != null)
                {
                    contextMenuStrip1.Dispose();
                }
                if (displayToolStripMenuItem != null)
                {
                    displayToolStripMenuItem.Dispose();
                }
                if (showMessagesToolStripMenuItem != null)
                {
                    showMessagesToolStripMenuItem.Dispose();
                }
                if (toolStripMenuItem2 != null)
                {
                    toolStripMenuItem2.Dispose();
                }
                if (toolStripMenuItem3 != null)
                {
                    toolStripMenuItem3.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(notifyForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(292, 52);
            this.textBox1.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Notification";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.showMessagesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 48);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.displayToolStripMenuItem.Text = "Display";
            this.displayToolStripMenuItem.Click += new System.EventHandler(this.displayToolStripMenuItem_Click);
            // 
            // showMessagesToolStripMenuItem
            // 
            this.showMessagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.showMessagesToolStripMenuItem.Name = "showMessagesToolStripMenuItem";
            this.showMessagesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.showMessagesToolStripMenuItem.Text = "Show Messages";
            this.showMessagesToolStripMenuItem.Click += new System.EventHandler(this.showMessagesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem2.Text = "5";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem3.Text = "10";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // notifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 52);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.Name = "notifyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "notifyForm";
            this.Resize += new System.EventHandler(this.notifyForm_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.notifyForm_FormClosing);
            this.Load += new System.EventHandler(this.notifyForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;

        #endregion

    }
}