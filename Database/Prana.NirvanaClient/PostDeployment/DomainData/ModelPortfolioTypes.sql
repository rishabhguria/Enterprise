 IF NOT EXISTS(SELECT * 
              FROM   T_ModelPortfolioType 
              WHERE  ModelPortfolioType = 'MasterFund') 
  BEGIN 
      INSERT INTO T_ModelPortfolioType 
                  (ModelPortfolioTypeId, 
                   ModelPortfolioType) 
      VALUES     (1, 
                  'MasterFund'); 
  END 
IF NOT EXISTS(SELECT * 
              FROM   T_ModelPortfolioType 
              WHERE  ModelPortfolioType = 'Account') 
  BEGIN 
      INSERT INTO T_ModelPortfolioType 
                  (ModelPortfolioTypeId, 
                   ModelPortfolioType) 
      VALUES     (2, 
                  'Account'); 
  END 
IF NOT EXISTS(SELECT * 
              FROM   T_ModelPortfolioType 
              WHERE  ModelPortfolioType = 'ModelPortfolio') 
  BEGIN 
      INSERT INTO T_ModelPortfolioType 
                  (ModelPortfolioTypeId, 
                   ModelPortfolioType) 
      VALUES     (3, 
                  'ModelPortfolio'); 
  END

  IF NOT EXISTS(SELECT * 
              FROM   T_ModelPortfolioType 
              WHERE  ModelPortfolioType = 'CustomGroup') 
  BEGIN 
      INSERT INTO T_ModelPortfolioType 
                  (ModelPortfolioTypeId, 
                   ModelPortfolioType) 
      VALUES     (4, 
                  'CustomGroup'); 
  END