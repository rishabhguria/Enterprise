using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.AdminTools
{
    public partial class FileUpLoadForm : Form, IPluggableTools
    {
        Dictionary<string, UsrCtrlFileUpLoad> _dictControls = new Dictionary<string, UsrCtrlFileUpLoad>();
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
                    if (fileType != ApplicationConstants.MappingFileType.ThirdPartyXSLT.ToString() && fileType != ApplicationConstants.MappingFileType.PMImportXSLT.ToString())
                    {
                        tabCtrl.Tabs.Add(fileType, fileType);
                        UsrCtrlFileUpLoad usrCtrl = new UsrCtrlFileUpLoad(fileTypeID);
                        tabCtrl.Tabs[fileType].TabPage.Controls.Add(usrCtrl);
                        usrCtrl.Dock = DockStyle.Fill;
                        _dictControls.Add(fileType, usrCtrl);
                    }
                }
                DataTable dt = PranaDataManager.GetAllFiles();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["FileType"] != null)
                    {
                        string key = ((ApplicationConstants.MappingFileType)int.Parse(row["FileType"].ToString())).ToString();
                        if (_dictControls.ContainsKey(key))
                        {
                            _dictControls[key].AddRow(row);
                        }
                    }
                }
                foreach (string fileType in fileTypes)
                {
                    if (_dictControls.ContainsKey(fileType))
                    {
                        _dictControls[fileType].BindData();
                    }
                }
                if (PluggableToolsClosed != null) { }
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


        #region IPluggableTools Members

        public void SetUP()
        {

        }

        #endregion
    }
}