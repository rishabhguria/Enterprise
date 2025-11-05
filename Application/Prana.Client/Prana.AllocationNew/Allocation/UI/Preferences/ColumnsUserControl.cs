using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Prana.Global;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
namespace Prana.AllocationNew
{
	/// <summary>
	/// Summary description for Coulmns.
	/// </summary>
	public class ColumnsUserControl : System.Windows.Forms.UserControl 
	{
		private System.Windows.Forms.ListBox lstbxLeft;
		private System.Windows.Forms.ListBox lstbxRight;
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
		
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.GroupBox groupBox1;


        public List<string> _leftList;
        public List<string> _rightList;
        string sortKey = "";
        bool _ascending = false;
		private System.ComponentModel.IContainer components;

		public ColumnsUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		

		//Set Up for Binding Retrieved Data
        public void SetUp(List<string> leftList, List<string> rightList,bool ascending)
		{
			try
			{
                _leftList = CopyData(leftList);
                _rightList = CopyData(rightList);
                foreach (string leftData in _rightList)
                {
                    _leftList.Remove(leftData);
                }
                if (ascending)
					rbAsc.Checked= true;
				else
					rbDesc.Checked=true;

                lstbxRight.DataSource = _rightList;			
                lstbxLeft.DataSource = _leftList;

				#region DropDown Binding
                FillSortDdl();				
				#endregion
			}
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
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
            this.ddlSortColumn.ValueChanged -= new System.EventHandler(this.ddlSortColumn_ValueChanged);
            this.ddlSortColumn.Dispose();
            this.ddlSortColumn = null;
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
			this.lstbxLeft = new System.Windows.Forms.ListBox();
			this.lstbxRight = new System.Windows.Forms.ListBox();
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
			// lstbxLeft
			// 
			this.lstbxLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lstbxLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstbxLeft.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lstbxLeft.Location = new System.Drawing.Point(6, 46);
			this.lstbxLeft.Name = "lstbxLeft";
			this.lstbxLeft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstbxLeft.Size = new System.Drawing.Size(122, 184);
			this.lstbxLeft.TabIndex = 2;
			// 
			// lstbxRight
			// 
			this.lstbxRight.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lstbxRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstbxRight.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lstbxRight.Location = new System.Drawing.Point(178, 46);
			this.lstbxRight.Name = "lstbxRight";
			this.lstbxRight.Size = new System.Drawing.Size(118, 184);
			this.lstbxRight.TabIndex = 1;
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
			this.groupBox1.Controls.Add(this.lstbxRight);
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
			this.groupBox1.Controls.Add(this.lstbxLeft);
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
		

		#region Events
		private void btnMoveRight_Click(object sender, System.EventArgs e)
		{
			MoveRight();
            FillSortDdl();
		}

		private void brnMoveAllRight_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsRight();
            FillSortDdl();
		}

		private void btnMoveAllLeft_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsLeft();
            FillSortDdl();
		}

		private void btnMoveLeft_Click(object sender, System.EventArgs e)
		{
			if (lstbxRight.Items.Count > 1)
			{
				MoveLeft();
                FillSortDdl();
			}
			else
			{
				MessageBox.Show("Atleast one Column should be displayed!!");
				
			}
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			MoveUpDown(lstbxRight,true);
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			MoveUpDown(lstbxRight,false);
		}

		private void rbAsc_CheckedChanged(object sender, System.EventArgs e)
		{
			
			_ascending = rbAsc.Checked; 
		}

		
	
		#endregion

        #region  MoveItem
        public void MoveLeft()
        {
            List<string> selectedvalues = GetSelectedItems(lstbxRight);
            RemoveSelected(_rightList, selectedvalues);
            AddSelected(_leftList, selectedvalues);
            BindDataColumns();
        }
        private void BindDataColumns()
        {
            lstbxLeft.DataSource = null;
            lstbxLeft.DataSource = _leftList;

            lstbxRight.DataSource = null;
            lstbxRight.DataSource = _rightList;
        }
        public void MoveRight()
        {
            List<string> selectedvalues = GetSelectedItems(lstbxLeft);
            RemoveSelected(_leftList, selectedvalues);
            AddSelected(_rightList, selectedvalues);
            BindDataColumns();
        }
        private void RemoveSelected(List<string> source, List<string> selectedvalues)
        {
            foreach (string value in selectedvalues)
            {
                source.Remove(value);
            }
        }
        private void AddSelected(List<string> source, List<string> selectedvalues)
        {
            foreach (string value in selectedvalues)
            {
                source.Add(value);
            }
        }
        private List<string> GetSelectedItems(ListBox source)
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection selected = source.SelectedItems;
            List<string> list = new List<string>();
            foreach (object obj in selected)
            {
                list.Add(obj.ToString());
            }
            return list;
        }
		public  void MoveAllItemsRight()
		{
            List<string> selectedvalues = CopyData(_leftList);
            RemoveSelected(_leftList, selectedvalues);
            AddSelected(_rightList , selectedvalues);
            BindDataColumns();
		}
        public void MoveAllItemsLeft()
        {
            List<string> selectedvalues = CopyData(_rightList);
            RemoveSelected(_rightList, selectedvalues);
            AddSelected(_leftList, selectedvalues);
            BindDataColumns();
        }
        private List<string> CopyData(List<string> data)
        {
            List<string> newData = new List<string>();
            foreach (string item in data)
            {
                newData.Add(item);
            }
            return newData;
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
           

			if ( moveUp && selectedIndex!=0)
			{
                _rightList[selectedIndex] = _rightList[selectedIndex - 1].ToString();
                _rightList[selectedIndex - 1] = selectedItem;
				selectedIndex--;
			}
			else if ( !moveUp && (selectedIndex != (listBox.Items.Count-1) ) )
			{
                _rightList[selectedIndex] = _rightList[selectedIndex + 1].ToString();
                _rightList[selectedIndex + 1] = selectedItem; 
				selectedIndex++;
			}

            BindDataColumns();
			listBox.SelectedIndex = selectedIndex; 

		}
	
		#endregion
	
	

		
	
		private void FillSortDdl()
		{
			ddlSortColumn.Items.Clear();
			
			foreach( string  column in _rightList)
			{
				Infragistics.Win.ValueListItem item = new Infragistics.Win.ValueListItem();
                item.DisplayText = column;
                item.DataValue = column;
				ddlSortColumn.Items.Add(item);
			}

            ddlSortColumn.Items.Insert(0, ApplicationConstants.C_COMBO_SELECT, string.Empty);
			
		}

		
	
	
	
		
		private void ddlSortColumn_ValueChanged(object sender, System.EventArgs e)
		{
			
			
			if ( ddlSortColumn.SelectedItem != null )
			{
				sortKey = ddlSortColumn.SelectedItem.DisplayText; 
			}
			
		}

		private void ColumnsUserControl_Load(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

        public List<string> DisplayColumns
        {
            get {return  _rightList ; }
        }

	}
}
