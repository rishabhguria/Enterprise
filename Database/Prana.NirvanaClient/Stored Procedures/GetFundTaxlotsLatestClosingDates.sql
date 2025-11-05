          
          
------------------------------------------------------------------------------------------------------------------------------------------------                        
--Author          : Abhishek Mehta                       
--Date            : 22-02-2008                        
--Description     : Returns the Initial closing only for open taxlots. This procedure will be used while doing the back date closing.          
--     Using this procedure we can display if any taxlot is closed in the future at a particular date.          
-- Usage    : PMGetFundTaxlotsInitialClosingDates          
        
-- Date : 20 May - Renamed from PMGetFundTaxlotsInitialClosingDates to GetFundTaxlotsLatestClosingDates.        
-- Modified by  : Abhishek Mehta        
                    
CREATE Procedure [dbo].[GetFundTaxlotsLatestClosingDates] As                            
-- Selecting the last close date for showing as the user needs to unwind that first.          
select positionalTaxlotId, Max(AuecLocalDate) as AUECLocalLastCloseDate from PM_TaxlotClosing where ClosingMode <>7   group by positionalTaxlotId        
Union       
select ClosingTaxlotId, Max(AuecLocalDate) as AUECLocalLastCloseDate from PM_TaxlotClosing    where ClosingMode <>7   group by ClosingTaxlotId      
  
---This can become a view containing the taxlotid, openqty and closedqty till aparticular date.          
--select VFA.AllocationId,VFA.AllocatedQty,IsNull(FundClose.TotalClosedQty,0) as TotalClosedQty from V_FundAllocation VFA Left Outer Join           
-- (select TaxlotId, IsNull(Sum(ClosedQty),0) as TotalClosedQty from V_FundPositionsClosing group by TaxlotId) as FundClose on VFA.AllocationId = FundClose.TaxlotId           
-- where (VFA.AllocatedQty - IsNull(FundClose.TotalClosedQty,0)) > 0   



  
  