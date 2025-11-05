/*
Name : <P_GetFundType>
created by :<Kanupriya>
date : <10/16/2006>
Purpose: <to fetch the fundtype for a particular fundtypeiD>
*/

CREATE PROCEDURE [dbo].[P_GetFundType]

	(
	@fundTypeID int 
	
	)
	
AS
	SELECT     FundTypeID, FundTypeName
	FROM         T_FundType
	WHERE     (FundTypeID = @fundTypeID)
