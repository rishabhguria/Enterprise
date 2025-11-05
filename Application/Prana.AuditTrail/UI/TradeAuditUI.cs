using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.AuditTrail
{
    public partial class TradeAuditUI : Form, IAuditTrailUI
    {
        Prana.BusinessObjects.CompanyUser _loginUser;
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            set
            {
                _loginUser = value;
                ctrlAuditTrail1.setLoginUser(_loginUser);
            }
        }
        public new event EventHandler FormClosed;
        public TradeAuditUI()
        {
            InitializeComponent();
            ctrlAuditTrail1.drawsthedefaultgridstructure();
            this.AcceptButton = ctrlAuditTrail1._btGetData;
        }
        private void TradeAuditUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (FormClosed != null)
                {
                    FormClosed(this, null);
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
        public System.Windows.Forms.Form Reference()
        {
            return this;
        }
        public void GetAndBindAuditUIDataForGroupIds(List<string> groups)
        {
            ctrlAuditTrail1.GetAndBindAuditUIDataForGroupIds(groups);
        }
        public void GetAndBindAuditUIDataForFilters(AuditTrailFilterParams auditTrailFilterParams)
        {
            ctrlAuditTrail1.GetAndBindAuditUIDataForFilters(auditTrailFilterParams);
        }
        public void BindNewTableToTradeAudit(DataTable entries)
        {
            ctrlAuditTrail1.BindNewTableToTradeAudit(entries);
        }

        private void TradeAuditUI_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_AUDIT_TRAIL);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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