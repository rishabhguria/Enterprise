using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.PM.Client.UI
{
    public partial class CtrlImportPreferences : UserControl
    {

        #region GLOBAL VARIABLES
        int _userID = int.MinValue;
        string _startPath = string.Empty;
        string _importPrefDirectoryPath = string.Empty;
        string _importPrefFilePath = string.Empty;
        #endregion

        private ImportPreferences _importPref = null;
        public ImportPreferences ImportPrefs
        {
            get { return _importPref; }
            set { _importPref = value; }
        }

        public CtrlImportPreferences()
        {
            InitializeComponent();
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                LoadPreferences();
            }

        }

        private void btnBrowseExpReport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            Control btn = sender as Control;

            if (folderDialog.ShowDialog(btn.FindForm()) == DialogResult.OK)
            {
                txtReportSavePath.Text = folderDialog.SelectedPath;
            }
        }

        public void SetPreferences()
        {
            if (_importPref != null)
            {
                txtReportSavePath.Text = _importPref.DirectoryPath;
            }
            else
            {
                txtReportSavePath.Text = _startPath;
            }

        }

        private void LoadPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _startPath = System.Windows.Forms.Application.StartupPath;
            _importPrefDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + _userID.ToString();
            _importPrefFilePath = _importPrefDirectoryPath + @"\ImportPreferences.xml";
            _importPref = new ImportPreferences();

            try
            {
                if (!Directory.Exists(_importPrefDirectoryPath))
                {
                    Directory.CreateDirectory(_importPrefDirectoryPath);
                }

                if (File.Exists(_importPrefFilePath))
                {
                    using (FileStream fs = File.OpenRead(_importPrefFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ImportPreferences));
                        _importPref = (ImportPreferences)serializer.Deserialize(fs);
                    }
                }
            }

            #region Catch
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
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _importPref.DirectoryPath = txtReportSavePath.Text;

                if (_importPref.DirectoryPath.Equals(string.Empty))
                {
                    lblStatus.Text = "Please enter Location";
                }
                if (this.SavePreferences())
                    lblStatus.Text = "Import Preferences saved successfully!";

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

        private bool SavePreferences()
        {
            bool isSaved = false;
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_importPrefFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ImportPreferences));
                    serializer.Serialize(writer, _importPref);

                    writer.Flush();
                    isSaved = true;
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
            return isSaved;
        }

        private void CtrlImportPreferences_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnBrowseExpReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowseExpReport.ForeColor = System.Drawing.Color.White;
                btnBrowseExpReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowseExpReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowseExpReport.UseAppStyling = false;
                btnBrowseExpReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

    }

    [XmlRoot("ImportPreferences")]
    [Serializable]
    public class ImportPreferences
    {
        private string _directoryPath = string.Empty;
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }
    }

}
