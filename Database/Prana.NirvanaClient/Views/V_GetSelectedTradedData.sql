CREATE view V_GetSelectedTradedData
as
select F.Symbol as Symbol, F.AveragePrice as AvgPrice,F.CumQty as CumQty,F.ClOrderID as ClOrderID from

(SELECT DISTINCT O.ParentClOrderID, MAX(F.NirvanaSeqNumber) AS NirvanaSeqNumber  
                            FROM          dbo.T_Order AS O INNER JOIN  
                                                   dbo.T_Sub AS S ON O.ParentClOrderID = S.ParentClOrderID INNER JOIN  
                                                   dbo.T_Fills AS F ON S.ClOrderID = F.ClOrderID  
                            GROUP BY O.ParentClOrderID) AS FillReport join dbo.T_Fills AS F ON F.NirvanaSeqNumber = FillReport.NirvanaSeqNumber 