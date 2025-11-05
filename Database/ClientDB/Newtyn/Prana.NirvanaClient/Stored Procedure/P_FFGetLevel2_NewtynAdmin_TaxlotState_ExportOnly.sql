/*    
 Created By: Ankit Gupta    
 Date: November 02, 2013    
 Description: To fetch Level2 Intraday trades'data for closed and open taxlots with their respective closing information     
     and maintain taxlot states for Executing brokers and Administrator.    
*/    
    
CREATE PROCEDURE [dbo].[P_FFGetLevel2_NewtynAdmin_TaxlotState_ExportOnly]                                                                                                                                                   
(                                                                                                                                                                  
 @thirdPartyID int,                                                                                                                                                                  
 @companyFundIDs varchar(max),                                                                                                                                                                  
 @inputDate datetime,                                                                                                                                                              
 @companyID int,                                                                                                                              
 @auecIDs varchar(max),                                                                    
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                                            



 @dateType int, -- 0 means for Process Date and 1 means Trade Date.       
 @fileFormatID int                                                                                                                                                                
)                                                                                                                                                                  
AS 

--Declare 
--@thirdPartyID int,                                                                                                                                                                  
-- @companyFundIDs varchar(max),                                                                                                                                                                  
-- @inputDate datetime,                                                                                                                                                              
-- @companyID int,                                                                                                                              
-- @auecIDs varchar(max),                                                                    
-- @TypeID int,
-- @dateType int,
-- @fileFormatID int 
 
-- Set @thirdPartyID=12
--Set @companyFundIDs=N'1199,1211,1268,1269,1214,1215,1270,1271,1274,1276,1275,1277,1212,1213,1272,1273'
--Set @inputDate='2021-05-21 08:37:35'
--Set @companyID=8
--Set @auecIDs=N'44,34,43,31,57,59,21,82,18,74,1,15,62,73,12,80'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=29                                                                            
                                                                        
Declare @Fund Table                                                                                                                            
(                                                                                                                            
FundID int                                                                                                                        
)                                                                           
                       
Declare @AUECID Table                                                                                                                            
(                                                                                                                            
AUECID int                                                                                                                        
)                                                           
                                                    
Insert into @Fund                                                                          
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')                                                                             
                                                   
Insert into @AUECID                                
Select Cast(Items as int) from dbo.Split(@auecIDs,',')                                 
                                
Create table #VT                                
(                                
TaxLotID varchar(50),                                                                          
FundID Int,                                                         
OrderTypeTagValue varchar(3),                                             
OrderSideTagValue varchar(3),                                                                                                 
Symbol varchar(100),                                                                             
CounterPartyID Int,                                                          
VenueID Int,                                                                                                             
OrderQty float,                                
AvgPrice float,                                                                                                                                                       
CumQty float,                                                                                                                              
Quantity float,                                                                                                                                                              
AUECID Int,                                                                                                                                                                  
AssetID Int,                                                                                          
UnderlyingID Int,                                                                                                   
ExchangeID Int,                                                                                             
CurrencyID Int,                                
GroupRefID Int,                                                                                                                                                                   
Level1AllocationID varchar(50),                                                                                                                                                             
Level2Percentage float,                                                                                             
TaxLotQty float,                                                                                 
SettlementDate datetime,                                                                                                                             
Commission float,                                                                                                                                                                  
OtherBrokerFees float,                                                                                                                      
TaxlotState varchar(50),                            
StampDuty float,                                                                                                             
TransactionLevy float,                                                                                                                  
ClearingFee float,                               
TaxOnCommissions float,                                              
MiscFees float,                                                                                                      
AUECLocalDate datetime,            
Level2ID int,                                                                            
FXRate float,                                                            
FXConversionMethodOperator varchar(10),                                                          
FromDeleted varchar(10),                                                    
ProcessDate datetime,                                             
OriginalPurchaseDate datetime,                                                 
AccruedInterest float,                                         
FXRate_Taxlot float,                      
FXConversionMethodOperator_Taxlot varchar(10),                              
SideMultiplier Int,                                              
Description varchar(200),      
ForexRate float   ,
SecFee float                                   
)                                
                                
