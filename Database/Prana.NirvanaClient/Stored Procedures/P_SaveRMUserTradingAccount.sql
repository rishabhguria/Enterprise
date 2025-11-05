CREATE PROCEDURE dbo.P_SaveRMUserTradingAccount
	
	(
	@rMUserTradingAccntID int ,
	@companyID int,
	@tradAccntID int,
	@companyUserID int,
	@exposureLimit int,
	@result int
	
	)
	
AS 
if(@rMUserTradingAccntID >0)
begin
UPDATE       T_RMUserTradingAccount
SET                 UserTAExposureLimit = @exposureLimit 
WHERE        (RMUserTradingAccntID = @rMUserTradingAccntID)

set @result = -1
end
else
begin
		INSERT INTO  T_RMUserTradingAccount
		 (CompanyID, CompanyUserID, UserTradingAccntID,UserTAExposureLimit)
		VALUES        (@companyID,@companyUserID,@tradAccntID,@exposureLimit)  
			
		Set @result = scope_identity()
end
select @result