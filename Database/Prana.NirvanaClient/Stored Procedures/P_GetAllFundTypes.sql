


/****** Object:  Stored Procedure dbo.P_GetAllFundTypes    Script Date: 03/28/2006 2:45:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllFundTypes
AS
	SELECT   FundTypeID, FundTypeName
FROM         T_FundType Order By FundTypeName



