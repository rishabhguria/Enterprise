using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    public class ImportCashFlow : IImport
    {
        DataTable _dt { get; set; }


        public ImportCashFlow(DataTable dt)
        {
            this._dt = dt;
        }

        /// <summary>
        /// Validate and Get the Validated List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<T> ValidateAndGetData<T>(ImportModel model) where T : class
        {
            if (_dt == null || _dt.Rows.Count <= 1 || _dt.Columns.Count < 2)
                return null;
            _dt.Rows[0].Delete();
            _dt.Columns[0].ColumnName = "Account Name";
            _dt.Columns[1].ColumnName = "Cash Flow";
            _dt.AcceptChanges();
            List<ImportCashFlowModel> importCashFlowList = new List<ImportCashFlowModel>();
            foreach (DataRow dr in _dt.Rows)
            {
                ImportCashFlowModel importCashFlowModel = new ImportCashFlowModel();
                decimal cash = 0;
                if (!Decimal.TryParse(dr[1].ToString(), out cash))
                {
                    importCashFlowModel.Comment = "Invalid Cash Flow.";
                }
                else if (string.IsNullOrWhiteSpace(dr[0].ToString()) || string.IsNullOrWhiteSpace(dr[1].ToString()))
                {
                    importCashFlowModel.Comment = "Either Account Name or Cash Flow missing.";
                }
                else if (!model.Accounts.Contains(dr[0].ToString().Trim()))
                {
                    importCashFlowModel.Comment = "Import Account Name does not match Account Name defined in Nirvana.";
                }

                importCashFlowModel.Cash = cash;
                if (string.IsNullOrWhiteSpace(importCashFlowModel.Comment))
                    importCashFlowModel.IsValid = true;
                else
                    importCashFlowModel.IsValid = false;

                importCashFlowModel.AccountName = dr[0].ToString().Trim();

                importCashFlowList.Add(importCashFlowModel);
            }

            return (List<T>)Convert.ChangeType(importCashFlowList, typeof(List<T>));
        }

        /// <summary>
        /// Dispose Data
        /// </summary>
        public void DisposeData()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
        /// Dispose the DataTable
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dt = null;
            }
        }
    }
}
