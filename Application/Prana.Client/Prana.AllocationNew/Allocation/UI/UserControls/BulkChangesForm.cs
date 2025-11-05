using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class BulkChangesForm : Form
    {
        private Dictionary<int, string> preferenceList;

        public BulkChangesForm()
        {
            InitializeComponent();
        }

        public BulkChangesForm(Dictionary<int, string> preferenceList)
        {
            InitializeComponent();
            try
            {
                this.preferenceList = preferenceList;
                bulkChangeControl1.BindPreferenceList(preferenceList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }                
            }
        }
        /// <summary>
        /// add on bulk change from to apply theme,	PRANA-10523 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bulkChangeControl1_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(this);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
