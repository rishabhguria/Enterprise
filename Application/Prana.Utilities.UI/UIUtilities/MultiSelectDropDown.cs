using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class MultiSelectDropDown : UserControl
    {
        public event EventHandler<ItemCheckEventArgs> CheckStateChanged;
        private bool _isUnsavedChanges = false;
        public MultiSelectDropDown()
        {
            InitializeComponent();
        }
        //return true if mouseclick event occurs on the user control
        public bool IsUnsavedChanges()
        {
            //modified by:sachin mishra Purpose:CHMW-2633
            if (_isUnsavedChanges)
            {
                _isUnsavedChanges = false;
                return true;
            }
            return false;

        }
        /// <summary>
        /// called when an item is checked on unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedMultipleItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (CheckStateChanged != null)
                {
                    CheckStateChanged(this, e);
                }

                //select deselect all the accounts on the basis of select all checkbox
                if (e.Index.Equals(0))
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    if (!e.NewValue.Equals(CheckState.Indeterminate))
                    {
                        for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                        {
                            checkedMultipleItems.SetItemCheckState(i, e.NewValue);
                        }
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue.Equals(CheckState.Unchecked))
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    checkedMultipleItems.SetItemCheckState(0, CheckState.Unchecked);
                    if (GetNoOfCheckedItems() == GetNoOfTotalItems())
                    {
                        checkedMultipleItems.SetItemCheckState(0, CheckState.Checked);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue.Equals(CheckState.Checked))
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    if (checkedMultipleItems.Items.Count == checkedMultipleItems.CheckedItems.Count + 2)
                    {
                        checkedMultipleItems.SetItemCheckState(0, CheckState.Checked);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void ClearAll()
        {
            try
            {
                checkedMultipleItems.Items.Clear();
                SetTitleText(GetNoOfCheckedItems());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddItemsToTheCheckList(Dictionary<int, string> dictItems, CheckState state)
        {
            try
            {
                //sort items in ascending order of value.
                //http://jira.nirvanasolutions.com:8080/browse/LAZARD-85
                List<KeyValuePair<int, string>> sortedList = SortDictionaryByValue(dictItems);
                //add select all option at top of checklist box
                KeyValuePair<int, string> itemSelectAll = new KeyValuePair<int, string>(int.MinValue, "Select All");
                this.checkedMultipleItems.Items.Clear();
                this.checkedMultipleItems.Items.Add(itemSelectAll, state);
                foreach (KeyValuePair<int, string> kvp in sortedList)
                {
                    checkedMultipleItems.Items.Add(kvp, state);
                }
                checkedMultipleItems.DisplayMember = "Value";
                checkedMultipleItems.ValueMember = "Key";
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

        public void AddItemsToTheCheckList(Dictionary<int, string> dictItems, CheckState state, Dictionary<int, string> dictSelectedAccounts)
        {
            try
            {
                //sort items in ascending order of value.
                //http://jira.nirvanasolutions.com:8080/browse/LAZARD-85
                List<KeyValuePair<int, string>> sortedList = SortDictionaryByValue(dictItems);
                //add select all option at top of checklist box
                KeyValuePair<int, string> itemSelectAll = new KeyValuePair<int, string>(int.MinValue, "Select All");
                checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                if (dictSelectedAccounts.Count == dictItems.Count)
                {
                    checkedMultipleItems.Items.Add(itemSelectAll, state);
                }
                else
                {
                    checkedMultipleItems.Items.Add(itemSelectAll, CheckState.Unchecked);
                }
                if (dictSelectedAccounts.Count != 0)
                {
                    if (dictSelectedAccounts.Count == dictItems.Count)
                    {
                        this.MultiSelectEditor.Text = "All " + _titleText + "(s) Selected";
                    }
                    else
                    {
                        this.MultiSelectEditor.Text = dictSelectedAccounts.Count + " " + _titleText + "(s) Selected";
                    }
                }
                else
                {
                    this.MultiSelectEditor.Text = "No " + _titleText + "(s) Selected";
                }

                foreach (KeyValuePair<int, string> kvp in sortedList)
                {
                    if (dictSelectedAccounts.ContainsKey(kvp.Key))
                    {
                        checkedMultipleItems.Items.Add(kvp, state);
                    }
                    else
                    {
                        checkedMultipleItems.Items.Add(kvp, CheckState.Unchecked);
                    }
                }
                checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                checkedMultipleItems.DisplayMember = "Value";
                checkedMultipleItems.ValueMember = "Key";
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

        public void AddItemsToTheCheckList(Dictionary<string, string> dictItems, CheckState state)
        {
            try
            {
                //add select all option at top of checklist box
                KeyValuePair<string, string> itemSelectAll = new KeyValuePair<string, string>(string.Empty, "Select All");
                this.checkedMultipleItems.Items.Add(itemSelectAll, state);
                foreach (KeyValuePair<string, string> kvp in dictItems)
                {
                    checkedMultipleItems.Items.Add(kvp, state);
                }
                checkedMultipleItems.DisplayMember = "Value";
                checkedMultipleItems.ValueMember = "Key";
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

        private static List<KeyValuePair<int, string>> SortDictionaryByValue(Dictionary<int, string> dictItems)
        {
            List<KeyValuePair<int, string>> sortedList = new List<KeyValuePair<int, string>>(dictItems);
            try
            {
                sortedList.Sort
                (
                    delegate (KeyValuePair<int, string> firstPair, KeyValuePair<int, string> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    }
                );
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
            return sortedList;
        }
        /// <summary>
        /// Adjust width of checklistbox based on the length of longest account name
        /// </summary>
        public void AdjustCheckListBoxWidth()
        {
            try
            {
                int LongestItemLength = 0;
                for (int i = 0; i < checkedMultipleItems.Items.Count; i++)
                {
                    Graphics g = checkedMultipleItems.CreateGraphics();
                    //get width of checklist value item
                    int tempLength = Convert.ToInt32((g.MeasureString(checkedMultipleItems.Items[i].ToString(), this.checkedMultipleItems.Font)).Width);
                    if (tempLength > LongestItemLength)
                    {
                        LongestItemLength = tempLength;
                    }
                }
                if (LongestItemLength > 0)
                {
                    checkedMultipleItems.Width = LongestItemLength;
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
        }
        public void SetTextEditorText(string newText)
        {
            try
            {
                MultiSelectEditor.Text = newText;
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

        private string _titleText = string.Empty;
        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value; }
        }

        public CheckedListBox CheckedMultipleItems
        {
            get { return checkedMultipleItems; }
        }

        public void SetTitleText(int selectedCount)
        {
            try
            {
                if (selectedCount != 0)
                {
                    if (selectedCount == GetNoOfTotalItems())
                    {
                        MultiSelectEditor.Text = "All " + _titleText + "(s) Selected";
                    }
                    else
                    {
                        MultiSelectEditor.Text = selectedCount + " " + _titleText + "(s) Selected";
                    }
                }
                else
                {
                    MultiSelectEditor.Text = "No " + _titleText + "(s) Selected";
                }
                SetAccessibleDescription();
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

        public int GetNoOfTotalItems()
        {
            try
            {
                return checkedMultipleItems.Items.Count;
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
            return 0;
        }
        public int GetNoOfCheckedItems()
        {
            try
            {
                return checkedMultipleItems.CheckedItems.Count;
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
            return 0;
        }

        string _commaSeparatedAccountIds = string.Empty;
        public string GetCommaSeperatedAccountIds()
        {
            string CommaSeparatedAccountIds = string.Empty;
            try
            {
                if (InvokeRequired)
                {
                    MethodInvoker del =
                        delegate
                        {
                            GetCommaSeperatedAccountIds();
                        };
                    this.Invoke(del);
                }
                else
                {
                    int noOfSelectedItems = checkedMultipleItems.CheckedItems.Count;
                    int noOfTotalItems = checkedMultipleItems.Items.Count;
                    if (noOfSelectedItems != 0)
                    {
                        for (int i = 0, count = noOfSelectedItems; i < count; i++)
                        {
                            //item of check list box is of KeyValuePair
                            KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.CheckedItems[i]);
                            //get accountid from KeyValuePair
                            int accountId = pair.Key;
                            if (!accountId.Equals(int.MinValue))
                            {
                                //add comma seperated accountid's
                                CommaSeparatedAccountIds = CommaSeparatedAccountIds + accountId.ToString() + Seperators.SEPERATOR_8;
                            }
                            else
                            {
                                noOfSelectedItems--;
                                noOfTotalItems--;
                            }
                        }
                        if (noOfSelectedItems.Equals(noOfTotalItems))
                        {
                            SetTextEditorText("All Account(s) Selected");
                        }
                        else if (noOfSelectedItems < noOfTotalItems)
                        {
                            SetTextEditorText(noOfSelectedItems + " Account(s) Selected");
                        }
                    }
                    else
                    {
                        SetTextEditorText("No Account(s) Selected");
                    }
                    _commaSeparatedAccountIds = CommaSeparatedAccountIds;
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
            return _commaSeparatedAccountIds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DropDownEditorButton1_AfterCloseUp(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            Control control = GetChildAtPoint(this.PointToClient(Control.MousePosition));
            if (control != null)
            {
                Infragistics.Win.Misc.UltraButton button = control as Infragistics.Win.Misc.UltraButton;
                if (button != null)
                {
                    button.PerformClick();
                }
                else
                {
                    control.Focus();
                }
            }
            SetTitleText(GetNoOfCheckedItems());
        }

        public Dictionary<int, string> GetSelectedItemsInDictionary()
        {
            Dictionary<int, string> selectedItemsList = new Dictionary<int, string>();
            try
            {
                int noOfSelectedItems = checkedMultipleItems.CheckedItems.Count;
                if (noOfSelectedItems > 0)
                {
                    for (int i = 0, count = noOfSelectedItems; i < count; i++)
                    {
                        //item of check list box is of KeyValuePair
                        KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.CheckedItems[i]);
                        int key = pair.Key;
                        if (!key.Equals(int.MinValue) && !selectedItemsList.ContainsKey(pair.Key))
                        {
                            selectedItemsList.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            noOfSelectedItems--;
                        }
                    }
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
            return selectedItemsList;
        }

        public Dictionary<int, string> GetAllItemsInDictionary()
        {
            Dictionary<int, string> selectedItemsList = new Dictionary<int, string>();
            try
            {
                int noOfSelectedItems = checkedMultipleItems.Items.Count;
                if (noOfSelectedItems > 0)
                {
                    for (int i = 0, count = noOfSelectedItems; i < count; i++)
                    {
                        //item of check list box is of KeyValuePair
                        KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.Items[i]);
                        int key = pair.Key;
                        if (!key.Equals(int.MinValue) && !selectedItemsList.ContainsKey(pair.Key))
                        {
                            selectedItemsList.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            noOfSelectedItems--;
                        }
                    }
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
            return selectedItemsList;
        }

        public Dictionary<string, string> GetSelectedItemsInDictionaryinString()
        {
            Dictionary<string, string> selectedItemsList = new Dictionary<string, string>();
            try
            {
                int noOfSelectedItems = checkedMultipleItems.CheckedItems.Count;
                if (noOfSelectedItems > 0)
                {
                    for (int i = 0, count = noOfSelectedItems; i < count; i++)
                    {

                        KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(checkedMultipleItems.CheckedItems[i]);
                        string key = pair.Key;
                        if (!key.Equals(string.Empty) && !selectedItemsList.ContainsKey(pair.Key))
                        {
                            selectedItemsList.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            noOfSelectedItems--;
                        }
                    }
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
            return selectedItemsList;
        }

        public Dictionary<string, string> GetAllItemsInDictionaryinString()
        {
            Dictionary<string, string> alltemsList = new Dictionary<string, string>();
            try
            {
                int noOfItems = checkedMultipleItems.Items.Count;
                if (noOfItems > 0)
                {
                    for (int i = 0, count = noOfItems; i < count; i++)
                    {

                        KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(checkedMultipleItems.Items[i]);
                        string key = pair.Key;
                        if (!key.Equals(string.Empty) && !alltemsList.ContainsKey(pair.Key))
                        {
                            alltemsList.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            noOfItems--;
                        }
                    }
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
            return alltemsList;
        }


        public void SelectUnselectAll(CheckState checkState)
        {
            try
            {
                if (checkState == CheckState.Checked)
                {
                    checkedMultipleItems.SetItemChecked(0, true);
                }
                else
                {
                    checkedMultipleItems.SetItemChecked(0, true);
                    checkedMultipleItems.SetItemChecked(0, false);
                }
                SetTitleText(GetNoOfCheckedItems());
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

        public void SelectUnselectItems(Dictionary<int, string> allowedAssets, CheckState checkState)
        {
            try
            {
                int noOfTotalItems = checkedMultipleItems.Items.Count;
                if (noOfTotalItems > 0)
                {
                    for (int i = 0, count = noOfTotalItems; i < count; i++)
                    {
                        //item of check list box is of KeyValuePair
                        KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.Items[i]);
                        if (!pair.Key.Equals(int.MinValue))
                        {
                            if (allowedAssets.ContainsKey(pair.Key))
                            {
                                if (checkState == CheckState.Checked)
                                {
                                    checkedMultipleItems.SetItemChecked(i, true);
                                }
                                else
                                {
                                    checkedMultipleItems.SetItemChecked(i, false);
                                }
                            }
                            else
                            {
                                if (checkState == CheckState.Checked)
                                {
                                    checkedMultipleItems.SetItemChecked(i, false);
                                }
                            }
                        }
                        else
                        {
                            noOfTotalItems--;
                        }
                    }
                    SetTitleText(GetNoOfCheckedItems());
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
        }
        //to check if changes are made on the user control
        private void checkedMultipleItems_Click(object sender, EventArgs e)
        {
            _isUnsavedChanges = true;
        }

        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1462
        // [Corporate Action] theme on some components in Corporate Action UI in CHMW (CH mode)
        // Temporary workaround to apply manual theme on this user control, please see work log on jira for details.
        public void SetManualTheme(Boolean isManualThemeRequired)
        {
            try
            {
                if (isManualThemeRequired)
                {
                    this.checkedMultipleItems.BackColor = System.Drawing.SystemColors.WindowText;
                    this.checkedMultipleItems.ForeColor = System.Drawing.Color.White;
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
        }

        public string GetCommaSeperatedIds()
        {
            string CommaSeparatedAccountIds = string.Empty;
            try
            {
                int noOfSelectedItems = checkedMultipleItems.CheckedItems.Count;
                int noOfTotalItems = checkedMultipleItems.Items.Count;
                if (noOfSelectedItems != 0)
                {
                    for (int i = 0, count = noOfSelectedItems; i < count; i++)
                    {
                        KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(checkedMultipleItems.CheckedItems[i]);
                        string auecid = pair.Key;
                        if (!auecid.Equals("") && auecid != null)
                        {
                            CommaSeparatedAccountIds = CommaSeparatedAccountIds + auecid.ToString() + Seperators.SEPERATOR_8;
                        }
                        else
                        {
                            noOfSelectedItems--;
                            noOfTotalItems--;
                        }
                    }
                    if (noOfSelectedItems.Equals(noOfTotalItems))
                    {
                        SetTextEditorText("All Items(s) Selected");
                    }
                    else if (noOfSelectedItems < noOfTotalItems)
                    {
                        SetTextEditorText(noOfSelectedItems + " Items(s) Selected");
                    }
                }
                else
                {
                    SetTextEditorText("No Items(s) Selected");
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
            return CommaSeparatedAccountIds;
        }
        /// <summary>
        /// to get comma seperated values.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetCommaSeperatedIDs(string itemName)
        {
            string CommaSeparatedItemIds = string.Empty;
            try
            {
                int noOfSelectedItems = checkedMultipleItems.CheckedItems.Count;
                int noOfTotalItems = checkedMultipleItems.Items.Count;
                if (noOfSelectedItems != 0)
                {
                    for (int i = 0, count = noOfSelectedItems; i < count; i++)
                    {
                        //item of check list box is of KeyValuePair
                        KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.CheckedItems[i]);
                        //get itemid from KeyValuePair
                        int itemId = pair.Key;
                        if (!itemId.Equals(int.MinValue) && !itemId.Equals("") && !String.IsNullOrEmpty(itemId.ToString()))
                        {
                            //add comma seperated itemID's
                            CommaSeparatedItemIds = CommaSeparatedItemIds + itemId.ToString() + Seperators.SEPERATOR_8;
                        }
                        else
                        {
                            noOfSelectedItems--;
                            noOfTotalItems--;
                        }
                    }
                    if (noOfSelectedItems.Equals(noOfTotalItems))
                    {
                        SetTextEditorText("All " + itemName + "(s) Selected");
                    }
                    else if (noOfSelectedItems < noOfTotalItems)
                    {
                        SetTextEditorText(noOfSelectedItems + " " + itemName + "(s) Selected");
                    }
                }
                else
                {
                    SetTextEditorText("No " + itemName + "(s) Selected");
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
            return CommaSeparatedItemIds;
        }

        #region http://jira.nirvanasolutions.com:8080/browse/PRANA-10554
        public void AssignNewCheckState(int index, CheckState checkState)
        {
            this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
            checkedMultipleItems.SetItemCheckState(index, checkState);
            if (checkState == CheckState.Indeterminate)
            {
                checkedMultipleItems.SetItemCheckState(0, CheckState.Unchecked);
            }
            else
            {
                if (checkedMultipleItems.Items.Count == checkedMultipleItems.CheckedItems.Count + 1 && checkedMultipleItems.GetItemCheckState(0) == CheckState.Unchecked)
                {
                    checkedMultipleItems.SetItemCheckState(0, CheckState.Checked);
                }
                else
                {
                    checkedMultipleItems.SetItemCheckState(0, CheckState.Unchecked);
                }
            }
            this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
            SetTitleText(GetNoOfCheckedItems());
        }

        public void SelectUnselectAllForMasterFund(CheckState checkState)
        {
            try
            {
                this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                checkedMultipleItems.SetItemCheckState(0, checkState);
                for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                {
                    checkedMultipleItems.SetItemCheckState(i, checkState);
                }
                this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                SetTitleText(GetNoOfCheckedItems());
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
        #endregion

        /// <summary>
        /// Gets the index of the key value from.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int GetKeyValueFromIndex(int index)
        {
            int key = -1;
            try
            {
                KeyValuePair<int, string> pair = (KeyValuePair<int, string>)(checkedMultipleItems.Items[index]);
                key = pair.Key;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return key;
        }

        // Sets AccessibleDescription property to a comma-separated list of checked items
        public void SetAccessibleDescription()
        {
            this.AccessibleDescription = string.Join(", ", GetSelectedItemsInDictionary().Values);
        }
    }
}
