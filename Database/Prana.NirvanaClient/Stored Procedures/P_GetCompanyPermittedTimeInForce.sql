CREATE PROCEDURE [dbo].[P_GetCompanyPermittedTimeInForce] (@companyID INT)
AS
SELECT T_CompanyTimeInForce.TimeInForceID
	,T_TimeInForce.TimeInForce
	,T_TimeInForce.TimeInForceTagValue
FROM T_CompanyTimeInForce
INNER JOIN T_TimeInForce
	ON T_CompanyTimeInForce.TimeInForceID = T_TimeInForce.TimeInForceID  and CompanyID = @companyID