    
/* Object:  Stored Procedure P_SaveAUECWeeklyHolidays    Script Date: 07/10/2008  
 Created: Bhupesh Bareja  
 Purpose: To save weekly holidays for a given AUEC available.  
 Execute: P_SaveAUECWeeklyHolidays 1, 1  
*/  
CREATE PROCEDURE dbo.P_SaveAUECWeeklyHolidays  
 (    
  @Xml nText    
  ,@ErrorMessage varchar(500) output    
  ,@ErrorNumber int output
 )    
AS    
SET @ErrorMessage = 'Success'    
SET @ErrorNumber = 0    
BEGIN TRAN TRAN1     
    
BEGIN TRY    
    
DECLARE @handle int       
exec sp_xml_preparedocument @handle OUTPUT,@Xml       

----This code updates old data.    
--UPDATE T_AUECWeeklyHolidays    
--SET     
-- T_AUECWeeklyHolidays.WeeklyHolidayID = XmlItem.WeeklyHolidayID    
--      
--FROM     
-- OPENXML(@handle, '//WeeklyHoliday', 2)       
-- WITH     
--  (WeeklyHolidayID Integer, WeeklyHolidayName nvarchar(50), AUECID Integer)  XmlItem    
-- WHERE     
--  T_AUECWeeklyHolidays.AUECID = XmlItem.AUECID    

--This code inserts new data.    
    
Insert Into     
  T_AUECWeeklyHolidays    
   (    
    WeeklyHolidayID,     
    AUECID    
   )    
  SELECT     
    WeeklyHolidayID,     
    AUECID    
  FROM     
   OPENXML(@handle, '//WeeklyHoliday', 2)       
  WITH     
   (WeeklyHolidayID Integer, WeeklyHolidayName nvarchar(50), AUECID Integer)  XmlItem    
--Where XmlItem.AUECID Not IN (Select AUECID from T_AUECWeeklyHolidays)    
    
EXEC sp_xml_removedocument @handle    
    
COMMIT TRANSACTION TRAN1    
    
END TRY    
BEGIN CATCH     
 SET @ErrorMessage = ERROR_MESSAGE();    
 SET @ErrorNumber = Error_number();     
 ROLLBACK TRANSACTION TRAN1    
     
END CATCH;
 --Insert Data    
--  Insert Into T_AUECWeeklyHolidays(WeeklyHolidayID, AUECID)    
--  Values(@weeklyHolidayID, @auecID)    























     