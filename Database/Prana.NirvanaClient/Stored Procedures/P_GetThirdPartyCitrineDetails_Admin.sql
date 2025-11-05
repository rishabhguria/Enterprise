/*                  
Created By: Ankit Gupta            
On: April 29, 2013            
Purpose: To make customized changes for EOD files for Prompt Date, Last TradeDate, Delivery Date.            
            
            
[P_GetThirdPartyCitrineDetails] 27,'1184,1183,1182,1186,1185','04/26/2013',5,'1,20,21,18,1,15,12,16,33, 80',1                                  
*/                  
                  
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails_Admin]                                                                                                                               
(                                                                                                                                              
 @thirdPartyID int,                                                                                                                                              
 @companyFundIDs varchar(max),                                                                                                                                              
 @inputDate datetime,                                                                                                                                          
 @companyID int,                                                                                                          
 @auecIDs varchar(max),                                                
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                                
 @dateType int  -- 0 means for Process Date and 1 means Trade Date.          
,@fileFormatID int   
)                                                                                                                                              
                                                                                                                                               
AS                                                         
Declare @Fund Table                                                                                                        
(                                                                                                        
FundID int                                                                                                    
)                                                       
                                                      
Declare @AUECID Table                                                                                                        
(                                                                                                        
AUECID int                                                                                                    
)                                       
                                
Declare @FXForwardAuecID int                                
Set @FXForwardAuecID = (Select Top 1 Auecid from T_AUEC where assetid = 11)                                
                                                
--Declare  @auecIDs varchar(max)                                                      
--Set  @auecIDs='1,11,12,15,18,61,62,81'                                                      
                                                      
Insert into @Fund                                                      
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')                                                         
                               
Insert into @AUECID                                                      
Select Cast(Items as int) from dbo.Split(@auecIDs,',')                                                                                  
                                         
                                               
Begin                                         
 Select                                                     
   VT.Level1AllocationID,                
   ISNULL(VT.FundID,0) as FundID,                  
 T_CompanyFunds.FundShortName as FundName,                                                                               
   ISNULL(T_OrderType.OrderTypesID,0)as OrderTypesID,                                                                                                                                              
   ISNULL(T_OrderType.OrderTypes,'Multiple') as OrderTypes,                                                                                                                                              
   VT.OrderSideTagValue as SideID,                                                 
   T_Side.Side as Side,                                                                                                                                         
   VT.Symbol,                                                                                                                                               
   VT.CounterPartyID,                  
 T_CounterParty.ShortName as CounterParty,                                                                                                                                       
   VT.VenueID,                                                                                                                        
   Sum(VT.TaxLotQty) as OrderQty,                                                                                                                               
   VT.AvgPrice as AveragePrice,                                                                       
   VT.CumQty as ExecutedQty,                                                                                              
   VT.Quantity as TotalQty,                                                                                                                                      
   VT.AUECID,                                                                                                                                              
   VT.AssetID,                                                                  
    T_Asset.AssetName as Asset,                                                                                                                                           
 VT.UnderlyingID,                                                                                                                                         
   VT.ExchangeID,                  
 T_Exchange.DisplayName as Exchange,                                                                                                                                          
   Currency.CurrencyID,                                                                                                                        
   Currency.CurrencyName,                                                                                                                                              
   Currency.CurrencySymbol,                                  
   '' as MappedName,                                              
   '' as FundAccntNo,                                                     
   0 as FundTypeID_FK,                                                             
   '' as FundTypeName,                                                                                                                                           
   VT.Level1AllocationID as EntityID,                                                                                  
   Sum(VT.Level2Percentage) as Level2Percentage,                                                                                                                   
   Sum(VT.TaxLotQty) as AllocatedQty,                                                                                                                                         
   SM.PutOrCall,        
   SM.StrikePrice,                                                                 
   convert(varchar,SM.ExpirationDate,101) as ExpirationDate,                                 
   convert(varchar,VT.SettlementDate,101) as SettlementDate,                                                          
   Sum(VT.Commission) as CommissionCharged,                                 
   Sum(VT.OtherBrokerFees) as OtherBrokerFees,                                                                                         
   0 as ThirdPartyTypeID,                                                               
  '' as ThirdPartyTypeName,                                                                              
  0 as SecFee ,                                                     
  '' as CVName,                                                           
  '' as CVIdentifier,                                                                                                                          
  VT.GroupRefID,                
  ISNULL(VT.TaxlotState, 0) as TaxLotStateID,                
 case                   
 when VT.TaxLotState = '0' or VT.TaxLotState IS NULL                
 then 'Allocated'                  
 when VT.TaxLotState = '1'                  
 then 'Sent'                  
 when VT.TaxLotState = '2'                  
 then 'Amemded'                  
 when VT.TaxLotState = '3'                  
 then 'Deleted'                  
 when VT.TaxLotState = '4'                  
 then 'Ignored'                  
 end                                                                                                   
 as TaxLotState,                                                                                                                    
  Sum(ISNULL(VT.StampDuty,0)) as StampDuty,                                                                                                            
  Sum(ISNULL(VT.TransactionLevy,0)) as TransactionLevy,                                                                                                     
  Sum(ISNULL(ClearingFee,0)) as ClearingFee,                                                                                                            
  Sum(ISNULL(TaxOnCommissions,0)) as TaxOnCommissions,                                                                                                            
  Sum(ISNULL(MiscFees,0)) as MiscFees ,                                                                                                        
  convert(varchar, VT.AUECLocalDate,101) as TradeDate,                                                                                            
  ISNULL(SM.Multiplier, 1) as AssetMultiplier,                                                                                      
  0 as Level2ID,                                                                                   
  SM.ISINSymbol,                                                                                  
  SM.CUSIPSymbol,                                                                                  
  SM.SEDOLSymbol,                                                                                  
  SM.ReutersSymbol,                                                                                  
  SM.BloombergSymbol as BBCode,                                                                                  
  SM.CompanyName,                                                                                  
  SM.UnderlyingSymbol,                                                                      
  SM.OSISymbol,                                                  
  SM.IDCOSymbol,                                                  
  SM.OpraSymbol,                                        
