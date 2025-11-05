      CREATE      PROCEDURE dbo.P_SaveManualFills  
(  
  @clOrderID varchar(50),  
   
  @lastprice float,  
  @lastShares float,  
  @execID varchar(50),  
  @avgPrice float,  
  @cumQty float,  
  @count int,  
  @orderStatus char,  
  @price float,  
    
  @quantity float,   
  @leavesQty float,  
  @Symbol varchar(50),  
  @MsgType char,  
  @Side char,  
  @OrderType char  
  
  
)  
AS  
Declare @result int  
Declare @total int   
Set @total = 0  
  
--if (@count = 0)  
--begin  
--Delete  from T_Fills  
--where ClorderID = @clOrderID  
--end  
  
  
Select @total = Count(*)  
From T_Fills  
Where ExecutionID = @execID  
   
 if(@total > 0)  
 begin   
  Update T_Fills   
   Set   
   --Quantity = @quantity,   
   LastPx = @lastprice,  
   LastShares = @lastShares,  
     
   AveragePrice = @avgPrice,  
   CumQty = @cumQty,  
   OrderStatus = @orderStatus,  
   Price = @price  
     
  Where ExecutionID = @execID  
    
  Set @result = 1  
 end  
 else  
 begin  
  INSERT INTO T_Fills(  
    
  ClOrderID,   
  ExecutionID,   
  --Quantity,   
  LastPx,   
  LastShares,  
    
  AveragePrice,  
  CumQty,  
  OrderStatus ,  
  Price ,  
  Quantity ,  
  LeavesQty ,  
  Symbol ,  
  MsgType ,  
SideID,  
OrderTypeID  
  )  
  VALUES(@clOrderID,   
  @execID,   
  --@quantity,  
   @lastprice,   
   @lastShares,  
  @avgPrice,  
  @cumQty,  
  @orderStatus,  
   @price,  
  @quantity ,   
  @leavesQty ,  
  @Symbol ,  
  @MsgType ,  
@Side,  
@OrderType  
   )  
    
  SET @result = scope_identity()  
 End  
   
 Select @result  
  
  
  
  
  