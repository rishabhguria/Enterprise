using Prana.BusinessObjects;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Prana.ThirdPartyManager
{

    /// <summary>
    /// Third Party Validation
    /// </summary>
    /// <remarks></remarks>
    public class ThirdPartyValidation
    {

        /// <summary>
        /// Determines whether [is valid daylight savings] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is valid daylight savings] [the specified value]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidDaylightSavings(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Determines whether [is valid third party type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is valid third party type] [the specified value]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidThirdPartyType(int value)
        {
            return value != int.MinValue;
        }
        /// <summary>
        /// Determines whether [is valid third party] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is valid third party] [the specified value]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidThirdParty(int value)
        {
            return value != int.MinValue;
        }
        /// <summary>
        /// Determines whether [is valid third party format] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is valid third party format] [the specified value]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidThirdPartyFormat(int value)
        {
            return value != int.MinValue;
        }

        /// <summary>
        /// Verifies the unallocated trades.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool VerifyUnallocatedTrades()
        {
            throw new NotImplementedException();
            //TODO: Load Data and verify
        }


        /// <summary>
        /// Verifies the data.
        /// </summary>
        /// <param name="thirdPartyId">The third party id.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="inputDate">The input date.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="level2Data">if set to <c>true</c> [level2 data].</param>
        /// <param name="auecIds">The auec ids.</param>
        /// <param name="exportOnly">if set to <c>true</c> [export only].</param>
        /// <param name="thirdPartyTypeId">The third party type id.</param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By: Ankit Gupta
        /// ON January 22, 2014
        /// Purpose: To fetch data based on the FileFormatID
        /// </remarks>
        public static ThirdPartyFlatFileDetailCollection FillData(int thirdPartyId, StringBuilder accountIds, DateTime inputDate, int companyID, bool level2Data, StringBuilder auecIds, bool exportOnly, int thirdPartyTypeId, int dateType, int fileFormatID, bool includeSent)
        {
            ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetailCollection = new ThirdPartyFlatFileDetailCollection();
            try
            {
                thirdPartyFlatFileDetailCollection = ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetails(thirdPartyId, accountIds, inputDate, companyID, level2Data, auecIds, exportOnly, thirdPartyTypeId, dateType, fileFormatID, includeSent);
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

            return thirdPartyFlatFileDetailCollection;
        }
        public static DataSet FillData(string storedProcedure, int thirdPartyId, StringBuilder accountIds, DateTime inputDate, int companyId, StringBuilder auecIds, int thirdPartyTypeId, int dateType, int fileFormatID)
        {
            try
            {
                return ThirdPartyFlatFileManager.GetFFThirdPartyAccountDetails_SPCall(storedProcedure, thirdPartyId, accountIds, inputDate, companyId, auecIds, thirdPartyTypeId, dateType, fileFormatID);
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
            return null;
        }

        /// <summary>
        /// Determines whether [is valid delimiter] [the specified delimiter].
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if [is valid delimiter] [the specified delimiter]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidDelimiter(string delimiter, string name)
        {
            return !(string.IsNullOrEmpty(delimiter) || string.IsNullOrEmpty(name));
        }
        /// <summary>
        /// Determines whether [is valid file display name] [the specified file display name].
        /// </summary>
        /// <param name="fileDisplayName">Display name of the file.</param>
        /// <returns><c>true</c> if [is valid file display name] [the specified file display name]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidFileDisplayName(string fileDisplayName)
        {
            return !string.IsNullOrEmpty(fileDisplayName);
        }
        /// <summary>
        /// Determines whether [is valid header file] [the specified header file].
        /// </summary>
        /// <param name="headerFile">The header file.</param>
        /// <returns><c>true</c> if [is valid header file] [the specified header file]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidHeaderFile(string headerFile)
        {
            return !string.IsNullOrEmpty(headerFile);
        }
        /// <summary>
        /// Determines whether [has valid header data] [the specified count].
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns><c>true</c> if [has valid header data] [the specified count]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool HasValidHeaderData(int count)
        {
            return count > 0 ? true : false;
        }
        /// <summary>
        /// Determines whether [is valid naming convention] [the specified file name].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if [is valid naming convention] [the specified file name]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidNamingConvention(string fileName)
        {
            int startIndex = fileName.IndexOf("{");
            int lastIndex = fileName.LastIndexOf("}");

            if (fileName.Contains("{") || fileName.Contains("}"))
            {
                if (startIndex == -1 || lastIndex == -1)
                {
                    return false;
                }
                int lengthOfFile = (lastIndex - startIndex) - 1;
                if (lengthOfFile <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Determines whether [is cache manager running].
        /// </summary>
        /// <returns><c>true</c> if [is cache manager running]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsCacheManagerRunning()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Determines whether [is pricing server running].
        /// </summary>
        /// <returns><c>true</c> if [is pricing server running]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsPricingServerRunning()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Starts the cache manager.
        /// </summary>
        /// <remarks></remarks>
        public static void StartCacheManager()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Starts the pricing server.
        /// </summary>
        /// <remarks></remarks>
        public static void StartPricingServer()
        {
            throw new NotImplementedException();
            //TODO: Start an instance of Pricing Server
        }
    }
}
