namespace Middleware.Installer
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbar = new System.Windows.Forms.ToolStripProgressBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleMiddlewareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleTouchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.checkSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unCheckSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgj = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgg = new System.Windows.Forms.DataGridView();
            this.tabNewTouch = new System.Windows.Forms.TabPage();
            this.dgNT = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgt = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgj)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgg)).BeginInit();
            this.tabNewTouch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNT)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgt)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.toolStripSeparator1,
            this.btnExecute,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(736, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConnect
            // 
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(72, 22);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExecute
            // 
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(67, 22);
            this.btnExecute.Text = "Execute";
            this.btnExecute.Visible = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(45, 22);
            this.toolStripButton2.Text = "Exit";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.pbar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 720);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(736, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(619, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "...";
            // 
            // pbar
            // 
            this.pbar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(100, 16);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "Root");
            this.imageList1.Images.SetKeyName(1, "U");
            this.imageList1.Images.SetKeyName(2, "V");
            this.imageList1.Images.SetKeyName(3, "P");
            this.imageList1.Images.SetKeyName(4, "FN");
            this.imageList1.Images.SetKeyName(5, "TF");
            this.imageList1.Images.SetKeyName(6, "IF");
            this.imageList1.Images.SetKeyName(7, "Selected");
            this.imageList1.Images.SetKeyName(8, "Database");
            this.imageList1.Images.SetKeyName(9, "Servers");
            this.imageList1.Images.SetKeyName(10, "Messages.bmp");
            this.imageList1.Images.SetKeyName(11, "Results.bmp");
            this.imageList1.Images.SetKeyName(12, "Listview.bmp");
            this.imageList1.Images.SetKeyName(13, "Treeview.bmp");
            this.imageList1.Images.SetKeyName(14, "Disconnect.bmp");
            this.imageList1.Images.SetKeyName(15, "SQLFile");
            this.imageList1.Images.SetKeyName(16, "Close");
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Size = new System.Drawing.Size(736, 671);
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleMiddlewareToolStripMenuItem,
            this.toggleTouchToolStripMenuItem,
            this.toolStripMenuItem1,
            this.checkSelectionToolStripMenuItem,
            this.unCheckSelectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 98);
            // 
            // toggleMiddlewareToolStripMenuItem
            // 
            this.toggleMiddlewareToolStripMenuItem.Name = "toggleMiddlewareToolStripMenuItem";
            this.toggleMiddlewareToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.toggleMiddlewareToolStripMenuItem.Text = "Check All";
            this.toggleMiddlewareToolStripMenuItem.Click += new System.EventHandler(this.toggleMiddlewareToolStripMenuItem_Click);
            // 
            // toggleTouchToolStripMenuItem
            // 
            this.toggleTouchToolStripMenuItem.Name = "toggleTouchToolStripMenuItem";
            this.toggleTouchToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.toggleTouchToolStripMenuItem.Text = "Uncheck All";
            this.toggleTouchToolStripMenuItem.Click += new System.EventHandler(this.toggleTouchToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
            // 
            // checkSelectionToolStripMenuItem
            // 
            this.checkSelectionToolStripMenuItem.Enabled = false;
            this.checkSelectionToolStripMenuItem.Name = "checkSelectionToolStripMenuItem";
            this.checkSelectionToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkSelectionToolStripMenuItem.Text = "Check Selection";
            this.checkSelectionToolStripMenuItem.Click += new System.EventHandler(this.checkSelectionToolStripMenuItem_Click);
            // 
            // unCheckSelectionToolStripMenuItem
            // 
            this.unCheckSelectionToolStripMenuItem.Enabled = false;
            this.unCheckSelectionToolStripMenuItem.Name = "unCheckSelectionToolStripMenuItem";
            this.unCheckSelectionToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.unCheckSelectionToolStripMenuItem.Text = "Uncheck Selection";
            this.unCheckSelectionToolStripMenuItem.Click += new System.EventHandler(this.unCheckSelectionToolStripMenuItem_Click);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(736, 304);
            this.txtLog.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(736, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtConnectionString);
            this.tabPage4.ImageIndex = 10;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(728, 336);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Database";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnectionString.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.Location = new System.Drawing.Point(0, 0);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(728, 336);
            this.txtConnectionString.TabIndex = 4;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgj);
            this.tabPage3.ImageIndex = 15;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(728, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SQL Jobs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgj
            // 
            this.dgj.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgj.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgj.ContextMenuStrip = this.contextMenuStrip1;
            this.dgj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgj.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgj.Location = new System.Drawing.Point(0, 0);
            this.dgj.Name = "dgj";
            this.dgj.Size = new System.Drawing.Size(728, 336);
            this.dgj.TabIndex = 2;
            this.dgj.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.GridVIew_CellBeginEdit);
            this.dgj.MouseLeave += new System.EventHandler(this.GridView_MouseLeave);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgg);
            this.tabPage5.ImageIndex = 15;
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(728, 336);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Gateway";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgg
            // 
            this.dgg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgg.ContextMenuStrip = this.contextMenuStrip1;
            this.dgg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgg.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgg.Location = new System.Drawing.Point(3, 3);
            this.dgg.Name = "dgg";
            this.dgg.Size = new System.Drawing.Size(722, 330);
            this.dgg.TabIndex = 0;
            // 
            // tabNewTouch
            // 
            this.tabNewTouch.Controls.Add(this.dgNT);
            this.tabNewTouch.ImageIndex = 15;
            this.tabNewTouch.Location = new System.Drawing.Point(4, 23);
            this.tabNewTouch.Name = "tabNewTouch";
            this.tabNewTouch.Size = new System.Drawing.Size(728, 336);
            this.tabNewTouch.TabIndex = 6;
            this.tabNewTouch.Text = "New Touch";
            this.tabNewTouch.UseVisualStyleBackColor = true;
            // 
            // dgNT
            // 
            this.dgNT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgNT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgNT.ContextMenuStrip = this.contextMenuStrip1;
            this.dgNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgNT.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgNT.Location = new System.Drawing.Point(0, 0);
            this.dgNT.Name = "dgNT";
            this.dgNT.Size = new System.Drawing.Size(728, 336);
            this.dgNT.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgt);
            this.tabPage2.ImageIndex = 15;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(728, 336);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Touch";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgt
            // 
            this.dgt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgt.ContextMenuStrip = this.contextMenuStrip1;
            this.dgt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgt.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgt.Location = new System.Drawing.Point(3, 3);
            this.dgt.Name = "dgt";
            this.dgt.Size = new System.Drawing.Size(722, 330);
            this.dgt.TabIndex = 1;
            this.dgt.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.GridVIew_CellBeginEdit);
            this.dgt.MouseLeave += new System.EventHandler(this.GridView_MouseLeave);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabNewTouch);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(736, 363);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 742);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgj)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgg)).EndInit();
            this.tabNewTouch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgNT)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgt)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toggleMiddlewareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleTouchToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar pbar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unCheckSelectionToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgt;
        private System.Windows.Forms.TabPage tabNewTouch;
        private System.Windows.Forms.DataGridView dgNT;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dgg;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgj;
        private System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.TextBox txtConnectionString;
    }
}