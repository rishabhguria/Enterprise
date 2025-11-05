CREATE PROCEDURE dbo.P_GetRMAUEC
	(
	  @companyID int,
	  @aUECID int
	)
AS
	SELECT        RM_AUEC_ID, AUECID, AUEC_ExposureLimit_RMBaseCurrency, AUEC_ExposureLimit_BaseCurrency, 
	                         Maximum_PNLLoss_RMBaseCurrency, Maximum_PNLLoss_BaseCurrency, CompanyID
	FROM            T_RMAUECs
	WHERE        (CompanyID = @companyID) AND (AUECID = @aUECID) 