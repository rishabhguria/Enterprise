

CREATE VIEW dbo.V_LatestFillByOrderSeqNumberNew            
AS            
          
SELECT     S.ParentClOrderID AS ParentClOrderID, MAX(F.NirvanaSeqNumber)  AS OrderSeqNumber     
FROM         dbo.T_Sub AS S INNER JOIN  dbo.T_Fills AS F ON F.OrderID = S.ParentClOrderID           
GROUP BY S.ParentClOrderID

