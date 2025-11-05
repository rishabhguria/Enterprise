using Prana.Global;
using Prana.LogManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI
{
    public class OpenFileDialogHelper
    {
        /// <summary>
        /// Load the default file format for washsale
        /// </summary>
        public static Boolean isComingFromWashSale = false;

        public static bool isComingFromRASImport = false;

        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetFileNameUsingOpenFileDialog(Boolean CheckAccessPermission)
        {
            String fileName = string.Empty;
            try
            {

                OpenFileDialog openFileDialogCtrl = new OpenFileDialog();
                openFileDialogCtrl.Title = "Select File to Import";
                if (isComingFromWashSale || isComingFromRASImport)
                    openFileDialogCtrl.Filter = "Data Files (*.xls,*.csv,*.xlsx)|*.xls;*.csv;*.xlsx| All Files|*.*";
                else
                    openFileDialogCtrl.Filter = "Data Files (*.xls,*.csv)|*.xls;*.csv| All Files|*.*";
                openFileDialogCtrl.FileName = String.Empty;
                //openFileDialogCtrl.RestoreDirectory = true;
                String folderName = "C:\\";
                String folderNameInConfig = ConfigurationHelper.Instance.GetAppSettingValueByKey("AccessibleFolderPathOnServer").ToString();
                bool isAllowToAccessOnServer = false;
                Boolean.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("AllowFileAccessOnReleaseServer").ToString(), out isAllowToAccessOnServer);
                if (!string.IsNullOrWhiteSpace(folderNameInConfig))
                {
                    folderName = folderNameInConfig;
                }

                openFileDialogCtrl.InitialDirectory = "\\\\tsclient\\" + folderName;
                if (isAllowToAccessOnServer)
                {
                    openFileDialogCtrl.InitialDirectory = folderName;
                }

                string path = openFileDialogCtrl.InitialDirectory;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                DialogResult dialogResult = DialogResult.OK;
                if (openFileDialogCtrl.ShowDialog() == DialogResult.OK)
                {
                    while (CheckAccessPermission && !Path.GetDirectoryName(openFileDialogCtrl.FileName).Contains(Path.GetDirectoryName(folderName)) && dialogResult != DialogResult.Cancel)
                    {

                        MessageBox.Show("You don't have permission on this folder. Please contact to admin.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dialogResult = openFileDialogCtrl.ShowDialog();

                    }
                }

                if (dialogResult == DialogResult.OK)
                {
                    fileName = openFileDialogCtrl.FileName;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    MessageBox.Show("Operation canceled by User.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            return fileName;
        }
    }
}
