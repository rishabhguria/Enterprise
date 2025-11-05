



CREATE PROCEDURE dbo.P_GetHolidays
	
AS
declare @nullExchangeID int
--set @nullExchangeID = -33
Select HolidayID, HolidayDate, Description, -33
From T_Holidays

	


