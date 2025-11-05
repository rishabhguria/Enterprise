CREATE PROCEDURE [dbo].[P_GetTaxlots_Automailer_Batch_Pharvision]                                                                                           
(                                                                                                          
 @inputDate datetime                                                                                                
)                                                                                                             
AS              
         
--Declare  @inputDate datetime      
--Set @inputDate='2025-09-16'    
    
Declare @CompanyFundIDs VARCHAR(200)    
Set @CompanyFundIDs = '8'    
    
DECLARE @Fund TABLE (FundID INT)    
INSERT INTO @Fund    
SELECT Cast(Items AS INT)    
 FROM dbo.Split(@CompanyFundIDs, ',')    
    
Select                     
    
 VT.TaxlotID as TaxlotID,    
 VT.FundID as FundID,    
 CF.FundName As AccountName,    
 VT.OrderSideTagValue as SideID,                                                                                                            
 T_Side.Side as Side,                                                                                                     
 VT.Symbol,                                                                                                           
 VT.CounterPartyID,                   
 T_CounterParty.ShortName as CounterParty ,    
 VT.AvgPrice,    
 VT.CumQty,    
 VT.Quantity,    
 T_Asset.AssetName as Asset,                                                                                                    
 Currency.CurrencyID,                                                                                                          
 Currency.CurrencySymbol,                   
 VT.Level1AllocationID as Level1AllocationID,      
 VT.TaxLotQty As AllocatedQty,    
 SM.ExpirationDate,    
 VT.SettlementDate As SettlementDate,    
 (VT.Commission) As CommissionCharged,    
 (VT.OtherBrokerFees) As OtherBrokerFees,    
 (VT.SecFee) As Secfee,      
 VT.GroupRefID,                                                                                
 (VT.StampDuty) as StampDuty,    
 (VT.TransactionLevy) as TransactionLevy,     
 (ClearingFee) as ClearingFee,    
 (TaxOnCommissions) as TaxOnCommissions,    
 (MiscFees) as MiscFees ,    
 VT.AUECLocalDate As TradeDate,    
 SM.Multiplier,                                                  
 SM.ISINSymbol,                                              
 SM.CUSIPSymbol,                                              
 SM.SEDOLSymbol,                                              
 SM.BloombergSymbol,                                              
 SM.CompanyName,                                              
 SM.UnderlyingSymbol,                                  
 Convert(money,CASE                 
 WHEN VT.CurrencyID <> CF.LocalCurrency                
 THEN CASE        
 WHEN IsNull(VT.FXRate, 0) <> 0                
 THEN CASE                 
 WHEN VT.FXConversionMethodOperator = 'M'                
 THEN ((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0) )               
 WHEN VT.FXConversionMethodOperator = 'D'                
 AND VT.FXRate > 0                
 THEN ((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / NULLIF(VT.FXRate,0))             
 END     
 ELSE 0              
 END                
 ELSE ((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses))    
 END)  AS NetNotionalValueBase,    
      
 (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) AS NetNotionalValue ,    
 (VT.OccFee) as OccFee,    
 (VT.OrfFee) as ORFFees,    
 (VT.ClearingBrokerFee) as ClearingBrokerFee,    
 (VT.SoftCommission) as SoftCommission,    
 SettleCurr.CurrencySymbol As SettlementCurrency,    
 CONVERT(Decimal(38,9),Round((ISNull(VT.FXRate_Taxlot,0)),8)) As TradeFXRate,    
 ISNull(VT.IsSwapped,0) AS IsSwapped,    
 VT.SideMultiplier    
    
InTo #Temp_Taxlots             
From V_TaxLots  VT with (Nolock)    
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID    
Inner Join T_CompanyFunds CF with (Nolock) ON CF.CompanyFundID = VT.FundID     
Inner Join T_Currency as Currency with (Nolock) On Currency.CurrencyID = VT.CurrencyID                                
Inner Join T_Side with (Nolock) ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue    
Inner Join V_SecMasterData SM with (Nolock) On SM.TickerSymbol=VT.Symbol                
Inner Join T_Asset with (Nolock) On T_Asset.AssetID = VT.AssetID                     
Inner Join T_CounterParty with (Nolock) On T_CounterParty.CounterPartyID=VT.CounterPartyID       
Left Outer Join T_Currency SettleCurr with (Nolock) On SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot     
Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0    
And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire'     
    
/*    
Delta values    
*/    
    
Select Max(TableID) As TableID    
InTo #Temp_TMGTableId    
From T_AutomailerMultipleTimeGenerationData MG    
INNER JOIN @Fund Fund ON Fund.FundID = MG.FundID    
Where DateDiff(Day,MG.AUECLocalDate, @inputDate) = 0    
Group By MG.TaxlotId    
    
SELECT         
    VT.TaxlotID,        
 --   VT.Symbol,    
 --   VT.TradeDate,    
 --   VT.SettlementDate,    
 --   VT.AccountName,    
 --   (VT.AllocatedQty - MG.TaxlotQty) AS AllocatedQty,    
 --   VT.Side,    
 --   CAST(((((VT.AvgPrice * VT.CumQty * VT.Multiplier) - (MG.AvgPrice * MG.CumQty * MG.Multiplier)) / (VT.CumQty - MG.CumQty)) / VT.Multiplier) AS DECIMAL(18,8)) AS AvgPrice,    
 --   VT.CumQty,    
 --   VT.Quantity,        
 --   VT.CounterParty,       
 --   VT.CurrencySymbol,    
 --   VT.SettlementCurrency,    
 --(VT.CommissionCharged - MG.Commission) AS CommissionCharged,    
 --(VT.OtherBrokerFees - MG.OtherBrokerFees) AS OtherBrokerFees,    
 --(VT.StampDuty - MG.StampDuty) AS StampDuty,    
 --(VT.TransactionLevy - MG.TransactionLevy) AS TransactionLevy,    
 --(VT.ClearingFee - MG.ClearingFee) AS ClearingFee,    
 --(VT.TaxOnCommissions - MG.TaxOnCommissions) AS TaxOnCommissions,    
 --(VT.MiscFees - MG.MiscFees) AS MiscFees,    
 --(VT.SecFee - MG.SecFee) AS SecFee,    
 --(VT.OccFee - MG.OccFee) AS OccFee,    
 --(VT.ORFFees - MG.ORFFees) AS OrfFee,    
 --(VT.ClearingBrokerFee - MG.ClearingBrokerFee) AS ClearingBrokerFee,    
 --   (VT.SoftCommission - MG.SoftCommission) AS SoftCommission,    
 --   VT.ExpirationDate,    
 --   VT.ISINSymbol,    
 --   VT.CUSIPSymbol,    
 --   VT.SEDOLSymbol,    
 --   VT.BloombergSymbol,    
 --   VT.CompanyName,    
 --VT.Multiplier,    
 --VT.SideMultiplier,    
 --VT.FundID,    
 --Cast(0 As Float) As TotalCommissionAndFee,    
 --Cast(0 As Float) As GrossAmount,    
 --Cast(0 As Float) As NetAmount,    
 VT.TaxlotID As ExternalRef    
    
InTo #Temp_DeltaTaxlots    
FROM #Temp_Taxlots VT    
Inner join T_AutomailerMultipleTimeGenerationData MG ON MG.TaxlotID = VT.TaxlotID AND DATEDIFF(DAY, VT.TradeDate, MG.AUECLocalDate) = 0    
And VT.FundID = MG.FundID    
Inner Join #Temp_TMGTableId TID On TID.TableID = MG.TableID    
WHERE (VT.AllocatedQty - ISNULL(MG.TaxlotQty, 0)) > 0    
    
--Update #Temp_DeltaTaxlots    
--Set TotalCommissionAndFee = (CommissionCharged + OtherBrokerFees  + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee + SoftCommission),    
--GrossAmount  = (AvgPrice * AllocatedQty * Multiplier),    
--NetAmount = (AvgPrice * AllocatedQty * Multiplier) +     
-- (CommissionCharged + OtherBrokerFees  + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee + SoftCommission) * SideMultiplier     
    
    
------ Get new taxlots which are not yet sent    
    
Create Table #Temp_AlreadyGeneratedTaxlots    
(    
TaxlotID Varchar(50)    
)    
    
Insert InTo #Temp_AlreadyGeneratedTaxlots    
Select MG.TaxlotID     
From T_AutomailerMultipleTimeGenerationData MG     
Inner join #Temp_Taxlots VT On VT.FundID = MG.FundID And VT.TaxlotID = MG.TaxLotID     
Where DateDiff(Day,VT.TradeDate , MG.AUECLocalDate) = 0    
    
    
Create Table #Temp_NewTaxlotIDs    
(    
TaxlotID varchar(50),    
ExternalRef Varchar(50)    
)    
    
---- Get New TaxlotID which comes first time    
Insert InTo #Temp_NewTaxlotIDs    
    
Select VT.TaxlotID, '' As ExternalRef    
From #Temp_Taxlots VT    
Where VT.TaxlotID Not In    
(    
    Select TaxlotID from #Temp_AlreadyGeneratedTaxlots    
)    
    
---- Get Delta TaxlotID    
Insert InTo #Temp_NewTaxlotIDs    
    
Select VT.TaxlotID, D.ExternalRef      
From #Temp_Taxlots VT    
Inner join #Temp_DeltaTaxlots D On D.TaxlotID = VT.TaxlotID    
    
    
SELECT         
    VT.TaxlotID,        
    VT.Symbol,    
    VT.TradeDate,    
    VT.SettlementDate,    
    VT.AccountName,    
    VT.AllocatedQty,    
    VT.Side,    
    VT.AvgPrice,    
    VT.CumQty,    
    VT.Quantity,        
    VT.CounterParty,       
    VT.CurrencySymbol,    
    VT.SettlementCurrency,    
    VT.CommissionCharged,    
    VT.OtherBrokerFees,    
    VT.StampDuty,    
    VT.TransactionLevy,    
    VT.ClearingFee,    
    VT.TaxOnCommissions,    
    VT.MiscFees,    
    VT.SecFee,    
    VT.OccFee,    
    VT.ORFFees,    
    VT.ClearingBrokerFee,    
    VT.SoftCommission,    
    VT.ExpirationDate,    
    VT.ISINSymbol,    
    VT.CUSIPSymbol,    
    VT.SEDOLSymbol,    
    VT.BloombergSymbol,    
    VT.CompanyName,    
    VT.Multiplier,    
    VT.SideMultiplier,    
    N.ExternalRef As ExternalRef,  
 Case   
  When Side = 'Sell'  
  Then  1  
  When Side = 'Sell short'  
  Then 2  
  When Side = 'Buy to Close'  
  Then 3  
  When Side = 'Buy'  
  Then 4  
 Else 1  
 End As CustomOrderBy    
InTo #Temp_NewTaxlots    
From #Temp_Taxlots VT    
Inner Join #Temp_NewTaxlotIDs N On N.TaxlotID = VT.TaxlotID    
    
SELECT     
 Multi.TaxlotID     
Into #Temp_AlreadyExistingTaxlots    
 FROM T_AutomailerMultipleTimeGenerationData Multi     
 Inner Join #Temp_Taxlots T ON Multi.TaxLotID = T.TaxlotID AND Multi.AvgPrice = T.AvgPrice AND Multi.TaxLotQty = T.AllocatedQty    
 AND Multi.CumQty = T.CumQty And Multi.FundID = T.FundID    
 Where Datediff(DAY,Multi.AUECLocalDate,T.TradeDate) = 0    
    
    
Insert InTo T_AutomailerMultipleTimeGenerationData    
Select    
GetDate() As FileGenerationDateTime,    
 TaxlotID As TaxLotID ,    
 FundID,    
 Symbol,    
 TradeDate As AUECLocalDate,    
 AvgPrice As AvgPrice,    
 CumQty,    
 AllocatedQty As TaxLotQty,    
 CommissionCharged As Commission ,    
 OtherBrokerFees,    
 StampDuty,    
 TransactionLevy,    
 ClearingFee,    
 TaxOnCommissions,    
 MiscFees,    
 SecFee,    
 OccFee,    
 ORFFees,    
 ClearingBrokerFee,    
 SoftCommission,    
 SideID As OrderSideTagValue,    
 Multiplier,    
 CounterPartyID    
    
From #Temp_Taxlots T    
WHERE T.TaxLotID Not In     
(    
 Select TaxlotID from #Temp_AlreadyExistingTaxlots    
)    
    
Select     
TaxlotID As TradeRefID,    
Convert(char(10), TradeDate, 101) as TradeDate,    
Convert(char(10), SettlementDate, 101) as SettlementDate,    
AccountName As Account,    
Counterparty As CounterParty,    
Side,    
SEDOLSymbol,    
CONVERT(Decimal(38,9),AllocatedQty) as AllocatedQty,     
CurrencySymbol As CurrencySymbol,    
Round(Convert(Decimal(18,8),AvgPrice),4) as AveragePrice,    
SettlementCurrency,    
ExternalRef    
From #Temp_NewTaxlots    
Order By CustomOrderBy  
FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')    
  
Drop Table #Temp_Taxlots, #Temp_DeltaTaxlots, #Temp_TMGTableId, #Temp_AlreadyGeneratedTaxlots, #Temp_NewTaxlots    
Drop Table #Temp_AlreadyExistingTaxlots, #Temp_NewTaxlotIDs