--Modified By : Bharat Kumar Jangir (21 February 2014)        
--Save mark prices and forward Points for Fx and FxForwards on saving of FX Conversion        
/****************************************************************************                                    
Name :   PMSaveForexRateNew                                   
Date Created: 03-Jan-2008                                     
Purpose:  Save Forex Rate DateWise.                       
Module: MarkPriceAndForexConversion/PM                               
Author: Sandeep Singh                        
                      
Select * from #TempForexRate                
Select * from T_CurrencyStandardPairs                
Select * from T_CurrencyConversionRate                
Execution Statement                       
[PMSaveForexRateNew]                        
'<NewDataSet>                      
  <Table1>                      
    <FromCurrencyID>1</FromCurrencyID>                      
    <ToCurrencyID>2</ToCurrencyID>                    
    <Date>1/8/2008 12:00:00 AM</Date>                      
    <ConversionFactor>7.8031</ConversionFactor>                      
    <Symbol>HKD A0-FX</Symbol>                      
  </Table1>                      
  <Table1>                      
    <FromCurrencyID>1</FromCurrencyID>                      
    <ToCurrencyID>3</ToCurrencyID>                      
    <Date>1/8/2008 12:00:00 AM</Date>                      
    <ConversionFactor>109.25</ConversionFactor>                      
    <Symbol>JPY A0-FX</Symbol>                      
  </Table1>                      
  <Table1>                      
    <FromCurrencyID>1</FromCurrencyID>                      
    <ToCurrencyID>4</ToCurrencyID>                      
    <ConversionType>1</ConversionType>                      
    <Date>1/8/2008 12:00:00 AM</Date>                      
    <ConversionFactor>1.976</ConversionFactor>                      
    <Symbol>GBP A0-FX</Symbol>                      
  </Table1>                      
  <Table1>                      
    <FromCurrencyID>1</FromCurrencyID>                      
    <ToCurrencyID>5</ToCurrencyID>                        
    <Date>1/8/2008 12:00:00 AM</Date>                      
    <ConversionFactor>3.6725</ConversionFactor>                      
    <Symbol>AED A0-FX</Symbol>                      
  </Table1>                      
</NewDataSet>','',0                      
                                  
****************************************************************************/  
Create PROCEDURE [dbo].[PMSaveForexRateNew] (  
 @Xml NTEXT  
 ,@ErrorMessage VARCHAR(500) OUTPUT  
 ,@ErrorNumber INT OUTPUT  
 )  
AS  
SET @ErrorMessage = 'Success'  
SET @ErrorNumber = 0  
  
BEGIN TRANSACTION TRAN1  
  
