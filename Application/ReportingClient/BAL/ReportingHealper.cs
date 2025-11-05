using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Interfaces;
using System.IO;
using Prana.Utilities.MiscUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.WCFConnectionMgr;
using System.Configuration;

namespace ReportingClient
{
    public class ReportingHealper
    {
        static List<TaxLot> riskReportServicesTaxlots=new List<TaxLot>();
        
        public static void CreateReportsForEachClient()
        {
            try
            {
                CreateIRiskReportingServicesProxy();
              
                List<ClientSettings> _lsClientSettings = new List<ClientSettings>();

                #region Temprary Clients Created To Test

                ClientSettings _clientSettings = new ClientSettings();
                _clientSettings.ClientID = 1; 

                _lsClientSettings.Add(_clientSettings);

                _clientSettings = new ClientSettings();
                _clientSettings.ClientID = 2;

                _lsClientSettings.Add(_clientSettings);

                #endregion
                ClientSettings clientDetail;

                foreach (ClientSettings currentClient in _lsClientSettings)
                {
                    //ClientRiskPref clientRiskPref = new ClientRiskPref();
                    //clientRiskPref.ClientID = 1;
                    clientDetail = RiskReportingServices.InnerChannel.GetRiskReport(currentClient);
                  
                    //Extra has Code removed From Here
                    if (clientDetail.DeliveryMethod != null)
                    {
                        IDelivery _delivery = DeliveryFactory.GetInstance().GetDeliveryClass(clientDetail.DeliveryMethod);
                        bool isSuccess = _delivery.SendFile(clientDetail);
                    }                    
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

        public static void test()
        {
            List<ClientSettings> _lsClientSettings = new List<ClientSettings>();

            ClientSettings _clientSettings = new ClientSettings();
            _clientSettings.ClientID = 1;

            _lsClientSettings.Add(_clientSettings);

            _clientSettings = new ClientSettings();
            _clientSettings.ClientID = 2;

            _lsClientSettings.Add(_clientSettings);

            List<string> columns =new List<string>(new string[] { "ClientID", "ClientName", "DeliveryMethod" });

            GeneralUtilities.CreateDataSetFromCollection(_lsClientSettings,columns);


        }

        #region Proxy Section



        static ProxyBase<IReportingServices> _RiskReportingServices = null;
        public static ProxyBase<IReportingServices> RiskReportingServices
        {
            set
            {
                _RiskReportingServices = value;

            }
            get { return _RiskReportingServices; }
        }

        public static void CreateIRiskReportingServicesProxy()
        {
            string endpointAddressInString = ConfigurationManager.AppSettings["RiskReportingEndpointAddress"];
            RiskReportingServices = new ProxyBase<IReportingServices>(endpointAddressInString);
            RiskReportingServices.IsLocalConnection = true;
        }

        #endregion        

        #region Extra Code

        #region Temporary Taxlots To Test

        //TaxLot testTaxlot = new TaxLot();
        //testTaxlot.AssetName = "TestAsset1"; testTaxlot.AvgPrice = 12.12;
        //testTaxlot.CumQty = 112;

        //riskReportServicesTaxlots.Add(testTaxlot);

        //testTaxlot = new TaxLot();
        //testTaxlot.AssetName = "TestAsset2"; testTaxlot.AvgPrice = 2423.12;
        //testTaxlot.CumQty = 232424;

        //riskReportServicesTaxlots.Add(testTaxlot);

        #endregion

        //IFileFormatter _fileFormatter = FileFormatterFactory.GetInstance().GetFormatterClass(_clientSettings.FileFormatter);

        //List<object> convertedList = new List<object>();
        //foreach (TaxLot t in riskReportServicesTaxlots)
        //    convertedList.Add((Object)t);

        //bool isFilecreated = _fileFormatter.CreateFile(convertedList, currentClient.DataLocation);                
        #endregion

    }
}
