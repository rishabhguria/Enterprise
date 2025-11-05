-- =============================================        
-- Author:  Gaurav        
-- Create date: 9 Nov 2011        
-- Description: diff in mark price        
-- =============================================        
CREATE FUNCTION [dbo].[DIFFINMARKPRICEBYDATE] (@Symbol varchar(50),
@TodayDate datetime,
@Level1Id int)
RETURNS float
AS        
BEGIN        
  DECLARE @YesterdayDate datetime,
          @diff float
    
  SET @YesterdayDate = DATEADD(dd, -1, @TodayDate)

  IF EXISTS (SELECT
      1
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
    AND FundID = @Level1Id)
  BEGIN
    SET @diff = ISNULL((SELECT
  FinalMarkPrice             
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
    AND FundID = @Level1Id
    AND DATEDIFF(DAY, Date, @TodayDate) = 0
    AND ISActive = 1)
    , 0)
    - ISNULL((SELECT
  FinalMarkPrice             
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
  AND FundID = @Level1Id
    AND DATEDIFF(DAY, Date, @YesterdayDate) = 0
    AND ISActive = 1)
    , 0)
  END
  ELSE

  BEGIN
        
    SET @diff = ISNULL((SELECT
      FinalMarkPrice
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
    AND FundID = 0
    AND DATEDIFF(DAY, Date, @TodayDate) = 0
    AND ISActive = 1)
    , 0)
    - ISNULL((SELECT
      FinalMarkPrice
    FROM PM_DayMarkPrice
    WHERE Symbol = @Symbol
    AND FundID = 0
    AND DATEDIFF(DAY, Date, @YesterdayDate) = 0
    AND ISActive = 1)
    , 0)
  END
   
  RETURN ROUND(@diff, 8)
        
END 