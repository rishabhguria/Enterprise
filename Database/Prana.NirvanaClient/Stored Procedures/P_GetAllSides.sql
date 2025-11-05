


/****** Object:  Stored Procedure dbo.P_GetAllSides    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllSides
AS
SELECT     SideID, Side, SideTagValue
FROM         T_Side Order by Side
