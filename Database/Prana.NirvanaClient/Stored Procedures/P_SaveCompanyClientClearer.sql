


CREATE  PROCEDURE dbo.P_SaveCompanyClientClearer

	(
		@CompanyClientID int,
		@CompanyClientClearerName varchar(50),
		@CompanyClientClearerShortName varchar(50),
		@ClearingFirmBrokerID varchar(50)
	)

AS
declare @result int
if((select count(CompanyClientID) from t_companyclientclearer where CompanyClientID=@CompanyClientID) =0)
	begin
	insert into t_companyclientclearer(CompanyClientID,CompanyClientClearerName,CompanyClientClearerShortName,ClearingFirmBrokerID) values(@CompanyClientID,@CompanyClientClearerName,@CompanyClientClearerShortName,@ClearingFirmBrokerID)
	set @result = scope_identity()
	end
	else
	begin
	update t_companyclientclearer 
	set CompanyClientClearerName=@CompanyClientClearerName ,
	CompanyClientClearerShortName=@CompanyClientClearerShortName,ClearingFirmBrokerID=@ClearingFirmBrokerID
	set @result=@CompanyClientID
	end
	select  @result ;
	 



