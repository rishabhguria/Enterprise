
Create PROCEDURE [dbo].[P_SaveTTPrefsForMultipleUsers] (    
  @Users VARCHAR(MAX)    
 ,@AUIDs VARCHAR(MAX)    
 ,@CounterPartyIds VARCHAR(MAX)    
 ,@VenueIds Varchar(MAX)    
 ,@fundID VARCHAR(MAX)    
 ,@tradingAccountID VARCHAR(MAX)    
 ,@strategyID VARCHAR(MAX)          
 ,@quantity VARCHAR(MAX)      
 ,@displayQuantity VARCHAR(MAX)      
 ,@quantityIncrement VARCHAR(MAX)      
 ,@priceLimitIncrement VARCHAR(MAX)      
 ,@stopPriceIncrement VARCHAR(MAX)      
 ,@pegOffset VARCHAR(MAX)      
 ,@discrOffset VARCHAR(MAX)      
 ,@orderTypeID VARCHAR(MAX)      
 ,@executionInstructionID VARCHAR(MAX)      
 ,@handlingInstructionID VARCHAR(MAX)      
 ,@tIF VARCHAR(MAX)           
 ,@OrderSideID VARCHAR(MAX)      
 ,@SettlCurrency INT
 ,@IsQuantityDefaultValueChecked VARCHAR(6)      
 )    
AS      
      
 DECLARE @Local_Users VARCHAR(MAX)      
 DECLARE @Local_AUIDs VARCHAR(MAX)      
 DECLARE @Local_CounterPartyIds VARCHAR(MAX)      
 DECLARE @Local_VenueIds VARCHAR(MAX)      
    
      
 SET @Local_Users = @Users     
 SET @Local_AUIDs = @AUIDs     
 SET @Local_CounterPartyIds = @CounterPartyIds      
 SET @Local_VenueIds = @VenueIds      
    
    
    
Create Table #AllUsersIDs(AllUserId varchar(10))    
Insert INTO #AllUsersIDs    
SELECT Items As AllUserId    
from dbo.Split(@Local_Users,',')    
    
Create Table #AllCounterPartyIDs(AllCounterPartyId varchar(10))    
Insert INTO #AllCounterPartyIDs    
SELECT Items As AllCounterPartyId    
from dbo.Split(@Local_CounterPartyIds,',')    
    
Create Table #AllVenueIDs(AllVenueId varchar(10))    
Insert INTO #AllVenueIDs    
SELECT Items As AllVenueId    
from dbo.Split(@Local_VenueIds,',')    
    
    
SELECT * INTO #temp1 FROM dbo.Split(@Local_AUIDs,',')    
    
select     
SUBSTRING(items,0,CHARINDEX('/',items)) as AllAssetId    
,SUBSTRING(items,CHARINDEX('/',items)+1,LEN(items)-CHARINDEX('/',items) ) as AllUnderlyingId into #AssetsAndUnderlying  from #temp1     
    
    
SELECT TOP 0 *    
INTO #T_TradingTicketPrefrencesSettingsTemp    
FROM T_TradingTicketPrefrencesSettings    
    
    
INSERT INTO #T_TradingTicketPrefrencesSettingsTemp    
SELECT #AssetsAndUnderlying.AllAssetId,    
#AssetsAndUnderlying.AllUnderlyingId,    
@tradingAccountID,    
@fundID,    
@strategyID,    
'',      
'false',  
@quantity,    
@displayQuantity,    
@quantityIncrement,    
@priceLimitIncrement,    
@stopPriceIncrement,    
@pegOffset,    
@discrOffset,    
#AllCounterPartyIDS.AllCounterPartyId,    
#AllVenueIDs.AllVenueId,    
@orderTypeID,    
@executionInstructionID,    
@handlingInstructionID,    
@tIF,    
@tradingAccountID,      
@fundID,      
@strategyID,    
null,    
'',    
#AllUsersIDs.AllUserId,    
'-2147483648',    
'-2147483648',    
@OrderSideID,    
@SettlCurrency,
@IsQuantityDefaultValueChecked
From #AssetsAndUnderlying,#AllCounterPartyIDs,#AllVenueIDs,#AllUsersIDs    
     
    
delete #T_TradingTicketPrefrencesSettingsTemp FROM T_TradingTicketPrefrencesSettings T inner JOIN     
    
