


/****** Object:  Stored Procedure dbo.P_GetAllFixs    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllFixs
AS
	SELECT   FIXID, FixVersion
FROM         T_FIX Order By FixVersion


