
/*      
Author: Narendra Kumar Jangir 
Date: 2015-07-09 
Description: This is used to check that symbol and fund is closed or corporate action is applied in future date       
Input: It takes comma separated taxlotclosingid as input  
EXEC PMCheckTaxlotsClosedOrCAAppliedInFutureDate '7A82E735-1551-4148-ACC3-C6F9FF1EFCB8,'   
*/ 

CREATE PROC [dbo].[PMCheckTaxlotsClosedOrCAAppliedInFutureDate] 
(      
 @taxlotClosingIDs VARCHAR(max)         
 )      
AS    

--Declare @taxlotClosingIDs Varchar(Max)     
--Set @taxlotClosingIDs = '7A82E735-1551-4148-ACC3-C6F9FF1EFCB8,'    

Create Table #TaxlotClosingID                                                    
(                                                    
 taxlotClosingID Varchar(Max)             
)                                                    
Insert Into #TaxlotClosingID         
Select * From dbo.Split(@taxlotClosingIDs, ',')   

--select * from #TaxlotClosingID
  

SELECT 
PT.Symbol as Symbol
,PT.FundID as FundID
,CF.FundName FundName
,MIN(PT.AUECModifiedDate) As AUECModifiedDate
,MIN(PTC.TimeOfSaveUTC) as TimeOfSaveUTC
INTO #ClosedData
FROM #TaxlotClosingID Temp
INNER JOIN PM_Taxlotclosing PTC ON PTC.TaxlotClosingID = Temp.taxlotClosingID
INNER JOIN PM_Taxlots PT ON PTC.TaxlotClosingID = PT.TaxlotClosingID_fk
INNER JOIN T_CompanyFunds CF On CF.CompanyFundID = PT.FundID 
Group By PT.Symbol,PT.FundID,CF.FundName 

--SELECT * from #ClosedData

SELECT
PT.Symbol as Symbol
,PT.FundID as FundID
,temp.FundName FundName
,PT.AUECModifiedDate As AUECModifiedDate
,PTC.TaxlotClosingID
,PTC.ClosingMode
,PT.TaxlotID
INTO #ClosedDataInFutureDate
FROM
PM_Taxlots PT
INNER JOIN #ClosedData temp ON 
(PT.Symbol = temp.Symbol 
AND 
PT.FundID = temp.FundID 
AND
Datediff(d,PT.AuecModifiedDate,temp.AUECModifiedDate)<=0
AND PT.TaxlotClosingID_FK IS NOT NULL
)
INNER JOIN PM_TaxlotClosing PTC ON 
PTC.TaxlotClosingID = PT.TaxlotClosingID_FK
AND (
(Datediff(DD,PTC.TimeOfSaveUTC,temp.TimeOfSaveUTC)=0 AND Datediff(ms,PTC.TimeOfSaveUTC,temp.TimeOfSaveUTC)<=0 ) 
OR 
(Datediff(DD,PTC.TimeOfSaveUTC,temp.TimeOfSaveUTC)<0)
)

DELETE FROM #ClosedDataInFutureDate WHERE TaxlotClosingID IN (SELECT taxlotClosingID FROM #TaxlotClosingID)

SELECT
MAX(TaxlotID) AS TaxlotID 
,Symbol
,FundName
,MIN(AUECModifiedDate) as AUECModifiedDate
,ClosingMode
from #ClosedDataInFutureDate
Group By Symbol,FundID,FundName,ClosingMode 


--select  taxlotClosingID FROM #TaxlotClosingID

drop table #TaxlotClosingID,#ClosedData,#ClosedDataInFutureDate

--select * from pm_taxlotclosing


