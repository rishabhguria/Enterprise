CREATE procedure [dbo].[P_GetLastSeqNumberReceived] 
( 
  @counterPartyIdentifier varchar(10),
  @ResetTime DateTime
)  
AS  
  
DECLARE @MsgSeqNumber BIGINT
DECLARE @TransactTime VARCHAR(100)

SELECT @TransactTime = max(TransactTime) FROM T_Fills WHERE TargetCompID= @counterPartyIdentifier
SELECT @MsgSeqNumber = max(MsgSeqNumber) FROM T_Fills WHERE TransactTime = @TransactTime AND TargetCompID= @counterPartyIdentifier

IF @ResetTime > CONVERT(DATETIME, REPLACE(@TransactTime, '-', ' '))
	SELECT 0
ELSE
	SELECT @MsgSeqNumber    
  
--select * from T_Fills