
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date, ,>  
-- Description: <Description, ,>  
-- =============================================  
CREATE FUNCTION [dbo].[GetMarkPriceForDayAndSymbol] (@Symbol varchar(50),
 @date datetime,
@Level1Id int)
RETURNS float
  
AS  
BEGIN  
   
  DECLARE @diff float
  
  IF EXISTS (SELECT
      1
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
    AND FundID = @Level1Id)
  BEGIN
    SET @diff = (SELECT
      finalmarkprice
    FROM pm_daymarkprice
    WHERE symbol = @Symbol
    AND fundid = @Level1Id
    AND DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) = DATEADD(DAY, DATEDIFF(DAY, 0, dbo.Getformatteddatepart(@date)), 0))

  END
  ELSE
  BEGIN
    SET @diff = (SELECT
      finalmarkprice
    FROM pm_daymarkprice
    WHERE symbol = @Symbol
    AND fundid = 0
    AND DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) = DATEADD(DAY, DATEDIFF(DAY, 0, dbo.Getformatteddatepart(@date)), 0))

  END
  RETURN ROUND(@diff, 8)
END  
