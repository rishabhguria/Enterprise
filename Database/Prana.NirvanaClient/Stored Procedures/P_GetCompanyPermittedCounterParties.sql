CREATE PROCEDURE dbo.P_GetCompanyPermittedCounterParties (@companyID INT)
AS
SELECT T_CounterParty.CounterPartyID
	,T_CounterParty.ShortName
FROM T_CounterParty
INNER JOIN T_CompanyCounterParties
	ON T_CounterParty.CounterPartyID = T_CompanyCounterParties.CounterPartyID
WHERE T_CompanyCounterParties.CompanyID = @companyID AND T_CounterParty.IsOTDorEMS = 0;
