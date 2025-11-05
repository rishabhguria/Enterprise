using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using Prana.BusinessObjects;
using Prana.Global;


namespace Prana.LiveFeedEngine
{
    public partial class frmLiveFeedEngineMain : Form
    {

        bool _isLiveFeedEngineRunning = false;
        private List<string> _connectedUserList = new List<string>();
        private string _strServerTitle = "Prana Live Feed Server";

        public frmLiveFeedEngineMain()
        {
            InitializeComponent();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(OnInformationReceived);
            DataTable dtCompany = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany();
            if (dtCompany != null)
            {
                this.Text = dtCompany.Rows[0]["CompanyName"].ToString() + ": " + _strServerTitle + ", v" + ConfigurationHelper.Instance.GetAppSettingValueByKey("ApplicationVersion");
            }
            
            AddLiveFeedConnectionDisplay();

        }

        private void AddLiveFeedConnectionDisplay()
        {
            try
            {
                LivefeedConnection livefeedConnection1 = new LivefeedConnection();
                livefeedConnection1.Location = new System.Drawing.Point(680, 294);
                livefeedConnection1.Name = "livefeedConnection1";
                livefeedConnection1.Size = new System.Drawing.Size(19, 22);
                livefeedConnection1.TabIndex = 39;
                livefeedConnection1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                this.Controls.Add(livefeedConnection1);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void Start()
        {
            try
            {
                bool result = ServiceManager.GetInstance().Start();

                if (result)
                {
                    btnStartEngine.Text = "Engine Started!";
                    btnStartEngine.Enabled = false;
                    btnStopEngine.Enabled = true;
                    ServiceManager.GetInstance().ClientBroadCastingManager.Connected+=new Prana.Interfaces.ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Connected);
                    ServiceManager.GetInstance().ClientBroadCastingManager.Disconnected += new Prana.Interfaces.ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Disconnected);

                    _isLiveFeedEngineRunning = true;
                }
                else
                {
                    MessageBox.Show("Unable to Start Engine! Try Again", "LiveFeed Engine");
                    btnStartEngine.Text = "Start Engine!";
                    btnStartEngine.Enabled = true;
                    btnStopEngine.Enabled = false;
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

        public void Stop()
        {
            try
            {
                if (_isLiveFeedEngineRunning)
                {
                    DialogResult result = MessageBox.Show("Do you want to stop LiveFeed Engine?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (result == DialogResult.Yes)
                    {
                        ServiceManager.GetInstance().Stop();
                        btnStartEngine.Text = "Start Engine";
                        btnStartEngine.Enabled = true;
                        //connectedUserList.Remove(ClientsCommonDataManager.GetCompanyUser);
                        //UpdateUserList();
                        btnStopEngine.Enabled = false;
                        _isLiveFeedEngineRunning = false;
                    }
                }

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

        private void UpdateUserList()
        {
            if (lstExpnlUsers.InvokeRequired)
            {
                MethodInvoker mi = new MethodInvoker(UpdateUserList);
                this.Invoke(mi, null);
            }
            else
            {
                lstExpnlUsers.DataSource = null;
                lstExpnlUsers.DataSource = _connectedUserList;
                //_connectedUserList[lstClient.SelectedIndex] 
                //lstClient.Items.Add(userName);
            }

        }

        void OnApplicationExit(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.ExitThread();
        }

        void OnInformationReceived(string message)
        {
            try
            {
                if (lstbxErrorLog.InvokeRequired)
                {
                    SetTextCallback2 mi = new SetTextCallback2(OnInformationReceived);
                    this.Invoke(mi, new object[] { message });
                }
                else
                {
                    lstbxErrorLog.Items.Add(message);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                throw new Exception("Caught Unhandled Exception", e.Exception);
                ///Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
            }
            //string formattedInfo = ex.Exception.StackTrace.ToString();
            //Logger.Write(formattedInfo, Prana.Global.Common.LOG_CATEGORY_EXCEPTION, 1,     1, System.Diagnostics.TraceEventType.Error, FORM_NAME);

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string formattedInfo = "Caught unhandled. IsTerminating : " + e.IsTerminating + " " + e.ExceptionObject.ToString();
                Logger.Write(formattedInfo, Prana.Global.ApplicationConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "LiveFeed Engine");
                ///Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
                Application.Exit();
            }
        }


        void ClientBroadCastingManager_Disconnected(ConnectionProperties connProperties)
        {
            try
            {
                if (_connectedUserList.Contains(connProperties.IdentifierName ))
                {
                    _connectedUserList.Remove(connProperties.IdentifierName);
                    UpdateUserList();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

        void ClientBroadCastingManager_Connected(ConnectionProperties connProperties)
        {
            try
            {
                if (!_connectedUserList.Contains(connProperties.IdentifierName ))
                {
                    _connectedUserList.Add(connProperties.IdentifierName);
                }
                UpdateUserList();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }


        #region Form events
        private void btnStartEngine_Click(object sender, EventArgs e)
        {
            Start();
        }


        private void btnStopEngine_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnOpenLogFile_Click(object sender, EventArgs e)
        {
            string strFilePath = "PranaLog.log";

            if (File.Exists(strFilePath))
            {
                Process.Start(strFilePath);
            }
        }

        private void btnClearLoggingWindow_Click(object sender, EventArgs e)
        {
            lstbxErrorLog.Items.Clear();
        }

        private void btnRestartLiveFeeds_Click(object sender, EventArgs e)
        {
            ServiceManager.GetInstance().RestartLiveFeeds();
        }


        private void frmLiveFeedEngineMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            if (_isLiveFeedEngineRunning)
            {
                e.Cancel = true;
            }
        }

        private void frmLiveFeedEngineMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void btnDisconnectUser_Click(object sender, EventArgs e)
        {
            if (lstExpnlUsers.SelectedItem != null)
            {
                if (ServiceManager.GetInstance().ClientBroadCastingManager.RemoveClient(lstExpnlUsers.SelectedItem.ToString()))
                {
                }
                else
                {
                    MessageBox.Show("No Such User", "ExPnL Calculator");
                }
            }
            else
            {
                MessageBox.Show("Please Select a User", "ExPnL Calculator");
            }
        }

    }

    delegate void SetTextCallback2(string msg);
}