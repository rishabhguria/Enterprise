/*                        
Created By: Ankit Gupta                  
On: April 29, 2013                  
Purpose: To make customized changes for EOD files for Prompt Date, Last TradeDate, Delivery Date.                  
                  
                  
[P_GetThirdPartyCitrineDetails] 27,'1184,1183,1182,1186,1185','04/26/2013',5,'1,20,21,18,1,15,12,16,33, 80',1                                        
*/                        
                        
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails_TaxlotState_Admin]                                                                                                                                     
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
      
Create table #VT                  
(          
   Level1AllocationID varchar(50),                      
   FundID int,                        
   SideID varchar(3),                                                       
   Symbol varchar(100),                                                                                                                                                     
   CounterPartyID int,                        
   AveragePrice float,                                                                             
   ExecutedQty float,                                                                                                    
   TotalQty float,                                                                                                                                            
   AUECID int,                                                                                                                                                    
   AssetID int,                                                                        
   UnderlyingID int,                                                                                                                                               
   ExchangeID int,                        
   CurrencyID int,                                                                                                                              
   Level2Percentage int,                                                                                                                         
   AllocatedQty float,                                                                                                                                               
   SettlementDate datetime,                                                                
   CommissionCharged float,                                       
   OtherBrokerFees float,                                                                                               
  TaxLotStateID int,                      
  StampDuty float,                                                                                                                  
  TransactionLevy float,                                                                                                           
  ClearingFee float,                                                                                                                  
  TaxOnCommissions float,                                                                                                                  
  MiscFees float,                                                                                                              
  AUECLocalDate datetime,      
 ProcessDate datetime,                                                                                                  
  FXRate float,                                              
  FXConversionMethodOperator varchar(3),                                            
  GroupRefID int,      
  PBID int,    
  LotID varchar(100),                                      
  ExternalTransID varchar(100),                                
  TradeAttribute1  varchar(200),                                      
  TradeAttribute2  varchar(200),                                    
  TradeAttribute3  varchar(200),                                    
  TradeAttribute4  varchar(200),                                    
  TradeAttribute5  varchar(200),                                    
  TradeAttribute6  varchar(200),                            
  Description varchar(500)                    
)      
      
insert into #VT         
 select        
VT.Level1AllocationID,      
ISNULL(VT.FundID,0) as FundID,      
VT.OrderSideTagValue as SideID,        
 VT.Symbol,                                                                
   VT.CounterPartyID,        
  VT.AvgPrice as AveragePrice,      
 VT.CumQty as ExecutedQty,                                                                                                    
   VT.Quantity as TotalQty,                                                                                              
   VT.AUECID,                                                                                                                                                    
   VT.AssetID,                    
VT.UnderlyingID,                                                                                                                                               
   VT.ExchangeID,      
 VT.CurrencyID,         
VT.Level2Percentage,      
     VT.TaxLotQty as AllocatedQty,      
VT.SettlementDate,      
VT.Commission as CommissionCharged,      
VT.OtherBrokerFees as OtherBrokerFees,       
PB.TaxLotState as TaxLotStateID,      
ISNULL(VT.StampDuty,0) as StampDuty,                                                                                                                  
  ISNULL(VT.TransactionLevy,0) as TransactionLevy,                                                                                                           
  ISNULL(ClearingFee,0) as ClearingFee,                                                                                                                  
  ISNULL(TaxOnCommissions,0) as TaxOnCommissions,                                                                                                                  
  ISNULL(MiscFees,0) as MiscFees ,                                                                                                              
  VT.AUECLocalDate ,      
 VT.ProcessDate,        
 VT.FXRate,                                              
VT.FXConversionMethodOperator,      
VT.GroupRefID,    
PB.PBID,       
VT.LotID,                                      
VT.ExternalTransID,                                
VT.TradeAttribute1  ,                                      
VT.TradeAttribute2  ,                                    
VT.TradeAttribute3  ,                                    
VT.TradeAttribute4  ,                                    
VT.TradeAttribute5  ,                                    
VT.TradeAttribute6  ,       
VT.Description      
from V_Taxlots VT      
 Inner Join T_PBWiseTaxlotState PB on PB.TaxlotID = VT.TaxlotID          
 Where @fileFormatID = FileFormatID        
                                             
      
  Union All                                                                        
                                                                         
  select            
  TDT.Level1AllocationID ,                                                                                                                                    
  ISNULL(TDT.FundID,0) as FundID,                                                                         
  TDT.OrderSideTagValue as SideID,                                                                                                                                          
  TDT.Symbol,                              
  TDT.CounterPartyID,                                                                                                                         
  TDT.AvgPrice,                                                                                                                                        
  TDT.CumQty as ExecutedQty, --ExecutedQty                                                                
  TDT.Quantity as TotalQty, --TotalQty                                                                                                                                        
  TDT.AUECID,                                    
  TDT.AssetID,                                                                                                               
  TDT.UnderlyingID,                                                                                            
  TDT.ExchangeID,                                     
 TDT.CurrencyID ,                                       
  TDT.Level2Percentage,--Percentage,                                                                                     
  TDT.TaxLotQty,                                                              
  TDT.SettlementDate,                                                                                                  
  TDT.Commission,                                                                                     
  TDT.OtherBrokerFees,                                                                                                                                      
  TDT.TaxLotState,                                                                            
  ISNULL(TDT.StampDuty,0) as StampDuty,                                                                                                      
  ISNULL(TDT.TransactionLevy,0) as TransactionLevy,                                                                                                      
  ISNULL(TDT.ClearingFee,0) as ClearingFee,                                                                                         
  ISNULL(TDT.TaxOnCommissions,0) as TaxOnCommissions,                                                                                                      
  ISNULL(TDT.MiscFees,0) as MiscFees,                                                                                                            
  TDT.AUECLocalDate,      
 TDT.ProcessDate,                                                                   
  TDT.FXRate as FXRate,                                  
  TDT.FXConversionMethodOperator as FXConversionMethodOperator,      
  TDT.GroupRefID,    
