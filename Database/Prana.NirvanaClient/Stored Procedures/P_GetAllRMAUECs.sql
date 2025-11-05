CREATE PROCEDURE dbo.P_GetAllRMAUECs
	(
		@companyID int
	)
AS
	SELECT        RM_AUEC_ID, AUECID, AUEC_ExposureLimit_RMBaseCurrency, AUEC_ExposureLimit_BaseCurrency, Maximum_PNLLoss_BaseCurrency, 
	                         Maximum_PNLLoss_RMBaseCurrency, CompanyID
	FROM            T_RMAUECs
	WHERE        (CompanyID = @companyID)
