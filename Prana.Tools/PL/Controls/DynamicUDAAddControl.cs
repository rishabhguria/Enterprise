using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Tools.PL.Controls
{
    public partial class DynamicUDAAddControl : UserControl
    {
        /// <summary>
        /// To Save Dynamic UDA
        /// </summary>
        public event EventHandler<EventArgs<DynamicUDA, string>> SaveDynamicUDA;

        /// <summary>
        /// To check master value assigned or not
        /// </summary>
        public event EventHandler<EventArgs<string, string>> CheckMasterValueAssigned;

        /// <summary>
        /// List to save renamed keys
        /// </summary>
        private List<string> _renamedKeys = new List<string>();

        /// <summary>
        /// Variable that contains true/false, if update operaion then true otherwise false
        /// </summary>
        private bool _isUpdate = false;

        /// <summary>
        /// List of existing colums of other UIs, e.g. symbollookup
        /// </summary>
        private List<string> _otherExistingColumns;

        /// <summary>
        /// Datatable that stores the master values
        /// </summary>
        DataTable _dt = new DataTable();

        /// <summary>
        /// Undefined String constants
        /// </summary>
        string _undefined = "Undefined";
        string _undefinedHyphen = "- Undefined -";

        /// <summary>
        /// Default Constructor for Dynamic UDA Add Control
        /// </summary>
        public DynamicUDAAddControl(Dictionary<string, string> existinCache, List<string> otherExistingColumns)
        {
            try
            {
                InitializeComponent();
                _existingCache = existinCache;
                _otherExistingColumns = otherExistingColumns;
                _dt.Columns.Add("MasterValues", typeof(String));
                BindDataToUltraComboEditor();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Constructor to bind the data on UI
        /// </summary>
        /// <param name="dynamicUDA">Dynamic UDA</param>
        /// <param name="existingCache">Cache of dynamic UDA names</param>
        /// <param name="otherExistingColumns">List of symbol lookup column names</param>
        /// <param name="isUpdate"></param>
        public DynamicUDAAddControl(DynamicUDA dynamicUDA, Dictionary<string, string> existingCache, List<string> otherExistingColumns)
        {
            try
            {
                InitializeComponent();
                _existingCache = existingCache;
                _otherExistingColumns = otherExistingColumns;
                _isUpdate = true;
                ultraTextEditorDefaultValue.Text = dynamicUDA.DefaultValue;
                ultraTextEditorHeaderCaption.Tag = dynamicUDA.Tag;
                ultraTextEditorHeaderCaption.Text = dynamicUDA.HeaderCaption;

                _dt.Columns.Add("MasterValues", typeof(String));

                foreach (var masterValues in dynamicUDA.MasterValues)
                {
                    _dt.Rows.Add(masterValues.Value);
                    UltraListViewItem item = new UltraListViewItem();
                    item.Key = masterValues.Key;
                    item.Value = masterValues.Value;
                    ultraListViewDynamicUDA.Items.Add(item);
                    ultraListViewDynamicUDA.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                }
                ultraComEdDefaultValue.Text = dynamicUDA.DefaultValue;
                BindDataToUltraComboEditor();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Bind Data to Combo Editor
        /// </summary>
        private void BindDataToUltraComboEditor()
        {
            try
            {
                _dt.Rows.Add(_undefinedHyphen);
                EnableDisableComboEditor(false);

                ultraComEdDefaultValue.DataSource = _dt;

                ultraComEdDefaultValue.DisplayMember = "Value";
                ultraComEdDefaultValue.ValueMember = "Key";

                if (ultraTextEditorDefaultValue.Text.Equals(_undefined))
                    ultraTextEditorDefaultValue.Text = _undefinedHyphen;
                if (ultraComEdDefaultValue.Text.Equals(_undefined))
                    ultraComEdDefaultValue.Text = _undefinedHyphen;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Context Menu Dynamic UDA Item Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMnuDynamicUDA_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string item = e.ClickedItem.Tag.ToString();
                cntxtMnuDynamicUDA.Visible = false;
                switch (item)
                {
                    case "AddMasterValue":
                        ultraTextEditorDefaultValue.Enabled = false;
                        ultraTextEditorHeaderCaption.Enabled = false;
                        ultraComEdDefaultValue.Enabled = false;
                        AddMasterValue();
                        break;

                    case "DeleteMasterValue":
                        DeleteMasterValue();
                        break;

                    case "RenameMasterValue":
                        ultraTextEditorDefaultValue.Enabled = false;
                        ultraTextEditorHeaderCaption.Enabled = false;
                        ultraComEdDefaultValue.Enabled = false;
                        RenameMasterValue();
                        break;
                    case "MarksAsDefault":
                        MarksDefaultValue();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Marks As Default Value from Ultra List View Selected Master Value when Clicked on Marks As Default Vale Context Menu Item
        /// </summary>
        private void MarksDefaultValue()
        {
            try
            {
                UltraListViewItem item = ultraListViewDynamicUDA.ActiveItem;
                if (item != null)
                {
                    ultraTextEditorDefaultValue.Text = item.Value.ToString();
                    ultraTextEditorDefaultValue.Value = item.Value;
                    ultraComEdDefaultValue.Text = item.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Rename The Master Value of Dynamic UDA when Rename Context Menu Cliecked
        /// </summary>
        private void RenameMasterValue()
        {
            try
            {
                ultraListViewDynamicUDA.SelectedItems.Clear();
                UltraListViewItem item = ultraListViewDynamicUDA.ActiveItem;
                cntxtMnuDynamicUDA.Enabled = false;
                if (item != null)
                {
                    item.Tag = "RENAME";
                    ultraListViewDynamicUDA.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                    item.BeginEdit();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Contains the deleted item
        /// </summary>
        UltraListViewItem _deleteItem;

        /// <summary>
        /// Delete Master Value of Dynamic UDA when Delete Context Menu Item Clicked
        /// </summary>
        private void DeleteMasterValue()
        {
            try
            {
                _deleteItem = ultraListViewDynamicUDA.ActiveItem;
                if (_deleteItem != null)
                {
                    DialogResult dr = MessageBox.Show(this, "Do you want to delete  " + _deleteItem.Text + "?", "Nirvana", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        if (CheckMasterValueAssigned != null)
                            CheckMasterValueAssigned(this, new EventArgs<string, string>(ultraTextEditorHeaderCaption.Tag.ToString(), _deleteItem.Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To identify added and modified master values
        /// </summary>
        List<string> _addItems = new List<string>();

        /// <summary>
        /// Add Master Value of Dynamic UDA when Add Context Menu Item Clicked
        /// </summary>
        private void AddMasterValue()
        {
            try
            {
                ultraListViewDynamicUDA.SelectedItems.Clear();
                UltraListViewItem item = new UltraListViewItem();
                item.Key = GetHighestKey().ToString();
                item.Tag = "ADD";
                ultraListViewDynamicUDA.Items.Add(item);
                cntxtMnuDynamicUDA.Enabled = false;
                ultraListViewDynamicUDA.ItemSettings.ActiveAppearance.ForeColor = Color.Black;
                item.BeginEdit();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Getting Highest Key of Dictionary
        /// </summary>
        /// <returns>key Value + 1 </returns>
        private int GetHighestKey()
        {
            try
            {
                int max = 0;
                foreach (UltraListViewItem item in ultraListViewDynamicUDA.Items.All)
                {
                    if (max < Convert.ToInt32(item.Key))
                        max = Convert.ToInt32(item.Key);
                }
                return max + 1;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ultra List View Layout Event to Set the Layout related properties
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">LayoutEventArgs</param>
        private void ultraListViewDynamicUDA_Layout(object sender, LayoutEventArgs e)
        {
            try
            {
                ultraListViewDynamicUDA.ViewSettingsList.ImageSize = Size.Empty;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ultra List View Mouse Up Event to Check which menu item have to visible, If row selected then Update,Rename and Add, Otherwise Add Only context menu item will display 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        private void ultraListViewDynamicUDA_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = ultraListViewDynamicUDA.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    UltraListViewItem item = element.GetContext(typeof(UltraListViewItem)) as UltraListViewItem;

                    if (item == null && e.Button == MouseButtons.Right)
                    {
                        ultraListViewDynamicUDA.SelectedItems.Clear();
                        cntxtMnuDynamicUDA.Visible = true;
                        addMasterValueToolStripMenuItem.Visible = true;
                        deleteMasterValueToolStripMenuItem.Visible = false;
                        renameMasterValueToolStripMenuItem.Visible = false;
                        toolStripMenuItemMarksAsDefault.Visible = false;
                    }
                    else if (item != null && e.Button == MouseButtons.Right)
                    {
                        item.Activate();
                        ultraListViewDynamicUDA.SelectedItems.Clear();
                        cntxtMnuDynamicUDA.Visible = true;
                        addMasterValueToolStripMenuItem.Visible = true;
                        deleteMasterValueToolStripMenuItem.Visible = true;
                        renameMasterValueToolStripMenuItem.Visible = true;
                        toolStripMenuItemMarksAsDefault.Visible = true;
                        ultraListViewDynamicUDA.SelectedItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save the Dynamic UDA When Button Save is Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDynamicUDASave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = IsValid();
                if (isValid)
                {
                    DynamicUDA dynamicUda = null;
                    SerializableDictionary<string, string> dicMasterValues = new SerializableDictionary<string, string>();
                    foreach (UltraListViewItem item in ultraListViewDynamicUDA.Items.All)
                    {
                        dicMasterValues.Add(item.Key, item.Value.ToString());
                    }
                    if (ultraComEdDefaultValue.Text.Equals(_undefinedHyphen))
                        ultraComEdDefaultValue.Text = _undefined;

                    if (ultraTextEditorDefaultValue.Text.Equals(_undefinedHyphen))
                        ultraTextEditorDefaultValue.Text = _undefined;

                    if (String.IsNullOrWhiteSpace(ultraComEdDefaultValue.Text))
                        ultraComEdDefaultValue.Text = _undefined;

                    if (String.IsNullOrWhiteSpace(ultraTextEditorDefaultValue.Text))
                        ultraTextEditorDefaultValue.Text = _undefined;

                    if (_isUpdate)
                        dynamicUda = new DynamicUDA(ultraTextEditorHeaderCaption.Tag.ToString().Trim(), ultraTextEditorHeaderCaption.Text.Trim(), ultraComEdDefaultValue.Visible ? ultraComEdDefaultValue.Text.Trim() : ultraTextEditorDefaultValue.Text.Trim(), dicMasterValues);
                    else
                        dynamicUda = new DynamicUDA(ultraTextEditorHeaderCaption.Text.Replace(" ", String.Empty), ultraTextEditorHeaderCaption.Text.Trim(), ultraComEdDefaultValue.Visible ? ultraComEdDefaultValue.Text.Trim() : ultraTextEditorDefaultValue.Text.Trim(), dicMasterValues);
                    //only renaming values which exists.
                    if (_existingCache.ContainsKey(dynamicUda.Tag.ToLower())
                        || _existingCache.ContainsValue(dynamicUda.HeaderCaption.ToLower())
                        )
                    {
                        statusProvider.SetError(ultraTextEditorHeaderCaption, "UDA already exists");
                    }
                    // checks if column with same name exists in symbol lookup
                    else if (!_isUpdate && (_otherExistingColumns.Contains(dynamicUda.Tag.ToLower()) || _otherExistingColumns.Contains(dynamicUda.HeaderCaption.ToLower())))
                    {
                        statusProvider.SetError(ultraTextEditorHeaderCaption, "Column with same name exists already or Name not allowed.");
                    }
                    else
                    {
                        string key = string.Join(",", _renamedKeys.Where(x => dicMasterValues.ContainsKey(x)).ToArray());
                        if (SaveDynamicUDA != null)
                            SaveDynamicUDA(this, new EventArgs<DynamicUDA, string>(dynamicUda, key));
                        this.FindForm().Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To check validation on Filled fields by the User
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            // Added regular expression validation for dynamic uda name and value input 
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-9369
            bool flag = false;
            bool headerCaptionFlag = false;
            bool defaultValueFlag = false;
            bool masterValuesFlag = true;
            try
            {
                if (ultraTextEditorHeaderCaption.Text.Count() > 0)
                {
                    if (Regex.IsMatch(ultraTextEditorHeaderCaption.Text, SecMasterConstants.CONST_DynamicUDANameRegex))
                    {
                        statusProvider.SetError(ultraTextEditorHeaderCaption, "");
                        headerCaptionFlag = true;
                    }
                    else
                    {
                        statusProvider.SetError(ultraTextEditorHeaderCaption, "Header caption must start with alphabet or '_' and only conatins alphanumeric, '_' and '-'");
                        headerCaptionFlag = false;
                    }
                }
                else
                {
                    statusProvider.SetError(ultraTextEditorHeaderCaption, "Please check Header Caption, It is Blank");
                    headerCaptionFlag = false;
                }

                if (Regex.IsMatch(ultraTextEditorDefaultValue.Text, SecMasterConstants.CONST_DynamicUDAValueRegex))
                {
                    if (ultraListViewDynamicUDA.Items.Count > 0)
                    {
                        if ((ultraComEdDefaultValue.Visible ? !String.IsNullOrEmpty(ultraComEdDefaultValue.Text) : !String.IsNullOrEmpty(ultraTextEditorDefaultValue.Text)) && !ultraComEdDefaultValue.Text.Equals(_undefinedHyphen))
                        {
                            foreach (UltraListViewItem item in ultraListViewDynamicUDA.Items.All)
                            {
                                if (ultraComEdDefaultValue.Visible ? item.Text.Equals(ultraComEdDefaultValue.Text) : item.Text.Equals(ultraTextEditorDefaultValue.Text))
                                {
                                    statusProvider.SetError(ultraTextEditorDefaultValue, "");
                                    statusProvider.SetError(ultraComEdDefaultValue, "");
                                    defaultValueFlag = true;
                                    break;
                                }
                                else
                                {
                                    statusProvider.SetError(ultraTextEditorDefaultValue, "Please check Default Value It Should be Exist in Master Value");
                                    statusProvider.SetError(ultraComEdDefaultValue, "Please check Default Value It Should be Exist in Master Value");
                                    defaultValueFlag = false;
                                }
                            }

                        }
                        else
                            defaultValueFlag = true;
                    }
                    else
                        defaultValueFlag = true;
                }
                else
                {
                    statusProvider.SetError(ultraTextEditorDefaultValue, "Default value should contain only alphanumeric and special characters. \nAllowed special characters are: &/:@*!^%-$_#~(=)`{}\"[];'><?| ");
                    defaultValueFlag = false;
                }

                if (ultraListViewDynamicUDA.Items.Count > 0)
                {
                    foreach (UltraListViewItem item in ultraListViewDynamicUDA.Items.All)
                    {
                        if (!Regex.IsMatch(item.Text, SecMasterConstants.CONST_DynamicUDAValueRegex))
                        {
                            item.Activate();
                            statusProvider.SetError(ultraListViewDynamicUDA, "Master value should contain only alphanumeric and special characters. \nAllowed special characters are: &/:@*!^%-$_#~(=)`{}\"[];'><?| ");
                            masterValuesFlag = false;
                            break;
                        }
                    }
                }

                if (headerCaptionFlag && defaultValueFlag && masterValuesFlag)
                    flag = true;
                else
                    flag = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Ultra List View Item Exited Edit Mode To Check some validation on List View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraListViewDynamicUDA_ItemExitedEditMode(object sender, ItemExitedEditModeEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(e.Item.Value.ToString()))
                    e.Item.Value = e.Item.Value.ToString().Trim();
                if (e.Item.Tag.ToString() == "RENAME")
                {
                    bool flag = false;
                    String defValue = ultraComEdDefaultValue.Text;
                    _dt.Clear();
                    foreach (UltraListViewItem val in ultraListViewDynamicUDA.Items.All)
                    {
                        if (val.Value.Equals(defValue))
                            flag = true;
                        if (!String.IsNullOrEmpty(val.Text.Trim()))
                            _dt.Rows.Add(val.Text);
                    }
                    _dt.Rows.Add(_undefinedHyphen);

                    if (!flag && !defValue.Equals(_undefinedHyphen))
                        ultraComEdDefaultValue.Text = e.Item.Value.ToString();

                    if (String.IsNullOrEmpty(ultraComEdDefaultValue.Text.Trim()))
                        ultraComEdDefaultValue.Text = _undefinedHyphen;
                }

                //if item is added and then renamed before saving then it should be treted as added not renameS
                if (!_addItems.Contains(e.Item.Key) && e.Item.Tag.ToString() == "RENAME")
                    _renamedKeys.Add(e.Item.Key);
                else if (e.Item.Tag.ToString() == "ADD")
                {
                    if (ultraTextEditorDefaultValue.Visible)
                    {
                        DialogResult dr = new DialogResult();
                        if (!ultraTextEditorDefaultValue.Text.Equals(_undefinedHyphen) && !String.IsNullOrEmpty(ultraTextEditorDefaultValue.Text.Trim()))
                        {
                            dr = MessageBox.Show(this, "If you add new item, you will lost the current default value " + ultraTextEditorDefaultValue.Text + ". Do you want to continue?", "Nirvana", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }

                        if (dr.Equals(DialogResult.No))
                            ultraListViewDynamicUDA.Items.Remove(e.Item);
                        else
                            ultraTextEditorDefaultValue.Text = _undefinedHyphen;
                    }

                    if (e.Item.Text.Equals(_undefinedHyphen) || e.Item.Text.Equals(_undefined) || e.Item.Text.Equals("- undefined -") || e.Item.Text.Equals("undefined"))
                    {
                        ultraListViewDynamicUDA.Items.Remove(e.Item);
                        MessageBox.Show(this, "Can not add Undefined or - Undefined  - as a Master value", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _addItems.Add(e.Item.Key);
                        if (ultraListViewDynamicUDA.Items.Count > 0 && !String.IsNullOrEmpty(e.Item.Text.Trim()))
                            _dt.Rows.Add(e.Item.Text);
                    }
                }
                cntxtMnuDynamicUDA.Enabled = true;
                ultraTextEditorDefaultValue.Enabled = true;
                ultraTextEditorHeaderCaption.Enabled = true;
                ultraComEdDefaultValue.Enabled = true;
                EnableDisableComboEditor(false);
                SetSortingOrderForList(true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enable/ Disable the ComboEditor and Text Field
        /// </summary>
        private void EnableDisableComboEditor(bool val)
        {
            try
            {
                if (val)
                    ultraTextEditorDefaultValue.Text = _undefinedHyphen;

                if (ultraListViewDynamicUDA.Items.Count > 0)
                {
                    ultraComEdDefaultValue.Visible = true;
                    ultraTextEditorDefaultValue.Visible = false;
                }
                else
                {
                    ultraComEdDefaultValue.Text = _undefinedHyphen;
                    ultraComEdDefaultValue.Visible = false;
                    ultraTextEditorDefaultValue.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To Close the form without saving the Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDynamicUDACancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.FindForm().Close();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Check item before exiting edit mode
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event</param>
        private void ultraListViewDynamicUDA_ItemExitingEditMode(object sender, ItemExitingEditModeEventArgs e)
        {
            try
            {
                //added to close form when cancel is clicked, PRANA-11259
                if (btnDynamicUDACancel.ContainsFocus)
                {
                    this.FindForm().Close();
                    return;
                }
                SetSortingOrderForList(false);
                if (e.ApplyChanges)
                {
                    bool flag = false;
                    if (string.IsNullOrWhiteSpace(e.Editor.CurrentEditText.Trim()))
                    {
                        MessageBox.Show(this, "Dynamic UDA MasterValue cannot be blank.", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        flag = true;
                    }
                    if (!flag)
                    {
                        foreach (var masterValueItems in ultraListViewDynamicUDA.Items)
                        {
                            if (masterValueItems.Text.ToLower().Equals(e.Editor.CurrentEditText.ToLower()) && !(masterValueItems.Key.Equals(e.Item.Key)))
                            {
                                MessageBox.Show(this, "Dynamic UDA MasterValue " + e.Editor.CurrentEditText + " already exists. Please add another value.", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        if (e.Item.Tag.ToString() == "RENAME")
                        {
                            e.Item.Value = ultraListViewDynamicUDA.Items[e.Item.Key].Text;
                            e.Editor.Value = e.Item.Value;
                            e.Cancel = true;
                            cntxtMnuDynamicUDA.Enabled = false;
                        }
                        else if (e.Item.Tag.ToString() == "ADD")
                        {
                            //if (ultraListViewDynamicUDA.Items.Exists(e.Item.Key))
                            //   ultraListViewDynamicUDA.Items.Remove(e.Item);

                            //added item is not removed and text is changed to edit mode if it already exist in list, PRANA-9097
                            e.Item.Value = e.Editor.CurrentEditText;
                            e.Editor.Value = e.Item.Value;
                            e.Cancel = true;
                            cntxtMnuDynamicUDA.Enabled = true;
                            ultraTextEditorDefaultValue.Enabled = true;
                            ultraTextEditorHeaderCaption.Enabled = true;
                            ultraComEdDefaultValue.Enabled = true;
                        }
                    }
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

        /// <summary>
        /// Delete master value of user choice
        /// </summary>
        internal void DeleteListViewMasterValue(bool result)
        {
            try
            {
                //UltraListViewItem item = ultraListViewDynamicUDA.ActiveItem;
                if (result)
                {
                    MessageBox.Show(this, "Cannot delete as this master value is in use by some securities", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ultraListViewDynamicUDA.Items.Remove(_deleteItem);
                    if (ultraListViewDynamicUDA.Items.Count > 0)
                        ultraListViewDynamicUDA.Items[0].Activate();

                    //Delete the value from default value combo box
                    foreach (DataRow dr1 in _dt.Rows)
                    {
                        if (dr1["MasterValues"] == _deleteItem.Value)
                        {
                            _dt.Rows.Remove(dr1);
                            break;
                        }
                    }
                    if (_deleteItem.Value.Equals(ultraComEdDefaultValue.Text))
                        ultraComEdDefaultValue.Text = _undefinedHyphen;
                    EnableDisableComboEditor(true);
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

        /// <summary>
        /// Cache to match if exists
        /// </summary>
        private Dictionary<string, string> _existingCache;

        /// <summary>
        /// Added to set sorting in exitingEdit and exitedEdit mode, PRANA-11259
        /// </summary>
        /// <param name="isExistingMode"></param>
        private void SetSortingOrderForList(bool isExistedMode)
        {
            try
            {
                if (isExistedMode)
                    ultraListViewDynamicUDA.MainColumn.Sorting = Sorting.Ascending;
                else
                    ultraListViewDynamicUDA.MainColumn.Sorting = Sorting.None;
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
    }
}
