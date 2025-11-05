using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace Prana.ExposurePnlCache
{
    public class ExPNLBindableViewTableCreatorForSecCurve : TableEditor, IDisposable
    {
        private List<string> _additionalAccountColumns;

        public List<string> AdditionalAccountColumns
        {
            get { return _additionalAccountColumns; }
        }

        private DataTable _modifiedTable;

        public ExPNLBindableViewTableCreatorForSecCurve(IVariableColumnsTable mainTable)
            : base(mainTable)
        {
            _modifiedTable = new DataTable();
            _additionalAccountColumns = new List<string>();
        }

        public override DataTable TableWithColums
        {
            get
            {
                return _modifiedTable.Copy();
            }
            set
            {
                base.TableWithColums = value;
            }
        }

        public override Type ObjectForDataTable
        {
            get
            {
                return base.ObjectForDataTable;
            }
            set
            {
                base.ObjectForDataTable = value;
            }
        }

        internal void AddNewRow()
        {
            if (_modifiedTable != null)
            {
                if (_modifiedTable.Rows.Count == 0)
                {
                    DataRow row = _modifiedTable.NewRow();
                    InitializeRow(row);
                    _modifiedTable.Rows.Add(row);
                }
                else if (_modifiedTable.Rows.Count > 0)
                {
                    _modifiedTable.Rows.Clear();
                    DataRow row = _modifiedTable.NewRow();
                    InitializeRow(row);
                    _modifiedTable.Rows.Add(row);
                }
            }
        }

        private void InitializeRow(DataRow row)
        {
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.DataType == typeof(System.Double))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(System.String))
                {
                    row[col] = String.Empty;
                    continue;
                }
                if (col.DataType == typeof(System.Int32))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(PositionType))
                {
                    row[col] = PositionType.Long;
                    continue;
                }
            }
        }

        public void LoadColumnsFromXML(string sectionName)
        {
            try
            {
                _modifiedTable.Columns.Clear();
                _modifiedTable = base.TableWithColums.Copy();
                DataColumn newColToBeAdded = null;
                string pathforXML = Application.StartupPath + "\\" + Prana.Global.ApplicationConstants.PREFS_FOLDER_NAME + "\\" + @"AdditionalColumns.xml"; ;

                if (System.IO.File.Exists(pathforXML))
                {
                    XmlTextReader myxmlreader = new XmlTextReader(pathforXML);
                    while (myxmlreader.Read())
                    {
                        myxmlreader.MoveToContent();
                        switch (sectionName)
                        {
                            case "AdditionalAccountColumnsForConsolidation":

                                if (myxmlreader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (myxmlreader.Name == "Column")
                                    {
                                        _additionalAccountColumns.Add(newColToBeAdded.ColumnName);

                                        AddColumnForAllAccounts(newColToBeAdded);
                                    }
                                }
                                if (myxmlreader.NodeType == System.Xml.XmlNodeType.Element)
                                {
                                    switch (myxmlreader.Name)
                                    {
                                        case "Column":
                                            newColToBeAdded = new DataColumn();
                                            break;
                                        case "Name":
                                            newColToBeAdded.ColumnName = myxmlreader.ReadString();
                                            break;
                                        case "Hidden":
                                            newColToBeAdded.ExtendedProperties.Add("Hidden", bool.Parse(myxmlreader.ReadString()));
                                            break;
                                        case "Format":
                                            newColToBeAdded.ExtendedProperties.Add("Format", myxmlreader.ReadString());
                                            break;
                                        case "OnlyInColChooser":
                                            newColToBeAdded.ExtendedProperties.Add("OnlyInColChooser", bool.Parse(myxmlreader.ReadString()));
                                            break;
                                        case "Caption":
                                            newColToBeAdded.Caption = myxmlreader.ReadString();
                                            break;
                                        case "DataType":
                                            newColToBeAdded.DataType = Type.GetType(myxmlreader.ReadString());
                                            break;
                                        case "IsIndexColumn":
                                            newColToBeAdded.ExtendedProperties.Add("IsIndexColumn", bool.Parse(myxmlreader.ReadString()));
                                            break;
                                    }
                                }
                                break;

                            case "AdditionalConsolidationColumnsForAccount":
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            AddNewRow();
        }

        private void AddColumnForAllAccounts(DataColumn newColToBeAdded)
        {
            int performanceNumberColumnsForAccountOrMasterFund = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_PerformanceNumberColumnsForAccountOrMasterFund));
            if (performanceNumberColumnsForAccountOrMasterFund == 1)
            {
                foreach (BusinessObjects.Account existingAccount in Prana.CommonDataCache.WindsorContainerManager.GetMasterFunds())
                {
                    AddSingleColumnForAccountOrMasterFund(existingAccount, newColToBeAdded);
                }
            }
            else
            {
                foreach (BusinessObjects.Account existingAccount in Prana.CommonDataCache.WindsorContainerManager.GetAccounts())
                {
                    AddSingleColumnForAccountOrMasterFund(existingAccount, newColToBeAdded);
                }
            }
        }

        private void AddSingleColumnForAccountOrMasterFund(Prana.BusinessObjects.Account existingAccount, DataColumn newColToBeAdded)
        {
            try
            {
                DataColumn colTemp = new DataColumn();
                colTemp.Caption = existingAccount.Name + " " + newColToBeAdded.Caption;
                colTemp.ColumnName = existingAccount.Name + " " + newColToBeAdded.ColumnName;
                colTemp.DataType = newColToBeAdded.DataType;
                colTemp.ExtendedProperties.Add("Hidden", newColToBeAdded.ExtendedProperties["Hidden"]);
                colTemp.ExtendedProperties.Add("Format", newColToBeAdded.ExtendedProperties["Format"]);
                colTemp.ExtendedProperties.Add("IsIndexColumn", newColToBeAdded.ExtendedProperties["IsIndexColumn"]);
                colTemp.ExtendedProperties.Add("OnlyInColChooser", newColToBeAdded.ExtendedProperties["OnlyInColChooser"]);
                _modifiedTable.Columns.Add(colTemp);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (_modifiedTable != null && isDisposing)
                {
                    _modifiedTable.Dispose();
                    _modifiedTable = null;
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
        #endregion
    }
}
