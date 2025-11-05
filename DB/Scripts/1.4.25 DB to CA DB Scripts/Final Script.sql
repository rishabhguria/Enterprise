DELETE from PM_Taxlots

Insert into PM_Taxlots      
(TaxLotID, Symbol, TaxLotOpenQty, AvgPrice, TimeOfSaveUTC, GroupID, AUECModifiedDate, FundID, Level2ID, OpenTotalCommissionandFees,       
ClosedTotalCommissionandFees,PositionTag,OrderSideTagValue)      
select  TaxLotID, Symbol, TaxLotQty, AvgPrice, CreationDate, GroupID, AUECLocalDate, FundID, Level2ID,      
  (Commission + OtherBrokerFees + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees) as OpenTotalCommissionandFees, 0 as ClosedTotalCommissionandFees,     
Case OrderSideTagValue    
	WHEN '1' THEN 0    
	WHEN 'A' THEN 0    
	WHEN 'B' THEN 0    
ELSE 1    
END ,    
OrderSideTagValue      
from V_Taxlots