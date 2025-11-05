

/****** Object:  Stored Procedure dbo.P_GetAllGroupNames    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllFundGroups]

AS
BEGIN
	SELECT   FundGroupID, GroupName
    FROM     T_FundGroups
END
