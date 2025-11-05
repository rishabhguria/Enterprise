CREATE PROCEDURE [dbo].[P_GetCompanyPermittedOrderTypes] (@companyID INT)
AS
SELECT T_CompanyOrderTypes.OrderTypeID
	,T_OrderType.OrderTypes
	,T_OrderType.OrderTypeTagValue
FROM T_CompanyOrderTypes
INNER JOIN T_OrderType
	ON T_CompanyOrderTypes.OrderTypeID = T_OrderType.OrderTypesID and CompanyID = @companyID