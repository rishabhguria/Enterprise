CREATE PROCEDURE [dbo].[P_GetAllBasicSides]  
AS  
SELECT     SideID, Side, SideTagValue  
FROM         T_Side Where IsBasicSide= 1 Order by Side  


