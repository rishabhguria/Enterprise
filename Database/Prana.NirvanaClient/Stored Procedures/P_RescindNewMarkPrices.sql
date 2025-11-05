
-----------------------------------------------------------------

--modified BY: omshiv
--Date:8/Aug
--Purpose: Rescind amended Mark Prices 

--Created BY: Bharat Raturi
--Date: 20-may-14

-----------------------------------------------------------------

CREATE procedure [dbo].[P_RescindNewMarkPrices]
(
@xmlMarkPrice nText
)
as
declare @handle int
exec sp_xml_preparedocument @handle output, @xmlMarkPrice

CREATE TABLE #TempMarkPrice                                                                               
(                                                                               
markPriceID int                    
)  
      
insert INTO #TempMarkPrice                                                                               
( markPriceID)

SELECT 
MarkPriceID
from openXML(@handle,'dsMarkPrice/dtMarkPrice',2)
with
(
MarkPriceID INT
)
update PM_DayMarkPrice
set 
IsApproved=1, AmendedMarkPrice =FinalMarkPrice
from #TempMarkPrice
where DayMarkPriceID=#TempMarkPrice.markPriceID
drop TABLE #TempMarkPrice
exec sp_xml_removedocument @handle
