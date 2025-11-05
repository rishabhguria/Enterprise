CREATE PROCEDURE P_GetYTDData_EOD_Batch_Stamos   
    @FromDate DATE,    
    @ToDate DATE,  
 @Accounts Varchar(1500)  
AS    
  
--Declare  @FromDate DATE, @ToDate DATE    
--Set @FromDate = '07-01-2025'  
--Set @ToDate = '07-01-2025'  
  
BEGIN    
    SET NOCOUNT ON;    
    
--Declare @Accounts Varchar(1500)  
  
--Set @Accounts = 'Alpha Omega JPM: 420-29971,Alpha Omega JPM EO: 420-29971,DVP: 420-64281,LRA: 420-64280,AO GS 420-40056,AO LB 420-40742'  
  
 DECLARE @thirdPartyID INT =91    
 --DECLARE @companyFundIDs VARCHAR(200) =N'1,12,34,35,49,50'     
 DECLARE @companyID INT =7    
 --DECLARE @auecIDs VARCHAR(max) =N'20,65,53,44,34,43,78,59,54,21,18,113,123,1,15,11,62,12,80,81,'    
 DECLARE @TypeID INT =1    
 DECLARE @dateType INT =1    
 DECLARE @fileFormatID INT =177    
 DECLARE @includeSent BIT =0    
      
Declare @CounterPartyId Int      
      
Set @CounterPartyId =       
(      
Select CounterPartyID From T_ThirdParty      
Where @TypeID <> 0 And ThirdPartyTypeID = 3 And ThirdPartyID = @thirdPartyID       
And CounterPartyID Is Not Null and CounterPartyID <> -2147483648      
)      
      
--Select @CounterPartyId      
      
Create Table #Temp_CounterPartyID      
(      
CounterPartyID INT      
)      
      
If (@CounterPartyId Is Null Or @CounterPartyId = '')      
Begin      
 Insert InTo #Temp_CounterPartyID      
 Select CounterPartyID From T_CounterParty      
End      
Else      
 Begin      
  Insert InTo #Temp_CounterPartyID      
  Select @CounterPartyId      
 End      
      
DECLARE @Fund TABLE (FundID INT)      
--DECLARE @AUECID TABLE (AUECID INT)      
DECLARE @IncludeExpiredSettledTransaction INT      
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT      
DECLARE @IncludeCATransaction INT      
      
SELECT @IncludeExpiredSettledTransaction = IncludeExercisedAssignedTransaction      
 ,@IncludeExpiredSettledUnderlyingTransaction = IncludeExercisedAssignedUnderlyingTransaction      
 ,@IncludeCATransaction = IncludeCATransaction      
FROM T_ThirdPartyFileFormat      
WHERE FileFormatId = @fileFormatID      
      
DECLARE @FXForwardAuecID INT      
      
SET @FXForwardAuecID = (      
  SELECT TOP 1 Auecid      
  FROM T_AUEC      
  WHERE assetid = 11      
  )      
   
Declare @Temp_Account Table  
(  
Account Varchar(100)  
)  
  
INSERT INTO @Temp_Account      
SELECT Cast(Items AS Varchar(100))      
FROM dbo.Split(@Accounts, ',')  
  
INSERT INTO @Fund   
Select CF.CompanyFundID  
From T_CompanyFunds CF  
Inner Join @Temp_Account T On T.Account = CF.FundName   
  
--SELECT Cast(Items AS INT)      
--FROM dbo.Split(@companyFundIDs, ',')      
      
--INSERT INTO @AUECID      
--SELECT Cast(Items AS INT)      
--FROM dbo.Split(@auecIDs, ',')      
      
