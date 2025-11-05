using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.IO;
using Prana.AutomationHandlers;

namespace Prana.ReportingServices
{
    class AutomationInputParameterProcessing
    {
        public static void Process(ClientSettings ObjClientSetting)
        {
            try
            {
                //ReportingServicesDataManager.GetClientDetail(ObjClientSetting);

                #region Getting ThirdParties From objClientSettings.ThirdPartyNames

                List<string> lsThirdPary=new List<string>();
                lsThirdPary.AddRange(ObjClientSetting.ThirdPartyNames.Split(','));

                #endregion      
                   
                FileMapperComponent.CreateStructure(ObjClientSetting, lsThirdPary);
                if (ObjClientSetting.InputDataLocationType == AutomationEnum.InputOutputType.FileSystem)
                {
                    //Code To Set The File Setting Objects For Each ThirdParty
                    foreach (string thirdPartyName in lsThirdPary)
                    {
                        FileSetting fileSettingObj = FileMapperComponent.GetFilePath(ObjClientSetting, thirdPartyName);
                        if (fileSettingObj != null)
                            ObjClientSetting.FileSettings.Add(fileSettingObj);
                    }

                }
                if (ObjClientSetting.ReportType !=AutomationEnum.ReprotTypeEnum.Internal )
                {
                    ObjClientSetting.OutputReportLocationPath = RiskPathCreator.GetRiskReportPath(ObjClientSetting);
                    Directory.CreateDirectory(ObjClientSetting.OutputReportLocationPath);
                    ObjClientSetting.ReportFileName = RiskPathCreator.GetRiskReportFileName(ObjClientSetting);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }
    }
}
