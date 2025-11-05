CREATE PROCEDURE dbo.P_SaveRMTradingAccount

	(
	@companyTradingAccntRMID int ,
	@companyID int,
	@companyTradingAccntID int,
	@tAExposureLimit int ,
	@tAPositivePNL int,
	@tANegativePNL int,
	@result int
	
	)
	
AS
	
if(@companyTradingAccntRMID > 0)
begin
UPDATE       T_RMCompanyTradAccntOverall
SET                TAExposureLimit = @tAExposureLimit,
				   TAPositivePNL = @tAPositivePNL, 
				   TANegativePNL = @tANegativePNL
WHERE        (CompanyTradAccntRMID = @companyTradingAccntRMID)

set @result = -1
end
else
begin
		INSERT INTO T_RMCompanyTradAccntOverall
		                         (CompanyID, CompanyTradAccntID, TAExposureLimit, TAPositivePNL, TANegativePNL)
		VALUES        (@companyID,@companyTradingAccntID,@tAExposureLimit,@tAPositivePNL,@tANegativePNL)  
			
		Set @result = scope_identity()
end
select @result