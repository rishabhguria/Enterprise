
CREATE view [dbo].[V_UniqueSymbol_Prana]  
as  
--select Distinct Symbol from PM_YTDPnL  
--union  
SELECT DISTINCT Symbol FROM T_Group 
--union
--SELECT DISTINCT Symbol FROM PM_Taxlots
--union
--SELECT DISTINCT Symbol FROM T_Order
