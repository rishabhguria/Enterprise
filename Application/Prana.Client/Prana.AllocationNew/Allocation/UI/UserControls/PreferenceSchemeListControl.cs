using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;
using Infragistics.Win;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Allocation.Common.Definitions;
using Prana.Utilities.MiscUtilities;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class PreferenceSchemeListControl : UserControl
    {

        public event AllocationPrefOperationHandler AllocationPrefOperationEvent;
        public event ApplyBulkChangeHandler ApplyBulkChangePrefEvent;

        /// <summary>
        /// List of newly added preferences
        /// </summary>
        List<string> _newAddedPreferences = new List<string>();
        
        public PreferenceSchemeListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Works according to item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {               
                switch (e.Tool.Key)
                {
                    case "BtnAdd":    // ButtonTool
                        e.Tool.SharedProps.Enabled = false;
                        AddPreference();
                        break;

                    case "BtnDelete":    // ButtonTool
                        DeletePreference();
                        break;
                    // Two new button added Import and Export All, PRANA-10360
                    case "BtnImport":    // ButtonTool
                        ImportPreference();
                        break;

                    case "BtnExportAll":    // ButtonTool
                        ExportAllPreference();
                        break;

                    case "BtnCopy":    // ButtonTool
                        e.Tool.SharedProps.Enabled = false;
                        CopyPreference();
                        break;

                    case "BtnBulkChanges":    // ButtonTool    
                        Dictionary<int, string> preferenceList = new Dictionary<int, string>();
                        foreach (UltraListViewItem item in ultraLstPrefSchemes.Items.All)
                        {
                            if (preferenceList.ContainsKey(int.Parse(item.Key)))
                                preferenceList[int.Parse(item.Key)] = item.Text;
                            else
                                preferenceList.Add(int.Parse(item.Key), item.Text);
                        }
                        BulkChangesForm form = new BulkChangesForm(preferenceList);
                        //TODO: Need to unwind event.
                        form.bulkChangeControl1.ApplyBulkChangeEvent += bulkChangeControl1_ApplyBulkChangeEvent;                        
                        DialogResult dr = form.ShowDialog(this.FindForm());                        
                        break;
                }
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
        /// Apply bulk changes to all preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bulkChangeControl1_ApplyBulkChangeEvent(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                ApplyBulkChangeEventArgs evntArgs = new ApplyBulkChangeEventArgs() { };
                if (ApplyBulkChangePrefEvent != null)
                    ApplyBulkChangePrefEvent(this, e);
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
        /// Creates new Item.
        /// </summary>
        private void CopyPreference()
        {
            try
            {               
                UltraListViewItem copyItem = new UltraListViewItem();
                if (copyItem != null)
                {
                    UltraListViewItem lastitem = (UltraListViewItem)ultraLstPrefSchemes.Items.GetItem(ultraLstPrefSchemes.Items.Count - 1);
                    copyItem.Tag = ultraLstPrefSchemes.ActiveItem.Key;
                    copyItem.Key = AllocationPrefOperation.Copy.ToString(); 
                    ultraLstPrefSchemes.SelectedItems.Clear(); 
                    ultraLstPrefSchemes.Items.Add(copyItem);
                    copyItem.SubItems["Position"].Value = Convert.ToInt32(lastitem.SubItems["Position"].Value) + 1;
                    ultraLstPrefSchemes.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                    ultraLstPrefSchemes.ItemSettings.AllowEdit = DefaultableBoolean.True;
                    copyItem.BeginEdit();
                }
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
        /// Raises Event for deleting Preference
        /// </summary>
        private void DeletePreference()
        {
            try
            {
                UltraListViewItem deleteItem = ultraLstPrefSchemes.ActiveItem;
                if (deleteItem != null)
                {
                    DialogResult dr =  MessageBox.Show(this, "Delete preference " + deleteItem.Text + "?", "Nirvana Preferences", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        if (AllocationPrefOperationEvent != null)
                            AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Delete, PrefId = Convert.ToInt32(deleteItem.Key) });
                    }
                }
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
        /// Adds new list view item
        /// </summary>
        private void AddPreference()
        {
            try
            {                
                // if there are items in ultraLstPrefSchemes, then set last item as lastitem
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8224
                UltraListViewItem lastitem = null;
                if (ultraLstPrefSchemes.Items.Count > 0)
                    lastitem = (UltraListViewItem)ultraLstPrefSchemes.Items.GetItem(ultraLstPrefSchemes.Items.Count - 1);
                ultraLstPrefSchemes.SelectedItems.Clear();
                UltraListViewItem item = new UltraListViewItem();                
                item.Key = "-2";
                item.Tag = AllocationPrefOperation.Add.ToString();
                ultraLstPrefSchemes.Items.Add(item);
                // if there are no items in ultraLstPrefSchemes, then set position equal to 1.
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8224
                item.SubItems["Position"].Value = (lastitem != null) ? (Convert.ToInt32(lastitem.SubItems["Position"].Value) + 1) : 1;
                ultraLstPrefSchemes.ItemSettings.AllowEdit = DefaultableBoolean.True;
                ultraLstPrefSchemes.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                item.BeginEdit();
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
        /// Works on item editing is exited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstPrefSchemes_ItemExitedEditMode(object sender, ItemExitedEditModeEventArgs e)
        {
            try
            {
                ultraLstPrefSchemes.ItemSettings.ActiveAppearance.ForeColor = Color.White;
                if (e.Item.Tag.ToString().Equals(AllocationPrefOperation.Add.ToString()))
                {
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Add, PrefName = e.Item.Text.Trim(), PrefId = Convert.ToInt32(e.Item.Key) });
                    this.ultraToolbarsManager1.Tools["BtnAdd"].SharedProps.Enabled = true;
                }
                else if (e.Item.Key.Equals(AllocationPrefOperation.Copy.ToString()))
                {
                    e.Item.Key = "-1";
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Copy, PrefName = e.Item.Text.Trim(), PrefId = Convert.ToInt32(e.Item.Key.ToString()), CopyPrefId = Convert.ToInt32(e.Item.Tag.ToString()) });
                    this.ultraToolbarsManager1.Tools["BtnCopy"].SharedProps.Enabled = true;
                }
                else if (e.Item.Tag.ToString().Equals(AllocationPrefOperation.Rename.ToString()))
                {
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Rename, PrefName = e.Item.Text.Trim(), PrefId = Convert.ToInt32(e.Item.Key) });
                }
                
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
        /// Updates preference list
        /// </summary>
        /// <param name="preferenceId"></param>
        /// <param name="preferenceName"></param>
        /// <param name="oldId"></param>
        internal void UpdateList(int preferenceId, string preferenceName, int oldId,int postion)
        {
            try
            {
                if (ultraLstPrefSchemes.Items.Exists(oldId.ToString()))
                    ultraLstPrefSchemes.Items.Remove(ultraLstPrefSchemes.Items[oldId.ToString()]);
                UltraListViewItem item = new UltraListViewItem();
                item.Key = preferenceId.ToString();
                item.Tag = preferenceId;
                item.Value = preferenceName;
                ultraLstPrefSchemes.Items.Add(item);
                ultraLstPrefSchemes.Items[item.Key].SubItems["Position"].Value = postion;
                ultraLstPrefSchemes.Items[item.Key].Activate();

                // If new preference is added store its id in _newAddedPreferences
                if (oldId == -2)
                    _newAddedPreferences.Add(item.Key);
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
        /// Remove Item from list view.
        /// </summary>
        /// <param name="prefernceId"></param>
        internal void RemoveItem(int prefernceId)
        {
            try
            {
                UltraListViewItem item = ultraLstPrefSchemes.Items[prefernceId.ToString()];
                ultraLstPrefSchemes.Items.Remove(item);
                if (ultraLstPrefSchemes.Items.Count > 0)
                    ultraLstPrefSchemes.Items[0].Activate();
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
        /// On item activation open preference for item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstPrefSchemes_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            try
            {
                if (e.Item != null && (e.Item.Tag.ToString() != AllocationPrefOperation.Add.ToString() && e.Item.Key.ToString() != AllocationPrefOperation.Copy.ToString()))
                {
                    ultraLstPrefSchemes.ItemSettings.AllowEdit = DefaultableBoolean.False;
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Open, PrefName = e.Item.Text.ToString(), PrefId = Convert.ToInt32(e.Item.Key) });
                }
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
        /// Updates list on List view control.
        /// </summary>
        /// <param name="allocationOperationPref"></param>
        internal void UpdateList(List<AllocationOperationPreference> allocationOperationPref)
        {
            try
            {
                foreach (AllocationOperationPreference pref in allocationOperationPref)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Key = pref.OperationPreferenceId.ToString();
                    item.Tag = pref.OperationPreferenceId;
                    item.Value = pref.OperationPreferenceName;
                    ultraLstPrefSchemes.Items.Add(item);
                    ultraLstPrefSchemes.Items[item.Key].SubItems["Position"].Value = pref.PositionPrefId;                    
                }
                if (ultraLstPrefSchemes.Items.Count > 0)
                    ultraLstPrefSchemes.Items[0].Activate();
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
        /// Returns selected item.
        /// </summary>
        /// <returns></returns>
        internal int GetSelectedItemId()
        {
            try
            {

                if (ultraLstPrefSchemes.Items.Count > 0)
                    return Convert.ToInt32(ultraLstPrefSchemes.ActiveItem.Key);
                else return -1;
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
                return -1;
            }
        }

        /// <summary>
        /// Sets context menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstPrefSchemes_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = ultraLstPrefSchemes.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    UltraListViewItem item = element.GetContext(typeof(UltraListViewItem)) as UltraListViewItem;

                    if (item == null && e.Button == MouseButtons.Right)
                    {
                        ultraLstPrefSchemes.SelectedItems.Clear();
                        cntxtMnuPreference.Visible = true;
                        addPreferenceToolStripMenuItem.Visible = true;
                        deletePreferenceToolStripMenuItem.Visible = false;
                        renamePreferenceToolStripMenuItem.Visible = false;
                        copyPreferenceToolStripMenuItem.Visible = false;
                        importPreferenceToolStripMenuItem.Visible = true;
                        exportAllPreferenceToolStripMenuItem.Visible = true;
                        exportPreferenceToolStripMenuItem.Visible = false;
                    }
                    else if (item != null && e.Button == MouseButtons.Right)
                    {
                        item.Activate();
                        ultraLstPrefSchemes.SelectedItems.Clear();
                        ultraLstPrefSchemes.SelectedItems.Add(item);
                        cntxtMnuPreference.Visible = true;
                        addPreferenceToolStripMenuItem.Visible = false;
                        deletePreferenceToolStripMenuItem.Visible = true;
                        renamePreferenceToolStripMenuItem.Visible = true;
                        copyPreferenceToolStripMenuItem.Visible = true;
                        importPreferenceToolStripMenuItem.Visible = false;
                        exportAllPreferenceToolStripMenuItem.Visible = false;
                        exportPreferenceToolStripMenuItem.Visible = true;
                    }
                    else if (item != null && e.Button == MouseButtons.Left)
                    {
                        item.Activate();
                        ultraLstPrefSchemes.SelectedItems.Clear();
                        ultraLstPrefSchemes.SelectedItems.Add(item);
                    }
                }
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
        /// Works on the basis of context menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMnuPreference_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string item = e.ClickedItem.Tag.ToString();
                cntxtMnuPreference.Visible = false;
                switch (item)
                {
                    case "AddPreference":
                        AddPreference();
                        break;
                    case "DeletePreference":
                        DeletePreference();
                        break;
                    case "CopyPreference":
                        CopyPreference();
                        break;
                    case "RenamePreference":
                        RenamePreference();
                        break;
                    case "ImportPreference":
                        ImportPreference();
                        break;
                    case "ExportPreference":
                        ExportPreference();
                        break;
                    case "ExportAllPreference":
                        ExportAllPreference();
                        break;


                }
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
        /// Start node editing for rename
        /// </summary>
        private void RenamePreference()
        {
            try
            {
                ultraLstPrefSchemes.SelectedItems.Clear();
                UltraListViewItem item = ultraLstPrefSchemes.ActiveItem;                
                item.Tag = AllocationPrefOperation.Rename.ToString();
                ultraLstPrefSchemes.ItemSettings.AllowEdit = DefaultableBoolean.True;
                ultraLstPrefSchemes.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                item.BeginEdit();
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
        /// Imports the preference
        /// </summary>
        private void ImportPreference()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Nirvana Allocation preference files (*.npref)|*.npref";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string[] importExportPaths;
                importExportPaths = openFileDialog.FileNames;
                foreach (string s in importExportPaths)
                {
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Import, ImportExportPath = s });
                }
            }
        }

        /// <summary>
        /// Exports the single preference
        /// </summary>
        private void ExportPreference()
        {
            try
            {
                string importExportPath = String.Empty;

                SaveFileDialog exportFileDialog = new SaveFileDialog();
                exportFileDialog.Title = "Choose location to save your file";
                exportFileDialog.DefaultExt = ".npref";
                exportFileDialog.Filter = "Nirvana Allocation preference files (*.npref)|*.npref";
                exportFileDialog.FilterIndex = 1;
                exportFileDialog.CheckPathExists = true;
                exportFileDialog.FileName = ultraLstPrefSchemes.ActiveItem.Text;
                if (exportFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    importExportPath = exportFileDialog.FileName;
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Export, PrefId = Convert.ToInt32(ultraLstPrefSchemes.ActiveItem.Key), ImportExportPath = importExportPath });
                }
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
        /// Exports preference list
        /// </summary>
        private void ExportAllPreference()
        {
            try
            {
                string importExportPath = String.Empty;

                FolderBrowserDialog exportBrowserDialog = new FolderBrowserDialog();
                if (exportBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    importExportPath = exportBrowserDialog.SelectedPath;
                    if (AllocationPrefOperationEvent != null)
                        AllocationPrefOperationEvent(this, new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.ExportAll, ImportExportPath = importExportPath });
                }
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
        /// gets newly added preferences
        /// </summary>
        /// <returns>list of new preference Ids</returns>
        internal List<string> GetNewPreferenceList()
        {
            try
            {
                return DeepCopyHelper.Clone(_newAddedPreferences);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
