using Prana.LogManager;
using Prana.User.ViewModel;
using System;
using System.Windows;

namespace Prana.User.View
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public UserProfile()
        {
            InitializeComponent();
            this.Closed += UserProfileWindow_Closed;
        }

        /// <summary>
        /// UserProfileWindow Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserProfileWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (userProfileViewModel != null)
                {
                    userProfileViewModel = null;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// user Profile ViewModel 
        /// </summary>
        public UserProfileViewModel userProfileViewModel
        {
            get { return DataContext as UserProfileViewModel; }
            set { DataContext = value; }
        }

    }
}
