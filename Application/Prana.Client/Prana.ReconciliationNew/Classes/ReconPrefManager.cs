using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PM.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.ReconciliationNew
{
    public class ReconPrefManager
    {
        //TODO: As there are now only one rule, so rulesDict will now have MatchinRule as value instead of List

        static string _reconPreferencesFilePath = string.Empty;

        public static string ReconPreferencesFilePath
        {
            get { return ReconPrefManager._reconPreferencesFilePath; }
            set { ReconPrefManager._reconPreferencesFilePath = value; }
        }
        static string _reconPreferencesDirectoryPath = string.Empty;
        static ReconPreferences _oldReconPreferences = null;
        static string _startPath = Application.StartupPath;
        //static int _userID = int.MinValue;

        public static event EventHandler prefsSaved;
        public static event EventHandler _launchForm;

        private static void LoadDefaultDataForTemplate(ReconTemplate template)
        {
            try
            {
                //Load master columns and matching rules from xsd and xml files
                #region ReconFilters
                #endregion
                #region Matching Rules
                DataSet dsMatchingRulesDefault = new DataSet();
                string xmlRulePath = ReconPrefManager.ReconPreferences.XmlRulePath;
                string MatchingRuleSchema = xmlRulePath + "//XmlMatchingRule.xsd";
                string xmlMatchingRulePath = xmlRulePath + "//XmlMatchingRule.xml";
                string xmlMasterColumnsPath = xmlRulePath + "//MasterColumns.xml";
                string xmlMasterColumnsSchema = xmlRulePath + "//MasterColumns.xsd";
                //Load matching rules
                dsMatchingRulesDefault.ReadXmlSchema(MatchingRuleSchema);
                dsMatchingRulesDefault.ReadXml(xmlMatchingRulePath);
                RemoveRowsBasedOnTemplateReconType(template.ReconType, dsMatchingRulesDefault);
                template.DsMatchingRules = dsMatchingRulesDefault;
                #endregion
                #region Master Columns
                //Load master columns
                DataSet dsMasterColumnsDefault = new DataSet();
                dsMasterColumnsDefault.ReadXmlSchema(xmlMasterColumnsSchema);
                dsMasterColumnsDefault.ReadXml(xmlMasterColumnsPath);
                //Load master columns which have datasouce PB

                DataTable dtMatchingRule = dsMatchingRulesDefault.Tables[0];
                //fetches the sp name form the matchigrules.xml
                template.SpName =
                    (from r in dtMatchingRule.AsEnumerable()
                     where r.Field<string>("Name").Equals(template.ReconType.ToString())
                     select r.Field<string>("SP")).FirstOrDefault().ToString();



                DataTable dtPBMasterColumns = GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, Prana.BusinessObjects.AppConstants.DataSourceType.PrimeBroker);
                dtPBMasterColumns.TableName = ReconConstants.TABLENAME_PBGridMasterColumns;
                //Load master columns which have datasouce Nirvana
                DataTable dtNirvanaMasterColumns = GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, Prana.BusinessObjects.AppConstants.DataSourceType.Nirvana);
                dtNirvanaMasterColumns.TableName = ReconConstants.TABLENAME_NirvanaGridColumns;
                template.DsMasterColumns.Tables.Clear();
                template.SortingColumnOrder = ReconPrefManager.ReconPreferences.getSortedColumnsWithoutDataSet(template.ReconType.ToString());

                //Add nirvana and pb tables to the template
                template.DsMasterColumns.Tables.Add(dtNirvanaMasterColumns);
                template.DsMasterColumns.Tables.Add(dtPBMasterColumns);
                //For New Grouping Logic for PNL
                //get grouping summary for the PNL
                template.DictGroupingSummary = GetGroupingSummary(dsMasterColumnsDefault, template.ReconType);
                #endregion
                #region Recon Report Layout
                //clear all the list which are used in the exception report layout
                template.AvailableColumnList.Clear();
                template.SelectedColumnList.Clear();
                template.ListGroupByColumns.Clear();
                template.ListSortByColumns.Clear();
                //Load default columns for the exception report layout
                GetDefaultSelectedColumnsForExceptionReport(template, dsMasterColumnsDefault);
                #endregion
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
        //Add default template when no preference are saved
        public static void AddDefaultTemplate(int clientID, string reconType, string templateName)
        {
            try
            {
                ReconTemplate template = _oldReconPreferences.AddDefaultTemplate(clientID, reconType, templateName);
                //Template name and key are also to be added in template property
                template.TemplateName = templateName;
                template.TemplateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID.ToString(), reconType, templateName);
                LoadDefaultDataForTemplate(template);
            }
            #region Catch
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
            #endregion
        }
        //copy template
        public static void copyTemplate(string templateKey, string newTemplateKey)
        {
            try
            {
                ReconTemplate templateToCopy = _oldReconPreferences.GetTemplates(templateKey);
                //int hashcodeold = templateToCopy.GetHashCode();
                ReconTemplate newTemplate = new ReconTemplate();
                newTemplate = (ReconTemplate)(DeepCopyHelper.Clone(templateToCopy));
                newTemplate.IsDirtyForSaving = true;
                // Add the client Id key to the new template created
                newTemplate.ClientID = Convert.ToInt32(ReconUtilities.GetClientIDFromTemplateKey(newTemplateKey));
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1442
                newTemplate.TemplateKey = newTemplateKey;
                //int newCode = newTemplate.GetHashCode();
                //ReconPreferences.newReconTemplates.Add(newTemplateName, newTemplate);
                _oldReconPreferences.UpdateTemplates(newTemplateKey, newTemplate);
            }
            #region Catch
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
            #endregion
        }

        public static void RemoveRowsBasedOnTemplateReconType(ReconType reconType, DataSet dsMatchingRules)
        {
            try
            {
                if (dsMatchingRules != null)
                {
                    DataTable dtRules = dsMatchingRules.Tables[0];
                    string idColumn = dtRules.Columns[4].Caption;
                    //string reconTypeColumn = dtRules.Columns[1].Caption;
                    string rowsToBind = string.Empty;
                    //Remove duplicate rows from the table so that only distict row exists
                    DataTable dtDistinctTable = dsMatchingRules.Tables[1].DefaultView.ToTable(/*distinct*/ true);
                    dsMatchingRules.Tables[1].Clear();
                    dsMatchingRules.Tables[1].Merge(dtDistinctTable);
                    foreach (DataRow dr in dtRules.Rows)
                    {
                        //((int)(ReconType)(Enum.Parse(typeof(ReconType),dr[reconTypeColumn].ToString()))
                        if ((!string.IsNullOrWhiteSpace(dr[ReconConstants.COLUMN_ReconType].ToString())) && (int.Parse(dr[ReconConstants.COLUMN_ReconType].ToString()) == ((int)reconType)))
                        {
                            rowsToBind = dr[idColumn].ToString();
                            //check that sp name is added for that template type.
                            //if (!ReconPrefManager.ReconPreferences.DictReconSP.ContainsKey(int.Parse(rowsToBind)))
                            //{
                            //    ReconPrefManager.ReconPreferences.DictReconSP.Add(int.Parse(rowsToBind), (string)dr[ReconConstants.COLUMN_SP]);
                            //}
                            //break;
                        }
                    }
                    List<DataRow> listRowstoRemove = new List<DataRow>();
                    foreach (DataRow row in dsMatchingRules.Tables[1].Rows)
                    {
                        if (row[idColumn].ToString() != rowsToBind)
                        {
                            listRowstoRemove.Add(row);
                        }
                    }
                    foreach (DataRow rowtoRemove in listRowstoRemove)
                    {
                        dsMatchingRules.Tables[1].Rows.Remove(rowtoRemove);
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
        }
        //For New Grouping Logic for PNL
        public static SerializableDictionary<string, string> GetGroupingSummary(DataSet dsMain, ReconType reconType)
        {
            SerializableDictionary<string, string> dictGroupingSummary = new SerializableDictionary<string, string>();
            try
            {
                if (dsMain != null && dsMain.Tables.Count > 0)
                {
                    foreach (DataRow Row in dsMain.Tables[1].Rows)
                    {
                        //check for recon type and contains key check for the dictionary
                        if (Row[ReconConstants.COLUMN_ReconId].ToString().Equals(((int)(reconType)).ToString()) && (!dictGroupingSummary.ContainsKey(Row[ReconConstants.COLUMN_Name].ToString())))
                        {
                            dictGroupingSummary.Add(Row[ReconConstants.COLUMN_Name].ToString(), Row[ReconConstants.COLUMN_Summary].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dictGroupingSummary;
        }
        public static DataTable GetFilteredMasterColumns(DataSet dsMain, ReconType reconType, Prana.BusinessObjects.AppConstants.DataSourceType dsType)
        {
            DataTable dtFiltered = new DataTable();
            try
            {
                if (dsMain != null)
                {
                    if (dsMain.Tables.Count > 0)
                    {
                        //changes done Table[0] to Table[1] 13/08/2012
                        dtFiltered = dsMain.Tables[1].Clone();
                        //foreach recon template
                        foreach (DataRow ColumnRow in dsMain.Tables[1].Rows)
                        {
                            //foreach column in template
                            if (((int.Parse(ColumnRow[ReconConstants.COLUMN_DataSourceType].ToString())).Equals(((int)dsType)) || int.Parse(ColumnRow[ReconConstants.COLUMN_DataSourceType].ToString()).Equals((int)Prana.BusinessObjects.AppConstants.DataSourceType.Both)) && (int.Parse(ColumnRow[ReconConstants.COLUMN_ReconId].ToString()).Equals((int)reconType)))
                            {
                                //DataRow[] result = dtFiltered.Select((string.Format(ReconConstants.COLUMN_Name + " ='{0}'", ColumnRow[ReconConstants.COLUMN_DataSourceType].ToString())));
                                //if (result.Length == 0)
                                //{
                                dtFiltered.Rows.Add(ColumnRow.ItemArray);
                                //}
                            }
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
            //remove duplicate rows and get only distinct rows
            dtFiltered = dtFiltered.DefaultView.ToTable( /*distinct*/ true);
            return dtFiltered;
        }
        private static void GetDefaultSelectedColumnsForExceptionReport(ReconTemplate template, DataSet dsMasterColumnsDefault)
        {
            try
            {
                //get rules for the selected template
                List<MatchingRule> rules = template.RulesList;
                if (dsMasterColumnsDefault.Tables.Count > 0)
                {
                    //add items to the exceptio report layout list
                    //change Table[0] to Table[1]
                    foreach (DataRow Row in dsMasterColumnsDefault.Tables[1].Rows)
                    {
                        if (((int)(template.ReconType)).Equals(int.Parse(Row[ReconConstants.COLUMN_ReconId].ToString())))
                        {
                            ColumnInfo item = new ColumnInfo();
                            item.ColumnName = (string)Row[ReconConstants.COLUMN_Name];
                            item.GroupType = (ColumnGroupType)int.Parse(Row[ReconConstants.COLUMN_GroupType].ToString());
                            //add accountname and symbol to the default sorting list
                            if (item.ColumnName.Equals("FundName") || item.ColumnName.Equals("Symbol"))
                            {
                                item.SortOrder = SortingOrder.Ascending;
                                template.ListSortByColumns.Add(item);
                            }
                            switch (item.GroupType)
                            {
                                case ColumnGroupType.Nirvana:
                                    template.AvailableColumnList.Add(item);
                                    break;
                                case ColumnGroupType.PrimeBroker:
                                    template.AvailableColumnList.Add(item);
                                    break;
                                case ColumnGroupType.Common:
                                    //modified by amit 30.03.2015
                                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3172
                                    if (rules[0].ComparisonFields.Contains(item.ColumnName) || (item.ColumnName.Equals(ReconConstants.MismatchReason)) || (item.ColumnName.Equals(ReconConstants.MismatchType)))
                                    {
                                        template.SelectedColumnList.Add(item);
                                    }
                                    else
                                    {
                                        template.AvailableColumnList.Add(item);
                                    }

                                    break;
                                case ColumnGroupType.Both:

                                    ColumnInfo NirvanaItem = item;
                                    NirvanaItem.GroupType = ColumnGroupType.Nirvana;

                                    ColumnInfo PBItem = item;
                                    PBItem.GroupType = ColumnGroupType.PrimeBroker;

                                    ColumnInfo DiffItem = item;
                                    DiffItem.GroupType = ColumnGroupType.Diff;

                                    if (rules[0].ComparisonFields.Contains(item.ColumnName))
                                    {
                                        template.SelectedColumnList.Add(NirvanaItem);
                                        template.SelectedColumnList.Add(PBItem);
                                        if (rules[0].NumericFields.Contains(item.ColumnName))
                                        {
                                            template.SelectedColumnList.Add(DiffItem);
                                        }
                                    }
                                    else
                                    {
                                        template.AvailableColumnList.Add(NirvanaItem);
                                        template.AvailableColumnList.Add(PBItem);
                                        if (rules[0].NumericFields.Contains(item.ColumnName))
                                        {
                                            template.AvailableColumnList.Add(DiffItem);
                                        }

                                    }
                                    break;
                                case ColumnGroupType.Diff:
                                    break;
                                default:
                                    break;
                            }
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
        }
        public static ReconPreferences ReconPreferences
        {
            get
            {
                if (_oldReconPreferences == null)
                {

                    _oldReconPreferences = GetPreferences();
                }
                return _oldReconPreferences;
            }
        }

        public static void SetUp(string startUpPath)
        {
            try
            {
                _startPath = startUpPath;
                GetPreferences();
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

        /// <summary>
        /// This method is only will be called only once when release is set up or new preference is loaded
        /// update recondictionary for proper client id if client id is 0
        /// </summary>
        /// <param name="dictTemplates"></param>
        /// <returns></returns>
        private static Dictionary<string, ReconTemplate> GetDictTemplates(KeyValuePair<string, ReconTemplate> templateKvp)
        {
            try
            {

                Dictionary<int, string> dictClient = CachedDataManager.GetUserPermittedCompanyList();

                Dictionary<string, ReconTemplate> dictNewTemplates = new Dictionary<string, ReconTemplate>();
                //every template is added in each client
                foreach (KeyValuePair<int, string> ClientKvp in dictClient)
                {
                    string templateKey = ReconUtilities.GetTemplateKeyFromParameters(ClientKvp.Key.ToString(), templateKvp.Value.ReconType.ToString(), templateKvp.Key);
                    templateKvp.Value.ClientID = ClientKvp.Key;
                    templateKvp.Value.IsDirtyForSaving = true;
                    //CHMW-1876	[Reconciliation] [Code Review] Code re-factoring for method GetPreferences() in ReconPreferences
                    //templateKvp.Value.TemplateName = ReconUtilities.GetTemplateNameFromTemplateKey(templateKey);
                    //templateKvp.Value.TemplateKey = templateKey;
                    //templateKvp.Value.XsltPath = Path.GetFileName(templateKvp.Value.XsltPath);
                    ReconTemplate reconTemplate = UpdatePropertiesInPrefrenceFile(templateKvp.Value, templateKvp.Key);
                    ReconTemplate template = (ReconTemplate)(DeepCopyHelper.Clone(reconTemplate));
                    if (!dictNewTemplates.ContainsKey(reconTemplate.TemplateKey))
                    {
                        dictNewTemplates.Add(templateKey, template);
                    }
                }
                return dictNewTemplates;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
        public static ReconPreferences GetPreferences()
        {
            //_userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            ReconPreferences reconPreferences = null;

            // Added By: Pranay Deep 20 Oct 2015
            // For getting the datatable from the database table T_ReconPrefrences
            #region Load Data table ReconPrefrences from database

            bool checkState;
            DataTable dtReconPreferences = new DataTable();
            dtReconPreferences = LoadReconPreferencesFromDatabase();

            #endregion

            // _reconPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _reconPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME;
            _reconPreferencesFilePath = _reconPreferencesDirectoryPath + @"\ReconPreference.xml";
            try
            {
                if (!Directory.Exists(_reconPreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_reconPreferencesDirectoryPath);
                }
                if (File.Exists(_reconPreferencesFilePath))
                {
                    //reconPreferences = XMLUtilities.DeserializeFromXMLFile<ReconPreferences>(_reconPreferencesFilePath);
                    //CHMW-1876	[Reconciliation] [Code Review] Code re-factoring for method GetPreferences() in ReconPreferences
                    //CHMW-3006	[Recon] When multiple users are working then sometimes on saving recon preference, error comes and preference doesn't save
                    if (Prana.Utilities.IO.File.IsFileOpen(_reconPreferencesFilePath))
                    {
                        using (FileStream fs = File.OpenRead(_reconPreferencesFilePath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ReconPreferences));
                            reconPreferences = (ReconPreferences)serializer.Deserialize(fs);
                            fs.Flush();
                        }
                        SerializableDictionary<string, ReconTemplate> dictReconTemplateTemp = reconPreferences.DictReconTemplates;
                        //contains all the template with client id                        
                        Dictionary<string, ReconTemplate> defaultTemplate = new Dictionary<string, ReconTemplate>();
                        //this will be running only once after release is deployed to make the preference file consistet to the system
                        #region update recon preference.

                        //string checkToUpdateReconPreferencesFilePath = Application.StartupPath + @"\MappingFiles\ReconRulesFile\CheckToUpdateReconPreferences.xml";
                        //this is to be done only once in the release

                        if (IsTemplatesToBeUpdated())
                        {
                            foreach (KeyValuePair<string, ReconTemplate> kvp in dictReconTemplateTemp)
                            {
                                #region if Client ID is zero then add the template in all the clients available
                                if (kvp.Value.ClientID == 0)
                                {
                                    //set new dictionary with structured key and client ID in template
                                    //reconPreferences.DictReconTemplates = getDicttemplates(reconPreferences.DictReconTemplates);
                                    //adds the dictionary and eliminate duplicates
                                    defaultTemplate = defaultTemplate.Concat(GetDictTemplates(kvp)).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                                }
                                #endregion
                                else
                                {
                                    // Add fields in the template if does not exist.
                                    ReconTemplate reconTemplate = UpdatePropertiesInPrefrenceFile(kvp.Value, kvp.Key);
                                    if (!defaultTemplate.ContainsKey(reconTemplate.TemplateKey))
                                    {
                                        defaultTemplate.Add(reconTemplate.TemplateKey, DeepCopyHelper.Clone(reconTemplate) as ReconTemplate);
                                    }
                                }
                            }
                            //clear and fill the template dictionary
                            reconPreferences.DictReconTemplates.Clear();
                            SerializableDictionary<string, ReconFilters> dictReconFilters = new SerializableDictionary<string, ReconFilters>();
                            foreach (KeyValuePair<string, ReconTemplate> kvp in defaultTemplate)
                            {
                                //create ReconFilters Dictionary
                                if (!dictReconFilters.ContainsKey(kvp.Key))
                                {
                                    dictReconFilters.Add(kvp.Key, kvp.Value.ReconFilters);
                                }
                                if (!reconPreferences.DictReconTemplates.ContainsKey(kvp.Key))
                                {
                                    reconPreferences.DictReconTemplates.Add(kvp.Key, DeepCopyHelper.Clone(kvp.Value));
                                }
                            }
                            #region Create batches

                            CreateBathcesForOldReconPreference(reconPreferences, dictReconFilters);

                            #endregion
                            //CHMW-3006	[Recon] When multiple users are working then sometimes on saving recon preference, error comes and preference doesn't save
                            if (Prana.Utilities.IO.File.IsFileOpen(_reconPreferencesFilePath))
                            {
                                using (XmlTextWriter writer = new XmlTextWriter(_reconPreferencesFilePath, Encoding.UTF8))
                                {
                                    writer.Formatting = Formatting.Indented;
                                    XmlSerializer serialize;
                                    serialize = new XmlSerializer(typeof(ReconPreferences));
                                    serialize.Serialize(writer, reconPreferences);
                                    writer.Flush();
                                }
                            }
                            //update the preference file so that it is modified 
                            //XMLUtilities.SerializeToXMLFile<ReconPreferences>(reconPreferences, _reconPreferencesFilePath);
                            CreateReconFiltersPreference(dictReconFilters);
                        }
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1384
                        //Overwrite Recon filters from separate user preference file
                        SerializableDictionary<string, ReconFilters> dictReconFilter = new SerializableDictionary<string, ReconFilters>();
                        string filtersFilepath = _reconPreferencesDirectoryPath + "\\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\ReconFilters.xml";
                        if (File.Exists(filtersFilepath))
                        {
                            //CHMW-3006	[Recon] When multiple users are working then sometimes on saving recon preference, error comes and preference doesn't save
                            if (Prana.Utilities.IO.File.IsFileOpen(filtersFilepath))
                            {
                                using (FileStream readReconFilters = File.OpenRead(filtersFilepath))
                                {
                                    XmlSerializer reconFiltersSerializer = new XmlSerializer(typeof(SerializableDictionary<string, ReconFilters>));
                                    dictReconFilter = (SerializableDictionary<string, ReconFilters>)reconFiltersSerializer.Deserialize(readReconFilters);
                                }
                            }
                            //dictReconFilter = XMLUtilities.DeserializeFromXMLFile<SerializableDictionary<string, ReconFilters>>(filtersFilepath);
                        }
                        foreach (KeyValuePair<string, ReconTemplate> kvp in reconPreferences.DictReconTemplates)
                        {
                            if (dictReconFilter.Keys.Contains(kvp.Key, StringComparer.InvariantCultureIgnoreCase))
                            {
                                //CashAccounts and third party are dependent on user permissioning and rest of the filters are independent.
                                if (dictReconFilter[kvp.Key].DictAccounts != null && dictReconFilter[kvp.Key].DictAccounts.Count > 0)
                                    kvp.Value.ReconFilters.DictAccounts = dictReconFilter[kvp.Key].DictAccounts;
                                if (dictReconFilter[kvp.Key].DictThirParty != null && dictReconFilter[kvp.Key].DictThirParty.Count > 0)
                                    kvp.Value.ReconFilters.DictThirParty = dictReconFilter[kvp.Key].DictThirParty;
                            }
                            else
                            {
                                kvp.Value.ReconFilters = new ReconFilters();
                            }

                            if (kvp.Value.IsReconTemplateGroupping())
                            {
                                UpdateNewGroupingSchemaInTemplate(kvp.Value);
                            }

                            // Added By: Pranay Deep 20 Oct 2015
                            // For getting the check state from the database
                            #region get check state for "IsShowCAGeneratedTrades"

                            checkState = GetCheckState(dtReconPreferences, kvp.Value);
                            kvp.Value.IsShowCAGeneratedTrades = checkState;

                            #endregion


                        }

                        #endregion
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    reconPreferences = new ReconPreferences();// GetDefualtPreferences();
                }
                _oldReconPreferences = reconPreferences;
            }
            #region Catch
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
            #endregion
            return reconPreferences;
        }

        /// <summary>
        /// Created By: Pranay Deep 20 Oct 2015
        /// This method is used for getting the check state,
        /// from data table dtReconPreferences
        /// </summary>
        /// <param name="dtReconPreferences"></param>
        /// <param name="reconTemplate"></param>
        /// <returns>checkstate</returns>
        private static bool GetCheckState(DataTable dtReconPreferences, ReconTemplate reconTemplate)
        {
            bool checkstate;

            try
            {

                if (dtReconPreferences.Rows.Count > 0)
                {
                    checkstate = (from T_ReconPreferences in dtReconPreferences.AsEnumerable()
                                  where
                                  T_ReconPreferences.Field<Int32>(ReconConstants.COLUMN_ClientID) == ((reconTemplate.ClientID))
                                  &
                                  (T_ReconPreferences.Field<string>(ReconConstants.COLUMN_TemplateName)).Equals((reconTemplate.TemplateName).ToString())
                                  &
                                  T_ReconPreferences.Field<string>(ReconConstants.COLUMN_TemplateKey) == ((reconTemplate.TemplateKey).ToString())

                                  select T_ReconPreferences.Field<bool>(ReconConstants.COLUMN_IsShowCAGeneratedTrades)).FirstOrDefault();
                    return checkstate;
                }
                else
                    return true;
            }

            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return true;
            }
        }

        /// <summary>
        /// Created By: Pranay Deep 20 Oct 2015
        /// This method is used, to get the data from database table "T_ReconPreferences",
        /// and fill it in the data table "dtReconPreferences"
        /// </summary>
        /// <returns>dtReconPreferences</returns>
        public static DataTable LoadReconPreferencesFromDatabase()
        {
            DataTable dtReconPreferences = new DataTable();
            try
            {

                DataSet dsReconPreferences = new DataSet();
                dsReconPreferences = DatabaseManager.GetInstance().GetReconPrefrencesFromDB();
                if (dsReconPreferences != null && dsReconPreferences.Tables.Count > 0)
                    dtReconPreferences = dsReconPreferences.Tables[0];
                dtReconPreferences.TableName = "T_ReconPreferences";

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
            return dtReconPreferences;
        }

        /// <summary>
        /// Created By: Pranay Deep 10 sept 2015
        /// Update New grouping schema in the recon template preference
        /// </summary>
        /// <param name="reconTemplate"></param>
        private static void UpdateNewGroupingSchemaInTemplate(ReconTemplate reconTemplate)
        {
            try
            {
                #region Creating the dictionary for the first time
                DataSet dsReconGrouping = new DataSet();
                dsReconGrouping = DatabaseManager.GetInstance().GetReconGroupingCriteria();
                DataTable dtRecReconGrouping = dsReconGrouping.Tables[0];
                dtRecReconGrouping.TableName = "Grouping";
                string data = null;

                data = (from Grouping in dtRecReconGrouping.AsEnumerable()
                            //TODO: Create constant
                        where Grouping.Field<string>(ReconConstants.COLUMN_ReconTypeName) == ((reconTemplate.ReconType).ToString())
                        select Grouping.Field<string>(ReconConstants.COLUMN_GroupingColumns)).First<string>();

                List<string> ListGroupingCriteria = data.Split(Seperators.SEPERATOR_14).ToList();

                foreach (string item in ListGroupingCriteria)
                {
                    if (!reconTemplate.GroupingCrieria.DictGroupingCriteria.ContainsKey(item))
                    {
                        reconTemplate.GroupingCrieria.DictGroupingCriteria.Add(item, false);
                    }
                }
                #endregion


                for (int i = 0; i < reconTemplate.GroupingCrieria.DictGroupingCriteria.Count; i++)
                {

                    if ((reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key).Equals("Account"))
                    {
                        if (reconTemplate.GroupingCrieria.IsGroupByAccount)
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = true;

                            reconTemplate.GroupingCrieria.IsGroupByAccount = false;
                        }
                        else
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = false;

                            reconTemplate.GroupingCrieria.IsGroupByAccount = false;
                        }
                    }

                    if ((reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key).Equals("Symbol"))
                    {

                        if (reconTemplate.GroupingCrieria.IsGroupBySymbol)
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = true;

                            reconTemplate.GroupingCrieria.IsGroupBySymbol = false;
                        }
                        else
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = false;

                            reconTemplate.GroupingCrieria.IsGroupBySymbol = false;
                        }
                    }

                    if ((reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key).Equals("Side"))
                    {

                        if (reconTemplate.GroupingCrieria.IsGroupBySide)
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = true;

                            reconTemplate.GroupingCrieria.IsGroupBySide = false;
                        }
                        else
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = false;

                            reconTemplate.GroupingCrieria.IsGroupBySide = false;
                        }
                    }

                    if ((reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key).Equals("MasterFund"))
                    {
                        if (reconTemplate.GroupingCrieria.IsGroupbyMasterFund)
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = true;

                            reconTemplate.GroupingCrieria.IsGroupbyMasterFund = false;
                        }
                        else
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = false;

                            reconTemplate.GroupingCrieria.IsGroupbyMasterFund = false;
                        }
                    }
                    if ((reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key).Equals("Broker"))
                    {
                        if (reconTemplate.GroupingCrieria.IsGroupByBroker)
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = true;

                            reconTemplate.GroupingCrieria.IsGroupByBroker = false;
                        }
                        else
                        {
                            reconTemplate.GroupingCrieria.DictGroupingCriteria[reconTemplate.GroupingCrieria.DictGroupingCriteria.ElementAt(i).Key] = false;

                            reconTemplate.GroupingCrieria.IsGroupByBroker = false;
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
        }
        /// <summary>
        /// Created By-sachin mishra   5/1/15 purpose-CHMW-3047
        /// </summary>
        /// <param name="reconPreferences"></param>
        /// <param name="dictReconFilters"></param>
        private static void CreateBathcesForOldReconPreference(ReconPreferences reconPreferences, SerializableDictionary<string, ReconFilters> dictReconFilters)
        {
            Dictionary<int, List<int>> _dictDataSourceSubAccountAssociation = new Dictionary<int, List<int>>();
            Dictionary<int, string> _dictThirdParties = new Dictionary<int, string>();

            int releaseID = 0;
            Dictionary<int, List<int>> dictReleaseAccounts = CachedDataManager.GetInstance.ReleaseWiseAccount;

            foreach (KeyValuePair<int, List<int>> thirdparty in CachedDataManagerRecon.GetInstance.GetAllCompanyThirdPartyAccounts())
            {
                if (CachedDataManager.GetInstance.GetAllThirdParties().ContainsKey(thirdparty.Key))
                {
                    foreach (int accountID in thirdparty.Value)
                    {

                        // dictionary of thirdparties ID and Names of selected client
                        if (!_dictThirdParties.ContainsKey(thirdparty.Key))
                        {
                            _dictThirdParties.Add(thirdparty.Key, CachedDataManager.GetInstance.GetAllThirdParties()[thirdparty.Key]);
                        }
                        // dictionary containing thirdpartyID and AccountsID of selected client
                        if (!_dictDataSourceSubAccountAssociation.ContainsKey(thirdparty.Key))
                        {
                            List<int> lstAccountIds = new List<int>();
                            lstAccountIds.Add(accountID);
                            _dictDataSourceSubAccountAssociation.Add(thirdparty.Key, lstAccountIds);
                        }
                        else if (!_dictDataSourceSubAccountAssociation[thirdparty.Key].Contains(accountID))
                        {
                            _dictDataSourceSubAccountAssociation[thirdparty.Key].Add(accountID);
                        }
                    }
                }
            }


            DataTable dtSetting = new DataTable("TempDataTable");
            dtSetting.Columns.Add("SettingID", typeof(int));
            dtSetting.Columns.Add("IsActive", typeof(bool));
            dtSetting.Columns.Add("FormatName", typeof(string));
            dtSetting.Columns.Add("ImportTypeID", typeof(int));
            dtSetting.Columns.Add("ClientID", typeof(int));
            dtSetting.Columns.Add("ReleaseID", typeof(int));
            dtSetting.Columns.Add("FundID", typeof(List<int>));
            dtSetting.Columns.Add("XSLTPath", typeof(string));
            dtSetting.Columns.Add("XSDPath", typeof(string));
            dtSetting.Columns.Add("ImportSPName", typeof(string));
            dtSetting.Columns.Add("FTPFolderPath", typeof(string));
            dtSetting.Columns.Add("LocalFolderPath", typeof(string));
            dtSetting.Columns.Add("FtpID", typeof(int));
            dtSetting.Columns.Add("EmailID", typeof(int));
            dtSetting.Columns.Add("EmailLogID", typeof(int));
            dtSetting.Columns.Add("DecryptionID", typeof(int));
            dtSetting.Columns.Add("ThirdPartyID", typeof(int));
            dtSetting.Columns.Add("PriceToleranceColumns", typeof(string));
            dtSetting.Columns.Add("FormatType", typeof(int));
            dtSetting.Columns.Add("BatchStartDate", typeof(string));
            dtSetting.Columns.Add("ImportFormatID", typeof(int));

            SerializableDictionary<string, ReconTemplate> dictDetails = reconPreferences.DictReconTemplates;
            List<ReconTemplate> newTemplates = new List<ReconTemplate>();
            foreach (KeyValuePair<string, ReconTemplate> kvpReconTemplate in dictDetails)
            {
                ReconTemplate reconTemp = kvpReconTemplate.Value;
                int clientID = reconTemp.ClientID;
                string formatName = reconTemp.FormatName;
                int thirdPartyID = int.MinValue;
                if (string.IsNullOrEmpty(formatName))
                {
                    int settingID = int.MinValue;
                    bool isActive = true;
                    int importTypeID = int.MinValue;
                    string xsltPath = reconTemp.XsltPath;
                    string xsdPath = string.Empty;
                    string importSPName = reconTemp.SpName;
                    string ftpFolderPath = string.Empty;
                    string localFolderPath = string.Empty;
                    int ftpId = int.MinValue;
                    int emailID = int.MinValue;
                    int emailLogId = int.MinValue;
                    int decryptID = int.MinValue;
                    string priceToleranceColumns = string.Empty;
                    int formatType = 1;
                    string dtBatchDate = DateTime.Now.ToString();
                    int importFormatID = int.MinValue;

                    ReconFilters filter = dictReconFilters[reconTemp.TemplateKey];
                    List<int> accountList = GetAccountList(filter.DictAccounts);
                    //Dictionary<int, List<int>> pbWithAccounts = GetAllPBwithAccounts(accountList);

                    Dictionary<string, List<int>> dictDiscretAccounts = new Dictionary<string, List<int>>();
                    foreach (int accountID in accountList)
                    {
                        if (dictReleaseAccounts.Any(innerList => innerList.Value.Any(s => s == accountID)))
                        {
                            int accountReleaseID = dictReleaseAccounts.FirstOrDefault(x => x.Value.Contains(accountID)).Key;

                            if (_dictDataSourceSubAccountAssociation.Any(innerList => innerList.Value.Any(s => s == accountID)))
                            {
                                int accountThirdPartyID = _dictDataSourceSubAccountAssociation.FirstOrDefault(x => x.Value.Contains(accountID)).Key;

                                string key = accountReleaseID.ToString().Trim() + Seperators.SEPERATOR_13 + accountThirdPartyID.ToString().Trim();
                                if (dictDiscretAccounts.ContainsKey(key))
                                {
                                    dictDiscretAccounts[key].Add(accountID);
                                }
                                else
                                {
                                    dictDiscretAccounts.Add(key, new List<int>() { accountID });
                                }
                            }
                        }
                    }


                    bool isFirst = true;
                    foreach (KeyValuePair<string, List<int>> key in dictDiscretAccounts)
                    {
                        thirdPartyID = Convert.ToInt32((key.Key.Split(Seperators.SEPERATOR_13[0])[1]));
                        string thirdpartyName = _dictThirdParties[thirdPartyID];
                        string clientName = CachedDataManager.GetCompanyText(clientID);
                        releaseID = Convert.ToInt32((key.Key.Split(Seperators.SEPERATOR_13[0])[0]));
                        formatName = clientName + Seperators.SEPERATOR_13 + reconTemp.ReconType + Seperators.SEPERATOR_13 + reconTemp.TemplateName + Seperators.SEPERATOR_13 + releaseID.ToString();
                        String reconTempFormat = "PB." + thirdpartyName + "." + reconTemp.ReconType + "." + formatName;

                        accountList = key.Value;
                        if (!isFirst)
                        {
                            ReconTemplate newTemplate = DeepCopyHelper.Clone(reconTemp);
                            newTemplate.TemplateName = reconTemp.TemplateName + Seperators.SEPERATOR_13 + key.Key.ToString();
                            newTemplate.TemplateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID.ToString(), reconTemp.ReconType.ToString(), (reconTemp.TemplateName + Seperators.SEPERATOR_13 + key.Key.ToString()));
                            newTemplates.Add(newTemplate);
                            dictReconFilters.Add(newTemplate.TemplateKey, newTemplate.ReconFilters);
                        }
                        else
                        {
                            reconTemp.FormatName = reconTempFormat;
                            isFirst = false;
                        }
                        dtSetting.Rows.Add(settingID, isActive, formatName, importTypeID, clientID,
                            releaseID, accountList, xsltPath, xsdPath, importSPName, ftpFolderPath, localFolderPath,
                            ftpId, emailID, emailLogId, decryptID, thirdPartyID, priceToleranceColumns, formatType, dtBatchDate, importFormatID);
                    }


                }
                if (dtSetting.Rows.Count > 0)
                {
                    int i = FileSettingManager.SaveFileSettingForRecon(dtSetting);
                    dtSetting.Rows.Clear();
                }
            }
            foreach (ReconTemplate template in newTemplates)
            {
                reconPreferences.DictReconTemplates.Add(template.TemplateKey, template);
            }
        }

        private static List<int> GetAccountList(SerializableDictionary<int, string> serializableDictionary)
        {
            List<int> accountList = new List<int>();
            try
            {
                foreach (int account in serializableDictionary.Keys)
                {
                    accountList.Add(account);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountList;

        }



        /// <summary>
        /// Create Recon Filters Preference for recon.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1384
        /// save separate preference for recon filters in all client folders for first time
        /// </summary>
        /// <param name="dictReconFilters"></param>
        private static void CreateReconFiltersPreference(SerializableDictionary<string, ReconFilters> dictReconFilters)
        {
            try
            {
                foreach (KeyValuePair<int, string> ClientKvp in CachedDataManager.GetInstance.GetAllUsersName())
                {
                    string reconFiltersFilepath = _reconPreferencesDirectoryPath + "\\" + ClientKvp.Key + "\\ReconFilters.xml";
                    //if (!File.Exists(reconFiltersFilepath))
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(reconFiltersFilepath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(reconFiltersFilepath));
                        }
                        using (XmlTextWriter xmlwriter = new XmlTextWriter(reconFiltersFilepath, Encoding.UTF8))
                        {
                            xmlwriter.Formatting = Formatting.Indented;
                            XmlSerializer serialize;
                            serialize = new XmlSerializer(typeof(SerializableDictionary<string, ReconFilters>));
                            serialize.Serialize(xmlwriter, dictReconFilters);
                            xmlwriter.Flush();
                        }
                        //XMLUtilities.SerializeToXMLFile<SerializableDictionary<string, ReconFilters>>(dictReconFilters, reconFiltersFilepath);
                    }
                }
            }
            #region Catch
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
            #endregion
        }

        /// <summary>
        /// Checks if template is to be updated or not from check to update recon preference file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsTemplatesToBeUpdated()
        {
            try
            {
                string filePath = Application.StartupPath + @"\MappingFiles\ReconRulesFile\CheckToUpdateReconPreferences.xml";

                if (File.Exists(filePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(filePath);
                    //ds = XMLUtilities.ReadXmlUsingBufferedStream(filePath);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["IsUpdateReconPreferences"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //update the xml file with false value so that it cannot run more than once
                        ds.Tables[0].Rows[0]["IsUpdateReconPreferences"] = "false";
                        ds.WriteXml(filePath);
                        return true;
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
            return false;
        }

        /// <summary>
        /// update properties in this method which are to be added or updated in previous preference file
        /// </summary>
        /// <param name="reconTemplate"></param>
        /// <returns></returns>
        private static ReconTemplate UpdatePropertiesInPrefrenceFile(ReconTemplate reconTemplate, string templateKey)
        {
            try
            {
                if (string.IsNullOrEmpty(reconTemplate.TemplateName))
                {
                    reconTemplate.TemplateName = ReconPreferences.GetTemplateNameFromTemplateKey(templateKey);
                }
                //TO rename the dictionary structure if the key loaded in file is not correct
                string templatekey = ReconUtilities.GetTemplateKeyFromParameters(reconTemplate.ClientID.ToString(), reconTemplate.ReconType.ToString(), reconTemplate.TemplateName);
                if (string.IsNullOrEmpty(reconTemplate.TemplateKey) || !reconTemplate.TemplateKey.Equals(templatekey, StringComparison.InvariantCultureIgnoreCase))
                {
                    reconTemplate.TemplateKey = ReconUtilities.GetTemplateKeyFromParameters(reconTemplate.ClientID.ToString(), reconTemplate.ReconType.ToString(), reconTemplate.TemplateName);
                }
                reconTemplate.XsltPath = Path.GetFileName(reconTemplate.XsltPath);
                #region Update XmlMatchingRule schema every time file is updated.
                //PRANA-5940    Tolerance preference not working in Recon
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1993
                //Update previous production user preference to update absolute difference tolerance value to double                
                if (reconTemplate.DsMatchingRules != null && reconTemplate.DsMatchingRules.Tables.Count > 0 && reconTemplate.DsMatchingRules.Tables["Parameter"].Columns.Contains("AbsoluteDifference"))
                //   && reconTemplate.DsMatchingRules.Tables["Parameter"].Columns["AbsoluteDifference"].DataType != typeof(double))
                {
                    string xmlMatchingRuleSchema = ReconPreferences.XmlRulePath + "//XmlMatchingRule.xsd";
                    DataSet dsMatchingRuleDefault = new DataSet();
                    dsMatchingRuleDefault.ReadXmlSchema(xmlMatchingRuleSchema);
                    //DataSet dsMatchingRuleDefault = reconTemplate.DsMatchingRules.Clone();
                    dsMatchingRuleDefault.Tables["Parameter"].Columns["AbsoluteDifference"].DataType = typeof(double);
                    foreach (DataTable dt in reconTemplate.DsMatchingRules.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dsMatchingRuleDefault.Tables[dt.TableName].ImportRow(row);
                        }
                    }
                    reconTemplate.DsMatchingRules = dsMatchingRuleDefault;
                    //reconTemplate.DsMatchingRules.Tables["Parameter"].Columns.Add("TempColumn", typeof(double));
                    //foreach (DataRow dr in reconTemplate.DsMatchingRules.Tables["Parameter"].Rows)
                    //{
                    //    dr["TempColumn"] = dr["AbsoluteDifference"];
                    //}
                    //reconTemplate.DsMatchingRules.Tables["Parameter"].Columns.Remove("AbsoluteDifference");
                    //reconTemplate.DsMatchingRules.Tables["Parameter"].Columns["TempColumn"].ColumnName = "AbsoluteDifference";
                }
                #endregion
                return reconTemplate;
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



        public static bool SavePreferences(ReconPreferences reconPreferences)
        {
            bool _isChangesSaved = true;
            try
            {
                if (reconPreferences != null)
                {
                    //reload full prefernces before saving...
                    GetPreferences();
                    //update any preferences dirty for saving...
                    //foreach (KeyValuePair<string, ReconTemplate> templatePair in reconPreferences.newReconTemplates)
                    //{
                    //    _oldReconPreferences.DictReconTemplates.Add(templatePair.Key,templatePair.Value);
                    //}
                    if (_oldReconPreferences.DictReconTemplates.Count > 0)
                    {
                        _oldReconPreferences.Update(reconPreferences);
                    }
                    else
                    {
                        _oldReconPreferences = reconPreferences;
                    }
                    reconPreferences.listDeletedTemplates.Clear();
                    //reconPreferences.newReconTemplates.Clear();
                    //if (reconPreferences != null)
                    //{
                    //    _oldReconPreferences = reconPreferences;
                    //}
                    //CHMW-3006	[Recon] When multiple users are working then sometimes on saving recon preference, error comes and preference doesn't save
                    if (Prana.Utilities.IO.File.IsFileOpen(_reconPreferencesFilePath))
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(_reconPreferencesFilePath, Encoding.UTF8))
                        {
                            writer.Formatting = Formatting.Indented;
                            XmlSerializer serializer;
                            serializer = new XmlSerializer(typeof(ReconPreferences));
                            serializer.Serialize(writer, _oldReconPreferences);
                            writer.Flush();
                        }
                    }
                    else
                    {
                        _isChangesSaved = false;
                    }
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-1384
                    //separately save user preference of recon filters in logged in user folder folder
                    SerializableDictionary<string, ReconFilters> dictReconFilters = new SerializableDictionary<string, ReconFilters>();
                    foreach (KeyValuePair<string, ReconTemplate> kvp in reconPreferences.DictReconTemplates)
                    {
                        if (!dictReconFilters.ContainsKey(kvp.Key))
                        {
                            dictReconFilters.Add(kvp.Key, kvp.Value.ReconFilters);
                        }
                    }
                    string filtersFilepath = _reconPreferencesDirectoryPath + "\\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\ReconFilters.xml";
                    if (!Directory.Exists(Path.GetDirectoryName(filtersFilepath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filtersFilepath));
                    }
                    //CHMW-3006	[Recon] When multiple users are working then sometimes on saving recon preference, error comes and preference doesn't save
                    if (Prana.Utilities.IO.File.IsFileOpen(filtersFilepath))
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(filtersFilepath, Encoding.UTF8))
                        {
                            writer.Formatting = Formatting.Indented;
                            XmlSerializer serializer;
                            serializer = new XmlSerializer(typeof(SerializableDictionary<string, ReconFilters>));
                            serializer.Serialize(writer, dictReconFilters);
                            writer.Flush();
                        }
                    }
                    else
                    {
                        _isChangesSaved = false;
                    }
                    if (prefsSaved != null)
                    {
                        prefsSaved(null, null);
                    }
                }


            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return _isChangesSaved;
        }



        public static DataTable GetAssets()
        {
            Dictionary<int, string> dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
            DataTable dtAssets = new DataTable();
            try
            {
                dtAssets.Columns.Add("Value");
                dtAssets.Columns.Add("Data");
                object[] rowAsset = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictAssets)
                {
                    if (!keyVal.Value.Equals("Cash"))
                    {
                        rowAsset[0] = keyVal.Key;
                        rowAsset[1] = keyVal.Value;
                        dtAssets.Rows.Add(rowAsset);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtAssets;
        }

        /// <summary>
        /// Created by: Pranay Deep
        /// This method is used to get the datatable of grouping criteria.
        /// This methed is used for making datatable from DictGroupingCriteria dictionary.
        /// </summary>
        /// <returns> dtGroupingCriteria</returns>
        public static DataTable GetGroupingCriteria(ReconTemplate template)
        {
            DataTable dtGroupingCriteria = new DataTable();
            try
            {
                dtGroupingCriteria.Columns.Add("Key");
                dtGroupingCriteria.Columns.Add("Value");
                object[] rowGroupingCriteria = new object[2];
                foreach (KeyValuePair<string, bool> keyVal in template.GroupingCrieria.DictGroupingCriteria)
                {
                    rowGroupingCriteria[0] = keyVal.Key;
                    rowGroupingCriteria[1] = keyVal.Value;
                    dtGroupingCriteria.Rows.Add(rowGroupingCriteria);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtGroupingCriteria;
        }

        public static DataTable GetAUECs()
        {
            Dictionary<int, string> dictAuecs = CommonDataCache.CachedDataManager.GetInstance.GetAllAuecs();
            DataTable dtAUECs = new DataTable();
            try
            {
                dtAUECs.Columns.Add("Value");
                dtAUECs.Columns.Add("Data");
                object[] rowAUEC = new object[2];
                foreach (KeyValuePair<int, string> kvpAuec in dictAuecs)
                {
                    string[] auecdetails = (kvpAuec.Value).Split(',');
                    //int auecID = kvpAuec.Key;

                    string Asset = CachedDataManager.GetInstance.GetAssetText(Convert.ToInt16(auecdetails[0].ToString()));
                    string Underlying = CachedDataManager.GetInstance.GetUnderLyingText(Convert.ToInt16(auecdetails[1].ToString()));
                    string Exchange = CachedDataManager.GetInstance.GetExchangeText(Convert.ToInt16(auecdetails[2].ToString()));
                    string DefaultCurrency = CachedDataManager.GetInstance.GetCurrencyText(Convert.ToInt16(auecdetails[3].ToString()));

                    StringBuilder AUECstring = new StringBuilder();
                    AUECstring.Append(Asset + "/");
                    AUECstring.Append(Underlying + "/");
                    AUECstring.Append(Exchange + "/");
                    AUECstring.Append(DefaultCurrency);

                    rowAUEC[0] = kvpAuec.Key;
                    rowAUEC[1] = AUECstring.ToString();
                    dtAUECs.Rows.Add(rowAUEC);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtAUECs;
        }

        public static DataTable GetAccounts()
        {
            Dictionary<int, string> dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
            DataTable dtAccounts = new DataTable();
            try
            {
                dtAccounts.Columns.Add("Value");
                dtAccounts.Columns.Add("Data");
                object[] rowAccount = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictAccounts)
                {
                    rowAccount[0] = keyVal.Key;
                    rowAccount[1] = keyVal.Value;
                    dtAccounts.Rows.Add(rowAccount);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtAccounts;
        }

        public static DataTable GetMasterFunds()
        {
            Dictionary<int, string> dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
            DataTable dtAccounts = new DataTable();
            try
            {
                dtAccounts.Columns.Add("Value");
                dtAccounts.Columns.Add("Data");
                object[] rowAccount = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictMasterFunds)
                {
                    rowAccount[0] = keyVal.Key;
                    rowAccount[1] = keyVal.Value;
                    dtAccounts.Rows.Add(rowAccount);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtAccounts;
        }
        public static DataTable GetPrimeBrokers()
        {
            List<GenericNameID> reconDataSourcecol = CachedDataManager.GetInstance.GetAllPrimeBrokers();
            DataTable dtPrimeBrokers = new DataTable();
            try
            {
                dtPrimeBrokers.Columns.Add("Value");
                dtPrimeBrokers.Columns.Add("Data");
                object[] rowAccount = new object[2];
                foreach (GenericNameID genID in reconDataSourcecol)
                {

                    rowAccount[0] = genID.ID;
                    rowAccount[1] = genID.Name;
                    dtPrimeBrokers.Rows.Add(rowAccount);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtPrimeBrokers;
        }

        public static DataTable GetBrokers()
        {
            Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties();
            DataTable dtBroker = new DataTable();
            try
            {
                dtBroker.Columns.Add("Value");
                dtBroker.Columns.Add("Data");
                object[] rowBroker = new object[2];
                foreach (KeyValuePair<int, string> keyVal in dictBrokers)
                {
                    rowBroker[0] = keyVal.Key;
                    rowBroker[1] = keyVal.Value;
                    dtBroker.Rows.Add(rowBroker);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtBroker;
        }





        public static void SetupLaunchForm(EventHandler LaunchForm)
        {
            _launchForm = LaunchForm;
        }

        public static EventHandler GetLaunchForm()
        {
            return _launchForm;
        }

    }


}
