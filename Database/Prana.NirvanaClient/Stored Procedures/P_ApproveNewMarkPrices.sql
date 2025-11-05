
-----------------------------------------------------------------

--modified BY: omshiv
--Date:8/Aug
--Purpose: Approve the mark prices in the DB and return approved data to update in cache

--Created BY: Bharat Raturi
--Date: 20-may-14
--Purpose: Approve the mark prices in the DB
-----------------------------------------------------------------

CREATE procedure [dbo].[P_ApproveNewMarkPrices]
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
IsApproved=1,
FinalMarkPrice = AmendedMarkPrice
from #TempMarkPrice
where DayMarkPriceID=#TempMarkPrice.markPriceID

select 
distinct FMP.Symbol,   
         Date,   
         FinalMarkPrice,   
         SM.AUECID,   
         A.ExchangeIdentifier as  AUECIdentifier,                    
         1 AS MarkPriceIndicator ,    
         ForwardPoints,  
         SM.AssetID,  
         SM.LeadCurrencyID,  
         SM.VsCurrencyID,
  FMP.fundID,  
 FMP.SourceID,
 SM.Symbol_PK,   
 FundName           
             
 From PM_DayMarkPrice FMP
 inner JOIN #TempMarkPrice TMP on TMP.markPriceID = FMP.DayMarkPriceID
 left JOIN T_CompanyFunds CF ON CF.CompanyFundID = FMP.FundID
 left JOIN V_SecMasterData SM on  FMP.Symbol = SM.TickerSymbol
 left JOIN T_AUEC A ON  SM.AUECID = A.AUECID  
 where DayMarkPriceID=TMP.markPriceID
 Order By Symbol, Date     



drop TABLE #TempMarkPrice
exec sp_xml_removedocument @handle

