using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

using System.Configuration;
using System.Text.RegularExpressions;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.UDATool
{
    public partial class UDAControl : UserControl
    {
        bool _ischanged = false;
        private List<int> _deletdIDS = new List<int>();
        private UDACollection _collection = new UDACollection();
        private List<int> _UDAIDsInUse = null;
        string _sp_GetName;
        string _sp_DeleteName;
        string _sp_InsertName;
        string _sp_GetIDsInUse;
        int _maxCount = 0;

        public UDAControl()
        {
            InitializeComponent();
        }
        public List<int> UDAsInUse
        {
            get { return _UDAIDsInUse; }
            set { _UDAIDsInUse = value; }
        }
        public void SetUp(string name, string sp_GetName, string sp_DeleteName, string sp_InsertName, string sp_GetIDsInUse)
        {
            try
            {
                _sp_GetName = sp_GetName;
                _sp_DeleteName = sp_DeleteName;
                _sp_InsertName = sp_InsertName;
                _sp_GetIDsInUse = sp_GetIDsInUse;
                UDACollection collection = DataManager.GetUDAAttributeData(_sp_GetName);
                CentralDataManager.AddUserControl(name, this);
                _collection = collection;

                grid.DataSource = _collection;

                grid.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[1].Header.Caption = name;

                if (collection.Count > 0)
                {
                    int maxID = 0;
                    foreach (UDA item in collection)
                        if (item.ID > maxID)
                        {
                            maxID = item.ID;
                        }
                    _maxCount = maxID + 1;
                    //_maxCount = collection.[collection.Count - 1].ID;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public UDACollection Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }
        public List<int> DeletedIDs
        {
            get { return _deletdIDS; }
        }


        public UltraGrid Grid
        {
            get { return grid; }
        }


        private void mnuItemAddNew_Click(object sender, EventArgs e)
        {
            _ischanged = true;
            UDA uda = new UDA();
            uda.ID = GetNextID();
            _collection.Add(uda);
        }

        private void mnuItemDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (grid.ActiveRow != null)
                {
                    UDA uda = (UDA)(grid.ActiveRow.ListObject);
                    if (_UDAIDsInUse.Contains(uda.ID))
                    {
                        DialogResult result = MessageBox.Show("This attribute is already assigned. Are you sure you want to delete their associations from the symbols?", "Prana", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                    _ischanged = true;
                    _deletdIDS.Add(uda.ID);
                    _UDAIDsInUse.Remove(uda.ID);
                    grid.ActiveRow.Delete(false); 
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void grid_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {
            if (_collection.Contains(e.Cell.Text))
            {
                MessageBox.Show("Duplicate Name!");
                e.Cancel = true;
            }
        }

        public bool IsChanged
        {
            get { return _ischanged; }
            set { _ischanged = value; }
        }
        public bool SaveChanges()
        {
            if (_ischanged)
        {
            UpdateCollection();
            DataManager.DeleteInformation(_sp_DeleteName, _deletdIDS);
            DataManager.SaveInformation(_sp_InsertName, _collection);
            _ischanged = false;
                return true;
            }
            else
                return false;
        }
        private void UpdateCollection()
        {
            try
            {
                List<UDA> nullUDAs = new List<UDA>();
                foreach (UDA uda in _collection)
                {
                    if (uda.Name == null)
                    {
                        nullUDAs.Add(uda);
                    }
                }
                foreach (UDA uda in nullUDAs)
                {
                    _collection.Remove(uda);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private int GetNextID()
        {
            return ++_maxCount;
        }

        public void FillInUseUDAsIDList()
        {
            _UDAIDsInUse = DataManager.GetInUseUDAIDs(_sp_GetIDsInUse);
        }

        internal void UndoChanges()
        {
            _collection = DataManager.GetUDAAttributeData(_sp_GetName);
            _deletdIDS.Clear();
            grid.DataSource = _collection;
            _ischanged = false;
        }

        private void grid_CellChange(object sender, CellEventArgs e)
        {
            _ischanged = true;
            if (e.Cell.Column.Header.Caption.Equals("SecurityType") || e.Cell.Column.Header.Caption.Equals("AssetClass") || e.Cell.Column.Header.Caption.Equals("Country"))
            {
                if (e.Cell.Text.Length > 20)
                {
                    MessageBox.Show("Security Type should be of atmost 20 characters");
                    e.Cell.Value = e.Cell.Text.Substring(0, 20);
                }
            }
            if (e.Cell.Column.Header.Caption.Equals("Sector"))
            {
                if (e.Cell.Text.Length > 30)
                {
                    MessageBox.Show("Security Type should be of atmost 30 characters");
                    e.Cell.Value = e.Cell.Text.Substring(0, 30);
                }
            }
        }
    }

}

