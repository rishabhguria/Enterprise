
-- =============================================
-- Author:		<Harsh Kumar>
-- Create date: <07-06-07>
-- Description:	<to save auto and manual volatilities calculated from analytics form>
-- =============================================
CREATE PROCEDURE [dbo].[P_SaveVolatilityData]
(
@securityName varchar(10),
@callPut varchar(10),
@UnderlyingSymbol varchar(10),
@strikePrice float,
@autoVolatility float,
@manualVolatility float,
@userID int
)
AS
BEGIN
if((select count(*) from T_Volatility where SecurityName = @securityName)=0)
insert into T_Volatility 
(
SecurityName,
CallPut,
StrikePrice ,
UnderlyingSymbol ,
AutoVolatility ,
UserVolatility ,
UserID
)
values
(
@securityName,
@callPut, 
@strikePrice ,
@UnderlyingSymbol ,
@autoVolatility ,
@manualVolatility,
@userID
)
else
update T_Volatility 
set 
CallPut = @callPut, 
StrikePrice =  @strikePrice,
UnderlyingSymbol = @UnderlyingSymbol ,
AutoVolatility = @autoVolatility ,
UserVolatility = @manualVolatility
--UserID = @userID

where 
UserID = @userID and SecurityName = @securityName

END
