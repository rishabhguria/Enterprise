using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Prana.Allocation.BLL;
namespace Prana.Allocation.Controls
{
	/// <summary>
	/// Summary description for FundStrategyControl.
	/// </summary>
	public class FundStrategyControl : System.Windows.Forms.UserControl
	{
		FundStrategies _fundStrategies=null;
		AllocationStrategies _companyStrategies=null;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdFundStrategy;
		private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownStrategies;
		private System.Windows.Forms.ErrorProvider errorProviderFundStrategy;
        private System.Windows.Forms.Label labelError;
        private IContainer components;

        private bool _isCurrentDate = true;

        public bool IsCurrentDate
        {
            get { return _isCurrentDate; }
            set { _isCurrentDate = value; }
        }

		public FundStrategyControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public void SetUp()
		{
			BindFundStrategy();
			BindStrategies();	
			RowsCollection rows = grdFundStrategy.Rows;
			try
			{
				foreach(UltraGridRow eachRow in rows)
				{
					int strategyID=int.Parse(eachRow.Cells["StrategyID"].Value.ToString());
				if(strategyID!=0)
						eachRow.Cells["Strategy"].Value=strategyID;
					else
						eachRow.Cells["Strategy"].Value=int.MinValue;
				
				}
//				ColumnsCollection columns = grdFundStrategy.DisplayLayout.Bands[0].Columns;
//				foreach (UltraGridColumn column in columns)
//				{
//				
//					if(column.Key != "Strategy")	
//						column.CellActivation=Activation.ActivateOnly;
//				
//				}
				grdFundStrategy.DisplayLayout.Bands[0].Columns["FundID"].Hidden=true;
				grdFundStrategy.DisplayLayout.Bands[0].Columns["StrategyName"].Hidden=true;
				grdFundStrategy.DisplayLayout.Bands[0].Columns["StrategyID"].Hidden=true;

				grdFundStrategy.DisplayLayout.Override.ActiveRowAppearance.BackColor=Color.LightYellow;
				grdFundStrategy.DisplayLayout.Override.ActiveRowAppearance.ForeColor=Color.Black;
				grdFundStrategy.DisplayLayout.Override.RowAppearance.BackColor=Color.White ;
				grdFundStrategy.DisplayLayout.Override.RowAppearance.BackColor2=Color.White;
				grdFundStrategy.DisplayLayout.Override.RowAppearance.ForeColor=Color.Black;


			}
			catch(Exception ex)
			{
				throw ex;
			}
	       
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Strategy", 0);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.grdFundStrategy = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownStrategies = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.errorProviderFundStrategy = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownStrategies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderFundStrategy)).BeginInit();
            this.SuspendLayout();
            // 
            // grdFundStrategy
            // 
            this.grdFundStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdFundStrategy.DisplayLayout.Appearance = appearance1;
            this.grdFundStrategy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 314;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
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
            this.grdFundStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdFundStrategy.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdFundStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFundStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdFundStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFundStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFundStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFundStrategy.Location = new System.Drawing.Point(0, 0);
            this.grdFundStrategy.Name = "grdFundStrategy";
            this.grdFundStrategy.Size = new System.Drawing.Size(316, 116);
            this.grdFundStrategy.TabIndex = 14;
            this.grdFundStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdFundStrategy.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdFundStrategy_InitializeLayout);
            // 
            // ultraDropDownStrategies
            // 
            this.ultraDropDownStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraDropDownStrategies.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraDropDownStrategies.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraDropDownStrategies.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownStrategies.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownStrategies.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraDropDownStrategies.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraDropDownStrategies.Location = new System.Drawing.Point(204, 124);
            this.ultraDropDownStrategies.Name = "ultraDropDownStrategies";
            this.ultraDropDownStrategies.Size = new System.Drawing.Size(108, 36);
            this.ultraDropDownStrategies.TabIndex = 34;
            this.ultraDropDownStrategies.Text = "ultraDropDown1";
            this.ultraDropDownStrategies.Visible = false;
            // 
            // errorProviderFundStrategy
            // 
            this.errorProviderFundStrategy.ContainerControl = this;
            // 
            // labelError
            // 
            this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelError.Location = new System.Drawing.Point(16, 134);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(12, 18);
            this.labelError.TabIndex = 35;
            // 
            // FundStrategyControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.ultraDropDownStrategies);
            this.Controls.Add(this.grdFundStrategy);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "FundStrategyControl";
            this.Size = new System.Drawing.Size(316, 158);
            ((System.ComponentModel.ISupportInitialize)(this.grdFundStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownStrategies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderFundStrategy)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void grdFundStrategy_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
//			
		}
		private void BindFunds()
	    {

		
		}
		private void BindStrategies()
		{
            // _companyStrategies= new AllocationStrategies();
            //ultraDropDownStrategies.ValueMember = "StrategyID";
            //ultraDropDownStrategies.DisplayMember = "StrategyName";
            //_companyStrategies=CompanyStrategyManager.GetAllStrategies();;				
            //_companyStrategies.Insert(0,new AllocationStrategy(int.MinValue,C_COMBO_SELECT));
            //ultraDropDownStrategies.DataSource=_companyStrategies;
            //ColumnsCollection columns = ultraDropDownStrategies.DisplayLayout.Bands[0].Columns;
            //foreach (UltraGridColumn column in columns)
            //{
            //    if(column.Key != "StrategyName")
            //    {
            //        column.Hidden = true;
            //    }
            //}
            ////if(companyStrategies.Count >=1)
            //grdFundStrategy.DisplayLayout.Bands[0].Columns["Strategy"].ValueList = ultraDropDownStrategies;
			
		}

		private void BindFundStrategy()
		{
			try
			{
				_fundStrategies=FundStraegyManager.GetFundStrategy();				
				if(_fundStrategies.Count !=0)
				{
                    grdFundStrategy.DataSource = null;
					grdFundStrategy.DataSource=_fundStrategies;
					grdFundStrategy.DataBind();
				}
				else 
				{
                    grdFundStrategy.DataSource = null;
					grdFundStrategy.DataSource=_fundStrategies;

				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
        //TBR
		private void btnAddNew_Click(object sender, System.EventArgs e)
		{
		
		}

		public FundStrategies GetFundStrategies()
		{
			errorProviderFundStrategy.SetError(labelError,"");
			

			FundStrategies  fundStrategies=(FundStrategies)grdFundStrategy.DataSource;
			FundStrategies deleted= new FundStrategies();
			bool bMatch=false;
			foreach(UltraGridRow row in grdFundStrategy.Rows )
			{
				bMatch=false;
				foreach(AllocationStrategy   temp in _companyStrategies)
				{
					if(row.Cells["Strategy"].Text.Trim()== temp.StrategyName)
					{
						bMatch=true;
						break;

					}
				}
				if(bMatch)
				{

					if(int.Parse(row.Cells["Strategy"].Value.ToString())==int.MinValue)
					{
						row.Cells["StrategyID"].Value=0;
						errorProviderFundStrategy.SetError(labelError,"All AllocationFunds should be Associated with Strategies");
					
						return null;
					
					}
					else
					{
					
					

						row.Cells["StrategyID"].Value=row.Cells["Strategy"].Value;
					}
				
				}
				else
				{
				errorProviderFundStrategy.SetError(labelError,"Please Enter Correct Value of Strategy");
					return null;
				}
			}
			return fundStrategies;
		}

		

		
		
	

		
		
			
			
	}
}