VT.FXRate,                                        
VT.FXConversionMethodOperator,                                      
VT.ProcessDate,                                    
VT.OriginalPurchaseDate,         
VT.AccruedInterest,                                    
-- Reserved for future use                                    
'' as Comment1,                                    
'' as Comment2,                                  
--Swap Parameters                       
VT.SwapDescription,                                  
VT.IsSwapped,                     
T_Country.CountryName as CountryName,                               
Case                                  
 When VT.AssetID=11 and SM.ExpirationDate <> '1800-01-01 00:00:00.000'                                  
 Then dbo.AdjustBusinessDays(SM.ExpirationDate,-1,@FXForwardAuecID)                                  
 Else ''                                  
End As RerateDateBusDayAdjusted1,                                         
Case                                  
 When VT.AssetID=11 and SM.ExpirationDate <> '1800-01-01 00:00:00.000'                                  
 Then dbo.AdjustBusinessDays(SM.ExpirationDate,-2,@FXForwardAuecID)                                  
 Else ''                                
End As RerateDateBusDayAdjusted2,                              
VT.FXRate_Taxlot,                                        
VT.FXConversionMethodOperator_Taxlot,                          
VT.LotID,                                
VT.ExternalTransID,                          
VT.TradeAttribute1  ,                                
VT.TradeAttribute2  ,                              
VT.TradeAttribute3  ,                              
VT.TradeAttribute4  ,                              
VT.TradeAttribute5  ,                              
VT.TradeAttribute6  ,                      
SM.AssetName As UDAAssetName,                      
SM.SecurityTypeName As UDASecurityTypeName,                      
SM.SectorName As UDASectorName,                      
SM.SubSectorName As UDASubSectorName,                      
SM.CountryName As UDACountryName,                  
VT.Description,                  
Case            
When (datediff(m,VT.AUECLocalDate,SM.ExpirationDate) = 3) and (Day(SM.ExpirationDate) = Day(VT.AUECLocalDate))            
Then '3M'            
When datepart(dw,SM.ExpirationDate) = 4 and DATEPART(DAY, SM.ExpirationDate - 1) / 7 = 2             
Then '3W'            
else             
'NO'            
End                  
as PromptDateCheck,            
Case             
When dbo.IsBusinessDay(SM.ExpirationDate, VT.AUECID) = 0             
Then convert(varchar, dbo.AdjustBusinessDays(SM.ExpirationDate, 1,VT.AUECID),101)            
Else            
convert(varchar, SM.ExpirationDate, 101)            
End as PromptDate,               
case           
when VT.AssetID = 4 and (T_Exchange.DisplayName = 'LME' or T_Exchange.DisplayName = 'LME-FO')          
then convert(varchar, dbo.AdjustBusinessDays(SM.ExpirationDate,-1,VT.AUECID),101)          
else convert(varchar, dbo.AdjustBusinessDays(SM.ExpirationDate,-2,VT.AUECID),101)          
end              
As LastTradeDate,          
convert(varchar, dbo.AdjustBusinessDays(SM.ExpirationDate, 2,VT.AUECID),101) As DeliveryDate                        
          
  From                                                                             
    V_TaxLots VT                                                                        
                                                                                                                                             
     left join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                                                                    
     left join  T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue                                                                                                                                         
     left JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue                   
 left join T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID                  
 left join T_Asset ON T_Asset.AssetID = VT.AssetID                  
 left join T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID                                                                                                                       
                                                                        
 left join  T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID                 
     left join  T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                                                                                                                      
                              
     Left Outer Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol                                                                     
 Where datediff(d,        
 (         
 case         
  when @dateType=1         
 then         
  VT.AUECLocalDate         
 else        
  VT.ProcessDate        
 end                          
    ),@inputdate)=0           
    and VT.FundID in (select FundID from @Fund)                                                     
    and VT.AUECID in (select AUECID from @AUECID)                                                         
                    
                                                                 
  group by                                                                  
