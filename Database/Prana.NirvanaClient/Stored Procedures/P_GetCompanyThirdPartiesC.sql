

CREATE PROCEDURE dbo.P_GetCompanyThirdPartiesC
	
AS
	SELECT     CTP.CompanyID, CTP.ThirdPartyID, TP.ThirdPartyName, TP.Description
	FROM         T_CompanyThirdParty CTP, T_ThirdParty TP
	WHERE CTP.ThirdPartyID = TP.ThirdPartyID

