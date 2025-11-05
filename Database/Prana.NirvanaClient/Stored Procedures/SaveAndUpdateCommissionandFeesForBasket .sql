
/*******************************************************  
Declare @ErrorMessage varchar(500)       
 Declare @ErrorNumber int       
exec [AL_SaveCommissionAndFees]'<?xml version="1.0"?>  
<ArrayOfAllocationGroup xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">  
  <AllocationGroup>  
    <Orders>  
      <AllocationOrder>  
        <OrderSideTagValue>1</OrderSideTagValue>  
        <OrderSide>Buy</OrderSide>  
        <OrderType>Market</OrderType>  
        <OrderTypeTagValue>1         </OrderTypeTagValue>  
        <Symbol>MSFT</Symbol>  
        <Venue>Auto</Venue>  
        <Quantity>100</Quantity>  
        <ClOrderID>2007102612043606</ClOrderID>  
        <AvgPrice>3.7</AvgPrice>  
        <AssetID>1</AssetID>  
        <AssetName>Equity</AssetName>  
        <UnderlyingID>1</UnderlyingID>  
        <UnderlyingName>US</UnderlyingName>  
        <ExchangeID>1</ExchangeID>  
        <ExchangeName>NASDAQ</ExchangeName>  
        <CurrencyID>-2147483648</CurrencyID>  
        <CurrencyName />  
        <AUECID>1</AUECID>  
        <TradingAccountID>9</TradingAccountID>  
        <TradingAccountName>SCR1</TradingAccountName>  
        <UserID>-2147483648</UserID>  
        <CounterPartyID>11</CounterPartyID>  
        <CounterPartyName>Source</CounterPartyName>  
        <VenueID>19</VenueID>  
        <CumQty>100</CumQty>  
        <AllocatedQty>0</AllocatedQty>  
        <Updated>false</Updated>  
        <NotAllExecuted>false</NotAllExecuted>  
        <ListID />  
        <GroupID>4176e821-deb8-466e-8d10-13d290b1371b</GroupID>  
        <FundID>-2147483648</FundID>  
        <StrategyID>-2147483648</StrategyID>  
        <AllocationTypeID>3</AllocationTypeID>  
        <OpenClose />  
        <OrigClOrderID />  
        <Commission>0</Commission>  
        <Fees>0</Fees>  
      </AllocationOrder>  
    </Orders>  
 </AllocationGroup>  
</ArrayOfAllocationGroup> ',@ErrorMessage,@ErrorNumber  
 select * from XMLITEM drop table xmlitem  
***********************************************************************************/  
  
-- =============================================    
-- Author:  Abhishek Mehta    
-- Create date: 25 oct 2007    
-- Description: save calculated commission    
-- =============================================    
CREATE PROCEDURE [dbo].[SaveAndUpdateCommissionandFeesForBasket ] (    
  @Xml nText                                                                                  
 ,@ErrorMessage varchar(500) output                                   
 ,@ErrorNumber int output      
)    
AS                   
                  
SET @ErrorNumber = 0                  
SET @ErrorMessage = 'Success'                  
                  
BEGIN TRAN TRAN1                
BEGIN TRY                  
                 
DECLARE @handle int                
                       
                                                             
exec sp_xml_preparedocument @handle OUTPUT,@Xml        
    
CREATE TABLE #XmlItem    
(    
 GroupID varchar(50)  
, Commission float    
, Fees float   
,FundID int   
,AllocatedQty float    
)    
INSERT INTO #XmlItem    
(    
 GroupID    
,Commission    
,Fees   
,FundID   
,AllocatedQty  
  
)    
Select    
GroupID    
,Commission    
,Fees   
,FundID  
,AllocatedQty  
    
FROM  OPENXML(@handle, '//anyType',2)                                                                                    
 WITH      
(    
 GroupID varchar(50)  
,Commission float     
,Fees float   
,FundID int  
,AllocatedQty float     
) 
Delete From   T_FundAllocationCommission where   AllocationId_Fk  in ( select AllocationId from T_FundAllocation Where Groupid in ( select distinct GroupId from #xmlitem))
Delete From   T_Groupcommission where   GroupID_fk  in ( select GroupID  from #XmlItem)
Delete From T_OrderCommission Where ParentClOrderID_FK in ( Select ClOrderID from T_grouporder where GroupId  in ( select distinct GroupId from #xmlitem))

INSERT INTO     
T_FundAllocationCommission     
(    
 AllocationId_fk    
,Commission    
,Fees    
)    
SELECT    
 T_FundAllocation.AllocationId    
,commission,Fees  from #XmlItem Join T_FundAllocation on   #XmlItem.groupid = T_FundAllocation.GroupId and #XmlItem.FundId = T_FundAllocation.FundId  
  
   
INSERT INTO    
T_Groupcommission                                   
(    
 GroupId_FK  
,Commission    
,Fees    
)    
SELECT      
GroupId    
,sum(Commission)  
,sum(Fees)    
FROM    
 #XmlItem Group by GroupId  
   
INSERT INTO T_OrderCommission  
(  
ParentClOrderID_FK  
,Commission  
,Fees  
)  
SELECT      
Distinct V_TradedOrders.CLOrderID    
    ,ISNULL(T_GroupCommission.Commission,0) * (V_TradedOrders.CumQty/T_Group.Quantity) As Commission    
    ,ISNULL(T_GroupCommission.Fees,0) * (V_TradedOrders.CumQty/T_Group.Quantity) As Fees    
FROM     
T_GroupOrder     
LEFT OUTER JOIN T_GroupCommission ON T_GroupOrder.GroupID = T_GroupCommission.GroupID_FK    
INNER JOIN V_TradedOrders On T_GroupOrder.ClOrderID = V_TradedOrders.ClOrderID    
INNER JOIN T_Group ON T_GroupOrder.GroupID = T_Group.GroupID Where T_GroupOrder.GroupId in ( select GroupId from #XmlItem)    
    
    
    
    
Drop Table #XmlItem     
   
EXEC sp_xml_removedocument @handle                    
                   
COMMIT TRANSACTION TRAN1                    
                    
                   
END TRY                    
BEGIN CATCH                     
 SET @ErrorMessage = ERROR_MESSAGE();                    
 SET @ErrorNumber = Error_number();                     
 ROLLBACK TRANSACTION TRAN1                       
END CATCH;
