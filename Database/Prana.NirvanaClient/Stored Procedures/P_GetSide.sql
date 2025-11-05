


/****** Object:  Stored Procedure dbo.P_GetSide    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetSide
(
		@sideID int
)
AS
SELECT     SideID, Side, SideTagValue
FROM         T_Side
Where SideID = @sideID



