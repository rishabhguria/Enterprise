CREATE PROC [dbo].[P_GetMaxModelPortfolioId] 
AS 
  BEGIN 
      SELECT MAX(ModelPortfolioId) AS MaxModelPortfolioId FROM T_ModelPortfolios
  END 
