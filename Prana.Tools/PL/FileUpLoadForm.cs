using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class FileUpLoadForm : Form, IPluggableTools
    {
        Dictionary<string, UserCtrlFileUpLoad> _dictControls = new Dictionary<string, UserCtrlFileUpLoad>();
        public FileUpLoadForm()
        {
            try
            {

                InitializeComponent();
                string[] fileTypes = Enum.GetNames(typeof(ApplicationConstants.MappingFileType));
                tabCtrl.Tabs.Clear();
                tabCtrl.Dock = DockStyle.Fill;
                _dictControls.Clear();
                foreach (string fileType in fileTypes)
                {
                    int fileTypeID = Convert.ToInt32(Enum.Parse(typeof(ApplicationConstants.MappingFileType), fileType));
                    tabCtrl.Tabs.Add(fileType, fileType);
                    UserCtrlFileUpLoad usrCtrl = new UserCtrlFileUpLoad(fileTypeID);
                    tabCtrl.Tabs[fileType].TabPage.Controls.Add(usrCtrl);
                    usrCtrl.Dock = DockStyle.Fill;
                    _dictControls.Add(fileType, usrCtrl);
                }
                DataTable dt = PranaDataManager.GetAllFiles();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["FileType"] != null)
                    {
                        string key = ((ApplicationConstants.MappingFileType)int.Parse(row["FileType"].ToString())).ToString();
                        _dictControls[key].AddRow(row);
                    }
                }
                foreach (string fileType in fileTypes)
                {
                    _dictControls[fileType].BindData();
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

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        #endregion

        /// <summary>
        /// Pluggable ToolClosed Event 
        /// Created  to avoid the error after applying Managed Rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PluggableToolClosed(object sender, EventArgs e)
        {
            try
            { }
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
        /// Pluggable ToolClosed Event 
        /// Created  to avoid the error after applying Managed Rules
        /// </summary>
        public void WireEvents()
        {
            try
            {
                PluggableToolsClosed += new EventHandler(PluggableToolClosed);
                PluggableToolsClosed(this, new EventArgs());
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

        public void SetUP()
        {
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }
        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }
        #endregion

        #region IPluggableTools Members


        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion
    }
}