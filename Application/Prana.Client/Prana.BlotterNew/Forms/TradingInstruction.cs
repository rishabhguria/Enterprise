using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Blotter
{
    public partial class TradingInstructionAcceptanceForm : Form, Prana.Interfaces.ITradingInstructionUI
    {
        private CompanyUser _loginUser;
        public TradingInstructionAcceptanceForm()
        {
            InitializeComponent();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
        }

        private void Desk_Load(object sender, EventArgs e)
        {
            try
            {
                ctrlAcceptRejectHandler1.InitControl();
                ctrlAcceptRejectHandler1.LoginUser = _loginUser;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //public void UpdateTradingInst(TradingInstruction tradingInst)
        //{
        //    try
        //    {
        //        ctrlAcceptRejectHandler1.UpdateTradeList(tradingInst);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void TradingInstructionAcceptanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //e.Cancel = true;
                //this.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ITradingInstructionUI Members

        public Form Reference()
        {
            return this;

        }
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }
        #endregion
    }
}