


CREATE PROCEDURE dbo.P_SaveCompanyAlertDetail
(
		--@rMCompanyAlertID int,
		@companyExposureLower int,
		@companyExposureUpper int,
		@alertTypeID int,
		@refreshRateCalculation int,
		--@rMCompanyOverallLimitID int,
		@alertMessage varchar(100),
		@emailAddress varchar(200),
		@blockTrading int,
		@companyID int,
		@rank int
			
	)
AS 
declare @overallID int, @result int
set @overallID = 0

SELECT     @overallID = RMCompanyOverallLimitID
FROM         T_RMCompanyOverallLimit
WHERE     (CompanyID = @companyID) 

INSERT INTO T_RMCompanyAlerts
                      (CompanyExposureUpper, AlertTypeID, RefreshRateCalculation, RMCompanyOverallLimitID, AlertMessage, EmailAddress, BlockTrading, CompanyID, 
                      Rank, CompanyExposureLower)
VALUES     (@companyExposureUpper, @alertTypeID, @refreshRateCalculation, @overallID, @alertMessage, @emailAddress, @blockTrading, @companyID, 
                      @rank, @companyExposureLower) 

		set @result = 1
		select @result   









