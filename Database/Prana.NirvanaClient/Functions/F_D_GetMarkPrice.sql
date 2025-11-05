-- =============================================
-- Author:        <Alan Hau>
-- Create date: <3/28/2011>
-- Description:    <Gets mark price from Veda, which a shared server if client is missing markprice. Coalesce with this if you are missing data from the client server>
-- sample:        select dbo.F_D_getMarkPrice('3/25/2011', 'AA')
-- sample:        select dbo.F_D_getMarkPrice('6/2/2011', 'MORN')
-- =============================================

CREATE FUNCTION [dbo].[F_D_GetMarkPrice]

(
     @toDate datetime,
     @Symbol varchar(50)
)

RETURNS float

AS

BEGIN
    DECLARE @Result float
    Set @Result = nullif(dbo.GetMarkPriceForDayAndSymbol(@Symbol, @toDate, 0),0)
    RETURN @Result
END