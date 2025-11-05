CREATE PROC [dbo].[P_DeleteModelPortfolio] 
(
@ModelPortfolioId int
)                                                                                    
AS 
  BEGIN 
      DELETE FROM T_ModelPortfolios WHERE ModelPortfolioId = @ModelPortfolioId
  END 