using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using static Prana.Global.ApplicationConstants;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IThirdPartyService : IServiceOnDemandStatus
    {
        [OperationContract]
        void SyncThirdPartyFileWithDataBase();

        [OperationContract]
        ThirdPartyBatch View(ThirdPartyBatch batch, DateTime runDate, bool isViewClicked);

        [OperationContract]
        List<ThirdPartyBatch> LoadThirdPartyBacthes(DateTime runDate);

        [OperationContract]
        ThirdParty GetThirdParty(int thirdPartyID);

        [OperationContract]
        ThirdPartyFtp GetThirdPartyFtp(int ftpId);

        [OperationContract]
        ThirdPartyGnuPGs GetThirdPartyGnuPGForDecryption(int gnuPGId);

        [OperationContract]
        ThirdPartyEmail GetThirdPartyEmail(int ftpId);

        [OperationContract]
        List<ThirdPartyType> GetThirdPartyTypes();

        [OperationContract]
        List<ThirdParty> GetCompanyThirdParties_DayEnd(int companyID);

        [OperationContract]
        List<ThirdParty> GetCompanyThirdPartyTypeParty(int companyID, int thirdPartyTypeID);

        [OperationContract]
        List<ThirdPartyPermittedAccount> GetThirdPartyPermittedAccounts(int thirdPartyID);

        [OperationContract]
        List<ThirdPartyPermittedAccount> GetThirdPartyAccounts(int companyID, int userID);

        [OperationContract]
        List<ThirdPartyFileFormat> GetCompanyThirdPartyFileFormats(int companyThirdPartyId);

        [OperationContract]
        List<ThirdPartyFileFormat> GetCompanyThirdPartyTypeFileFormats(int companyThirdPartyId);

        [OperationContract]
        ThirdPartyFileFormat GetFormat(ThirdPartyBatch batch);

        [OperationContract]
        void ExportFile(ThirdPartyBatch batch, DateTime runDate);

        [OperationContract]
        bool SendFile(ThirdPartyBatch batch, DateTime runDate, string loggedInUser);

        [OperationContract]
        List<ThirdParty> GetThirdParties(ThirdPartyBatch batch);

        [OperationContract]
        List<ThirdPartyFileFormat> GetThirdPartyFormats(ThirdPartyBatch batch);

        [OperationContract]
        List<ThirdPartyBatch> GetThirdPartyBatch();

        [OperationContract]
        int SaveThirdPartyFtp(ThirdPartyFtp ftp);

        [OperationContract]
        int SaveThirdPartyGnuPG(ThirdPartyGnuPG gnuPG);

        [OperationContract]
        void AutoCreateBatchEntries();

        [OperationContract]
        int SaveThirdPartyEmail(ThirdPartyEmail email);

        [OperationContract]
        void DeleteThirdPartyBatch(ThirdPartyBatch batch);

        [OperationContract]
        int DeleteThirdPartyFtp(ThirdPartyFtp ftp);

        [OperationContract]
        int DeleteThirdPartyGnuPG(ThirdPartyGnuPG gnuPG);

        [OperationContract]
        int DeleteThirdPartyEmail(ThirdPartyEmail email);

        [OperationContract]
        List<ThirdPartyFtp> GetThirdPartyFtps();

        [OperationContract]
        List<ThirdPartyGnuPG> GetThirdPartyGnuPG();

        [OperationContract]
        List<ThirdPartyEmail> GetThirdPartyEmails();

        [OperationContract]
        List<ThirdPartyEmail> GetThirdPartyLogEmail();

        [OperationContract]
        List<ThirdPartyEmail> GetThirdPartyDataEmail();

        [OperationContract]
        bool CheckDuplicateBatch(ThirdPartyBatch batch);

        [OperationContract]
        int SaveThirdPartyBatch(ThirdPartyBatch batch);

        [OperationContract]
        void LogMessage(string file, string message);

        [OperationContract]
        string GetServerPath();

        /// <summary>
        /// This method encrypts and sends file for given batch
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="rundate"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        [OperationContract]
        bool EncryptAndSendFile(ThirdPartyBatch batch, DateTime rundate, string loggedInUser);

        /// <summary>
        /// This method is to check for unallocated trades
        /// </summary>
        /// <param name="tradedate"></param>
        /// <returns>true if there are unallocated trades, otherwise false</returns>
        [OperationContract]
        bool CheckForUnallocatedTrades(DateTime tradedate);

        /// <summary>
        /// Gets Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ThirdPartyForceConfirm> GetThirdPartyForceConfirmAuditData(int thirdPartyBatchId, DateTime runDate);

        /// <summary>
        /// Saves Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        int SaveThirdPartyForceConfirmAuditData(List<ThirdPartyForceConfirm> data, ThirdPartyBatch batch);

        /// <summary>
        /// This method is to get block allocation details
        /// </summary>
        /// <param name="thirdPartyBatchid"></param>
        /// <param name="runDate"></param>
        /// <returns>List of Block Level Details if available, otherwise empty list</returns>
        [OperationContract]
        List<ThirdPartyBlockLevelDetails> GetBlockAllocationDetails(int thirdPartyBatchid, DateTime runDate);

        /// <summary>
        /// This method is to get Force Confirm Details
        /// </summary>
        /// <param name="allocationId"></param>
        /// <param name="broker"></param>
        /// <returns></returns>
        [OperationContract]
        List<ThirdPartyForceConfirm> GetForceConfirmDetails(string allocationId, string broker);

        [OperationContract]
        bool SendAUMsg(List<string> allocationIdList, int counterPartyID, int affirmStatus);

        /// <summary>
        /// This method sends AT msg for specified allocId and allocStatus
        /// </summary>
        /// <param name="allocIdAllocReportIdPairs"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="allocStatus"></param>
        /// <returns></returns>
        [OperationContract]
        bool SendATMsg(Dictionary<string, string> allocIdAllocReportIdPairs, int counterPartyID, int allocStatus);


        /// <summary>
        /// This method provides the Third Party Batch Event Data
        /// </summary>
        /// <param name="blockId"></param>
        [OperationContract]
        List<ThirdPartyBatchEventData> GetThirdPartyBatchEventData(int blockId);

        /// <summary>
        /// This method checks if there is a mismatch in groups and asks for user confirmation to send
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="runDate"></param>
        /// <returns></returns>
        [OperationContract]
        bool CheckForMismatchAndGetConfirmation(ThirdPartyBatch batch, DateTime runDate);

        #region Tolerance Profile
        /// <summary>
        /// This method will Delete the Third Party Tolerance Profile Data
        /// </summary>
        /// <param name="ThirdPartyToleranceProfileId"></param>
        [OperationContract]
        void DeleteThirdPartyToleranceProfile(int ThirdPartyToleranceProfileId);

        /// <summary>
        /// This method will Update the Third Party Tolerance Profile Data
        /// </summary>
        /// <param name="toleranceProfile"></param>
        [OperationContract]
        void UpdateThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile);

        /// <summary>
        /// This method will Get the Third Party Tolerance Profile Data
        /// </summary>
        /// <param name="thirdPartyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ThirdPartyToleranceProfile> GetThirdPartyToleranceProfiles();

        /// <summary>
        /// This method will Save the Third Party Tolerance Profile Data
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OperationContract]
        int SaveThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile);

        /// <summary>
        /// This method will Get the Third Party Tolerance Profile Common Data
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ThirdPartyToleranceProfileCommon> GetToleranceProfileCommonData();
        #endregion

        /// <summary>
        /// This method is to get AllocationMatchStatus for a batch
        /// </summary>
        /// <param name="thirdPartyBatchId"></param>
        /// <param name="date"></param>
        /// <param name="isFileTransmission"></param>
        /// <returns>AllocationMatchStatus</returns>
        [OperationContract]
        AllocationMatchStatus GetAllocationMatchStatusForBatch(int thirdPartyBatchId, string date, bool isFileTransmission);

        /// <summary>
        /// This method provides the Third Party File Logs 
        /// </summary>
        /// <param name="runDate"></param>
        [OperationContract]
        List<ThirdPartyFileLog> GetThirdPartyFileLogs(DateTime runDate);
    }
}
