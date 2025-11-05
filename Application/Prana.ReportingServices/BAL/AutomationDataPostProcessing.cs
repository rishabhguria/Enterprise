using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Utilities.MiscUtilities;
using Prana.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.ReportingServices
{
    class AutomationDataPostProcessing
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AutomationDataPostProcessing)); 
        public  void Process(List<ClientSettings> clientSetting)
        {
            // send data based on dispatcher types
            try
            {
                ClientSettings ObjectToSend = clientSetting[0];
                ObjectToSend.BaseSettings.MailSubject = "All Senario Executed";
                IDelivery _delivery = DeliveryFactory.GetInstance().GetDeliveryClass(ObjectToSend.DeliveryMethod);
                string mailbody = "";

               
                List<string> files = new List<string>();
                foreach (ClientSettings setting in clientSetting)
                {
                    if (setting.BaseSettings.Success)
                    {
                        mailbody = "\n "+mailbody + " \n\n Senario Executed Successfully \n ";
                    }
                    else
                    {
                        mailbody ="\n "+ mailbody+" \n\n Senario Failed \n";
                    }
                    //if (setting.ReportType != AutomationEnum.ReprotTypeEnum.Internal)
                    //{
                        if (setting.BaseSettings.Success)
                        {
                            string msgInBody = "";
                            if (setting.ReportType == AutomationEnum.ReprotTypeEnum.Internal)
                            {
                                msgInBody = "\n Data Imported Successfully ";
                            }
                            else
                            {
                                msgInBody = "\n Please find attached reports ";
                            }
                            mailbody = mailbody  +msgInBody+ setting.ToString();
                        }
                        else
                        {
                            mailbody = mailbody + setting.BaseSettings.MailBody + " " + setting.ToString();
                        }
                        
                    
                    
                    if (setting.ReportFileName != null && setting.ReportFileName != "")
                    {
                        files.Add(setting.ReportFileName);
                    }
                }
                MailDeliveryParameters mailDeliveryParameters = new MailDeliveryParameters(ObjectToSend.BaseSettings.HostID, ObjectToSend.BaseSettings.SupoortMailID, ObjectToSend.BaseSettings.SupportMailPassword, ObjectToSend.EmailID, ObjectToSend.BaseSettings.MailSubject, mailbody, files);
                bool isSuccess = _delivery.SendFile(mailDeliveryParameters);

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
    }
}
