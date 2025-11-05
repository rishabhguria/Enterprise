using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.NirvanaQualityChecker
{
    public partial class ErrorViewer : Form
    {
        public ErrorViewer()
        {
            InitializeComponent();
        }
        int _key = 0;

        Detail ds = new Detail();
        int _maxtable;

        string[] _arrErrorMessage = null;

        private void ErrorViewer_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME,
                    CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" +
                                                              CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text,
                    CustomThemeHelper.UsedFont);

                ultraGrid1.DisplayLayout.Override.AllowRowFiltering =
                    DefaultableBoolean.True;
                try
                {
                    _maxtable = QualityCheck.ErrorDataSet.Tables.Count;
                    ButtonEnable();

                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("No Errors Detected");
                    Dispose();
                }
                _arrErrorMessage = ds.Error.Split('|');
                ultraGrid1.DataSource = QualityCheck.ErrorDataSet.Tables[_key];
                ultraGrid1.DisplayLayout.Bands[0].Override.HeaderAppearance.TextHAlign = HAlign.Center;
                ultraGrid1.DisplayLayout.Appearance.TextHAlign = HAlign.Right;
                lblMoudleDetail.Text = ds.Module;
                lblScriptDetail.Text = ds.Script;
                lblErrorMsgDetail.Text = _arrErrorMessage[_key];
                label3.Text = _key + 1 + "/" + _maxtable;
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

        public void ButtonEnable()
        {
            if (_maxtable == 1)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                if (_maxtable > 1)
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _key++;
                if (_key < _maxtable)
                {
                    ultraGrid1.DataSource = QualityCheck.ErrorDataSet.Tables[_key];
                    button1.Enabled = true;
                    ultraGrid1.Refresh();
                    if (_key == _maxtable - 1)
                        button2.Enabled = false;
                }
                else
                {
                    _key = 0;
                    ultraGrid1.Refresh();
                }
                label3.Text = _key + 1 + "/" + _maxtable.ToString();

                lblErrorMsgDetail.Text = _arrErrorMessage[_key];
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


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _key--;
                ultraGrid1.DataSource = QualityCheck.ErrorDataSet.Tables[_key];
                if (_key == 0)
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                }
                ultraGrid1.Refresh();
                label3.Text = _key + 1 + "/" + _maxtable.ToString();
                lblErrorMsgDetail.Text = _arrErrorMessage[_key];
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
        /// <summary>
        /// function for fenerating .xls file.
        /// </summary>
        public async void SaveToExcel()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();

                saveFileDialog.Filter = "Excel file (*.xls)|*.xls";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string theFile = saveFileDialog.FileName;
                    await Task.Run(() => ultraGridExcelExporter1.Export(ultraGrid1, theFile));
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

        /// <summary>
        /// toolstripmenu for export to xls file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveToExcel();
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
        /// <summary>
        /// menu will popup after Right Click on Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Right) return;
                contextMenuStrip1.Show(this, new Point(e.X, e.Y + 55));
                contextMenuStrip1.BackColor = Color.White;
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

        private void ultraGrid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                ultraGrid1.DisplayLayout.Bands[0].Override.RowAppearance.BackColor = Color.White;
                ultraGrid1.DisplayLayout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.FromArgb(240, 248, 255);
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


    }
}
