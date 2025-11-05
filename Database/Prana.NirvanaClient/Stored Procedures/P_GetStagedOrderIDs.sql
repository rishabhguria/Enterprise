Create PROCEDURE P_GetStagedOrderIDs
(                              
@lowerdate datetime,                
@upperdate datetime 
)
AS
BEGIN
    SELECT ClOrderID,TimeInForce FROM T_Sub where NirvanaMsgType=3              
and datediff(d,AuecLocalDate,@lowerdate) <= 0                
and datediff(d,AuecLocalDate,@upperdate) >= 0
order by ParentClOrderID 
END;
