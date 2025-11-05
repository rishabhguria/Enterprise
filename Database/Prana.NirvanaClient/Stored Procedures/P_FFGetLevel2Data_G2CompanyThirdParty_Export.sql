          
            
Create PROCEDURE [dbo].[P_FFGetLevel2Data_G2CompanyThirdParty_Export]                                                                                                                                                         
            
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
            
  Level2Percentage float,--Percentage,                                                                                                   
            
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
            
  PBID int,                                             
            
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
            
LotID varchar(50),        
        
SettlementCurrency Int  ,  
IsSwapped bit                       
            
)                                      
            
                                      
            
insert into #VT                                      
            
select                                                                 
            
  VT.TaxLotID as TaxLotID,                                                                              
            
  ISNULL(VT.FundID,0) as AccountID,                                                                                                           
            
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
            
  VT.Level2Percentage,--Percentage,                                                                                                   
            
  VT.TaxLotQty,                                                                                        
            
  VT.SettlementDate,                                                                                                        
            
  ISNULL(VT.Commission,0) As Commission,                                                                                                                                                                        
            
  IsNull(VT.OtherBrokerFees,0) As OtherBrokerFees ,                                                          
            
  T_PBWiseTaxlotState.TaxLotState as TaxlotState,                                                             
            
  ISNULL(VT.StampDuty,0) as StampDuty,                                                                                                                                
            
  ISNULL(VT.TransactionLevy,0) as TransactionLevy,                               
            
  ISNULL(ClearingFee,0) as ClearingFee,                                                                                     
            
  ISNULL(TaxOnCommissions,0) as TaxOnCommissions,                   
            
  ISNULL(MiscFees,0) as MiscFees ,                                                                                                                                  
            
  VT.AUECLocalDate,                                           
            
   VT.Level2ID as Level2ID,                                                    
            
  T_PBWiseTaxlotState.PBID,                                                                  
            
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
            
 VT.LotID,        
        
VT.SettlCurrency_Group As SettlementCurrency    ,  
VT.IsSwapped as IsSwapped                                  
            
                                                                      
            
  from                                                                   
            
    V_TaxLots VT                                                                                             
            
    Left Outer Join T_PBWiseTaxlotState on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID              
                              
Where T_PBWiseTaxlotState.fileFormatID = 0 or @fileFormatID = FileFormatID             
                                                                        
            
  Union All                                                                                                        
            
                                                                                                  
            
  select                                                                                                                                                       
            
   TDT.TaxLotID as TaxLotID,                                                                                                                                  
            
  ISNULL(TDT.FundID,0) as AccountID,                                                                     
            
  TDT.OrderTypeTagValue,                                       
            
  TDT.OrderSideTagValue as SideID,                                                                                                                                                                         
            
  TDT.Symbol,                                      
            
  IsNull(TDT.CounterPartyID,0) As CounterPartyID,                                                                                                                                                                 
            
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
            
  IsNull(TDT.Commission,0) As Commission,                                                                             
            
  IsNull(TDT.OtherBrokerFees, 0) As OtherBrokerFees,                              
            
  TDT.TaxLotState ,                                                                                       
            
  ISNULL(TDT.StampDuty,0) as StampDuty,                                               
            
  ISNULL(TDT.TransactionLevy,0) as TransactionLevy,                                                                                                                                      
            
  ISNULL(TDT.ClearingFee,0) as ClearingFee,                                                                    
     
  ISNULL(TDT.TaxOnCommissions,0) as TaxOnCommissions,                                
            
  ISNULL(TDT.MiscFees,0) as MiscFees,                                                                                                                                            
            
  TDT.AUECLocalDate,                                                                      
            
  0 as Level2ID,                                                                      
            
  TDT.PBID,                                                                  
            
  IsNull(TDT.FXRate,0) as FXRate,                                                     
            
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
            
 TDT.LotID ,        
        
