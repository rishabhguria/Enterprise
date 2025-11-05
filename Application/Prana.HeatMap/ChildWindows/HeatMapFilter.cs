using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.HeatMap.BLL;
using Prana.HeatMap.Enums;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.HeatMap.ChildWindows
{
    public partial class HeatMapFilter : Form
    {
        List<EnumerationValue> _fieldsNamesList = new List<EnumerationValue>();

        /// <summary>
        /// Loads the values for drop down and buils the attribute cache
        /// </summary>
        public HeatMapFilter()
        {
            try
            {
                InitializeComponent();
                _fieldsNamesList = EnumHelper.ConvertEnumForBindingWithCaption(typeof(GroupingAttributes));

                // update or build the attribute cache
                foreach (GroupingAttributes attri in Enum.GetValues(typeof(GroupingAttributes)))
                {
                    GroupAttributesCache.GetInstance().UpdateCache(attri, DataCache.GetInstance().GetDistinctColumnValues(attri));
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

        /// <summary>
        /// add a new condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SearchingCntrl cntrl = new SearchingCntrl();
                cntrl.SetUp(_fieldsNamesList);
                cntrl.RemoveConditionEvent += cntrl_RemoveConditionEvent;
                cntrl.GetColumnTypeReq += cntrl_GetColumnTypeReq;

                ultraPanel1.ClientArea.Controls.Add(cntrl);
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

        /// <summary>
        /// Updates the values for an attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cntrl_GetColumnTypeReq(object sender, EventArgs<string> e)
        {
            try
            {
                SearchingCntrl searchCntrl = sender as SearchingCntrl;
                if (searchCntrl != null)
                {
                    GroupingAttributes attribute = (GroupingAttributes)Enum.Parse(typeof(GroupingAttributes), e.Value, true);
                    searchCntrl.SetConditionControls("Enum", GroupAttributesCache.GetInstance().GetValuelist(attribute));
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

        /// <summary>
        /// Removes a filter condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cntrl_RemoveConditionEvent(object sender, EventArgs<string> e)
        {
            try
            {
                if (ultraPanel1.ClientArea.Controls.ContainsKey(e.Value))
                {
                    Control cntrl = ultraPanel1.ClientArea.Controls[e.Value];
                    cntrl.Dispose();
                    ultraPanel1.ClientArea.Controls.Remove(cntrl);
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

        /// <summary>
        ///  save the current filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                List<SearchCondition> conditions = new List<SearchCondition>();
                foreach (Control cntrl in ultraPanel1.ClientArea.Controls)
                {
                    SearchingCntrl searchControls = cntrl as SearchingCntrl;
                    if (searchControls != null)
                    {
                        SearchCondition condition = searchControls.GetSearchCondition();
                        conditions.Add(condition);
                    }
                }
                HeatMapManager.GetInstance().UpdateFilterCache(conditions);
                this.Close();
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

        /// <summary>
        /// display the current filters on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeatMapFilter_Load(object sender, EventArgs e)
        {
            try
            {
                HandelTheming();
                List<SearchCondition> conditions = HeatMapManager.GetInstance().GetFiltercache();
                foreach (SearchCondition condition in conditions)
                {
                    if (condition.FieldValue == null)
                        continue;

                    SearchingCntrl cntrl = new SearchingCntrl();
                    cntrl.SetUp(_fieldsNamesList);

                    GroupingAttributes attribute = (GroupingAttributes)Enum.Parse(typeof(GroupingAttributes), condition.FieldName, true);
                    cntrl.PutValues("Enum", GroupAttributesCache.GetInstance().GetValuelist(attribute), condition.AndOr, condition.FieldName, condition.FieldValue);

                    cntrl.RemoveConditionEvent += cntrl_RemoveConditionEvent;
                    cntrl.GetColumnTypeReq += cntrl_GetColumnTypeReq;

                    ultraPanel1.ClientArea.Controls.Add(cntrl);
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

        /// <summary>
        /// Handels the Prana theme
        /// </summary>
        private void HandelTheming()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_HEAT_MAP);
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
    }
}
