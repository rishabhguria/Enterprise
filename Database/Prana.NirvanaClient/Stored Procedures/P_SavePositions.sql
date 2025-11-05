CREATE Procedure [dbo].[P_SavePositions]                                                                            
As                                                                                                                                   
Begin

delete from PM_SnapShot

DECLARE @SpecifiedDate datetime;
set @SpecifiedDate=(Select SpecifiedDate From T_PositionDate);

insert into PM_SnapShot(TaxLotID, Symbol, TaxLotOpenQty, AvgPrice, TimeOfSaveUTC, GroupID, AUECModifiedDate, FundID, Level2ID, OpenTotalCommissionandFees,                                                     
ClosedTotalCommissionandFees,PositionTag,OrderSideTagValue,ParentRow_Pk,AccruedInterest)  
(Select TaxLotID, Symbol, TaxLotOpenQty, AvgPrice, TimeOfSaveUTC, GroupID, AUECModifiedDate, FundID, Level2ID, OpenTotalCommissionandFees,ClosedTotalCommissionandFees,PositionTag,OrderSideTagValue,ParentRow_Pk ,AccruedInterest
from PM_Taxlots Where                                                 
 taxlot_PK in 
(Select max(taxlot_PK) from PM_Taxlots                                                                                 
  Inner join  T_Group G on G.GroupID=PM_Taxlots.GroupID                                                    
  inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                              
   where Datediff(d,PM_Taxlots.AUECModifiedDate,@SpecifiedDate) >= 0       
  group by taxlotid)and TaxLotOpenQty<>0) 
end