TDT.SettlCurrency As SettlementCurrency   ,  
TDT.IsSwapped as IsSwapped                                      
            
  from                                                                                                                                                                         
            
  T_DeletedTaxLots TDT     where (FileFormatID = @fileFormatID  or FileFormatID = 0)                                     
            
                    
            
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
            
  VT.Level2Percentage as Level2Percentage,--Percentage,                                                                                                                                        
            
  VT.TaxLotQty as AllocatedQty,                                                                                                                                         
            
  SM.PutOrCall,                                                                                   
            
  IsNull(SM.StrikePrice,0) as StrikePrice,               
            
  convert(varchar,SM.ExpirationDate,101) as ExpirationDate,                                                                                                         
            
  convert(varchar,VT.SettlementDate,101) as SettlementDate,                                                                             
            
  VT.Commission as CommissionCharged,                                                               
            
  VT.OtherBrokerFees as OtherBrokerFee,                                         
            
  T_ThirdPartyType.ThirdPartyTypeName,                                                                                                                 
            
  0 as SecFee ,                                                                                                        
            
  ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName,                                    
            
  case                                   
            
  when VT.TaxLotState = '0'                                            
            
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
            
  VT.FromDeleted,                                                                                           
            
  ISNULL(VT.StampDuty,0) as StampDuty,                                                                                                   
            
  ISNULL(VT.TransactionLevy,0) as TransactionLevy,                                                                                
            
  ISNULL(VT.ClearingFee,0) as ClearingFee,                                                                                                                                      
            
  ISNULL(VT.TaxOnCommissions,0) as TaxOnCommissions,                   
            
  ISNULL(VT.MiscFees,0) as MiscFees ,                                                                                                                                  
            
  convert(varchar,VT.AUECLocalDate,101) as TradeDate,                                                           
            
  ISNULL(SM.Multiplier, 1) as AssetMultiplier,                              
            
 Case                               
            
  When VT.AssetID = 8                              
            
  then  Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * 0.01)) As Decimal(32,2)) As Varchar(500))                           
  
    
            
  Else                                      
            
  Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * VT.SideMultiplier)) As Decimal(32,2)) As Varchar(500))            
  
   
      
        
          
            
            
              
            
                
            
                 
            
  End                              
            
  As NetAmount,                                                                                                             
            
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
            
CTPM.FundAccntNo as AccountNo,        
        
TC.CurrencySymbol as SettlementCurrency ,  
VT.IsSwapped as IsSwapped                                                                                                                
            
                                                                                                              
            
  from                                       
            
  #VT VT                                                                                                       
            
 inner join T_CompanyThirdPartyMappingDetails as CTPM on  CTPM.InternalFundNameID_FK = VT.FundID                                                                                                                                                               
  
    
     
        
         
            
 inner join T_FundType as FT on FT.FundTypeID = CTPM.FundTypeID_FK                                           
            
Left Outer Join PM_TaxLotClosing Closing On Closing.ClosingTaxLotID = VT.TaxLotID                                      
            
Left OUTER JOIN T_ClosingAlgos On T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo                                      
            
Left Outer Join #VTaxlot  On Closing.PositionalTaxLotID = #VTaxlot.TaxLotID                                      
            
 left join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID        
          
 LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlementCurrency                                                                        
            
 left join  T_Side ON dbo.T_Side.SideTagValue = VT.SideID                                             
            
 left join T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID                                            
            
 left join T_Asset ON T_Asset.AssetID = VT.AssetID                                            
            
 left join T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID                            
            
           
            
 left join  T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID                                                               
            
 left join  T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                                                              
            
                                  
            
 left JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue                                                                    
            
 INNER JOIN dbo.T_CompanyThirdParty on T_CompanyThirdParty.CompanyThirdPartyID=CTPM.CompanyThirdPartyID_FK                                                                                                                                                     
           
            
 INNER JOIN dbo.T_ThirdParty on T_ThirdParty.ThirdPartyId=T_CompanyThirdParty.ThirdPartyId AND  T_CompanyThirdParty.CompanyID=@companyID                                                                                                                       
  
           
 INNER JOIN dbo.T_ThirdPartyType on T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID                                                                            
            
 LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK                                                                                            
            
 LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID And T_CounterPartyVenue.VenueID=VT.VenueID                                                                                                                            
  
                                 
            
  LEFT OUTER JOIN T_CompanyCounterPartyVenues                                                                                                 
            
  ON T_CompanyCounterPartyVenues.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID                                                                 
            
  And T_CompanyCounterPartyVenues.CompanyID=@companyID                                                                                                     
            
  LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier                                                              
            
  ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK                                                                           
            
  And T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID                                                                                                  
            
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
            
    ),@inputdate) = 0                                 
            
  And CTPM.InternalFundNameID_FK in (select FundID from @Fund)                                                                                                                                      
            
  And VT.AUECID in (select AUECID from @AUECID)                                                                               
            
                          
            
 order by TaxlotId                                      
            
                                      
            
drop table #VT, #VTaxlot 