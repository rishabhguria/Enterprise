using Prana.LogManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI.MiscUtilities
{
    public class ExcelUtilities
    {
        public static String FindSavePathForExcel()
        {
            string filepath = null;
            try
            {
                SaveFileDialog saveFileDialogSymbol = new SaveFileDialog();
                saveFileDialogSymbol.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return filepath;
        }
    }
}
