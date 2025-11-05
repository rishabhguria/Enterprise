
-- Author : Rajat    
-- Description : This SP is used to fetch the positional taxlotid for which the name change has been applied on the given date    
CREATE Procedure PM_GetFutureNameChangeTaxlotIdForDate    
(    
 @date datetime    
)    
As    
    
    
Select distinct Corp.TaxlotId     
from PM_Taxlots PT inner join PM_CorpActionTaxlots Corp on PT.TaxLotClosingId_Fk = Corp.ClosingId    
where DateDiff(d,@date,PT.AUECModifiedDate) >= 0 and Corp.ClosingId is not null
