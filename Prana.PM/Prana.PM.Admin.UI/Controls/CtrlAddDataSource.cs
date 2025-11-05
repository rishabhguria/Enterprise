using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Prana.Utilities.UIUtilities;
using Prana.PM.BLL;
using Prana.PM.DAL;
//using Prana.PM.Common;
using Prana.BusinessObjects;
using Prana.PM.Admin.UI.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using Prana.PM.Common.Properties;
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.Admin.UI.Controls
{
	/// <summary>
	/// Summary description for CreateStrategy.
	/// </summary>
    public partial class CtrlAddDataSource : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnAdd_Previous;
		private System.Windows.Forms.GroupBox grpDataSource;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblShortName;
        private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.Button btnClose_Previous;
        private System.Windows.Forms.TextBox txtFullName;
		private System.Windows.Forms.Label lblIsFullNameEmpty;
		private System.Windows.Forms.Label lblIslShortNameEmpty;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbThirdParyList;

        public CtrlAddDataSource()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[AttributeProvider(typeof(IListSource))]
        public object DataSource
        {

            get { return bindingSourceAddDataSource.DataSource; }
            set { bindingSourceAddDataSource.DataSource = value; }
        }

        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[Editor("System.Windows.Forms.Design.DataMemberListEditor,System.Design, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get { return bindingSourceAddDataSource.DataMember; }
            set { bindingSourceAddDataSource.DataMember = value; }
        }
		

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlAddDataSource));
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            this.btnAdd_Previous = new System.Windows.Forms.Button();
            this.grpDataSource = new System.Windows.Forms.GroupBox();
            this.lblIslShortNameEmpty = new System.Windows.Forms.Label();
            this.lblIsFullNameEmpty = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblShortName = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnClose_Previous = new System.Windows.Forms.Button();
            this.grpImportBox = new System.Windows.Forms.GroupBox();
            this.lblThirdParty = new System.Windows.Forms.Label();
            this.cmbThirdParyList = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.chkBoxImportThirdParty = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.errProviderAddDataSource = new System.Windows.Forms.ErrorProvider(this.components);
            this.bindingSourceAddDataSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.grpDataSource.SuspendLayout();
            this.grpImportBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdParyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProviderAddDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAddDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd_Previous
            // 
            this.btnAdd_Previous.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd_Previous.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd_Previous.BackgroundImage")));
            this.btnAdd_Previous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd_Previous.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd_Previous.Location = new System.Drawing.Point(0, 168);
            this.btnAdd_Previous.Name = "btnAdd_Previous";
            this.btnAdd_Previous.Size = new System.Drawing.Size(75, 23);
            this.btnAdd_Previous.TabIndex = 2;
            this.btnAdd_Previous.UseVisualStyleBackColor = false;
            this.btnAdd_Previous.Visible = false;
            this.btnAdd_Previous.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grpDataSource
            // 
            this.grpDataSource.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpDataSource.Controls.Add(this.lblIslShortNameEmpty);
            this.grpDataSource.Controls.Add(this.lblIsFullNameEmpty);
            this.grpDataSource.Controls.Add(this.txtFullName);
            this.grpDataSource.Controls.Add(this.lblName);
            this.grpDataSource.Controls.Add(this.lblShortName);
            this.grpDataSource.Controls.Add(this.txtShortName);
            this.grpDataSource.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpDataSource.Location = new System.Drawing.Point(2, 86);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(266, 69);
            this.grpDataSource.TabIndex = 5;
            this.grpDataSource.TabStop = false;
            this.grpDataSource.Text = "New Data Source";
            this.grpDataSource.Enter += new System.EventHandler(this.grpDataSource_Enter);
            // 
            // lblIslShortNameEmpty
            // 
            this.lblIslShortNameEmpty.ForeColor = System.Drawing.Color.Red;
            this.lblIslShortNameEmpty.Location = new System.Drawing.Point(68, 44);
            this.lblIslShortNameEmpty.Name = "lblIslShortNameEmpty";
            this.lblIslShortNameEmpty.Size = new System.Drawing.Size(12, 8);
            this.lblIslShortNameEmpty.TabIndex = 35;
            this.lblIslShortNameEmpty.Text = "*";
            // 
            // lblIsFullNameEmpty
            // 
            this.lblIsFullNameEmpty.ForeColor = System.Drawing.Color.Red;
            this.lblIsFullNameEmpty.Location = new System.Drawing.Point(63, 21);
            this.lblIsFullNameEmpty.Name = "lblIsFullNameEmpty";
            this.lblIsFullNameEmpty.Size = new System.Drawing.Size(12, 8);
            this.lblIsFullNameEmpty.TabIndex = 35;
            this.lblIsFullNameEmpty.Text = "*";
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFullName.Location = new System.Drawing.Point(96, 18);
            this.txtFullName.MaxLength = 50;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(148, 21);
            this.txtFullName.TabIndex = 0;
            this.txtFullName.Enter += new System.EventHandler(this.txtDataSourceFullName_GotFocus);
            this.txtFullName.Leave += new System.EventHandler(this.txtDataSourceFullName_LostFocus);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(8, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(56, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Full Name";
            // 
            // lblShortName
            // 
            this.lblShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortName.Location = new System.Drawing.Point(8, 44);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(72, 14);
            this.lblShortName.TabIndex = 0;
            this.lblShortName.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(96, 40);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.Enter += new System.EventHandler(this.txtDataSourceShortName_GotFocus);
            this.txtShortName.Leave += new System.EventHandler(this.txtDataSourceShortName_LostFocus);
            // 
            // btnClose_Previous
            // 
            this.btnClose_Previous.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose_Previous.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose_Previous.BackgroundImage")));
            this.btnClose_Previous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose_Previous.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose_Previous.Location = new System.Drawing.Point(193, 168);
            this.btnClose_Previous.Name = "btnClose_Previous";
            this.btnClose_Previous.Size = new System.Drawing.Size(75, 23);
            this.btnClose_Previous.TabIndex = 3;
            this.btnClose_Previous.UseVisualStyleBackColor = false;
            this.btnClose_Previous.Visible = false;
            this.btnClose_Previous.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpImportBox
            // 
            this.grpImportBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpImportBox.Controls.Add(this.lblThirdParty);
            this.grpImportBox.Controls.Add(this.cmbThirdParyList);
            this.grpImportBox.Controls.Add(this.chkBoxImportThirdParty);
            this.grpImportBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpImportBox.Location = new System.Drawing.Point(3, 3);
            this.grpImportBox.Name = "grpImportBox";
            this.grpImportBox.Size = new System.Drawing.Size(266, 77);
            this.grpImportBox.TabIndex = 6;
            this.grpImportBox.TabStop = false;
            this.grpImportBox.Text = "Third Party";
            // 
            // lblThirdParty
            // 
            this.lblThirdParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblThirdParty.Location = new System.Drawing.Point(7, 51);
            this.lblThirdParty.Name = "lblThirdParty";
            this.lblThirdParty.Size = new System.Drawing.Size(56, 14);
            this.lblThirdParty.TabIndex = 2;
            this.lblThirdParty.Text = "Select ";
            this.lblThirdParty.Visible = false;
            // 
            // cmbThirdParyList
            // 
            this.cmbThirdParyList.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            this.cmbThirdParyList.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbThirdParyList.DisplayLayout.Appearance = appearance15;
            this.cmbThirdParyList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbThirdParyList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParyList.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdParyList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this.cmbThirdParyList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance18.BackColor2 = System.Drawing.SystemColors.Control;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbThirdParyList.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this.cmbThirdParyList.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbThirdParyList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbThirdParyList.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.SystemColors.Highlight;
            appearance20.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbThirdParyList.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this.cmbThirdParyList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbThirdParyList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParyList.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbThirdParyList.DisplayLayout.Override.CellAppearance = appearance22;
            this.cmbThirdParyList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbThirdParyList.DisplayLayout.Override.CellPadding = 0;
            appearance23.BackColor = System.Drawing.SystemColors.Control;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbThirdParyList.DisplayLayout.Override.GroupByRowAppearance = appearance23;
            appearance24.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbThirdParyList.DisplayLayout.Override.HeaderAppearance = appearance24;
            this.cmbThirdParyList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbThirdParyList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.Color.Silver;
            this.cmbThirdParyList.DisplayLayout.Override.RowAppearance = appearance25;
            this.cmbThirdParyList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbThirdParyList.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.cmbThirdParyList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbThirdParyList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbThirdParyList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbThirdParyList.DisplayMember = "";
            this.cmbThirdParyList.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbThirdParyList.Location = new System.Drawing.Point(95, 47);
            this.cmbThirdParyList.Name = "cmbThirdParyList";
            this.cmbThirdParyList.Size = new System.Drawing.Size(148, 23);
            this.cmbThirdParyList.TabIndex = 1;
            this.cmbThirdParyList.ValueMember = "";
            this.cmbThirdParyList.Visible = false;
            this.cmbThirdParyList.TextChanged += new System.EventHandler(this.cmbThirdParyList_TextChanged);
            this.cmbThirdParyList.ValueChanged += new System.EventHandler(this.cmbThirdParyList_ValueChanged);
            // 
            // chkBoxImportThirdParty
            // 
            this.chkBoxImportThirdParty.Location = new System.Drawing.Point(10, 21);
            this.chkBoxImportThirdParty.Name = "chkBoxImportThirdParty";
            this.chkBoxImportThirdParty.Size = new System.Drawing.Size(165, 20);
            this.chkBoxImportThirdParty.TabIndex = 0;
            this.chkBoxImportThirdParty.Text = "Import from Third Party";
            this.chkBoxImportThirdParty.CheckedChanged += new System.EventHandler(this.chkBoxImportThirdParty_CheckedChanged);
            // 
            // errProviderAddDataSource
            // 
            this.errProviderAddDataSource.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errProviderAddDataSource.ContainerControl = this;
            this.errProviderAddDataSource.DataSource = this.bindingSourceAddDataSource;
            // 
            // bindingSourceAddDataSource
            // 
            this.bindingSourceAddDataSource.DataSource = typeof(Prana.BusinessObjects.PositionManagement.DataSourceNameID);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance28.Image = ((object)(resources.GetObject("appearance28.Image")));
            appearance28.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnClose.Appearance = appearance28;
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(146, 176);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            appearance27.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnAdd.Appearance = appearance27;
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAdd.Location = new System.Drawing.Point(51, 176);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ShowFocusRect = false;
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // CtrlAddDataSource
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpImportBox);
            this.Controls.Add(this.btnAdd_Previous);
            this.Controls.Add(this.grpDataSource);
            this.Controls.Add(this.btnClose_Previous);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CtrlAddDataSource";
            this.Size = new System.Drawing.Size(272, 203);
            this.Load += new System.EventHandler(this.CtrlAddDataSource_Load);
            this.grpDataSource.ResumeLayout(false);
            this.grpDataSource.PerformLayout();
            this.grpImportBox.ResumeLayout(false);
            this.grpImportBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbThirdParyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProviderAddDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAddDataSource)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        bool isThirdPartyDropDownSet = false;

		#region Focus Colors
        private void txtDataSourceFullName_GotFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.LemonChiffon;
		}
        private void txtDataSourceFullName_LostFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.White;
		}
        private void txtDataSourceShortName_GotFocus(object sender, System.EventArgs e)
		{
            txtShortName.BackColor = Color.LemonChiffon;
		}
        private void txtDataSourceShortName_LostFocus(object sender, System.EventArgs e)
		{
            txtShortName.BackColor = Color.White;
		} 
		#endregion

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string shortName = txtShortName.Text.Trim();

            //if (fullName.Length == 0)
            //{
            //    lblIsFullNameEmpty.Visible = true;
            //}
            //else
            //{
            //    lblIsFullNameEmpty.Visible = false;
            //}
            //if (shortName.Length == 0)
            //{
            //    lblIslShortNameEmpty.Visible = true;
            //}
            //else
            //{
            //    lblIslShortNameEmpty.Visible = false;
            //}

            Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceName = new Prana.BusinessObjects.PositionManagement.DataSourceNameID();
            dataSourceName.FullName = fullName;
            dataSourceName.ShortName = shortName;
            
            //if (!lblIsFullNameEmpty.Visible && !lblIslShortNameEmpty.Visible)
            if(dataSourceName.IsValid == true)
            {
                //Populate new DataSource Object
                //DataSourceNameID dataSourceName = new DataSourceNameID();
                //dataSourceName.FullName = fullName;
                //dataSourceName.ShortName = shortName;

                if (selectedThirdParty != null)
                {
                    if ((selectedThirdParty.FullName != dataSourceName.FullName) && (selectedThirdParty.ShortName != dataSourceName.ShortName))
                    {
                        selectedThirdPartyID = 0;
                    }
                }

                int isSuccessfull = 0;

                try
                {
                    isSuccessfull = DataSourceManager.AddDataSource(dataSourceName, selectedThirdPartyID);
                    if (isSuccessfull != 0)
                    {
                        MessageBox.Show(this, "Data Source already exists !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(this, "Data Source added !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {

                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

                
                    this.FindForm().Close();
                              

            }

            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                // Close the Container Form
                this.FindForm().Close();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        public class Utility
        {
            public void CloseForm(Form form)
            {
                form.Close();
            }
        }

        private void CtrlAddDataSource_Load(object sender, EventArgs e)
        {
            //lblIsFullNameEmpty.Visible = false;
            //lblIslShortNameEmpty.Visible = false;

            //DataSourceNameIDList dataSourceNameIDList = dataSourceNameIDList;
            //bindingSourceAddDataSource
        }

        private void grpDataSource_Enter(object sender, EventArgs e)
        {

        }

        SortableSearchableList<ThirdPartyNameID> thirdPartyList = new SortableSearchableList<ThirdPartyNameID>();
        bool isInternalCall = false;
        /// <summary>
        /// Handles the CheckedChanged event of the chkBoxImportThirdParty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void chkBoxImportThirdParty_CheckedChanged(object sender, EventArgs e)
        {
            if (bool.Equals(chkBoxImportThirdParty.Checked, true))
            {
                grpImportBox.Height = 83;
                grpDataSource.Location = new System.Drawing.Point(2, 86);
                btnAdd.Location = new System.Drawing.Point(51, 176);
                btnClose.Location = new System.Drawing.Point(146, 176);
                this.FindForm().ClientSize = new System.Drawing.Size(281, 208);
                //grpDataSource.Location.X = 
                if (bool.Equals(isThirdPartyDropDownSet, false))
                {
                    thirdPartyList = PM.BLL.ThirdPartyNameID.RetrieveList();
                    isInternalCall = true;
                    cmbThirdParyList.DataSource = null;
                    cmbThirdParyList.DataSource = thirdPartyList;
                    cmbThirdParyList.ValueMember = "ID";
                    cmbThirdParyList.DisplayMember = "FullName";
                    cmbThirdParyList.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
                    cmbThirdParyList.Rows[0].Selected = true;
                    Utils.UltraComboFilter(cmbThirdParyList, "FullName");
                    isThirdPartyDropDownSet = true;
                    isInternalCall = false;
                }
                //grpDataSource.Size = System.Drawing.Size(266, 83);
                lblThirdParty.Visible = true;
                cmbThirdParyList.Visible = true;
            }
            else
            {
                grpImportBox.Height = 44;
                grpDataSource.Location = new System.Drawing.Point(2, 47);
                btnAdd.Location = new System.Drawing.Point(51, 137);
                btnClose.Location = new System.Drawing.Point(146, 137);
                this.FindForm().ClientSize = new System.Drawing.Size(281, 169);

                lblThirdParty.Visible = false;
                cmbThirdParyList.Visible = false;
                //txtFullName.Text = ""; BB
                //txtShortName.Text = ""; BB
                ((DataSourceNameID)(bindingSourceAddDataSource.DataSource)).FullName = ""; //BB
                ((DataSourceNameID)(bindingSourceAddDataSource.DataSource)).ShortName = ""; //BB
            }

        }

        
        /// <summary>
        /// Inits the control.
        /// </summary>
        public void InitControl()
        {
            

            
            
            Utils.UltraComboFilter(cmbThirdParyList, "FullName");

        }

        int selectedThirdPartyID = 0;
        ThirdPartyNameID selectedThirdParty = null;
        /// <summary>
        /// Handles the ValueChanged event of the cmbThirdParyList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbThirdParyList_ValueChanged(object sender, EventArgs e)
        {
            if (!isInternalCall)
            {
                if (cmbThirdParyList.Value != null)
                {
                    selectedThirdParty = (ThirdPartyNameID)cmbThirdParyList.SelectedRow.ListObject;
                    selectedThirdPartyID = selectedThirdParty.ID;

                    txtFullName.Text = selectedThirdParty.FullName;
                    txtShortName.Text = selectedThirdParty.ShortName;

                    ((DataSourceNameID)(bindingSourceAddDataSource.DataSource)).FullName = selectedThirdParty.FullName;
                    ((DataSourceNameID)(bindingSourceAddDataSource.DataSource)).ShortName = selectedThirdParty.ShortName;

                    //int isSuccessfull = DataSourceManager.ImportThirdPartyDataByThirdPartyID(thirdPartyID);

                    //if (isSuccessfull == 0)
                    //{
                    //    //CloseParentForm();
                    //}
                    //else if (isSuccessfull == 2627)
                    //{
                    //    MessageBox.Show(Constants.C_DUPLICATE_ENTRY);
                    //}
                    //else
                    //{

                    //    MessageBox.Show(Constants.C_FATAL_ERROR);
                    //    this.FindForm().Close();
                    //}
                }
            }
            
        }

        /// <summary>
        /// Closes the parent form.
        /// </summary>
        private void CloseParentForm()
        {
            MessageBox.Show("Added to DataBase.");
            
            this.FindForm().Close();
        }

        private void cmbThirdParyList_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This is called from the Parent Control of this control after the bindingSourceAddDataSource is populated with appropriate DataSource Object.
        /// Adding these bindings in the Constructor i.e. at design time throws the error that we can not bind to the property.
        /// Clearing datasources so that the same control's same property is not bound twice.
        /// </summary>
        public void PopulateDetails()
        {
            //Clear DataBindings and add them again for the controls. 
            this.txtFullName.DataBindings.Clear();
            this.txtFullName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddDataSource, "FullName", true));

            this.txtShortName.DataBindings.Clear();
            this.txtShortName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddDataSource, "ShortName", true));
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {

        }

		
	
		
	}
}
