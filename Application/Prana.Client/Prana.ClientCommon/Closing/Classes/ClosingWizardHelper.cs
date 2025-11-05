using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public class ClosingWizardHelper : IDisposable
    {

        #region Private

        #region Fields

        // DuplexProxyBase<ISubscription> _proxySubscription;
        ProxyBase<IClosingServices> _closingServices = null;
        ClosingWizard _closingWizardUI = null;
        BackgroundWorker _bgRunClosingBasedOnTemplates = new BackgroundWorker();
        BackgroundWorker _bgRunUnwindingBasedOnTemplates = new BackgroundWorker();
        private static ClosingWizardHelper _singleton = null;
        private static object _locker = new object();

        #endregion


        #region Delegates

        public delegate void OperationHandler(string statusMsg);

        #endregion

        #region Events

        //public event OperationHandler OperationStarted;
        //public event OperationHandler OperationCompleted;

        public event EventHandler<EventArgs<String>> OperationStarted;
        public event EventHandler<EventArgs<String>> OperationCompleted;

        public event EventHandler ClosingWizardClosed;
        public event EventHandler PreviewData;

        #endregion

        #region Functions

        void closingWizardUI_RunUnwinding(object sender, EventArgs e)
        {

            try
            {
                if (!_bgRunUnwindingBasedOnTemplates.IsBusy)
                {
                    List<ClosingTemplate> closingTemplates = sender as List<ClosingTemplate>;
                    object[] arguments = new object[1];
                    arguments[0] = closingTemplates;
                    _bgRunUnwindingBasedOnTemplates.RunWorkerAsync(arguments);
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
        void closingWizardUI_RunClosing(object sender, EventArgs e)
        {

            try
            {
                if (!_bgRunClosingBasedOnTemplates.IsBusy)
                {
                    List<ClosingTemplate> closingTemplates = sender as List<ClosingTemplate>;
                    object[] arguments = new object[1];
                    arguments[0] = closingTemplates;
                    _bgRunClosingBasedOnTemplates.RunWorkerAsync(arguments);
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

        void _bgRunUnwindingBasedOnTemplates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgRunUnwindingBasedOnTemplates.CancellationPending)
                {
                    object[] parameters = e.Argument as object[];

                    List<ClosingTemplate> closingTemplates = parameters[0] as List<ClosingTemplate>;

                    if (closingTemplates.Count > 0)
                    {
                        if (OperationStarted != null)
                        {
                            string statusMessage = "Unwinding Data Please Wait";
                            OperationStarted(this, new EventArgs<String>(statusMessage));
                        }
                        ClosingData closingData = _closingServices.InnerChannel.UnwindClosingBasedOnTemplates(closingTemplates);
                        e.Result = closingData;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                CancelOperation();
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                if (OperationCompleted != null)
                {
                    OperationCompleted(this, new EventArgs<String>("Operation has been cancelled!"));
                }
            }

        }

        void _bgRunUnwindingBasedOnTemplates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {
                if (!e.Cancelled)
                {
                    var closingData = e.Result as ClosingData;
                    if (closingData.IsNavLockFailed)
                    {
                        MessageBox.Show("The closing date for some of the taxlots in the templates(" + closingData.NavLockFailedTemplates
                                + ") you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (OperationCompleted != null)
                        {
                            string statusMessage = "Nav Lock Failed";
                            OperationCompleted(this, new EventArgs<string>(statusMessage));
                        }
                    }
                    if (closingData.ErrorMsg != null && closingData.ErrorMsg.Length > 0)
                    {
                        string path = logErrors(closingData.ErrorMsg.ToString());
                        if (!string.IsNullOrEmpty(path))
                        {
                            StringBuilder boxMessage = new StringBuilder();
                            boxMessage.AppendLine("Some Taxlots could not be unwound. Do you want to view details?");
                            DialogResult dr = MessageBox.Show(boxMessage.ToString(), "Unwinding Error", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(path);
                            }

                            if (OperationCompleted != null)
                            {
                                string statusMessage = "Some errors occurred while unwinding";
                                OperationCompleted(this, new EventArgs<String>(statusMessage));
                            }
                        }
                        else
                        {
                            if (OperationCompleted != null)
                            {
                                string statusMessage = "Data unwound Successfully";
                                OperationCompleted(this, new EventArgs<String>(statusMessage));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Operation has been canceled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
                _closingWizardUI.OperationCompleted();
                if (OperationCompleted != null)
                {
                    OperationCompleted(this, new EventArgs<string>("Operation has been cancelled!"));
                }
            }
        }

        void _bgRunClosingBasedOnTemplates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgRunClosingBasedOnTemplates.CancellationPending)
                {
                    object[] parameters = e.Argument as object[];

                    List<ClosingTemplate> closingTemplates = parameters[0] as List<ClosingTemplate>;
                    if (closingTemplates.Count > 0)
                    {
                        if (OperationStarted != null)
                        {
                            string statusMessage = "Closing Data Please Wait";
                            OperationStarted(this, new EventArgs<string>(statusMessage));
                        }
                        var closingData = _closingServices.InnerChannel.AutomaticClosingBasedOnTemplates(closingTemplates);

                        e.Result = closingData;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                CancelOperation();
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                if (OperationCompleted != null)
                {
                    OperationCompleted(this, new EventArgs<string>("Operation has been cancelled!"));
                }
            }
        }

        void _bgRunClosingBasedOnTemplates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    var closingData = e.Result as ClosingData;
                    if (closingData.IsNavLockFailed)
                    {
                        MessageBox.Show("The closing date for some of the taxlots in the templates(" + closingData.NavLockFailedTemplates 
                                + ") you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (OperationCompleted != null)
                        {
                            string statusMessage = "Nav Lock Failed";
                            OperationCompleted(this, new EventArgs<string>(statusMessage));
                        }
                    }
                    if (closingData.ErrorMsg != null && closingData.ErrorMsg.Length > 0)
                    {
                        string path = logErrors(closingData.ErrorMsg.ToString());
                        if (!string.IsNullOrEmpty(path))
                        {
                            if (OperationCompleted != null)
                            {
                                string statusMessage = "Some erros occurred while closing";
                                OperationCompleted(this, new EventArgs<string>(statusMessage));
                            }
                            StringBuilder boxMessage = new StringBuilder();
                            boxMessage.AppendLine("Some Taxlots could not be Closed Successfully.Do you want to view details?");
                            DialogResult dr = MessageBox.Show(boxMessage.ToString(), "Close Trade Error", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(path);
                            }
                        }
                        else
                        {
                            if (OperationCompleted != null)
                            {
                                string statusMessage = "Close Trade Data Saved";
                                OperationCompleted(this, new EventArgs<string>(statusMessage));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Operation has been cancelled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
                _closingWizardUI.OperationCompleted();
                if (OperationCompleted != null)
                {
                    OperationCompleted(this, new EventArgs<string>("Operation has been cancelled!"));
                }
            }

            finally
            {


            }

        }


        private string logErrors(string errorMessage)
        {

            int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            String path = Application.StartupPath + @"\Logs\ClosingErrorsLog_" + userID + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                streamWriter.WriteLine(Environment.NewLine + DateTime.Now.ToString());
                streamWriter.Write(errorMessage.ToString());
            }

            return path.ToString();
        }



        private void WireEvents()
        {
            try
            {
                _closingWizardUI.RunClosing += new EventHandler(closingWizardUI_RunClosing);
                _closingWizardUI.RunUnwinding += new EventHandler(closingWizardUI_RunUnwinding);
                _closingWizardUI.FormClosed += new FormClosedEventHandler(closingWizardUI_FormClosed);
                _closingWizardUI.PreviewDataBasedOnTemplate += new EventHandler(_closingWizardUI_PreviewDataBasedOnTemplate);



                _bgRunClosingBasedOnTemplates.DoWork += new DoWorkEventHandler(_bgRunClosingBasedOnTemplates_DoWork);
                _bgRunClosingBasedOnTemplates.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgRunClosingBasedOnTemplates_RunWorkerCompleted);


                _bgRunUnwindingBasedOnTemplates.DoWork += new DoWorkEventHandler(_bgRunUnwindingBasedOnTemplates_DoWork);
                _bgRunUnwindingBasedOnTemplates.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgRunUnwindingBasedOnTemplates_RunWorkerCompleted);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _closingWizardUI_PreviewDataBasedOnTemplate(object sender, EventArgs e)
        {
            if (PreviewData != null)
            {
                PreviewData(sender, null);
            }
        }

        private void UnwireEvents()
        {
            try
            {
                _closingWizardUI.RunClosing -= new EventHandler(closingWizardUI_RunClosing);
                _closingWizardUI.RunUnwinding -= new EventHandler(closingWizardUI_RunUnwinding);
                _closingWizardUI.FormClosed -= new FormClosedEventHandler(closingWizardUI_FormClosed);

                if (_bgRunClosingBasedOnTemplates != null)
                {
                    _bgRunClosingBasedOnTemplates.DoWork -= new DoWorkEventHandler(_bgRunClosingBasedOnTemplates_DoWork);
                    _bgRunClosingBasedOnTemplates.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgRunClosingBasedOnTemplates_RunWorkerCompleted);
                }

                if (_bgRunUnwindingBasedOnTemplates != null)
                {
                    _bgRunUnwindingBasedOnTemplates.DoWork -= new DoWorkEventHandler(_bgRunUnwindingBasedOnTemplates_DoWork);
                    _bgRunUnwindingBasedOnTemplates.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgRunUnwindingBasedOnTemplates_RunWorkerCompleted);
                }



            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        void closingWizardUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                UnwireEvents();
                if (ClosingWizardClosed != null)
                {
                    ClosingWizardClosed(null, null);
                }
                _closingWizardUI = null;
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

        #endregion

        #endregion

        #region Public

        #region Properties


        public ProxyBase<IClosingServices> ClosingServices
        {
            set { _closingServices = value; }
        }



        #endregion

        #region Functions

        public static ClosingWizardHelper GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new ClosingWizardHelper();
                    }
                }
            }
            return _singleton;
        }
        public void LaunchClosingWizard()
        {
            try
            {
                if (_closingWizardUI == null)
                {

                    LaunchClosingWizardOnNewThread();
                    //Thread ClosingWizardThread = new Thread(LaunchClosingWizardOnNewThread);
                    //ClosingWizardThread.IsBackground = true;
                    //ClosingWizardThread.SetApartmentState(ApartmentState.STA);
                    //ClosingWizardThread.Start();

                    // _closingWizardUI.ShowDialog();
                }
                else
                {
                    BringFormToFront();
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

        private void LaunchClosingWizardOnNewThread()
        {
            try
            {
                if (_closingWizardUI == null)
                {

                    _closingWizardUI = new ClosingWizard();
                    _closingWizardUI.ShowInTaskbar = false;
                    _closingWizardUI.setup(CachedDataManager.GetInstance.LoggedInUser);
                    WireEvents();
                    _closingWizardUI.Show();
                    //Application.Run(_closingWizardUI);
                }
                else
                {
                    BringFormToFront();
                }
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

        private void BringFormToFront()
        {
            try
            {
                if (UIValidation.GetInstance().validate(_closingWizardUI))
                {
                    if (_closingWizardUI.InvokeRequired)
                    {
                        _closingWizardUI.BeginInvoke(new MethodInvoker(BringFormToFront));
                    }

                    else
                    {
                        _closingWizardUI.BringToFront();
                    }
                }
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
        public void CancelOperation()
        {
            try
            {
                if (UIValidation.GetInstance().validate(_closingWizardUI))
                {
                    if (_closingWizardUI.InvokeRequired)
                    {
                        _closingWizardUI.BeginInvoke(new MethodInvoker(CancelOperation));
                    }
                    else
                    {

                        if (_closingWizardUI != null)
                        {
                            _closingWizardUI.CancelOperation();
                            _closingWizardUI.OperationCompleted();
                        }
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

        #endregion

        #endregion



        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_bgRunClosingBasedOnTemplates != null)
                        _bgRunClosingBasedOnTemplates.Dispose();

                    if (_bgRunUnwindingBasedOnTemplates != null)
                    {
                        _bgRunUnwindingBasedOnTemplates.Dispose();
                    }
                    if (_closingWizardUI != null)
                    {
                        _closingWizardUI.Dispose();
                    }
                    if (_closingServices != null)
                    {
                        _closingServices.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
