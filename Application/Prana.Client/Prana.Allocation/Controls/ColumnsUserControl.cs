using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Prana.Global;
using System.Collections.Generic;
using Prana.Allocation.BLL;
namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for Coulmns.
	/// </summary>
	public class ColumnsUserControl : System.Windows.Forms.UserControl 
	{
		private System.Windows.Forms.ListBox lbAvailableColumns;
		private System.Windows.Forms.ListBox lbDisplayColumns;
		private System.Windows.Forms.Button btnMoveRight;
		private System.Windows.Forms.Button btnMoveLeft;
		private System.Windows.Forms.Button brnMoveAllRight;
		private System.Windows.Forms.Button btnMoveAllLeft;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor ddlSortColumn;
		private System.Windows.Forms.RadioButton rbDesc;
		private System.Windows.Forms.RadioButton rbAsc;
		private System.Windows.Forms.Label label4;
		public  ArrayList availableColumnList;
		private System.Windows.Forms.ImageList imageList1;
		private GridColumns    allocationColumnPreferenceData;
		private string _gridType;
		private System.Windows.Forms.GroupBox groupBox1;
		
	
		
		private System.ComponentModel.IContainer components;

		public ColumnsUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		

		//Set Up for Binding Retrieved Data
		public void SetUp(GridColumns allocationColumnPreferenceData1,string gridType)
		{
			try
			{
				availableColumnList= new ArrayList();
				_gridType=gridType;
				allocationColumnPreferenceData=allocationColumnPreferenceData1;
			
				if(allocationColumnPreferenceData.Ascending)
					rbAsc.Checked= true;
				else
					rbDesc.Checked=true;
				#region Columns  Binding
				availableColumnList.Clear();

				if(_gridType==AllocationConstants.UNALLOCATED_GRID )
				{

                    foreach (string str in OrderFields.UnAllocaedDisplayColumns)
						availableColumnList.Add(str);
				}
				if(_gridType==AllocationConstants.GROUPED_GRID )
				{
                    foreach (string str in OrderFields.GroupedDisplayColumns)
						availableColumnList.Add(str);

				}
				if(_gridType==AllocationConstants.ALLOCATED_GRID)
				{
					foreach(string str in OrderFields.AllocatedDisplayColumns)
						availableColumnList.Add(str);

				}
				//BindAvailableColumns();
		
				ArrayList dispList= new ArrayList();
				foreach(DisplayColumn dispColumn in allocationColumnPreferenceData.DisplayColumns)
				{
					dispList.Add(dispColumn.DisplayName);
				}
				lbDisplayColumns.DataSource=dispList;			

				foreach(object obj in dispList)
					availableColumnList.Remove(obj);
				BindAvailableColumns();
//				if(availableColumnList.Count==0)
//					lbAvailableColumns.DataSource=null;			    
//				else
//				{
//					lbAvailableColumns.DataSource=null;			    
//					lbAvailableColumns.DataSource=availableColumnList;			    
//				}
				#endregion

				#region DropDown Binding
				FillSortDdl(allocationColumnPreferenceData.DisplayColumns );				
				int index=1;
				bool bMatch=false;
				foreach(DisplayColumn dispColumn in allocationColumnPreferenceData.DisplayColumns)
				{
					if(dispColumn.DisplayName.Equals(allocationColumnPreferenceData.SortKey))
					{
						bMatch=true;
						break;
					}
					index++;
				}
				if(bMatch)
					ddlSortColumn.SelectedIndex=index;
				else
					ddlSortColumn.SelectedIndex=0;
				#endregion
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColumnsUserControl));
			this.lbAvailableColumns = new System.Windows.Forms.ListBox();
			this.lbDisplayColumns = new System.Windows.Forms.ListBox();
			this.btnMoveRight = new System.Windows.Forms.Button();
			this.btnMoveLeft = new System.Windows.Forms.Button();
			this.brnMoveAllRight = new System.Windows.Forms.Button();
			this.btnMoveAllLeft = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ddlSortColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.rbDesc = new System.Windows.Forms.RadioButton();
			this.rbAsc = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumn)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbAvailableColumns
			// 
			this.lbAvailableColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lbAvailableColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbAvailableColumns.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbAvailableColumns.Location = new System.Drawing.Point(6, 46);
			this.lbAvailableColumns.Name = "lbAvailableColumns";
			this.lbAvailableColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbAvailableColumns.Size = new System.Drawing.Size(122, 184);
			this.lbAvailableColumns.TabIndex = 2;
			// 
			// lbDisplayColumns
			// 
			this.lbDisplayColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lbDisplayColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDisplayColumns.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbDisplayColumns.Location = new System.Drawing.Point(178, 46);
			this.lbDisplayColumns.Name = "lbDisplayColumns";
			this.lbDisplayColumns.Size = new System.Drawing.Size(118, 184);
			this.lbDisplayColumns.TabIndex = 1;
			// 
			// btnMoveRight
			// 
			this.btnMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnMoveRight.BackColor = System.Drawing.Color.Moccasin;
			this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveRight.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveRight.Image")));
			this.btnMoveRight.Location = new System.Drawing.Point(138, 46);
			this.btnMoveRight.Name = "btnMoveRight";
			this.btnMoveRight.Size = new System.Drawing.Size(30, 24);
			this.btnMoveRight.TabIndex = 50;
			this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
			// 
			// btnMoveLeft
			// 
			this.btnMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnMoveLeft.BackColor = System.Drawing.Color.Moccasin;
			this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLeft.Image")));
			this.btnMoveLeft.Location = new System.Drawing.Point(136, 196);
			this.btnMoveLeft.Name = "btnMoveLeft";
			this.btnMoveLeft.Size = new System.Drawing.Size(30, 24);
			this.btnMoveLeft.TabIndex = 51;
			this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
			// 
			// brnMoveAllRight
			// 
			this.brnMoveAllRight.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.brnMoveAllRight.BackColor = System.Drawing.Color.Moccasin;
			this.brnMoveAllRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.brnMoveAllRight.Image = ((System.Drawing.Image)(resources.GetObject("brnMoveAllRight.Image")));
			this.brnMoveAllRight.Location = new System.Drawing.Point(138, 88);
			this.brnMoveAllRight.Name = "brnMoveAllRight";
			this.brnMoveAllRight.Size = new System.Drawing.Size(30, 24);
			this.brnMoveAllRight.TabIndex = 48;
			this.brnMoveAllRight.Click += new System.EventHandler(this.brnMoveAllRight_Click);
			// 
			// btnMoveAllLeft
			// 
			this.btnMoveAllLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnMoveAllLeft.BackColor = System.Drawing.Color.Moccasin;
			this.btnMoveAllLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveAllLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveAllLeft.Image")));
			this.btnMoveAllLeft.Location = new System.Drawing.Point(136, 148);
			this.btnMoveAllLeft.Name = "btnMoveAllLeft";
			this.btnMoveAllLeft.Size = new System.Drawing.Size(30, 24);
			this.btnMoveAllLeft.TabIndex = 49;
			this.btnMoveAllLeft.Click += new System.EventHandler(this.btnMoveAllLeft_Click);
			// 
			// btnDown
			// 
			this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnDown.BackColor = System.Drawing.Color.Moccasin;
			this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
			this.btnDown.Location = new System.Drawing.Point(298, 136);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(30, 24);
			this.btnDown.TabIndex = 53;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnUp.BackColor = System.Drawing.Color.Moccasin;
			this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
			this.btnUp.Location = new System.Drawing.Point(298, 96);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(30, 24);
			this.btnUp.TabIndex = 52;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.label3.Location = new System.Drawing.Point(176, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 56;
			this.label3.Text = "Display Columns";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 55;
			this.label2.Text = "Available Columns";
			// 
			// ddlSortColumn
			// 
			this.ddlSortColumn.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ddlSortColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.ddlSortColumn.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
			this.ddlSortColumn.Location = new System.Drawing.Point(98, 236);
			this.ddlSortColumn.Name = "ddlSortColumn";
			this.ddlSortColumn.Size = new System.Drawing.Size(108, 19);
			this.ddlSortColumn.TabIndex = 61;
			this.ddlSortColumn.ValueChanged += new System.EventHandler(this.ddlSortColumn_ValueChanged);
			// 
			// rbDesc
			// 
			this.rbDesc.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbDesc.Checked = true;
			this.rbDesc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbDesc.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.rbDesc.Location = new System.Drawing.Point(272, 238);
			this.rbDesc.Name = "rbDesc";
			this.rbDesc.Size = new System.Drawing.Size(54, 16);
			this.rbDesc.TabIndex = 60;
			this.rbDesc.TabStop = true;
			this.rbDesc.Text = "DESC";
			// 
			// rbAsc
			// 
			this.rbAsc.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.rbAsc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbAsc.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.rbAsc.Location = new System.Drawing.Point(216, 238);
			this.rbAsc.Name = "rbAsc";
			this.rbAsc.Size = new System.Drawing.Size(54, 16);
			this.rbAsc.TabIndex = 59;
			this.rbAsc.Text = "ASC";
			this.rbAsc.CheckedChanged += new System.EventHandler(this.rbAsc_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.label4.Location = new System.Drawing.Point(10, 238);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 16);
			this.label4.TabIndex = 57;
			this.label4.Text = "Sort Column";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnMoveAllLeft);
			this.groupBox1.Controls.Add(this.lbDisplayColumns);
			this.groupBox1.Controls.Add(this.btnMoveRight);
			this.groupBox1.Controls.Add(this.btnUp);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.ddlSortColumn);
			this.groupBox1.Controls.Add(this.rbDesc);
			this.groupBox1.Controls.Add(this.rbAsc);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.btnMoveLeft);
			this.groupBox1.Controls.Add(this.btnDown);
			this.groupBox1.Controls.Add(this.brnMoveAllRight);
			this.groupBox1.Controls.Add(this.lbAvailableColumns);
			this.groupBox1.Location = new System.Drawing.Point(2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 260);
			this.groupBox1.TabIndex = 62;
			this.groupBox1.TabStop = false;
			this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// ColumnsUserControl
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.groupBox1);
			this.Name = "ColumnsUserControl";
			this.Size = new System.Drawing.Size(342, 264);
			this.Load += new System.EventHandler(this.ColumnsUserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumn)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private void BindAvailableColumns()
		{
			lbAvailableColumns.DataSource=availableColumnList;
		
		}

		#region Events
		private void btnMoveRight_Click(object sender, System.EventArgs e)
		{
			MoveItem(lbAvailableColumns,lbDisplayColumns);
			UpdateAllocationPrefrence(true); 
		}

		private void brnMoveAllRight_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsRight(lbAvailableColumns,lbDisplayColumns);
			UpdateAllocationPrefrence(true );
		}

		private void btnMoveAllLeft_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsLeft(lbDisplayColumns,lbAvailableColumns);
			UpdateAllocationPrefrence(true );
		}

		private void btnMoveLeft_Click(object sender, System.EventArgs e)
		{
			if (lbDisplayColumns.Items.Count > 1)
			{
				MoveItem(lbDisplayColumns,lbAvailableColumns); 
				UpdateAllocationPrefrence(true );
			}
			else
			{
				MessageBox.Show("Atleast one Column should be displayed!!");
				
			}
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			MoveUpDown(lbDisplayColumns,true);
			UpdateAllocationPrefrence(false);
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			MoveUpDown(lbDisplayColumns,false);
			UpdateAllocationPrefrence(false);
		}

		private void rbAsc_CheckedChanged(object sender, System.EventArgs e)
		{
			
			allocationColumnPreferenceData.Ascending = rbAsc.Checked; 
		}

		
	
		#endregion

        #region  MoveItem
		public  void MoveItem(ListBox source, ListBox destination )
		{
			if (source.Items.Count > 0)
			{
				System.Windows.Forms.ListBox.SelectedObjectCollection selected = source.SelectedItems;
				string[] list = new string[selected.Count];
				int index = 0;
			
				//destination.DataSource = null;
				ArrayList dsDestination = null;
				if ( destination.DataSource == null)
				{
					dsDestination = new ArrayList();
				}
				else
				{
					dsDestination  =(ArrayList) destination.DataSource;
				}

				// add to destination list
				foreach ( object obj in selected )
				{
					//destination.Items.Add(obj.ToString());
					list[index++] = obj.ToString();
					dsDestination.Add(obj.ToString());
				}
				destination.DataSource = null;
				destination.DataSource = dsDestination;
			

				// delete from source 
				ArrayList dsList = new ArrayList();
				foreach(object obj in source.Items)
				{
					dsList.Add(obj.ToString());
				}

				for(int i = 0;i<list.Length;i++)
				{
					if ( dsList.Contains(list[i]) )
					{
						dsList.Remove(list[i]); 
					}
				}

				source.DataSource = null;
				source.Items.Clear();
				source.DataSource = dsList;
			}
		}

		public  void MoveAllItemsRight(ListBox source, ListBox destination )
		{
			// copy source items in  a list
			object[] objects = new object[source.Items.Count + destination.Items.Count ]; 
			int index=0;
			foreach(object obj in source.Items)
			{
				objects[index++] = obj.ToString();
			}

			// add destination items in the list
			foreach(object obj in destination.Items)
			{
				objects[index++] = obj.ToString();
			}
			destination.DataSource=null;
			destination.Items.Clear();
			destination.Items.AddRange(objects);
			
			// delete from source 
			source.DataSource = null;
			source.Items.Clear();
		}
		public  void MoveAllItemsLeft(ListBox source, ListBox destination)
		{
			try
			{
				// copy source items in  a list
				object[] objects = new object[source.Items.Count + destination.Items.Count - 1]; 
				
				int index=0;
				int sourceCount = int.Parse(source.Items.Count.ToString());
				if (sourceCount !=1)
				{
					int i = 1;
					ArrayList arrList = new ArrayList();
					foreach(object obj in source.Items)
					{
						if (i <= sourceCount )
						{
							if(i != 1)
							{
								objects[index++] = obj.ToString();
							}
							else
							{
								arrList.Add(obj);
							}
						}
						else
						{
							break;
						}

						i++;
					}
				
				
					//				foreach(object obj in source.Items)
					//				{
					//					objects[index++] = obj.ToString();
					//				}

					// add destination items in the list
					foreach(object obj in destination.Items)
					{
						objects[index++] = obj.ToString();
					}
				
					//MessageBox.Show("AfterDestination" + sourceCount);
					destination.DataSource=null;
					destination.Items.Clear();
					destination.Items.AddRange(objects);
			
					// delete from source 
				
					source.DataSource = arrList;
				
					//source.Items.Clear();
				}
				else
				{
					MessageBox.Show("Atleast one Column should be displayed!!");
				}
			}
			catch(Exception)
			{
				#region Catch
				//				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				//				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
				//					FORM_NAME);
				//				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				//				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
				#endregion
			}
		}
	
		public  void MoveUpDown(ListBox listBox, bool moveUp )
		{
			if ( listBox.SelectedItem == null ) 
			{
				return;
			}

			string selectedItem = listBox.SelectedItem.ToString();
			int    selectedIndex = Convert.ToInt32(listBox.SelectedIndex); 
			
			// copy source items in a list
			ArrayList list = new ArrayList();
			foreach(object obj in listBox.Items)
			{
				list.Add(obj.ToString());
			}

			if ( moveUp && selectedIndex!=0)
			{
				list[selectedIndex]   = list[selectedIndex-1].ToString();
				list[selectedIndex-1] = selectedItem;
				selectedIndex--;
			}
			else if ( !moveUp && (selectedIndex != (listBox.Items.Count-1) ) )
			{
				list[selectedIndex]   = list[selectedIndex+1].ToString();
				list[selectedIndex+1] = selectedItem; 
				selectedIndex++;
			}
  
			listBox.DataSource = null;
			listBox.Items.Clear();
			listBox.DataSource = list;
			listBox.SelectedIndex = selectedIndex; 

		}
	
		#endregion
	
		private void UpdateAllocationPrefrence(bool isSortingToBeChanged)
		{
			
			ArrayList list = new ArrayList();
			if(allocationColumnPreferenceData==null)
				allocationColumnPreferenceData= new GridColumns();
			allocationColumnPreferenceData.ClearColumns();
			foreach(object obj in lbDisplayColumns.Items)
			{
				list.Add(obj.ToString());
				allocationColumnPreferenceData.AddColumn (obj.ToString());
			}
            
			string sortKey = string.Empty;
            if (isSortingToBeChanged)
           FillSortDdl(allocationColumnPreferenceData.DisplayColumns);
		
		}

		
	
		private void FillSortDdl(DisplayColumn[] displayList)
		{
			ddlSortColumn.Items.Clear();
			
			foreach( DisplayColumn obj in displayList)
			{
				Infragistics.Win.ValueListItem item = new Infragistics.Win.ValueListItem();
				item.DisplayText = obj.DisplayName;
				item.DataValue   = obj.DisplayName;
				ddlSortColumn.Items.Add(item);
			}

			ddlSortColumn.Items.Insert(0," --Select --",string.Empty);
			
			
			
		}

		
	
		public GridColumns AllocationColumnPreferenceData
				{
		
					get	{
						GridColumns newColumnData=(GridColumns) allocationColumnPreferenceData.Clone();
						return newColumnData;
						
						
						}
					
				
				
				
				
				}
	
		
		private void ddlSortColumn_ValueChanged(object sender, System.EventArgs e)
		{
			string sortKey="";
			
			if ( ddlSortColumn.SelectedItem != null )
			{
				sortKey = ddlSortColumn.SelectedItem.DisplayText; 
			}

					
			allocationColumnPreferenceData.SortKey = sortKey;
		}

		private void ColumnsUserControl_Load(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}
	
		
	}
}
