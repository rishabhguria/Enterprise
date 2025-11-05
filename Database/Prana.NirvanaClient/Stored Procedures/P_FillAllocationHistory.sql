             
/**********************************************              
              
P_FillAllocationHistory              
Author:Praveen Bora  
Date: May 16,2014              
**********************************************/              
CREATE PROC P_FillAllocationHistory              
AS                                                                                                          
BEGIN              
                                  
Set NOCOUNT  on   
  
Delete T_AllocationHistory  
From T_AllocationHistory AlloHis  
Inner Join V_TaxLots VT On VT.Symbol = AlloHis.Symbol  
Where DateDiff(Day,AlloHis.Tradedate,VT.AUECLocalDate) = 0             
               
insert into  T_AllocationHistory               
Select                                                                  
 VTL.AUECLocalDate as Tradedate,              
 VTL.ProcessDate as ProcessDate,              
 VTL.SettlementDate,                  
 T_Side.Side,                                                                                                
 VTL.Symbol AS Symbol,                                                                                                         
  
 TaxLotQty,                                                                                                  
 VTL.AvgPrice,                                                                                        
 dbo.GetSideMultiplier(VTL.OrderSideTagValue) as sideMultiplier,                                                                                                                                                                                          
 isnull(VTL.Commission,0) as Commission,                                                                                                                                              
 isnull(VTL.OtherBrokerFees,0)+isnull(VTL.StampDuty,0) + isnull(VTL.TransactionLevy,0) + isnull(VTL.ClearingFee,0) + isnull(VTL.TaxOnCommissions,0) + isnull(VTL.MiscFees,0) as OtherFees                                      
 ,VTL.CounterPartyID               
 ,CP.ShortName AS 'Broker'                
 ,CU.shortName              
 ,VTL.description                   
 ,G.GroupID              
 ,VTL.TaxLotID              
 ,TOrder.ParentClOrderID              
 ,VTL.fundID             
 ,VTL.OrderSideTagValue          
 ,VTL.AUECLocalDate
 ,VTL.TradeAttribute1            
  
 from V_TaxLots VTL               
 join T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                                          
 INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID                   
 INNER JOIN T_CompanyUser CU ON VTL.UserID = CU.UserID               
 LEFT OUTER JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                                                                                   
 LEFT OUTER JOIN T_TradedOrders TOrder ON TOrder.GroupID = VTL.GroupID              
            
              
END 