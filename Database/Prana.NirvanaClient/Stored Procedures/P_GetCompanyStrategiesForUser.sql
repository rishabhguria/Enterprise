


/****** Object:  Stored Procedure dbo.P_GetCompanyStrategiesForUser    Script Date: 04/12/2006 2:50:24 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyStrategiesForUser
(
		@companyUserID int		
)
AS
	
	Select CUS.CompanyUserStrategyID, CUS.CompanyStrategyID, CS.StrategyName
	From T_CompanyStrategy CS, T_CompanyUserStrategies CUS
	Where CS.CompanyStrategyID = CUS.CompanyStrategyID
	And CUS.CompanyUserID = @companyUserID


	

