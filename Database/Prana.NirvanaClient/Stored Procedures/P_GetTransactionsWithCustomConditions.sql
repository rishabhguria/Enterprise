CREATE Procedure [dbo].[P_GetTransactionsWithCustomConditions]                                                                                                                                                            
(                                  
@FromDate VARCHAR(MAX),                                                                                                                                                                                                                       
@ToDate VARCHAR(MAX),                                                 
@AssetIds VARCHAR(MAX),                                                
@FundIds VARCHAR(MAX),    
@ReconDateType INT ,    
@CustomConditions VARCHAR(MAX)                                                                  
)      
                                                                                                                                 
As                   
          
/*                                                                      
Author: SURENDRA BISHT           
      
This Sp is for customised use of P_getTransactions with Custom Conditions.     
    
Usage Of  @CustomConditions : @CustomConditions=' AND ( Symbol like 'Dell' ) '                                                             
        
SP Revision used : ClientDB_1482_P_GetTransactions     
     
------------usage--------------------                  
declare @FromAllAUECDatesString VARCHAR(MAX)                                                                                                                                                                                                                   
  
    
declare @ToAllAUECDatesString VARCHAR(MAX)                                                            
declare @AssetIds VARCHAR(MAX)                                                             
declare @FundIds VARCHAR(MAX)                   
declare @ReconDateType INT         
              
set @FromAllAUECDatesString='0^2/27/2013 12:00:00 AM~1^2/27/2013 12:00:00 AM~'              
set @ToAllAUECDatesString='0^2/27/2013 12:00:00 AM~1^2/27/2013 12:00:00 AM~'              
set @AssetIds=''              
set @FundIds=''     
@ReconDateType 0 then consider Trade Date , 1 then consider Process Date,2 then consider Prana Process Date    
set @CustomConditions=' AND ( Symbol like 'Dell' ) '    
           
*/    
                                                                                                                                                                                                                  
Begin                                               
                                                  
 Select TaxlotID, PositionTag into #TempPosTag from PM_Taxlots                                                                    
  where TaxLot_PK in (Select Min(TaxLot_PK) from PM_Taxlots group by TaxlotID)                                                       
                                                  
Create Table #SecMasterDataTempTable                                                                                                                                                                                                                    
(                                                          
 AUECID int,                                                                                    
 TickerSymbol Varchar(100),                                                                                                 
 CompanyName  VarChar(500),                                                                                                                                                                    
 AssetName Varchar(100),                                                                                                     
 SecurityTypeName Varchar(200),                                                                                    
 SectorName Varchar(100),                                                                                                                                                                                                                                     
 SubSectorName Varchar(100),                                                                                                                                                          
 CountryName  Varchar(100),                                                                                                                                                                                
 PutOrCall Varchar(5),                                    
 Multiplier Float,                                               
LeadCurrencyID int,                                
 VsCurrencyID int,                                              
 CurrencyID int,                                             
 UnderlyingSymbol Varchar(100),                                                  
 ExpirationDate datetime,                                                 
 Coupon float,                                                  
 IssueDate datetime,                                                                          
 MaturityDate datetime,                                                                          
 FirstCouponDate datetime,                                                                     
 CouponFrequencyID int,                                                                          
 AccrualBasisID int,                                                                          
 BondTypeID int,                                                                          
 IsZero int,                                                                
 IsNDF bit,                                                                
 FixingDate datetime,                                                     
 IDCOSymbol varchar(50),                                                            
 OSISymbol varchar(50),                                                             
 SEDOLSymbol varchar(50),                                                             
 CUSIPSymbol varchar(50),                                              
 BloombergSymbol varchar(200),                                             
 Delta float,                                                  
 StrikePrice float ,                        
UnderlyingDelta float,                      
ISINSymbol varchar(50),              
ProxySymbol varchar(100),  
ReutersSymbol varchar(50)                                               
)                                                                                                                                              
                                                                            