Insert into #VT                                
Select                                                                                      
VT.TaxLotID as TaxLotID,                                                                                       
VT.FundID as AccountID,                                                                                                     
VT.OrderTypeTagValue,                                                                                                                         
VT.OrderSideTagValue ,                                                        
VT.Symbol,          
Case           
When           
VT.CounterPartyID = '-2147483648' OR VT.CounterPartyID Is Null                                                                                                               
then 0          
Else VT.CounterPartyID                                                                                        
End as CounterPartyID,          
IsNull(VT.VenueID,0) As VenueID,                                                                           
VT.TaxLotQty as OrderQty,                                
VT.AvgPrice,                                                                                                                                                       
VT.CumQty,                                                                                                                              
VT.Quantity,                                                                                                                                                              
VT.AUECID,                                                                                                                                                                  
VT.AssetID,                                                                                          
VT.UnderlyingID,                                                                                                   
VT.ExchangeID,                                                                                             
VT.CurrencyID,                                
VT.GroupRefID,                                                                                                                                                                   
VT.Level1AllocationID as Level1AllocationID,                                                                
VT.Level2Percentage,                                          
VT.TaxLotQty,        
VT.SettlementDate,                                                                                                                             
VT.Commission As Commission,                                                                                                                                                                  
VT.OtherBrokerFees As OtherBrokerFees ,                                                                                                                
0 as TaxlotState,                                                       
VT.StampDuty as StampDuty,                                                                                                                          
VT.TransactionLevy as TransactionLevy,                                                                                       
VT.ClearingFee as ClearingFee,                                                                               
VT.TaxOnCommissions as TaxOnCommissions,                                          
VT.MiscFees as MiscFees ,                            
VT.AUECLocalDate,                                     
VT.Level2ID as Level2ID,                                                                                                 
ISNULL(VT.FXRate,0) As FXRate,                                                            
VT.FXConversionMethodOperator,                                                          
'No' as FromDeleted,                                                    
VT.ProcessDate,                                                        
VT.OriginalPurchaseDate,                          
ISNULL(VT.AccruedInterest,0) As AccruedInterest,                                                      
ISNULL(VT.FXRate_Taxlot,0) As FXRate_Taxlot,                                                  
VT.FXConversionMethodOperator_Taxlot ,                              
SideMultiplier,                                              
VT.Description,      
1  ,
VT.SecFee                                               
 From V_TaxLots VT 
Inner Join @Fund F On F.FundID = VT.FundID 
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID                                                                          
WHERE Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) >= 0
			
Union All                                                                                                    
                                                                                              
  select                                                                                                                                                                     
   TDT.TaxLotID as TaxLotID,                                                                                                                              
  ISNULL(TDT.FundID,0) as AccountID,                                                                 
  TDT.OrderTypeTagValue,                                   
  TDT.OrderSideTagValue as SideID,                                                                                                                                                                     
  TDT.Symbol,                                  
  ISNULL(TDT.CounterPartyID,0) As CounterPartyID,                                                                                                                                                             
  IsNull(TDT.VenueID,0) As VenueID,                                   
  TDT.TaxLotQty as OrderQty,                    
  TDT.AvgPrice,                                                                                                                                                      
  TDT.CumQty,            
  TDT.Quantity,                                      
  TDT.AUECID,                                                                
  TDT.AssetID,                                                                                                                                           
  TDT.UnderlyingID,                                                                                                                                              
  TDT.ExchangeID,                           
  TDT.CurrencyID ,                                  
  TDT.GroupRefID,                                                                                                                                               
  TDT.Level1AllocationID as Level1AllocationID,                                          
  TDT.Level2Percentage,                                                              
  TDT.TaxLotQty as AllocatedQty,                                    
  TDT.SettlementDate,                             
  TDT.Commission As Commission,                                                                         
  TDT.OtherBrokerFees As OtherBrokerFees,                                                                                                               
  TDT.TaxLotState ,                                                                                                                                          
  TDT.StampDuty as StampDuty,                                                                                             
  TDT.TransactionLevy as TransactionLevy,                                          
  TDT.ClearingFee as ClearingFee,                                                                
  TDT.TaxOnCommissions as TaxOnCommissions,                                              
  TDT.MiscFees as MiscFees,                                                                
  TDT.AUECLocalDate,                                                      
  0 as Level2ID,                                                                  
  ISNULL(TDT.FXRate,0) as FXRate,                                                              
  TDT.FXConversionMethodOperator as FXConversionMethodOperator,                                    
 'Yes' as FromDeleted,                                                          
 TDT.ProcessDate,                                                          
 TDT.OriginalPurchaseDate,                                                          
 IsNull(TDT.AccruedInterest,0) As AccruedInterest,                                     
 IsNull(TDT.FXRate_Taxlot,0) As FXRate_Taxlot,                                                    
 TDT.FXConversionMethodOperator_Taxlot ,                                 
