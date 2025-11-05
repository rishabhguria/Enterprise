using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.RuleEngine.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Interfaces;


namespace Prana.RuleEngine.ImportExport
{
    class UserDefinedExportImport : IImportHelper
    {

       // int _userId = -1;
        String _cacheKey = "UserDefinedImportExportCommunication";
        String _exchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
        public event ImportExportHandler ImportExportActionReceived;

        List<IAmqpReceiver> amqpReceiverList = new List<IAmqpReceiver>();

        //internal void Initialise(int userId)
        //{
        //    try
        //    {
        //        //LoadAppSettings(userId);
        //        IntialiseAmqpPlugins();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        /// <summary>
        /// Initializing amqp for user defined import export.
        /// </summary>
        public void IntialiseAmqpPlugins()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                AmqpHelper.InitializeSender(_cacheKey, _exchangeName, MediaType.Exchange_Direct);

                List<String> exportRoutingKey = new List<string>();
                exportRoutingKey.Add("UserDefinedImportExportComplete");
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, exportRoutingKey);

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

        private void AmqpHelper_Stopped(IAmqpReceiver amqpReceiver, ListenerStopCause cause)
        {
            try
            {
                if (amqpReceiver.MediaName.Equals(_exchangeName))
                {
                    amqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
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

        private void AmqpHelper_Started(IAmqpReceiver amqpReceiver)
        {
            try
            {
                if (amqpReceiver.MediaName == _exchangeName && amqpReceiver.RoutingKey[0].Equals("UserDefinedImportExportComplete"))
                {
                    amqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived);
                    amqpReceiverList.Add(amqpReceiver);
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
/// Recieving response for import export from rule mediator
/// </summary>
/// <param name="dsReceived"></param>
/// <param name="mediaName"></param>
/// <param name="mediaType"></param>
/// <param name="routingKey"></param>
        void amqpReceiver_AmqpDataReceived(System.Data.DataSet dsReceived, string mediaName, MediaType mediaType, string routingKey)
        {

            try
            {
                if (ImportExportActionReceived != null)
                    ImportExportActionReceived(dsReceived, RuleCategory.UserDefined);
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


        //private void LoadAppSettings(int userId)
        //{
        //    try
        //    {

        //        _userId = userId;
        //        //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
        //        _exchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Sending Request for import export of user defined rules to rule mediator.
        /// </summary>
        /// <param name="requestDict"></param>
        /// <param name="requestType"></param>
        internal void SendRequest(Dictionary<String, String> requestDict, string requestType)
        {
            try
            {
                //Dictionary<String, String> requestDict = new Dictionary<string, string>();
                //requestDict.Add("MessageType", requestType);

                //if (requestType.Equals("ExportUserDefinedRule"))
                //{
                //    requestDict.Add("PackageName", packageName);
                //    requestDict.Add("DirectoryPath", directoryPath);
                //    requestDict.Add("RuleName", ruleName);
                //    requestDict.Add("RuleCategory", ruleCategory);
                //    requestDict.Add("RuleId", ruleId);
                //}

                requestDict.Add("MessageType", requestType);
                AmqpHelper.SendObject(requestDict, _cacheKey, requestType);
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




        public void ImportRule(Dictionary<String, String> requestDict)
        {
            try
            {
                SendRequest(requestDict, "ImportUserDefinedRule");
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

        public void ExportRule(Dictionary<String, String> requestDict)
        {
            try
            {
                SendRequest(requestDict, "ExportUserDefinedRule");
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

        //public void SendPathForExport(DataSet dsReceived)
        //{

        //    if (ImportExportActionReceived != null)
        //        ImportExportActionReceived(dsReceived, RuleCategory.UserDefined);
        //    //ImportExportManager.ExportRuleDef(dsReceived);
        //}





        #region IDisposable Members

        public void Dispose()
        {
            //TODO: dispose any listener

            AmqpHelper.Started -= new ListenerStarted(AmqpHelper_Started);
            AmqpHelper.Stopped -= new ListenerStopped(AmqpHelper_Stopped);

            foreach (IAmqpReceiver amqpReceiver in amqpReceiverList)
            {
                amqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
                amqpReceiver.CloseListener();
            }

            

        }

        #endregion
    }
}
