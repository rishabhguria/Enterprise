

CREATE  VIEW dbo.V_FillDetails
AS
SELECT     ClOrderID, LastShares AS ExeQty, LastShares * LastPx AS Price,
Quantity as TotalQuantity 
FROM         dbo.T_Fills









