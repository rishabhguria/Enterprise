using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.BusinessLogic;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prana.ThirdPartyManager.Helpers
{
    public class Setup
    {
        /// <summary>
        /// Gets or sets the third party id.
        /// </summary>
        /// <value>The third party id.</value>
        /// <remarks></remarks>
        public int ThirdPartyId;
        /// <summary>
        /// Gets or sets the third party type id.
        /// </summary>
        /// <value>The third party type id.</value>
        /// <remarks></remarks>
        public int ThirdPartyTypeId;
        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        /// <remarks></remarks>
        public int CompanyId;
        /// <summary>
        /// Gets or sets the format id.
        /// </summary>
        /// <value>The format id.</value>
        /// <remarks></remarks>
        public int FormatId;
        /// <summary>
        /// Gets or sets the third party company id.
        /// </summary>
        /// <value>The third party company id.</value>
        /// <remarks></remarks>
        public int ThirdPartyCompanyId;


        /// <summary>
        /// Gets a value indicating whether this instance is all parties.
        /// </summary>
        /// <remarks></remarks>
        public bool IsAllParties
        {
            get
            {
                return (ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties);
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is exec broker.
        /// </summary>
        /// <remarks></remarks>
        public bool IsExecBroker
        {
            get
            {
                return (ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker);
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is prime broker.
        /// </summary>
        /// <remarks></remarks>
        public bool IsPrimeBroker
        {
            get
            {
                return (ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer);
            }
        }

        /// <summary>
        /// Creates the tables.
        /// </summary>
        /// <remarks></remarks>
        public void ExecuteScripts(string filter)
        {
            try
            {
                string[] tables = Directory.GetFiles(".\\Scripts", filter);

                foreach (string file in tables)
                {
                    CreateTable(Path.GetFileName(file));
                }
            }
            #region Catch
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
            #endregion
        }
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <remarks></remarks>
        public void CreateTable(string script)
        {
            string sql = File.ReadAllText(string.Format(".\\Scripts\\{0}", script));
            string[] datasets = Regex.Split(sql, @"\b" + "GO" + @"\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (string dataset in datasets)
            {
                try
                {
                    QueryData queryData = new QueryData();
                    queryData.Query = dataset;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                }
                #region Catch
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
                #endregion

            }


        }


        /// <summary>
        /// Autoes the create batch entries.
        /// </summary>
        /// <remarks></remarks>
        public void AutoCreateBatchEntries()
        {
            CompanyId = CommonDataCache.CachedDataManager.GetInstance.GetCompanyID(); ;

            List<ThirdParty> thirdParties;

            List<ThirdPartyType> thirdPartyTypes = ThirdPartyDataManager.GetThirdPartyTypes();

            foreach (ThirdPartyType type in thirdPartyTypes)
            {
                ThirdPartyTypeId = type.ThirdPartyTypeID;

                if (IsPrimeBroker)
                    thirdParties = ThirdPartyDataManager.GetCompanyThirdParties_DayEnd(CompanyId);
                else
                    thirdParties = ThirdPartyDataManager.GetCompanyThirdPartyTypeParty(CompanyId, ThirdPartyTypeId);

                foreach (ThirdParty a in thirdParties)
                {
                    ThirdPartyId = a.ThirdPartyID;
                    ThirdPartyCompanyId = a.CompanyThirdPartyID;

                    List<ThirdPartyFileFormat> formats;

                    if (IsPrimeBroker)
                        formats = ThirdPartyDataManager.GetCompanyThirdPartyFileFormats(ThirdPartyCompanyId);
                    else
                        formats = ThirdPartyDataManager.GetCompanyThirdPartyTypeFileFormats(ThirdPartyCompanyId);

                    foreach (ThirdPartyFileFormat format in formats)
                    {
                        try
                        {
                            FormatId = format.FileFormatId;
                            bool? allowedFixTransmission = null;
                            if (format.FIXEnabled)
                            {
                                allowedFixTransmission = true;
                            }

                            string name = String.Format("{0}.{1}.{2}", type.ThirdPartyTypeName.Replace("PrimeBrokerClearer", "PB").Replace("ExecutingBroker", "EB").Replace("AllDataParties", "All"), a.ThirdPartyName, format.FileFormatName);

                            ThirdPartyBatch batch = new ThirdPartyBatch()
                            {
                                Description = name,
                                ThirdPartyTypeId = ThirdPartyTypeId,
                                ThirdPartyId = ThirdPartyId,
                                ThirdPartyFormatId = FormatId,
                                IsLevel2Data = false,
                                Active = true,
                                ThirdPartyCompanyId = ThirdPartyCompanyId,
                                FileEnabled = format.FileEnabled,
                                AllowedFixTransmission = allowedFixTransmission
                            };
                            ThirdPartyDataManager.SaveThirdPartyBatchForRefresh(batch);

                        }
                        catch (Exception ex)
                        {
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                            if (rethrow)
                            {
                                throw;
                            }
                            continue;
                        }
                    }
                }
            }

            ThirdPartyTimedBatchHelper.SetTimedBatchesScheduler();

            ThirdPartyCache.AutomatedBatchStatus = ThirdPartyLogic.LoadThirdPartyAutomatedBatchStatus();

            const string removeDeletedBatches = "Delete From T_ThirdPartyBatch Where ThirdPartyFormatId Not In (Select FileFormatId From T_ThirdPartyFileFormat)";

            QueryData query = new QueryData();
            query.Query = removeDeletedBatches;

            DatabaseManager.DatabaseManager.ExecuteNonQuery(query);
        }
    }
}