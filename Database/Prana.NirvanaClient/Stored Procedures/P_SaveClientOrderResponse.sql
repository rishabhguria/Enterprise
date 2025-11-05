CREATE PROCEDURE [dbo].[P_SaveClientOrderResponse] (
	@AvgPrice FLOAT
	,@ClOrdID VARCHAR(50)
	,@CumQty FLOAT
	,@ExecID VARCHAR(50)
	,@ExecTransType CHAR
	,@ExecType CHAR
	,@LastMarket VARCHAR(50)
	,@LastPrice FLOAT
	,@LastShares FLOAT
	,@LeavesQty FLOAT
	,@MsgSeqNum VARCHAR(50)
	,@MsgType VARCHAR(50)
	,@OrderID VARCHAR(500)
	,@OrderStatus CHAR
	,@OrderType CHAR
	,@OrigClOrderID VARCHAR(50)
	,@Price FLOAT
	,@Quantity FLOAT
	,@SenderCompID VARCHAR(50)
	,@SendingTime VARCHAR(50)
	,@Side CHAR
	,@Symbol VARCHAR(50)
	,@TargetCompID VARCHAR(50)
	,@Text VARCHAR(200)
	,@TimeInForce CHAR
	,@TransactionTime VARCHAR(50)
	,@ServerRecieveTime VARCHAR(50) -- this is the field order.SendingTime    
	,@fillSeqNumber BIGINT
	,@SenderSubID VARCHAR(50)
	)
AS
DECLARE @result INT
 
INSERT INTO T_ClientFills (
	AveragePrice
	,ClOrderID
	,CumQty
	,ExecutionID
	,ExecTransType
	,ExecType
	,LastMkt
	,LastPx
	,LastShares
	,LeavesQty
	,MsgSeqNumber
	,MsgType
	,OrderID
	,OrderStatus
	,OrderTypeID
	,OrigClOrderID
	,Price
	,Quantity
	,SenderCompID
	,SendingTime
	,SideID
	,Symbol
	,TargetCompID
	,TEXT
	,TimeInForceID
	,TransactTime
	,InsertionTime
	,FillRecieveServerTime
	,NirvanaSeqNumber
	,SenderSubID
	)
VALUES (
	@AvgPrice
	,@ClOrdID
	,@CumQty
	,@ExecID
	,@ExecTransType
	,@ExecType
	,@LastMarket
	,@LastPrice
	,@LastShares
	,@LeavesQty
	,@MsgSeqNum
	,@MsgType
	,@OrderID
	,@OrderStatus
	,@OrderType
	,@OrigClOrderID
	,@Price
	,@Quantity
	,@SenderCompID
	,@SendingTime
	,@Side
	,@Symbol
	,@TargetCompID
	,@Text
	,@TimeInForce
	,@TransactionTime
	,GetUTCDate()
	,@ServerRecieveTime
	,@fillSeqNumber
	,@SenderSubID
	)

SET @result = scope_identity()

SELECT @result