    
CREATE PROCEDURE [dbo].[P_FFGetLevel2Data_G2CompanyThirdParty_Export]                                                                                                                                                 
(                                                                                                                                                                
 @thirdPartyID int,                                                                                                                                                                
 @companyFundIDs varchar(max),                                                                                                                                                                
 @inputDate datetime,                                                                                                                                                            
 @companyID int,                                                                                                                            
 @auecIDs varchar(max),                                                                  
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                                            
 @dateType int -- 0 means for Process Date and 1 means Trade Date.        
,@fileFormatID int                                                                                                                                                             
)                                                                                                                                                         
AS                                                                       
    
--Declare @thirdPartyID int,                                                                                                                                                                
-- @companyFundIDs varchar(max),                                                                                                                                                                
-- @inputDate datetime,                                                                                                                                                            
-- @companyID int,                                                                                                                            
-- @auecIDs varchar(max),                                                                  
-- @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                                            
-- @dateType int -- 0 means for Process Date and 1 means Trade Date.        
--,@fileFormatID int 

--Set @thirdPartyID=38
--Set @companyFundIDs=N'1234,1238'
--Set @inputDate='2021-05-11 04:27:53'
--Set @companyID = 7
--Set @auecIDs=N'63,44,34,43,59,31,54,18,61,74,103,1,15,11,62,73,12,32,81'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=3
                                                                   
    
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
OrderTypeTagValue varchar(10),                   
SideID varchar(10),                                                                                     
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
LotID varchar(50)                 
)                              
    
Insert into #VT                              
Select                                                         
	VT.TaxLotID as TaxLotID,                                                                      
	VT.FundID as FundID,                                                                                                   
	VT.OrderTypeTagValue,                                                                                                                                       
	VT.OrderSideTagValue as SideID,                                                                                          
	VT.Symbol,                                                                                                                            
	IsNull(VT.CounterPartyID,0) As CounterPartyID,                                                                                                 
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
	ISNULL(VT.Commission,0) As Commission,                                                                                                                                                                
	IsNull(VT.OtherBrokerFees,0) As OtherBrokerFees ,                                                  
	0 as TaxlotState,                                                     
	ISNULL(VT.StampDuty,0) as StampDuty,                                                                                                                        
	ISNULL(VT.TransactionLevy,0) as TransactionLevy,                       
	ISNULL(ClearingFee,0) as ClearingFee,                                                                             
	ISNULL(TaxOnCommissions,0) as TaxOnCommissions,           
	ISNULL(MiscFees,0) as MiscFees ,                                                                                                                          
	VT.AUECLocalDate,                                   
	VT.Level2ID as Level2ID,                                            
	IsNull(VT.FXRate,0) As FXRate,                                                           
	VT.FXConversionMethodOperator,                                
	'No' as FromDeleted,                                                  
	VT.ProcessDate,                                                      
	VT.OriginalPurchaseDate,                                                      
	IsNull(VT.AccruedInterest,0) As AccruedInterest,                                                    
	IsNull(VT.FXRate_Taxlot,0) As FXRate_Taxlot,                        
	VT.FXConversionMethodOperator_Taxlot ,                            
	SideMultiplier,                                            
	VT.Description,                        
	VT.LotID                               
 From V_TaxLots VT
Inner Join @Fund F On F.FundID = VT.FundID 
Inner Join @AUECID AUEC On AUEC.AUECID  = VT.AUECID                                                                                      
	Where 
		Datediff(DAY,                              
		(                               
			Case                               
			When @DateType = 1                     
			Then VT.AUECLocalDate                               
			Else VT.ProcessDate                              
			End                                                
		),@inputdate) = 0         
    
    
