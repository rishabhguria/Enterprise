          
/****************************           
          
P_fillBlotterHistory          
Author: Praveen Bora        
Date: May 16,2014        
P_FillBlotterHistory '2014/11/25'        
          
***************************/          
    
CREATE proc P_FillBlotterHistory           
(        
 @Date DateTime        
)        
as           
begin          
 set NOCOUNT  on          
          
 delete FROM T_BlotterHistory           
 where DateDiff(Day,TradeDate,@Date) = 0           
         
    
Select           
   T_Fills.InsertionTime as 'TradeDate'          
  ,T_Side.Side          
  ,T_Order.Symbol as 'Symbol'          
  ,T_Fills.LastShares 'LastShares'          
  ,T_Fills.LastPx 'Price'          
  ,T_CounterParty.ShortName AS 'Broker'          
  ,T_CompanyUser.ShortName  AS 'users'          
  ,T_Sub.Text as 'Descriptions'          
  ,dbo.GetSideMultiplier(T_Order.OrderSidetagValue) as SideMultiplier          
  ,T_Order.ParentCLOrderID as ParentCLOrderID           
  ,T_Sub.CLOrderID as CLOrderID           
  ,T_Fills.Quantity as Quantity        
  ,T_Fills.CumQty as CumQty          
  ,T_Fills.AveragePrice as AveragePrice           
  ,T_Order.OrderSidetagValue as OrderSidetagValue     
  ,T_Fills.Fills_PK as Fills_PK    
  ,T_Fills.OrderStatus as OrderStatus     
  ,cast(1 as bit) as IsSummary         
 Into #TempBlotterData          
 From T_Order           
 Inner Join T_Sub On T_Sub.ParentCLOrderID = T_Order.ParentCLOrderID          
 Inner Join T_Fills On T_Sub.CLOrderID = T_Fills.CLOrderID          
 Inner JOIN T_Side on T_Order.OrderSidetagValue= T_Side.SideTagValue          
 INNER JOIN T_CounterParty on T_CounterParty.CounterPartyID = T_Sub.CounterPartyID          
 INNER JOIN T_CompanyUser on T_CompanyUser.UserID = T_Sub.UserID          
Where DateDiff(Day,T_Fills.InsertionTime,@Date) = 0      
    
    
    
insert into T_BlotterHistory    
(    
   TradeDate          
  ,Side          
  ,Symbol          
  ,LastShares          
  ,LastPx         
  ,Broker          
  ,users          
  ,Description          
  ,SideMultiplier          
  ,ParentCLOrderID           
  ,CLOrderID           
  ,Quantity        
  ,CumQty          
  ,AveragePrice           
  ,OrderSidetagValue     
  ,Fills_PK    
  ,IsSummary     
)    
select     
  TradeDate          
  ,Side          
  ,Symbol          
  ,LastShares          
  ,Price         
  ,Broker          
  ,users          
  ,Descriptions          
  ,SideMultiplier          
  ,ParentCLOrderID           
  ,CLOrderID           
  ,Quantity        
  ,CumQty          
  ,AveragePrice           
  ,OrderSidetagValue     
  ,Fills_PK    
  ,IsSummary     
 from #TempBlotterData    
    
delete from #TempBlotterData    
where OrderStatus <> '0'    
    
select     
 T_Sub.ParentCLOrderID,    
 T_Sub.CLOrderId,    
 T_Fills.Fills_PK,    
 T_Fills.OrderStatus    
into #ToBeDeletedFills    
from #TempBlotterData    
inner join T_Sub on (#TempBlotterData.CLOrderId = T_Sub.CLOrderId and  T_Sub.ParentCLOrderID = #TempBlotterData.ParentCLOrderID )     
inner join T_Fills on  (T_Sub.CLOrderId = T_Fills.CLOrderId and T_Fills.Fills_PK < #TempBlotterData.Fills_PK)    
     
    
update T_BlotterHistory    
set IsSummary = 0    
from     
T_BlotterHistory    
inner join #ToBeDeletedFills as Del on T_BlotterHistory.ParentCLOrderId = Del.ParentCLOrderId and T_BlotterHistory.CLOrderId = Del.CLOrderId AND Del.Fills_PK = T_BlotterHistory.Fills_PK    
    
drop table #TempBlotterData,#ToBeDeletedFills    
    
    
End          
          
          
          
          
          
          
          
          
          