Case                                
WHEN (ORderSideTagValue IN ('2', '5', '6', 'C', 'D'))                                                             
 THEN  -1                                
 ELSE 1                                
END As SideMultiplier,                                               
 TDT.Description,      
 1 ,
 TDT.SecFee                       
From T_DeletedTaxLots TDT 	
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID
WHERE Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) >= 0
AND FileFormatID = @fileFormatID
AND TDT.TaxLotState = 3
	
			          
Update #VT          
Set VenueID = 1         
      
Update #VT      
Set ForexRate = FX.ConversionRate      
from #VT      
INNER JOIN T_CurrencyConversionRate FX on #VT.CurrencyID = FX.CurrencyPairID_FK      
and DateDiff(D, #VT.AUECLocalDate, FX.Date) = 0

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = #VT.TaxlotID
WHERE PB.TaxlotState <> 0 AND PB.FileFormatID = @fileFormatID       
                                                                    
Select * into #VTaxlot from #VT                                      
                                                
 Select                           
VT.TaxLotID,                
T_CompanyFunds.FundShortName as AccountName,                                         
T_Side.Side as Side,                                                                              
VT.Symbol,                                                                                 
T_CounterParty.ShortName as CounterParty,                                                                                                                                                 
VT.TaxLotQty as OrderQty,                                                        
VT.AvgPrice as AveragePrice,                                                                                                                                   
VT.CumQty as ExecutedQty,                                                                                                                                                              
VT.Quantity as TotalQty,                                                                                                                                                                 
VT.GroupRefID,                                
T_Asset.AssetName as Asset,                                                                                                                                  
T_Exchange.DisplayName as Exchange,                                                                                                                                                              
Currency.CurrencySymbol,                                                                         
CTPM.MappedName,                                         
CTPM.FundAccntNo,                                                                                                                                   
FT.FundTypeName,                                                                                                                                        
VT.TaxLotID as EntityID,                                                                                                                                                 
VT.Level2Percentage as Level2Percentage,                                                                                             
VT.TaxLotQty as AllocatedQty,                                 
SM.PutOrCall,                                                                             
ISNULL(SM.StrikePrice, 0) As StrikePrice,                                                                                                               
convert(varchar,SM.ExpirationDate,101) as ExpirationDate,                                                                                                 
convert(varchar,VT.SettlementDate,101) as SettlementDate,                                                                       
VT.Commission as CommissionCharged,                                                         
VT.OtherBrokerFees as OtherBrokerFee,                                                                                                             
T_ThirdPartyType.ThirdPartyTypeName,                              
VT.SecFee ,                                                                                                  
Case                                         
  When VT.TaxLotState = 0          
  Then 'Allocated'                                        
  When VT.TaxLotState = 1                                        
  Then 'Sent'                                        
  When VT.TaxLotState = 2                                        
  Then 'Amemded'                 
  When VT.TaxLotState = 3            
  Then 'Deleted'                         
  When VT.TaxLotState = 4                                        
  Then 'Ignored'                                        
End As TaxLotState,                        
VT.FromDeleted,                                                                                                                 
ISNULL(VT.StampDuty,0) as StampDuty,                                                                                             
ISNULL(VT.TransactionLevy,0) as TransactionLevy,                                                                                                                                
ISNULL(VT.ClearingFee,0) as ClearingFee,                                                                                                                                
ISNULL(VT.TaxOnCommissions,0) as TaxOnCommissions,                                                                                                                 
ISNULL(VT.MiscFees,0) as MiscFees ,                                                          
convert(varchar,VT.AUECLocalDate,101) as TradeDate,                                                                                                         
ISNULL(SM.Multiplier, 1) as AssetMultiplier,                       
Case            
When T_CounterParty.ShortName = 'GWEP' and VT.AssetID = 8                    
then  Cast(Cast(((Round(VT.AvgPrice, 4) * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees + VT.SecFee) * 0.01)) As Decimal(32,2)) As Varchar(500))       
          
