using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Prana.Tools
{
    public partial class UDAUIForm : Form, IPluggableTools
    {
        public UDAUIForm()
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
        //ProxyBase<IPranaPositionServices> _pranaPositionServices = null;

        //private int _hashCode = int.MinValue;
        ISecurityMasterServices _securityMaster = null;

        public delegate void ResponseInvokeDelegate(object sender, EventArgs<QueueMessage> e);
        public delegate void ConnectionInvokeDelegate(object sender, EventArgs e);
        public delegate void UDADataResInvokeDelegate(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e);
        public delegate void InUsedUDADataResInvokeDelegate(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> InUsedUDADataCol);

        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }
        public void SetUp(ISecurityMasterServices securityMaster)
        {
            try
            {
                _securityMaster = securityMaster;
                if (_securityMaster != null)
                {
                    _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                    //new UDAAttributesDataResponse(_securityMaster_UDAAttributesResponse);
                    _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                    //new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                    _securityMaster.Disconnected += new EventHandler(_securityMaster_Disconnected);
                    _securityMaster.Connected += new EventHandler(_securityMaster_Connected);
                    _securityMaster.EventInUsedUDARes += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_EventInUsedUDARes);
                    //new DelegateInUsedUDARes(_securityMaster_EventInUsedUDARes);
                    usrControlUDA1.EventHandlerSaveUDAData += new UserControlUDA.DelegateSaveUDAData(usrControlUDA1_EventHandlerSaveUDAData);
                    if (_securityMaster.IsConnected)
                    {
                        _securityMaster.GetInUsedUDAIds();
                        _securityMaster.GetAllUDAAtrributes();
                    }
                    else
                    {
                        MessageBox.Show("TradeService not connected", "Nirvana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("TradeService not connected", "Nirvana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // _hashCode = this.GetHashCode();

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

        void usrControlUDA1_EventHandlerSaveUDAData(Dictionary<String, Dictionary<string, object>> udaData)
        {
            try
            {
                if (_securityMaster.IsConnected)
                {

                    _securityMaster.SaveUDAData(udaData);
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

        void _securityMaster_EventInUsedUDARes(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> inUsedUDAsDict = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        InUsedUDADataResInvokeDelegate mainThread = new InUsedUDADataResInvokeDelegate(_securityMaster_EventInUsedUDARes);
                        this.BeginInvoke(mainThread, new object[] { sender, e });
                    }
                    else
                    {
                        usrControlUDA1.LoadDataInUsedUDAs(inUsedUDAsDict);
                    }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
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

        void _securityMaster_UDAAttributesResponse(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UDADataResInvokeDelegate mainThread = new UDADataResInvokeDelegate(_securityMaster_UDAAttributesResponse);
                        this.BeginInvoke(mainThread, new object[] { sender, e });
                    }
                    else
                    {
                        usrControlUDA1.DictUDAAttributes = e.Value;
                        usrControlUDA1.LoadData();
                    }
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




        #region IUDAForm Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        #endregion



        #region IPluggableTools Members



        public void SetUP()
        {
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }
        #endregion


        private void UDAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool allDataSaved = true;
                foreach (UDAControl udaCtrl in CentralDataManager.UDACtrlCollection)
                {
                    //UpdateData function is called to save the edited data in grid. ,PRANA-8666
                    udaCtrl.Grid.UpdateData();
                    if (udaCtrl.IsChanged)
                    {
                        allDataSaved = false;
                    }
                }
                DialogResult userChoice = DialogResult.No;
                if (!allDataSaved)
                {
                    userChoice = MessageBox.Show("Would you like to save UDA changes?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (userChoice == DialogResult.Yes)
                    {
                        if (!allDataSaved)
                        {
                            usrControlUDA1.SaveChanges();
                        }

                    }
                    if (userChoice == DialogResult.Cancel)
                    {
                        e.Cancel = true; ;
                    }
                }

                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
                }
                // _pranaPositionServices.Dispose();

                //unwire _securityMaster events
                if (_securityMaster != null)
                {
                    _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                    //new UDAAttributesDataResponse(_securityMaster_UDAAttributesResponse);
                    _securityMaster.ResponseCompleted -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                    //new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                    _securityMaster.Disconnected -= new EventHandler(_securityMaster_Disconnected);
                    _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);
                    _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);
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




        #region IPluggableTools Members


        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        /// <summary>
        /// Handle event on Sever connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {

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
        /// Handle on Server disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {

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
        /// On response completed from Security master services at sever.
        /// </summary>
        /// <param name="qMsg"></param>
        void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {

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
        /// Unwire events.
        /// </summary>
        private void UnWireEvents()
        {
            if (_securityMaster != null)
            {
                _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                _securityMaster.ResponseCompleted -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                _securityMaster.Disconnected -= new EventHandler(_securityMaster_Disconnected);
                _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);
                _securityMaster.EventInUsedUDARes -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_EventInUsedUDARes);
            }
            if (usrControlUDA1 != null)
            {
                usrControlUDA1.EventHandlerSaveUDAData -= new UserControlUDA.DelegateSaveUDAData(usrControlUDA1_EventHandlerSaveUDAData);
                usrControlUDA1 = null;
            }
        }


    }
}