SELECT      
T_Side.Side AS [Type],    
--'Deleted' As [Type],     
Case     
When VT.AssetID = 2    
Then SM.BloombergSymbol    
Else SM.SEDOLSymbol    
End As Security_Identifier,    
convert(varchar(10),VT.AUECLocalDate,101)  As Record_Date,    
convert(varchar(10),VT.SettlementDate,101) As Settle_Date,    
Sum(VT.TaxLotQty) As Quantity,    
((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + (VT.SideMultiplier * VT.TotalExpenses)) / VT.TaxLotQty / SM.Multiplier As Price,    
--VT.AvgPrice  As Price,    
Case     
When CF.FundName = 'Alpha Omega JPM EO: 420-29971' or CF.FundName = 'Alpha Omega JPM: 420-29971'    
Then CONCAT('AO',SM.UnderLyingSymbol, VT.Level1AllocationID)    
When CF.FundName = 'DVP: 420-64281'     
Then CONCAT('DVP',SM.UnderLyingSymbol, VT.Level1AllocationID)    
When CF.FundName = 'LRA: 420-64280'    
Then CONCAT('LRA',SM.UnderLyingSymbol, VT.Level1AllocationID)    
When CF.FundName = 'AO LB 420-40742'    
Then CONCAT('AOLB',SM.UnderLyingSymbol, VT.Level1AllocationID)    
When CF.FundName = 'AO GS 420-40056'    
Then CONCAT('AOGS',SM.UnderLyingSymbol, VT.Level1AllocationID)    
Else VT.Level1AllocationID    
End As External_Unique_ID,    
CF.FundName As Portpolio    
    
----------Dynamic UDA---------------      
FROM V_TaxLots VT      
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID      
--INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID      
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = VT.CounterPartyID      
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID    
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID      
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK      
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID      
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot      
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue      
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID      
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country      
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue      
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK      
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId      
 AND T_CompanyThirdParty.CompanyID = @companyID      
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID      
LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK      
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID      
 AND T_CounterPartyVenue.VenueID = VT.VenueID      
LEFT OUTER JOIN T_CompanyCounterPartyVenues ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID      
 AND T_CompanyCounterPartyVenues.CompanyID = @companyID      
LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK      
 AND T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID      
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol      
LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK      
LEFT OUTER JOIN PM_DailyVWAP AS DV ON VT.Symbol = DV.Symbol AND DATEDIFF(d,VT.AUECLocalDate,DV.Date) = 0 AND VWAP != 0      
WHERE     
Datediff(d,VT.ProcessDate, @FromDate) <= 0      
And Datediff(d,VT.ProcessDate, @ToDate) >= 0     
    
 AND (      
  (      
   VT.TransactionType IN (      
    'Buy'      
    ,'BuytoClose'      
    ,'BuytoOpen'      
    ,'Sell'      
    ,'Sellshort'      
    ,'SelltoClose'      
    ,'SelltoOpen'      
    ,'LongAddition'      
    ,'LongWithdrawal'      
    ,'ShortAddition'      
    ,'ShortWithdrawal'      
    ,''      
    )      
   AND (      
    VT.TransactionSource IN (      
     0      
     ,1      
     ,2      
     ,3      
     ,4      
     ,14      
     )      
    )      
   )      
  OR (      
   @IncludeExpiredSettledTransaction = 1      
   AND VT.TransactionType IN (      
    'Exercise'      
    ,'Expire'      
    ,'Assignment'      
    )      
   AND VT.AssetID IN (      
    2      
    ,4      
    )      
   )      
  OR (      
   @IncludeExpiredSettledTransaction = 1      
   AND VT.TransactionType IN (      
    'CSCost'      
    ,'CSZero'      
    ,'DLCost'      
    ,'CSClosingPx'      
    ,'Expire'      
    ,'DLCostAndPNL'      
    )      
   AND VT.AssetID IN (3)      
   )      
  OR (      
   @IncludeExpiredSettledUnderlyingTransaction = 1      
   AND VT.TransactionType IN (      
    'Exercise'      
    ,'Expire'      
    ,'Assignment'      
    )      
   AND TaxlotClosingID_FK IS NOT NULL      
   AND VT.AssetID IN (      
    1      
    ,3      
    )      
   )      
  OR (      
   @IncludeCATransaction = 1      
   AND (      
    VT.TransactionSource IN (      
     6      
     ,7      
     ,8      
     ,9      
     ,10      
     ,11      
     )      
    )      
   )      
  OR TransactionSource = 13      
  )      
GROUP BY    
T_Side.Side      
,VT.AvgPrice      
,VT.AssetID      
,VT.SettlementDate      
,VT.AUECLocalDate      
,SM.ISINSymbol      
,SM.SEDOLSymbol      
,SM.BloombergSymbol    
,CF.FundName    
,VT.GroupRefID    
,VT.Level1AllocationID    
--,VT.Symbol    
,SM.UnderLyingSymbol    
,SM.Multiplier    
,VT.TaxLotQty    
,VT.SideMultiplier    
,VT.TotalExpenses    
 Order By VT.AUECLocalDate , CF.FundName     
Drop Table #Temp_CounterPartyID    
END