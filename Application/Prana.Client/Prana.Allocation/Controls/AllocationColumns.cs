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
using Prana.Allocation.BLL;
namespace Prana.Allocation
{
    public partial class AllocationColumns : UserControl
    {
        private ArrayList _gridTypes;

        public AllocationColumns()
        {
            InitializeComponent();
        }
        public void SetUp(AllocationPreferences  _allocationPreferences,PranaInternalConstants.TYPE_OF_ALLOCATION typeOfAllocation)
        {
            _gridTypes = new ArrayList();
            _gridTypes.Clear();
            foreach (string str in Enum.GetNames(typeof(GridType.GridColumns)))
                _gridTypes.Add(str);

            lbGridType.DataSource=_gridTypes;
            lbGridType.SelectedIndex=0;

            if (typeOfAllocation == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
            {
                uctUnAllocatedColumn.SetUp(_allocationPreferences.FundType.UnAllocatedGridColumns, AllocationConstants.UNALLOCATED_GRID);
                uctGroupedColumns.SetUp(_allocationPreferences.FundType.GroupedGridColumns, AllocationConstants.GROUPED_GRID);
                uctAllocatedColumns.SetUp(_allocationPreferences.FundType.AllocatedGridColumns, AllocationConstants.ALLOCATED_GRID);
            }
            else
            {
                uctUnAllocatedColumn.SetUp(_allocationPreferences.StrategyType.UnAllocatedGridColumns, AllocationConstants.UNALLOCATED_GRID);
                uctGroupedColumns.SetUp(_allocationPreferences.StrategyType.GroupedGridColumns, AllocationConstants.GROUPED_GRID);
                uctAllocatedColumns.SetUp(_allocationPreferences.StrategyType.AllocatedGridColumns, AllocationConstants.ALLOCATED_GRID);
            }
        }
        private void uctGroupedColumns_Load(object sender, EventArgs e)
        {
           

        }
        public ColumnsUserControl UnAllocatedColumns
        { 
            get{return  uctUnAllocatedColumn;}
        }
        public ColumnsUserControl GroupedColumns
        {
            get { return uctGroupedColumns; }
        }
        public ColumnsUserControl AllocatedColumns
        {
            get { return uctAllocatedColumns; }
        }
        private void lbGridType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            uctUnAllocatedColumn.Visible = false;
            uctGroupedColumns.Visible = false;
            uctAllocatedColumns.Visible = false;
            switch (lbGridType.SelectedIndex)
            {
                case 0:
                    uctUnAllocatedColumn.Visible = true;
                    break;
                case 1:
                    uctGroupedColumns.Visible = true;
                    break;
                case 2:
                    uctAllocatedColumns.Visible = true;
                    break;
            }
        }
    }
}
