//using Prana.Reconciliation;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class RuleUserControl : UserControl
    {
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public RuleUserControl()
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
        public void SetUp(DataTable dt)
        {
            try
            {
                List<Prana.BusinessObjects.EnumerationValue> valueList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ComparisionType));
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("ID");
                dt1.Columns.Add("Name");
                foreach (Prana.BusinessObjects.EnumerationValue value in valueList)
                {
                    dt1.Rows.Add(new object[] { value.Value, value.DisplayText });
                }
                cmbbxMatchingType.DataSource = dt1;
                cmbbxMatchingType.DataBind();
                cmbbxMatchingType.DisplayMember = "Name";
                cmbbxMatchingType.ValueMember = "ID";
                cmbbxMatchingType.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                grdData.DataSource = dt;
                grdData.DataBind();
                grdData.DisplayLayout.Bands[0].Columns["MatchingType"].ValueList = cmbbxMatchingType;
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
