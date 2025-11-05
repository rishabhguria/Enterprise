using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.WCFConnectionMgr;
using System.Configuration;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;


namespace Prana.UDATool
{
    public partial class UDAForm : Form, IPluggableTools
    {
        public UDAForm()
        {
            InitializeComponent();
        }
        ProxyBase<IPranaPositionServices> _pranaPositionServices = null;

        private int _hashCode = int.MinValue;
        ISecurityMasterServices _securityMaster = null;

        public delegate void ResponseInvokeDelegate(QueueMessage qMsg);
        public delegate void ConnectionInvokeDelegate(object sender, EventArgs e);
        public delegate void UDADataResInvokeDelegate(UDASymbolDataCollection UDASymbolDataCol);

        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                usrControlUDA1.LoadData();
                CreatePranaPositionServicesProxy();

                // binding get and Save UDA Data events 
                usrCtrlSymbolUDAData1.GetUDASymbolInfoEventHandler += new UsrCtrlSymbolUDAData.GetUDASymbolInfo(usrCtrlSymbolUDAData1_GetUDASymbolInfoEvent);
                usrCtrlSymbolUDAData1.SaveUDASymbolInfoEventHandler += new UsrCtrlSymbolUDAData.SaveUDASymbolInfo(usrCtrlSymbolUDAData1_SaveUDASymbolInfoEventHandler);


                //binding _securityMaster events, UDA data related
                _securityMaster.udaUISymbolDataResponse += new UDAUISymbolDataResponse(_securityMaster_udaUISymbolDataResponse);
                _securityMaster.ResponseCompleted += new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                _securityMaster.Disconnected += new EventHandler(_securityMaster_Disconnected);
                _securityMaster.Connected += new EventHandler(_securityMaster_Connected);

                _hashCode = this.GetHashCode();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void usrCtrlSymbolUDAData1_SaveUDASymbolInfoEventHandler(UDASymbolDataCollection UDASymbolDataCol)
        {
            try
            {
                
                UDASymbolDataCol.RequestID = System.Guid.NewGuid().ToString();

                if (UDASymbolDataCol.Count > 0)
                {
                    _securityMaster.SaveUDASymbolsData(UDASymbolDataCol);
                   
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
            set { ; }
        }
        #endregion

        private void CreatePranaPositionServicesProxy()
        {
            try
            {
                string endpointAddressInString = ConfigurationManager.AppSettings["PositionManagementEndpointAddress"];
                _pranaPositionServices = new ProxyBase<IPranaPositionServices>(endpointAddressInString);
                usrCtrlSymbolUDAData1.PranaPositionServices = _pranaPositionServices;
               
                
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
        }
        private void UDAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool allDataSaved = true;
                foreach (UDAControl udaCtrl in CentralDataManager.UDACtrlCollection)
                {
                    if (udaCtrl.IsChanged)
                    {
                        allDataSaved = false;
                    }
                }
                DialogResult userChoice = DialogResult.No;
                if (!allDataSaved || usrCtrlSymbolUDAData1.IsChanged)
                {
                    userChoice = MessageBox.Show("Would you like to save UDA changes?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (userChoice == DialogResult.Yes)
                    {
                        if (!allDataSaved)
                        {
                            usrControlUDA1.SaveChanges();
                        }
                        if (usrCtrlSymbolUDAData1.IsChanged)
                        {
                            usrCtrlSymbolUDAData1.SaveUDASymbolData();
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
                _pranaPositionServices.Dispose();

                //unwire _securityMaster events
                if (_securityMaster != null)
                {
                    _securityMaster.udaUISymbolDataResponse -= new UDAUISymbolDataResponse(_securityMaster_udaUISymbolDataResponse);
                    _securityMaster.ResponseCompleted -= new CompletedReceivedDelegate(_securityMaster_ResponseCompleted);
                    _securityMaster.Disconnected -= new EventHandler(_securityMaster_Disconnected);
                    _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key == "UDASymbolData")
            {
                bool allDataSaved = true;
                foreach (UDAControl udaCtrl in CentralDataManager.UDACtrlCollection)
                {
                    if (udaCtrl.IsChanged)
                    {
                        allDataSaved = false;
                    }
                }
                DialogResult userChoice = DialogResult.No;
                if (!allDataSaved)
                {
                    userChoice = MessageBox.Show("Would you like to save UDA changes?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (userChoice == DialogResult.No)
                    {
                        usrControlUDA1.UndoAllChanges();
                    }
                    if (userChoice == DialogResult.Yes)
                    {
                        usrControlUDA1.SaveChanges();
                    }
                    if (userChoice == DialogResult.Cancel)
                    {
                        e.PreviousSelectedTab.Selected = true;
                    }
                }
            }
        }


        #region IPluggableTools Members


        public IPricingAnalysis PricingAnalysis
        {
            set { ; }
        }

        #endregion

        #region IPluggableTools Members


        public IContainerServices ContainerServices
        {
            set { ; }
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
                if (this.InvokeRequired)
                {
                    ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Connected);
                    this.Invoke(connectionStatusDelegate, new object[] { sender, e });
                }
                else
                {
                    //TODO
                    //SetGetButtonDetails("Get Data", true);
                    // toolStripStatusLabel1.Text = "Trade Server : Connected";
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
                if (this.InvokeRequired)
                {
                    ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Disconnected);
                    this.Invoke(connectionStatusDelegate, new object[] { sender, e });
                }
                else
                {
                    usrCtrlSymbolUDAData1.SetStatusBarText("Trade Server : Disconnected");
                    
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
        void _securityMaster_ResponseCompleted(QueueMessage qMsg)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ResponseInvokeDelegate invokeDel = new ResponseInvokeDelegate(_securityMaster_ResponseCompleted);
                    this.Invoke(invokeDel, new object[] { qMsg });
                }
                else
                {
                    usrCtrlSymbolUDAData1.SetStatusBarText(qMsg.Message.ToString());

                }
               
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event Handle On UDA data recieved from server
        /// </summary>
        /// <param name="secMasterList"></param>
        void _securityMaster_udaUISymbolDataResponse(UDASymbolDataCollection UDASymbolDataCol)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    UDADataResInvokeDelegate udaDataResInvokeDelegate = new UDADataResInvokeDelegate(HandleResponse);
                    this.Invoke(udaDataResInvokeDelegate, new object[] { UDASymbolDataCol });
                }
                else
                {
                   
                    HandleResponse(UDASymbolDataCol);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Send recieved UDA data to user control symbolUDAData  to show on UI.
        /// </summary>
        /// <param name="SecMasterList"></param>
        private void HandleResponse(UDASymbolDataCollection UDASymbolDataCol)
        {

            try
            {

                usrCtrlSymbolUDAData1.HandelUDASymbolDataResponse(UDASymbolDataCol);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
       
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// handle of getting request for UDA data, Send request to server 
        /// </summary>
        /// <param name="udaDataReqObj"></param>
        void usrCtrlSymbolUDAData1_GetUDASymbolInfoEvent(UDADataReqObj udaDataReqObj)
        {

            try
            {

                if (!_securityMaster.IsConnected)
                {
                    // connect to server
                    _securityMaster.ConnectToServer();
                    usrCtrlSymbolUDAData1.SetStatusBarText("Server not connected. Please contact to admin.");
                    return;
                }

                udaDataReqObj.RequestID = System.Guid.NewGuid().ToString();
                _securityMaster.GetSymbolsUDAData(udaDataReqObj);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        
    }
}