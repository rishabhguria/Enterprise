namespace FactSetSample
{
    partial class FactSetManager
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
            _rtConsumer.Dispose();
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
            this.textBoxFactSetSymbol = new System.Windows.Forms.TextBox();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.buttonSnapshot = new System.Windows.Forms.Button();
            this.buttonSubscription = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxResponse = new System.Windows.Forms.ListBox();
            this.contextMenuStripResponse = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.responseCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.responseClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.logCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxException = new System.Windows.Forms.ListBox();
            this.contextMenuStripException = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exceptionCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exceptionClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.checkBoxWriteInFile = new System.Windows.Forms.CheckBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.contextMenuStripResponse.SuspendLayout();
            this.contextMenuStripLog.SuspendLayout();
            this.contextMenuStripException.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFactSetSymbol
            // 
            this.textBoxFactSetSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFactSetSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxFactSetSymbol.Enabled = false;
            this.textBoxFactSetSymbol.Location = new System.Drawing.Point(570, 10);
            this.textBoxFactSetSymbol.Name = "textBoxFactSetSymbol";
            this.textBoxFactSetSymbol.Size = new System.Drawing.Size(246, 20);
            this.textBoxFactSetSymbol.TabIndex = 3;
            this.textBoxFactSetSymbol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFactSetSymbol_KeyDown);
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new System.Drawing.Point(114, 12);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(0, 13);
            this.labelConnectionStatus.TabIndex = 1;
            // 
            // buttonSnapshot
            // 
            this.buttonSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSnapshot.Enabled = false;
            this.buttonSnapshot.Location = new System.Drawing.Point(822, 7);
            this.buttonSnapshot.Name = "buttonSnapshot";
            this.buttonSnapshot.Size = new System.Drawing.Size(75, 23);
            this.buttonSnapshot.TabIndex = 4;
            this.buttonSnapshot.Text = "Snapshot";
            this.buttonSnapshot.UseVisualStyleBackColor = true;
            this.buttonSnapshot.Click += new System.EventHandler(this.buttonSnapshot_Click);
            // 
            // buttonSubscription
            // 
            this.buttonSubscription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSubscription.Enabled = false;
            this.buttonSubscription.Location = new System.Drawing.Point(903, 7);
            this.buttonSubscription.Name = "buttonSubscription";
            this.buttonSubscription.Size = new System.Drawing.Size(75, 23);
            this.buttonSubscription.TabIndex = 5;
            this.buttonSubscription.Text = "Subscription";
            this.buttonSubscription.UseVisualStyleBackColor = true;
            this.buttonSubscription.Click += new System.EventHandler(this.buttonSubscription_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 7);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxResponse
            // 
            this.listBoxResponse.ContextMenuStrip = this.contextMenuStripResponse;
            this.listBoxResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxResponse.FormattingEnabled = true;
            this.listBoxResponse.HorizontalExtent = 99999;
            this.listBoxResponse.HorizontalScrollbar = true;
            this.listBoxResponse.Location = new System.Drawing.Point(0, 40);
            this.listBoxResponse.Name = "listBoxResponse";
            this.listBoxResponse.Size = new System.Drawing.Size(1165, 276);
            this.listBoxResponse.TabIndex = 1;
            this.listBoxResponse.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxResponse_MouseDown);
            // 
            // contextMenuStripResponse
            // 
            this.contextMenuStripResponse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.responseCopyToolStripMenuItem,
            this.responseClearToolStripMenuItem});
            this.contextMenuStripResponse.Name = "contextMenuStripResponse";
            this.contextMenuStripResponse.Size = new System.Drawing.Size(103, 48);
            // 
            // responseCopyToolStripMenuItem
            // 
            this.responseCopyToolStripMenuItem.Name = "responseCopyToolStripMenuItem";
            this.responseCopyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.responseCopyToolStripMenuItem.Text = "Copy";
            this.responseCopyToolStripMenuItem.Click += new System.EventHandler(this.responseCopyToolStripMenuItem_Click);
            // 
            // responseClearToolStripMenuItem
            // 
            this.responseClearToolStripMenuItem.Name = "responseClearToolStripMenuItem";
            this.responseClearToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.responseClearToolStripMenuItem.Text = "Clear";
            this.responseClearToolStripMenuItem.Click += new System.EventHandler(this.responseClearToolStripMenuItem_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.ContextMenuStrip = this.contextMenuStripLog;
            this.listBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalExtent = 99999;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.Location = new System.Drawing.Point(0, 319);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(1165, 186);
            this.listBoxLog.TabIndex = 3;
            this.listBoxLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxLog_MouseDown);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logCopyToolStripMenuItem,
            this.logClearToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            this.contextMenuStripLog.Size = new System.Drawing.Size(103, 48);
            // 
            // logCopyToolStripMenuItem
            // 
            this.logCopyToolStripMenuItem.Name = "logCopyToolStripMenuItem";
            this.logCopyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.logCopyToolStripMenuItem.Text = "Copy";
            this.logCopyToolStripMenuItem.Click += new System.EventHandler(this.logCopyToolStripMenuItem_Click);
            // 
            // logClearToolStripMenuItem
            // 
            this.logClearToolStripMenuItem.Name = "logClearToolStripMenuItem";
            this.logClearToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.logClearToolStripMenuItem.Text = "Clear";
            this.logClearToolStripMenuItem.Click += new System.EventHandler(this.logClearToolStripMenuItem_Click);
            // 
            // listBoxException
            // 
            this.listBoxException.ContextMenuStrip = this.contextMenuStripException;
            this.listBoxException.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxException.FormattingEnabled = true;
            this.listBoxException.HorizontalExtent = 99999;
            this.listBoxException.HorizontalScrollbar = true;
            this.listBoxException.Location = new System.Drawing.Point(0, 508);
            this.listBoxException.Name = "listBoxException";
            this.listBoxException.Size = new System.Drawing.Size(1165, 82);
            this.listBoxException.TabIndex = 5;
            this.listBoxException.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxException_MouseDown);
            // 
            // contextMenuStripException
            // 
            this.contextMenuStripException.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exceptionCopyToolStripMenuItem,
            this.exceptionClearToolStripMenuItem});
            this.contextMenuStripException.Name = "contextMenuStripException";
            this.contextMenuStripException.Size = new System.Drawing.Size(103, 48);
            // 
            // exceptionCopyToolStripMenuItem
            // 
            this.exceptionCopyToolStripMenuItem.Name = "exceptionCopyToolStripMenuItem";
            this.exceptionCopyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.exceptionCopyToolStripMenuItem.Text = "Copy";
            this.exceptionCopyToolStripMenuItem.Click += new System.EventHandler(this.exceptionCopyToolStripMenuItem_Click);
            // 
            // exceptionClearToolStripMenuItem
            // 
            this.exceptionClearToolStripMenuItem.Name = "exceptionClearToolStripMenuItem";
            this.exceptionClearToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.exceptionClearToolStripMenuItem.Text = "Clear";
            this.exceptionClearToolStripMenuItem.Click += new System.EventHandler(this.exceptionClearToolStripMenuItem_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(984, 7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.checkBoxWriteInFile);
            this.panelTop.Controls.Add(this.buttonConnect);
            this.panelTop.Controls.Add(this.textBoxFactSetSymbol);
            this.panelTop.Controls.Add(this.labelConnectionStatus);
            this.panelTop.Controls.Add(this.buttonSnapshot);
            this.panelTop.Controls.Add(this.buttonCancel);
            this.panelTop.Controls.Add(this.buttonSubscription);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1165, 40);
            this.panelTop.TabIndex = 0;
            // 
            // checkBoxWriteInFile
            // 
            this.checkBoxWriteInFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxWriteInFile.AutoSize = true;
            this.checkBoxWriteInFile.Enabled = false;
            this.checkBoxWriteInFile.Location = new System.Drawing.Point(1073, 10);
            this.checkBoxWriteInFile.Name = "checkBoxWriteInFile";
            this.checkBoxWriteInFile.Size = new System.Drawing.Size(82, 17);
            this.checkBoxWriteInFile.TabIndex = 7;
            this.checkBoxWriteInFile.Text = "Write In File";
            this.checkBoxWriteInFile.UseVisualStyleBackColor = true;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 316);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1165, 3);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 505);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1165, 3);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // FactSetManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 590);
            this.Controls.Add(this.listBoxResponse);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.listBoxException);
            this.Name = "FactSetManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FactSet Sample";
            this.contextMenuStripResponse.ResumeLayout(false);
            this.contextMenuStripLog.ResumeLayout(false);
            this.contextMenuStripException.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFactSetSymbol;
        private System.Windows.Forms.Label labelConnectionStatus;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonSnapshot;
        private System.Windows.Forms.Button buttonSubscription;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListBox listBoxResponse;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.ListBox listBoxException;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripResponse;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripException;
        private System.Windows.Forms.ToolStripMenuItem responseCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem responseClearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logClearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exceptionCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exceptionClearToolStripMenuItem;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.CheckBox checkBoxWriteInFile;
    }
}

