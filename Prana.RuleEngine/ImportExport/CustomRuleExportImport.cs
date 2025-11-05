using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.Interfaces;
using Prana.Global;
using Prana.RuleEngine.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Prana.RuleEngine.ImportExport
{
    class CustomRuleExportImport : IImportHelper
    {


        //static CustomRuleExportImport _customRuleExportImport;
        //internal static CustomRuleExportImport GetInstance()
        //{
        //    if (_customRuleExportImport == null)
        //        _customRuleExportImport = new CustomRuleExportImport();

        //    return _customRuleExportImport;
        //}

        //private CustomRuleExportImport()
        //{

        //}

        String _cacheKey = "CustomImportExportCommunication";
        String _exchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
        public event ImportExportHandler ImportExportActionReceived;
        List<IAmqpReceiver> amqpReceiverList = new List<IAmqpReceiver>();
        /// <summary>
        /// Intialising AmqpPlugin for Custom rule import export.
        /// </summary>
        public void IntialiseAmqpPlugins()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                AmqpHelper.InitializeSender(_cacheKey, _exchangeName, MediaType.Exchange_Direct);

                List<String> routingKey = new List<string>();
                routingKey.Add("CustomRuleImportExportComplete");
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, routingKey);

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
                if (amqpReceiver.MediaName == _exchangeName && amqpReceiver.RoutingKey[0].Equals("CustomRuleImportExportComplete"))
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
        /// Recieving response for import export from esper engine
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
                    ImportExportActionReceived(dsReceived, RuleCategory.CustomRule);
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
        /// Sending request for import export to esper engine
        /// </summary>
        /// <param name="requestDict"></param>
        /// <param name="requestType"></param>
        internal void SendRequest(Dictionary<String, String> requestDict, string requestType)
        {
            try
            {
                //Dictionary<String, String> requestDict = new Dictionary<string, string>();
                //requestDict.Add("MessageType", requestType);

                //if (requestType.Equals("ExportCustomRule"))
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
                SendRequest(requestDict, "ImportCustomRule");
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
                SendRequest(requestDict, "ExportCustomRule");
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




        #region IDisposable Members

        public void Dispose()
        {
            //TODO:Dispose any listener
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
