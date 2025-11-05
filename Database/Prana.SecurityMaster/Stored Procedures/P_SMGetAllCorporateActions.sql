CREATE PROCEDURE [dbo].[P_SMGetAllCorporateActions]             
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
 select * from T_SMCorporateActions           
 where IsApplied = @isApplied and        
       (DateDiff(d,@fromDate,EffectiveDate) >= 0 and DateDiff(d,EffectiveDate,@toDate) >= 0)        
    order by UTCInsertiontime desc          
else          
 select * from T_SMCorporateActions           
 where IsApplied = @isApplied and CorporateActionType= @corporateActionType and        
    (DateDiff(d,@fromDate,EffectiveDate) >= 0  and DateDiff(d,EffectiveDate,@toDate) >= 0)         
    order by UTCInsertiontime desc          
End 

