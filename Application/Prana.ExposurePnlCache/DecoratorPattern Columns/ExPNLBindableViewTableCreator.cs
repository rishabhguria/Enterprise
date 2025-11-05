using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.ExposurePnlCache
{
    public class ExPNLBindableViewTableCreator : TableEditor, IDisposable
    {
        private DataTable _modifiedTable;

        public ExPNLBindableViewTableCreator(IVariableColumnsTable mainTable)
            : base(mainTable)
        {
            _modifiedTable = new DataTable();
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
                if (col.DataType == typeof(Double))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(String))
                {
                    row[col] = String.Empty;
                    continue;
                }
                if (col.DataType == typeof(Int32))
                {
                    row[col] = 0;
                    continue;
                }
                if (col.DataType == typeof(PositionType))
                {
                    row[col] = PositionType.Long;
                }
            }
        }

        public void LoadColumnsFromXML()
        {
            try
            {
                _modifiedTable.Columns.Clear();
                _modifiedTable = base.TableWithColums.Copy();
                RemovePnlColumnBasedOnConfigValue();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            AddNewRow();
        }

        private void RemovePnlColumnBasedOnConfigValue()
        {
            try
            {
                if (!bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIGKEY_IsPerformanceNumberColumnsEnabled)))
                {
                    _modifiedTable.Columns.Remove("YTDReturn");
                    _modifiedTable.Columns.Remove("QTDReturn");
                    _modifiedTable.Columns.Remove("MTDReturn");
                    _modifiedTable.Columns.Remove("MTDPnL");
                    _modifiedTable.Columns.Remove("QTDPnL");
                    _modifiedTable.Columns.Remove("YTDPnL");
                }
                _modifiedTable.AcceptChanges();
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}




