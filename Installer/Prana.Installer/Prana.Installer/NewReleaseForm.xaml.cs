using System.Windows;

namespace Prana.Installer
{
    public partial class NewReleaseForm : Window
    {
        public string ClientName = "";

        public NewReleaseForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Next_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(releaseName.Text))
                {
                    ClientName = releaseName.Text;

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please fill all required fields", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Please fill correct values", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}