Update #VT            
Set VenueID =1            
Select * into #VTaxlot from #VT                                    
    
 Select                              
	VT.TaxLotID,                                                                                             
	T_CompanyFunds.FundShortName as AccountName,                                                                                                 
	T_Side.Side as Side,                                                                            
	VT.Symbol,                                                                               
	T_CounterParty.ShortName as CounterParty,                                                                                                                                               
	VT.TaxLotQty as OrderQty,--AllocatedQty                                                       
	VT.AvgPrice as AveragePrice,        
	VT.CumQty as ExecutedQty, --ExecutedQty                                                                                                                                                                
	VT.Quantity as TotalQty, --TotalQty                                          
	VT.GroupRefID,                              
	T_Asset.AssetName as Asset,                                                                                                                                                             
	IsNull(T_Exchange.DisplayName,'') as Exchange,                                                                              
	Currency.CurrencySymbol,                                                                       
	CTPM.MappedName,                                                                        
	CTPM.FundAccntNo,                                                                                                       
	FT.FundTypeName,                                                                                                                                                                 
	VT.Level1AllocationID as EntityID,                                                                                                                             
	VT.Level2Percentage as Level2Percentage,                                                                                                                                
	VT.TaxLotQty as AllocatedQty,                                                                                                                                 
	SM.PutOrCall,                                                                           
	IsNull(SM.StrikePrice,0) as StrikePrice,                                                                                                             
	convert(varchar,SM.ExpirationDate,101) as ExpirationDate,                                             
	convert(varchar,VT.SettlementDate,101) as SettlementDate,                                                                     
	VT.Commission as CommissionCharged,                                                       
	VT.OtherBrokerFees as OtherBrokerFee,                                 
	T_ThirdPartyType.ThirdPartyTypeName,                                                                                                         
	0 as SecFee ,                                                                                                
	'Allocated' As TaxLotState,                               
	VT.FromDeleted,                                                                                                               
	VT.StampDuty As StampDuty,                                                                                           
	VT.TransactionLevy as TransactionLevy,                                                                        
	VT.ClearingFee as ClearingFee,                                                                                                                              
	VT.TaxOnCommissions as TaxOnCommissions,                                                            
	VT.MiscFees as MiscFees ,                                                                                                                          
	convert(varchar,VT.AUECLocalDate,101) as TradeDate,                                                   
	ISNULL(SM.Multiplier, 1) as AssetMultiplier,                      
	Case                       
	When VT.AssetID = 8                      
	then  Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * 0.01)) As Decimal(32,2)) As Varchar(500))                       
	Else                              
	Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * VT.SideMultiplier)) As Decimal(32,2)) As Varchar(500))           
	End As NetAmount,                                                                                                     
	VT.Level2ID,                                                                                           
	SM.ISINSymbol,                                                                                   
	SM.CUSIPSymbol,                                                                                                    
	SM.SEDOLSymbol,                                                                                                    
	SM.ReutersSymbol,                                                                                         
	SM.BloombergSymbol as BBCode,                                                                                                    
	SM.CompanyName,                  
	SM.UnderlyingSymbol,                                                                                        
	SM.LeadCurrency,                                                                             
	SM.VsCurrency,                                                                
	SM.OSISymbol as OSIOptionSymbol,                                                                
	SM.IDCOSymbol as IDCOOptionSymbol,                                                                    
	SM.OpraSymbol as OpraOptionSymbol,                           
	IsNull(VT.FXRate,0) As FXRate,                                                          
	VT.FXConversionMethodOperator,                                                        
	VT.ProcessDate,                                                      
	VT.OriginalPurchaseDate,                                                      
	IsNull(VT.AccruedInterest,0) As AccruedInterest,                                                      
	T_Country.CountryName as CountryName,                                                  
	IsNull(VT.FXRate_Taxlot,0) As FXRate_Taxlot,                                                
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
	then  0                               
	else  Closing.ClosedQty                              
	end As ClosedQty,                              
	ISNULL(Closing.OpenPrice, 0) as OpenPriceAgainstClosing,                              
	convert(varchar,#VTaxlot.AUECLocalDate, 101) as TradeDateAgainstClosing,                        
	#VTaxlot.LotID as LotIDAgainstClosing,                        
	VT.Level1AllocationID,                        
	VT.LotID,            
	CTPM.FundAccntNo as AccountNo                                                                                                         
  from  #VT VT 	                                                                                              
	Inner join T_CompanyThirdPartyMappingDetails as CTPM on  CTPM.InternalFundNameID_FK = VT.FundID                                                                                                                                                              
	Inner join T_FundType as FT on FT.FundTypeID = CTPM.FundTypeID_FK
	Inner Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol 
	INNER JOIN dbo.T_CompanyThirdParty on T_CompanyThirdParty.CompanyThirdPartyID=CTPM.CompanyThirdPartyID_FK                                                                                                                                                    
	INNER JOIN dbo.T_ThirdParty on T_ThirdParty.ThirdPartyId=T_CompanyThirdParty.ThirdPartyId AND  T_CompanyThirdParty.CompanyID=@companyID                                                                                                                       
	INNER JOIN dbo.T_ThirdPartyType on T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID    
	INNER JOIN T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                                                                  
	INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID                                     
	INNER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID                                    
	INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID                      
	INNER JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID 
	Left Outer Join PM_TaxLotClosing Closing On Closing.ClosingTaxLotID = VT.TaxLotID                              
	Left OUTER JOIN T_ClosingAlgos On T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo                              
	Left Outer Join #VTaxlot  On Closing.PositionalTaxLotID = #VTaxlot.TaxLotID                    
	left join T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID                    
	left join  T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                                                      
 Order by TaxlotId    
                           
Drop Table #VT, #VTaxlot