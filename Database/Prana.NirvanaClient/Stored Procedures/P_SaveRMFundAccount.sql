CREATE PROCEDURE dbo.P_SaveRMFundAccount
(
		@companyFundAccntRMID int,
        @companyID int,
        @companyFundAccntID int,
        @exposureLimitRMBaseCurrency int,
        @fAPositivePNL int,
        @fANegativePNL int,
        @result int
	
			
)
AS 
if(@companyFundAccntRMID >0)
begin
UPDATE       T_RMCompanyFundAccntOverall
SET                ExposureLimitRMBaseCurrency = @exposureLimitRMBaseCurrency, 
					FANegativePNL = @fANegativePNL,
					FAPositivePNL = @fAPositivePNL
WHERE        (CompanyFundAccntRMID = @companyFundAccntRMID)


set @result = -1
end
else
begin
INSERT INTO T_RMCompanyFundAccntOverall
                         (ExposureLimitRMBaseCurrency, CompanyID, FANegativePNL, FAPositivePNL, CompanyFundAccntID)
VALUES        (@exposureLimitRMBaseCurrency,@companyID,@fANegativePNL,@fAPositivePNL,@companyFundAccntID)
		
		Set @result = scope_identity()
end
select @result