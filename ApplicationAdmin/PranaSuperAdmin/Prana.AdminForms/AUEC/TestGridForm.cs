using Infragistics.Win;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for TestGridForm.
    /// </summary>
    public class TestUserForm : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "TestUserForm : ";
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog ultraPrintPreviewDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdTestUser;
        private System.ComponentModel.IContainer components;
        public TestUserForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            BindUser();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (ultraPrintPreviewDialog1 != null)
                {
                    ultraPrintPreviewDialog1.Dispose();
                }
                if (textBox1 != null)
                {
                    textBox1.Dispose();
                }
                if (grdTestUser != null)
                {
                    grdTestUser.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MailingAddress", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneMobile", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LoginName", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Password", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephonePager", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsActive", 16, 33163454);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 17);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(33163454);
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ultraPrintPreviewDialog1 = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.grdTestUser = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdTestUser)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
            this.btnClose.Location = new System.Drawing.Point(336, 452);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
            this.btnSave.Location = new System.Drawing.Point(514, 454);
            this.btnSave.Name = "btnSave";
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraPrintPreviewDialog1
            // 
            this.ultraPrintPreviewDialog1.Name = "ultraPrintPreviewDialog1";
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(294, 372);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "textBox1";
            this.textBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver);
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            this.textBox1.DragLeave += new System.EventHandler(this.textBox1_DragLeave);
            // 
            // grdTestUser
            // 
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn16.Header.VisiblePosition = 15;
            ultraGridColumn17.Header.VisiblePosition = 16;
            ultraGridColumn18.Header.VisiblePosition = 17;
            ultraGridColumn18.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
                                                             ultraGridColumn1,
                                                             ultraGridColumn2,
                                                             ultraGridColumn3,
                                                             ultraGridColumn4,
                                                             ultraGridColumn5,
                                                             ultraGridColumn6,
                                                             ultraGridColumn7,
                                                             ultraGridColumn8,
                                                             ultraGridColumn9,
                                                             ultraGridColumn10,
                                                             ultraGridColumn11,
                                                             ultraGridColumn12,
                                                             ultraGridColumn13,
                                                             ultraGridColumn14,
                                                             ultraGridColumn15,
                                                             ultraGridColumn16,
                                                             ultraGridColumn17,
                                                             ultraGridColumn18});
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderColor = System.Drawing.Color.White;
            ultraGridBand1.Override.ActiveCellAppearance = appearance1;
            ultraGridBand1.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand1.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Raised;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            ultraGridBand1.Override.CellPadding = 2;
            ultraGridBand1.Override.CellSpacing = 0;
            ultraGridBand1.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            this.grdTestUser.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdTestUser.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdTestUser.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdTestUser.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdTestUser.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTestUser.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.grdTestUser.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdTestUser.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTestUser.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdTestUser.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdTestUser.DisplayLayout.Override.CellPadding = 0;
            this.grdTestUser.DisplayLayout.Override.CellSpacing = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Control;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.grdTestUser.DisplayLayout.Override.GroupByRowAppearance = appearance5;
            appearance6.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdTestUser.DisplayLayout.Override.HeaderAppearance = appearance6;
            this.grdTestUser.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdTestUser.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            this.grdTestUser.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdTestUser.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdTestUser.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTestUser.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            valueList1.Key = "VL1";
            valueListItem1.DataValue = 0;
            valueListItem1.DisplayText = "InActive";
            valueListItem2.DataValue = 1;
            valueListItem2.DisplayText = "Active";
            valueList1.ValueListItems.Add(valueListItem1);
            valueList1.ValueListItems.Add(valueListItem2);
            this.grdTestUser.DisplayLayout.ValueLists.AddRange(new Infragistics.Win.ValueList[] {
                                                                                                    valueList1});
            this.grdTestUser.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdTestUser.UseFlatMode = DefaultableBoolean.True;
            this.grdTestUser.Location = new System.Drawing.Point(4, 0);
            this.grdTestUser.Name = "grdTestUser";
            this.grdTestUser.Size = new System.Drawing.Size(884, 326);
            this.grdTestUser.TabIndex = 1;
            this.grdTestUser.TabStop = false;
            this.grdTestUser.Text = "grdTestUser";
            this.grdTestUser.Layout += new System.Windows.Forms.LayoutEventHandler(this.yy);
            this.grdTestUser.DragLeave += new System.EventHandler(this.grdTestUser_DragLeave);
            this.grdTestUser.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdTestUser_InitializeLayout);
            this.grdTestUser.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grdTestUser_SelectionDrag);
            // 
            // TestUserForm
            // 
            //this.AutoScale = false;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ClientSize = new System.Drawing.Size(892, 515);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdTestUser);
            this.Font = new System.Drawing.Font("Verdana", 8F);
            this.Name = "TestUserForm";
            this.Text = "TestGridForm";
            this.Load += new System.EventHandler(this.TestUserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdTestUser)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private void TestForm_Load()
        {
            BindUser();
        }
        private void BindUser()
        {
            Users users = new Users();
            users = UserManager.GetUsers();
            this.grdTestUser.DataSource = users;
            int i = 0;

            this.grdTestUser.ImageList = new System.Windows.Forms.ImageList();
            Image image = null;
            Image image1 = null;
            try
            {
                image = Image.FromFile("d:\\image1.bmp");
                image1 = Image.FromFile("d:\\image2.bmp");
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Error opening file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Add the image to the image list. 
            int imageIndex = this.grdTestUser.ImageList.Images.Add(image, Color.Transparent);
            imageIndex = this.grdTestUser.ImageList.Images.Add(image1, Color.Transparent);


            // Set the Image properties of various appearances to the index of the image 
            // in the ultra grid. 
            //			this.grdTestUser.DisplayLayout.Override.RowSelectorAppearance.Image = imageIndex; 
            //			this.grdTestUser.DisplayLayout.Override.HeaderAppearance.Image = imageIndex; 

            // You can also set the Image properties to the image itself. 
            //			this.grdTestUser.DisplayLayout.Override.CellAppearance.Image = this.grdTestUser.ImageList.Images[0]; 


            foreach (User user in users)
            {

                if (grdTestUser.Rows[i].Cells["IsActive"].Value.ToString() == "1")    // .ToString())== 1)
                {
                    grdTestUser.Rows[int.Parse(grdTestUser.Rows[i].Index.ToString())].Appearance.BackColor = System.Drawing.Color.SeaGreen;
                    grdTestUser.Rows[int.Parse(grdTestUser.Rows[i].Index.ToString())].Cells["LastName"].Appearance.Image = this.grdTestUser.ImageList.Images[1];

                    //		MessageBox.Show("This row is active Row No." + grdTestUser.Rows[i].Index.ToString()+ grdTestUser.Rows[i].Cells["FirstName"].Value.ToString());// "   ");// +int.Parse(grdTestUser.ActiveRow.Cells["IsActive"].Value.ToString())); //grdTestUser.ActiveRow.Cells["FirstName"].Value.ToString());    //Rows[i].Band.Columns["FirstName"].ToString());
                }
                else
                {
                    grdTestUser.Rows[int.Parse(grdTestUser.Rows[i].Index.ToString())].Appearance.BackColor = System.Drawing.Color.IndianRed;
                    //		MessageBox.Show("This row is INACTIVE Row No." + grdTestUser.Rows[i].Cells["FirstName"].Value.ToString());//.Cells["FirstName"].Value.ToString());   //grdTestUser.Rows[i].Band.Columns["FirstName"].ToString());
                    grdTestUser.Rows[int.Parse(grdTestUser.Rows[i].Index.ToString())].Cells["LastName"].Appearance.Image = this.grdTestUser.ImageList.Images[0];

                }
                i++;
            }


        }

        private void grdTestUser_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
        }
        private void TestUserForm_Load(object sender, System.EventArgs e)
        {
        }
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Users users = (Prana.Admin.BLL.Users)grdTestUser.DataSource;
                UserManager.SaveUsers(users);
                BindUser();
            }
            catch (Exception ex)
            {
                #region Catch
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
                #endregion
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }

        private void grdTestUser_DragLeave(object sender, System.EventArgs e)
        {
            //string symbol = ((Infragistics.Win.UltraWinGrid.UltraGrid)ultraTabControl1.SharedControls[0]).Rows[int.Parse(ultraGrid1.ActiveRow.Index.ToString())].Cells["LastName"].Value.ToString();
            //	string symbol = grdTestUser.Rows[int.Parse(grdTestUser.ActiveRow.Index.ToString())].Cells["LastName"].Value.ToString();
        }




        Symbolic symbolic = new Symbolic();
        private void grdTestUser_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
        {

            symbolic.Symbol = grdTestUser.Rows[int.Parse(grdTestUser.ActiveRow.Index.ToString())].Cells["LastName"].Value.ToString();
        }


        private void yy(object sender, System.Windows.Forms.LayoutEventArgs e)
        {

        }


        private void textBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            textBox1.Text = symbolic.Symbol.ToString();
        }

        private void textBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            textBox1.Text = symbolic.Symbol.ToString();


        }

        private void textBox1_DragLeave(object sender, System.EventArgs e)
        {
            textBox1.Text = symbolic.Symbol.ToString();

        }

        private void textBox1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {

            textBox1.Text = symbolic.Symbol.ToString();
        }
    }
}
