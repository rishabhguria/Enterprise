CREATE PROC [dbo].[P_GetModelPortfolios] 
AS 
  BEGIN 
      SELECT ModelPortfolioId,
			 ModelPortfolioName, 
             MP.ModelPortfolioTypeId, 
             ModelPortfolioType,
             PositionsTypeId,
             UseToleranceId,
             ToleranceFactorId,
             TargetPercentTypeId,
             ReferenceId, 
             MP.ModelPortfolioData
      FROM   T_ModelPortfolios MP 
             INNER JOIN T_ModelPortfolioType MPT 
                     ON MP.ModelPortfolioTypeId = MPT.ModelPortfolioTypeId 
             LEFT JOIN T_CompanyMasterFunds MF 
                    ON MF.CompanyMasterFundID = MP.ReferenceId 
             LEFT JOIN T_CompanyFunds Funds 
                    ON Funds.CompanyFundID = MP.ReferenceId 
  END 