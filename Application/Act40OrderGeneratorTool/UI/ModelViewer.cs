using Act40OrderGeneratorTool.Classes;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Act40OrderGeneratorTool
{
    internal partial class ModelViewer : Form
    {
        public ModelViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show the modle to the user
        /// </summary>
        /// <param name="posilist"></param>
        public void SetUp(List<Position> posilist, Double NAV)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_HEAT_MAP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }

                ultraGrid1.DataSource = posilist;
                //ultraGrid1.DisplayLayout.Bands[0].Columns.Add("Percentage");
                foreach (var row in ultraGrid1.Rows)
                {
                    Double d = Convert.ToDouble(row.Cells["DollarDelta"].Value) / NAV * 100;
                    if (Double.IsInfinity(d) || Double.IsNaN(d))
                        d = 0;
                    row.Cells["Percentage"].Value = d.ToString("0.##") + " %";
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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGrid1.Rows.Count > 0)
                {
                    String folderPath = "";
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "CSV Files|*.csv;|Excel Files|*.xls;|All Files|*.*";
                    dialog.Title = "Export Trades";
                    dialog.FileName = String.Format("Position_Export_{0:dd_MMM_yyyy}", DateTime.Now);

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        folderPath = dialog.FileName;
                    }
                    if (!String.IsNullOrEmpty(folderPath))
                    {
                        if (folderPath.ToLower().EndsWith(".xls"))
                        {
                            ultraGridExcelExporter1.Export(ultraGrid1, folderPath);
                        }
                        else
                        {
                            List<String> columnList = new List<String>();
                            List<String> columnHeaderList = new List<String>();
                            foreach (var column in ultraGrid1.DisplayLayout.Bands[0].Columns)
                            {
                                columnList.Add(column.Key);
                                columnHeaderList.Add(column.Header.Caption);
                            }

                            if (File.Exists(folderPath))
                                File.Delete(folderPath);
                            //File.CreateText(folderPath);



                            using (StreamWriter sw = File.AppendText(folderPath))
                            {
                                sw.WriteLine(String.Join(",", columnHeaderList));
                                foreach (var row in ultraGrid1.Rows)
                                {
                                    foreach (var column in columnList)
                                        sw.Write(row.GetCellValue(column) + ",");
                                    sw.WriteLine();
                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show(this, "Nothing To Export.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(this, "Export failed.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
