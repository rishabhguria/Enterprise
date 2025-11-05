/****************************************************************************          
Name :   [PMGetClosedPositionsExceptionalTransactions]          
Date Created: 07-25-2019
Purpose:  Get the closing transaction of FX and FX Forward asset class trades for which cash activity is missing.          
Execution Statement:           
   EXEC [PMGetClosedPositionsExceptionalTransactions] 
	@FromDate = '2019-07-01'
	,@ToDate = '2019-07-25'
	,@FundIds = ''      
****************************************************************************/ 
CREATE PROCEDURE [dbo].[PMGetClosedPositionsExceptionalTransactions]
(                                           
	@FromDate	DATETIME,
	@ToDate		DATETIME,                                                                                                                                                                                
	@FundIds	VARCHAR(MAX)                                                                                          
)                                                                                                                                
AS                                                                                
BEGIN                                                                                                             
 
DECLARE @Local_FromDate	DATETIME    
DECLARE @Local_ToDate	DATETIME     
DECLARE @Local_FundIds	VARCHAR(MAX)    

SET @Local_FromDate = @FromDate    
SET @Local_ToDate = @ToDate    
SET @Local_FundIds = @FundIds    
  
                                                                                                
DECLARE @ToAUECDatesTable TABLE 
(
	AUECID			INT,
	CurrentAUECDate	DATETIME
)
                                                                                                
CREATE TABLE #Funds 
(
	FundID	INT
)
 
 IF (@Local_FundIds is NULL or @Local_FundIds = '')  
 BEGIN                                                  
	INSERT INTO #Funds
	SELECT CompanyFundID AS FundID 
	FROM T_CompanyFunds Where IsActive=1                                                 
 END
 ELSE
 BEGIN                                                    
	INSERT INTO #Funds                                                    
	SELECT Items AS FundID 
	FROM dbo.Split(@Local_FundIds,',')              
 END
      
               
CREATE TABLE #SecurityMasterTemp                
(                
	TickerSymbol	VARCHAR(100),                
	Multiplier		FLOAT,      
	LeadCurrencyId	INT,
	VsCurrencyId	INT  
)               
               
              
CREATE TABLE #PM_Taxlots              
(            
	Symbol							VARCHAR(100),              
	OrderSideTagValue				CHAR(1),              
	AvgPrice						FLOAT,              
	FundID							INT,              
	Level2ID						INT,              
	ClosedTotalCommissionandFees	FLOAT,              
	PositionTag						INT,              
	TaxLotClosingId_Fk				UNIQUEIDENTIFIER,              
	TaxlotID						VARCHAR(50),              
	GroupID							VARCHAR(50)              
)              

--- Cash Transaction Source: 7: Closing, 10: Unwinding    
CREATE TABLE #TempFKID 
(
	FKID	VARCHAR(50)
)

INSERT INTO #TempFKID 
SELECT FKID
FROM T_AllActivity
WHERE TransactionSource IN (7,10)
	AND datediff(dd, TradeDate, @Local_FromDate) <= 0
	AND datediff(dd, TradeDate, @Local_ToDate) >= 0	          
              
INSERT INTO #PM_Taxlots              
SELECT           
	PM.Symbol,              
	PM.OrderSideTagValue ,              
	PM.AvgPrice ,              
	PM.FundID ,              
	PM.Level2ID ,              
	PM.ClosedTotalCommissionandFees ,              
	PM.PositionTag ,              
	PM.TaxLotClosingId_Fk,              
	PM.TaxlotID,              
	PM.GroupID               
FROM PM_Taxlots PM
WHERE TaxLotID NOT IN 
	(
		SELECT FKID
		FROM #TempFKID		
	)

-- Keep SM data in a temp table
INSERT INTO #SecurityMasterTemp                            
SELECT	SM.TickerSymbol, SM.Multiplier, SM.LeadCurrencyID, SM.VsCurrencyID 
FROM	V_SecMasterData SM
INNER JOIN #PM_Taxlots PT ON PT.Symbol = SM.TickerSymbol 
      
