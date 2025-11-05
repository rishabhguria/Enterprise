using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.RuleDefinition.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public partial class ImportRuleSelection : Form
    {
        private Dictionary<string, ImportDefinition> _importDefCache;
        private RulePackage rulePackage;
        private RuleCategory ruleCategory;


        #region Unused code

        ///// <summary>
        ///// Adds item(rules which can be imported) to the list 
        ///// </summary>
        ///// <param name="importDefCache"></param>
        //public ImportRuleSelection(ref Dictionary<string, ImportDefinition> importDefCache)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        this._importDefCache = importDefCache;
        //        foreach (String key in importDefCache.Keys)
        //        {
        //            ListViewItem item = new ListViewItem(key);
        //            item.Text = key;
        //            item.SubItems.AddRange(new String[] { importDefCache[key].OldRuleName, importDefCache[key].RuleName, importDefCache[key].Category.ToString(), importDefCache[key].Package.ToString() });
        //            //item.SubItems["RuleName"].Text = importDefCache[key].NewRuleName;
        //            //item.SubItems["Category"].Text = importDefCache[key].RuleCategory;
        //            //item.SubItems["Package"].Text = importDefCache[key].PackageName;
        //            ulstView.Items.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        #endregion

        /// <summary>
        /// Initializes components
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleCategory"></param>
        public ImportRuleSelection(RulePackage rulePackage, RuleCategory ruleCategory)
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    // TODO: Complete member initialization
                    this.rulePackage = rulePackage;
                    this.ruleCategory = ruleCategory;
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
        /// When clicked on OK remove unselected rules from the cache.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ulstView.CheckedItems.Count > 0)
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
                else
                {
                    this._importDefCache.Clear();
                    MessageBox.Show(this, "No rule to import.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Gets all rule from the path
        /// differentiate valid and invalid paths.
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                Dictionary<String, ImportDefinition> importDefCache = new Dictionary<string, ImportDefinition>();
                List<String> path = GetRuleDirectoryFromUser();

                if (path.Count == 0)
                {
                    this.Close();
                    return;
                }


                List<String> inValidPath = new List<string>();
                List<String> validPath = new List<string>();
                foreach (String rulePath in path)
                {
                    if (String.IsNullOrEmpty(rulePath) || !ValidatePath(rulePath))
                        inValidPath.Add(rulePath);
                    else
                        validPath.Add(rulePath);
                }

                List<string> newInvalidPath = new List<string>();
                foreach (String valPath in validPath)
                {
                    ImportDefinition def = GetImportDefinitionFromPath(valPath);
                    if (def != null && CheckValidImport(def, rulePackage, ruleCategory))
                    {
                        def.Package = rulePackage;
                        // Checking for the given rule in importdef and rulecache
                        if (importDefCache.ContainsKey(def.Package + "_" + def.RuleName))//|| RuleCache.GetInstance().IsRuleNameExists(def.RuleName, def.Package))
                            def.RuleName = GetnewRuleNameIfRuleExists(importDefCache, def.Package, def.RuleName);

                        importDefCache.Add(def.Package + "_" + def.RuleName, def);
                    }
                    else
                    {
                        newInvalidPath.Add(valPath);
                        inValidPath.Add(valPath);
                    }
                    //else
                }
                foreach (String newInpath in newInvalidPath)
                    validPath.Remove(newInpath);


                if (validPath.Count == 0)
                {
                    MessageBox.Show(this, "No valid rules found", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                //Asking if user wants to continue with partial selection
                if (inValidPath.Count > 0)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append(inValidPath.Count);
                    message.AppendLine(" rule path are invalid.");
                    message.AppendLine("Invalid rules are:");

                    int i = 0;
                    foreach (String invapath in inValidPath)
                    {
                        if (i > 10)
                        {
                            message.AppendLine("And some more");
                            break;
                        }
                        else
                        {
                            message.AppendLine(invapath);
                            i++;
                        }
                    }
                    message.AppendLine("\n\nDo you want to continue with valid rules?");
                    DialogResult dr = MessageBox.Show(this, message.ToString(), "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.No)
                    {
                        this.Close();
                        return;
                    }
                }
                this._importDefCache = importDefCache;
                foreach (String key in importDefCache.Keys)
                {
                    ListViewItem item = new ListViewItem(key);
                    item.Text = key;
                    item.SubItems.AddRange(new String[] { importDefCache[key].RuleName, importDefCache[key].OldRuleName, importDefCache[key].Category.ToString(), importDefCache[key].Package.ToString() });
                    ulstView.Items.Add(item);
                }
                if (importDefCache.Count <= 0)
                    MessageBox.Show(this, "No rule to import.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Checks if rule exists in import rule defition cache and rule cache.
        /// </summary>
        /// <param name="importDefCache"></param>
        /// <param name="packageName"></param>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        private string GetnewRuleNameIfRuleExists(Dictionary<string, ImportDefinition> importDefCache, RulePackage packageName, string ruleName)
        {
            try
            {
                if (importDefCache.ContainsKey(packageName + "_" + ruleName))
                {
                    String newRuleName = "";
                    bool newRuleNameFound = false;
                    while (!newRuleNameFound)
                    {
                        newRuleName = ruleName + "_" + _newRuleCounter++;
                        if (RuleCache.GetInstance().IsRuleNameExists(newRuleName, packageName) || importDefCache.ContainsKey(packageName + "_" + newRuleName))
                            newRuleNameFound = false;
                        else
                            newRuleNameFound = true;
                    }
                    return newRuleName;
                }
                else
                    return ruleName;
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
                return String.Empty;
            }
        }

        /// <summary>
        /// Checks fro valid and invalid rules.
        /// same category, same package.
        /// </summary>
        /// <param name="def"></param>
        /// <param name="package"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private bool CheckValidImport(ImportDefinition def, RulePackage package, RuleCategory category)
        {
            try
            {
                if (def.Category == category)
                {
                    if (def.Package == package)
                    {
                        return true;
                    }
                    else
                    {
                        if (def.Category == RuleCategory.UserDefined && ComplianceCacheManager.GetPrePostCrossImportAllowed())
                            return true;
                        else
                            return false;

                    }
                }
                else return false;
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
                return false;
            }
        }

        /// <summary>
        /// checks if export def contains metadata or not.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool ValidatePath(string path)
        {
            return File.Exists(path + "\\Metadata.xml");
        }

        /// <summary>
        /// Creates import definition for rules at path
        /// </summary>
        /// <param name="rulePath"></param>
        /// <returns></returns>
        private ImportDefinition GetImportDefinitionFromPath(string rulePath)
        {
            try
            {
                //ImportDefinition definition = new ImportDefinition();
                //DataTable dtTemp = GetImportExportRuleMetadata(rulePath);
                ImportDefinition definition = GetImportExportRuleMetadata(rulePath);

                if (definition != null)
                {
                    #region Unused
                    /* definition.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dtTemp.Rows[0]["PackageName"].ToString(), true);
                    definition.DirectoryPath = rulePath;
                    definition.OldRuleName = dtTemp.Rows[0]["RuleName"].ToString();
                    definition.RuleName = GetnewRuleNameIfRuleExists(definition.OldRuleName);
                    definition.Category = (RuleCategory)Enum.Parse(typeof(RuleCategory), dtTemp.Rows[0]["RuleCategory"].ToString(), true);
                    definition.Notification.PopUpEnabled = Convert.ToBoolean(dtTemp.Rows[0]["PopUpEnabled"].ToString());
                    definition.Notification.EmailEnabled = Convert.ToBoolean(dtTemp.Rows[0]["EmailEnabled"].ToString());
                    definition.Notification.EmailToList = dtTemp.Rows[0]["EmailToList"].ToString();
                    definition.Notification.EmailCCList = dtTemp.Rows[0]["EmailCCList"].ToString();
                    definition.Notification.StopAlertAfterMarketHours = Convert.ToBoolean(dtTemp.Rows[0]["StopAlertAfterMarketHours"].ToString());
                    definition.Notification.StopAlertOnHolidays = Convert.ToBoolean(dtTemp.Rows[0]["StopAlertOnHolidays"].ToString());
*/
                    //definition.MetaData = dtTemp;
                    //definition.NewRuleName = GetnewRuleNameIfRuleExists(definition.RuleName);                


                    //bool PrePostCrossImportAllowed = ComplianceCacheManager.GetPrePostCrossImportAllowed(CachedData.CompanyID);
                    //definition.RuleCategory = _selectedTreeNode.Tag.ToString();

                    #endregion
                    if (definition.Category == RuleCategory.CustomRule && definition.OldRuleName != definition.RuleName)
                        return null;
                    else
                        return definition;
                }
                else
                {
                    return null;
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
                return null;
            }

        }


        long _newRuleCounter = 1000;

        /// <summary>
        /// Updates rule name if exists
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        private String GetnewRuleNameIfRuleExists(string ruleName, RulePackage packageName)
        {
            try
            {
                if (RuleCache.GetInstance().IsRuleNameExists(ruleName, packageName))
                {
                    String newRuleName = "";
                    bool newRuleNameFound = false;
                    while (!newRuleNameFound)
                    {
                        newRuleName = ruleName + "_" + _newRuleCounter++;
                        if (RuleCache.GetInstance().IsRuleNameExists(newRuleName, packageName))
                            newRuleNameFound = false;
                        else
                            newRuleNameFound = true;
                    }
                    return newRuleName;
                }
                else
                    return ruleName;
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
                return null;
            }
        }

        /// <summary>
        /// Convert Xml to data table
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ImportDefinition GetImportExportRuleMetadata(string filePath)
        {
            try
            {
                // DataTable dtTemp = new DataTable("MetaData");
                if (File.Exists(filePath + "\\MetaData.xml"))
                {
                    XmlDocument myXml = new XmlDocument();
                    myXml.Load(filePath + "\\MetaData.xml");
                    ImportDefinition definition = new ImportDefinition();
                    if (myXml.SelectSingleNode("//ClientName") != null)
                        definition.ClientName = myXml.SelectSingleNode("//ClientName").InnerText;
                    else
                        definition.ClientName = String.Empty;
                    if (myXml.SelectSingleNode("//RuleCategory") != null)
                        definition.Category = (RuleCategory)Enum.Parse(typeof(RuleCategory), myXml.SelectSingleNode("//RuleCategory").InnerText, true);
                    if (myXml.SelectSingleNode("//PackageName") != null)
                        definition.Package = (RulePackage)Enum.Parse(typeof(RulePackage), myXml.SelectSingleNode("//PackageName").InnerText, true);

                    if (myXml.SelectSingleNode("//RuleName") != null)
                        definition.OldRuleName = myXml.SelectSingleNode("//RuleName").InnerText;
                    definition.RuleName = GetnewRuleNameIfRuleExists(definition.OldRuleName, rulePackage);


                    //if (myXml.SelectSingleNode("//DirectoryPath") != null)
                    definition.DirectoryPath = filePath;

                    if (myXml.SelectSingleNode("//PopUpEnabled").InnerText != null)
                        definition.Notification.PopUpEnabled = Convert.ToBoolean(myXml.SelectSingleNode("//PopUpEnabled").InnerText);
                    else
                        definition.Notification.PopUpEnabled = true;

                    if (!String.IsNullOrEmpty(definition.ClientName) && definition.ClientName == CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyName)
                    {
                        if (myXml.SelectSingleNode("//EmailEnabled") != null)
                            definition.Notification.EmailEnabled = Convert.ToBoolean(myXml.SelectSingleNode("//EmailEnabled").InnerText);
                        else
                            definition.Notification.EmailEnabled = false;

                        if (myXml.SelectSingleNode("//EmailList") != null)
                            definition.Notification.EmailToList = myXml.SelectSingleNode("//EmailList").InnerText;
                        else
                            definition.Notification.EmailToList = String.Empty;

                        if (myXml.SelectSingleNode("//EmailCCList") != null)
                            definition.Notification.EmailCCList = myXml.SelectSingleNode("//EmailCCList").InnerText;
                        else
                            definition.Notification.EmailCCList = String.Empty;
                    }
                    else
                    {
                        definition.Notification.EmailEnabled = false;
                        definition.Notification.EmailToList = String.Empty;
                        definition.Notification.EmailCCList = String.Empty;
                    }

                    if (myXml.SelectSingleNode("//LimitFrequencyMinutes") != null)
                        definition.Notification.LimitFrequencyMinutes = Convert.ToInt32(myXml.SelectSingleNode("//LimitFrequencyMinutes").InnerText);
                    else
                        definition.Notification.LimitFrequencyMinutes = 2;

                    if (myXml.SelectSingleNode("//AlertInTimeRange") != null)
                        definition.Notification.AlertInTimeRange = Convert.ToBoolean(myXml.SelectSingleNode("//AlertInTimeRange").InnerText);
                    else
                        definition.Notification.AlertInTimeRange = false;

                    if (myXml.SelectSingleNode("//StopAlertOnHolidays") != null)
                        definition.Notification.StopAlertOnHolidays = Convert.ToBoolean(myXml.SelectSingleNode("//StopAlertOnHolidays").InnerText);
                    else
                        definition.Notification.StopAlertOnHolidays = true;

                    if (myXml.SelectSingleNode("//StartTime") != null)
                        definition.Notification.StartTime = Convert.ToDateTime(myXml.SelectSingleNode("//StartTime").InnerText);
                    else
                        definition.Notification.StartTime = DateTime.Now;

                    if (myXml.SelectSingleNode("//EndTime") != null)
                        definition.Notification.EndTime = Convert.ToDateTime(myXml.SelectSingleNode("//EndTime").InnerText);
                    else
                        definition.Notification.EndTime = DateTime.Now;

                    if (myXml.SelectSingleNode("//SendInOneEmail") != null)
                        definition.Notification.SendInOneEmail = Convert.ToBoolean(myXml.SelectSingleNode("//SendInOneEmail").InnerText);
                    else
                        definition.Notification.SendInOneEmail = false;

                    if (myXml.SelectSingleNode("//Slot1") != null)
                        definition.Notification.TimeSlots[0] = Convert.ToDateTime(myXml.SelectSingleNode("//Slot1").InnerText);
                    else
                        definition.Notification.TimeSlots[0] = Prana.BusinessObjects.DateTimeConstants.MinValue;

                    if (myXml.SelectSingleNode("//Slot2") != null)
                        definition.Notification.TimeSlots[1] = Convert.ToDateTime(myXml.SelectSingleNode("//Slot2").InnerText);
                    else
                        definition.Notification.TimeSlots[1] = Prana.BusinessObjects.DateTimeConstants.MinValue;

                    if (myXml.SelectSingleNode("//Slot3") != null)
                        definition.Notification.TimeSlots[2] = Convert.ToDateTime(myXml.SelectSingleNode("//Slot3").InnerText);
                    else
                        definition.Notification.TimeSlots[2] = Prana.BusinessObjects.DateTimeConstants.MinValue;

                    if (myXml.SelectSingleNode("//Slot4") != null)
                        definition.Notification.TimeSlots[3] = Convert.ToDateTime(myXml.SelectSingleNode("//Slot4").InnerText);
                    else
                        definition.Notification.TimeSlots[3] = Prana.BusinessObjects.DateTimeConstants.MinValue;

                    if (myXml.SelectSingleNode("//Slot5") != null)
                        definition.Notification.TimeSlots[4] = Convert.ToDateTime(myXml.SelectSingleNode("//Slot5").InnerText);
                    else
                        definition.Notification.TimeSlots[4] = Prana.BusinessObjects.DateTimeConstants.MinValue;

                    definition.GroupId = "-1";


                    #region Unused
                    /*dtTemp.Columns.Add("RuleCategory");
                    dtTemp.Columns.Add("RuleName");
                    dtTemp.Columns.Add("PackageName");
                    dtTemp.Columns.Add("DirectoryPath");
                    dtTemp.Columns.Add("PopUpEnabled");
                    dtTemp.Columns.Add("EmailEnabled");
                    dtTemp.Columns.Add("EmailToList");
                    dtTemp.Columns.Add("EmailCCList");
                    dtTemp.Columns.Add("LimitFrequencyMinutes");
                    dtTemp.Columns.Add("StopAlertAfterMarketHours");
                    dtTemp.Columns.Add("StopAlertOnHolidays");
                    dtTemp.Columns.Add("NewRuleName");
                    //dtTemp.Columns.Add("SoundEnabled");
                   // dtTemp.Columns.Add("SoundFilePath");
                    dtTemp.Rows.Add(new object[] { });
                    dtTemp.Rows[0]["RuleCategory"] = myXml.SelectSingleNode("//RuleCategory").InnerText;
                    dtTemp.Rows[0]["RuleName"] = myXml.SelectSingleNode("//RuleName").InnerText;
                    dtTemp.Rows[0]["PackageName"] = myXml.SelectSingleNode("//PackageName").InnerText;
                    dtTemp.Rows[0]["DirectoryPath"] = myXml.SelectSingleNode("//DirectoryPath").InnerText;
                    dtTemp.Rows[0]["PopUpEnabled"] = myXml.SelectSingleNode("//PopUpEnabled").InnerText;
                    dtTemp.Rows[0]["EmailEnabled"] = myXml.SelectSingleNode("//EmailEnabled").InnerText;
                    dtTemp.Rows[0]["EmailToList"] = myXml.SelectSingleNode("//EmailList").InnerText;                    
                    dtTemp.Rows[0]["EmailCCList"] = myXml.SelectSingleNode("//EmailCCList").InnerText;
                    dtTemp.Rows[0]["LimitFrequencyMinutes"] = myXml.SelectSingleNode("//LimitFrequencyMinutes").InnerText;
                    dtTemp.Rows[0]["StopAlertAfterMarketHours"] = myXml.SelectSingleNode("//StopAlertAfterMarketHours").InnerText;
                    dtTemp.Rows[0]["StopAlertOnHolidays"] = myXml.SelectSingleNode("//StopAlertOnHolidays").InnerText;
                    //dtTemp.Rows[0]["ManualTradeEnabled"] = myXml.SelectSingleNode("//ManualTradeEnabled").InnerText;
                    //dtTemp.Rows[0]["WarningFrequencyMinutes"] = 1;
                    //dtTemp.Rows[0]["SoundEnabled"] = false;
                    //dtTemp.Rows[0]["SoundFilePath"] = String.Empty;*/
                    #endregion

                    return definition;

                }
                else
                {
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// Gets path for importing rules from the user.
        /// </summary>
        /// <returns></returns>
        private List<string> GetRuleDirectoryFromUser()
        {
            List<string> rulePathList = new List<string>();
            try
            {
                FolderBrowserDialog diag = new FolderBrowserDialog();
                if (Directory.Exists(ComplianceCacheManager.GetImportExportPath()))
                    diag.SelectedPath = ComplianceCacheManager.GetImportExportPath() + "\\" + rulePackage + "\\";

                diag.ShowNewFolderButton = false;

                DialogResult dr = diag.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    //String[] probPath = Directory.GetDirectories(diag.SelectedPath);
                    rulePathList.AddRange(Directory.GetDirectories(diag.SelectedPath));
                }
                return rulePathList;
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
            return null;
        }

        /// <summary>
        /// Event raised when UI is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportRuleSelection_Shown(object sender, EventArgs e)
        {
            InitializeControl();
        }


        /// <summary>
        /// Returns Import rule definition list to Rule Manager.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, ImportDefinition> GetImportListDefinition()
        {
            return this._importDefCache;
        }

        private void ImportRuleSelection_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_COMPLIANCE_ENGINE);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();
                }
                else
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.ulstView.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                    this.ulstView.ForeColor = System.Drawing.Color.Black;
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                ultraButton2.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ultraButton2.ForeColor = System.Drawing.Color.White;
                ultraButton2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton2.UseAppStyling = false;
                ultraButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton1.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        // set the default theme when there is no theme specified
        private void SetAppearanceWithoutTheme()
        {
            try
            {
                this.ulstView.BackColor = System.Drawing.Color.DimGray;
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
    }
}
