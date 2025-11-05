
/****** Object:  Stored Procedure dbo.P_GetCompanyClientClearer    Script Date: 01/17/2006 4:48:24 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyClientClearer
	(
		@CompanyClientID int
	)
AS
	--select  CompanyClientID,CompanyClientClearerName,CompanyClientClearerShortName,ClearingFirmBrokerID from T_CompanyClientClearer where CompanyClientID=@CompanyClientID
select  CompanyClientID,CompanyClientClearerName,CompanyClientClearerShortName,ClearingFirmBrokerID from T_CompanyClientClearer where CompanyClientID=@CompanyClientID




/*ALTER PROCEDURE   P_GetCompanyClientClearer  
@CompanyClientID  int 
AS
begin
select  * from T_CompanyClientClearer where CompanyClientID=@CompanyClientID
end*/