CREATE TABLE #Temp_Positions            
(      
	PositionalTaxlotID			VARCHAR(50),                                                    
	ClosingTaxlotID				VARCHAR(50),                                                            
	Symbol						VARCHAR(100),                                                           
	PositionSideID				NCHAR(10),                                                            
	ClosingSideID				NCHAR(10),                                                            
	PositionTradeDate			DATETIME,                                                            
	ClosingTradeDate			DATETIME,                                                          
	OpenPrice					FLOAT,                                                            
	ClosingPrice				FLOAT,                                                            
	FundID						INT,                                    
	Level2ID					INT,                                                          
	AssetID						INT,                                                          
	UnderLyingID				INT,                                                          
	ExchangeID					INT,                                                          
	CurrencyID					INT ,                                                          
	PositionalTaxlotCommission	FLOAT,                                                            
	ClosingTaxlotCommission		FLOAT,                                                            
	ClosingMode					INT,                                                           
	Multiplier					FLOAT,                                                          
	OpeiningPositionTag			INT ,                                                          
	ClosingPositionTag			INT,                                                  
	ClosedQty					FLOAT,                      
	PositionNotionalValue		FLOAT,                                                              
	PositionBenchMarkRate		FLOAT,                                                                  
	PositionDifferential		FLOAT,                                                                      
	PositionOrigCostBasis		FLOAT,                                                                          
	PositionDayCount			INT,                                             
	PositionalSwapDescription	VARCHAR(500),                    
	PositionFirstResetDate		DATETIME,                                                                      
	PositionOrigTransDate		DATETIME,                                                             
	NotionalValue				FLOAT,                  
	BenchMarkRate				FLOAT,                                                                      
	[Differential]				FLOAT,                                                                      
	OrigCostBasis				FLOAT,                                                                            
	DayCount					INT,                                                                      
	ClosingSwapDescription		VARCHAR(500),                                                    
	FirstResetDate				DATETIME,                                                                      
	OrigTransDate				DATETIME ,                                                          
	IsSwapped					BIT,                            
	ClosingIsSwapped			BIT,                                                      
	TaxLotClosingId_Fk			UNIQUEIDENTIFIER,                                      
	PositionSide				NCHAR(20),              
	ClosingAlgo					INT ,      
	LeadCurrencyId				INT,
	VsCurrencyId				INT  
)                 
      
      
INSERT INTO #Temp_Positions          
SELECT	DISTINCT                                                           
	PTC.PositionalTaxlotID,                                                    
	PTC.ClosingTaxlotID,                                                            
	PT.Symbol AS Symbol,                                                           
	PT.OrderSideTagValue AS PositionSideID,                                                            
	PT1.OrderSideTagValue AS ClosingSideID,                                                            
	G.ProcessDate AS PositionTradeDate,                                                            
	PTC.AUECLocalDate AS ClosingTradeDate, --now closing taxlot Trade date is cloisng date                                                            
	PTC.OpenPrice AS OpenPrice ,                                                            
	PTC.ClosePrice AS ClosingPrice ,                                                            
	PT.FundID AS FundID,                                    
	PT.Level2ID AS Level2ID,                                                          
	G.AssetID,                                                          
	G.UnderLyingID,                                                          
	G.ExchangeID,                                                          
	G.CurrencyID,                                                          
	PT.ClosedTotalCommissionandFees AS PositionalTaxlotCommission,                                                            
	PT1.ClosedTotalCommissionandFees AS ClosingTaxlotCommission,                                                            
	PTC.ClosingMode AS ClosingMode,                                                           
	SM.Multiplier AS  Multiplier,                                                          
	PT.PositionTag AS OpeiningPositionTag,                                                          
	PT1.PositionTag AS ClosingPositionTag,                                                  
	PTC.ClosedQty  ,                      
	CASE G.CumQty 
		WHEN 0	THEN 0
		ELSE 
		ISNULL(SW.NotionalValue*((PTC.ClosedQty)/G.CumQty) ,0) 
	END AS PositionNotionalValue,                                                              
	ISNULL(SW.BenchMarkRate,0) AS PositionBenchMarkRate,                                                                  
	ISNULL(SW.Differential,0) AS PositionDifferential,                                                                      
	ISNULL(SW.OrigCostBasis,0) AS PositionOrigCostBasis,                                                                          
	ISNULL(SW.DayCount,0) AS PositionDayCount,                                             
	SW.SwapDescription AS PositionalSwapDescription,                                                    
	SW.FirstResetDate AS PositionFirstResetDate,                                                                      
	SW.OrigTransDate AS PositionOrigTransDate,                                                             
	CASE G.CumQty 
		WHEN 0	THEN 0
		ELSE 
		ISNULL(SW1.NotionalValue*((PTC.ClosedQty)/G1.CumQty),0) 
	END AS NotionalValue,                                                                      
	ISNULL(SW1.BenchMarkRate,0) AS BenchMarkRate,                                                                      
	ISNULL(SW1.[Differential],0) AS [Differential],                                                                      
	ISNULL(SW1.OrigCostBasis,0) AS OrigCostBasis,                                                                            
	ISNULL(SW1.DayCount,0) AS DayCount,                                                                      
	SW1.SwapDescription AS ClosingSwapDescription,                                                    
	SW1.FirstResetDate AS FirstResetDate,                                                                      
	SW1.OrigTransDate AS OrigTransDate ,                                                          
	G.IsSwapped,                                                          
	G1.IsSwapped ,                                                      
	PT.TaxLotClosingId_Fk ,                                      
	PTC.PositionSide ,                
	PTC.ClosingAlgo, 
	SM.LeadCurrencyId,
	SM.VsCurrencyId                                    
FROM PM_TaxlotClosing  PTC                                                            
INNER JOIN #PM_Taxlots PT ON ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                       
INNER JOIN #PM_Taxlots PT1 ON (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk) 
INNER JOIN #Funds funds ON PT.FundId=funds.FundId                                                         
INNER JOIN T_Group G ON G.GroupID = PT.GroupID                                                                    
INNER JOIN T_Group G1 ON G1.GroupID = PT1.GroupID                                                           
INNER JOIN T_AUEC AUEC ON AUEC.AUECID=G.AUECID                                          
LEFT OUTER JOIN #SecurityMasterTemp SM ON PT.Symbol = SM.TickerSymbol                                                                                                                       
LEFT OUTER JOIN  T_SwapParameters SW ON SW.GroupID=G.GroupID                                                            
LEFT OUTER JOIN  T_SwapParameters SW1 ON SW1.GroupID=G1.GroupID 
WHERE	DATEDIFF(d,@Local_FromDate,PTC.AUECLocalDate) >= 0          
		AND DATEDIFF(d,PTC.AUECLocalDate,@Local_ToDate) >= 0   
		AND G.AssetID IN (5,11) 
		--5: FX, 11: FXForward 
              
SELECT * FROM #Temp_Positions      
        
DROP TABLE #SecurityMasterTemp,#PM_Taxlots,#Temp_Positions,#Funds,#TempFKID                      
                                              
END