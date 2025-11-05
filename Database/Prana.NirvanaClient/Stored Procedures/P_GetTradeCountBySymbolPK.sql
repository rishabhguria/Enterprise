/*
Author: Ankit Gupta
Date: 19 Oct, 2016
Description: https://jira.nirvanasolutions.com:8443/browse/PRANA-19274
Before modifying a symbol from symbol Lookup UI, we need to make sure that the ticker itself
and none of its options are are already traded in the system. 

EXEC [P_GetTradeCountBySymbolPK] 20161019102545807

*/

Create Procedure [dbo].[P_GetTradeCountBySymbolPK]    
(                              
 @Symbol_PK bigint                              
)                              
As    

Declare @TradesCount int  

Select @TradesCount = COUNT(*) FROM V_SecMasterData_WithUnderlying
WHERE Symbol_PK = @Symbol_PK 
  
Select @TradesCount