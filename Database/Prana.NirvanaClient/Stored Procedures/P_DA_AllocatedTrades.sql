-- =============================================        
-- Modified By: Sumit Kakra  
-- Modification Date: July 8 2013  
-- Description: Nirvana Data access webservice proedure for webmethod AllocatedTrades  
-- =============================================        
CREATE Procedure [dbo].[P_DA_AllocatedTrades]  
(  
@date DateTime,          
@errorMessage varchar(max) output,                                        
@errorNumber int output                                 
)                              
As    
SET @ErrorMessage = 'Success'                                        
SET @ErrorNumber = 0                                 
                              
BEGIN TRY                          
      -- To ensure no locking, it allows dirty reads, so check for blank symbols and Qty>0  
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;   
  
  
 SELECT       
 G.GroupID,   
 L2.TaxLotID,   
 dbo.T_CompanyFunds.FundName,   
 T_CompanyStrategy.StrategyName,   
 G.Symbol,   
 dbo.V_SecmasterData.CompanyName SecurityName,   
 T_CounterParty.ShortName BrokerName,  
 dbo.T_Side.Side,   
 L2.TaxLotQty AllocatedQty,    
 G.AvgPrice,  
 G.AUECLocalDate TradeDate,   
 G.AllocationDate,  
 G.SettlementDate,                          
 L2.OtherBrokerFees,   
 L2.StampDuty,   
 L2.TransactionLevy,  
 L2.ClearingFee,   
 L2.TaxOnCommissions,   
 L2.MiscFees,  
 L2.Commission,--convert(int,'as'),  
 V_SecmasterData.UnderlyingSymbol,  
 V_SecmasterData.PutOrCall,  
 V_SecmasterData.StrikePrice,  
 V_SecmasterData.Multiplier,  
 T_Currency.CurrencySymbol,   
 T_Asset.AssetName,  
 V_SecmasterData.ExpirationDate,  
 T_Companyuser.Login UserName,  
 TP.ShortName as PrimeBrokerCode,  
 TP.ThirdPartyName AS PrimeBrokerName,  
 V_SecmasterData.AssetName AS UDAAssetName,  
 V_SecmasterData.SecurityTypeName AS UDASecurityTypeName,  
 V_SecmasterData.SectorName AS UDASectorName,  
 V_SecmasterData.SubSectorName AS UDASubSectorName,  
 V_SecmasterData.CountryName AS UDACountryName,  
 V_SecmasterData.BloombergSymbol AS BloombergSymbol,  
 V_SecmasterData.OSISymbol AS OSISymbol,  
 L2.SecFee,  
 L2.OccFee,  
 L2.OrfFee,                          
 L2.ClearingBrokerFee,  
 L2.SoftCommission  
 FROM dbo.T_Level2Allocation AS L2 INNER JOIN                          
  dbo.T_FundAllocation AS L1 ON L2.Level1AllocationID = L1.AllocationId INNER JOIN                          
  dbo.T_Group AS G ON G.GroupID = L1.GroupID LEFT OUTER JOIN                          
  dbo.T_Side ON G.OrderSideTagValue = dbo.T_Side.SideTagValue INNER JOIN  
  dbo.T_Currency ON T_Currency.CurrencyID = G.CurrencyID INNER JOIN   
  dbo.T_CompanyFunds ON L1.FundID = dbo.T_CompanyFunds.CompanyFundID  INNER JOIN  
  dbo.T_CompanyStrategy ON L2.Level2ID = dbo.T_CompanyStrategy.CompanyStrategyID  INNER JOIN  
  dbo.V_SecmasterData ON dbo.V_SecmasterData.TickerSymbol = G.Symbol  INNER JOIN   
  dbo.T_Asset ON dbo.T_Asset.AssetID = V_SecmasterData.AssetID INNER JOIN  
  dbo.T_CounterParty ON dbo.T_CounterParty.CounterPartyID = G.CounterPartyID INNER JOIN   
  dbo.T_Companyuser ON dbo.T_Companyuser.UserID = G.UserID  LEFT OUTER JOIN   
  dbo.T_CompanyThirdPartyMappingDetails CTPMD ON L1.FundID = CTPMD.InternalFundNameID_FK LEFT OUTER JOIN  
  dbo.T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID LEFT OUTER JOIN  
  dbo.T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID     
 WHERE Datediff(d,G.AUECLocalDate,@Date) = 0 And Symbol is Not Null  
  
END TRY                                        
BEGIN CATCH                                
                                          
 SET @ErrorMessage = ERROR_MESSAGE();                                        
 SET @ErrorNumber = Error_number();            
                                         
END CATCH;              

