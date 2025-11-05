using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Interfaces;
using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class AllocationPrefMainControl : UserControl, IPreferences
    {
        /// <summary>
        /// Dictionary storing allocation preferences according to the preference ID.
        /// </summary>
        Dictionary<int, AllocationOperationPreference> _allocationOperationPref = new Dictionary<int, AllocationOperationPreference>();

        /// <summary>
        /// Cache that stores changed preference data.
        /// </summary>
        Dictionary<int, AllocationOperationPreference> _openAllocationOperationPref = new Dictionary<int, AllocationOperationPreference>();

        /// <summary>
        /// Dictionary storing initial allocation preferences according to the preference ID.
        /// </summary>
        Dictionary<int, AllocationOperationPreference> _initialAllocationOperationPref=new Dictionary<int,AllocationOperationPreference>();

        /// <summary>
        /// Dictionary storing invalid allocation preferences according to the preference ID.
        /// </summary>
        Dictionary<int, AllocationOperationPreference> _invalidAllocationOperationPref = new Dictionary<int, AllocationOperationPreference>();

        /// <summary>
        /// event to check if Allocation Pref tab selected or not
        /// </summary>
        public event EventHandler IsAllocationPrefTabSelected;

        /// <summary>
        /// variable to check if Allocation Preference tab is selected in Preferences
        /// </summary>
        bool _isAllocationPrefTabSelected = false;

        /// <summary>
        /// Initializes control.
        /// </summary>
        public AllocationPrefMainControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Calls set up method for the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllocationPrefMainControl_Load(object sender, EventArgs e)
        {
            try
            {
                this.SetUp(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser);
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
        /// Loads preferences in _allocationOperationPref dictionary and updates preference scheme list control.
        /// </summary>
        private void LoadPreferences()
        {
            try
            {
                List<AllocationOperationPreference> allocationOperationPrefList = new List<AllocationOperationPreference>();
                allocationOperationPrefList.AddRange(AllocationManager.GetInstance().Allocation.InnerChannel.GetPreferenceByCompanyId(CommonDataCache.CachedDataManager.GetInstance.GetCompanyID(), CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                foreach (AllocationOperationPreference pref in allocationOperationPrefList)
                {
                    if (!pref.OperationPreferenceName.StartsWith("*Custom#_") && !pref.OperationPreferenceName.StartsWith("*WorkArea#_") && !pref.OperationPreferenceName.StartsWith("*PST#_"))
                    {
                        lock (_allocationOperationPref)
                        {
                            if (_allocationOperationPref.ContainsKey(pref.OperationPreferenceId))
                            {
                                _allocationOperationPref[pref.OperationPreferenceId] = pref;
                            }
                            else
                            {
                                _allocationOperationPref.Add(pref.OperationPreferenceId, pref);
                            }
                        }
                    }
                }
                preferenceSchemeListControl1.UpdateList(_allocationOperationPref.Values.ToList());
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
        /// Binds events for all controls.
        /// </summary>
        private void BindAllEvents()
        {
            try
            {
                preferenceSchemeListControl1.AllocationPrefOperationEvent += preferenceSchemeListControl1_AllocationPrefOperationEvent;
                bulkChangeControl1.ApplyBulkChangeEvent += bulkChangeControl1_ApplyBulkChangeEvent;
                preferenceSchemeListControl1.ApplyBulkChangePrefEvent += preferenceSchemeListControl1_ApplyBulkChangePrefEvent;
                defaultRuleControl1.Event += defaultRuleControl1_Event;
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
        /// Updates default rule in general rule control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void defaultRuleControl1_Event(object sender, EventArgs e)
        {
            try
            {
                generalRuleControl1.UpdateDefaultRule(defaultRuleControl1.GetCurrentValues().Clone());
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
        /// Apply bulk changes to all preferences' general allocation rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void preferenceSchemeListControl1_ApplyBulkChangePrefEvent(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                foreach (int prefId in _allocationOperationPref.Keys)
                {
                    if ((e.ApplyOnSelectedPref && e.PreferenceList.Contains(prefId)) || !e.ApplyOnSelectedPref)
                    {
                        if (_openAllocationOperationPref.ContainsKey(prefId))
                        {
                            foreach (int checkListId in _openAllocationOperationPref[prefId].CheckListWisePreference.Keys)
                            {
                                CheckListWisePreference checkList = new CheckListWisePreference();
                                checkList = _openAllocationOperationPref[prefId].CheckListWisePreference[checkListId];

                                if (e.allocationBaseChecked)
                                    checkList.Rule.BaseType = e.Rule.BaseType;
                                if (e.matchingRuleChecked)
                                    checkList.Rule.RuleType = e.Rule.RuleType;
                                if (e.preferencedAccountChecked)
                                    checkList.Rule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                                if (e.matchPortfolioPostionChecked)
                                    checkList.Rule.MatchPortfolioPosition = e.Rule.MatchPortfolioPosition;

                                _allocationOperationPref[prefId].TryUpdateCheckList(checkList, true);
                                if (_openAllocationOperationPref.ContainsKey(prefId))
                                    _openAllocationOperationPref[prefId].TryUpdateCheckList(checkList, true);
                            }
                        }
                        else
                        {
                            foreach (int checkListId in _allocationOperationPref[prefId].CheckListWisePreference.Keys)
                            {
                                CheckListWisePreference checkList = new CheckListWisePreference();
                                checkList = _allocationOperationPref[prefId].CheckListWisePreference[checkListId];

                                if (e.allocationBaseChecked)
                                    checkList.Rule.BaseType = e.Rule.BaseType;
                                if (e.matchingRuleChecked)
                                    checkList.Rule.RuleType = e.Rule.RuleType;
                                if (e.preferencedAccountChecked)
                                    checkList.Rule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                                if (e.matchPortfolioPostionChecked)
                                    checkList.Rule.MatchPortfolioPosition = e.Rule.MatchPortfolioPosition;

                                _allocationOperationPref[prefId].TryUpdateCheckList(checkList, true);
                                if (_openAllocationOperationPref.ContainsKey(prefId))
                                    _openAllocationOperationPref[prefId].TryUpdateCheckList(checkList, true);
                            }
                        }
                        if (e.ApplyOnDefaultRule)
                        {
                            if (e.allocationBaseChecked)
                                _allocationOperationPref[prefId].DefaultRule.BaseType = e.Rule.BaseType;
                            if (e.matchingRuleChecked)
                                _allocationOperationPref[prefId].DefaultRule.RuleType = e.Rule.RuleType;
                            if (e.preferencedAccountChecked)
                                _allocationOperationPref[prefId].DefaultRule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                            if (e.matchPortfolioPostionChecked)
                                _allocationOperationPref[prefId].DefaultRule.MatchPortfolioPosition = e.Rule.MatchPortfolioPosition;

                            _allocationOperationPref[prefId].TryUpdateDefaultRule(e.Rule);
                            if (_openAllocationOperationPref.ContainsKey(prefId))
                                _openAllocationOperationPref[prefId].TryUpdateDefaultRule(e.Rule);
                        }
                    }
                }
                if ((e.ApplyOnSelectedPref && e.PreferenceList.Contains(preferenceSchemeListControl1.GetSelectedItemId())) || !e.ApplyOnSelectedPref)
                {
                    if (e.ApplyOnDefaultRule)
                    defaultRuleControl1.ApplyBulkChange(e);
                    generalRuleControl1.ApplyBulkChanges(e);
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
        /// Apply bulk changes on general grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bulkChangeControl1_ApplyBulkChangeEvent(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                generalRuleControl1.ApplyBulkChanges(e);
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
        /// Assignning default value.
        /// Wiil be changed when pref will be opened.
        /// </summary>
        int openPrefId = -1;

        /// <summary>
        /// Raised from preference list control on different operations.
        /// ADD,Delete,Copy,Rename,Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains operation and prefID, Pref name</param>
        void preferenceSchemeListControl1_AllocationPrefOperationEvent(object sender, AllocationPrefOperationEventArgs e)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = null;
                string errorMessage;
                switch (e.AllocationPrefOperation)
                {
                    case AllocationPrefOperation.Add:
                        if (!string.IsNullOrWhiteSpace(e.PrefName))
                        {
                            if (e.PrefName.Length > 50)
                            {
                                MessageBox.Show(this, "Preference Name can not be greater than 50 characters", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            }
                            else if (e.PrefName.StartsWith("*Custom#_"))
                            {
                                MessageBox.Show(this, "Preference Name can not be start with Custom_", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            }
                            else if (e.PrefName.StartsWith("*WorkArea#_"))
                            {
                                MessageBox.Show(this, "Preference Name can not be start with WorkArea_", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            }                              
                            else if (e.PrefName.StartsWith("*PST#_"))
                            {
                                MessageBox.Show(this, "Preference Name can not be start with PST_", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            }
                            else
                                preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.AddPreference(e.PrefName, CommonDataCache.CachedDataManager.GetInstance.GetCompanyID());
                        }
                        else
                        {
                            MessageBox.Show(this, "Preference name should not be blank.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            preferenceSchemeListControl1.RemoveItem(e.PrefId);
                        }
                        break;
                    case AllocationPrefOperation.Copy:
                        if (!string.IsNullOrWhiteSpace(e.PrefName))
                        {
                            if (_allocationOperationPref[e.CopyPrefId].IsValid(out errorMessage))
                            {
                                if (e.PrefName.Length > 50)
                                {
                                    MessageBox.Show(this, "Preference Name can not be greater than 50 characters", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    preferenceSchemeListControl1.RemoveItem(e.PrefId);
                                }
                                else
                                    preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.CopyPreference(e.CopyPrefId, e.PrefName);
                            }
                            else
                            {
                                MessageBox.Show(this, " Preference is invalid." + errorMessage + "\n So, can not copy.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "New Preference name cannot be blank. So, can not copy.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            preferenceSchemeListControl1.RemoveItem(e.PrefId);
                        }
                        break;
                    case AllocationPrefOperation.Delete:
                        if (_allocationOperationPref.ContainsKey(e.PrefId))
                            preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.DeletePreference(e.PrefId);
                        else
                        {
                            MessageBox.Show(this, "Preference does not exists. So, can not delete.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            preferenceSchemeListControl1.RemoveItem(e.PrefId);
                        }
                        break;
                    case AllocationPrefOperation.Open:
                        lock (_allocationOperationPref)
                        {
                            if (openPrefId != -1)
                            {                                
                                _openAllocationOperationPref[openPrefId].TryUpdateTargetPercentage(accountAllocationControl1.GetCurrentValues());
                                _openAllocationOperationPref[openPrefId].TryUpdateDefaultRule(defaultRuleControl1.GetCurrentValues());
                                _openAllocationOperationPref[openPrefId].TryUpdateCheckList(generalRuleControl1.GetCurrentCheckListPref());
                            }

                            openPrefId = e.PrefId;
                            if (!_openAllocationOperationPref.ContainsKey(openPrefId))
                                _openAllocationOperationPref.Add(openPrefId, _allocationOperationPref[openPrefId].Clone());
                            defaultRuleControl1.UpdateGrid(_openAllocationOperationPref[e.PrefId].DefaultRule);
                            generalRuleControl1.AddRowsToGrid(_openAllocationOperationPref[e.PrefId].CheckListWisePreference);
                            generalRuleControl1.UpdateDefaultRule(_openAllocationOperationPref[e.PrefId].DefaultRule.Clone());
                            accountAllocationControl1.UpdateGrid(_openAllocationOperationPref[e.PrefId].TargetPercentage);
                        }
                        break;
                    case AllocationPrefOperation.Rename:
                        if (_allocationOperationPref.ContainsKey(e.PrefId) && !string.IsNullOrWhiteSpace(e.PrefName))
                        {
                            if (e.PrefName.Length > 50)
                            {
                                MessageBox.Show(this, "Preference Name can not be greater than 50 characters", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                preferenceSchemeListControl1.UpdateList(e.PrefId, _allocationOperationPref[e.PrefId].OperationPreferenceName, e.PrefId, _allocationOperationPref[e.PrefId].PositionPrefId);
                            }
                            else
                            {
                                if (_allocationOperationPref[e.PrefId].OperationPreferenceName.Equals(e.PrefName))
                                    preferenceSchemeListControl1.UpdateList(e.PrefId, _allocationOperationPref[e.PrefId].OperationPreferenceName, e.PrefId, _allocationOperationPref[e.PrefId].PositionPrefId);
                                else
                                preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.RenamePreference(e.PrefId, e.PrefName);
                        }
                        }
                        else
                        {
                            MessageBox.Show(this, "Preference does not exists or Preference name is empty. So, can not rename.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            preferenceSchemeListControl1.UpdateList(e.PrefId, _allocationOperationPref[e.PrefId].OperationPreferenceName, e.PrefId, _allocationOperationPref[e.PrefId].PositionPrefId);
                        }
                        break;
                    case AllocationPrefOperation.Export:

                        lock (_allocationOperationPref)
                        {
                            if (_allocationOperationPref.ContainsKey(e.PrefId))
                            {
                                using (Stream stream = new FileStream(e.ImportExportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    IFormatter formatter = new BinaryFormatter();
                                    formatter.Serialize(stream, _allocationOperationPref[e.PrefId]);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Preference does not exists. So, can not export.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                       break;
					case AllocationPrefOperation.ExportAll:

                        lock (_allocationOperationPref)
                        {
                            foreach (int key in _allocationOperationPref.Keys)
                            {
                                string exportPath = e.ImportExportPath + "\\" + _allocationOperationPref[key].OperationPreferenceName + ".npref";
                                using (Stream stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    IFormatter formatter = new BinaryFormatter();
                                    formatter.Serialize(stream, _allocationOperationPref[key]);
                                }
                            }
                        }

                        break;

                    case AllocationPrefOperation.Import:

                        if (File.Exists(e.ImportExportPath))
                        {
                            AllocationOperationPreference importedPref;
                            using (Stream stream = new FileStream(e.ImportExportPath, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                IFormatter formatter = new BinaryFormatter();
                                importedPref = (AllocationOperationPreference)formatter.Deserialize(stream);
                             }
                            if (importedPref != null)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(e.ImportExportPath);
                                var preferenceName = importedPref.OperationPreferenceName;
                                var containsId = _allocationOperationPref.ContainsKey(importedPref.OperationPreferenceId);
                                var containsName = (_allocationOperationPref.Where(a => a.Value.OperationPreferenceName == importedPref.OperationPreferenceName).Count()) > 0;

                                // to check the file name if it already exists with similar pref id & pref name,PRANA-6714
                                if (containsId && containsName && (fileName == preferenceName))
                                {
                   
                                    MessageBox.Show(this, "Preference with same name exists. So, can not import " + importedPref.OperationPreferenceName, "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (containsName)
                                {
                                    MessageBox.Show(this, "Similar preference exists. So, can not import " + importedPref.OperationPreferenceName, "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (containsId)
                                {
                                    preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.ImportPreference(importedPref);
                                    MessageBox.Show(this, "Similar preference exists. So, cannot import " + importedPref.OperationPreferenceName + "", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    preferenceUpdateResult = AllocationManager.GetInstance().Allocation.InnerChannel.ImportPreference(importedPref);
                                }
                            }

                        }
                        break;

                }
                if (preferenceUpdateResult != null)
                {
                    if (preferenceUpdateResult.Error == null)
                    {
                        if (preferenceUpdateResult.Preference != null)
                        {
                            lock (_allocationOperationPref)
                            {
                                if (_allocationOperationPref.ContainsKey(preferenceUpdateResult.Preference.OperationPreferenceId))
                                    _allocationOperationPref[preferenceUpdateResult.Preference.OperationPreferenceId] = preferenceUpdateResult.Preference;
                                else
                                    _allocationOperationPref.Add(preferenceUpdateResult.Preference.OperationPreferenceId, preferenceUpdateResult.Preference);
                            }
                            preferenceSchemeListControl1.UpdateList(preferenceUpdateResult.Preference.OperationPreferenceId, preferenceUpdateResult.Preference.OperationPreferenceName, e.PrefId, preferenceUpdateResult.Preference.PositionPrefId);
                            defaultRuleControl1.UpdateGrid(preferenceUpdateResult.Preference.DefaultRule);
                            generalRuleControl1.AddRowsToGrid(preferenceUpdateResult.Preference.CheckListWisePreference);
                            generalRuleControl1.UpdateDefaultRule(preferenceUpdateResult.Preference.DefaultRule.Clone());
                            accountAllocationControl1.UpdateGrid(preferenceUpdateResult.Preference.TargetPercentage);
                        }
                        else
                        {
                            preferenceSchemeListControl1.RemoveItem(e.PrefId);
                            _allocationOperationPref.Remove(e.PrefId);
                            //added to remove the deleted item, PRANA-11281
                            if(_openAllocationOperationPref.ContainsKey(e.PrefId))
                                _openAllocationOperationPref.Remove(e.PrefId);
                        }

                    }
                    else
                    {
                        MessageBox.Show(this, preferenceUpdateResult.Error, "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (e.AllocationPrefOperation == AllocationPrefOperation.Add || e.AllocationPrefOperation == AllocationPrefOperation.Copy)
                            preferenceSchemeListControl1.RemoveItem(e.PrefId);
                        else if (e.AllocationPrefOperation == AllocationPrefOperation.Rename)
                            preferenceSchemeListControl1.UpdateList(e.PrefId, _allocationOperationPref[e.PrefId].OperationPreferenceName, e.PrefId, _allocationOperationPref[e.PrefId].PositionPrefId);
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
        
        #region IPreferences Members

        /// <summary>
        /// BInds events and loads preferences.
        /// </summary>
        /// <param name="user"></param>
        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            try
            {
                BindAllEvents();
                LoadPreferences();
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

        public UserControl Reference()
        {
            return this;
        }

        /// <summary>
        /// Ask user if wants to save all preferences or only the selected one.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            try
            {
                UpdateInitialPreferences();
                //dailogBox is in beginning of the function so that it can be checked whether current tab is allocation pref tab or not
                if (IsAllocationPrefTabSelected != null)
                    IsAllocationPrefTabSelected(this, null);
                foreach (int id in _invalidAllocationOperationPref.Keys)
                {
                    if (_allocationOperationPref.ContainsKey(id))
                    {
                        if (!_openAllocationOperationPref.ContainsKey(id))
                            _openAllocationOperationPref.Add(id, _invalidAllocationOperationPref[id].Clone());
                    }
                }
                if (IsAllocationPreferenceUpdated())
                {
                DialogResult dr = MessageBox.Show(this, "There are changes in other preferences also. Do you want to save all Preferences?", "Nirvana Preferences", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (_isAllocationPrefTabSelected || dr == DialogResult.Yes)
                {
                    int selectedId = preferenceSchemeListControl1.GetSelectedItemId();
                    AllocationOperationPreference pref = new AllocationOperationPreference();
                    lock (_allocationOperationPref)
                    {
                        //if preference does not exists then do not save.
                        if (_allocationOperationPref.ContainsKey(selectedId))
                            pref = _allocationOperationPref[selectedId].Clone();
                        else
                            return false;
                    }
                        bool result = pref.TryUpdateTargetPercentage(accountAllocationControl1.GetCurrentValues());
                    if (result)
                        result = pref.TryUpdateDefaultRule(defaultRuleControl1.GetCurrentValues());
                    else
                        MessageBox.Show(this, "Percentage is not valid.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result)
                    {
                        SerializableDictionary<int, CheckListWisePreference> dict = generalRuleControl1.GetCurrentCheckListPref();
                        if (dict != null)
                        {
                            result = pref.TryUpdateCheckList(dict);
                            // Message when result from TryUpdateCheckList() method is false
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                            if (!result)
                            {
                                MessageBox.Show(this, "General Rule is duplicating.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                        }
                        else
                            result = false;
                    }
                    else
                        MessageBox.Show(this, "Invalid/non reachable default rule.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (result)
                    {
                        if (dr == DialogResult.No)
                        {
                            bool isPrefernceSaved = false;
                            foreach (int id in _openAllocationOperationPref.Keys)
                            {
                                if (!_invalidAllocationOperationPref.ContainsKey(id) && !_openAllocationOperationPref[id].IsValid())
                                    _invalidAllocationOperationPref.Add(id, _openAllocationOperationPref[id].Clone());
                                _allocationOperationPref[id] = _openAllocationOperationPref[id].Clone();
                            }
                            lock (_allocationOperationPref)
                            {
                                _openAllocationOperationPref.Clear();
                                _openAllocationOperationPref.Add(selectedId, pref);
                                isPrefernceSaved = SavePreference(false);
                                defaultRuleControl1.UpdateGrid(_allocationOperationPref[selectedId].DefaultRule);
                                generalRuleControl1.AddRowsToGrid(_allocationOperationPref[selectedId].CheckListWisePreference);
                                generalRuleControl1.UpdateDefaultRule(_allocationOperationPref[selectedId].DefaultRule.Clone());
                                    accountAllocationControl1.UpdateGrid(_allocationOperationPref[selectedId].TargetPercentage);
                                //Commented so that invalid preferences are not cleared, PRANA-12381
                                //_openAllocationOperationPref.Clear();
                                openPrefId = -1;
                            }
                            return isPrefernceSaved;
                        }
                        else if (dr == DialogResult.Yes)
                        {
                            lock (_allocationOperationPref)
                            {
                                _openAllocationOperationPref[selectedId] = pref;
                                SavePreference(true);
                                defaultRuleControl1.UpdateGrid(_allocationOperationPref[selectedId].DefaultRule);
                                generalRuleControl1.AddRowsToGrid(_allocationOperationPref[selectedId].CheckListWisePreference);
                                generalRuleControl1.UpdateDefaultRule(_allocationOperationPref[selectedId].DefaultRule.Clone());
                                    accountAllocationControl1.UpdateGrid(_allocationOperationPref[selectedId].TargetPercentage);
                                //_openAllocationOperationPref.Clear();
                                openPrefId = -1;
                            }
                            return true;
                        }
                        else
                            return false;

                        //return false;
                    }
                }
                }
                return true;
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
                return false;
            }
        }

        /// <summary>
        /// Saves preferences through IAllocationService
        /// </summary>
        /// <returns></returns>
        private bool SavePreference(bool saveAll)
        {
            try
            {
                //Added to add invalid preferences to _openAllocationOperationPref, PRANA-12381
                if (saveAll)
                {
                    foreach (int id in _invalidAllocationOperationPref.Keys)
                    {
                        if (!_openAllocationOperationPref.ContainsKey(id) && _allocationOperationPref.ContainsKey(id))
                            _openAllocationOperationPref.Add(id, _invalidAllocationOperationPref[id].Clone());
                    }
                }
                List<int> listToSave = new List<int>(this._openAllocationOperationPref.Keys);
                //Variable added to store invalid open preferences, PRANA-12381
                Dictionary<int, AllocationOperationPreference> openAllocationOperationPref = new Dictionary<int, AllocationOperationPreference>();
                string errorMessage;
                foreach (int id in _openAllocationOperationPref.Keys)
                {
                    if (id != -1)
                    {
                        if (_openAllocationOperationPref[id].IsValid(out errorMessage))
                        {
                            PreferenceUpdateResult result = AllocationManager.GetInstance().Allocation.InnerChannel.UpdatePreference(_openAllocationOperationPref[id]);
                            if (result != null)
                            {
                                if (result.Error == null)
                                {
                                    _allocationOperationPref[result.Preference.OperationPreferenceId] = result.Preference;
                                    listToSave.Remove(result.Preference.OperationPreferenceId);
                                }
                                else
                                {
                                    MessageBox.Show(this, result.Error, "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    listToSave.Remove(id);
                                }
                            }
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show(this, "Preference " + _openAllocationOperationPref[id].OperationPreferenceName + " is not valid." + errorMessage + "\n So, can not save preference. Do you want to revert your changes?", "Nirvana Preferences", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr.Equals(DialogResult.Yes))
                            {
                                if (_initialAllocationOperationPref.ContainsKey(id))
                                _allocationOperationPref[id] = _initialAllocationOperationPref[id].Clone();
                                if (_invalidAllocationOperationPref.ContainsKey(id) && _allocationOperationPref[id].IsValid())
                                    _invalidAllocationOperationPref.Remove(id);
                                listToSave.Remove(id);
                            }
                            else if (dr.Equals(DialogResult.No))
                            {                               
                                _allocationOperationPref[id] = _openAllocationOperationPref[id].Clone();
                                openAllocationOperationPref.Add(id, _openAllocationOperationPref[id].Clone());
                                if (!_invalidAllocationOperationPref.ContainsKey(id))
                                    _invalidAllocationOperationPref.Add(id, _openAllocationOperationPref[id].Clone());
                                listToSave.Remove(id);
                                MessageBox.Show(this, "Changes not saved for preference " + _openAllocationOperationPref[id].OperationPreferenceName + ". So, changes will be lost if you reopen UI.", "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (!saveAll)
                                {
                                    _openAllocationOperationPref = openAllocationOperationPref;
                                    return false;
                            }
                        }
                    }
                    }
                    else
                        listToSave.Remove(id);

                    if (listToSave.Count <= 0)
                    {
                        _openAllocationOperationPref = openAllocationOperationPref;
                        return true;
                    }

                }
                return false;
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
                return false;
            }
        }

        public void RestoreDefault()
        {
            
        }

        public Prana.BusinessObjects.IPreferenceData GetPrefs()
        {
            return null;
        }

        //public event EventHandler SaveClicked;

        public string SetModuleActive
        {
            set {  }
        }

        /// <summary>
        /// Delete invalid new added preference 
        /// </summary>
        /// <returns>false if user want to delete, true otherwise</returns>
        public bool RemoveInvalidNewPreferences()
        {
            try
            {
                List<string> newPreferences = preferenceSchemeListControl1.GetNewPreferenceList();
                List<string> preferenceNameList = new List<string>();
                List<int> removePreferenceIds = new List<int>();
                string errorMessage;
                foreach (int key in _allocationOperationPref.Keys)
                {
                    if (newPreferences.Contains(key.ToString()))
                    {
                        if (key != -1)
                        {
                            if (!_allocationOperationPref[key].IsValid(out errorMessage))
                            {
                                preferenceNameList.Add(_allocationOperationPref[key].OperationPreferenceName);
                                removePreferenceIds.Add(key);
                            }
                        }
                    }
                }
                if (preferenceNameList != null && preferenceNameList.Count > 0)
                {
                    DialogResult dr = MessageBox.Show(this, "The preference(s) " + string.Join(",", preferenceNameList.ToArray()) + " are invalid. Do you want to delete the new preference(s)?", "Nirvana Preferences", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr.Equals(DialogResult.Yes))
                    {
                        foreach (int key in removePreferenceIds)
                        {
                            AllocationManager.GetInstance().Allocation.InnerChannel.DeletePreference(key);
                            preferenceSchemeListControl1.RemoveItem(key);
                        }
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        #endregion

        /// <summary>
        /// update _isAllocationPrefTabSelected that current tab is selected or not
        /// </summary>
        /// <param name="value"></param>
        internal void UpdateIsCurrentTabSelcted(bool value)
        {
            try
            {
                _isAllocationPrefTabSelected = value;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update _initialAllocationOperationPref dictionary, PRANA-12381
        /// </summary>
        private void UpdateInitialPreferences()
        {
            try
            {
                foreach (int key in _allocationOperationPref.Keys)
                {
                    AllocationOperationPreference pref = new AllocationOperationPreference();
                    pref = _allocationOperationPref[key].Clone();
                    if (!_initialAllocationOperationPref.ContainsKey(key) && pref.IsValid())
                        _initialAllocationOperationPref.Add(key, pref);
                    else if (_initialAllocationOperationPref.ContainsKey(key) && pref.IsValid())
                        _initialAllocationOperationPref[key] = _allocationOperationPref[key].Clone();
    }
}
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Check if preferences are updated or not, PRANA-12383
        /// </summary>
        /// <returns></returns>
        private bool IsAllocationPreferenceUpdated()
        {
            try
            {
                AllocationOperationPreference pref=null;
                foreach (int key in _openAllocationOperationPref.Keys)
                {
                    int selectedId = preferenceSchemeListControl1.GetSelectedItemId();
                    AllocationOperationPreference openPref=_openAllocationOperationPref[key];
                    if (selectedId == key)
                    {
                        openPref.TryUpdateTargetPercentage(accountAllocationControl1.GetCurrentValues());
                        openPref.TryUpdateDefaultRule(defaultRuleControl1.GetCurrentValues());
                        SerializableDictionary<int, CheckListWisePreference> dict = generalRuleControl1.GetCurrentCheckListPref();
                        if (dict != null)
                            openPref.TryUpdateCheckList(dict);
    }
                    if (_initialAllocationOperationPref.TryGetValue(key, out pref))
                    {
                        if (!pref.Equals(openPref))
                            return true;
}
                    if (pref == null)
                        return true;
                }
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

    }
}
