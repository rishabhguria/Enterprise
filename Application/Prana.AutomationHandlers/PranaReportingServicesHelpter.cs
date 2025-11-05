using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.AutomationHandlers
{  

    public class PranaReportingServicesHelpter
    {
        private static PranaReportingServicesHelpter _singletonInstance = null;

        private static object _locker = new object();

        public static PranaReportingServicesHelpter GetInstance()
        {
            if (_singletonInstance == null)
            {
                lock (_locker)
                {
                    if (_singletonInstance == null)
                    {
                        _singletonInstance = new PranaReportingServicesHelpter();
                    }
                }
            }
            return _singletonInstance;
        }


        public PranaRiskObjColl ConvertRiskObjFromTaxlot(List<TaxLot> taxlots)
        {
            PranaRiskObjColl riskObjColl = null;
            try
            {
                riskObjColl = new PranaRiskObjColl();

                foreach (TaxLot taxlot in taxlots)
                {
                    PranaRiskObj riskObj = new PranaRiskObj();
                    riskObj.CopyBasicDetails(taxlot);

                    riskObj.SectorName = taxlot.SectorName;
                    riskObj.CountryName = taxlot.SectorName;
                    riskObj.SubSectorName = taxlot.SectorName;
                    riskObj.SecurityTypeName = taxlot.SecurityTypeName;
                    riskObjColl.Add(riskObj);                 
                }                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return riskObjColl;
        }
        //public PranaRiskObjColl ConvertRiskObjFromTaxlot(List<PositionMaster > taxlots)
        //{
        //    PranaRiskObjColl riskObjColl = null;
        //    try
        //    {
        //        riskObjColl = new PranaRiskObjColl();

        //        foreach (PositionMaster taxlot in taxlots)
        //        {
        //            PranaRiskObj riskObj = CopyBasicDetails(taxlot);
                    
        //            riskObjColl.Add(riskObj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return riskObjColl;
        //}
        //private   PranaRiskObj CopyBasicDetails(PositionMaster basicMsg)
        //{
        //    PranaRiskObj riskObj = new PranaRiskObj();
        //    riskObj.OrderSideTagValue = basicMsg.SideTagValue;


        //    riskObj.OrderSide  = basicMsg.Side;



        //    riskObj.Symbol = basicMsg.Symbol;

        //    riskObj.UnderlyingSymbol = basicMsg.UnderlyingSymbol;

        //    riskObj.Quantity = basicMsg.NetPosition;


        //    //riskObj.AvgPrice = basicMsg.p;


        //    riskObj.AssetID = basicMsg.AssetID;


        //  //  riskObj.AssetName = basicMsg.AssetName;


        //    riskObj.UnderlyingID = basicMsg.UnderlyingID;


        //  //  riskObj.UnderlyingName = basicMsg.UnderlyingName;


        //    riskObj.ExchangeID = basicMsg.ExchangeID;


        //    riskObj.ExchangeName = basicMsg.ExchangeName;


        //    riskObj.CurrencyID = basicMsg.CurrencyID;


        //    riskObj.CurrencyName = basicMsg.CurrencyName;


        //    riskObj.AUECID = basicMsg.AUECID;


        //    riskObj.TradingAccountID = basicMsg.TradingAccountID;


        //   // riskObj.TradingAccountName = basicMsg.TradingAccountName;


        //    riskObj.CompanyUserID = basicMsg.UserID;
        //    //riskObj.CompanyUserName = basicMsg.user;
        //    riskObj.CounterPartyID = basicMsg.CounterPartyID;
        //    //riskObj.CounterPartyName = basicMsg.CounterPartyName;
        //    riskObj.VenueID = basicMsg.VenueID;
        //   // riskObj.Venue = basicMsg.Venue;
        //   // riskObj.CumQty = basicMsg.CumQty;
        //    riskObj.ContractMultiplier = basicMsg.Multiplier;
        //    //riskObj.CompanyName = basicMsg.CompanyName;
        //    riskObj.AUECLocalDate = basicMsg.AUECLocalDate;
        //    riskObj.SettlementDate = basicMsg.SettlementDate;
        //    riskObj.ExpirationDate = basicMsg.ExpirationDate;
        //    riskObj.Description = basicMsg.Description;
        //    riskObj.FXRate = basicMsg.SettlCurrFxRate;
        //    riskObj.FXConversionMethodOperator = basicMsg.SettlCurrFxRateCalc;
        //    riskObj.Level1Name = basicMsg.FundName;
        //    riskObj.Level2Name = basicMsg.Strategy;
            
        //}
        public void UpdateRiskObjFromPranaRequestCarrier(PranaRequestCarrier pranaRequestCarrier, PranaRiskObjColl riskObjs)
        {
         
            try
            {
                

                foreach (PranaRiskObj riskObj in riskObjs)
                {
                    PranaRiskResult riskResult= pranaRequestCarrier.GetIndvRiskResult(riskObj.Symbol);
                    if (riskResult != null)
                    {
                        riskObj.SetRiskData(riskResult);
                    }
                  
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
          
        }

        public List<string> ConvertDetailsObjectToList(ClientSettings objClientSetting)
        {
            List<string> clientDetailsList = null;
            try
            {
                ClientRiskPref clientRiskPref = objClientSetting.RiskPreferences;
                clientDetailsList = new List<string>();
               // clientDetailsList.Add(clientRiskPref.RiskCalculationCriteria.ToString());
                if (!string.IsNullOrEmpty(clientRiskPref.GroupingCriteria))
                {
                    if (clientRiskPref.GroupingCriteria.Contains(","))
                    {
                        string[] grping = clientRiskPref.GroupingCriteria.Split(',');
                        foreach (string grpStr in grping)
                        {
                            clientDetailsList.Add(grpStr);
                        }                        
                    }
                    else
                    {
                        clientDetailsList.Add(clientRiskPref.GroupingCriteria);
                    }
                }               
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return clientDetailsList;
        }

    }
}
