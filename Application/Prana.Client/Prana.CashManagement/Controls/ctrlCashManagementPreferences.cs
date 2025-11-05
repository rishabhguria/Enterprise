using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using System.Configuration;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.CashManagement
{
    public partial class ctrlCashManagementPreferences : UserControl, IPreferencesSavedClicked
    {
        private CashPreferences _objCashPreferences;

        public CashPreferences ObjCashPreferences
        {
            get { return _objCashPreferences; }
            set { _objCashPreferences = value; }
        }    
       
        public ctrlCashManagementPreferences()
        {
            try
            {
                InitializeComponent();               
                //commented by: Bharat raturi, 04 aug 2014
                //control not in use and the getcashpreferences() method has been redefined
                //ObjCashPreferences = CashDataManager.GetInstance().GetCashPreferences();
                //chkbxBond.Checked = ObjCashPreferences.IsCalculateBondAccurals;
                //chkbxBond.Enabled = false;
                //chkbxDividend.Checked = ObjCashPreferences.IsCalculateDividend;
                //chkbxDividend.Enabled = false;
                //chkbxPnL.Checked = ObjCashPreferences.IsCalculatePnL;
                //chkbxPnL.Enabled = false;
                //chkbxRevalPublish.Checked = ObjCashPreferences.IsPublishRevaluationData;
                //uDtCashManagementStartDate.DateTime = ObjCashPreferences.CashMgmtStartDate.Date;
                //txtMarginPercentage.Text = ObjCashPreferences.MarginPercentage.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        #region IPreferences Members

        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            //All The Setup work 4 this form;
        }

        public UserControl Reference()
        {
            return this;
        }

        public bool Save()
        {
            try
            {
                //This Code must be moved to DAL layer 
                //ProxyBase<ICashManagementService> _proxyCashManagementServices = null;
                //string endpointAddressInString = ConfigurationManager.AppSettings["CashManagementEndpointAddress"];
                //_proxyCashManagementServices = new ProxyBase<ICashManagementService>(endpointAddressInString);
                bool isSaved = true;
                if (ObjCashPreferences != null)
                {
                    ObjCashPreferences.CashMgmtStartDate = uDtCashManagementStartDate.DateTime;
                    ObjCashPreferences.MarginPercentage =double.Parse(txtMarginPercentage.Text);
                    ObjCashPreferences.IsCalculateBondAccurals = chkbxBond.Checked;
                    ObjCashPreferences.IsCalculateDividend = chkbxDividend.Checked;
                    ObjCashPreferences.IsCalculatePnL = chkbxPnL.Checked;
                    ObjCashPreferences.IsPublishRevaluationData = chkbxRevalPublish.Checked;

                    isSaved = CashDataManager.GetInstance().SaveCashPreferences(ObjCashPreferences);

                    //ObjStartDate.StartDate = uDtCashManagementStartDate.DateTime;
                    //marginPercent = txtMarginPercentage.Text;
                    //isSaved = CashDataManager.GetInstance().SaveCashMgmtStartDate(ObjStartDate);
                    //isSaved = CashDataManager.GetInstance().SaveMarginPercent(marginPercent);
                    
                }

                //_proxyCashManagementServices.Dispose();
                if (isSaved)
                    MessageBox.Show("Cash Management Preferences Saved !");
                else
                    MessageBox.Show("Server Side Error Occured ! Please Contract System Administrator!");
                //Implementaion Of Save Code for T_CashmgtStartDate Table
                return isSaved;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return false;
        }

        public void RestoreDefault()
        {
            if (ObjCashPreferences != null)
            {
                uDtCashManagementStartDate.DateTime = ObjCashPreferences.CashMgmtStartDate;
                txtMarginPercentage.Text = ObjCashPreferences.MarginPercentage.ToString();
            }
        }

        public IPreferenceData GetPrefs()
        {
            CashManagementPreferences Preferences = new CashManagementPreferences();
            return Preferences;
        }

        public event EventHandler SaveClicked;

        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }



        #endregion



        private void chkbx_MainOption_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            if (chkbx_MainOption.CheckState==CheckState.Checked)
            {
                chkbxBond.Checked = true;
                chkbxDividend.Checked = true;
                chkbxPnL.Checked = true;
                    chkbxRevalPublish.Checked = false;
            }
            else if(chkbx_MainOption.CheckState==CheckState.Unchecked)
            {
                chkbxBond.Checked = false;
                chkbxDividend.Checked = false;
                chkbxPnL.Checked = false;
                    chkbxRevalPublish.Checked = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void ChangeStatus_chkbxMainOption()
        {
            try
            {
                if (chkbxPnL.Checked == false && chkbxDividend.Checked == false && chkbxBond.Checked == false && chkbxRevalPublish.Checked == false)
                {
                    //event is unwired and wired to stop recursion of method
                    this.chkbx_MainOption.CheckedChanged -= new System.EventHandler(this.chkbx_MainOption_CheckedChanged);
                chkbx_MainOption.Checked = false;
                    this.chkbx_MainOption.CheckedChanged += new System.EventHandler(this.chkbx_MainOption_CheckedChanged);
                }
            else
                {
                    //event is unwired and wired to stop recursion of method
                    this.chkbx_MainOption.CheckedChanged -= new System.EventHandler(this.chkbx_MainOption_CheckedChanged);
                chkbx_MainOption.CheckState = CheckState.Checked;
                    this.chkbx_MainOption.CheckedChanged += new System.EventHandler(this.chkbx_MainOption_CheckedChanged);
                }

                //else if (chkbxPnL.Checked == true && chkbxDividend.Checked == true && chkbxBond.Checked == true)
                //    chkbx_MainOption.CheckState = CheckState.Checked;
                //else
                //    chkbx_MainOption.CheckState = CheckState.Indeterminate;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void chkbxPnL_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            ChangeStatus_chkbxMainOption();
        }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
        
        private void chkbxDividend_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            ChangeStatus_chkbxMainOption();
        }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void chkbxBond_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            ChangeStatus_chkbxMainOption();
        }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
        
        private void grdActivityJournalMapping_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void grdActivityJournalMapping_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

        }
        
        private void chkbxRevalPublish_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeStatus_chkbxMainOption();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
        
    }
}
