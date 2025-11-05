          
CREATE PROCEDURE [dbo].[PMGetAllSymbolsDelta] (                                        
  @FromDate DateTime,                                  
  @ToDate DateTime,                                  
  @Type int, -- 0 for Same Date,1 for Week , 2 for Month                                        
  @ErrorMessage varchar(500) output,                                        
  @ErrorNumber int output                                        
 )                                        
AS       
--declare @FromDate datetime    
--declare @ToDate datetime    
--declare @Type int    
--declare @ErrorMessage varchar(500)    
--declare @ErrorNumber int    
--Set @FromDate = '08-08-2012'    
--Set @ToDate = '08-08-2012'    
--Set @Type = 0                                      
                                  
DECLARE @FirstDateofMonth varchar(50)                                  
DECLARE @LastDateofMonth varchar(50)                                  
                                  
If(@Type=0) -- Daily view                        
Begin                                  
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                   
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                   
end                                  
Else If(@Type=1) -- Weekly view                        
Begin                                  
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                   
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)                                   
End                                  
Else If(@Type=2) -- Monthly view                        
Begin                                 
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)                        
 Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                             
End                                  
                                        
SET @ErrorMessage = 'Success'                                        
SET @ErrorNumber = 0                                        
                                        
BEGIN TRY                                        
                                
                                   
 DECLARE @Dates varchar(2000)                                    
 SET @Dates = ''                                    
 SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                                    
     FROM (select Top 35 AllDates.Items                                       
  from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items desc) MarkDate                                    
   SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)                                    
    
 --SELECT @Dates                                  
        
    CREATE TABLE #TempTaxLotId (Taxlot_PK BIGINT)

	INSERT INTO #TempTaxLotId
	SELECT max(Taxlot_PK)
	FROM PM_Taxlots
	WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) >= 0
	GROUP BY taxlotid

	CREATE TABLE #PM_Taxlots (
		Symbol VARCHAR(100)
		,AssetID INT
		,AUECID INT
		)

	INSERT INTO #PM_Taxlots (
		Symbol
		,AssetID
		,AUECID
		)
	SELECT T_GROUP.Symbol
		,T_GROUP.AssetID
		,T_GROUP.AUECID
	FROM PM_Taxlots
	INNER JOIN #TempTaxLotId TaxLot ON PM_Taxlots.TaxLot_PK = TaxLot.Taxlot_PK
	INNER JOIN T_GROUP ON T_GROUP.GroupID = PM_Taxlots.GroupId
	WHERE TaxLotOpenQty <> 0

	CREATE TABLE #T_AUEC (
		AuecID INT
		,ExchangeIdentifier VARCHAR(100)
		)

	INSERT INTO #T_AUEC (
		AuecID
		,ExchangeIdentifier
		)
	SELECT AuecID
		,ExchangeIdentifier
	FROM T_AUEC

	CREATE TABLE #SecMasterData (
		TickerSymbol VARCHAR(100)
		,UnderlyingSymbol VARCHAR(100)
		,AssetID INT
		,BloombergSymbol VARCHAR(100)
		)

	INSERT INTO #SecMasterData
	SELECT TickerSymbol
		,UnderlyingSymbol
		,AssetID
		,BloombergSymbol
	FROM V_SecMasterData

	-- Get Unallocate Symbol --  
	CREATE TABLE #TempUnallocateSymbol (
		Symbol VARCHAR(200)
		,AUECID INT
		,AssetID INT
		)

	INSERT INTO #TempUnallocateSymbol
	SELECT DISTINCT Symbol
		,AUECID
		,AssetID
	FROM T_Group
	WHERE (
			DateDiff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
			AND T_Group.StateID = 1
			AND T_Group.CumQty <> 0
			)

	CREATE TABLE [dbo].#TempUniqueSymbol (
		Symbol VARCHAR(200)
		,AUECID INT
		,AUECIdentifier NVARCHAR (50)
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		)
	SELECT DISTINCT GP.Symbol AS Symbol
		,MAX(GP.AUECID) AS AUECID
		,MAX(AUEC.ExchangeIdentifier)
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM #TempUnallocateSymbol GP
	LEFT JOIN #T_AUEC AUEC ON GP.AUECID = AUEC.AUECID
	LEFT JOIN #SecMasterData SM ON GP.Symbol = SM.TickerSymbol
	GROUP BY Symbol

	-- collect open trade symbols                                            
	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		)
	SELECT DISTINCT PT.Symbol
		,MAX(PT.AUECID) AS AUEDID		
		,MAX(AUEC.ExchangeIdentifier)
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM #PM_Taxlots PT
	LEFT JOIN #SecMasterData SM ON PT.Symbol = SM.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON PT.AUECID = AUEC.AUECID
	GROUP BY PT.Symbol

	CREATE TABLE [dbo].#TempUniqueSymbolFinal (
		Symbol VARCHAR(200)
		,AUECID INT
		,AUECIdentifier NVARCHAR (50)
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO [dbo].#TempUniqueSymbolFinal (
		Symbol
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		)
	SELECT DISTINCT SYMBOL
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.AUECIdentifier)
		,MAX(TPAATA.BloombergSymbol)
	FROM #TempUniqueSymbol TPAATA
	GROUP BY Symbol

	EXEC (
			'select *                                  
	from (Select DISTINCT UniqueSymbol.Symbol,Date,Delta,UniqueSymbol.AUECID,UniqueSymbol.AUECIdentifier,sec.BloombergSymbol  
	from #TempUniqueSymbolFinal UniqueSymbol 
	left outer join  PM_DailyDelta on PM_DailyDelta.Symbol=UniqueSymbol.Symbol  
	Left Outer Join #SecMasterData as sec on sec.TickerSymbol = UniqueSymbol.Symbol )                  
	AS DDE PIVOT (MAX(Delta) FOR Date IN (' + @Dates + ')) AS pvt ; '
				)
                               
	DROP TABLE #TempTaxLotId
	DROP TABLE #PM_Taxlots
	DROP TABLE #SecMasterData
	DROP TABLE #T_AUEC
	DROP TABLE #TempUnallocateSymbol
	DROP TABLE #TempUniqueSymbol
	DROP TABLE #TempUniqueSymbolFinal                                             
                               
END TRY                                        
BEGIN CATCH                                         
 SET @ErrorMessage = ERROR_MESSAGE();                   
 SET @ErrorNumber = Error_number();                                         
END CATCH;           
          


