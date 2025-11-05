using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    public class ImportCustomGroup : IImport, IDisposable
    {
        DataTable _dt { get; set; }

        public ImportCustomGroup(DataTable dt)
        {
            this._dt = dt;
        }

        public List<T> ValidateAndGetData<T>(ImportModel model) where T : class
        {
            if (_dt == null || _dt.Rows.Count <= 1 || _dt.Columns.Count < 2)
                return null;
            _dt.Rows[0].Delete();
            _dt.Columns[0].ColumnName = "CustomGroup Name";
            _dt.Columns[1].ColumnName = "Account Name";
            _dt.AcceptChanges();
            List<ImportCustomGroupModel> lst = new List<ImportCustomGroupModel>();
            foreach (DataRow dr in _dt.Rows)
            {
                ImportCustomGroupModel importCustomGroupModel = new ImportCustomGroupModel();
                if (string.IsNullOrWhiteSpace(dr[0].ToString()) || string.IsNullOrWhiteSpace(dr[1].ToString()))
                {
                    importCustomGroupModel.Comment = "Either CustomGroup Name or Account Name missing.";
                }
                else if (model.CustomGroups.Contains(dr[0].ToString().Trim()))
                {
                    importCustomGroupModel.Comment = "Custom Group with this name already exists.";
                }
                else if (!model.Accounts.Contains(dr[1].ToString().Trim()))
                {
                    importCustomGroupModel.Comment = "Import Account Name does not match Account Name defined in Nirvana.";
                }

                if (string.IsNullOrWhiteSpace(importCustomGroupModel.Comment))
                    importCustomGroupModel.IsValid = true;
                else
                    importCustomGroupModel.IsValid = false;

                importCustomGroupModel.CustomGroupName = dr[0].ToString().Trim();
                importCustomGroupModel.AccountName = dr[1].ToString().Trim();

                lst.Add(importCustomGroupModel);
            }
            return (List<T>)Convert.ChangeType(lst, typeof(List<T>));
        }


        public void DisposeData()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dt = null;
            }
        }
    }
}
