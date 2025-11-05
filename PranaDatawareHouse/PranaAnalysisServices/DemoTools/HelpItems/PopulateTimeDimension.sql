--USE YOUR_DATA_WAREHOUSE
--optional delete contents of Date Dimension Table
--comment out if you do not want to clear table before use!!!
TRUNCATE TABLE Dim_Time
--declare variables
DECLARE @FullDate DATETIME
DECLARE @DayofWeek tinyint
DECLARE @DayName nvarchar(10)
DECLARE @DayofMonth tinyint
DECLARE @DayofYear smallint
DECLARE @MonthName nvarchar(10)
DECLARE @CalWEEK  tinyint
DECLARE @CalMONTH  int
DECLARE @CalQUARTER tinyint
DECLARE @CalYEAR int
DECLARE @FisWEEK  tinyint
DECLARE @FisMONTH  int
DECLARE @FisQUARTER tinyint
DECLARE @FisYEAR int
DECLARE @IsWeekend BIT
--DECLARE @IsHoliday  BIT
DECLARE @LeapYear BIT
--initialize variables
--at the moment, our calendar year & fiscal years are the same,
--however I have created a separate Fiscal time dimension in case this ever changes.
--In which case, the SQL below would need to be amended
SELECT @FisWeek = 1 --assuming we start with 1 Jan YYYY
SELECT @CalWeek = 1
SELECT @LeapYear =0
--the starting date for the date dimension
SELECT @FullDate  = '1/1/1970'
--start looping, stop at ending date
WHILE (@FullDate <= '12/31/2020')
BEGIN
--get information about the data
 SELECT @FullDate = @FullDate
 SELECT @DayofWeek   = DATEPART (DW , @FullDate)
 SELECT @DayName = DATENAME(Weekday,@FullDate)
 SELECT @DayofMonth   = DATEPART (DAY , @FullDate)
 SELECT @DayofYear   = DATEPART (DY , @FullDate)
 SELECT @MonthName = DATENAME(Month,@FullDate)
 SELECT @CalYEAR = DATEPART (YEAR, @FullDate)
 SELECT @CalQUARTER = DATEPART (QUARTER, @FullDate)
 SELECT @CalMONTH = DATEPART (MONTH , @FullDate)
 SELECT @CalWEEK  = DATEPART (WEEK , @FullDate)
 SELECT @FisYEAR = DATEPART (YEAR, @FullDate)
 SELECT @FisQUARTER = DATEPART (QUARTER, @FullDate)
 SELECT @FisMONTH = DATEPART (MONTH , @FullDate)
 SELECT @FisWEEK  = DATEPART (WEEK , @FullDate)
 SELECT @IsWeekend  = 0
--note if weekend or not
IF ( @DayofWeek = 1 OR  @DayofWeek = 7 )
BEGIN
 SELECT @IsWeekend   = 1
END
--check for leap year
IF ((@calyear % 4 = 0)  AND (@calYEAR % 100 != 0 OR @calYEAR % 400 = 0))
 SELECT @LeapYear =1
 ELSE SELECT @LeapYear =0
--insert values into Date Dimension table
INSERT Dim_Time
 ( FullDateAlternateKey,
  DayNumberOfWeek,
  DayNameOfWeek,
  DayNumberOfMonth,
  DayNumberOfYear,
  MonthName,
  CalendarWeekNo,
  CalendarMonthNo,
  CalendarQuarter,
  CalendarYear,
  FiscalWeekNo,
  FiscalMonthNo,
  FiscalQuarter,
  FiscalYear,
  IsWeekend,
 -- IsHoliday,
  IsLeapYear
 )
 VALUES
 ( @FullDate,
  @DayofWeek,
  @DayName,
  @DayofMonth,
  @DayofYear,
  @MonthName,
  @CalWEEK,
  @CalMONTH,
  @CalQUARTER,
  @CalYEAR,
  @FisWEEK,
  @FisMONTH,
  @FisQUARTER,
  @FisYEAR,
  @IsWeekend,
 -- @IsHoliday,
  @LeapYear
 )
--increment the date one day
SELECT @FullDate  = DATEADD(DAY, 1, @Fulldate)
END
GO

Update
Dim_Time
set IsHoliday = IsWeekend