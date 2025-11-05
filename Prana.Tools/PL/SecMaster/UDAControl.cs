using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class UDAControl : UserControl
    {
        bool _ischanged = false;
        private List<int> _deletdIDS = new List<int>();

        private UDACollection _collection = new UDACollection();
        public UDACollection Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        private UDACollection _addedUDACollection = new UDACollection();
        public UDACollection AddedUDACollection
        {
            get { return _addedUDACollection; }
            set { _addedUDACollection = value; }
        }





        private List<int> _UDAIDsInUse = new List<int>();
        public List<int> UDAsInUse
        {
            get { return _UDAIDsInUse; }
            set { _UDAIDsInUse = value; }
        }


        string _sp_DeleteName = string.Empty;
        public string SP_DeleteName
        {
            get { return _sp_DeleteName; }
            set { _sp_DeleteName = value; }
        }

        string _UDAType = string.Empty;
        public string UDAType
        {
            get { return _UDAType; }
            set { _UDAType = value; }
        }
        string _sp_InsertName = string.Empty;
        public string SP_InsertName
        {
            get { return _sp_InsertName; }
            set { _sp_InsertName = value; }
        }
        public List<int> DeletedIDs
        {
            get { return _deletdIDS; }
        }


        public UltraGrid Grid
        {
            get { return grid; }
        }


        int _maxCount = 0;
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public UDAControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        //public void SetUp(string name, UDACollection collection,string sp_GetName, string sp_DeleteName, string sp_InsertName, string sp_GetIDsInUse)
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="name"></param>
        /// <param name="udaCollection"></param>
        /// <param name="sp_DeleteName"></param>
        /// <param name="sp_InsertName"></param>
        internal void SetUp(string name, UDACollection udaCollection, string sp_DeleteName, string sp_InsertName)
        {
            try
            {
                //TODO - om
                //_sp_GetName = sp_GetName;
                _sp_DeleteName = sp_DeleteName;
                _sp_InsertName = sp_InsertName;
                _UDAType = name;
                //_sp_GetIDsInUse = sp_GetIDsInUse;
                //UDACollection collection = DataManager.GetUDAAttributeData(_sp_GetName);
                CentralDataManager.AddUserControl(name, this);
                _collection = udaCollection;


                grid.DataSource = udaCollection;

                grid.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[1].Header.Caption = name;
                grid.DisplayLayout.Bands[0].Columns[1].Header.Appearance.TextHAlign = HAlign.Center;


                if (udaCollection.Count > 0)
                {
                    int maxID = 0;
                    foreach (UDA item in udaCollection)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                _ischanged = true;
                UDA uda = new UDA();
                uda.ID = GetNextID();
                _collection.Add(uda);
                _addedUDACollection.Add(uda);
                //Added to set newly added cell to editmode, PRANA-11307
                grid.Refresh();
                grid.Rows[_collection.IndexOf(uda)].Cells[1].Activate();
                grid.PerformAction(UltraGridAction.EnterEditMode);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

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
                        MessageBox.Show("You can not delete used UDA!", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        //DialogResult result = MessageBox.Show("This attribute is already assigned. Are you sure you want to delete their associations from the symbols?", "Prana", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        //if (result == DialogResult.No)
                        //{
                        //    return;
                        //}
                    }
                    _ischanged = true;
                    _deletdIDS.Add(uda.ID);
                    _UDAIDsInUse.Remove(uda.ID);
                    grid.ActiveRow.Delete(false);
                }
                else
                {
                    MessageBox.Show("Please select UDA to delete.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void grid_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (_collection.Contains(e.Cell.Text))
                {
                    MessageBox.Show("Duplicate Name!", "UDA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(e.Cell.Text))
                {
                    MessageBox.Show("UDA must contain some value!", "UDA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (!e.Cancel)
                {
                    int UdaId = int.MinValue;
                    int.TryParse(e.Cell.Row.Cells["ID"].Value.ToString(), out UdaId);

                    if (UdaId != int.MinValue && _collection.Contains(UdaId))
                    {
                        _collection.GetUDA(UdaId).Name = e.Cell.Text;

                        if (_addedUDACollection.Contains(UdaId))
                        {
                            _addedUDACollection.GetUDA(UdaId).Name = e.Cell.Text;

                        }
                        else
                        {
                            _addedUDACollection.Add(_collection.GetUDA(UdaId));
                        }
                        _ischanged = true;
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool IsChanged
        {
            get { return _ischanged; }
            set { _ischanged = value; }
        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            try
            {
                if (_ischanged)
                {
                    UpdateCollection();



                    //   DataManager.DeleteInformation(_sp_DeleteName, _deletdIDS);
                    //   DataManager.SaveInformation(_sp_InsertName, _collection);
                    _ischanged = false;

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        internal Boolean UpdateCollection()
        {
            Boolean isDataSendToSave = true;
            try
            {
                if (_ischanged)
                {
                    List<int> nullUDAs = new List<int>();
                    foreach (UDA uda in _collection)
                    {
                        //modified by omshiv, checking for null or empty UDA name
                        if (String.IsNullOrEmpty(uda.Name))
                        {
                            nullUDAs.Add(uda.ID);
                        }
                    }
                    foreach (int udaId in nullUDAs)
                    {
                        if (_collection.Contains(udaId))
                            _collection.Remove(_collection.GetUDA(udaId));

                        if (_addedUDACollection.Contains(udaId))
                            _addedUDACollection.Remove(_addedUDACollection.GetUDA(udaId));

                    }
                    _ischanged = false;
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isDataSendToSave;
        }

        private int GetNextID()
        {
            return ++_maxCount;
        }

        public void FillInUseUDAsIDList()
        {
            // _UDAIDsInUse = DataManager.GetInUseUDAIDs(_sp_GetIDsInUse);
        }

        internal void UndoChanges()
        {
            try
            {
                //  _collection = DataManager.GetUDAAttributeData(_sp_GetName);
                _deletdIDS.Clear();
                grid.DataSource = _collection;
                _ischanged = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grid_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                _ischanged = true;
                //Change UpdateMode to OnUpdate Mode to save changes. PRANA-8666 
                grid.UpdateMode = UpdateMode.OnUpdate;
                //if (e.Cell.Column.Header.Caption.Equals("SecurityType") || e.Cell.Column.Header.Caption.Equals("AssetClass") || e.Cell.Column.Header.Caption.Equals("Country"))
                //{
                //    if (e.Cell.Text.Length > 20)
                //    {
                //        MessageBox.Show("Security Type should be of atmost 20 characters");
                //        e.Cell.Value = e.Cell.Text.Substring(0, 20);
                //    }
                //}
                //if (e.Cell.Column.Header.Caption.Equals("Sector"))
                //{
                //    if (e.Cell.Text.Length > 30)
                //    {
                //        MessageBox.Show("Security Type should be of atmost 30 characters");
                //        e.Cell.Value = e.Cell.Text.Substring(0, 30);
                //    }
                //}

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grid_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch
            {
                //Do Nothing as user can try again
            }
        }




    }

}