when T_CounterParty.ShortName = 'GWEP'            
then   Cast(Cast(((Round(VT.AvgPrice, 4) * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees + VT.SecFee) * VT.SideMultiplier)) As Decimal(32,2)) As Varchar(500))                  
When VT.AssetID = 8 and T_CounterParty.ShortName != 'GWEP'                   
then  Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees + VT.SecFee) * 0.01)) As Decimal(32,2)) As Varchar(500))                  
   
Else                    
Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees + VT.SecFee) * VT.SideMultiplier)) As Decimal(32,2)) As Varchar(500))  
          
End As NetAmount,                                                                                                        
VT.Level2ID,                                                                                             
SM.ISINSymbol,                                        
SM.CUSIPSymbol,                                                                                                      
SM.SEDOLSymbol,     
SM.ReutersSymbol,                                                                                                      
SM.BloombergSymbol as BBCode,                                                      
SM.CompanyName as FullSecurityName,                                                                                                      
SM.UnderlyingSymbol,                                                                                          
SM.LeadCurrency,                                                            
SM.VsCurrency,                                                                  
SM.OSISymbol,                                                         
SM.IDCOSymbol,                                                                      
SM.OpraSymbol,                                                            
VT.FXRate,                                          
VT.FXConversionMethodOperator,  
VT.ProcessDate,                                                        
VT.OriginalPurchaseDate,                                                        
VT.AccruedInterest,                                                                                                       
T_Country.CountryName as CountryName,                                                    
VT.FXRate_Taxlot,                                                  
VT.FXConversionMethodOperator_Taxlot,                                              
ISNULL(SM.AssetName,'') As UDAAssetName,                                          
ISNULL(SM.SecurityTypeName, '') As UDASecurityTypeName,                                          
ISNULL(SM.SectorName, '') As UDASectorName,                                          
ISNULL(SM.SubSectorName, '') As UDASubSectorName,                                          
ISNULL(SM.CountryName, '') As UDACountryName,                                      
VT.Description,                                 
ISNULL(T_ClosingAlgos.AlgorithmAcronym, '') as ClosingAlgo,                                
case                                
when Closing.ClosedQty is null                                
then 0                                 
else Closing.ClosedQty                                
end as ClosedQty,                                
ISNULL(Closing.OpenPrice, 0) as OpenPriceAgainstClosing,                                
convert(varchar,#VTaxlot.AUECLocalDate, 101) as TradeDateAgainstClosing,                                
VT.Level1AllocationID,      
ISNULL(VT.ForexRate, 1) as ForexRate                                                                                                 
From #VT VT  
Inner Join V_SecMasterData SM On SM.TickerSymbol=VT.Symbol                                      
inner join T_CompanyThirdPartyMappingDetails as CTPM on  CTPM.InternalFundNameID_FK = VT.FundID                                                                                  
inner join T_FundType as FT on FT.FundTypeID = CTPM.FundTypeID_FK 
INNER JOIN dbo.T_CompanyThirdParty on T_CompanyThirdParty.CompanyThirdPartyID=CTPM.CompanyThirdPartyID_FK 
INNER JOIN dbo.T_ThirdParty on T_ThirdParty.ThirdPartyId=T_CompanyThirdParty.ThirdPartyId AND  T_CompanyThirdParty.CompanyID=@companyID                                                                                                                      





INNER JOIN dbo.T_ThirdPartyType on T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID      
INNER join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                                                                                                  
INNER join  T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue                           
INNER join T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID                                      
INNER join T_Asset ON T_Asset.AssetID = VT.AssetID   
INNER join  T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID                                         
Left Outer Join PM_TaxLotClosing Closing On Closing.ClosingTaxLotID = VT.TaxLotID                                
Left OUTER JOIN T_ClosingAlgos On T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo                               
Left Outer Join #VTaxlot  On Closing.PositionalTaxLotID = #VTaxlot.TaxLotID                                
left join T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID                     
left join T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                                                   
Where                                                          
 (
 (VT.TaxLotState <> 1) OR 
 (VT.TaxLotState = 1 AND Datediff(DAY,VT.ProcessDate,@inputdate)=0)                          
  )                   
  And ((VT.TaxLotState <> 4) OR 
  (VT.TaxLotState = 4 AND Datediff(DAY,VT.ProcessDate,@inputdate)=0)                        
  )                                                          
                                                                              
Order by TaxlotId                                                
                                
Drop Table #VT, #VTaxlot