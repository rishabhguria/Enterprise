using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.AutomationHandlers;
using System.Threading;
using Prana.Utilities.MiscUtilities;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.ReportingServices
{
    public class AutomationProcessor        
    {
        DataHandlerFactory _handler;
        public AutomationProcessor(DataHandlerFactory handler)
        {
            _handler = handler;
        }
        static object locker = new object();
        
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AutomationProcessor));
        public void ProcessData(List<ClientSettings> settings)
        {
            Thread workerThread = new Thread(StartProcessging);
            workerThread.Start(settings);
        }

        //internal void ProcessData(ClientSettings clientSettings)
        //{
            
        //    ThreadPool.QueueUserWorkItem(StartProcessging, clientSettings);
        //}
       
        public  void StartProcessging(object state)
        {
            lock (locker)
            {

                try
                {
                    List<ClientSettings> settings = state as List<ClientSettings>;
                    Dictionary<string, List<ClientSettings>> _listofSuccessFull = new Dictionary<string, List<ClientSettings>>();
                    Dictionary<string, List<ClientSettings>> _listofUnSuccessFull = new Dictionary<string, List<ClientSettings>>();
                    foreach (ClientSettings clientSettings in settings)
                    {
                        
                        try
                        {
                            ReportingServicesDataManager.GetClientDetail(clientSettings);
                            logger.Info(clientSettings.ClientSettingName + " Started Processing.... ");
                            //This Will Retrive All the Client Setting Information
                            logger.Info(clientSettings.ClientSettingName + " Input Parameters Processing Started.......");
                            AutomationInputParameterProcessing.Process(clientSettings);
                            ISecMasterServices secMaster = SecurityMasterWrapper.GetSecurityMaster(clientSettings.ClientID);
                            _handler.SecMasterServices = secMaster;
                            _handler.PositionServices.SecMasterServices = secMaster;


                            logger.Info(clientSettings.ClientSettingName + " Input Parameters Processed.");
                            AutomationDataRetrieval retriever = new AutomationDataRetrieval(_handler);
                            //retriever.PositionServices = DataHandlerFactory.PositionServices;
                            //retriever.AllocationServices = DataHandlerFactory.AllocationServices;

                            logger.Info(clientSettings.ClientSettingName + " Retrieving data ......");
                            IList data = retriever.RetrieveData(clientSettings);

                            logger.Info(clientSettings.ClientSettingName + " Data Retrieved .");

                            logger.Info(clientSettings.ClientSettingName + " Processing data ......");
                            AutomationDataProcessing processing = new AutomationDataProcessing(_handler);
                            processing.Process(clientSettings, data);
                            logger.Info(clientSettings.ClientSettingName + " Processing Done.");
                            logger.Info(clientSettings.ClientSettingName + " Dispatching Data.....");
                            List<ClientSettings> temp=new List<ClientSettings>();
                            if (_listofSuccessFull.ContainsKey(clientSettings.ClientName))
                            {
                                temp = _listofSuccessFull[clientSettings.ClientName];
                            }
                            else
                            {
                                _listofSuccessFull.Add(clientSettings.ClientName,temp);
                            }
                            temp.Add(clientSettings);
                            clientSettings.BaseSettings.Success = true;

                        }
                        catch (Exception ex)
                        {
                            if (clientSettings != null)
                            {
                                Exception reportingException = new Exception("Exception for " + clientSettings.ClientSettingName + " " + ex.Message);
                                logger.Error(reportingException.Message);

                                clientSettings.BaseSettings.MailBody = " Error Message: " + ex.Message;// +clientSettings.ToString();
                                clientSettings.BaseSettings.Success = false;
                                List<ClientSettings> tempUnsuccess = new List<ClientSettings>();
                                if (_listofUnSuccessFull.ContainsKey(clientSettings.ClientName))
                                {
                                    tempUnsuccess = _listofUnSuccessFull[clientSettings.ClientName];
                                }
                                else
                                {
                                    _listofUnSuccessFull.Add(clientSettings.ClientName, tempUnsuccess);
                                }
                                tempUnsuccess.Add(clientSettings);
                            }
                        }
                       
                    }
                    foreach (KeyValuePair<string, List<ClientSettings>> values in _listofSuccessFull)
                    {
                        List<ClientSettings> allforthisClient = values.Value;
                        if (_listofUnSuccessFull.ContainsKey(values.Key))
                        {
                            allforthisClient.AddRange(_listofUnSuccessFull[values.Key]);
                        }
                        _listofUnSuccessFull.Remove(values.Key);
                        AutomationDataPostProcessing postprocessing = new AutomationDataPostProcessing();
                        postprocessing.Process(allforthisClient);
                    }
                    foreach (KeyValuePair<string, List<ClientSettings>> values in _listofUnSuccessFull)
                    {
                        List<ClientSettings> allforthisClient = values.Value;
                        AutomationDataPostProcessing postprocessing = new AutomationDataPostProcessing();
                        postprocessing.Process(allforthisClient);
                    }
                  
                   
                   

                }
                catch (Exception ex)
                {
                    


                }
            }
        }        
    }
}
