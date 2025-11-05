   
/****************************************************************************    
Execution Statement :   
  
   select * from PM_CompanyStrategyDailyPNL  
	Declare @ErrorMessage varchar(500) 
	Declare @ErrorNumber int 
	 exec [PMSaveCompanyStrategyDailyPNL] '<?xml version="1.0"?>
	<ArrayOfCompanyStrategyDailyPNL xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	  <CompanyStrategyDailyPNL>
		<BrokenRulesCollection />
		<CompanyID>11</CompanyID>
		<StrategyID>19</StrategyID>
		<Date>Aug 23 2007</Date>
		<ApplicationRealizedPNL>36</ApplicationRealizedPNL>
		<PBRealizedPNL>0</PBRealizedPNL>
		<ApplicationUnrealizedPNL>10</ApplicationUnrealizedPNL>
		<PBUnrealizedPNL>0</PBUnrealizedPNL>
	  </CompanyStrategyDailyPNL> 
	  <CompanyStrategyDailyPNL>
		<BrokenRulesCollection />
		<CompanyID>1</CompanyID>
		<StrategyID>1</StrategyID>
		<Date>Aug 16 2007</Date>
		<ApplicationRealizedPNL>123.65</ApplicationRealizedPNL>
		<PBRealizedPNL>0</PBRealizedPNL>
		<ApplicationUnrealizedPNL>10</ApplicationUnrealizedPNL>
		<PBUnrealizedPNL>0</PBUnrealizedPNL>
	  </CompanyStrategyDailyPNL>  
	</ArrayOfCompanyStrategyDailyPNL>',@ErrorMessage,@ErrorNumber
  
Date Created: Aug-24-2007
Description: Insertion and Updation for CompanyStrategyDailyPNL Values 
Created By: Sandeep
****************************************************************************/  
CREATE Proc [dbo].[PMSaveCompanyStrategyDailyPNL]  
(  
   @Xml nText --XML input
 , @ErrorMessage varchar(500) output  
 , @ErrorNumber int output  
)  
AS   
  
SET @ErrorMessage = 'Success'  
SET @ErrorNumber = 0  
BEGIN TRAN TRAN1   
  
BEGIN TRY  

DECLARE @handle int     
exec sp_xml_preparedocument @handle OUTPUT,@Xml     

--This code updates old data.  
UPDATE PM_CompanyStrategyDailyPNL  
SET   
	PM_CompanyStrategyDailyPNL.ApplicationRealizedPNL = XmlItem.ApplicationRealizedPNL,  
	PM_CompanyStrategyDailyPNL.PBRealizedPNL = XmlItem.PBRealizedPNL , 
	PM_CompanyStrategyDailyPNL.ApplicationUnrealizedPNL = XmlItem.ApplicationUnrealizedPNL , 
	PM_CompanyStrategyDailyPNL.PBUnrealizedPNL = XmlItem.PBUnrealizedPNL     
FROM   
 OPENXML(@handle, '/ArrayOfCompanyStrategyDailyPNL/CompanyStrategyDailyPNL', 2)     
 WITH   
  (
		CompanyID int,
		StrategyID int,
		Date varchar(100) ,
		ApplicationRealizedPNL float,
		PBRealizedPNL float,
		ApplicationUnrealizedPNL float,
		PBUnrealizedPNL float)  XmlItem  
 WHERE   
		PM_CompanyStrategyDailyPNL.CompanyID = XmlItem.CompanyID And 
		PM_CompanyStrategyDailyPNL.StrategyID =  XmlItem.StrategyID  And
		CONVERT(VARCHAR(20), PM_CompanyStrategyDailyPNL.Date, 107)= CONVERT(VARCHAR(20),Cast(XmlItem.Date as datetime), 107)--Aug 24, 2007
  
--This code inserts new data.  

Insert Into   
  PM_CompanyStrategyDailyPNL  
   (       
	 CompanyID, 
     StrategyID,   
     Date,   
     ApplicationRealizedPNL,
     PBRealizedPNL,
     ApplicationUnrealizedPNL,
     PBUnrealizedPNL
   )  
  SELECT   
		CompanyID,   
		StrategyID,   
		Convert(Datetime, Date, 107), --Getutcdate(),
		ApplicationRealizedPNL,
		PBRealizedPNL,
		ApplicationUnrealizedPNL,
		PBUnrealizedPNL
  FROM   
		OPENXML(@handle, '/ArrayOfCompanyStrategyDailyPNL/CompanyStrategyDailyPNL', 2)     
  WITH   
   (
		CompanyID int,
		StrategyID int, 
		Date varchar(100) ,
		ApplicationRealizedPNL float,
		PBRealizedPNL float,
		ApplicationUnrealizedPNL float,
		PBUnrealizedPNL float)  XmlItem 
			Where XmlItem.CompanyID Not IN (Select CompanyID from PM_CompanyStrategyDailyPNL)
				  OR XmlItem.StrategyID Not IN (Select StrategyID from PM_CompanyStrategyDailyPNL)
				  OR CONVERT(VARCHAR(20), Cast(XmlItem.Date as datetime), 107) Not IN (Select CONVERT(VARCHAR(20), Date, 107) from PM_CompanyStrategyDailyPNL)
  
EXEC sp_xml_removedocument @handle  
  
COMMIT TRANSACTION TRAN1  
  
END TRY  
BEGIN CATCH   
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();   
 ROLLBACK TRANSACTION TRAN1     
END CATCH;  
  