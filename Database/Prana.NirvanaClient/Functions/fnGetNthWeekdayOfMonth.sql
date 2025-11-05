-- Usage : SELECT DBO.fnGetNthWeekdayOfMonth( '20110526',5,3)
-- This will give 3rd Friday of May 2011
CREATE FUNCTION dbo.fnGetNthWeekdayOfMonth
(
    @theDate DATETIME,
    @theWeekday TINYINT,
    @theNth SMALLINT
)
RETURNS DATETIME
BEGIN
    RETURN  (
                SELECT  theDate + DATEADD(DAY, DATEDIFF(DAY, @theDate, 0), @theDate)
                FROM    (
                            SELECT  DATEADD(DAY, 7 * @theNth - 7 * SIGN(SIGN(@theNth) + 1) +(@theWeekday + 6 - DATEDIFF(DAY, '17530101', DATEADD(MONTH, DATEDIFF(MONTH, @theNth, @theDate), '19000101')) % 7) % 7, DATEADD(MONTH, DATEDIFF(MONTH, @theNth, @theDate), '19000101')) AS theDate
                            WHERE   @theWeekday BETWEEN 1 AND 7
                                    AND @theNth IN (-5, -4, -3, -2, -1, 1, 2, 3, 4, 5)
                        ) AS d
                WHERE   DATEDIFF(MONTH, theDate, @theDate) = 0
            )
END