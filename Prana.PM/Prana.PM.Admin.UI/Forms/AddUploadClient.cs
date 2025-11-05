using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class AddUploadClient : Form
    {
        public AddUploadClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the child controls.
        /// </summary>
        public void PopulateChildControls()
        {
            this.ctrlAddUploadClient1.PopulateControl();
        }
    }
}