BEGIN TRY  
 DECLARE @handle INT  
  
 EXEC sp_xml_preparedocument @handle OUTPUT  
  ,@Xml  
  
 CREATE TABLE #TempForexRate (  
  FromCurrencyID INT  
  ,ToCurrencyID INT  
  ,Date DATETIME  
  ,ConversionRate FLOAT  
  ,eSignalSymbol VARCHAR(100)  
  ,CurrencyPairID INT NOT NULL DEFAULT(0)  
  ,FundID INT NOT NULL DEFAULT(0)  
  )  
  
 INSERT INTO #TempForexRate (  
  FromCurrencyID  
  ,ToCurrencyID  
  ,Date  
  ,ConversionRate  
  ,eSignalSymbol  
  ,FundID  
  )  
 SELECT FromCurrencyID  
  ,ToCurrencyID  
  ,Date  
  ,ConversionFactor  
  ,Symbol  
  ,AccountID  
 FROM OPENXML(@handle, '//Table1', 2) WITH (  
   FromCurrencyID INT  
   ,ToCurrencyID INT  
   ,Date DATETIME  
   ,ConversionFactor FLOAT  
   ,Symbol VARCHAR(100)  
   ,AccountID INT  
   )  

	DECLARE @StartDate DATETIME

	SET @StartDate = (
			SELECT Min(DATE)
			FROM #TempForexRate
			)

	DECLARE @EndDate DATETIME

	SET @EndDate = (
			SELECT Max(DATE)
			FROM #TempForexRate
			)

	UPDATE TFR
	SET TFR.CurrencyPairID = CSP.CurrencyPairID
	FROM #TempForexRate TFR
	INNER JOIN T_CurrencyStandardPairs CSP ON TFR.FromCurrencyID = CSP.FromCurrencyID
		AND TFR.ToCurrencyID = CSP.ToCurrencyID

	UPDATE CSP
	SET CSP.eSignalSymbol = TFR.eSignalSymbol
	FROM T_CurrencyStandardPairs CSP
	INNER JOIN #TempForexRate TFR ON CSP.CurrencyPairID = TFR.CurrencyPairID

	INSERT INTO T_CurrencyStandardPairs (
		FromCurrencyID
		,ToCurrencyID
		,eSignalSymbol
		)
	SELECT DISTINCT #TempForexRate.FromCurrencyID
		,#TempForexRate.ToCurrencyID
		,#TempForexRate.eSignalSymbol
	FROM #TempForexRate
	WHERE #TempForexRate.CurrencyPairID = 0

	UPDATE TFR
	SET TFR.CurrencyPairID = CSP.CurrencyPairID
	FROM #TempForexRate TFR
	INNER JOIN T_CurrencyStandardPairs CSP ON TFR.FromCurrencyID = CSP.FromCurrencyID
		AND TFR.ToCurrencyID = CSP.ToCurrencyID

	DELETE T_CurrencyConversionRate
	FROM #TempForexRate TFXR
	INNER JOIN T_CurrencyConversionRate CCR ON CCR.CurrencyPairID_FK = TFXR.CurrencyPairID
	WHERE DateDiff(d, CCR.DATE, TFXR.DATE) = 0
		AND CCR.FundID = TFXR.FundID

	INSERT INTO T_CurrencyConversionRate (
		CurrencyPairID_FK
		,ConversionRate
		,DATE
		,FundID
		)
	SELECT #TempForexRate.CurrencyPairID
		,#TempForexRate.ConversionRate
		,#TempForexRate.DATE
		,#TempForexRate.FundID
	FROM #TempForexRate

	--Recalculate MarkPrice for the given date based on the forward points          
	--Fetching data for standard and inverse pairs again for the given data otherwise had to insert the data for inverse pairs           
	--into #TempForexRate which will result in duplication of logic          
	SELECT Symbol
		,ProcessDate
	INTO #T_Group
	FROM T_Group
	WHERE AssetID IN (
			5
			,11
			)
		AND StateID = 1
		AND CumQty <> 0

 
	SELECT TickerSymbol
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
	INTO #Temp_V_SecMasterData
	FROM V_SecMasterData
	WHERE AssetId IN (
			5
			,11
			)

	-- Here making a copy of FX/FWD taxlots so that we can reduce the join time.    
	SELECT Taxlot_PK
		,PM_Taxlots.Symbol
		,AUECModifiedDate
		,TaxlotId
		,TaxlotopenQty
		,FundID
	INTO #PM_Taxlots
	FROM PM_Taxlots
	INNER JOIN #Temp_V_SecMasterData SM ON PM_Taxlots.Symbol = SM.TickerSymbol
	WHERE SM.AssetId IN (
			5
			,11
			)

	SELECT #PM_Taxlots.Symbol AS Symbol  
     ,MAX(#PM_Taxlots.AUECModifiedDate) AS max_auecmodified_date  
    INTO #PM_TaxlotsMaxAuecDate  
    FROM #PM_Taxlots  
    WHERE #PM_Taxlots.Symbol NOT IN (  
      SELECT Symbol  
      FROM #T_Group  
      )
    GROUP BY Symbol  

	SELECT DISTINCT (#PM_Taxlots.Symbol)
		,PMTAM.max_auecmodified_date AS AUECModifiedDate
		,0 AS FundID
	INTO #PM_TaxlotsTemp
	FROM #PM_Taxlots
	INNER JOIN #PM_TaxlotsMaxAuecDate PMTAM ON #PM_Taxlots.Symbol = PMTAM.Symbol

	
	CREATE TABLE #FXConversionRates (
		FromCurrencyID INT
		,ToCurrencyID INT
		,RateValue FLOAT
		,ConversionMethod INT
		,DATE DATETIME
		,eSignalSymbol VARCHAR(max)
		,FundID INT
		)

		CREATE TABLE #Temp_TaxlotPK
		(Taxlot_PK bigint)

	SELECT DISTINCT [Date]
	INTO #DistinctDates
	FROM #TempForexRate;

	DECLARE @CurrentDate DATE;

	WHILE EXISTS (
			SELECT 1
			FROM #DistinctDates
			)
	BEGIN
		--loop beigns
		TRUNCATE TABLE #FXConversionRates
		
		SELECT TOP 1 @CurrentDate = [Date]
		FROM #DistinctDates
		ORDER BY [Date];

		--Check for FX SPOT  and FX Forward          
		INSERT INTO #FXConversionRates
		EXEC P_GetAllFXConversionRatesForGivenDateRange @CurrentDate
			,@CurrentDate

		UPDATE #FXConversionRates
		SET RateValue = 1.0 / RateValue
		WHERE RateValue <> 0
			AND ConversionMethod = 1

		UPDATE #FXConversionRates
		SET RateValue = 0
		WHERE RateValue IS NULL

		SELECT *
		INTO #PM_DayMarkPrice
		FROM PM_DayMarkPrice
		-- WHERE Date BETWEEN @StartDate  
		-- AND @EndDate
		WHERE DATE = @CurrentDate

		SELECT DMP.*
		INTO #PM_DayMarkPrice_DeletedBackUp
		FROM #PM_DayMarkPrice DMP
		INNER JOIN #Temp_V_SecMasterData TSM ON TSM.TickerSymbol = DMP.Symbol
		INNER JOIN #FXConversionRates FXDayRates ON (
				FXDayRates.FromCurrencyID = TSM.LeadCurrencyID
				AND FXDayRates.ToCurrencyID = TSM.VsCurrencyID
				AND DateDiff(Day, FXDayRates.DATE, DMP.DATE) = 0
				AND DMP.FundID = FXDayRates.FundID
				)

		DELETE PM_DayMarkPrice
		WHERE DayMarkPriceID IN (
				SELECT DayMarkPriceID
				FROM #PM_DayMarkPrice_DeletedBackUp
				)

		Insert INTO #Temp_TaxlotPK
		SELECT Max(Taxlot_PK) AS Taxlot_PK
		FROM #PM_Taxlots
		WHERE DateDiff(d, #PM_Taxlots.AUECModifiedDate, @CurrentDate) >= 0
		GROUP BY TaxLotID


		SELECT DISTINCT (#PM_Taxlots.Symbol)
			,#PM_Taxlots.AUECModifiedDate
			,FXDayRates.DATE
			,FXDayRates.RateValue
			,FXDayRates.FundID
		INTO #OpenSymbols
		FROM #PM_Taxlots
		INNER JOIN #Temp_V_SecMasterData TSM ON TSM.TickerSymbol = #PM_Taxlots.Symbol
		INNER JOIN #FXConversionRates FXDayRates ON (
				FXDayRates.FromCurrencyID = TSM.LeadCurrencyID
				AND FXDayRates.ToCurrencyID = TSM.VsCurrencyID
				AND FXDayRates.FundID = #PM_Taxlots.FundID
				)
		WHERE TaxLotOpenQty <> 0
			AND Taxlot_PK IN (
				SELECT Taxlot_PK
				FROM #Temp_TaxlotPK
				)

		-- Allocated trades with FX rates of FundID=0  
		INSERT INTO #OpenSymbols
		SELECT DISTINCT (#PM_Taxlots.Symbol)
			,#PM_Taxlots.AUECModifiedDate
			,FXDayRates.DATE
			,FXDayRates.RateValue
			,FXDayRates.FundID
		FROM #PM_Taxlots
		INNER JOIN #Temp_V_SecMasterData TSM ON TSM.TickerSymbol = #PM_Taxlots.Symbol
		INNER JOIN #FXConversionRates FXDayRates ON (
				FXDayRates.FromCurrencyID = TSM.LeadCurrencyID
				AND FXDayRates.ToCurrencyID = TSM.VsCurrencyID
				AND FXDayRates.FundID = 0
				)
		WHERE TaxLotOpenQty <> 0
			AND Taxlot_PK IN (
				SELECT Taxlot_PK
				FROM #Temp_TaxlotPK
				)

		INSERT INTO #OpenSymbols
		SELECT DISTINCT (G.Symbol)
			,G.ProcessDate
			,FXDayRates.DATE
			,FXDayRates.RateValue
			,FXDayRates.FundID
		FROM #T_Group G
		INNER JOIN #Temp_V_SecMasterData TSM ON TSM.TickerSymbol = G.Symbol
		INNER JOIN #FXConversionRates FXDayRates ON (
				FXDayRates.FromCurrencyID = TSM.LeadCurrencyID
				AND FXDayRates.ToCurrencyID = TSM.VsCurrencyID
				AND DateDiff(d, G.ProcessDate, FXDayRates.DATE) >= 0
				AND FXDayRates.FundID = 0
				)

		INSERT INTO #OpenSymbols
		SELECT DISTINCT (PMTT.Symbol)
			,PMTT.AUECModifiedDate
			,FXDayRates.DATE
			,FXDayRates.RateValue
			,FXDayRates.FundID
		FROM #PM_TaxlotsTemp PMTT
		INNER JOIN #Temp_V_SecMasterData TSM ON TSM.TickerSymbol = PMTT.Symbol
		INNER JOIN #FXConversionRates FXDayRates ON (
				FXDayRates.FromCurrencyID = TSM.LeadCurrencyID
				AND FXDayRates.ToCurrencyID = TSM.VsCurrencyID
				AND DateDiff(d, PMTT.AUECModifiedDate, FXDayRates.DATE) >= 0
				AND FXDayRates.FundID = 0
				)


				SELECT DISTINCT OS.DATE
			,OS.Symbol
			,0
			,0
			,IsNull(OS.RateValue, 0)
			,1
			,0
			,OS.FundID
		FROM #OpenSymbols OS
		WHERE DateDiff(d, OS.AUECModifiedDate, CONVERT(VARCHAR(25), OS.DATE, 101)) >= 0
			AND OS.Symbol NOT IN (
				SELECT Symbol
				FROM PM_DayMarkPrice
				WHERE DateDiff(Day, OS.DATE, PM_DayMarkPrice.DATE) = 0
				)
		--This SP use from Daily valuation so we assume these FX rates are retriveing form LiveFees so use PB Mark Price = 0        
		INSERT INTO PM_DayMarkPrice (
			DATE
			,Symbol
			,ApplicationMarkPrice
			,PrimeBrokerMarkPrice
			,FinalMarkPrice
			,IsActive
			,ForwardPoints
			,FundID
			)
		SELECT DISTINCT OS.DATE
			,OS.Symbol
			,0
			,0
			,IsNull(OS.RateValue, 0)
			,1
			,0
			,OS.FundID
		FROM #OpenSymbols OS
		WHERE DateDiff(d, OS.AUECModifiedDate, CONVERT(VARCHAR(25), OS.DATE, 101)) >= 0
			AND OS.Symbol NOT IN (
				SELECT Symbol
				FROM PM_DayMarkPrice
				WHERE DateDiff(Day, OS.DATE, PM_DayMarkPrice.DATE) = 0
				)

		UPDATE DMP
		SET DMP.ForwardPoints = DMP_Back.ForwardPoints
			,DMP.FinalMarkPrice = DMP.FinalMarkPrice + DMP_Back.ForwardPoints
		FROM PM_DayMarkPrice DMP
		INNER JOIN #PM_DayMarkPrice_DeletedBackUp DMP_Back ON DMP_Back.Symbol = DMP.Symbol
			AND DMP_Back.DATE = DMP.DATE
			AND DMP_Back.ForwardPoints != 0
			AND DMP.FundID = DMP_Back.FundID

		DELETE
		FROM #DistinctDates
		WHERE [Date] = @CurrentDate;

		Truncate Table #Temp_TaxlotPK

		DROP TABLE #OpenSymbols
			,#PM_DayMarkPrice
			,#PM_DayMarkPrice_DeletedBackUp
	END

	-- Cleanup
	DROP TABLE #DistinctDates;

	DROP TABLE #TempForexRate
		,#FXConversionRates
		,#Temp_V_SecMasterData
		,#T_Group
		,#PM_Taxlots
		,#PM_TaxlotsTemp
		,#PM_TaxlotsMaxAuecDate
		,#Temp_TaxlotPK

	EXEC sp_xml_removedocument @handle

	COMMIT TRANSACTION TRAN1
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();

	PRINT @errormessage

	SET @ErrorNumber = Error_number();

	ROLLBACK TRANSACTION TRAN1
END CATCH;
