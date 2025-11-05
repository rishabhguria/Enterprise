using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Prana.Global;
using Prana.BusinessObjects;
//using Prana.PostTrade;

namespace Prana.AllocationNew
{
    public partial class AllocationColumns : UserControl
    {
        private ArrayList _gridTypes;

        public AllocationColumns()
        {
            InitializeComponent();
        }
        public void SetUp(AllocationPreferences _allocationPreferences, PranaInternalConstants.TYPE_OF_ALLOCATION typeOfAllocation)
        {
            _gridTypes = new ArrayList();
            _gridTypes.Clear();
            foreach (string str in Enum.GetNames(typeof(AllocationConstants.AllocationGrid)))
                _gridTypes.Add(str);

            lbGridType.DataSource = _gridTypes;
            lbGridType.SelectedIndex = 0;

            List<string> unallocatedAvailableList = new List<string>();
                foreach (string str in AllocationConstants.UnAllocatedDisplayColumns)
                    unallocatedAvailableList.Add(str);
                List<string> allocatedAvailableList = new List<string>();
                foreach (string str in AllocationConstants.AllocatedDisplayColumns)
                    allocatedAvailableList.Add(str);


                uctUnAllocatedColumn.SetUp(unallocatedAvailableList, _allocationPreferences.ColumnList.UnAllocatedGridColumns.DisplayColumns, _allocationPreferences.ColumnList.UnAllocatedGridColumns.Ascending );
                uctAllocatedColumns.SetUp(allocatedAvailableList, _allocationPreferences.ColumnList.AllocatedGridColumns.DisplayColumns, _allocationPreferences.ColumnList.AllocatedGridColumns.Ascending);

        }
        private void uctGroupedColumns_Load(object sender, EventArgs e)
        {
           

        }
        public ColumnsUserControl UnAllocatedColumns
        { 
            get{return  uctUnAllocatedColumn;}
        }
        
        public ColumnsUserControl AllocatedColumns
        {
            get { return uctAllocatedColumns; }
        }
        private void lbGridType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            uctUnAllocatedColumn.Visible = false;
            uctAllocatedColumns.Visible = false;
            switch (lbGridType.SelectedIndex)
            {
                case 0:
                    uctUnAllocatedColumn.Visible = true;
                    break;
               
                case 1:
                    uctAllocatedColumns.Visible = true;
                    break;
            }
        }
    }
}
