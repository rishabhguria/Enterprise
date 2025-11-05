using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.StringUtilities;
using Prana.CommonDataCache;
using Prana.Allocation.BLL;
namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for FundStrategiesDefaults.
	/// </summary>
	public class FundStrategiesDefaults : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid grdFundStrategy;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownFunds;
        private IContainer components;
        private Prana.BusinessObjects.FundCollection  _funds;		
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtDefaultName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ErrorProvider errorProviderDefault;
		private System.Windows.Forms.Label label2;
		private DataTable _dt;
		private int _userID=int.MinValue;
		Default _default;
		private object[] nullRow=new object[2];
		private bool bNullRowAdded=false;
        Prana.BusinessObjects.StrategyCollection  _companyStrategies;
		bool _bFund=true;
        

		public FundStrategiesDefaults(int userID,bool bFund)
		{
            InitControl(userID,bFund );
            
            
		}
        private void InitControl(int userID, bool bFund)
        {
            _bFund = bFund;
           
           
            _userID = userID;
            InitializeComponent();

            GetFundOrStrategyFromDB(bFund);
            nullRow[0] = int.MinValue;
            nullRow[1] = string.Empty;
            BindGrid();
            if (bFund)
                BindFunds();
            else
                BindStrategies();
            GridSettings();
            if (_bFund)
            {
                this.Text = "FundDefault";
            }
            else
            {
                this.Text = "StrategyDefault";
            }
        }
		public FundStrategiesDefaults(int userID,Default default1,bool bFund)
		{
			_default=default1;
            InitControl(userID, bFund);

            
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AllocationFund", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentage", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 2);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FundStrategiesDefaults));
            this.grdFundStrategy = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownFunds = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDefaultName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProviderDefault = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownFunds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            this.SuspendLayout();
            // 
            // grdFundStrategy
            // 
            this.grdFundStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdFundStrategy.DisplayLayout.Appearance = appearance1;
            this.grdFundStrategy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn1.TabStop = false;
            ultraGridColumn1.Width = 159;
            ultraGridColumn2.Header.Caption = "%";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 188;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.Header.Caption = "";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.NullText = "Delete";
            ultraGridColumn3.TabStop = false;
            ultraGridColumn3.Width = 81;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.grdFundStrategy.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdFundStrategy.DisplayLayout.CaptionAppearance = appearance2;
            this.grdFundStrategy.DisplayLayout.GroupByBox.Hidden = true;
            this.grdFundStrategy.DisplayLayout.MaxColScrollRegions = 1;
            this.grdFundStrategy.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdFundStrategy.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdFundStrategy.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdFundStrategy.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdFundStrategy.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdFundStrategy.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdFundStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdFundStrategy.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdFundStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFundStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdFundStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFundStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFundStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFundStrategy.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdFundStrategy.Location = new System.Drawing.Point(12, 38);
            this.grdFundStrategy.Name = "grdFundStrategy";
            this.grdFundStrategy.Size = new System.Drawing.Size(430, 150);
            this.grdFundStrategy.TabIndex = 15;
            this.grdFundStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdFundStrategy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdFundStrategy_MouseUp);
            this.grdFundStrategy.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdFundStrategy_CellChange);
            // 
            // ultraDropDownFunds
            // 
            this.ultraDropDownFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraDropDownFunds.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraDropDownFunds.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraDropDownFunds.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownFunds.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownFunds.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownFunds.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraDropDownFunds.Location = new System.Drawing.Point(330, 226);
            this.ultraDropDownFunds.Name = "ultraDropDownFunds";
            this.ultraDropDownFunds.Size = new System.Drawing.Size(108, 36);
            this.ultraDropDownFunds.TabIndex = 35;
            this.ultraDropDownFunds.Text = "ultraDropDown1";
            this.ultraDropDownFunds.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.DarkSalmon;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(254, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.BurlyWood;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(148, 196);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 23);
            this.btnSave.TabIndex = 36;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDefaultName
            // 
            this.txtDefaultName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtDefaultName.Location = new System.Drawing.Point(216, 8);
            this.txtDefaultName.Name = "txtDefaultName";
            this.txtDefaultName.Size = new System.Drawing.Size(96, 20);
            this.txtDefaultName.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(110, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 39;
            this.label1.Text = "Default Name";
            // 
            // errorProviderDefault
            // 
            this.errorProviderDefault.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(202, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 9);
            this.label2.TabIndex = 40;
            this.label2.Text = "*";
            // 
            // FundStrategiesDefaults
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(458, 269);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDefaultName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ultraDropDownFunds);
            this.Controls.Add(this.grdFundStrategy);
            this.Name = "FundStrategiesDefaults";
            this.ShowInTaskbar = false;
            this.Text = "FundStrategiesDefaults";
            ((System.ComponentModel.ISupportInitialize)(this.grdFundStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownFunds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void BindFunds()
		{
			try
			{
				
				ultraDropDownFunds.ValueMember = "FundID";
				ultraDropDownFunds.DisplayMember = "Name";
                ultraDropDownFunds.DataSource = null;
				ultraDropDownFunds.DataSource=_funds;
                ultraDropDownFunds.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                ultraDropDownFunds.DisplayLayout.Bands[0].Columns["FullName"].Hidden = true;
				grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].ValueList = ultraDropDownFunds;
                grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].Header.Caption = "Fund";
			}
			catch(Exception ex)
			{
				throw ex;
			}

			
		}
		private void BindStrategies()
		{
			try
			{
				ultraDropDownFunds.DisplayMember ="Name";
				ultraDropDownFunds.ValueMember="StrategyID";
                ultraDropDownFunds.DataSource = null;
				ultraDropDownFunds.DataSource=_companyStrategies;

                ultraDropDownFunds.DisplayLayout.Bands[0].Columns["StrategyID"].Hidden = true;
                ultraDropDownFunds.DisplayLayout.Bands[0].Columns["FullName"].Hidden = true;
                grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].ValueList = ultraDropDownFunds;
				grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].Header.Caption="Strategy";
			}
			catch(Exception ex)
			{
				throw ex;
			}

			
		}
		private void BindGrid()
		{
			try
			{
				
				
				_dt= new DataTable();
				_dt.Columns.Add("AllocationFund");
				_dt.Columns.Add("Percentage");
                //grdFundStrategy.DataSource = null;
                grdFundStrategy.DataSource = _dt;
				if(_default==null)
				{
                    CreateNewRow();
                   
				}
				else
				{
                    CreateRowForExistingData();
                    CreateNewRow(); 
				}

                grdFundStrategy.DataBind();
               
			}
			catch(Exception ex)
			{
				throw ex;
			}


			
		}

		private void grdFundStrategy_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if(e.Cell.Column.Key!="AllocationFund")
					return;
				
				string oldfundID= grdFundStrategy.ActiveCell.Value.ToString();				
				if(oldfundID==int.MinValue.ToString())
					bNullRowAdded=false;
				grdFundStrategy.UpdateData();				
				string fundID= grdFundStrategy.ActiveCell.Value.ToString();	
				int count=0;
				
			
				for(int i=0;i<_dt.Rows.Count;i++)
				{
					object[] obj= _dt.Rows[i].ItemArray; 
					if(fundID==obj[0].ToString() && fundID !=int.MinValue.ToString() ) //&& int.Parse(fundID)!= int.MinValue)
					{
						count++;
//						
					}
		
				}
				if(count>1)
				{
					fundID=oldfundID;
					grdFundStrategy.ActiveCell.Value=int.Parse(oldfundID);
					if(_bFund)
						MessageBox.Show("AllocationFund Already Exist");										
					else
						MessageBox.Show("Strategy Already Exist");										


				}
                    CreateNewRow();
			
			}
			
			catch(Exception ex)
			{
				throw ex;
			}

		
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(!ValidatePercentage())
				{
					MessageBox.Show("Enter Positive Numeric Percentages for AllocationFunds");
					return;
				}
				if(SumOfPercentage()!=100.0)
				{
					MessageBox.Show("Sum Of Percentages should be 100 %");
					return;

				}
				if(txtDefaultName.Text.Trim()==String.Empty)
				{
					MessageBox.Show("Please Enter Default Name");
					return;
				}

				DataTable  dtnew=(DataTable)grdFundStrategy.DataSource;
				string FundIDs=string.Empty;
				string Percentages=string.Empty;
			
				for(int i=0;i<_dt.Rows.Count;i++)
				{
					object[] obj= _dt.Rows[i].ItemArray; 
					string fundID=obj[0].ToString();
					if(fundID !=int.MinValue.ToString())
					{
						if(FundIDs==string.Empty)
						{
							FundIDs=fundID;
						}
						else
						{
							FundIDs=FundIDs +","+ fundID;
						}
						string percentage=obj[1].ToString();
					
						if(Percentages ==string.Empty)
						{
							Percentages=percentage;
						}
						else
						{
							Percentages=Percentages +","+ percentage;
						}
					}
			
		
				}
				_default= new Default();
                _default.DefaultID = AllocationIDGenerator.GenerateDefaultID();
				_default.DefaultName=txtDefaultName.Text;
				_default.FundIDs=FundIDs;
				_default.Percentages=Percentages;
				this.Hide();
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			
			this.Hide();
		
		}
	
		public Default Default
		{
			get{return _default ;}
			set{ _default=value ;}
		}
		private bool ValidatePercentage()
		{
			
			for(int i=0;i<_dt.Rows.Count;i++)
			{
				object[] obj= _dt.Rows[i].ItemArray; 
				string percentage=obj[1].ToString();
				if(obj[0].ToString()!=int.MinValue.ToString())
				{
					if(! RegularExpressionValidation.IsPositiveNumber(percentage))
					{
						return false;
					}
				}
				
			}
			return true;

		}
		private float SumOfPercentage()
		{
			float  sumOfpercentage=0;
			for(int i=0;i<_dt.Rows.Count;i++)
			{
				object[] obj= _dt.Rows[i].ItemArray; 
				string percentage=obj[1].ToString();
				if(obj[0].ToString()!=int.MinValue.ToString())
				{
					sumOfpercentage=sumOfpercentage+float.Parse(percentage);
					
				}
				
			}
			return sumOfpercentage;

		}
		private void GridSettings()
		{
			grdFundStrategy.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.Cursor = Cursors.Hand;
			grdFundStrategy.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.ForeColor = Color.Red;
			grdFundStrategy.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
		}

		private void grdFundStrategy_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            #region Variable Declaration
            UIElement objUIElement;
			UltraGridCell objUltraGridCell;		
			objUIElement = grdFundStrategy.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;
            #endregion

            if (objUltraGridCell.Text  == "Delete" )
			{
                if (MessageBox.Show(this, "Do you want to delete this Row?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteSelectedRow(objUltraGridCell.Row);

                    CreateNewRow();
                    
                }

			}
		
		}
        private void CreateNewRow()
        {
            int maxCount;
            if(_bFund)
                maxCount= _funds.Count;
            else
                maxCount = _companyStrategies.Count;
            if (maxCount == 0)
                return;
            int numberOfRows = _dt.Rows.Count;
            if (!bNullRowAdded && numberOfRows < maxCount - 1)
            {
                _dt.Rows.Add(nullRow);
                bNullRowAdded = true;
                numberOfRows++;

            }
            
            
            grdFundStrategy.DataBind();
            grdFundStrategy.DisplayLayout.Bands[0].Columns["delete"].NullText = "Delete";
            bool bShouldDeactivate = true ;
            foreach (UltraGridRow row in grdFundStrategy.Rows)
            {
                row.Cells["delete"].Value = "Delete";
                if (int.Parse(row.Cells["AllocationFund"].Value.ToString()) == int.MinValue)
                {
                    bShouldDeactivate = false ;
                }
            }
            if( bShouldDeactivate && numberOfRows == maxCount -1)
            {
            grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].CellActivation = Activation.Disabled;
            }
            else 
            {
                grdFundStrategy.DisplayLayout.Bands[0].Columns["AllocationFund"].CellActivation = Activation.AllowEdit;
            }
            if (numberOfRows != 0)
            {
                if (int.Parse(grdFundStrategy.Rows[numberOfRows - 1].Cells["AllocationFund"].Value.ToString()) == int.MinValue)
                {
                    grdFundStrategy.Rows[numberOfRows - 1].Cells["delete"].Value = "";
                }
                else
                {
                    grdFundStrategy.Rows[numberOfRows - 1].Cells["delete"].Value = "Delete";
                }
            }

        }
        private void CreateRowForExistingData()
        {
            string percentage = string.Empty;
            string[] FundIDS = _default.FundIDs.Split(',');
            string[] Percentages = _default.Percentages.Split(',');
            int i = 0;
            txtDefaultName.Text = _default.DefaultName;
            foreach (string fundID in FundIDS)
            {
                percentage = Percentages[i].ToString();
                object[] obj = new object[2];
                obj[0] = fundID;
                obj[1] = percentage;
                _dt.Rows.Add(obj);
                i++;
            }
            grdFundStrategy.DataBind();
            CreateNewRow();
            

        }
        private void DeleteSelectedRow(UltraGridRow selectedRow)
        {
            string fundID = selectedRow.Cells["AllocationFund"].Value.ToString();
            grdFundStrategy.ActiveRow.Delete(false);
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                object[] obj = _dt.Rows[i].ItemArray;
                if (fundID == obj[0].ToString())
                {
                    _dt.Rows.RemoveAt(i);
                }
            }

        }
        private void GetFundOrStrategyFromDB(bool bFund )
        {
            if (bFund)
            {
                _funds = CachedDataManager.GetInstance.GetUserFunds();
            }
            else
            {
                _companyStrategies = CachedDataManager.GetInstance.GetUserStrategies();
            }

        }
	}
}
