using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;

using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Prana.Utilities.XMLUtilities;
using Prana.Allocation.Common.Interfaces;
using Prana.WCFConnectionMgr;

namespace Prana.AllocationNew.Allocation.UI.Preferences
{
    public partial class CtrlMasterFundRatio : UserControl
    {
        public CtrlMasterFundRatio()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  initialize view
        /// </summary>
        public void IntializeControl(bool isMasterFundAllocationEnabled)
        {
            try
            {
                if (isMasterFundAllocationEnabled)
                {
                    chkbxMasterFundRatioAllocation.Checked = true;
                    grdMasterFunds.Enabled = true;

                    
                }
                else
                {
                    chkbxMasterFundRatioAllocation.Checked = false;
                    grdMasterFunds.Enabled = false;
                    
                }
                lblMessage.Text = "*Sum of allocation ratio should be 100.";
                lblMessage.Appearance.ForeColor = Color.Black ;
                DataSet dsMasterFunds = GetMasterFundTargetRatio();

                grdMasterFunds.DataSource = dsMasterFunds;

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

        /// <summary>
        ///  set style of columns
        /// </summary>
        private void SetGridColumns()
        {
            try
            {
                UltraGridColumn msAccountNameCol = grdMasterFunds.DisplayLayout.Bands[0].Columns[0];
                msAccountNameCol.CellActivation = Activation.NoEdit;
                msAccountNameCol.Header.Caption = "Master Fund";
                msAccountNameCol.Width = 190;
              
                UltraGridColumn msaccountIdCol = grdMasterFunds.DisplayLayout.Bands[0].Columns[1];
                msaccountIdCol.CellActivation = Activation.NoEdit;
                msaccountIdCol.Hidden = true;
               

                UltraGridColumn targetPctCol = grdMasterFunds.DisplayLayout.Bands[0].Columns[2];
                targetPctCol.CellActivation = Activation.AllowEdit;
                targetPctCol.Header.Caption = "Target Ratio";
                targetPctCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                targetPctCol.Width = 100;
                targetPctCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                grdMasterFunds.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdMasterFunds.DisplayLayout.GroupByBox.Hidden = true;
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

        /// <summary>
        /// Save MasterFund Target Ratio in DB
        /// </summary>
        public bool SaveMasterFundTargetRatio()
        {
            bool isSaved = false;
            try
            {

                int rowsAffected = 0;
                int errorNumber = 0;
                string errorMessage = string.Empty;
                if (grdMasterFunds.DataSource != null)
                {
                    DataSet ds = (DataSet)grdMasterFunds.DataSource;

                  Boolean isValid=   validateMasterFundRatios(ds);
                  if (isValid)
                  {
                      ds.Tables[0].TableName = "MasterFund";
                      string generatedXml = string.Empty;
                      generatedXml = ds.GetXml();
                      Database db = DatabaseFactory.CreateDatabase();
                      DbCommand cmd = new SqlCommand();
                      cmd.CommandText = "P_SaveMasterFundTargetRatio";
                      cmd.CommandType = CommandType.StoredProcedure;
                      db.AddInParameter(cmd, "@Xml", DbType.String, generatedXml);

                      XMLSaveManager.AddOutErrorParameters(db, cmd);

                      rowsAffected = db.ExecuteNonQuery(cmd);

                      XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, cmd);

                        lblMessage.Appearance.ForeColor = Color.Black;
                        isSaved = true;
                  }
                  else
                  {
                        MessageBox.Show(this, "Sum of Master Fund Ratio is not 100.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      lblMessage.Appearance.ForeColor = Color.Red;
                        isSaved = false;
                  }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
            return isSaved;
        }

        /// <summary>
        /// Validate sum of account ratio equal to 100
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private bool validateMasterFundRatios(DataSet ds)
        {
            try
            {
                float totalAllocationPct = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["TargetRatioPct"] != DBNull.Value)
                    {
                        float allocationPct = Convert.ToSingle(row["TargetRatioPct"]);
                    totalAllocationPct += allocationPct;
                }
                }

                if (totalAllocationPct == 100)

                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }


        }
       
        /// <summary>
        /// Get no of MasterFunds, PRANA-10389
        /// </summary>
        /// <returns></returns>
        public int GetNoOfMasterFunds()
        {
            int noOfMasterFunds = int.MinValue;
            try
            {
                noOfMasterFunds = grdMasterFunds.Rows.Count;
            }
            catch (Exception ex)
            {
                
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return noOfMasterFunds;
        }
        /// <summary>
        /// Get target ratio from DB
        /// </summary>
        /// <returns></returns>
        public DataSet GetMasterFundTargetRatio()
        {
            DataSet masterfndDs = new DataSet();
            try
            {
                masterfndDs =AllocationManager.GetInstance().GetAllMasterFunds();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return masterfndDs;
        }

        /// <summary>
        /// Handle on MasterFundRatioAllocation check box click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxMasterFundRatioAllocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxMasterFundRatioAllocation.Checked)
            {
                grdMasterFunds.Enabled = true;
            }
            else
            {
                grdMasterFunds.Enabled = false;
            }
        }

        /// <summary>
        /// return true or false status for Master fund target ration allocation scheme enabled/ not
        /// </summary>
        /// <returns></returns>
        internal bool GetIsMAsterAccountAllocEnabed()
        {
            try
            {
                return chkbxMasterFundRatioAllocation.Checked;
            }
            catch (Exception ex)
            {
               
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
