using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.RuleEngine.ImportExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.RuleEngine.UserControls
{
    public partial class ImportRuleSelection : Form
    {
        private Dictionary<string, ImportExport.ImportDefinition> _importDefCache;

        public ImportRuleSelection()
        {

        }

        public ImportRuleSelection(ref Dictionary<string, ImportDefinition> importDefCache)
        {
            try
            {
            InitializeComponent();
            this._importDefCache = importDefCache;
            foreach (String key in importDefCache.Keys)
            {
                ListViewItem item = new ListViewItem(key);
                item.Text = key;
                item.SubItems.AddRange(new String[] {importDefCache[key].NewRuleName, importDefCache[key].RuleName, importDefCache[key].RuleCategory, importDefCache[key].PackageName });
                //item.SubItems["RuleName"].Text = importDefCache[key].NewRuleName;
                //item.SubItems["Category"].Text = importDefCache[key].RuleCategory;
                //item.SubItems["Package"].Text = importDefCache[key].PackageName;
                ulstView.Items.Add(item);
            }
            }
            catch (Exception ex)
            {
             // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ulstView.CheckedItems.Count >= 0)
            {
                List<String> notSelectedKeyList = new List<string>();
                foreach (ListViewItem item in ulstView.Items)
                {
                    if (!item.Checked)
                        notSelectedKeyList.Add(item.Text);
                }


                foreach (String key in notSelectedKeyList)
                {
                    this._importDefCache.Remove(key);
                }
            }
        }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
    }
}