TDT.PBID,                                
TDT.LotID,                          
TDT.ExternalTransID,                     
TDT.TradeAttribute1  ,                          
TDT.TradeAttribute2  ,                        
TDT.TradeAttribute3  ,                        
TDT.TradeAttribute4  ,                        
TDT.TradeAttribute5  ,                        
TDT.TradeAttribute6  ,            
TDT.Description      
 from                                                                                                                                         
   T_DeletedTaxLots TDT where FileFormatID = @fileFormatID      
      
      
Select                                                           
 T_CompanyFunds.FundShortName as AccountName,                                                                                     
   T_Side.Side as Side,                                                                                                                                               
   VT.Symbol,                                                                                                                                                     
 T_CounterParty.ShortName as CounterParty,                                                                                                                                             
   Sum(VT.AllocatedQty) as OrderQty,                                                                                                                                     
   VT.AveragePrice ,                                                                             
   VT.ExecutedQty,                                                                                                    
   VT.TotalQty,                                                                                                                                            
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
   Sum(VT.AllocatedQty) as AllocatedQty,                                                                                                                                               
   SM.PutOrCall,              
   ISNULL(SM.StrikePrice,0) as StrikePrice,                                                                       
   convert(varchar,SM.ExpirationDate,101) as ExpirationDate,                                       
   convert(varchar,VT.SettlementDate,101) as SettlementDate,                                                                
   Sum(VT.CommissionCharged) as CommissionCharged,                                       
   Sum(VT.OtherBrokerFees) as OtherBrokerFees,                                                                                               
   0 as ThirdPartyTypeID,                                                                     
  '' as ThirdPartyTypeName,                                                                                    
  '' as CVName,                                                                 
  '' as CVIdentifier,                                                                                                                                
  VT.GroupRefID,                      
  ISNULL(VT.TaxlotStateID, 0) as TaxLotStateID,                      
 case                         
 when VT.TaxlotStateID = '0' or VT.TaxlotStateID IS NULL                      
 then 'Allocated'                        
 when VT.TaxlotStateID = '1'                        
 then 'Sent'                        
 when VT.TaxlotStateID = '2'                        
 then 'Amemded'                        
 when VT.TaxlotStateID = '3'                        
 then 'Deleted'                        
 when VT.TaxlotStateID = '4'                        
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
VT.ProcessDate,                                          
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
VT.FXRate,                                              
VT.FXConversionMethodOperator,       
VT.GroupRefID,                               
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
    #VT VT                                                                              
     left join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                                                                          
     left join  T_Side ON dbo.T_Side.SideTagValue = VT.SideID                                                                                                                                               
 left join T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID                        
 left join T_Asset ON T_Asset.AssetID = VT.AssetID                        
 left join T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID                                                                                                                             
                                                                              
 left join  T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID                       
     left join  T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                                                                                                                            
                                    
     Left Outer Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol                                                                           
  Where      
 (      
   datediff(d,          
   (           
  case           
   when @dateType=1           
  then           
   VT.AUECLocalDate           
  else          
   VT.ProcessDate          
  end                            
   ) ,@inputdate)=0                                                                                         
  --and CTPM.InternalFundNameID_FK in (select FundID from @Fund)                                               
  or ((VT.TaxLotStateID = 2 OR VT.TaxLotStateID = 3) AND           
  datediff(d,          
 (           
 case           
  when @dateType=1           
 then           
  VT.AUECLocalDate           
 else          
  VT.ProcessDate          
 end                            
    ),@inputdate)>=0 ) )                                                                                         
  and (VT.TaxLotStateID <> 4 OR (VT.TaxLotStateID = 4 AND           
  datediff(d,          
 (           
 case           
  when @dateType=1           
 then           
  VT.AUECLocalDate           
 else          
  VT.ProcessDate          
 end                            
    ),@inputdate)=0 ))               
    and VT.FundID in (select FundID from @Fund)                                                           
    and VT.AUECID in (select AUECID from @AUECID)    
 and VT.PBID= @thirdPartyID                                                                
                          
                                                                       
  group by                                                                        
--  VT.TaxlotID,                                                                                                
  VT.Level1AllocationID,                                                                                     
  T_CompanyFunds.FundShortName,                                                                                       
  T_Side.Side,                                                                                                                    
  VT.Symbol,                                                                                                                                                     
  T_CounterParty.ShortName,                   
  VT.AveragePrice,                                                                                                                                                    
  VT.ExecutedQty,                                         
  VT.TotalQty,                                                                           
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
  VT.TaxLotStateID,                                                                                                                                        
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
VT.GroupRefID,                                            
VT.ProcessDate,      
T_Country.CountryName,                                           
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
                                                                               
 order by VT.GroupRefID             
end 