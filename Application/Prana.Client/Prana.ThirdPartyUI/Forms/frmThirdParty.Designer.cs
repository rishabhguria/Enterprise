using Prana.ThirdPartyReport;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.ThirdPartyUI
{
    partial class frmThirdParty
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThirdParty));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsToggleBatch = new System.Windows.Forms.ToolStripButton();
            this.RunDate = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.recordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.splitContainer1 = new System.Windows.Forms.MySplitContainer();
            this.GrdJob = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataView = new Prana.ThirdPartyReport.Controls.ThirdPartyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.thirdPartyGrid1 = new Prana.ThirdPartyReport.Controls.ThirdPartyGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chkLstAuec = new System.Windows.Forms.CheckedListBox();
            this.chkLstThirdPartyAccounts = new System.Windows.Forms.CheckedListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmThirdParty_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdParty_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdParty_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.thirdPartyBatchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdJob)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyGrid1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyBatchBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus});
            this.statusStrip1.Location = new System.Drawing.Point(8, 589);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1058, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tsStatus.Image = ((System.Drawing.Image)(resources.GetObject("tsStatus.Image")));
            this.tsStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(55, 17);
            this.tsStatus.Text = "Status";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLoad,
            this.toolStripSeparator3,
            this.btnView,
            this.toolStripSeparator1,
            this.btnExport,
            this.toolStripSeparator2,
            this.btnSend,
            this.toolStripSeparator4,
            this.tsToggleBatch});
            this.toolStrip1.Location = new System.Drawing.Point(8, 56);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1058, 25);
            this.inboxControlStyler1.SetStyleSettings(this.toolStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsLoad
            // 
            this.tsLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsLoad.Image")));
            this.tsLoad.Name = "tsLoad";
            this.tsLoad.Size = new System.Drawing.Size(53, 22);
            this.tsLoad.Text = "Load";
            this.tsLoad.Visible = false;
            this.tsLoad.Click += new System.EventHandler(this.tsLoad_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // btnView
            // 
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(52, 22);
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            this.btnView.AccessibleName = "toolStrip_ViewButton";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(74, 22);
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnExport.AccessibleName = "toolStrip_ExportButton";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSend
            // 
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(53, 22);
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.AccessibleName = "toolStrip_SendButton";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Visible = false;
            // 
            // tsToggleBatch
            // 
            this.tsToggleBatch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tsToggleBatch.CheckOnClick = true;
            this.tsToggleBatch.Image = ((System.Drawing.Image)(resources.GetObject("tsToggleBatch.Image")));
            this.tsToggleBatch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsToggleBatch.Name = "tsToggleBatch";
            this.tsToggleBatch.Size = new System.Drawing.Size(145, 22);
            this.tsToggleBatch.Text = "Show/Hide Active Jobs";
            this.tsToggleBatch.Visible = false;
            this.tsToggleBatch.Click += new System.EventHandler(this.tsToggleBatch_Click);
            // 
            // RunDate
            // 
            this.RunDate.Location = new System.Drawing.Point(239, 58);
            this.RunDate.Name = "RunDate";
            this.RunDate.Size = new System.Drawing.Size(183, 20);
            this.inboxControlStyler1.SetStyleSettings(this.RunDate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.RunDate.TabIndex = 3;
            this.RunDate.ValueChanged += new EventHandler(this.tsLoad_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.tableManagerToolStripMenuItem,
            this.fileLogToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(8, 32);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1058, 24);
            this.inboxControlStyler1.SetStyleSettings(this.menuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // recordsToolStripMenuItem
            // 
            this.recordsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.recordsToolStripMenuItem.Name = "recordsToolStripMenuItem";
            this.recordsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.recordsToolStripMenuItem.Text = "Records";
            this.recordsToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // tableManagerToolStripMenuItem
            // 
            this.tableManagerToolStripMenuItem.Name = "tableManagerToolStripMenuItem";
            this.tableManagerToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.tableManagerToolStripMenuItem.Text = "Setup";
            this.tableManagerToolStripMenuItem.Click += new System.EventHandler(this.tableManagerToolStripMenuItem_Click);
            //
            // fileLogToolStripMenuItem
            //
            this.fileLogToolStripMenuItem.Name = "fileLogToolStripMenuItem";
            this.fileLogToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.fileLogToolStripMenuItem.Text = "File Log";
            this.fileLogToolStripMenuItem.Click += new System.EventHandler(this.FileLogToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(8, 81);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.GrdJob);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1058, 508);
            this.splitContainer1.SplitterDistance = 223;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 0;
            // 
            // GrdJob
            // 
            this.GrdJob.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.GrdJob.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.GrdJob.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.GrdJob.DisplayLayout.MaxColScrollRegions = 1;
            this.GrdJob.DisplayLayout.MaxRowScrollRegions = 1;
            this.GrdJob.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.GrdJob.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.GrdJob.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.GrdJob.DisplayLayout.Override.CellPadding = 0;
            this.GrdJob.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.GrdJob.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.GrdJob.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.GrdJob.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.GrdJob.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.GrdJob.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.GrdJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrdJob.Location = new System.Drawing.Point(0, 0);
            this.GrdJob.Name = "GrdJob";
            this.GrdJob.Size = new System.Drawing.Size(1058, 223);
            this.GrdJob.TabIndex = 1;
            this.GrdJob.Text = "GrdJob";
            this.GrdJob.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.GrdJob_InitializeLayout);
            this.GrdJob.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.GrdJob_ClickCellButton);
            this.GrdJob.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.GrdJob_Error);
            this.GrdJob.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.GrdJob_ClickCell);
            this.GrdJob.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.GrdJob_AfterCellUpdate);
            this.GrdJob.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.GrdJob_InitializeRow);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            //this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1058, 281);
            this.inboxControlStyler1.SetStyleSettings(this.tabControl1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataView);
            this.tabPage1.ImageIndex = 217;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.tabPage1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Active Tax Lots";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataView
            // 
            this.dataView.DataSource = null;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(3, 3);
            this.dataView.Name = "dataView";
            this.dataView.Size = new System.Drawing.Size(1044, 248);
            this.dataView.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.thirdPartyGrid1);
            this.tabPage2.ImageIndex = 217;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.tabPage2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Ignored Tax Lots";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // thirdPartyGrid1
            // 
            this.thirdPartyGrid1.DataSource = null;
            this.thirdPartyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirdPartyGrid1.Location = new System.Drawing.Point(3, 3);
            this.thirdPartyGrid1.Name = "thirdPartyGrid1";
            this.thirdPartyGrid1.Size = new System.Drawing.Size(1044, 248);
            this.thirdPartyGrid1.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkLstAuec);
            this.tabPage3.Controls.Add(this.chkLstThirdPartyAccounts);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.ImageIndex = 222;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.tabPage3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Accounts";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkLstAuec
            // 
            this.chkLstAuec.FormattingEnabled = true;
            this.chkLstAuec.Location = new System.Drawing.Point(384, 27);
            this.chkLstAuec.Name = "chkLstAuec";
            this.chkLstAuec.Size = new System.Drawing.Size(167, 124);
            this.inboxControlStyler1.SetStyleSettings(this.chkLstAuec, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkLstAuec.TabIndex = 27;
            // 
            // chkLstThirdPartyAccounts
            // 
            this.chkLstThirdPartyAccounts.FormattingEnabled = true;
            this.chkLstThirdPartyAccounts.Location = new System.Drawing.Point(19, 27);
            this.chkLstThirdPartyAccounts.Name = "chkLstThirdPartyAccounts";
            this.chkLstThirdPartyAccounts.Size = new System.Drawing.Size(167, 124);
            this.inboxControlStyler1.SetStyleSettings(this.chkLstThirdPartyAccounts, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkLstThirdPartyAccounts.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(381, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label8, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label8.TabIndex = 24;
            this.label8.Text = "AUEC Selection";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label5.TabIndex = 22;
            this.label5.Text = "Fund Accounts";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtOutput);
            this.tabPage4.ImageIndex = 180;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.tabPage4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Output";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.txtOutput, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtOutput.TabIndex = 2;
            this.txtOutput.Text = "";
            this.txtOutput.WordWrap = false;
            this.txtOutput.Click += new System.EventHandler(this.OutPut_txtOuputClick);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtLog);
            this.tabPage5.ImageIndex = 159;
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.tabPage5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Log";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1050, 254);
            this.inboxControlStyler1.SetStyleSettings(this.txtLog, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtLog.TabIndex = 2;
            this.txtLog.Text = "";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmThirdParty_UltraFormManager_Dock_Area_Left
            // 
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.Name = "_frmThirdParty_UltraFormManager_Dock_Area_Left";
            this._frmThirdParty_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 579);
            // 
            // _frmThirdParty_UltraFormManager_Dock_Area_Right
            // 
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1066, 32);
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.Name = "_frmThirdParty_UltraFormManager_Dock_Area_Right";
            this._frmThirdParty_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 579);
            // 
            // _frmThirdParty_UltraFormManager_Dock_Area_Top
            // 
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.Name = "_frmThirdParty_UltraFormManager_Dock_Area_Top";
            this._frmThirdParty_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1074, 32);
            // 
            // _frmThirdParty_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 611);
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.Name = "_frmThirdParty_UltraFormManager_Dock_Area_Bottom";
            this._frmThirdParty_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1074, 8);
            // 
            // thirdPartyBatchBindingSource
            // 
            this.thirdPartyBatchBindingSource.DataSource = typeof(Prana.BusinessObjects.ThirdPartyBatch);
            // 
            // frmThirdParty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 619);
            this.Controls.Add(this.RunDate);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._frmThirdParty_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmThirdParty_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmThirdParty_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmThirdParty_UltraFormManager_Dock_Area_Bottom);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmThirdParty";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Third Party Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmThirdParty_FormClosing);
            this.Load += new System.EventHandler(this.frmThirdParty_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdJob)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyGrid1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyBatchBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MySplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private ThirdPartyReport.Controls.ThirdPartyGrid dataView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSend;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem recordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ImageList imageList1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn thirdPartyTypeNameDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn thirdPartyNameIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn formatTypeIdDataGridViewTextBoxColumn;
        private ThirdPartyReport.Controls.ThirdPartyGrid thirdPartyGrid1;
        private System.Windows.Forms.CheckedListBox chkLstAuec;
        private System.Windows.Forms.CheckedListBox chkLstThirdPartyAccounts;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.BindingSource thirdPartyBatchBindingSource;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn isLevel2DataDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton tsToggleBatch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn thirdPartyTypeIdDataGridViewTextBoxColumn;
        public System.Windows.Forms.DateTimePicker RunDate;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdParty_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdParty_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdParty_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdParty_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ToolStripButton tsLoad;
        private System.Windows.Forms.ToolStripMenuItem tableManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private PranaUltraGrid GrdJob;
    }
}