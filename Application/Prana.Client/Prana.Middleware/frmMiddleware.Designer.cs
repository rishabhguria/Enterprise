namespace Prana.Middleware
{
    partial class frmMiddleware
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMiddleware));
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblJobStatus = new Infragistics.Win.Misc.UltraLabel();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.btnStart = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblLastRun = new Infragistics.Win.Misc.UltraLabel();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.StatusThread = new System.ComponentModel.BackgroundWorker();
            this.lblState = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listJobs = new System.Windows.Forms.ListView();
            this.JobName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastRun = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmMiddleware_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmMiddleware_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmMiddleware_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.groupBox1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label1.Location = new System.Drawing.Point(18, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Job Status";
            // 
            // lblJobStatus
            // 
            this.lblJobStatus.Location = new System.Drawing.Point(22, 45);
            this.lblJobStatus.Name = "lblJobStatus";
            this.lblJobStatus.Size = new System.Drawing.Size(81, 18);
            this.lblJobStatus.TabIndex = 1;
            this.lblJobStatus.Text = "Idle";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(22, 76);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(380, 23);
            this.pb.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btnStart.Enabled = false;
            this.btnStart.ImageList = this.imageList1;
            this.btnStart.Location = new System.Drawing.Point(307, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "1-1.bmp");
            this.imageList1.Images.SetKeyName(1, "1-2.bmp");
            this.imageList1.Images.SetKeyName(2, "1-3.bmp");
            this.imageList1.Images.SetKeyName(3, "1-4.bmp");
            this.imageList1.Images.SetKeyName(4, "1-5.bmp");
            this.imageList1.Images.SetKeyName(5, "2-1.bmp");
            this.imageList1.Images.SetKeyName(6, "2-2.bmp");
            this.imageList1.Images.SetKeyName(7, "2-3.bmp");
            this.imageList1.Images.SetKeyName(8, "2-4.bmp");
            this.imageList1.Images.SetKeyName(9, "2-5.bmp");
            this.imageList1.Images.SetKeyName(10, "2-6.bmp");
            this.imageList1.Images.SetKeyName(11, "2-7.bmp");
            this.imageList1.Images.SetKeyName(12, "2-8.bmp");
            this.imageList1.Images.SetKeyName(13, "2-9.bmp");
            this.imageList1.Images.SetKeyName(14, "2-10.bmp");
            this.imageList1.Images.SetKeyName(15, "about.bmp");
            this.imageList1.Images.SetKeyName(16, "add.bmp");
            this.imageList1.Images.SetKeyName(17, "addexistingitem.bmp");
            this.imageList1.Images.SetKeyName(18, "addnewitem.bmp");
            this.imageList1.Images.SetKeyName(19, "address.bmp");
            this.imageList1.Images.SetKeyName(20, "adjustment.bmp");
            this.imageList1.Images.SetKeyName(21, "amex.bmp");
            this.imageList1.Images.SetKeyName(22, "arrow.bmp");
            this.imageList1.Images.SetKeyName(23, "assign_check.bmp");
            this.imageList1.Images.SetKeyName(24, "attach.bmp");
            this.imageList1.Images.SetKeyName(25, "attach_header.bmp");
            this.imageList1.Images.SetKeyName(26, "auto_width.bmp");
            this.imageList1.Images.SetKeyName(27, "bands_view.bmp");
            this.imageList1.Images.SetKeyName(28, "binary.bmp");
            this.imageList1.Images.SetKeyName(29, "bitmap.bmp");
            this.imageList1.Images.SetKeyName(30, "blanksolution.bmp");
            this.imageList1.Images.SetKeyName(31, "bold.bmp");
            this.imageList1.Images.SetKeyName(32, "bug.bmp");
            this.imageList1.Images.SetKeyName(33, "bullet.bmp");
            this.imageList1.Images.SetKeyName(34, "bullets.bmp");
            this.imageList1.Images.SetKeyName(35, "button_mode.bmp");
            this.imageList1.Images.SetKeyName(36, "Calendar_Small.bmp");
            this.imageList1.Images.SetKeyName(37, "cars_saloon.bmp");
            this.imageList1.Images.SetKeyName(38, "cars_sport.bmp");
            this.imageList1.Images.SetKeyName(39, "cars_truck.bmp");
            this.imageList1.Images.SetKeyName(40, "cascade.bmp");
            this.imageList1.Images.SetKeyName(41, "cash.bmp");
            this.imageList1.Images.SetKeyName(42, "category01.bmp");
            this.imageList1.Images.SetKeyName(43, "category02.bmp");
            this.imageList1.Images.SetKeyName(44, "category03.bmp");
            this.imageList1.Images.SetKeyName(45, "category04.bmp");
            this.imageList1.Images.SetKeyName(46, "category05.bmp");
            this.imageList1.Images.SetKeyName(47, "category06.bmp");
            this.imageList1.Images.SetKeyName(48, "category07.bmp");
            this.imageList1.Images.SetKeyName(49, "category08.bmp");
            this.imageList1.Images.SetKeyName(50, "center.bmp");
            this.imageList1.Images.SetKeyName(51, "check_list.bmp");
            this.imageList1.Images.SetKeyName(52, "checked.bmp");
            this.imageList1.Images.SetKeyName(53, "class.bmp");
            this.imageList1.Images.SetKeyName(54, "classview.bmp");
            this.imageList1.Images.SetKeyName(55, "clear.bmp");
            this.imageList1.Images.SetKeyName(56, "clients.bmp");
            this.imageList1.Images.SetKeyName(57, "close.bmp");
            this.imageList1.Images.SetKeyName(58, "closesolution.bmp");
            this.imageList1.Images.SetKeyName(59, "collapse.bmp");
            this.imageList1.Images.SetKeyName(60, "Contacts_Small.bmp");
            this.imageList1.Images.SetKeyName(61, "copy.bmp");
            this.imageList1.Images.SetKeyName(62, "csclass.bmp");
            this.imageList1.Images.SetKeyName(63, "csproject.bmp");
            this.imageList1.Images.SetKeyName(64, "customdraw_cells.bmp");
            this.imageList1.Images.SetKeyName(65, "customdraw_columns.bmp");
            this.imageList1.Images.SetKeyName(66, "customdraw_footer.bmp");
            this.imageList1.Images.SetKeyName(67, "customdraw_footer_cell.bmp");
            this.imageList1.Images.SetKeyName(68, "customdraw_header_cell.bmp");
            this.imageList1.Images.SetKeyName(69, "customdraw_header_indent.bmp");
            this.imageList1.Images.SetKeyName(70, "customdraw_indicator.bmp");
            this.imageList1.Images.SetKeyName(71, "customdraw_node_button.bmp");
            this.imageList1.Images.SetKeyName(72, "customdraw_node_images.bmp");
            this.imageList1.Images.SetKeyName(73, "customdraw_row_cell.bmp");
            this.imageList1.Images.SetKeyName(74, "customdraw_separator.bmp");
            this.imageList1.Images.SetKeyName(75, "customdraw_tree_buttons.bmp");
            this.imageList1.Images.SetKeyName(76, "customization.bmp");
            this.imageList1.Images.SetKeyName(77, "customizationform.bmp");
            this.imageList1.Images.SetKeyName(78, "customizing.bmp");
            this.imageList1.Images.SetKeyName(79, "cut.bmp");
            this.imageList1.Images.SetKeyName(80, "date.bmp");
            this.imageList1.Images.SetKeyName(81, "del.bmp");
            this.imageList1.Images.SetKeyName(82, "delete.bmp");
            this.imageList1.Images.SetKeyName(83, "Deleted_Large.bmp");
            this.imageList1.Images.SetKeyName(84, "Deleted_Small.bmp");
            this.imageList1.Images.SetKeyName(85, "disk.bmp");
            this.imageList1.Images.SetKeyName(86, "drag_nodes.bmp");
            this.imageList1.Images.SetKeyName(87, "earth.bmp");
            this.imageList1.Images.SetKeyName(88, "edit.bmp");
            this.imageList1.Images.SetKeyName(89, "empty.bmp");
            this.imageList1.Images.SetKeyName(90, "even_odd_style.bmp");
            this.imageList1.Images.SetKeyName(91, "event.bmp");
            this.imageList1.Images.SetKeyName(92, "event_i.bmp");
            this.imageList1.Images.SetKeyName(93, "event_p.bmp");
            this.imageList1.Images.SetKeyName(94, "extension.bmp");
            this.imageList1.Images.SetKeyName(95, "female.bmp");
            this.imageList1.Images.SetKeyName(96, "field.bmp");
            this.imageList1.Images.SetKeyName(97, "field_i.bmp");
            this.imageList1.Images.SetKeyName(98, "field_p.bmp");
            this.imageList1.Images.SetKeyName(99, "file.bmp");
            this.imageList1.Images.SetKeyName(100, "find.bmp");
            this.imageList1.Images.SetKeyName(101, "findinfiles.bmp");
            this.imageList1.Images.SetKeyName(102, "findresults.bmp");
            this.imageList1.Images.SetKeyName(103, "fixed.bmp");
            this.imageList1.Images.SetKeyName(104, "folder.bmp");
            this.imageList1.Images.SetKeyName(105, "folder_open.bmp");
            this.imageList1.Images.SetKeyName(106, "folder1.bmp");
            this.imageList1.Images.SetKeyName(107, "folder2.bmp");
            this.imageList1.Images.SetKeyName(108, "folderclose.bmp");
            this.imageList1.Images.SetKeyName(109, "FolderList_Small.bmp");
            this.imageList1.Images.SetKeyName(110, "folderopen.bmp");
            this.imageList1.Images.SetKeyName(111, "font.bmp");
            this.imageList1.Images.SetKeyName(112, "font_color.bmp");
            this.imageList1.Images.SetKeyName(113, "footer1.bmp");
            this.imageList1.Images.SetKeyName(114, "footer2.bmp");
            this.imageList1.Images.SetKeyName(115, "form.bmp");
            this.imageList1.Images.SetKeyName(116, "glyph.bmp");
            this.imageList1.Images.SetKeyName(117, "grayed.bmp");
            this.imageList1.Images.SetKeyName(118, "high.bmp");
            this.imageList1.Images.SetKeyName(119, "horizontal_flip.bmp");
            this.imageList1.Images.SetKeyName(120, "icon.bmp");
            this.imageList1.Images.SetKeyName(121, "Inbox_Small.bmp");
            this.imageList1.Images.SetKeyName(122, "inspector_view.bmp");
            this.imageList1.Images.SetKeyName(123, "italic.bmp");
            this.imageList1.Images.SetKeyName(124, "Junk_Small.bmp");
            this.imageList1.Images.SetKeyName(125, "key.bmp");
            this.imageList1.Images.SetKeyName(126, "left.bmp");
            this.imageList1.Images.SetKeyName(127, "loadlayout.bmp");
            this.imageList1.Images.SetKeyName(128, "locals.bmp");
            this.imageList1.Images.SetKeyName(129, "long_time.bmp");
            this.imageList1.Images.SetKeyName(130, "long_zip_code.bmp");
            this.imageList1.Images.SetKeyName(131, "low.bmp");
            this.imageList1.Images.SetKeyName(132, "mail.bmp");
            this.imageList1.Images.SetKeyName(133, "Mail_Small.bmp");
            this.imageList1.Images.SetKeyName(134, "male.bmp");
            this.imageList1.Images.SetKeyName(135, "master.bmp");
            this.imageList1.Images.SetKeyName(136, "medium.bmp");
            this.imageList1.Images.SetKeyName(137, "method.bmp");
            this.imageList1.Images.SetKeyName(138, "method_i.bmp");
            this.imageList1.Images.SetKeyName(139, "method_p.bmp");
            this.imageList1.Images.SetKeyName(140, "middle.bmp");
            this.imageList1.Images.SetKeyName(141, "net.bmp");
            this.imageList1.Images.SetKeyName(142, "new.bmp");
            this.imageList1.Images.SetKeyName(143, "newproject.bmp");
            this.imageList1.Images.SetKeyName(144, "no.bmp");
            this.imageList1.Images.SetKeyName(145, "Note_Small.bmp");
            this.imageList1.Images.SetKeyName(146, "notes.bmp");
            this.imageList1.Images.SetKeyName(147, "numeric.bmp");
            this.imageList1.Images.SetKeyName(148, "open.bmp");
            this.imageList1.Images.SetKeyName(149, "openfavorite.bmp");
            this.imageList1.Images.SetKeyName(150, "openfile.bmp");
            this.imageList1.Images.SetKeyName(151, "opensolution.bmp");
            this.imageList1.Images.SetKeyName(152, "Outbox_Small.bmp");
            this.imageList1.Images.SetKeyName(153, "output.bmp");
            this.imageList1.Images.SetKeyName(154, "pagesetup.bmp");
            this.imageList1.Images.SetKeyName(155, "paste.bmp");
            this.imageList1.Images.SetKeyName(156, "phone.bmp");
            this.imageList1.Images.SetKeyName(157, "picture.bmp");
            this.imageList1.Images.SetKeyName(158, "position.bmp");
            this.imageList1.Images.SetKeyName(159, "postponed.bmp");
            this.imageList1.Images.SetKeyName(160, "print.bmp");
            this.imageList1.Images.SetKeyName(161, "print_designer.bmp");
            this.imageList1.Images.SetKeyName(162, "print_preview.bmp");
            this.imageList1.Images.SetKeyName(163, "priority.bmp");
            this.imageList1.Images.SetKeyName(164, "project.bmp");
            this.imageList1.Images.SetKeyName(165, "properties.bmp");
            this.imageList1.Images.SetKeyName(166, "properties1.bmp");
            this.imageList1.Images.SetKeyName(167, "read.bmp");
            this.imageList1.Images.SetKeyName(168, "read_header.bmp");
            this.imageList1.Images.SetKeyName(169, "records_view.bmp");
            this.imageList1.Images.SetKeyName(170, "redo.bmp");
            this.imageList1.Images.SetKeyName(171, "reference.bmp");
            this.imageList1.Images.SetKeyName(172, "referencesclose.bmp");
            this.imageList1.Images.SetKeyName(173, "referencesopen.bmp");
            this.imageList1.Images.SetKeyName(174, "refresh.bmp");
            this.imageList1.Images.SetKeyName(175, "rejected.bmp");
            this.imageList1.Images.SetKeyName(176, "replace.bmp");
            this.imageList1.Images.SetKeyName(177, "Report_Small.bmp");
            this.imageList1.Images.SetKeyName(178, "request.bmp");
            this.imageList1.Images.SetKeyName(179, "resx.bmp");
            this.imageList1.Images.SetKeyName(180, "right.bmp");
            this.imageList1.Images.SetKeyName(181, "rotate90.bmp");
            this.imageList1.Images.SetKeyName(182, "run.bmp");
            this.imageList1.Images.SetKeyName(183, "salon.bmp");
            this.imageList1.Images.SetKeyName(184, "save.bmp");
            this.imageList1.Images.SetKeyName(185, "saveall.bmp");
            this.imageList1.Images.SetKeyName(186, "savefile.bmp");
            this.imageList1.Images.SetKeyName(187, "savelayout.bmp");
            this.imageList1.Images.SetKeyName(188, "security.bmp");
            this.imageList1.Images.SetKeyName(189, "Sent_Small.bmp");
            this.imageList1.Images.SetKeyName(190, "short_time.bmp");
            this.imageList1.Images.SetKeyName(191, "short_zip_code.bmp");
            this.imageList1.Images.SetKeyName(192, "show_buttons.bmp");
            this.imageList1.Images.SetKeyName(193, "show_columns.bmp");
            this.imageList1.Images.SetKeyName(194, "show_focused_frame.bmp");
            this.imageList1.Images.SetKeyName(195, "show_footer.bmp");
            this.imageList1.Images.SetKeyName(196, "show_grid.bmp");
            this.imageList1.Images.SetKeyName(197, "show_horzlines.bmp");
            this.imageList1.Images.SetKeyName(198, "show_indent.bmp");
            this.imageList1.Images.SetKeyName(199, "show_indicator.bmp");
            this.imageList1.Images.SetKeyName(200, "show_preview.bmp");
            this.imageList1.Images.SetKeyName(201, "show_root.bmp");
            this.imageList1.Images.SetKeyName(202, "show_row_footer_summary.bmp");
            this.imageList1.Images.SetKeyName(203, "show_vertlines.bmp");
            this.imageList1.Images.SetKeyName(204, "showallfiles.bmp");
            this.imageList1.Images.SetKeyName(205, "solutionexplorer.bmp");
            this.imageList1.Images.SetKeyName(206, "sort_ascending.bmp");
            this.imageList1.Images.SetKeyName(207, "sort_descending.bmp");
            this.imageList1.Images.SetKeyName(208, "sport.bmp");
            this.imageList1.Images.SetKeyName(209, "start.bmp");
            this.imageList1.Images.SetKeyName(210, "string.bmp");
            this.imageList1.Images.SetKeyName(211, "Task_Small.bmp");
            this.imageList1.Images.SetKeyName(212, "tasklist.bmp");
            this.imageList1.Images.SetKeyName(213, "text.bmp");
            this.imageList1.Images.SetKeyName(214, "tile_horizontal.bmp");
            this.imageList1.Images.SetKeyName(215, "tile_vertical.bmp");
            this.imageList1.Images.SetKeyName(216, "toolbox.bmp");
            this.imageList1.Images.SetKeyName(217, "treelines_type.bmp");
            this.imageList1.Images.SetKeyName(218, "truck.bmp");
            this.imageList1.Images.SetKeyName(219, "unchecked.bmp");
            this.imageList1.Images.SetKeyName(220, "underline.bmp");
            this.imageList1.Images.SetKeyName(221, "undo.bmp");
            this.imageList1.Images.SetKeyName(222, "unknown.bmp");
            this.imageList1.Images.SetKeyName(223, "unread.bmp");
            this.imageList1.Images.SetKeyName(224, "usercontrol.bmp");
            this.imageList1.Images.SetKeyName(225, "vertical_flip.bmp");
            this.imageList1.Images.SetKeyName(226, "visa.bmp");
            this.imageList1.Images.SetKeyName(227, "watch.bmp");
            this.imageList1.Images.SetKeyName(228, "web.bmp");
            this.imageList1.Images.SetKeyName(229, "wheel.bmp");
            this.imageList1.Images.SetKeyName(230, "yes.bmp");
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ImageList = this.imageList1;
            this.btnClose.Location = new System.Drawing.Point(388, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLastRun
            // 
            this.lblLastRun.Location = new System.Drawing.Point(266, 45);
            this.lblLastRun.Name = "lblLastRun";
            this.lblLastRun.Size = new System.Drawing.Size(136, 18);
            this.lblLastRun.TabIndex = 5;
            this.lblLastRun.Text = "Middleware";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label4.Location = new System.Drawing.Point(262, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Last Run";
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            // 
            // StatusThread
            // 
            this.StatusThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Status_DoWork);
            this.StatusThread.WorkerSupportsCancellation = true;
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(109, 45);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(118, 18);
            this.lblState.TabIndex = 7;
            this.lblState.Text = "Idle";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.groupBox1.Controls.Add(this.lblLastRun);
            this.groupBox1.Controls.Add(this.lblState);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblJobStatus);
            this.groupBox1.Controls.Add(this.pb);
            this.groupBox1.Location = new System.Drawing.Point(35, 291);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 117);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Job Status";
            this.groupBox1.Visible = false;
            // 
            // listJobs
            // 
            this.listJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.JobName,
            this.Status,
            this.State,
            this.LastRun});
            this.listJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listJobs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listJobs.Location = new System.Drawing.Point(0, 0);
            this.listJobs.MultiSelect = false;
            this.listJobs.Name = "listJobs";
            this.listJobs.Size = new System.Drawing.Size(482, 182);
            this.listJobs.SmallImageList = this.imageList1;
            this.inboxControlStyler1.SetStyleSettings(this.listJobs, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.listJobs.TabIndex = 10;
            this.listJobs.UseCompatibleStateImageBehavior = false;
            this.listJobs.View = System.Windows.Forms.View.Details;
            this.listJobs.SelectedIndexChanged += new System.EventHandler(this.listJobs_SelectedIndexChanged);
            // 
            // JobName
            // 
            this.JobName.Text = "Job Name";
            this.JobName.Width = 165;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 100;
            // 
            // State
            // 
            this.State.Text = "State";
            this.State.Width = 100;
            // 
            // LastRun
            // 
            this.LastRun.Text = "Last Run";
            this.LastRun.Width = 120;
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(4, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(14, 14);
            this.lblName.TabIndex = 11;
            this.lblName.Text = "...";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.listJobs);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(482, 182);
            this.ultraPanel1.TabIndex = 12;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.lblName);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnStart);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Location = new System.Drawing.Point(4, 163);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(482, 46);
            this.ultraPanel2.TabIndex = 13;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmMiddleware_UltraFormManager_Dock_Area_Left
            // 
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.Name = "_frmMiddleware_UltraFormManager_Dock_Area_Left";
            this._frmMiddleware_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 182);
            // 
            // _frmMiddleware_UltraFormManager_Dock_Area_Right
            // 
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(486, 27);
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.Name = "_frmMiddleware_UltraFormManager_Dock_Area_Right";
            this._frmMiddleware_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 182);
            // 
            // _frmMiddleware_UltraFormManager_Dock_Area_Top
            // 
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.Name = "_frmMiddleware_UltraFormManager_Dock_Area_Top";
            this._frmMiddleware_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(490, 27);
            // 
            // _frmMiddleware_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 209);
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.Name = "_frmMiddleware_UltraFormManager_Dock_Area_Bottom";
            this._frmMiddleware_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(490, 4);
            // 
            // frmMiddleware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(490, 213);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._frmMiddleware_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmMiddleware_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmMiddleware_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmMiddleware_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMiddleware";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Middleware Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMiddleware_FormClosing);
            this.Load += new System.EventHandler(this.frmMiddleware_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel lblJobStatus;
        private System.Windows.Forms.ProgressBar pb;
        private Infragistics.Win.Misc.UltraButton btnStart;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblLastRun;
        private Infragistics.Win.Misc.UltraLabel label4;
        private System.ComponentModel.BackgroundWorker worker;
        private System.ComponentModel.BackgroundWorker StatusThread;
        private Infragistics.Win.Misc.UltraLabel lblState;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listJobs;
        private System.Windows.Forms.ColumnHeader JobName;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.ColumnHeader LastRun;
        private Infragistics.Win.Misc.UltraLabel lblName;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmMiddleware_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmMiddleware_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmMiddleware_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmMiddleware_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}