Insert Into #SecMasterDataTempTable                                                                                                                                                             
Select                                                                                                                                                 
 AUECID ,                   
 TickerSymbol ,                                                                                                 
 CompanyName  ,                                                                            
 AssetName,                                                                     
 SecurityTypeName,                                                                                                 
SectorName ,                                                                                             
 SubSectorName ,                                                                                                                                                                      
 CountryName ,                                                                                                                        
 PutOrCall ,                                                        
 Multiplier ,                                                                   
 LeadCurrencyID ,                                                                                                                               
 VsCurrencyID,                                                                                                              
 CurrencyID,                                                                                                        
 UnderlyingSymbol ,                                                  
 ExpirationDate,                                                  
 Coupon,                                                  
 IssueDate,                                                                          
 MaturityDate,                                                                          
 FirstCouponDate,                                                                          
 CouponFrequencyID,                                                                          
 AccrualBasisID,                                                       
 BondTypeID,                                                                          
 IsZero,                                                                
 IsNDF,                                                                
 FixingDate,                                                   
 IDCOSymbol  ,                                                            
OSISymbol ,                                                  
SEDOLSymbol,                                                             
CUSIPSymbol,                                            
BloombergSymbol,                                                
Delta,                                                  
StrikePrice,                        
UnderlyingDelta,                      
ISINSymbol,              
ProxySymbol,  
ReutersSymbol                                                         
From V_SecMasterData                                                   
                                                  
Create Table #V_Taxlots                                          
(                                                  
TaxLotID varchar (50),                                                  
OrderSideTagValue char(1),                                                  
TotalExpenses float,                                                  
Level2ID int,                                                  
TaxLotQty float,                                                  
AvgPrice float,                                                  
Commission float,                                                 
OtherBrokerFees float,                                                  
ClearingFee float,                                                  
MiscFees float,                                                  
StampDuty float,                                                  
AUECLocalDate datetime,                                                  
OriginalPurchaseDate datetime,                                                  
ProcessDate datetime,                                                  
AssetID int,                                                  
UnderLyingID int,                                                  
ExchangeID int,                                          
CurrencyID int,                                                  
CurrencySymbol varchar(20),    
AUECID int,                                            
SettlementDate datetime,                                                  
Description varchar(max),                     
AllocationDate datetime,                                                  
IsSwapped bit,                                                  
GroupID varchar (50),                                                  
FXRate float,                                                  
FXConversionMethodOperator Varchar(3),                           
IsPreAllocated bit,                                                  
CumQty float,                                                  
AllocatedQty float,                        
Quantity float,                                                  
UserID int,                                                  
CounterPartyID int,                                                  
FundID int,                                                  
symbol varchar(50),                                                  
SideMultiplier int,                                                  
Side Varchar(50),                    
--following field are added to fulfill the ss requirement                    
LotID Varchar(200),                    
ExternalTransID Varchar(100),                                
TradeAttribute1  Varchar(200),                    
TradeAttribute2  Varchar(200),                  
TradeAttribute3  Varchar(200),                  
TradeAttribute4  Varchar(200),                  
TradeAttribute5  Varchar(200),                  
TradeAttribute6  Varchar(200),                                        
SecFee float,                                        
OccFee float,                                        
OrfFee float,    
ClearingBrokerFee float,                                        
SoftCommission float  ,          
TaxOnCommissions float ,          
TransactionLevy float,    
TransactionType Varchar(200),
SettlCurrency_Group varchar(50),
SettlCurrency_Taxlot varchar(50),
AdditionalTradeAttributes varchar(max),
)
                                    
