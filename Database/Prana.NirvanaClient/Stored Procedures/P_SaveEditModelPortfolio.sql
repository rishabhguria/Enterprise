CREATE Proc [dbo].[P_SaveEditModelPortfolio]
(
@ModelPortfolioId int,
@ModelPortfolioName varchar(max),
@ModelPortfolioTypeId int,
@ReferenceId int,
@ModelPortfolioData varchar(max),
@PositionsTypeId int,
@UseToleranceId int,
@ToleranceFactorId int,
@TargetPercentTypeId int
)                                                                                   
AS                                
BEGIN  

DECLARE @tempReferenceId int
DECLARE @tempModelPortfolioData varchar(max)

SET @tempReferenceId = CASE WHEN @ModelPortfolioTypeId in (1,2,3,4) THEN @ReferenceId
		   ELSE NULL END

IF NOT EXISTS(SELECT * 
              FROM   T_ModelPortfolios 
              WHERE  ModelPortfolioId = @ModelPortfolioId)
   BEGIN
INSERT INTO [dbo].[T_ModelPortfolios]
           ([ModelPortfolioId]
		   ,[ModelPortfolioName]
           ,[ModelPortfolioTypeId]
           ,[ReferenceId]
           ,[ModelPortfolioData]
		   ,[PositionsTypeId]
		   ,[UseToleranceId]
		   ,[ToleranceFactorId]
		   ,[TargetPercentTypeId])

		   SELECT 
		    @ModelPortfolioId
		   ,@ModelPortfolioName
		   ,@ModelPortfolioTypeId
		   ,@tempReferenceId
		   ,@ModelPortfolioData
		   ,@PositionsTypeId
		   ,@UseToleranceId
		   ,@ToleranceFactorId
		   ,@TargetPercentTypeId
    END   
 ELSE
	    UPDATE T_ModelPortfolios
		SET ModelPortfolioName = @ModelPortfolioName,
		ModelPortfolioTypeId = @ModelPortfolioTypeId,
		ReferenceId = @tempReferenceId,
        ModelPortfolioData = @ModelPortfolioData,
		PositionsTypeId = @PositionsTypeId,
		UseToleranceId = @UseToleranceId,
		ToleranceFactorId = @ToleranceFactorId,
		TargetPercentTypeId = @TargetPercentTypeId
		WHERE ModelPortfolioId = @ModelPortfolioId
END

