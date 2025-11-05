using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class PricingDataLookUp : Form, IPluggableTools
    {
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public PricingDataLookUp()
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

        #region IPluggableTools Members

        public ISecurityMasterServices _securityMaster { get; set; }

        public void SetUP()
        {

        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        private void PricingDataLookUp_Load(object sender, EventArgs e)
        {
            try
            {
                BindFields();
                DisableEdit();
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetButtonsColor()
        {
            try
            {
                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// To disable editing in grid
        /// 
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void DisableEdit()
        {
            try
            {
                this.grdPricingData.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
                this.grdPricingData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                this.grdPricingData.DisplayLayout.Override.HeaderAppearance.Cursor = System.Windows.Forms.Cursors.Arrow;
                this.grdPricingData.DisplayLayout.Override.RowSizing = RowSizing.Fixed;
                this.grdPricingData.DisplayLayout.Override.AllowColSizing = AllowColSizing.None;
                this.grdPricingData.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// To bind pricing fields
        /// </summary>
        private void BindFields()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                cmbField.NullText = "-Select-";
                ConcurrentDictionary<string, StructPricingField> dictFields = SMBatchManager.GetSecurityFields();
                foreach (KeyValuePair<string, StructPricingField> kvp in dictFields)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value.FieldName, kvp.Key);
                    listValues.Add(value);
                }
                cmbField.DataSource = null;
                cmbField.DataSource = listValues;

                if (cmbField.DataSource != null)
                {
                    if (!cmbField.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbBatch = cmbField.DisplayLayout.Bands[0].Columns.Add();
                        cbBatch.Key = "Selected";
                        cbBatch.Header.Caption = string.Empty;
                        cbBatch.Width = 25;
                        cbBatch.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbBatch.DataType = typeof(bool);
                        cbBatch.Header.VisiblePosition = 0;
                    }
                    cmbField.CheckedListSettings.CheckStateMember = "Selected";
                    cmbField.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbField.CheckedListSettings.ListSeparator = " , ";
                    cmbField.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbField.DisplayMember = "DisplayText";
                    cmbField.ValueMember = "Value";
                    cmbField.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                    cmbField.Value = -1;
                    cmbField.DisplayLayout.Bands[0].Columns["DisplayText"].Header.Caption = "Select All";
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
        /// To get pricing data according to condition filters.
        /// </summary>
        private void GetPricingData()
        {
            try
            {
                StringBuilder fieldName = new StringBuilder();
                string symbol = txtSearchSymbol.Text.Trim();
                DateTime startDate = cmbStartDate.DateTime.Date;
                DateTime endDate = cmbEndDate.DateTime.Date;

                List<object> listFields = (List<object>)cmbField.Value;
                foreach (string field in listFields)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        if (fieldName.Length > 0)
                        {
                            fieldName.Append(",");
                        }
                        fieldName.Append(field);
                    }
                }
                DataTable dtSMBatchData = PricingDataDAL.GetPricingDetails(symbol, fieldName, startDate, endDate);
                grdPricingData.DataSource = dtSMBatchData;
                grdPricingData.DataBind();
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
        /// To fetch pricing data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                GetPricingData();
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

        private void PricingDataLookUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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
    }
}
