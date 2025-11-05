using Prana.LogManager;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
namespace Prana.Tools
{
    public partial class DuplicateDataForm : Form
    {
        DataTable data;
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="dt"></param>
        internal DuplicateDataForm(DataTable dt)
        {
            try
            {
                data = dt;
                data.TableName = "SecMasterTable";
                InitializeComponent();
                grdMessages.DataSource = data;
                grdMessages.DataBind();
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
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Prana.Utilities.UI.MiscUtilities.ExcelAndPrintUtilities util = new Prana.Utilities.UI.MiscUtilities.ExcelAndPrintUtilities();
                util.ExportToExcel(grdMessages);
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
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXml_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);

                    data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
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
    }
}