        
                    
CREATE Procedure [dbo].[GetFundTaxlotsLatestExerciseDates] 
as
         
      
select ClosingTaxlotId, Max(AuecLocalDate) as AUECLocalLastCloseDate from PM_TaxlotClosing    where ClosingMode in (2,4)  group by ClosingTaxlotId      
  

  