using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.HeatMap.ChildWindows
{
    /// <summary>
    /// Just a form wrapper for grouping control
    /// </summary>
    public partial class HeatMapGrouping : Form
    {
        public HeatMapGrouping()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allpy the theme on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeatMapGrouping_Load(object sender, EventArgs e)
        {
            try
            {
                HandelTheming();
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
        /// Get the current grouping
        /// </summary>
        /// <returns></returns>
        public List<String> GetSortSettings()
        {
            try
            {
                return groupingToolCntrl.GetSortSettings();
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
                return null;
            }
        }

        /// <summary>
        /// apply a grouping on the UI
        /// </summary>
        /// <param name="keys"></param>
        public void ApplyGroup(List<String> keys)
        {
            try
            {
                groupingToolCntrl.ApplyGroup(keys);
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
        /// Set the list of attributes that can be grouped
        /// </summary>
        /// <param name="list"></param>
        public void SetAttributes(List<String> list)
        {

            try
            {
                groupingToolCntrl.SetAttributes(list);
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
