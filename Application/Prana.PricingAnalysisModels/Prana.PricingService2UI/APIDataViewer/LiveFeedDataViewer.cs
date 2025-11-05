using Infragistics.Win.UltraWinGrid;
using Newtonsoft.Json;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.PricingService2UI.APIDataViewer
{
    public partial class LiveFeedDataViewer : Form
    {
        private const string CONST_LayoutFolder = "Layouts";
        private const string CONST_LayoutAppDataFile = "ApplicationLiveDataViewerGrid.xml";
        private const string CONST_LayoutLiveFeedFile = "LiveFeedDataViewerGrid.xml";

        /// <summary>
        /// PricingService
        /// </summary>
        public IPricingService PricingService { get; set; }

        /// <summary>
        /// LiveFeedDataViewer
        /// </summary>
        public LiveFeedDataViewer()
        {
            try
            {
                InitializeComponent();
                if (!Directory.Exists(Application.StartupPath + "\\" + CONST_LayoutFolder))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\" + CONST_LayoutFolder);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// handle btn Fetch Live Feed Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFetchLiveFeed_Click(object sender, EventArgs e)
        {
            try
            {
                var data = await PricingService2Manager.PricingService2Manager.GetInstance.GetUpdatedLiveFeedDataFromLiveCache();

                if (data != null)
                {
                    var list = data.Select(x => x.Value).ToList();
                    grdAppData.DataSource = list;

                    String filePath = Application.StartupPath + "\\" + CONST_LayoutFolder + "\\" + CONST_LayoutAppDataFile;
                    if (File.Exists(filePath))
                    {
                        grdAppData.DisplayLayout.LoadFromXml(filePath);
                    }

                    toolStripLabel1.Text = "Data Loaded. rows: " + list.Count;
                }
                else
                {
                    MessageBox.Show(this, "Something went wrong or Pricing service not started yet", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void chkDebugMode_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                var message = string.Empty;
                if (chkDebugMode.Checked)
                {
                    var tolerancePct = Convert.ToDouble(textBxTolerance.Text);
                    await PricingService2Manager.PricingService2Manager.GetInstance.SetDebugEnableDisable(true, tolerancePct);
                    message = "Debugging started with tolerance multiple of " + tolerancePct;
                }
                else
                {
                    await PricingService2Manager.PricingService2Manager.GetInstance.SetDebugEnableDisable(false, 0.0);
                    message = "Debugging stopped.";
                }
                toolStripLabel1.Text = message;

                Logger.LoggerWrite(DateTime.Now.ToLongTimeString() + " " + message, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                InformationReporter.GetInstance.Write(DateTime.Now.ToLongTimeString() + " " + message);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser((PranaUltraGrid)sender);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// handle btn Fetch Live Feed Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGetActualFeed_Click(object sender, EventArgs e)
        {
            try
            {
                var dataObject = await PricingService2Manager.PricingService2Manager.GetInstance.GetLiveDataDirectlyFromFeed();
                if (dataObject != null)
                {
                    DataTable data = JsonConvert.DeserializeObject<DataTable>(dataObject.ToString());
                    grdLiveFeedData.DataSource = data;
                    String filePath = Application.StartupPath + "\\" + CONST_LayoutFolder + "\\" + CONST_LayoutLiveFeedFile;
                    if (File.Exists(filePath))
                    {
                        grdLiveFeedData.DisplayLayout.LoadFromXml(filePath);
                    }
                    toolStripLabel1.Text = "Data Loaded.";
                }
                else
                {
                    MessageBox.Show(this, "Pricing service not started yet", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// handle btn Fetch Export Data Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportData_Click(object sender, EventArgs e)
        {
            try
            {
                //Saving to Excel file. This launches the Save dialog for the user to select the Save Path
                if (grdAppData.Rows.Count > 0)
                {
                    CreateExcel(FindSavePathForExcel(), grdAppData);
                }
                else
                    toolStripLabel1.Text = "No data to Export !";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //Any cleanup code
                this.Cursor = Cursors.Default;
            }
        }

        public static String FindSavePathForExcel()
        {
            string filepath = null;
            try
            {
                SaveFileDialog saveFileDialogSymbol = new SaveFileDialog();
                saveFileDialogSymbol.InitialDirectory = Application.StartupPath;
                saveFileDialogSymbol.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialogSymbol.RestoreDirectory = true;
                if (saveFileDialogSymbol.ShowDialog() == DialogResult.OK)
                {
                    filepath = saveFileDialogSymbol.FileName;
                }
                else
                {
                    return filepath;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return filepath;
        }

        /// <summary>
        /// Create Excel 
        /// </summary>
        /// <param name="filepath"></param>
        private void CreateExcel(String filepath, UltraGrid grid)
        {
            try
            {
                if (filepath != null)
                {
                    grdExclExporter.Export(grid, filepath);
                    MessageBox.Show("Grid data successfully downloaded to " + filepath, "Prana Excel Exporter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// handle btn Save Layout Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\" + CONST_LayoutFolder + "\\" + CONST_LayoutAppDataFile;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                grdAppData.DisplayLayout.SaveAsXml(filePath);

                toolStripLabel1.Text = "Layout has been Saved!";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// LiveFeedDataViewer_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LiveFeedDataViewer_Load(object sender, EventArgs e)
        {
            try
            {
                var data = await PricingService2Manager.PricingService2Manager.GetInstance.GetDebugEnableDisableParams();
                if (data != null)
                {
                    textBxTolerance.Text = data.Item2.ToString();
                    chkDebugMode.CheckedChanged -= chkDebugMode_CheckedChanged;
                    chkDebugMode.Checked = data.Item1;
                    chkDebugMode.CheckedChanged += chkDebugMode_CheckedChanged;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnExportFeedData_Click(object sender, EventArgs e)
        {
            try
            {
                //Saving to Excel file. This launches the Save dialog for the user to select the Save Path
                if (grdLiveFeedData.Rows.Count > 0)
                {
                    CreateExcel(FindSavePathForExcel(), grdLiveFeedData);
                }
                else
                    toolStripLabel1.Text = "No data to Export !";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //Any cleanup code
                this.Cursor = Cursors.Default;
            }
        }

        private void btnFeedSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\" + CONST_LayoutFolder + "\\" + CONST_LayoutLiveFeedFile;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                grdLiveFeedData.DisplayLayout.SaveAsXml(filePath);

                toolStripLabel1.Text = "Layout has been Saved!";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
