CREATE TABLE [dbo].[T_ModelPortfolios] 
  ( 
	 [ModelPortfolioId] INT   NOT NULL,
     [ModelPortfolioName]   Varchar(50) NOT NULL , 
     [ModelPortfolioTypeId] INT NOT NULL, 
     [ReferenceId]          INT NULL, 
     [ModelPortfolioData]   TEXT NULL, 
     [PositionsTypeId]      INT NULL,
     [UseToleranceId]      INT NULL,
     [ToleranceFactorId]      INT NULL,
     [TargetPercentTypeId]      INT NULL,
    CONSTRAINT [FK_T_ModelPortfolios_T_ModelPortfolioType] FOREIGN KEY ( 
     [ModelPortfolioTypeId]) REFERENCES [dbo].[T_ModelPortfolioType] ( 
     [ModelPortfolioTypeId]), 
    CONSTRAINT [PK_T_ModelPortfolios] PRIMARY KEY ([ModelPortfolioId]), 
    CONSTRAINT [AK_T_ModelPortfolios] UNIQUE ([ModelPortfolioName]) 
  ) 