CREATE FUNCTION [dbo].[UTCtoLocalDateF] (@UTCDate DATETIME, @TZ INT) 
RETURNS DATETIME AS 
BEGIN 
IF ( DATEPART(hh, @UTCDate) >= 0 ) 
BEGIN 
DECLARE @LocalDate DATETIME 
DECLARE @UTCDelta INT 
DECLARE @thisYear INT 
DECLARE @DSTDay INT 
DECLARE @NormalDay INT 
DECLARE @DSTDate DATETIME 
DECLARE @NormalDate DATETIME 

SET @thisYear = YEAR(@UTCDate) 

IF (@thisYear < 2007 ) 
BEGIN 
SET @DSTDay = ( 2 + 6 * @thisYear - FLOOR(@thisYear / 4) ) % 7 + 1 
SET @NormalDay = ( 31 - ( FLOOR( @thisYear * 5 / 4) + 1) % 7) 

SET @DSTDate = '4/' + CAST(@DSTDay AS VARCHAR(2)) + '/' + CAST(@thisYear AS VARCHAR(4)) + ' 2:00:00.000 AM' 
SET @NormalDate = '10/' + CAST(@NormalDay AS VARCHAR(2)) + '/' + CAST(@thisYear AS VARCHAR(4)) + ' 2:00:00.000 AM' 
END 
ELSE 
BEGIN 
SET @DSTDay = ( 14 - ( FLOOR( 1 + @thisYear * 5 / 4 ) ) % 7 ) 
SET @NormalDay = ( 7 - ( FLOOR ( 1 + @thisYear * 5 / 4) ) % 7 ) 

SET @DSTDate = '3/' + CAST(@DSTDay AS VARCHAR(2)) + '/' + CAST(@thisYear AS VARCHAR(4)) + ' 2:00:00.000 AM' 
SET @NormalDate = '11/' + CAST(@NormalDay AS VARCHAR(2)) + '/' + CAST(@thisYear AS VARCHAR(4)) + ' 2:00:00.000 AM' 
END 

IF ((@UTCDate > @DSTDate) AND (@UTCDate < @NormalDate)) 
BEGIN 
SET @UTCDelta = @TZ + 1 
END 
ELSE 
BEGIN 
SET @UTCDelta = @TZ 
END 
-- now convert utc date to local date 
SET @LocalDate = DATEADD(Hour, @UTCDelta, @UTCDate) 
END 
ELSE 
BEGIN 
SET @LocalDate = @UTCDate 
END 
RETURN(@LocalDate) 
END
