CREATE PROCEDURE dbo.P_SaveRMAUECs
	(
		@rMAUECID int,
        @companyID int,
        @aUECID int,
        @exposureLimitRMBaseCurrency int,
        @exposureLimitBaseCurrency int,
        @maximumPNLLossRMBaseCurrency int,
        @maximumPNLLossBaseCurrency int,
        @result int
	
			
	)
AS 
if(@rMAUECID >0)
begin
UPDATE       T_RMAUECs
SET                AUEC_ExposureLimit_RMBaseCurrency = @exposureLimitRMBaseCurrency, 
                         AUEC_ExposureLimit_BaseCurrency = @exposureLimitBaseCurrency, 
                         Maximum_PNLLoss_BaseCurrency = @maximumPNLLossRMBaseCurrency, 
                         Maximum_PNLLoss_RMBaseCurrency = @maximumPNLLossBaseCurrency
WHERE        (RM_AUEC_ID = @rMAUECID)

set @result = -1
end
else
begin
		INSERT INTO T_RMAUECs
		 (AUECID, AUEC_ExposureLimit_RMBaseCurrency, AUEC_ExposureLimit_BaseCurrency,
		  CompanyID, Maximum_PNLLoss_BaseCurrency, 
		                         Maximum_PNLLoss_RMBaseCurrency)
		VALUES        (@aUECID,@exposureLimitRMBaseCurrency,@exposureLimitBaseCurrency,
		@companyID,@maximumPNLLossRMBaseCurrency,@maximumPNLLossBaseCurrency)  
			
		Set @result = scope_identity()
end
select @result