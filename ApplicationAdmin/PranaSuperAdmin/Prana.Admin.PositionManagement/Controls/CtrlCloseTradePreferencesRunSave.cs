using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCloseTradePreferencesRunSave : UserControl
    {
        BindingSource _formBindingSource = new BindingSource();
        private CloseTradePreferences _closeTradePreferences = new CloseTradePreferences();

        Forms.CloseTradeMatchedTradeReport _frmCloseTradeMatchedTradeReport = null;

        public CtrlCloseTradePreferencesRunSave()
        {
            InitializeComponent();
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        
	

        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl()
        {
            if (!_isInitialized)
            {
                SetupBinding();
                _isInitialized = true;
            }
        }

        #endregion

        /// <summary>
        /// Setups the binding.
        /// </summary>
        private void SetupBinding()
        {
            //_closeTradePreferences.DataSourceNameID.ID = 1;
            //_closeTradePreferences.DataSourceNameID.ShortName = "GS";
            //_closeTradePreferences.Asset.ID = 3;
            //_closeTradePreferences.Asset.Name = "Futures";
            ////_closeTradePreferences.Underlyings = GetCurrentUnderlyings();
            ////_closeTradePreferences.Exchanges = GetCurrentExchanges();
            ////_closeTradePreferences.Funds = GetCurrentFunds();
            //_closeTradePreferences.DefaultMethodology = CloseTradeMethodology.Manual;
            //_closeTradePreferences.Algorithm = CloseTradeAlogrithm.FIFO;

            //ctrlCloseTradePreferences1.InitControl(_closeTradePreferences);

            ctrlCloseTradePreferences1.InitControl();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //To Do: Add Validation for Manual and None Combination!!

            if (ctrlCloseTradePreferences1.DefaultMethodology == CloseTradeMethodology.Automatic && ctrlCloseTradePreferences1.Alogrithm ==CloseTradeAlogrithm.None)
            {
                MessageBox.Show("Automatic Methodology should have an Algorthm", "Invalid Methodology - Algorithm Combination");
            }

            if (ctrlCloseTradePreferences1.DefaultMethodology == CloseTradeMethodology.Manual && ctrlCloseTradePreferences1.Alogrithm == CloseTradeAlogrithm.None)
            {
                if (_frmCloseTradeMatchedTradeReport == null)
                {
                    _frmCloseTradeMatchedTradeReport = new Nirvana.Admin.PositionManagement.Forms.CloseTradeMatchedTradeReport();
                }

                _frmCloseTradeMatchedTradeReport.Show();
                _frmCloseTradeMatchedTradeReport.Activate();
                _frmCloseTradeMatchedTradeReport.Disposed += new EventHandler(_frmCloseTradeMatchedTradeReport_Disposed);
            }
            
        }

        void _frmCloseTradeMatchedTradeReport_Disposed(object sender, EventArgs e)
        {
            _frmCloseTradeMatchedTradeReport = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }
    }
}