#T_TradingTicketPrefrencesSettingsTemp USID ON T.CompanyUserID=USID.CompanyUserID     
and     
T.AssetID=USID.AssetID      
and    
 T.UnderlyingID = USID.UnderlyingID     
and     
 T.CounterPartyID = USID.CounterPartyID     
and    
 T.VenueID = USID.VenueID    
      
    
set IDENTITY_INSERT T_TradingTicketPrefrencesSettings off     
    
Insert into T_TradingTicketPrefrencesSettings  SELECT     
AssetID,    
UnderlyingID,    
TradingAccountID,    
FundID,    
StrategyID,    
BrokerID,    
IsDefaultCV,    
Quantity,    
DisplayQuantity,    
QuantityIncrement,    
PriceLimitIncrement,    
StopPriceIncrement,    
PegOffset,    
DiscrOffset,    
CounterPartyID,    
VenueID,    
OrderTypeID,    
ExecutionInstructionID,    
HandlingInstructionID,    
TIF,    
CVTradingAccountID,    
CVFundID,    
CVStrategyID,    
CVBrokerID,    
CVBorrowerFirmID,    
CompanyUserID,    
CMTAID,    
GiveUpID,    
OrderSideID,    
SettlCurrency,
IsQuantityDefaultValueChecked    
 from #T_TradingTicketPrefrencesSettingsTemp    
    
    
IF @tradingAccountID >0    
begin    
 UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.TradingAccountID = @tradingAccountID , T_TradingTicketPrefrencesSettings.CVTradingAccountID = @tradingAccountID   
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
end    
    
IF @fundID >0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.FundID = @fundID , T_TradingTicketPrefrencesSettings.CVFundID = @fundID          
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
IF @strategyID >=0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.StrategyID = @strategyID , T_TradingTicketPrefrencesSettings.CVStrategyID = @strategyID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
IF cast(@quantity as float) >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.Quantity = @quantity     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF cast(@displayQuantity as float)  >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.DisplayQuantity = @displayQuantity     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF cast(@quantityIncrement as float)  >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.QuantityIncrement = @quantityIncrement     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
    
 IF cast(@priceLimitIncrement as float)  >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.PriceLimitIncrement = @priceLimitIncrement     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF cast(@stopPriceIncrement as float)  >=0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.StopPriceIncrement = @stopPriceIncrement     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF cast(@pegOffset as float)  >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.PegOffset = @pegOffset     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF cast(@discrOffset as float)   >0.0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.DiscrOffset = @discrOffset     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
 IF ( isnumeric(@orderTypeID) > 0 ) 
begin
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.OrderTypeID = @orderTypeID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
 end
else
begin
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.OrderTypeID = @orderTypeID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
 end

  
IF ( isnumeric(@executionInstructionID) >= 0 )   
begin  
 UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.ExecutionInstructionID = @executionInstructionID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
end  
  
else  
begin  
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.ExecutionInstructionID = @executionInstructionID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
end  
   
   
 IF  @handlingInstructionID > 0  
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.HandlingInstructionID = @handlingInstructionID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
    
IF @tIF >= 0  
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.TIF = @tIF     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
  
    
  IF ( isnumeric(@OrderSideID) > 0 )
begin    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.OrderSideID = @OrderSideID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
end
else
  begin    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.OrderSideID = @OrderSideID     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
end
 IF @SettlCurrency  >0    
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.SettlCurrency = @SettlCurrency     
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId    
   
UPDATE T_TradingTicketPrefrencesSettings SET T_TradingTicketPrefrencesSettings.IsQuantityDefaultValueChecked = @IsQuantityDefaultValueChecked    
 FROM T_TradingTicketPrefrencesSettings T inner JOIN     
 #AllUsersIDs USID ON T.CompanyUserID=USID.AllUserId  inner JOIN     
 #AssetsAndUnderlying ASID  ON T.AssetID=ASID.AllAssetId  inner JOIN     
 #AssetsAndUnderlying AUID on T.UnderlyingID = AUID.AllUnderlyingId inner JOIN     
 #AllCounterPartyIDs ACID ON T.CounterPartyID = ACID.AllCounterPartyId inner JOIN     
 #AllVenueIDs AVID ON  T.VenueID = AVID.AllVenueId     
    
    
drop TABLE #AllUsersIDs    
drop TABLE #AssetsAndUnderlying    
drop TABLE #AllCounterPartyIDs     
drop Table #AllVenueIDs    
drop TABLE #temp1    
drop TABLE #T_TradingTicketPrefrencesSettingsTemp     
return 0

