using System;
using System.Collections;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for Coulmns.
    /// </summary>
    public class UsrCtrlColumns : System.Windows.Forms.UserControl
    {
        #region Controls

        private System.Windows.Forms.ListBox lbAvailableColumns;
        private System.Windows.Forms.ListBox lbDisplayColumns;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button brnMoveAllRight;
        private System.Windows.Forms.Button btnMoveAllLeft;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label lblDisplayColumns;
        private System.Windows.Forms.Label lblAvailableColumns;
        public ArrayList availableColumnList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox grpbxColumns;
        private System.ComponentModel.IContainer components;

        #endregion


        public UsrCtrlColumns()
        {
            InitializeComponent();
        }

        public void SetUp(ArrayList availableColumns, ArrayList alreadyBindedColumns)
        {

            InitControl(availableColumns, alreadyBindedColumns);
        }

        private void InitControl(ArrayList availableColumns1, ArrayList alreadyBindedColumns)
        {

            try
            {

                availableColumnList = availableColumns1;

                #region ColumnsBinding
                //  availableColumnList.Clear();

                ArrayList dispList = alreadyBindedColumns;

                //lbDisplayColumns.DataSource=dispList;			


                foreach (object obj in dispList)
                    availableColumns1.Remove(obj);
                BindAvailableColumns();
                #endregion

            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (lblAvailableColumns != null)
                {
                    lblAvailableColumns.Dispose();
                }
                if (lblDisplayColumns != null)
                {
                    lblDisplayColumns.Dispose();
                }
                if (lbAvailableColumns != null)
                {
                    lbAvailableColumns.Dispose();
                }
                if (lbDisplayColumns != null)
                {
                    lbDisplayColumns.Dispose();
                }
                if (btnMoveRight != null)
                {
                    btnMoveRight.Dispose();
                }
                if (btnMoveLeft != null)
                {
                    btnMoveLeft.Dispose();
                }
                if (btnMoveAllLeft != null)
                {
                    btnMoveAllLeft.Dispose();
                }
                if (brnMoveAllRight != null)
                {
                    brnMoveAllRight.Dispose();
                }
                if (btnDown != null)
                {
                    btnDown.Dispose();
                }
                if (btnUp != null)
                {
                    btnUp.Dispose();
                }
                if (lblDisplayColumns != null)
                {
                    lblDisplayColumns.Dispose();
                }
                if (lblAvailableColumns != null)
                {
                    lblAvailableColumns.Dispose();
                }
                if (imageList1 != null)
                {
                    imageList1.Dispose();
                }
                if (grpbxColumns != null)
                {
                    grpbxColumns.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsrCtrlColumns));
            this.lbAvailableColumns = new System.Windows.Forms.ListBox();
            this.lbDisplayColumns = new System.Windows.Forms.ListBox();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.brnMoveAllRight = new System.Windows.Forms.Button();
            this.btnMoveAllLeft = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.lblDisplayColumns = new System.Windows.Forms.Label();
            this.lblAvailableColumns = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.grpbxColumns = new System.Windows.Forms.GroupBox();
            this.grpbxColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAvailableColumns
            // 
            this.lbAvailableColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbAvailableColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAvailableColumns.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbAvailableColumns.Location = new System.Drawing.Point(12, 23);
            this.lbAvailableColumns.Name = "lbAvailableColumns";
            this.lbAvailableColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableColumns.Size = new System.Drawing.Size(122, 184);
            this.lbAvailableColumns.TabIndex = 0;
            // 
            // lbDisplayColumns
            // 
            this.lbDisplayColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDisplayColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbDisplayColumns.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbDisplayColumns.Location = new System.Drawing.Point(184, 23);
            this.lbDisplayColumns.Name = "lbDisplayColumns";
            this.lbDisplayColumns.Size = new System.Drawing.Size(118, 184);
            this.lbDisplayColumns.TabIndex = 3;
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveRight.BackColor = System.Drawing.Color.Moccasin;
            this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveRight.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveRight.Image")));
            this.btnMoveRight.Location = new System.Drawing.Point(144, 23);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(30, 24);
            this.btnMoveRight.TabIndex = 1;
            this.btnMoveRight.UseVisualStyleBackColor = false;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveLeft.BackColor = System.Drawing.Color.Moccasin;
            this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLeft.Image")));
            this.btnMoveLeft.Location = new System.Drawing.Point(142, 173);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(30, 24);
            this.btnMoveLeft.TabIndex = 7;
            this.btnMoveLeft.UseVisualStyleBackColor = false;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // brnMoveAllRight
            // 
            this.brnMoveAllRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.brnMoveAllRight.BackColor = System.Drawing.Color.Moccasin;
            this.brnMoveAllRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.brnMoveAllRight.Image = ((System.Drawing.Image)(resources.GetObject("brnMoveAllRight.Image")));
            this.brnMoveAllRight.Location = new System.Drawing.Point(144, 65);
            this.brnMoveAllRight.Name = "brnMoveAllRight";
            this.brnMoveAllRight.Size = new System.Drawing.Size(30, 24);
            this.brnMoveAllRight.TabIndex = 2;
            this.brnMoveAllRight.UseVisualStyleBackColor = false;
            this.brnMoveAllRight.Click += new System.EventHandler(this.brnMoveAllRight_Click);
            // 
            // btnMoveAllLeft
            // 
            this.btnMoveAllLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveAllLeft.BackColor = System.Drawing.Color.Moccasin;
            this.btnMoveAllLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveAllLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveAllLeft.Image")));
            this.btnMoveAllLeft.Location = new System.Drawing.Point(142, 125);
            this.btnMoveAllLeft.Name = "btnMoveAllLeft";
            this.btnMoveAllLeft.Size = new System.Drawing.Size(30, 24);
            this.btnMoveAllLeft.TabIndex = 6;
            this.btnMoveAllLeft.UseVisualStyleBackColor = false;
            this.btnMoveAllLeft.Click += new System.EventHandler(this.btnMoveAllLeft_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.BackColor = System.Drawing.Color.Moccasin;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(304, 113);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 24);
            this.btnDown.TabIndex = 5;
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.BackColor = System.Drawing.Color.Moccasin;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(304, 73);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 24);
            this.btnUp.TabIndex = 4;
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // lblDisplayColumns
            // 
            this.lblDisplayColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDisplayColumns.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblDisplayColumns.Location = new System.Drawing.Point(182, -1);
            this.lblDisplayColumns.Name = "lblDisplayColumns";
            this.lblDisplayColumns.Size = new System.Drawing.Size(98, 16);
            this.lblDisplayColumns.TabIndex = 56;
            this.lblDisplayColumns.Text = "Third Party Accounts";
            // 
            // lblAvailableColumns
            // 
            this.lblAvailableColumns.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAvailableColumns.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblAvailableColumns.Location = new System.Drawing.Point(12, -1);
            this.lblAvailableColumns.Name = "lblAvailableColumns";
            this.lblAvailableColumns.Size = new System.Drawing.Size(128, 16);
            this.lblAvailableColumns.TabIndex = 55;
            this.lblAvailableColumns.Text = "Client Accounts";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            // 
            // grpbxColumns
            // 
            this.grpbxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxColumns.Controls.Add(this.btnMoveAllLeft);
            this.grpbxColumns.Controls.Add(this.lbDisplayColumns);
            this.grpbxColumns.Controls.Add(this.btnMoveRight);
            this.grpbxColumns.Controls.Add(this.btnUp);
            this.grpbxColumns.Controls.Add(this.lblDisplayColumns);
            this.grpbxColumns.Controls.Add(this.lblAvailableColumns);
            this.grpbxColumns.Controls.Add(this.btnMoveLeft);
            this.grpbxColumns.Controls.Add(this.btnDown);
            this.grpbxColumns.Controls.Add(this.brnMoveAllRight);
            this.grpbxColumns.Controls.Add(this.lbAvailableColumns);
            this.grpbxColumns.Location = new System.Drawing.Point(10, 28);
            this.grpbxColumns.Name = "grpbxColumns";
            this.grpbxColumns.Size = new System.Drawing.Size(348, 214);
            this.grpbxColumns.TabIndex = 62;
            this.grpbxColumns.TabStop = false;
            // 
            // UsrCtrlColumns
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpbxColumns);
            this.Name = "UsrCtrlColumns";
            this.Size = new System.Drawing.Size(370, 255);
            this.grpbxColumns.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private void BindAvailableColumns()
        {
            try
            {
                //lbAvailableColumns.DataSource = null;
                //lbAvailableColumns.DataSource = availableColumnList;
                lbAvailableColumns.Items.Add(availableColumnList[0]);
                lbAvailableColumns.Refresh();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Events
        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            MoveItem(lbAvailableColumns, lbDisplayColumns);
            UpdateAllocationPrefrence();
        }

        private void brnMoveAllRight_Click(object sender, EventArgs e)
        {
            MoveAllItemsRight(lbAvailableColumns, lbDisplayColumns);
            UpdateAllocationPrefrence();
        }

        private void btnMoveAllLeft_Click(object sender, EventArgs e)
        {
            MoveAllItemsLeft(lbDisplayColumns, lbAvailableColumns);
            UpdateAllocationPrefrence();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            if (lbDisplayColumns.Items.Count > 1)
            {
                MoveItem(lbDisplayColumns, lbAvailableColumns);
                UpdateAllocationPrefrence();
            }
            else
            {
                MessageBox.Show("Atleast one Column should be displayed!!");

            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveUpDown(lbDisplayColumns, true);
            UpdateAllocationPrefrence();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveUpDown(lbDisplayColumns, false);
            UpdateAllocationPrefrence();
        }





        #endregion

        #region  MoveItem
        public void MoveItem(ListBox source, ListBox destination)
        {
            if (source.Items.Count > 0)
            {
                System.Windows.Forms.ListBox.SelectedObjectCollection selected = source.SelectedItems;
                string[] list = new string[selected.Count];
                int index = 0;

                //destination.DataSource = null;
                ArrayList dsDestination = null;
                if (destination.DataSource == null)
                {
                    dsDestination = new ArrayList();
                }
                else
                {
                    dsDestination = (ArrayList)destination.DataSource;
                }

                // add to destination list
                foreach (object obj in selected)
                {
                    //destination.Items.Add(obj.ToString());
                    list[index++] = obj.ToString();
                    dsDestination.Add(obj.ToString());
                }
                destination.DataSource = null;
                destination.DataSource = dsDestination;


                // delete from source 
                ArrayList dsList = new ArrayList();
                foreach (object obj in source.Items)
                {
                    dsList.Add(obj.ToString());
                }

                for (int i = 0; i < list.Length; i++)
                {
                    if (dsList.Contains(list[i]))
                    {
                        dsList.Remove(list[i]);
                    }
                }

                source.DataSource = null;
                source.Items.Clear();
                source.DataSource = dsList;
            }
        }

        public void MoveAllItemsRight(ListBox source, ListBox destination)
        {
            // copy source items in  a list
            object[] objects = new object[source.Items.Count + destination.Items.Count];
            int index = 0;
            foreach (object obj in source.Items)
            {
                objects[index++] = obj.ToString();
            }

            // add destination items in the list
            foreach (object obj in destination.Items)
            {
                objects[index++] = obj.ToString();
            }
            destination.DataSource = null;
            destination.Items.Clear();
            destination.Items.AddRange(objects);

            // delete from source 
            source.DataSource = null;
            source.Items.Clear();
        }
        public void MoveAllItemsLeft(ListBox source, ListBox destination)
        {
            try
            {
                // copy source items in  a list
                object[] objects = new object[source.Items.Count + destination.Items.Count - 1];

                int index = 0;
                int sourceCount = int.Parse(source.Items.Count.ToString());
                if (sourceCount != 1)
                {
                    int i = 1;
                    ArrayList arrList = new ArrayList();
                    foreach (object obj in source.Items)
                    {
                        if (i <= sourceCount)
                        {
                            if (i != 1)
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
                    foreach (object obj in destination.Items)
                    {
                        objects[index++] = obj.ToString();
                    }

                    //MessageBox.Show("AfterDestination" + sourceCount);
                    destination.DataSource = null;
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
            catch (Exception)
            {
            }
        }

        public void MoveUpDown(ListBox listBox, bool moveUp)
        {
            if (listBox.SelectedItem == null)
            {
                return;
            }

            string selectedItem = listBox.SelectedItem.ToString();
            int selectedIndex = Convert.ToInt32(listBox.SelectedIndex);

            // copy source items in a list
            ArrayList list = new ArrayList();
            foreach (object obj in listBox.Items)
            {
                list.Add(obj.ToString());
            }

            if (moveUp && selectedIndex != 0)
            {
                list[selectedIndex] = list[selectedIndex - 1].ToString();
                list[selectedIndex - 1] = selectedItem;
                selectedIndex--;
            }
            else if (!moveUp && (selectedIndex != (listBox.Items.Count - 1)))
            {
                list[selectedIndex] = list[selectedIndex + 1].ToString();
                list[selectedIndex + 1] = selectedItem;
                selectedIndex++;
            }

            listBox.DataSource = null;
            listBox.Items.Clear();
            listBox.DataSource = list;
            listBox.SelectedIndex = selectedIndex;

        }

        #endregion

        private void UpdateAllocationPrefrence()
        {

            ArrayList list = new ArrayList();
            //if (allocationColumnPreferenceData == null)
            //    allocationColumnPreferenceData = new GridColumns();
            //allocationColumnPreferenceData.ClearColumns();
            foreach (object obj in lbDisplayColumns.Items)
            {
                list.Add(obj.ToString());
                //  allocationColumnPreferenceData.AddColumn(obj.ToString());
            }
            // string sortKey = string.Empty;

            // FillSortDdl(allocationColumnPreferenceData.DisplayColumns);

        }

        public ArrayList GetSelectedColumns()
        {
            ArrayList list = new ArrayList();
            foreach (object obj in lbDisplayColumns.Items)
            {
                list.Add(obj.ToString());
            }
            return list;
        }

        #region Validation and Clear
        public bool ValidateUsrCtrl()
        {
            bool IsValidated = false;
            if (lbDisplayColumns.Items.Count == 0)
            {
                MessageBox.Show("Please Select Display Columns");
                return IsValidated;
            }
            else
            {
                IsValidated = true;
                return IsValidated;
            }
        }
        public void ClearUsrCtrl()
        {
            lbDisplayColumns.DataSource = null;
            lbDisplayColumns.Items.Clear();
            //lbDisplayColumns.Refresh();
            lbAvailableColumns.DataSource = null;
            lbAvailableColumns.DataSource = availableColumnList;


        }
        #endregion
    }
}

