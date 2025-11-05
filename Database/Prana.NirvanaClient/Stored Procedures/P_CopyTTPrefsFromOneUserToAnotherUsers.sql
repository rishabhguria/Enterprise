    
CREATE PROCEDURE [dbo].[P_CopyTTPrefsFromOneUserToAnotherUsers] (    
  @Users VARCHAR(MAX)    
 ,@AUIDs VARCHAR(MAX)    
 ,@CounterPartyIds VARCHAR(MAX)    
 ,@VenueIds Varchar(MAX)    
  ,@FromUser VARCHAR(MAX)    
 ,@FromAssetId VARCHAR(MAX)    
 ,@FromUnderlyingId VARCHAR(MAX)    
 ,@FromCounterPartyId VARCHAR(MAX)    
 ,@FromVenueId Varchar(MAX)    
     
 )    
AS      
      
 DECLARE @Local_Users VARCHAR(MAX)      
 DECLARE @Local_AUIDs VARCHAR(MAX)      
 DECLARE @Local_CounterPartyIds VARCHAR(MAX)      
 DECLARE @Local_VenueIds VARCHAR(MAX)      
    
 DECLARE @FromLocal_User VARCHAR(MAX)      
 DECLARE @FromLocal_AssetId VARCHAR(MAX)      
 DECLARE @FromLocal_UnderlyingId VARCHAR(MAX)      
 DECLARE @FromLocal_CounterPartyId VARCHAR(MAX)      
 DECLARE @FromLocal_VenueId VARCHAR(MAX)      
    
      
 SET @Local_Users = @Users     
 SET @Local_AUIDs = @AUIDs      
 SET @Local_CounterPartyIds = @CounterPartyIds      
 SET @Local_VenueIds = @VenueIds      
    
      
 SET @FromLocal_User = @FromUser     
 SET @FromLocal_AssetId = @FromAssetId      
 SET @FromLocal_UnderlyingId = @FromUnderlyingId      
 SET @FromLocal_CounterPartyId = @FromCounterPartyId      
 SET @FromLocal_VenueId = @FromVenueId      
    
    
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
  
   
SELECT  * into #T_TradingTicketSelectRecord from T_TradingTicketPrefrencesSettings T where    
T.AssetID = @FromLocal_AssetId and     
T.UnderlyingID = @FromLocal_UnderlyingId and    
T.CounterPartyID = @FromLocal_CounterPartyId and    
T.VenueID = @FromLocal_VenueId and    
T.CompanyUserID = @FromLocal_User    
    
    
IF NOT EXISTS (SELECT * FROM #T_TradingTicketSelectRecord)    
BEGIN    
  
  RETURN    
    
END    
delete   T_TradingTicketPrefrencesSettings  from #AllUsersIDs T  where    
 T_TradingTicketPrefrencesSettings.CompanyUserID = T.AllUserId    
    
Select AllAssetId,    
AllUnderlyingId,    
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
AllCounterPartyId,    
AllVenueId,    
OrderTypeID,    
ExecutionInstructionID,    
HandlingInstructionID,    
TIF,    
CVTradingAccountID,    
CVFundID,    
CVStrategyID,    
CVBrokerID,    
CVBorrowerFirmID,    
AllUserId,    
CMTAID,    
GiveUpID,    
OrderSideID,    
SettlCurrency,
IsQuantityDefaultValueChecked    
into #CopyUserPrefs  From #AssetsAndUnderlying,#AllCounterPartyIDs,#AllVenueIDs,#AllUsersIDs,#T_TradingTicketSelectRecord    
    
    
set IDENTITY_INSERT T_TradingTicketPrefrencesSettings off     
    
Insert into T_TradingTicketPrefrencesSettings      
SELECT     
AllAssetId,    
AllUnderlyingId,    
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
AllCounterPartyId,    
AllVenueId,    
OrderTypeID,    
ExecutionInstructionID,    
HandlingInstructionID,    
TIF,    
CVTradingAccountID,    
CVFundID,    
CVStrategyID,    
CVBrokerID,    
CVBorrowerFirmID,    
AllUserId,    
CMTAID,    
GiveUpID,    
OrderSideID,    
SettlCurrency,
IsQuantityDefaultValueChecked    
from #CopyUserPrefs    
    
    
    
drop TABLE #AllUsersIDs    
drop TABLE #AllCounterPartyIDs     
drop Table #AllVenueIDs    
drop TABLE #AssetsAndUnderlying  
drop TABLE #temp1   
return 0