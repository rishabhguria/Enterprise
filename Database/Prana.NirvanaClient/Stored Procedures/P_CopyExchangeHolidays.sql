




/****** Object:  Stored Procedure dbo.P_CopyExchangeHolidays    Script Date: 11/21/2005 3:20:24 PM ******/
CREATE PROCEDURE [dbo].[P_CopyExchangeHolidays]
	(
		@destinationExchangeID int,
		@sourceExchangeID int,
         @holidayID varchar(max)
	)
AS
Declare @holidayIDTable TABLE
(
holidayID int
)
Insert into @holidayIDTable
select Cast(Items as int) from dbo.Split( @holidayID, ',')

Declare @ExchangeHolidayTable Table
(
ExchangeId int,
HolidayDate dateTime
)
Insert into @ExchangeHolidayTable
Select 
@destinationExchangeID,
EH.HolidayDate
From T_ExchangeHolidays EH
Where EH.ExchangeID=@SourceExchangeID  
      And EH.ExchangeHolidayID in (select holidayID from @holidayIDTable)
 
	
--DELETE FROM T_ExchangeHolidays WHERE ExchangeID = @destinationExchangeID

Delete T_ExchangeHolidays
From T_ExchangeHolidays EH
Inner Join @ExchangeHolidayTable TempHoliday
On TempHoliday.ExchangeID=EH.ExchangeID
And DateDiff(d,EH.HolidayDate,TempHoliday.HolidayDate)=0

Insert Into T_ExchangeHolidays
(
  ExchangeID,
  HolidayDate, 
  Description
)
 SELECT 
 @destinationExchangeID,
 EH.HolidayDate,
 EH.Description from T_ExchangeHolidays EH
 WHERE EH.ExchangeID = @sourceExchangeID and EH.ExchangeHolidayID in (select holidayID from @holidayIDTable)
		


		
		
	
	



