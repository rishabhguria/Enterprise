CREATE PROCEDURE P_GetVolatilityData
(
@securityName varchar(10),
@userID int
)
AS
BEGIN
select SecurityName,CallPut,StrikePrice ,UnderlyingSymbol ,AutoVolatility ,UserVolatility ,UserID
from T_Volatility

where
SecurityName = @SecurityName and
UserID = @userID

END