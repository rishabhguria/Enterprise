-- Description : This sp directly returns the table rather than returning in form of xml and transforming on ui.  
-- It's the modified form of P_SMGetAllCorporateActions which is used from CA UI (Get ca for date range)  
  
CREATE PROCEDURE [dbo].[P_GetCADetailsForDateRange]                 
(              
 @corporateActionType int,              
 @fromDate datetime,              
 @toDate datetime,              
 @isApplied bit              
)     
  
As                 
                  
Begin              
-- 0 Represents corporateActionType.all so no check is required              
if (@corporateActionType = 0)              
 select * from v_corpactiondata               
 where IsApplied = @isApplied and            
       (DateDiff(d,@fromDate,EffectiveDate) >= 0 and DateDiff(d,EffectiveDate,@toDate) >= 0)            
    order by EffectiveDate desc              
else              
 select * from v_corpactiondata               
 where IsApplied = @isApplied and CorpActionTypeID= @corporateActionType and            
    (DateDiff(d,@fromDate,EffectiveDate) >= 0  and DateDiff(d,EffectiveDate,@toDate) >= 0)             
    order by EffectiveDate desc              
End 