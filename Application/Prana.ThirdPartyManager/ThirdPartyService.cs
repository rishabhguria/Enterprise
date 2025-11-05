using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ThirdPartyManager.BusinessLogic;
using Prana.ThirdPartyManager.DataAccess;
using Prana.ThirdPartyManager.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyManager
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class ThirdPartyService : IThirdPartyService
    {
        private string _BatchLogFile = string.Empty;
        private string _BatchDescription = string.Empty;
        private string _BatchRunDataShortDate = string.Empty;

        public ThirdPartyService()
        {
        }

        public void SyncThirdPartyFileWithDataBase()
        {
            try
            {
                FileAndDbSyncManager.SyncFileWithDataBase(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ApplicationConstants.MappingFileType.ThirdPartyXSLT);
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

        public List<ThirdPartyBatch> LoadThirdPartyBacthes(DateTime runDate)
        {
            try
            {
                List<ThirdPartyBatch> btaches = new List<ThirdPartyBatch>();
                ThirdPartyBatches thirdPartyBatches = ThirdPartyLogic.LoadThirdPartyBacthes(runDate);
                foreach (ThirdPartyBatch b in thirdPartyBatches)
                    btaches.Add(b);
                return btaches;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public ThirdParty GetThirdParty(int thirdPartyID)
        {
            try
            {
                return ThirdPartyLogic.GetThirdParty(thirdPartyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public ThirdPartyFtp GetThirdPartyFtp(int ftpId)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyFtp(ftpId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public ThirdPartyGnuPGs GetThirdPartyGnuPGForDecryption(int gnuPGId)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyGnuPGForDecryption(gnuPGId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public ThirdPartyEmail GetThirdPartyEmail(int ftpId)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyEmail(ftpId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyType> GetThirdPartyTypes()
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyTypes();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdParty> GetCompanyThirdParties_DayEnd(int companyID)
        {
            try
            {
                return ThirdPartyLogic.GetCompanyThirdParties_DayEnd(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdParty> GetCompanyThirdPartyTypeParty(int companyID, int thirdPartyTypeID)
        {
            try
            {
                return ThirdPartyLogic.GetCompanyThirdPartyTypeParty(companyID, thirdPartyTypeID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyPermittedAccount> GetThirdPartyPermittedAccounts(int thirdPartyID)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyPermittedAccounts(thirdPartyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyPermittedAccount> GetThirdPartyAccounts(int companyID, int userID)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyAccounts(companyID, userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyFileFormat> GetCompanyThirdPartyFileFormats(int companyThirdPartyId)
        {
            try
            {
                return ThirdPartyLogic.GetCompanyThirdPartyFileFormats(companyThirdPartyId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyFileFormat> GetCompanyThirdPartyTypeFileFormats(int companyThirdPartyId)
        {
            try
            {
                return ThirdPartyLogic.GetCompanyThirdPartyTypeFileFormats(companyThirdPartyId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// This Method is to handle View Operation
        /// </summary>
        /// <param name="batch">The ThirdPartyBatch object.</param>
        /// <param name="runDate">The date of the batch run.</param>
        /// <param name="isViewClicked">Indicates whether the view action was clicked.</param>
        /// <returns>The retrieved ThirdPartyBatch.</returns>
        public ThirdPartyBatch View(ThirdPartyBatch batch, DateTime runDate, bool isViewClicked)
        {
            ThirdPartyBatch resultBatch = batch;
            try
            {
                _BatchDescription = batch.Description;
                _BatchLogFile = batch.LogFile;
                _BatchRunDataShortDate = runDate.ToShortDateString();
                resultBatch = ThirdPartyLogic.View(batch, runDate, isViewClicked, false, OnStatus, OnMessage);
            }            
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return resultBatch;
        }

        /// <summary>
        /// This method is to export file
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="runDate"></param>
        public void ExportFile(ThirdPartyBatch batch, DateTime runDate)
        {
            try
            {
                _BatchDescription = batch.Description;
                _BatchLogFile = batch.LogFile;
                _BatchRunDataShortDate = runDate.ToShortDateString();
                bool isFixTransmissionType = batch.TransmissionType.Equals(((int)TransmissionType.FIX).ToString());

                batch = View(batch, runDate, false);
                if (string.IsNullOrEmpty(batch.SerializedDataSource))
                {
                    if (isFixTransmissionType)
                        OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_EXPORT_FAILED));
                    return;
                }

                if (!isFixTransmissionType)
                    OnStatus(this, new StatusEventArgs("Exporting output file..."));

                ThirdPartyExecutor executor = ThirdPartyLogic.GetCustomisedExecutor(batch, runDate, OnMessage, true);
                if (executor != null)
                {
                    batch.Format.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", batch.Format.FileName);
                    if (ThirdPartyLogic.GenerateFile(batch, executor))
                    {
                        if (isFixTransmissionType)
                            OnStatus(this, new StatusEventArgs(ThirdPartyConstants.STATUS_EXPORT_SUCCESSFUL));
                        else
                            OnStatus(this, new StatusEventArgs("Successfully Exported"));
                    }
                }
                else
                {
                    if (isFixTransmissionType)
                        OnMessage(this, new MessageEventArgs(ThirdPartyConstants.STATUS_EXPORT_FAILED));
                    else
                        OnMessage(this, new MessageEventArgs("Problem in Exporting output file"));
                }
            }
            catch (Exception ex)
            {
                OnMessage(this, new MessageEventArgs("Problem in Exporting output file"));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name="batch">The ThirdPartyBatch object.</param>
        /// <param name="runDate">The date of the batch run.</param>
        /// <param name="loggedInUser">The username of the logged-in user.</param>
        public bool SendFile(ThirdPartyBatch batch, DateTime runDate, string loggedInUser)
        {
            bool isSuccessful = false;
            try
            {
                _BatchDescription = batch.Description;
                _BatchLogFile = batch.LogFile;
                _BatchRunDataShortDate = runDate.ToShortDateString();
                isSuccessful = ThirdPartyLogic.SendFile(batch, runDate, loggedInUser, false, OnStatus, OnMessage);
            }
            catch (Exception ex)
            {             
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccessful;
        }         

        /// <summary>
        /// Gets the third parties.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<ThirdParty> GetThirdParties(ThirdPartyBatch batch)
        {
            List<ThirdParty> thirdParties = null;
            try
            {
                if (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                    thirdParties = ThirdPartyLogic.GetCompanyThirdParties_DayEnd(batch.CompanyId);
                else
                    thirdParties = ThirdPartyLogic.GetCompanyThirdPartyTypeParty(batch.CompanyId, batch.ThirdPartyTypeId);

                thirdParties.Insert(0, new ThirdParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return thirdParties;
        }

        /// <summary>
        /// Gets the third party formats.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<ThirdPartyFileFormat> GetThirdPartyFormats(ThirdPartyBatch batch)
        {
            if (batch.ThirdPartyTypeId < 0) return null;

            List<ThirdPartyFileFormat> thirdPartyFileFormats = null;
            try
            {
                if (batch.ThirdPartyTypeId == (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer)
                    thirdPartyFileFormats = ThirdPartyLogic.GetCompanyThirdPartyFileFormats(batch.ThirdPartyCompanyId);
                else
                    thirdPartyFileFormats = ThirdPartyLogic.GetCompanyThirdPartyTypeFileFormats(batch.ThirdPartyId);

                thirdPartyFileFormats.Insert(0, new ThirdPartyFileFormat(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyFileFormats;
        }

        

        public List<ThirdPartyBatch> GetThirdPartyBatch()
        {
            try
            {
                List<ThirdPartyBatch> btaches = new List<ThirdPartyBatch>();
                ThirdPartyBatches thirdPartyBatches = ThirdPartyLogic.GetThirdPartyBatch();
                foreach (ThirdPartyBatch b in thirdPartyBatches)
                    btaches.Add(b);
                return btaches;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return null;
        }

        public int SaveThirdPartyFtp(ThirdPartyFtp ftp)
        {
            try
            {
                return ThirdPartyLogic.SaveThirdPartyFtp(ftp);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }

        public int SaveThirdPartyGnuPG(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                return ThirdPartyLogic.SaveThirdPartyGnuPG(gnuPG);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return int.MinValue;
        }

        public Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }

        public void AutoCreateBatchEntries()
        {
            try
            {
                new Setup().AutoCreateBatchEntries();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public int SaveThirdPartyEmail(ThirdPartyEmail email)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyEmail(email);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public void DeleteThirdPartyBatch(ThirdPartyBatch batch)
        {
            try
            {
                ThirdPartyDataManager.DeleteThirdPartyBatch(batch);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public int DeleteThirdPartyFtp(ThirdPartyFtp ftp)
        {
            try
            {
                return ThirdPartyDataManager.DeleteThirdPartyFtp(ftp);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public int DeleteThirdPartyGnuPG(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                return ThirdPartyDataManager.DeleteThirdPartyGnuPG(gnuPG);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public int DeleteThirdPartyEmail(ThirdPartyEmail email)
        {
            try
            {
                return ThirdPartyDataManager.DeleteThirdPartyEmail(email);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        public List<ThirdPartyFtp> GetThirdPartyFtps()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyFtps();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyGnuPG> GetThirdPartyGnuPG()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyGnuPG();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyEmail> GetThirdPartyEmails()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyEmail();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyEmail> GetThirdPartyLogEmail()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyLogEmail();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public List<ThirdPartyEmail> GetThirdPartyDataEmail()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyDataEmail();
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public bool CheckDuplicateBatch(ThirdPartyBatch batch)
        {
            try
            {
                return ThirdPartyDataManager.CheckDuplicateBatch(batch);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public int SaveThirdPartyBatch(ThirdPartyBatch batch)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyBatch(batch);
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                messageEventArgs.LogFile = _BatchLogFile;
                messageEventArgs.Description = _BatchDescription;

                var statusFormattedMessage = string.Empty;
                if (!string.IsNullOrEmpty(messageEventArgs.LogFile) && !string.IsNullOrEmpty(messageEventArgs.Description))
                {
                    statusFormattedMessage = string.Format("{4} - [{0}]  {3} {1}{2}",
                        messageEventArgs.Description, messageEventArgs.Message, Environment.NewLine, _BatchRunDataShortDate, DateTime.Now.ToShortTimeString());
                    LogMessage(messageEventArgs.LogFile, statusFormattedMessage);
                }
                else
                {
                    statusFormattedMessage = string.Format("{4} - [{0}]  {3} {1}{2}",
                         "", messageEventArgs.Message, Environment.NewLine, _BatchRunDataShortDate, DateTime.Now.ToShortTimeString());
                }
                messageEventArgs.StatusFormattedMessage = statusFormattedMessage;

                List<object> listData = new List<object>();
                listData.Add(messageEventArgs);

                MessageData messageData = new MessageData();
                messageData.EventData = listData;
                messageData.TopicName = Topics.Topic_ThirdPartyMessage;
                ThirdPartyLogic.Publish(messageData);
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

        private void OnStatus(object sender, StatusEventArgs statusEventArgs)
        {
            try
            {
                statusEventArgs.LogFile = _BatchLogFile;
                statusEventArgs.Description = _BatchDescription;

                var statusMessage = string.Empty;
                if (!string.IsNullOrEmpty(statusEventArgs.LogFile) && !string.IsNullOrEmpty(statusEventArgs.Description))
                {
                    statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}", statusEventArgs.Description, statusEventArgs.Text, Environment.NewLine, _BatchRunDataShortDate, DateTime.Now.ToShortTimeString());

                    LogMessage(statusEventArgs.LogFile, statusMessage);
                }
                else
                {
                    statusMessage = string.Format("{4} - [{0}]  {3} {1}{2}", "", statusEventArgs.Text, Environment.NewLine, _BatchRunDataShortDate, DateTime.Now.ToShortTimeString());
                }

                statusEventArgs.StatusFormattedMessage = statusMessage;

                List<object> listData = new List<object>();
                listData.Add(statusEventArgs);

                MessageData messageData = new MessageData();
                messageData.EventData = listData;
                messageData.TopicName = Topics.Topic_ThirdPartyStatusMessage;
                ThirdPartyLogic.Publish(messageData);
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
        /// Log Message
        /// </summary>
        /// <param name="file"></param>
        /// <param name="message"></param>
        public void LogMessage(string file, string message)
        {
            try
            {
                File.AppendAllText(file, message);
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

        public string GetServerPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// This method is to check for unallocated trades
        /// </summary>
        /// <param name="tradedate"></param>
        /// <returns>true if there are unallocated trades, otherwise false</returns>
        public bool CheckForUnallocatedTrades(DateTime tradedate)
        {
            try
            {
                return ThirdPartyLogic.CheckForUnallocatedTrades(tradedate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        public List<ThirdPartyForceConfirm> GetThirdPartyForceConfirmAuditData(int thirdPartyBatchId, DateTime runDate)
        {
            try
            {
                return ThirdPartyLogic.GetThirdPartyForceConfirmAuditData(thirdPartyBatchId, runDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Saves Third Party Force Confirm Audit Data From DB
        /// </summary>
        /// <returns></returns>
        public int SaveThirdPartyForceConfirmAuditData(List<ThirdPartyForceConfirm> dataSaveList, ThirdPartyBatch batch)
        {
            try
            {
                return ThirdPartyLogic.SaveThirdPartyForceConfirmAuditData(dataSaveList, batch);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        /// <summary>
        /// This method is to get block allocation details
        /// </summary>
        /// <param name="thirdPartyBatchid"></param>
        /// <param name="runDate"></param>
        /// <returns>List of Block Level Details if available, otherwise empty list</returns>
        public List<ThirdPartyBlockLevelDetails> GetBlockAllocationDetails(int thirdPartyBatchid, DateTime runDate)
        {
            List<ThirdPartyBlockLevelDetails> blockAllocationDetails = new List<ThirdPartyBlockLevelDetails>();
            try
            {
                blockAllocationDetails = ThirdPartyLogic.GetBlockAllocationDetails(thirdPartyBatchid, runDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return blockAllocationDetails;
        }

        /// <summary>
        /// This method is to get Force Confirm Details
        /// </summary>
        /// <param name="allocationId"></param>
        /// <param name="broker"></param>
        /// <returns></returns>
        public List<ThirdPartyForceConfirm> GetForceConfirmDetails(string allocationId, string broker)
        {
            List<ThirdPartyForceConfirm> result = new List<ThirdPartyForceConfirm>();
            try
            {
                result = ThirdPartyLogic.GetForceConfirmDetails(allocationId, broker);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// This method sends AU msg after getting taxlots for specified alloc ids in the list
        /// </summary>
        /// <param name="allocationIdList"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="affirmStatus"></param>
        /// <returns></returns>
        public bool SendAUMsg(List<string> allocationIdList, int counterPartyID, int affirmStatus)
        {
            try
            {
                return ThirdPartyLogic.GetRequiredTaxlotsAndSendAUMsg(allocationIdList, counterPartyID, affirmStatus);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This method sends AT msg for specified allocId and allocStatus
        /// </summary>
        /// <param name="allocIdAllocReportIdPairs"></param>
        /// <param name="counterPartyID"></param>
        /// <param name="allocStatus"></param>
        /// <returns></returns>
        public bool SendATMsg(Dictionary<string, string> allocIdAllocReportIdPairs, int counterPartyID, int allocStatus)
        {
            try
            {
                return ThirdPartyLogic.GetRequiredBlocksAndSendATMsg(allocIdAllocReportIdPairs, counterPartyID, allocStatus);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This method provides the Third Party Batch Event Data
        /// </summary>
        /// <param name="blockId"></param>
        public List<ThirdPartyBatchEventData> GetThirdPartyBatchEventData(int blockId)
        {
            List<ThirdPartyBatchEventData> thirdPartyBatchEventData = new List<ThirdPartyBatchEventData>();
            try
            {
                thirdPartyBatchEventData = ThirdPartyLogic.GetThirdPartyBatchEventData(blockId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return thirdPartyBatchEventData;
        }

        #region Tolerance Profile
        /// <summary>
        /// Delete Third Party Tolerance Profile
        /// </summary>
        /// <param name="thirdPartyToleranceProfileId"></param>
        public void DeleteThirdPartyToleranceProfile(int thirdPartyToleranceProfileId)
        {
            try
            {
                ThirdPartyDataManager.DeleteThirdPartyToleranceProfile(thirdPartyToleranceProfileId);
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
        /// Get Third Party Tolerance Profile
        /// </summary>
        /// <returns></returns>
        public List<ThirdPartyToleranceProfile> GetThirdPartyToleranceProfiles()
        {
            try
            {
                return ThirdPartyDataManager.GetThirdPartyToleranceProfiles();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Save Third Party Tolerance Profile
        /// </summary>
        /// <param name="toleranceProfile"></param>
        /// <returns></returns>
        public int SaveThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile)
        {
            try
            {
                return ThirdPartyDataManager.SaveThirdPartyToleranceProfile(toleranceProfile);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Get Third Party Tolerance Profile Common Data
        /// </summary>
        /// <returns></returns>
        public List<ThirdPartyToleranceProfileCommon> GetToleranceProfileCommonData()
        {
            try
            {
                return ThirdPartyDataManager.GetToleranceProfileCommonData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// update Third Party Tolerance Profile
        /// </summary>
        /// <param name="toleranceProfile"></param>
        public void UpdateThirdPartyToleranceProfile(ThirdPartyToleranceProfile toleranceProfile)
        {
            try
            {
                ThirdPartyDataManager.UpdateThirdPartyToleranceProfile(toleranceProfile);
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
        #endregion

        /// <summary>
        /// This method is to get AllocationMatchStatus for a batch
        /// </summary>
        /// <param name="thirdPartyBatchId"></param>
        /// <param name="date"></param>
        /// <param name="isFileTransmission"></param>
        /// <returns>AllocationMatchStatus</returns>
        public AllocationMatchStatus GetAllocationMatchStatusForBatch(int thirdPartyBatchId, string date, bool isFileTransmission)
        {
            try
            {
                if (isFileTransmission)
                {
                    return ThirdPartyCache.DateWiseFileStatusDetails.ContainsKey(date)
                        && ThirdPartyCache.DateWiseFileStatusDetails[date].Contains(thirdPartyBatchId)
                        ? AllocationMatchStatus.CompleteMatch : AllocationMatchStatus.NotSent;
                }
                else if (ThirdPartyCache.DateWiseAllocationBlockDetails.ContainsKey(date)
                        && ThirdPartyCache.DateWiseAllocationBlockDetails[date].ContainsKey(thirdPartyBatchId))
                {
                    return ThirdPartyCache.DateWiseAllocationBlockDetails[date][thirdPartyBatchId].AllocationMatchStatus;
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
            return AllocationMatchStatus.NotSent;
        }

        /// <summary>
        /// This method is to get third party file logs.
        /// </summary>
        /// <param name="runDate"></param>
        public List<ThirdPartyFileLog> GetThirdPartyFileLogs(DateTime runDate)
        {
            List<ThirdPartyFileLog> fileAuditLog = new List<ThirdPartyFileLog>();
            try
            {
                fileAuditLog = ThirdPartyLogic.GetThirdPartyFileLogs(runDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return fileAuditLog;
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ThirdPartyFileFormat GetFormat(ThirdPartyBatch batch)
        {
            ThirdPartyFileFormat format = null;
            try
            {
              format = ThirdPartyLogic.GetFormat(batch);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return format;
        }

        /// <summary>
        /// This method encrypts and sends file for given third party batch and date
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="rundate"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public bool EncryptAndSendFile(ThirdPartyBatch batch, DateTime rundate, string loggedInUser)
        {
            bool result = false;
            try
            {
                result = ThirdPartyLogic.EncryptAndSendFile(batch, rundate, OnStatus, OnMessage, loggedInUser);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// This method checks if there is a mismatch in groups and asks for user confirmation 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="runDate"></param>
        /// <returns></returns>
        public bool CheckForMismatchAndGetConfirmation(ThirdPartyBatch batch, DateTime runDate)
        {
            bool isMismatch = false;
            try
            {
               isMismatch = ThirdPartyLogic.CheckForMismatchAndGetConfirmation(batch, runDate);
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
            return isMismatch;
        }
    }
}
