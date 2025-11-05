using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;


namespace Prana.Tools
{
    public partial class ctrlAmendments : UserControl
    {

        #region Commented Code
        /// <summary>
        ///  return the bool value weather recon report is save dor not
        /// </summary>
        //public bool IsExceptionReportGenerated
        //{
        //get { return ctrlReconOutput1.IsExceptionReportGenerated; }
        //}

        #endregion

        #region Private Methods
        /// <summary>
        /// launc the postTransactionUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPostTransaction_Click(object sender, EventArgs e)
        {
            try
            {

                launchForm = ReconPrefManager.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_MANUAL_TRADING_TICKET_UI.ToString());
                    launchForm(this, args);
                }
            }
            #region Catch
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
            #endregion
        }

        private void SetButtonsColor()
        {
            try
            {
                btnPostTransaction.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPostTransaction.ForeColor = System.Drawing.Color.White;
                btnPostTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPostTransaction.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPostTransaction.UseAppStyling = false;
                btnPostTransaction.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        #endregion

        #region Public Variable
        public static event EventHandler launchForm;
        #endregion

        #region Construtors

        public ctrlAmendments()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// sets data on the grid of ctrlreconoutput
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reconType"></param>
        /// <param name="templateName"></param>
        /// <param name="toDate"></param>
        //internal bool SetDataonControl(ReconParameters reconParameters)
        //{
        //    try
        //    {
        //        if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(Convert.ToInt32(reconParameters.ClientID)))
        //        {
        //            tbClient.Text = CachedDataManager.GetUserPermittedCompanyList()[Convert.ToInt32(reconParameters.ClientID)];
        //        }

        //        tbReconType.Text = reconParameters.ReconType;
        //        tbTemplate.Text = reconParameters.TemplateName;
        //        string filePath = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters) + ".xml";
        //        if (!File.Exists(filePath))
        //        {
        //            //MessageBox.Show("File does not Exist at path: " + filePath, "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return false;
        //        }
        //        DataSet ds = new DataSet();
        //        //ds.ReadXml(filePath);
        //        ds = XMLUtilities.ReadXmlUsingBufferedStream(filePath);
        //        string rootDirectoryPath = ReconConstants.ReconDataDirectoryPath;
        //        reconParameters.PBFilePath = ReconUtilities.GetReconFilePath(rootDirectoryPath, reconParameters);
        //        //set the file path  for amendments
        //        //ctrlReconOutput1.SetValues(reconParameters);

        //        //checks if table exist in dataset
        //        if (ds.Tables.Count == 0)
        //        {

        //            //ctrlReconOutput1.SetGridDataSource(null, null);
        //            return false;
        //        }
        //        ListEventAargs args = new ListEventAargs();
        //        args.argsObject = ds.Tables[0];
        //        //ctrlReconOutput1.SetGridDataSource(args,  reconParameters.TemplateKey, true);


        //        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1431
        //        //[Recon] - Data is not amending from recon dashboard row
        //        //Prana.Admin.BLL.ModuleResources module = Prana.Admin.BLL.ModuleResources.ReconCancelAmend;
        //        //var hasWritePermForRecon = Prana.Admin.BLL.AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Write);
        //        //if (!hasWritePermForRecon )
        //        //{
        //        AuthAction permissionLevel = CachedDataManagerRecon.GetInstance.GetPermissionLevel();
        //        if (permissionLevel == AuthAction.Write || permissionLevel == AuthAction.Approve)
        //{
        //            btnPostTransaction.Enabled = true;
        //        }
        //        else
        //        {
        //            btnPostTransaction.Enabled = false;
        //        }
        //        //ctrlReconOutput1.DisableAmendments(false);
        //        return true;
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //    return true;
        //    #endregion
        //}
        #endregion

    }
}
