//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;
//using Prana.PM.BLL;


//namespace Prana.PM.Client.UI.Controls
//{
//    public partial class CtrlManualEntry : UserControl
//    {
//        BindingSource _formBindingSource = new BindingSource();
//        private TradeReconManualEntry _tradeReconManualEntry = new TradeReconManualEntry();

//        public CtrlManualEntry()
//        {
//            InitializeComponent();
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            this.FindForm().Close();

//        }

//        private void btnSave_Click(object sender, EventArgs e)
//        {
//            Forms.ManualEntryPassword frmManualEntryPassword = new Forms.ManualEntryPassword();
//            frmManualEntryPassword.ShowDialog();

//        }

//        //internal event EventHandler ManualEntryClosed;

//        #region Initialize the control
//        private bool _isInitialized = false;

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance is initialized.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsInitialized
//        {
//            get { return _isInitialized; }
//            set { _isInitialized = value; }
//        }


//        /// <summary>
//        /// Initialize the control.
//        /// </summary>
//        public void InitControl()
//        {
//            if (!_isInitialized)
//            {
//                SetupBinding();
//                _isInitialized = true;
//            }
//        }

//        #endregion

//        private void SetupBinding()
//        {
//            _formBindingSource.DataSource = RetrieveManualEntry(); 

//            //create a binding object
//            Binding userNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "User");
//            //add new binding
//            txtUserName.DataBindings.Add(userNameBinding);

//            Binding symbolBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "ExceptionReportEntryItem.Symbol");
//            txtSymbol.DataBindings.Add(symbolBinding);

//            Binding sourceDataBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "ExceptionReportEntryItem.SourceData");
//            txtSourceData.DataBindings.Add(sourceDataBinding);

//            Binding applicationDataBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "ExceptionReportEntryItem.ApplicationData");
//            txtApplicationData.DataBindings.Add(applicationDataBinding);

//            Binding manualEntryBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "ExceptionReportEntryItem.ManualEntry");
//            txtManualEntry.DataBindings.Add(manualEntryBinding);

//            Binding commentsBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "Comments");
//            txtComments.DataBindings.Add(commentsBinding);
//        }

//        private TradeReconManualEntry RetrieveManualEntry()
//        {
//            _tradeReconManualEntry.ExceptionReportEntryItem.SourceData = "1200";
//            _tradeReconManualEntry.ExceptionReportEntryItem.ApplicationData = "1000";
//            _tradeReconManualEntry.ExceptionReportEntryItem.ManualEntry = "1100";
//            _tradeReconManualEntry.ExceptionReportEntryItem.Symbol = "MSFT";
//            _tradeReconManualEntry.User.ID = "1";
//            _tradeReconManualEntry.User.UserName = "Shams";
//            _tradeReconManualEntry.Comments = "This is a test comment!";
//            return _tradeReconManualEntry;
//        }

//        private void CtrlManualEntry_Load(object sender, EventArgs e)
//        {
//            this.ActiveControl = txtManualEntry;
//            this.txtManualEntry.Focus();

//        }

//    }
//}
