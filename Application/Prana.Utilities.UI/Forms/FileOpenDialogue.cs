using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;


namespace Prana.Utilities.UI
{
    public partial class FileOpenDialogue : System.Windows.Forms.Form
    {
        public FileOpenDialogue()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        string _filePath;
        public FileOpenDialogue(string fileName, string filePath)
        {
            InitializeComponent();

            _filePath = filePath;
            lblFileName.Text = "The File named  ' " + fileName.ToString() + " '  has generated.";
        }

        private void SetButtonsColor()
        {
            try
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    //System.Diagnostics.Process.Start(_filePath);
                    //http://jira.nirvanasolutions.com:8080/browse/SENS-54
                    //IF we only specify the path in Process.Start(), windows has to determine the application to open that file.
                    //and if the PM data writer is on, already an excel file is being processed. 
                    // the application hangs on opening a new file as we are not able to close file open dialog box. 
                    //Thus if we specify the type of file like "Excel.exe" it opens properly and closes the file open dialog box.
                    //NKJ 20120807
                    //to check excel installed or not on the target machine
                    Type officeType = Type.GetTypeFromProgID("Excel.Application");
                    if (officeType != null)
                    {
                        //"\""
                        System.Diagnostics.Process.Start("Excel.exe", "\"" + _filePath + "\"");
                    }
                    else
                    {
                        MessageBox.Show("File cannot be viewed as Microsoft Office Excel is not installed on the machine", "Warning");
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("File does Not Exists", "Warning");
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                MessageBox.Show(ex.Message.ToString());
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}