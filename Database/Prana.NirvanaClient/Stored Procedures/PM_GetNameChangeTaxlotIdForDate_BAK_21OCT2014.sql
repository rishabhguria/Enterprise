-- Author : Rajat        
-- Description : This SP is used to fetch the positional taxlotid for which the name change has been applied on the given date        
CREATE Procedure PM_GetNameChangeTaxlotIdForDate_BAK_21OCT2014        
(        
 @date datetime        
)        
As        
        
        
-- Select distinct Corp.TaxlotId         
-- from PM_Taxlots PT inner join PM_CorpActionTaxlots Corp on PT.TaxLotClosingId_Fk = Corp.ClosingId        
-- where DateDiff(d,PT.AUECModifiedDate,@date) = 0 and Corp.ClosingId is not null    
    
--Select distinct PT.TaxlotId from PM_taxlots PT where DateDiff(d,PT.AUECModifiedDate,@date) = 0 and PT.PositionTag in (3,5)    
--    0 - Long  
--    1 - Short  
--    2 - None   
--        3 - LongAddition,  
--        4 - LongWithdrawal,  
--        5 - ShortAddition,  
--        6 - ShortWithdrawal,  
--        //LongNotionalChange,  
--        7 - LongCostAdj,  
--        //ShortNotionalChange,  
--        8 - ShortCostAdj,  
--        9 - LongWithdrawalCashInLieu,  
--        10 - ShortWithdrawalCashInLieu  
Select distinct PT.TaxlotId from PM_taxlots PT where DateDiff(d,PT.AUECModifiedDate,@date) = 0 and PT.PositionTag in (3,5,4,6,7,8,9,10)   