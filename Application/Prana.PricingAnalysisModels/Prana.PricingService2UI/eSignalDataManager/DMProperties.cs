using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.PricingService2UI.EsignalDM
{
    public partial class DMProperties : Form
    {
        /// <summary>
        /// The username
        /// </summary>
        private string _username = string.Empty;
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        /// <summary>
        /// The password
        /// </summary>
        private string _passWord = string.Empty;
        public string Password
        {
            get
            {
                return _passWord;
            }
            set
            {
                _passWord = value;
            }
        }
        /// <summary>
        /// The server address
        /// </summary>
        private string _serverAddress = string.Empty;
        public string ServerAddress
        {
            get
            {
                return _serverAddress;
            }
            set
            {
                _serverAddress = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DMProperties" /> class.
        /// </summary>
        public DMProperties()
        {
            try
            {
                InitializeComponent();
                SetUIFields();
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

        public void SetUIFields()
        {
            this.UserNameTextBox.Text = UserName;
            this.PasswordTextBox.Text = Password;
            this.AddressTextBox.Text = ServerAddress;
        }

        /// <summary>
        /// Handles the Click event of the OKButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                if (string.IsNullOrWhiteSpace(UserNameTextBox.Text) || UserNameTextBox.Text.Contains(Seperators.SEPERATOR_6))
                {
                    builder.Append(", Username");
                }
                if (string.IsNullOrWhiteSpace(PasswordTextBox.Text) || PasswordTextBox.Text.Contains(Seperators.SEPERATOR_6))
                {
                    builder.Append(", Password");
                }
                if (string.IsNullOrWhiteSpace(AddressTextBox.Text) || AddressTextBox.Text.Contains(Seperators.SEPERATOR_6))
                {
                    builder.Append(", CM IP Address");
                }
                if (builder.Length != 0)
                {
                    builder.Remove(0, 1);
                    builder.Insert(0, "\u2022 Please enter valid");
                    builder.AppendLine(".");
                }
                if (builder.Length != 0)
                {
                    ErrorMessageBar.Text = builder.ToString();
                }
                else
                {
                    DialogResult response = MessageBox.Show("Changing Data manager properties will reset Connection." + Environment.NewLine + "Do you want to continue?", "Data Manager Alert", MessageBoxButtons.YesNo);
                    if (response.Equals(DialogResult.Yes))
                    {
                        UserName = UserNameTextBox.Text;
                        Password = PasswordTextBox.Text;
                        ServerAddress = AddressTextBox.Text;

                        System.Threading.Tasks.Task.Run(async () => await PricingService2Manager.PricingService2Manager.GetInstance.UpdateLiveFeedDetails(UserName, Password, ServerAddress));
                    }
                    this.Close();
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
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
