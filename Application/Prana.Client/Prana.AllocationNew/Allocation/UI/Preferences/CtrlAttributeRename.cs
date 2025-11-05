using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Prana.BusinessObjects;
using Prana.Global;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Win.UltraWinGrid;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Prana.Utilities.XMLUtilities;

namespace Prana.AllocationNew.Allocation.UI.Preferences
{
    public partial class CtrlAttributeRename : UserControl
    {
        public CtrlAttributeRename()
        {
            InitializeComponent();
        }

        public void IntializeControl()
        {
            try
            {
                DataSet dsAttributes = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNames();
           //modified by -Faisal 
            if (dsAttributes == null || dsAttributes.Tables[0].Rows.Count == 0)
                {
                    dsAttributes = SetGridLayout();
                    gridAttirbuteNames.DataSource = dsAttributes;
                }
                else
                {
                    gridAttirbuteNames.DataSource = dsAttributes;
                }
                SetGridColumns();
            }
            catch (Exception ex)
            {
               bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DataSet SetGridLayout()
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                dt.Columns.Add("AttributeValue", typeof(string));
                dt.Columns.Add("AttributeName", typeof(string));
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute1, OrderFields.PROPERTY_TradeAttribute1);
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute2, OrderFields.PROPERTY_TradeAttribute2);
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute3, OrderFields.PROPERTY_TradeAttribute3);
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute4, OrderFields.PROPERTY_TradeAttribute4);
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute5, OrderFields.PROPERTY_TradeAttribute5);
                dt.Rows.Add(OrderFields.PROPERTY_TradeAttribute6, OrderFields.PROPERTY_TradeAttribute6);

            }
            catch (Exception ex)
            {
               bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return ds;
        }

        private void SetGridColumns()
        {
            try
            {
                UltraGridColumn attributeValue = gridAttirbuteNames.DisplayLayout.Bands[0].Columns[0];
                attributeValue.CellActivation = Activation.NoEdit;
                attributeValue.Header.Caption = "Attributes";
                attributeValue.Width = 200;


                UltraGridColumn attributeName = gridAttirbuteNames.DisplayLayout.Bands[0].Columns[1];
                attributeName.CellActivation = Activation.AllowEdit;
                attributeName.Header.Caption = "Name";
                attributeName.Width = 200;

                gridAttirbuteNames.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                gridAttirbuteNames.DisplayLayout.GroupByBox.Hidden = true;
            }
            catch (Exception ex)
            {
             bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        

        public void SaveAttributeNames()
        {
            try
            {
                int rowsAffected = 0;
                int errorNumber = 0;
                string errorMessage = string.Empty;
                if (gridAttirbuteNames.DataSource != null)
                {
                DataSet ds = (DataSet)gridAttirbuteNames.DataSource;
                ds.Tables[0].TableName = "Attributes";
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmd = new SqlCommand();
                cmd.CommandText = "P_SaveAttributeNames";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@Xml", DbType.String, generatedXml);

                XMLSaveManager.AddOutErrorParameters(db, cmd);

                rowsAffected = db.ExecuteNonQuery(cmd);

                XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, cmd);
                if(rowsAffected >0)
                CommonDataCache.CachedDataManager.RefreshAttibutesCache(ds);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

    }
}
