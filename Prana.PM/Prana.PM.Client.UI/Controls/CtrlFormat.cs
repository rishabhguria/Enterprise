using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.PM.Client.UI.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace Prana.PM.Client.UI
{
    public partial class CtrlFormat : UserControl
    {
        public CtrlFormat()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            try
            {
                DataSet ds = DataSetSchema();
                BindGridDataSource(ds);
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

        private void BindGridDataSource(DataSet ds)
        {
            try
            {
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                if (pMAppearances.DecimalPlaceLimitsForColumns != null)
                {
                    foreach (KeyValuePair<string, int> DecLimDict in pMAppearances.DecimalPlaceLimitsForColumns)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["ColumnNames"] = DecLimDict.Key.ToString();
                        dr["NumberOfDecimalDigits"] = Convert.ToInt16(DecLimDict.Value.ToString());
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
                else
                {
                    Type type = typeof(ExposurePnlCacheItem);
                    PropertyInfo[] propertyList = type.GetProperties();

                    foreach (PropertyInfo property in propertyList)
                    {
                        if (property.PropertyType == typeof(System.Double) || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            bool isBrowsableFalse = false;
                            object[] att = property.GetCustomAttributes(typeof(BrowsableAttribute), false);
                            if (att != null && att.Length > 0)
                            {
                                BrowsableAttribute b = att[0] as BrowsableAttribute;
                                if (!b.Browsable)
                                {
                                    isBrowsableFalse = true;
                                }
                            }

                            if (!isBrowsableFalse)
                            {
                                DataRow dr = ds.Tables[0].NewRow();
                                string caption = PMConstantsHelper.GetCaptionByColumnName(property.Name);
                                int numOfDecPlaces = PMConstantsHelper.GetDefaultCloumnWiseDecimalDigits(caption);

                                if (!ds.Tables[0].Columns.Contains(property.Name) && !caption.Equals(string.Empty))
                                {
                                    dr["ColumnNames"] = caption;
                                    dr["NumberOfDecimalDigits"] = numOfDecPlaces;
                                    ds.Tables[0].Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                grdFormat.DataSource = ds;
                grdFormat.DisplayLayout.Bands[0].Columns[0].SortIndicator = SortIndicator.Ascending;
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

        private DataSet DataSetSchema()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("Columns");
                ds.Tables[0].Columns.Add("ColumnNames");
                ds.Tables[0].Columns.Add("NumberOfDecimalDigits");

                return ds;
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
                return null;
            }
        }

        private void grdFormat_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridColumn colNumberOfDecimalDidgits = grdFormat.DisplayLayout.Bands[0].Columns["NumberOfDecimalDigits"];
                colNumberOfDecimalDidgits.Header.VisiblePosition = 1;
                colNumberOfDecimalDidgits.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegativeWithSpin;
                colNumberOfDecimalDidgits.Header.Caption = "Number Of Decimal Digits";
                colNumberOfDecimalDidgits.Width = 150;
                colNumberOfDecimalDidgits.MaxValue = 12;
                colNumberOfDecimalDidgits.NullText = "0";

                UltraGridColumn colNames = grdFormat.DisplayLayout.Bands[0].Columns["ColumnNames"];
                colNames.Header.VisiblePosition = 0;
                colNames.Header.Caption = "Columns";
                colNames.Width = 200;
                colNames.CellActivation = Activation.NoEdit;

                grdFormat.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
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


        internal void SaveFormat(PMAppearances pMAppearances)
        {
            try
            {
                string key = string.Empty;
                int value = 0;
                pMAppearances.DecimalPlaceLimitsForColumns = new SerializableDictionary<string, int>();
                foreach (UltraGridRow row in grdFormat.Rows)
                {
                    key = row.Cells["ColumnNames"].Value.ToString();
                    if (row.Cells["NumberOfDecimalDigits"].Value.ToString() != string.Empty)
                    {
                        value = Convert.ToInt16((row.Cells["NumberOfDecimalDigits"]).Value.ToString());
                    }
                    if (!key.Equals(string.Empty) && !pMAppearances.DecimalPlaceLimitsForColumns.ContainsKey(key) && value <= 12)
                    {
                        pMAppearances.DecimalPlaceLimitsForColumns.Add(key, value);
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
    }
}