Create Table #FXConversionRatesForTradeDate                                                                             
(                                                                                
 FromCurrencyID int,                                                                                              
 ToCurrencyID int,                                                                                                                    
 RateValue float,                                     
 ConversionMethod int,                                                                                              
 Date DateTime,                                                                                     
 eSignalSymbol varchar(max),
 FundId int                                      
)                       
Insert into #FXConversionRatesForTradeDate                                                  
Select StanPair.FromCurrencyID AS FromCurrencyID,                        
      StanPair.ToCurrencyID AS ToCurrencyID,                        
      CCR.ConversionRate AS RateValue,                         
      0 AS ConversionMethod,                         
      CCR.Date AS Date,                        
      StanPair.eSignalSymbol AS eSignalSymbol,
	  FundID as FundId                        
   From T_CurrencyConversionRate AS CCR                        
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                   
   WHERE DateDiff(d,@FromDate,CCR.Date) >=0 -- dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)            
  AND DateDiff(d,CCR.Date,@ToDate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)              
                      
  UNION                      
                      
  Select StanPair.ToCurrencyID AS FromCurrencyID,                        
      StanPair.FromCurrencyID AS ToCurrencyID,                        
      CCR.ConversionRate AS RateValue,                         
      1 AS ConversionMethod,                         
      CCR.Date AS Date,                        
      StanPair.eSignalSymbol AS eSignalSymbol,
	  FundID as FundId                        
   from T_CurrencyConversionRate AS CCR                        
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                     
   WHERE DateDiff(d,@FromDate,CCR.Date) >=0 --dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)            
  AND DateDiff(d,CCR.Date,@ToDate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                
                                          
 Update #FXConversionRatesForTradeDate                                                                                      
 Set RateValue = 1.0/RateValue                                                                                                                                                           
 Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                                                          
                                                                                                                               
Update #FXConversionRatesForTradeDate                                                                                                                                               
 Set RateValue = 0                                                                                                  
 Where RateValue is Null                     
                                                  
 Select * into #ZeroFundFXRates
 from #FXConversionRatesForTradeDate
 Where FundId = 0
 
 Select * into #NonZeroFundFXRates
 from #FXConversionRatesForTradeDate
 Where FundId <> 0
                   
                                                  
Create Table #AssetClass                                                   
(                                                  
AssetID int                                                  
)                                                                  
if (@AssetIds is NULL or @AssetIds = '')                                                                  
 Begin                                                  
  Insert into #AssetClass                                                                  
  Select AssetID from T_Asset                                                                  
 End                                                  
 Else                                                                   
 Insert into #AssetClass                                                                  
 Select Items as AssetID from dbo.Split(@AssetIds,',')                                                                                              
                                                  
Create Table #Funds                                                   
(                                                  
 FundID int                                                  
)                                                                  
 if (@FundIds is NULL or @FundIds = '')                                                  
 Begin                                                                  
 Insert into #Funds                                                                  
  Select CompanyFundID as FundID from T_CompanyFunds        
  Where IsActive=1                                                
 End                                                  
 Else                                 
 Insert into #Funds         
 Select Items as FundID from dbo.Split(@FundIds,',')                                                       
                                                  
                                                  
Insert into #V_Taxlots                                               
(                                              
TaxLotID,                                              
OrderSideTagValue,                                              
TotalExpenses,                                              
Level2ID,                                              
TaxLotQty,                                              
AvgPrice,                    
Commission,                                              
OtherBrokerFees,                                              
ClearingFee,                                              
MiscFees,                                              
StampDuty,                                              
AUECLocalDate,                                              
OriginalPurchaseDate,                                              
ProcessDate,                                              
AssetID,                                              
UnderLyingID,                                              
ExchangeID,                                      
CurrencyID,                                              
CurrencySymbol,    
AUECID,                                            
SettlementDate,                                                  
[Description],                                                  
AllocationDate,                                                  
IsSwapped,                                                  
GroupID,                                                  
FXRate,                                                  
FXConversionMethodOperator,                                                  
IsPreAllocated,                                     
CumQty,                                                  
AllocatedQty,                                                  
Quantity,                                                  
UserID,                                                  
CounterPartyID,                                                  
FundID,                                                  
symbol,                                                  
SideMultiplier,                                                  
Side,                    
LotID,                    
ExternalTransID,                
TradeAttribute1  ,                    
TradeAttribute2  ,                  
TradeAttribute3  ,                  
TradeAttribute4  ,                  
TradeAttribute5  ,                  
TradeAttribute6,    
SecFee,    
OccFee,    
OrfFee,    
ClearingBrokerFee,    
SoftCommission ,          
TaxOnCommissions,          
TransactionLevy,    
TransactionType,
SettlCurrency_Group,
SettlCurrency_Taxlot,
AdditionalTradeAttributes
)                                                  
                                                  
Select                                                   
 VT.TaxLotID,                                                  
 OrderSideTagValue,                                                  
 TotalExpenses,                                                  
 Level2ID,                                               
 TaxLotQty,                             
 AvgPrice,                                                  
 Commission,                                                  
 OtherBrokerFees,                                                  
 ClearingFee,                                                  
 MiscFees,                                                  
 StampDuty,                                                  
 AUECLocalDate,                                                  
 OriginalPurchaseDate,                                                  
 ProcessDate,                                                  
 VT.AssetID,       
 VT.UnderLyingID,                                                  
 VT.ExchangeID,                                                  
 VT.CurrencyID,                                                  
 CUR.CurrencySymbol,    
 VT.AUECID,                                            
 SettlementDate,                                         
 Description,                               
 AllocationDate,                                                  
 IsSwapped,                                                  
 VT.GroupID,                                                  
 IsNull(VT.FXRate_Taxlot,VT.FXRate) As FXRate ,                                                 
 IsNull(FXConversionMethodOperator_Taxlot,FXConversionMethodOperator) As FXConversionMethodOperator,                                                  
 IsPreAllocated,                                                  
 CumQty,                                                  
 AllocatedQty,                                                  
 Quantity,                                                  
 UserID,                                                  
 CounterPartyID,                                                  
 VT.FundID,                                                  
 Symbol,                                                  
 SideMultiplier,                                     
 T_Side.Side,                    
VT.LotID,                    
VT.ExternalTransID ,                
VT.TradeAttribute1  ,                    
VT.TradeAttribute2  ,                  
VT.TradeAttribute3  ,                  
VT.TradeAttribute4  ,                  
VT.TradeAttribute5  ,                  
VT.TradeAttribute6,    
 SecFee,    
 OccFee,    
 OrfFee,    
 ClearingBrokerFee,    
 SoftCommission  ,          
VT.TaxOnCommissions   ,          
VT.TransactionLevy ,    
TransactionType,
VT.SettlCurrency_Group,
VT.SettlCurrency_Taxlot,
VT.AdditionalTradeAttributes
from V_Taxlots VT                                                         
 Inner join #AssetClass on VT.AssetID = #AssetClass.AssetID                                                                  
 Inner join #Funds on VT.FundID = #Funds.FundID                                                                  
 INNER JOIN T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                                                             
inner join T_currency  CUR on CUR.CurrencyID=VT.CurrencyID                             
    
where         
--Modified by: Aman Seth, if @ReconDateType 0 then consider Trade Date , 1 then consider Process Date,2 then consider Prana Process Date      
(        
    (@ReconDateType = '0' AND  Datediff(d,VT.AUECLocalDate,@ToDate) >= 0 and  Datediff(d,VT.AUECLocalDate,@FromDate) <= 0 )        
      or        
    (@ReconDateType = '1' AND Datediff(d,VT.ProcessDate,@ToDate) >= 0 and  Datediff(d,VT.ProcessDate,@FromDate) <= 0)        
       or        
    (@ReconDateType = '2' AND Datediff(d,VT.NirvanaProcessDate,@ToDate) >= 0 and  Datediff(d,VT.NirvanaProcessDate,@FromDate) <= 0)        
       
 )         
and         
VT.TaxLotQty <> 0                                                   
                                             
 Select                                                                                         
@FromDate as Rundate,                                                                                        
 VT.TaxLotID as TaxLotID,                                                                                                                     
 VT.AUECLocalDate as TradeDate,                                                                        
 VT.OriginalPurchaseDate,                                     
 VT.ProcessDate,     
-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS     
 VT.OrderSideTagValue as SideID,                                                                                                                                                               
 VT.Symbol as Symbol ,      
 -- It was earlier considered as quantity but there is already Quantity named field selected below . So changed to TaxLotQuantity.     
 (VT.TaxLotQty) as TaxLotQuantity ,         
 VT.AvgPrice as AvgPX ,                                                                                                                                     
 VT.FundID as FundID,                                                              
 VT.AssetID as AssetID,                                                  
 VT.UnderLyingID as UnderLyingID,                                                                                                                       
 VT.ExchangeID as ExchangeID,                                                                                                                      
 VT.CurrencyID as CurrencyID,                                                                                                             
  VT.CurrencySymbol as CurrencySymbol,    
 VT.AUECID as AUECID ,                                                        
 VT.TotalExpenses as TotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                                                                                           
 isnull(SM.Multiplier,1) as Multiplier,                                                                                                         
 VT.SettlementDate as SettlementDate,                                                                                                          
 SM.LeadCurrencyID,                                                                                             
 SM.VsCurrencyID,                                                                                         
 isnull(SM.ExpirationDate,'1/1/1800') as ExpirationDate,                                                                                                                            
 VT.Description as Description,                                                          
 VT.Level2ID as Level2ID,                                                                                                                                                                        
 isnull( (VT.TaxLotQty * SW.NotionalValue / VT.CumQty) ,0) as NotionalValue,                                                                                                                                                                                  
  
 isnull(SW.BenchMarkRate,0) as BenchMarkRate,                                                                                  
 isnull(SW.Differential,0) as Differential,                                                                                                                                
 isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                                                                   
 isnull(SW.DayCount,0) as DayCount,                                                                                                                                                                              
 isnull(SW.SwapDescription,'') as SwapDescription,                                                      
 SW.FirstResetDate as FirstResetDate,                                                                     
 SW.OrigTransDate as OrigTransDate,                                                                                                                                            
 VT.IsSwapped as IsSwapped,                                    
 VT.AllocationDate as AUECLocalDate,                                                                               
 VT.GroupID,                                                                                        
 tag.PositionTag,                                                                                                           
 COALESCE(VT.FXRate,NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0) as FXRate,                                                                                                                                           
 VT.FXConversionMethodOperator,                                                                                                
 isnull(SM.CompanyName,'') as CompanyName,                                                                                                                                                        
 isnull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                                                                           
 IsNull(SM.Delta,1) as Delta,                                                                    
 IsNull(SM.PutOrCall,'') as PutOrCall,                                                                                                                  
 VT.IsPreAllocated,                       
 VT.CumQty,                                                                                                
 VT.AllocatedQty,                                                                                  
-- Daily calculation is done on the basis of CumQty, not on quantity 
 --VT.Quantity                                                                              
 VT.TaxLotQty as Quantity,                                                                              
 IsNull(SM.StrikePrice,0) as StrikePrice,                                                                                        
 VT.UserID,                                                                                        
 VT.CounterPartyID,                                                                                    
 SM.Coupon,                                                                  
 SM.IssueDate,                                                                          
 SM.MaturityDate,                                                                   
 SM.FirstCouponDate,                                                                          
 SM.CouponFrequencyID,                                                                          
 SM.AccrualBasisID,                                                                          
 SM.BondTypeID,                                           
 SM.IsZero,                                                                
 SM.IsNDF,                                                                
 SM.FixingDate,                                                           
 VT.TaxLotQty* VT.AvgPrice*SM.Multiplier as GrossNotionalValue,              
case            
when T_Asset.Assetid=8            
then   VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses *100                                                               
 else                                                                 
 VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses   end            
as NetNotionalValue            
,                              
 T_CompanyFunds.FundName,                                                              
 VT.Commission as Commission,                                                                                          
 VT.OtherBrokerFees as Fees,                                 
 VT.ClearingFee,                                                            
 VT.MiscFees,                                                            
 VT.StampDuty,                                                            
 SM.IDCOSymbol as IDCO ,                                                            
 SM.OSISymbol as OSI,                                                             
 SM.SEDOLSymbol as SEDOL,                          
 SM.CUSIPSymbol as CUSIP ,                                           
 SM.BloombergSymbol as Bloomberg,              
 VT.Side,                                                 
 T_Asset.AssetName as Asset,                                                
 CP.ShortName as CounterParty,                                           
 IsNull(MF.MasterFundName, T_CompanyFunds.FundName) as MasterFund   ,                                          
 TP.ShortName as PrimeBroker  ,                        
SM.UnderlyingDelta,                                                                    
Case                                          
 When VT.CurrencyID <> TC.BaseCurrencyID                                           
 Then                                
  CASE                                    
   WHEN  VT.FXConversionMethodOperator = 'M'                
   THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier) * IsNull(VT.FXRate,0)                                                             
   WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
   THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier)* 1/VT.FXRate                                                                
   ELSE IsNull((VT.TaxLotQty* VT.AvgPrice*SM.Multiplier)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                         
  END                             
Else VT.TaxLotQty* VT.AvgPrice*SM.Multiplier END as GrossNotionalValueBase ,                                                                   
           
CASE            
when T_Asset.Assetid=8            
 THEN                                           
Case                                        
When VT.CurrencyID <> TC.BaseCurrencyID                                               
Then                                                           
 CASE                                                                     
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses *100) * IsNull(VT.FXRate,0)                                                             
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses *100)* 1/VT.FXRate                                                       
  ELSE IsNull((VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses*100)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                              
 END                                          
ELSE (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses *100) End          
          
          
ELSE          
          
Case                                        
When VT.CurrencyID <> TC.BaseCurrencyID                                              
Then                                                           
 CASE                                                                     
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses) * IsNull(VT.FXRate,0)                  
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses)* 1/VT.FXRate                                                       
  ELSE IsNull((VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                              
 END                                          
ELSE (VT.TaxLotQty* VT.AvgPrice*SM.Multiplier + VT.SideMultiplier*VT.TotalExpenses) End          
                            
END as NetNotionalValueBase ,                                                              
                                             
Case                                          
When VT.CurrencyID <> TC.BaseCurrencyID             
Then                                                                 
 CASE                  
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.TotalExpenses) * IsNull(VT.FXRate,0)                                                             
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.TotalExpenses)* 1/VT.FXRate                                                                
  ELSE IsNull((VT.TotalExpenses)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                             
 END                                          
ELSE VT.TotalExpenses                             
End as TotalCommissionandFeesBase ,                             
                                            
Case                                          
When VT.CurrencyID <> TC.BaseCurrencyID                                           
Then                                              
 CASE                                                 
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.Commission) * IsNull(VT.FXRate,0)                        
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.Commission)* 1/VT.FXRate                                                                
  ELSE IsNull((VT.Commission)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                    
 END                                          
ELSE VT.Commission                             
END  as CommissionBase,                                                            
                                             
Case                                          
When VT.CurrencyID <> TC.BaseCurrencyID                                           
Then                                                           
 CASE                                                                   
  WHEN  VT.FXConversionMethodOperator = 'M'                             
  THEN (VT.OtherBrokerFees) * IsNull(VT.FXRate,0)                                                             
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.OtherBrokerFees)* 1/VT.FXRate                                                                
  ELSE IsNull((VT.OtherBrokerFees)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                            
 END                            
ELSE VT.OtherBrokerFees                             
END as FeesBase,                                                 
                                          
Case                                                      
When VT.CurrencyID <> TC.BaseCurrencyID                                           
Then                                                            
 CASE                                                                   
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.ClearingFee) * IsNull(VT.FXRate,0)                                                             
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.ClearingFee)* 1/VT.FXRate                                                                
  ELSE IsNull((VT.ClearingFee)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                                     
 END                                          
ELSE VT.ClearingFee                             
END as ClearingFeeBase ,                                                            
                                             
Case                                          
When VT.CurrencyID <> TC.BaseCurrencyID                                           
Then                                                            
 CASE                                                                   
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.MiscFees) * IsNull(VT.FXRate,0)                                                             
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                      
  THEN (VT.MiscFees)* 1/VT.FXRate                                                  
  ELSE IsNull((VT.MiscFees)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                                      
 END                                          
ELSE VT.MiscFees                             
END as MiscFeesBase,                                                               
                 
Case                                          
When VT.CurrencyID <> TC.BaseCurrencyID                                           
Then                                                              
 CASE                                                                   
  WHEN  VT.FXConversionMethodOperator = 'M'                              
  THEN (VT.StampDuty) * IsNull(VT.FXRate,0)                                           
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                             
  THEN (VT.StampDuty)* 1/VT.FXRate                                                                
  ELSE IsNull((VT.StampDuty)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                                   
 END                                           
ELSE VT.StampDuty                             
END as StampDutyBase,                    
VT.LotID,                    
VT.ExternalTransID,                
VT.TradeAttribute1  ,                    
VT.TradeAttribute2  ,                  
VT.TradeAttribute3  ,                  
VT.TradeAttribute4  ,                  
VT.TradeAttribute5  ,                  
VT.TradeAttribute6 ,              
SM.ProxySymbol,                         
 SM.AssetName,                                                                                                                                                    
 SM.SecurityTypeName,                                                                                                 
 SM.SectorName ,                                                                                             
 SM.SubSectorName ,                                                                                                                                                                                                         
 SM.CountryName,                                                  
 VT.SecFee,                                                  
 VT.OccFee,                                                  
 VT.OrfFee,           
    
Case                                            
When VT.CurrencyID <> TC.BaseCurrencyID                                       
Then                                                  
 CASE                                                         
  WHEN  VT.FXConversionMethodOperator = 'M'                    
  THEN (VT.SecFee) * IsNull(VT.FXRate,0)                                                   
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                   
  THEN (VT.SecFee)* 1/VT.FXRate                                                      
  ELSE IsNull((VT.SecFee)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                           
 END                                
ELSE VT.SecFee                   
END as SecFeeBase,    
    
Case                                            
When VT.CurrencyID <> TC.BaseCurrencyID                                       
Then                                                  
 CASE                                                         
  WHEN  VT.FXConversionMethodOperator = 'M'                    
  THEN (VT.OccFee) * IsNull(VT.FXRate,0)                                                   
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                   
  THEN (VT.OccFee)* 1/VT.FXRate                                                      
  ELSE IsNull((VT.OccFee)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                           
 END                                
ELSE VT.OccFee                   
END as OccFeeBase,    
    
Case                                
When VT.CurrencyID <> TC.BaseCurrencyID                                       
Then                                             
 CASE                                                         
  WHEN  VT.FXConversionMethodOperator = 'M'                    
  THEN (VT.OrfFee) * IsNull(VT.FXRate,0)                                                   
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                   
  THEN (VT.OrfFee)* 1/VT.FXRate                                                      
  ELSE IsNull((VT.OrfFee)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                           
 END                                
ELSE VT.OrfFee                   
END as OrfFeeBase,                                                               
    
VT.ClearingBrokerFee as ClearingBrokerFee,    
    
Case                                
When VT.CurrencyID <> TC.BaseCurrencyID                                       
Then                                                 
 CASE                                                         
  WHEN  VT.FXConversionMethodOperator = 'M'                   
  THEN (VT.ClearingBrokerFee) * IsNull(VT.FXRate,0)                                                   
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0                   
  THEN (VT.ClearingBrokerFee)* 1/VT.FXRate                                                   
  ELSE IsNull((VT.ClearingBrokerFee)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                                  
 END                                 
ELSE VT.ClearingBrokerFee                   
END as ClearingBrokerFeeBase,                                                    
 VT.SoftCommission as SoftCommission,    
Case                            
When VT.CurrencyID <> TC.BaseCurrencyID                                   
Then                                
 CASE                                                     
  WHEN  VT.FXConversionMethodOperator = 'M'                
  THEN (VT.SoftCommission) * IsNull(VT.FXRate,0)                                               
  WHEN VT.FXConversionMethodOperator = 'D' and VT.FXRate > 0               
  THEN (VT.SoftCommission)* 1/VT.FXRate                                                  
  ELSE IsNull((VT.SoftCommission)* COALESCE(NonZeroFXDayRatesForTradeDate.RateValue,ZeroFXDayRatesForTradeDate.RateValue,0),0)                                      
 END                            
ELSE VT.SoftCommission               
END  as SoftCommissionBase,      
 VT.TaxOnCommissions,      
VT.TransactionLevy,      
VT.SideMultiplier,    
TransactionType,  
SM.ReutersSymbol
,COALESCE(LTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency
,VT.AdditionalTradeAttributes
into #TemporaryTable                                         
from #V_Taxlots VT                                                  
 Inner join #TempPosTag tag on VT.TaxlotID = tag.TaxlotID                                               
 Inner Join T_CompanyFunds  on T_CompanyFunds.CompanyFundID =  VT.FundID                                               
 inner JOIN T_Company AS TC on T_CompanyFunds.CompanyID = TC.CompanyID    
 Left outer join T_CompanyMasterFundSubAccountAssociation CMF on CMF.CompanyFundID = VT.FundID                                       
 Left outer join T_companyMasterFunds MF on MF.CompanyMasterFundID = CMF.CompanyMasterFundID                                      
Left outer join T_ThirdParty TP on TP.ThirdPartyID = T_CompanyFunds.CompanyThirdPartyID                                                  
 Left outer join  T_SwapParameters SW on VT.GroupID=SW.GroupID                                                 
 Left Outer Join #SecMasterDataTempTable SM ON VT.Symbol = SM.TickerSymbol                                                  
 left outer join T_CounterParty CP on CP.CounterPartyID = VT.CounterPartyID                                            
 Inner join T_Asset on T_Asset.AssetID  = VT.AssetID
LEFT OUTER JOIN T_currency CUR1 ON CUR1.CurrencyID = T_CompanyFunds.LocalCurrency
	LEFT OUTER JOIN T_currency GCUR ON GCUR.CurrencyID = VT.SettlCurrency_Group
	LEFT OUTER JOIN T_currency LTCUR ON LTCUR.CurrencyID = VT.SettlCurrency_Taxlot                                        
 Left outer join #ZeroFundFXRates ZeroFXDayRatesForTradeDate                                                    
 on (ZeroFXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID And ZeroFXDayRatesForTradeDate.ToCurrencyID = TC.BaseCurrencyID                                                    
 And DateDiff(d,VT.AUECLocalDate,ZeroFXDayRatesForTradeDate.Date)=0 and ZeroFXDayRatesForTradeDate.FundId = VT.FundID)
 Left outer join #NonZeroFundFXRates NonZeroFXDayRatesForTradeDate                                                    
 on (NonZeroFXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID And NonZeroFXDayRatesForTradeDate.ToCurrencyID = TC.BaseCurrencyID                                                    
 And DateDiff(d,VT.AUECLocalDate,NonZeroFXDayRatesForTradeDate.Date)=0 and NonZeroFXDayRatesForTradeDate.FundId = VT.FundID)        
     
     
DECLARE @sqlCommand VARCHAR(MAX)                  
SET @sqlCommand  = 'SELECT * FROM #TemporaryTable WHERE 1=1 '                       
    
        
If(@CustomConditions is not null)    
Begin    
   SET @sqlCommand=  @sqlCommand + @CustomConditions       
End        
      
EXEC (@sqlCommand)        
        
Drop Table #TemporaryTable        
Drop table #V_Taxlots                                                   
Drop table #AssetClass                                                              
Drop table #Funds                                                
Drop Table #SecMasterDataTempTable                                                            
Drop table #TempPosTag                                                              
Drop Table #ZeroFundFXRates                                                             
Drop Table #NonZeroFundFXRates
                                                                                                               
RETURN;    
                                                                                                                                  
End    
