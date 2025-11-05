using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.ExtensionUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ErrorEventArgs = Infragistics.Win.UltraWinGrid.ErrorEventArgs;

namespace Prana.TradingTicket.Controls
{
    public partial class CounterPartyWiseDefaultCommissions : UserControl
    {
        public CounterPartyWiseDefaultCommissions()
        {
            InitializeComponent();
        }

        private CounterPartyWiseCommissionBasis _commissionBasis = null;
        private DataTable _gridDataSourceTable = null;
        private static Dictionary<int, string> _venues = new Dictionary<int, string>();
        private static Dictionary<string, string> _algoType = new Dictionary<string, string>();
        UltraDropDown cmbVenue = new UltraDropDown
        {
            DataSource = CachedDataManager.GetInstance.GetAllVenues(),
            DisplayMember = "Value",
            ValueMember = "Key"
        };
        UltraDropDown cmbAlgoType = new UltraDropDown
        {

            DataSource = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().AlgoStrategyNames,
            DisplayMember = "Value",
            ValueMember = "Key"
        };


        public void Setup()
        {
            try
            {
                _commissionBasis = TradingTktPrefs.CpwiseCommissionBasis;
                SetupDataSourceGrid();
                CreateDataTableFromCounterparties();
                SetupGridColumns();
                SetGridDataSource();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetGridDataSource()
        {
            grdBrokerWiseSettings.DataSource = null;
            grdBrokerWiseSettings.DataSource = _gridDataSourceTable;
        }

        private void SetupGridColumns()
        {
            try
            {
                foreach (DataRow row in _gridDataSourceTable.Rows)
                {
                    int counterpartyID = Convert.ToInt32(row["CounterPartyID"].ToString());
                    if (_commissionBasis.DictCounterPartyWiseCommissionBasis.ContainsKey(counterpartyID))
                    {
                        row["CommissionBasis"] = _commissionBasis.DictCounterPartyWiseCommissionBasis[counterpartyID];
                    }
                    if (_commissionBasis.DictCounterPartyWiseSoftCommissionBasis.ContainsKey(counterpartyID))
                    {
                        row["SoftCommissionBasis"] = _commissionBasis.DictCounterPartyWiseSoftCommissionBasis[counterpartyID];
                    }
                    if (_commissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(counterpartyID))
                    {
                        row["ExecutionInstruction"] = _commissionBasis.DictCounterPartyWiseExecutionInstructions[counterpartyID];
                    }
                    if (_commissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(counterpartyID))
                    {
                        row["Venue"] = _commissionBasis.DictCounterPartyWiseExecutionVenue[counterpartyID];
                    }
                    if (_commissionBasis.DictCounterPartyWiseExecutionAlgoType.ContainsKey(counterpartyID))
                    {
                        row["AlgoType"] = _commissionBasis.DictCounterPartyWiseExecutionAlgoType[counterpartyID];
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetupDataSourceGrid()
        {
            try
            {
                _gridDataSourceTable = new DataTable();
                _gridDataSourceTable.Columns.Add("CounterPartyID", typeof(int));
                _gridDataSourceTable.Columns.Add("Venue", typeof(int));
                _gridDataSourceTable.Columns.Add("AlgoType", typeof(int));
                _gridDataSourceTable.Columns.Add("CommissionBasis", typeof(int));
                _gridDataSourceTable.Columns.Add("SoftCommissionBasis", typeof(int));
                _gridDataSourceTable.Columns.Add("ExecutionInstruction", typeof(int));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        public void Save(int userID)
        {
            try
            {
                _commissionBasis.UserID = userID;
                GetPreferenceObjectFromDataGrid();
                SavePreferences(_commissionBasis);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetPreferenceObjectFromDataGrid()
        {
            try
            {
                _gridDataSourceTable = (DataTable)grdBrokerWiseSettings.DataSource;
                foreach (DataRow row in _gridDataSourceTable.Rows)
                {
                    int counterPartyID = Convert.ToInt32(row["CounterpartyID"].ToString());
                    CheckAndUpdateDictionary("Venue", _commissionBasis.DictCounterPartyWiseExecutionVenue, row, counterPartyID);
                    CheckAndUpdateDictionary("AlgoType", _commissionBasis.DictCounterPartyWiseExecutionAlgoType, row, counterPartyID);
                    CheckAndUpdateDictionary("CommissionBasis", _commissionBasis.DictCounterPartyWiseCommissionBasis, row, counterPartyID);
                    CheckAndUpdateDictionary("SoftCommissionBasis", _commissionBasis.DictCounterPartyWiseSoftCommissionBasis, row, counterPartyID);
                    CheckAndUpdateDictionary("ExecutionInstruction", _commissionBasis.DictCounterPartyWiseExecutionInstructions, row, counterPartyID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CheckAndUpdateDictionary(string columnKey, SerializableDictionary<int, int> serializableDictionary, DataRow row, int counterPartyId)
        {
            if (row[columnKey] != null && !String.IsNullOrEmpty(row[columnKey].ToString()))
            {
                if (serializableDictionary.ContainsKey(counterPartyId))
                {
                    serializableDictionary[counterPartyId] = Convert.ToInt32(row[columnKey].ToString());
                }
                else
                {
                    serializableDictionary.Add(counterPartyId, Convert.ToInt32(row[columnKey].ToString()));
                }
            }
            else if (row[columnKey] == DBNull.Value)
            {
                if (serializableDictionary.ContainsKey(counterPartyId))
                {
                    serializableDictionary.Remove(counterPartyId);
                }
            }
        }

        private void CreateDataTableFromCounterparties()
        {
            Dictionary<int, string> dictBrokers = CachedDataManager.GetInstance.GetAllCounterParties();
            foreach (KeyValuePair<int, string> dictBroker in dictBrokers)
            {
                DataRow dr = _gridDataSourceTable.NewRow();
                dr["CounterPartyID"] = dictBroker.Key;
                _gridDataSourceTable.Rows.Add(dr);
            }
            _gridDataSourceTable.AcceptChanges();
        }

        public static void SavePreferences(CounterPartyWiseCommissionBasis commissionBasis)
        {
            try
            {
                string startPath = Application.StartupPath;
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + commissionBasis.UserID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\CounterPartyWiseCommissionBasis.xml";

                using (XmlTextWriter writer = new XmlTextWriter(preferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(CounterPartyWiseCommissionBasis));
                    serializer.Serialize(writer, commissionBasis);
                    writer.Flush();
                    //writer.Close();
                    TradingTktPrefs.CpwiseCommissionBasis = commissionBasis;
                }
            }

            #region catch

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }

            #endregion catch
        }

        private void grdBrokerWiseSettings_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                cmbVenue.DisplayLayout.Bands[0].ColHeadersVisible = false;
                if (cmbVenue.DisplayLayout.Bands[0].Columns.Exists("Key"))
                    cmbVenue.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;

                cmbAlgoType.DisplayLayout.Bands[0].ColHeadersVisible = false;
                if (cmbAlgoType.DisplayLayout.Bands[0].Columns.Exists("Key"))
                    cmbAlgoType.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;

                UltraDropDown cmbCounterParty = new UltraDropDown
                {
                    DataSource = CachedDataManager.GetInstance.GetAllCounterParties(),
                    DisplayMember = "Value",
                    ValueMember = "Key"
                };

                IList list = EnumHelper.ToList(typeof(CalculationBasis));
                for (int i = 0; i < 4; i++)
                {
                    list.RemoveAt(2);
                }

                Dictionary<int, string> commissionBasis = new Dictionary<int, string>();
                for (int i = 0; i < list.Count; i++)
                {
                    commissionBasis.Add(i, ((KeyValuePair<Enum, string>)list[i]).Value);
                }

                UltraDropDown commissionDropDown = new UltraDropDown
                {
                    DataSource = commissionBasis,
                    DisplayMember = "Value",
                    ValueMember = "Key"
                };

                UltraDropDown executionInstructionDropDown = new UltraDropDown
                {
                    DataSource = TagDatabaseManager.GetInstance.GetAllExecutionInstructions(),
                    DisplayMember = OrderFields.PROPERTY_EXECUTION_INST,
                    ValueMember = OrderFields.PROPERTY_EXECUTION_INSTID
                };

                commissionDropDown.DisplayLayout.Bands[0].ColHeadersVisible = false;
                commissionDropDown.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;

                executionInstructionDropDown.DisplayLayout.Bands[0].ColHeadersVisible = false;
                executionInstructionDropDown.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_INSTID].Hidden = true;
                executionInstructionDropDown.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Hidden = true;

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CounterPartyID"].ValueList = cmbCounterParty;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CounterPartyID"].Header.Caption = "Broker";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CounterPartyID"].CellActivation = Activation.NoEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CounterPartyID"].Header.VisiblePosition = 0;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CounterPartyID"].Header.Fixed = true;

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].Header.Caption = "Venue";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].ValueList = cmbVenue;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].CellActivation = Activation.AllowEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].Header.VisiblePosition = 1;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].Header.Fixed = true;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["Venue"].NullText = "(Select Venue)";

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].Header.Caption = "Algo Type";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].ValueList = cmbAlgoType;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].CellActivation = Activation.AllowEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].Header.VisiblePosition = 2;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].Header.Fixed = true;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["AlgoType"].NullText = "(Select Algo Type)";

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].Header.Caption = "Commission Basis";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].ValueList = commissionDropDown;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].CellActivation = Activation.AllowEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].Header.VisiblePosition = 3;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].Header.Fixed = true;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["CommissionBasis"].NullText = "(Select Commission Basis)";

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].Header.Caption = "Soft Commission Basis";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].ValueList = commissionDropDown;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].CellActivation = Activation.AllowEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].Header.VisiblePosition = 4;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].Header.Fixed = true;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["SoftCommissionBasis"].NullText = "(Select Soft Commission Basis)";

                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].Header.Caption = "Execution Instruction";
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].ValueList = executionInstructionDropDown;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].CellActivation = Activation.AllowEdit;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].Header.VisiblePosition = 5;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].Header.Fixed = true;
                grdBrokerWiseSettings.DisplayLayout.Bands[0].Columns["ExecutionInstruction"].NullText = "(Select Execution Instruction)";

                grdBrokerWiseSettings.CellDataError += grdBrokerWiseSettings_CellDataError;
                grdBrokerWiseSettings.Error += grdBrokerWiseSettings_Error;
                grdBrokerWiseSettings.AfterCellUpdate += grdBrokerWiseSettings_AfterCellUpdate;
                grdBrokerWiseSettings.ClickCell += grdBrokerWiseSettings_ClickCell;
                grdBrokerWiseSettings.AfterCellListCloseUp += grdBrokerWiseSettings_AfterCellListCloseUp;
                grdBrokerWiseSettings.CellChange += grdBrokerWiseSettings_CellChange;
                grdBrokerWiseSettings.InitializeRow += grdBrokerWiseSettings_InitializeRow;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// grdBrokerWiseSettings InitializeRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBrokerWiseSettings_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.Columns["Venue"].Key.Equals("Venue"))
            {
                var value = e.Row.Cells["Venue"].Text;
                if (value != null && !value.Equals("Algo"))
                {
                    e.Row.Cells["AlgoType"].Activation = Activation.Disabled;
                }
            }
        }

        /// <summary>
        /// grdBrokerWiseSettings CellChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBrokerWiseSettings_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                string column = e.Cell.Column.Key;
                string columnText = e.Cell.Text;
                if (columnText.Equals("Algo") && column.Equals("Venue"))
                {
                    this.grdBrokerWiseSettings.ActiveRow.Cells["AlgoType"].Activation = Activation.AllowEdit;
                }
                else if (!columnText.Equals("Algo") && column.Equals("Venue"))
                {
                    this.grdBrokerWiseSettings.ActiveRow.Cells["AlgoType"].Activation = Activation.Disabled;
                    this.grdBrokerWiseSettings.ActiveRow.Cells["AlgoType"].Value = int.MinValue;
                }
                BindPermittedVenueAlgoTypes();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// grdBrokerWiseSettings AfterCellListCloseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBrokerWiseSettings_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            try
            {
                cmbVenue.DataSource = CachedDataManager.GetInstance.GetAllVenues();
                cmbAlgoType.DataSource = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().AlgoStrategyNames;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// grdBrokerWiseSettings ClickCell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBrokerWiseSettings_ClickCell(object sender, EventArgs e)
        {
            BindPermittedVenueAlgoTypes();
        }

        private void BindPermittedVenueAlgoTypes()
        {
            try
            {
                int brokerId = Convert.ToInt32(grdBrokerWiseSettings.ActiveRow.Cells["CounterPartyID"].Value);
                if (this.grdBrokerWiseSettings.ActiveCell != null)
                {
                    if (this.grdBrokerWiseSettings.ActiveCell.Column.Key.Equals("Venue"))
                    {
                        //this.grdBrokerWiseSettings.ActiveCell.Column.CellDisplayStyle.ScrollStyle = ScrollStyle.Immediate;
                        VenueCollection permittedAlgo = TradingTicketPreferenceDataManager.GetVenues(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, brokerId, TradingTicketPreferenceType.User);
                        _venues.Clear();

                        foreach (Venue item in permittedAlgo)
                        {
                            if (!_venues.ContainsKey(item.VenueID))
                                _venues.Add(item.VenueID, item.Name);
                        }
                        if (_venues.Count == 0)
                        {
                            _venues.Add(int.MinValue, "-Select Venue-");
                        }
                        cmbVenue.DataSource = _venues;

                    }
                    else if (this.grdBrokerWiseSettings.ActiveCell.Column.Key.Equals("AlgoType"))
                    {
                        cmbAlgoType.DataSource = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetCPWiseAllStrategies(brokerId.ToString());
                    }
                    else
                    {
                        cmbVenue.DataSource = CachedDataManager.GetInstance.GetAllVenues();
                        cmbAlgoType.DataSource = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().AlgoStrategyNames;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// grdBrokerWiseSettings Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdBrokerWiseSettings_Error(object sender, ErrorEventArgs e)
        {
            e.ErrorText = "The value entered is invalid. Please select a valid value";
        }
        /// <summary>
        /// grdBrokerWiseSettings_CellDataError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdBrokerWiseSettings_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RestoreOriginalValue = true;
            e.StayInEditMode = true;
        }
        /// <summary>
        /// grdBrokerWiseSettings_AfterCellUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBrokerWiseSettings_AfterCellUpdate(object sender, CellEventArgs e)
        {

            // If the cell is null or not activated return, no further action is needed
            if ((!e.Cell.IsActiveCell) || e.Cell.Value == DBNull.Value)
                return;


            if (!ValueListUtilities.CheckIfValueExistsInValuelist(e.Cell.Column.ValueList, e.Cell.Value.ToString()))
            {

                e.Cell.Value = DBNull.Value;
            }
        }

        /// <summary>
        /// To export data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(grdBrokerWiseSettings, exportFilePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}