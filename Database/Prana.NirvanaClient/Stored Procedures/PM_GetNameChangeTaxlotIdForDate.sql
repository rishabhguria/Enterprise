
/*  
-- Author : Rajat          
-- Description : This SP is used to fetch the positional taxlotid for which the name change has been applied on the given date       
EXEC PM_GetNameChangeTaxlotIdForDate '10-16-2014','e888c0bb-4833-4e83-8791-beb9c3338753'  
*/  
   
CREATE Procedure PM_GetNameChangeTaxlotIdForDate          
(          
 @Date Datetime,  
 @caID Varchar(500)  
)          
As          
          
Select Distinct   
Corp.TaxlotId         
From PM_Taxlots PT   
Inner Join PM_CorpActionTaxlots Corp On PT.TaxLotID = Corp.TaxlotId          
 where DateDiff(d,PT.AUECModifiedDate,@Date) = 0 and CorpActionId = @caID ----and Corp.ClosingId is not null       
      
   
----Select distinct PT.TaxlotId from PM_taxlots PT where DateDiff(d,PT.AUECModifiedDate,@date) = 0 and PT.PositionTag in (3,5,4,6,7,8,9,10) 

