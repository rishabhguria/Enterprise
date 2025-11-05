-----------------------------------------------------  
--Created By: Bharat raturi  
--Date: 12/may/2014  
--Purpose: Get the max file setting ID from the database 
--usage:    P_GetMaxFileSettingID
-----------------------------------------------------  
CREATE procedure [dbo].[P_GetMaxFileSettingID]  
as 
BEGIN
select max(ImportFileSettingID) from T_ImportFileSettings
END
