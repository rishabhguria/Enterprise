using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Prana.ExposurePnlCache
{
    public class BaseDataTable : IVariableColumnsTable, IDisposable
    {
        #region IVariableColumnsTable Members
        private DataTable _baseDataTable;

        public DataTable TableWithColums
        {
            get
            {
                if (_baseDataTable == null)
                {
                    _baseDataTable = new DataTable();
                    PropertyInfo[] properties = _objectForDataTable.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (!_baseDataTable.Columns.Contains(property.Name))
                        {
                            DataColumn col = new DataColumn
                            {
                                DataType = property.PropertyType,
                                ColumnName = property.Name
                            };
                            //TODO : Add Custom Attributes
                            object[] attributes = property.GetCustomAttributes(false);
                            if (attributes.Length == 0)
                            {
                                continue;
                            }
                            BrowsableAttribute browsableAttrib = attributes[0] as BrowsableAttribute;

                            bool browsable = true;
                            if (browsableAttrib != null)
                            {
                                browsable = browsableAttrib.Browsable;
                            }

                            if (browsable)
                            {
                                ColPropertyAttribute c = (ColPropertyAttribute)attributes[0];
                                col.Caption = c.Caption;
                                col.ExtendedProperties.Add("Format", ((ColPropertyAttribute)(attributes[0])).Format);
                                col.ExtendedProperties.Add("OnlyInColChooser", ((ColPropertyAttribute)(attributes[0])).OnlyInColChooser);
                                col.ExtendedProperties.Add("Hidden", "false");

                            }
                            else
                            {
                                col.ExtendedProperties.Add("Hidden", "true");
                            }
                            _baseDataTable.Columns.Add(col);
                        }
                    }
                }
                return _baseDataTable;

            }
            set
            {
                _baseDataTable = value;
            }
        }


        private Type _objectForDataTable;

        public Type ObjectForDataTable
        {
            get
            {
                return _objectForDataTable;
            }
            set
            {
                _objectForDataTable = value;
            }
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (_baseDataTable != null && isDisposing)
                {
                    _baseDataTable.Dispose();
                    _baseDataTable = null;
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
    }
}
