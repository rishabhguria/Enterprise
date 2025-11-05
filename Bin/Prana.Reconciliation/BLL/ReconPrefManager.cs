using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using Prana.CommonDataCache;
using Prana.Global;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects;
using System.Windows.Forms;

namespace Prana.Reconciliation
{
    public class ReconPrefManager
    {
        //TODO: As there are now only one rule, so rulesDict will now have MatchinRule as value instead of List

        public static string xmlRulePath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString();


        static string _reconPreferencesFilePath = string.Empty;
        static string _reconPreferencesDirectoryPath = string.Empty;
        static ReconPreferences _oldReconPreferences = null;
        static string _startPath = Application.StartupPath;
        static int _userID = int.MinValue;
        public const string NirvanaNameAppend = " (Nirvana)";
        public const string PBNameAppend = " (PB)";
        public const string CommonNameAppend = " (Common)";
        public const string DiffNameAppend = " (Diff)";

        public static event EventHandler prefsSaved;

        private static void LoadDefaultDataForTemplate(ReconTemplate template)
        {
            //Load master columns and matching rules from xsd and xml files
            #region ReconFilters
            #endregion
            #region Matching Rules
            DataSet dsMatchingRulesDefault = new DataSet();          
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
            DataTable dtPBMasterColumns = GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, DataSourceType.PrimeBroker);
            dtPBMasterColumns.TableName = ReconConstants.TABLENAME_PBGridMasterColumns;
            //Load master columns which have datasouce Nirvana
            DataTable dtNirvanaMasterColumns = GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, DataSourceType.Nirvana);
            dtNirvanaMasterColumns.TableName = ReconConstants.TABLENAME_NirvanaGridColumns;
            template.DsMasterColumns.Tables.Clear();
            //Add nirvana and pb tables to the template
            template.DsMasterColumns.Tables.Add(dtNirvanaMasterColumns);
            template.DsMasterColumns.Tables.Add(dtPBMasterColumns);
            //For New Grouping Logic for PNL
            //get grouping summary for the PNL
            template.DictGroupingSummary = GetGroupingSummary(dsMasterColumnsDefault,template.ReconType);
            #endregion
            #region Recon Report Layout
            //clear all the list which are used in the exception report layout
            template.AvailableColumnList.Clear();
            template.SelectedColumnList.Clear();
            template.ListGroupByColumns.Clear();
            template.ListSortByColumns.Clear();
            //Load default columns for the exception report layout
            GetDefaultSelectedColumnsForExceptionReport(template,dsMasterColumnsDefault);
            #endregion
        }
        //Add default template when no preference are saved
        public static void AddDefaultTemplate(string reconType, string templateName)
        {
            try
            {
                //TODO: Here clientid 0 is hardcoded
                ReconTemplate template = _oldReconPreferences.AddDefaultTemplate(0,reconType, templateName);
                LoadDefaultDataForTemplate(template);
            }
            #region Catch
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
            #endregion
        }
        //copy template
        public static void copyTemplate(string reconType, string newTemplateName, string template)
        {
            try
            {
                ReconTemplate templateToCopy = _oldReconPreferences.GetTemplates(template);
                //int hashcodeold = templateToCopy.GetHashCode();
                ReconTemplate newTemplate = (ReconTemplate)templateToCopy.Clone();
                newTemplate.IsDirtyForSaving = true;
                //int newCode = newTemplate.GetHashCode();
                //ReconPreferences.newReconTemplates.Add(newTemplateName, newTemplate);
                _oldReconPreferences.UpdateTemplates(newTemplateName, newTemplate);
            }
            #region Catch
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
            #endregion
        }

        public static void RemoveRowsBasedOnTemplateReconType(ReconType reconType, DataSet dsMatchingRules)
        {
            try
            {
                DataTable dtRules = dsMatchingRules.Tables[0];
                string idColumn = dtRules.Columns[4].Caption;
                string reconTypeColumn = dtRules.Columns[1].Caption;
                string rowsToBind = string.Empty;
                //Remove duplicate rows from the table so that only distict row exists
                DataTable dtDistinctTable = dsMatchingRules.Tables[1].DefaultView.ToTable(/*distinct*/ true);
                dsMatchingRules.Tables[1].Clear();
                dsMatchingRules.Tables[1].Merge(dtDistinctTable);
                foreach (DataRow dr in dtRules.Rows)
                {
                    //((int)(ReconType)(Enum.Parse(typeof(ReconType),dr[reconTypeColumn].ToString()))
                    if ((!(dr[ReconConstants.COLUMN_ReconType].ToString().Equals(string.Empty))) && (int.Parse(dr[ReconConstants.COLUMN_ReconType].ToString()) == ((int)reconType)))
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
        //For New Grouping Logic for PNL
        public static SerializableDictionary<string, string> GetGroupingSummary(DataSet dsMain, ReconType recontype)
        {
            SerializableDictionary<string, string> dictGroupingSummary = new SerializableDictionary<string, string>();
            if (dsMain.Tables.Count > 0)
            {
                foreach (DataRow Row in dsMain.Tables[1].Rows)
                {
                    //check for recon type and contains key check for the dictionary
                    if (Row[ReconConstants.COLUMN_ReconId].ToString().Equals(((int)(recontype)).ToString()) && (!dictGroupingSummary.ContainsKey(Row[ReconConstants.COLUMN_Name].ToString())))
                    {
                        dictGroupingSummary.Add(Row[ReconConstants.COLUMN_Name].ToString(), Row[ReconConstants.COLUMN_Summary].ToString());
                    }
                }
            }
            return dictGroupingSummary;
        }
        public static DataTable GetFilteredMasterColumns(DataSet dsMain, ReconType reconType, DataSourceType dsType)
        {
            DataTable dtFiltered = new DataTable();
            try
            {
                if (dsMain.Tables.Count > 0)
                {
                    //changes done Table[0] to Table[1] 13/08/2012
                    dtFiltered = dsMain.Tables[1].Clone();
                    //foreach recon template
                    foreach (DataRow ColumnRow in dsMain.Tables[1].Rows)
                    {
                        //foreach column in template
                        if (((int.Parse(ColumnRow[ReconConstants.COLUMN_DataSourceType].ToString())).Equals(((int)dsType)) || int.Parse(ColumnRow[ReconConstants.COLUMN_DataSourceType].ToString()).Equals((int)DataSourceType.Both)) && (int.Parse(ColumnRow[ReconConstants.COLUMN_ReconId].ToString()).Equals((int)reconType)))
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
            //remove duplicate rows and get only distinct rows
            dtFiltered = dtFiltered.DefaultView.ToTable( /*distinct*/ true);
            return dtFiltered;
        }
        public static void GetDefaultSelectedColumnsForExceptionReport(ReconTemplate template, DataSet dsMasterColumnsDefault)
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
                            //add fundname and symbol to the default sorting list
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
                                    if (rules[0].ComparisonFields.Contains(item.ColumnName) || (item.ColumnName == ReconConstants.CAPTION_MismatchReason) || (item.ColumnName == ReconConstants.CAPTION_MismatchType))
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

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
            _startPath = startUpPath;
            GetPreferences();

        }

        public static ReconPreferences GetPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            ReconPreferences reconPreferences = null;
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
                    using (FileStream fs = File.OpenRead(_reconPreferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ReconPreferences));
                        reconPreferences = (ReconPreferences)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    reconPreferences = new ReconPreferences();// GetDefualtPreferences();
                }
                _oldReconPreferences = reconPreferences;

                return reconPreferences;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion
        }


        public static void SavePreferences(ReconPreferences reconPreferences)
        {
            try
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
                using (XmlTextWriter writer = new XmlTextWriter(_reconPreferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ReconPreferences));
                    serializer.Serialize(writer, _oldReconPreferences);
                    writer.Flush();
                    writer.Close();
                }

                if (prefsSaved != null)
                {
                    prefsSaved(null, null);
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }



        public static DataTable GetAssets()
        {
            Dictionary<int, string> dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
            DataTable dtAssets = new DataTable();
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
            return dtAssets;
        }

        public static DataTable GetAUECs()
        {

            Dictionary<int, string> dictAuecs = CommonDataCache.CachedDataManager.GetInstance.GetAllAuecs();
            DataTable dtAUECs = new DataTable();
            dtAUECs.Columns.Add("Value");
            dtAUECs.Columns.Add("Data");
            object[] rowAUEC = new object[2];
            foreach (KeyValuePair<int, string> kvpAuec in dictAuecs)
            {
                string[] auecdetails = (kvpAuec.Value).Split(',');
                int auecID = kvpAuec.Key;

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
            return dtAUECs;
        }

        public static DataTable GetFunds()
        {
            Dictionary<int, string> dictFunds = CommonDataCache.CachedDataManager.GetInstance.GetFundsWithFullName();
            DataTable dtFunds = new DataTable();
            dtFunds.Columns.Add("Value");
            dtFunds.Columns.Add("Data");
            object[] rowFund = new object[2];
            foreach (KeyValuePair<int, string> keyVal in dictFunds)
            {
                rowFund[0] = keyVal.Key;
                rowFund[1] = keyVal.Value;
                dtFunds.Rows.Add(rowFund);
            }
            return dtFunds;
        }

        public static DataTable GetMasterFunds()
        {
            Dictionary<int, string> dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
            DataTable dtFunds = new DataTable();
            dtFunds.Columns.Add("Value");
            dtFunds.Columns.Add("Data");
            object[] rowFund = new object[2];
            foreach (KeyValuePair<int, string> keyVal in dictMasterFunds)
            {
                rowFund[0] = keyVal.Key;
                rowFund[1] = keyVal.Value;
                dtFunds.Rows.Add(rowFund);
            }
            return dtFunds;
        }
        public static DataTable GetPrimeBrokers()
        {
            List<GenericNameID> reconDataSourcecol = CachedDataManager.GetInstance.GetAllPrimeBrokers();
            DataTable dtPrimeBrokers = new DataTable();
            dtPrimeBrokers.Columns.Add("Value");
            dtPrimeBrokers.Columns.Add("Data");
            object[] rowFund = new object[2];
            foreach (GenericNameID genID in reconDataSourcecol)
            {

                rowFund[0] = genID.ID;
                rowFund[1] = genID.Name;
                dtPrimeBrokers.Rows.Add(rowFund);

            }
            return dtPrimeBrokers;
        }

        public static DataTable GetBrokers()
        {
            Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties();
            DataTable dtBroker = new DataTable();
            dtBroker.Columns.Add("Value");
            dtBroker.Columns.Add("Data");
            object[] rowBroker = new object[2];
            foreach (KeyValuePair<int, string> keyVal in dictBrokers)
            {
                rowBroker[0] = keyVal.Key;
                rowBroker[1] = keyVal.Value;
                dtBroker.Rows.Add(rowBroker);
            }
            return dtBroker;
        }
    }


}