--  VT.TaxlotID,                                                                                          
  VT.Level1AllocationID,                                                                               
  VT.FundID,                   
  T_CompanyFunds.FundShortName,                                                                                                                                                                           
  T_OrderType.OrderTypesID,                                                                          
  T_OrderType.OrderTypes,                                   
  VT.OrderSideTagValue,                                                 
  T_Side.Side,                                                                                                              
  VT.Symbol,                                                                                                                                               
  VT.CounterPartyID,                   
  T_CounterParty.ShortName,                                                                                                       
  VT.VenueID,                                                                                                              
  VT.AvgPrice,                                                                                                                                              
  VT.CumQty,                                   
  VT.Quantity,                                                                     
  VT.AUECID,                                                                                                                                              
  VT.AssetID,                                                 
    T_Asset.AssetName,                                                                                                                                           
VT.UnderlyingID,                                                           
  VT.ExchangeID,                                                                       
    T_Exchange.DisplayName,                                       
 Currency.CurrencyID,                                                                                                                                              
  Currency.CurrencyName,                                                                            
  Currency.CurrencySymbol,                                                                                           
  SM.PutOrCall,                                                                                           
  SM.StrikePrice,                                    
  SM.ExpirationDate,                                                                                                                                              
  VT.SettlementDate,                                                                                                                    
  VT.GroupRefID,                                                                                                                    
  VT.TaxLotState,                                                                                                                                  
  VT.AUECLocalDate,         
  SM.Multiplier,                                                         
  SM.ISINSymbol,                           
  SM.CUSIPSymbol,                                                                                  
  SM.SEDOLSymbol,                                                                                
  SM.ReutersSymbol,                                                                                  
  SM.BloombergSymbol,                                
  SM.CompanyName,                                                                                  
  SM.UnderlyingSymbol,                                                                      
  SM.OSISymbol,                                                  
  SM.IDCOSymbol,                                                  
  SM.OpraSymbol,                                        
VT.FXRate,                   
VT.FXConversionMethodOperator,                                      
VT.ProcessDate,                                    
VT.OriginalPurchaseDate,                                    
VT.AccruedInterest,                                  
--Swap Parameters                                  
VT.SwapDescription,                                  
VT.IsSwapped,                                
T_Country.CountryName,                              
VT.FXRate_Taxlot,                              
VT.FXConversionMethodOperator_Taxlot,                          
VT.LotID,                                
VT.ExternalTransID,                          
VT.TradeAttribute1  ,                                
VT.TradeAttribute2  ,                              
VT.TradeAttribute3  ,                              
VT.TradeAttribute4  ,                              
VT.TradeAttribute5  ,                              
VT.TradeAttribute6 ,                      
SM.AssetName,                      
SM.SecurityTypeName,                      
SM.SectorName,                      
SM.SubSectorName,                      
SM.CountryName,                  
VT.Description                                             
--VT.FromDeleted                                                                          
                                                                         
 order by GroupRefID       
end 