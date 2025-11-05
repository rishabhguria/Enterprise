using System;
using System.Data;


namespace Prana.ExposurePnlCache
{
    public abstract class TableEditor : IVariableColumnsTable
    {

        private IVariableColumnsTable _table;
        private IVariableColumnsTable Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
            }
        }

        public TableEditor(IVariableColumnsTable table)
        {
            this._table = table;
        }



        #region IVariableColumnsTable Members

        public virtual DataTable TableWithColums
        {
            get
            {
                return _table.TableWithColums;
            }
            set
            {
                _table.TableWithColums = value;
            }
        }

        public virtual Type ObjectForDataTable
        {
            get
            {
                return _table.ObjectForDataTable;
            }
            set
            {
                _table.ObjectForDataTable = value;
            }
        }

        #endregion
    }
}
