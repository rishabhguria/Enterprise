using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Tools
{
    class ReconUIUtilities
    {     /// <summary>
          /// Loads Data on the cmbThirdPartyFormat from db
          /// </summary>
        internal static void BindThirdPartyCombo(UltraCombo cmbThirdPartyFormat, string formatName, UltraToolTipInfo ultraToolTipInfo1, UltraToolTipManager ultraToolTipManager1)
        {
            try
            {
                Dictionary<string, RunUpload> runUploadValueList = ReconPreferences.DictRunUpload;
                if (runUploadValueList != null)
                {
                    cmbThirdPartyFormat.DataSource = runUploadValueList.Keys.ToArray();
                    cmbThirdPartyFormat.DisplayMember = "Value";
                    //cmbThirdPartyFormat.ValueMember = "Value";
                    //hides the column value
                    //cmbThirdPartyFormat.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                    cmbThirdPartyFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    //set column width equal to control
                    cmbThirdPartyFormat.DisplayLayout.Bands[0].Columns["Value"].Width = cmbThirdPartyFormat.Size.Width;

                    //set tool tip for every row
                    int index = 0;
                    foreach (UltraGridRow row in cmbThirdPartyFormat.Rows)
                    {
                        row.ToolTipText = row.Cells["Value"].Text;
                        index++;
                    }
                    //set the template preference value to combo box
                    if (string.IsNullOrEmpty(formatName))
                    {
                        formatName = "No File selected";
                    }
                    cmbThirdPartyFormat.Text = formatName;
                    ultraToolTipInfo1.ToolTipText = formatName;
                    //ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Standard;
                    ultraToolTipManager1.SetUltraToolTip(cmbThirdPartyFormat, ultraToolTipInfo1);
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
            // return null;

        }

        /// <summary>
        /// Bind Recon Date Type Combo
        /// </summary>
        /// <param name="cmbRunByDate"></param>
        /// <param name="reconDateType"></param>
        internal static void BindReconDateTypeCombo(UltraCombo cmbRunByDate, ReconDateType reconDateType)
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                // EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));


                listValues.Insert(0, new EnumerationValue("Trade Date", ReconDateType.TradeDate.ToString()));
                listValues.Insert(1, new EnumerationValue("Process Date", ReconDateType.ProcessDate.ToString()));
                listValues.Insert(2, new EnumerationValue("Nirvana Process Date", ReconDateType.NirvanaProcessDate.ToString()));


                cmbRunByDate.DataSource = null;
                cmbRunByDate.DataSource = listValues;
                cmbRunByDate.DisplayMember = "DisplayText";
                cmbRunByDate.ValueMember = "Value";
                cmbRunByDate.DataBind();
                cmbRunByDate.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbRunByDate.DisplayLayout.Bands[0].Header.Enabled = false;
                cmbRunByDate.Value = reconDateType;
                cmbRunByDate.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// <summary>
        /// Bind Template Combo
        /// </summary>
        /// <param name="templateKey"></param>
        /// <param name="cmbTemplate"></param>
        //added by amit on 21/04/2015
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
        public static void BindTemplateCombo(string reconTypeKey, Infragistics.Win.UltraWinGrid.UltraCombo cmbTemplate)
        {
            try
            {
                SerializableDictionary<string, ReconTemplate> _dictReconTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                foreach (KeyValuePair<string, ReconTemplate> kvp in _dictReconTemplates)
                {
                    //key in previous preference file is not as pr new key structure so key frm its template is checked
                    //modified by amit on 21/04/2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
                    if (kvp.Value.TemplateKey.StartsWith(reconTypeKey))
                    {
                        string templateName = ReconUtilities.GetTemplateNameFromTemplateKey(kvp.Key);
                        EnumerationValue value = new EnumerationValue(templateName, kvp.Key);
                        listValues.Add(value);
                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                listValues = listValues.OrderBy(e => e.DisplayText).ToList();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                cmbTemplate.DataSource = null;
                cmbTemplate.DataSource = listValues;
                cmbTemplate.DisplayMember = "DisplayText";
                cmbTemplate.ValueMember = "Value";
                cmbTemplate.DataBind();
                cmbTemplate.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbTemplate.DisplayLayout.Bands[0].Header.Enabled = false;
                cmbTemplate.Value = -1;
                cmbTemplate.DisplayLayout.Bands[0].ColHeadersVisible = false;
                int i = 0;
                //Narendra Kumar Jangir 2012/08/17 
                //show tooltip to show template name onmousehover of recotemplate combobox
                foreach (EnumerationValue item in listValues)
                {
                    cmbTemplate.Rows[i].ToolTipText = item.DisplayText;
                    i++;
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
        /// For comma separated values on grid and Export files.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="_reconTemplate"></param>
        internal static void CommaSeparatedMethod(InitializeLayoutEventArgs e, ReconTemplate _reconTemplate)
        {
            try
            {

                DataFilterHelper dataFilter = new DataFilterHelper();
                foreach (string col in _reconTemplate.RulesList[0].NumericFields)
                {
                    if (e.Layout.Bands[0].Columns.Exists(ReconConstants.CONST_Nirvana + col))
                    {
                        dataFilter.SetValues(ReconConstants.CONST_Nirvana + col, "#,0.####");
                        e.Layout.Bands[0].Columns[ReconConstants.CONST_Nirvana + col].Editor.DataFilter = dataFilter;
                    }
                    if (e.Layout.Bands[0].Columns.Exists(ReconConstants.CONST_Broker + col))
                    {
                        dataFilter.SetValues(ReconConstants.CONST_Broker + col, "#,0.####");
                        e.Layout.Bands[0].Columns[ReconConstants.CONST_Broker + col].Editor.DataFilter = dataFilter;
                    }
                    if (e.Layout.Bands[0].Columns.Exists(ReconConstants.CONST_OriginalValue + col))
                    {
                        dataFilter.SetValues(ReconConstants.CONST_OriginalValue + col, "#,0.####");
                        e.Layout.Bands[0].Columns[ReconConstants.CONST_OriginalValue + col].Editor.DataFilter = dataFilter;
                    }
                    if (e.Layout.Bands[0].Columns.Exists(ReconConstants.CONST_Diff + col))
                    {
                        dataFilter.SetValues(ReconConstants.CONST_Diff + col, "#,0.####");
                        e.Layout.Bands[0].Columns[ReconConstants.CONST_Diff + col].Editor.DataFilter = dataFilter;
                    }
                    if (e.Layout.Bands[0].Columns.Exists(col))
                    {
                        dataFilter.SetValues(col, "#,0.####");
                        e.Layout.Bands[0].Columns[col].Editor.DataFilter = dataFilter;
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
