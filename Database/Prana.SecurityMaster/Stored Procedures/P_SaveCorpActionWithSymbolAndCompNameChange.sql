
/*  
-- Author    : Rajat Tandon                                          
-- Modification Date : 24 March 09        
  
Modified By: Sandeep Singh  
Date: Dec 10, 2013  
Desc: UDA fields copied for new security 

Modified By: Disha Sharma
Date: 11/25/2015
Description: Added dynamic UDA when new Security is added
*/                                    
                                          
CREATE PROCEDURE [dbo].[P_SaveCorpActionWithSymbolAndCompNameChange]                                                                        
(                                                                        
 @Xml			VARCHAR(MAX),
 @ErrorMessage	VARCHAR(500) OUTPUT,
 @ErrorNumber	INT OUTPUT,
 @NewSymbol_PK	BIGINT
)                                                                        
AS                                                                    
SET @ErrorNumber = 0                                                                                                
SET @ErrorMessage = 'Success' 

BEGIN TRY                                                                                 
BEGIN TRAN TRAN1

	DECLARE @handle INT
	EXEC sp_xml_preparedocument @handle OUTPUT, @Xml

	CREATE TABLE #TempCorporateActions                                                                                                                                          
	(                                                                         
		EffectiveDate		DATETIME,
		OrigSymbol			VARCHAR(100),
		NewSymbol			VARCHAR(100),
		CorporateActionType	VARCHAR(5),
		CompanyName			VARCHAR(100),
		NewCompanyName		VARCHAR(100),
		CorpActionID		UNIQUEIDENTIFIER,
		Symbology			VARCHAR(50)
	)

	INSERT INTO #TempCorporateActions                                                                                                                                                                                           
	(                                                                                                                                                                                          
		EffectiveDate,
		OrigSymbol,
		NewSymbol,
		CorporateActionType,
		CompanyName,
		NewCompanyName,
		CorpActionID,
		Symbology
	)
	SELECT
		EffectiveDate,
		OrigSymbol,
		NewSymbol,
		CorporateActionType,
		CompanyName,
		NewCompanyName,
		CorpActionID,
		SymbologyID
	FROM OPENXML(@handle, '//CaFullTable', 2)
	WITH
	(
		EffectiveDate		DATETIME,
		OrigSymbol			VARCHAR(20),
		NewSymbol			VARCHAR(20),
		CorporateActionType	VARCHAR(5),
		CompanyName			VARCHAR(100),
		NewCompanyName		VARCHAR(100),
		CorpActionID		UNIQUEIDENTIFIER,
		SymbologyID			VARCHAR(50)
	)
	DECLARE	@corpActionID	UNIQUEIDENTIFIER
	Declare @isApplied		BIT

	SELECT	@corpActionID = CorpActionID	FROM #TempCorporateActions
	SELECT	@isApplied = IsApplied			FROM T_SMCorporateActions	WHERE CorpActionID = @corpActionID

	-- check if the same corporate action already present.
	DECLARE	@sameCACount AS INT
	SET		@sameCACount = (SELECT	COUNT(SM.CorpActionID)
							FROM	T_SMCorporateActions SM INNER JOIN #TempCorporateActions TempCA
									ON DATEPART(d,SM.EffectiveDate) = DATEPART(d,TempCA.EffectiveDate)      -- Check for the date part only or full date.
									AND SM.Symbol = TempCA.OrigSymbol
									AND SM.CorporateActionType = TempCA.CorporateActionType )

	--PRINT @sameCACount
	--
	--Declare @IsRedo bit
	--set @IsRedo = 0
	--
	--Declare @NewSymbol_PK bigint
	--set @NewSymbol_PK = 90330123214

	IF(@sameCACount > 0 AND @isApplied <> 0)
	-- It means that Applying single CA but CA already present so we need to throw exception
	BEGIN
	RAISERROR('Unable to save the corporate action as the corporate action already present. Please check the saveed corporate actions on the undo tab.', 16, 1)
	END

	-- It has to go into sm_Ca table anyway no matter if it is a company name change or just symbol name change or both.                          
	SET	@isApplied = 1
	EXEC P_SaveCorpActionInfo  @Xml, @isApplied,'',0

	DECLARE @newCompanyName	VARCHAR(100)
	DECLARE @newSymbol		VARCHAR(100)

	SELECT @newSymbol = NewSymbol, @newCompanyName = NewCompanyName FROM #TempCorporateActions

	-- If symbol changed then we need to insert into all tables related to symbol
	IF ((@newSymbol IS NOT NULL) AND (@newSymbol <> ''))
	BEGIN 
		IF NOT EXISTS(SELECT * FROM T_SMSymbolLookupTable WHERE TickerSymbol = @newSymbol)
		BEGIN
			--if there are no corporate actions already present in the table perform operations
			DECLARE	@symbolLookupStr VARCHAR(MAX)
			SET		@symbolLookupStr = 'INSERT INTO T_SMSymbolLookUpTable
										(
											TickerSymbol, 
											AssetID, 
											UnderLyingID, 
											AUECID,
											BloombergSymbol, 
											ISINSymbol, 
											SEDOLSymbol, 
											CUSIPSymbol, 
											ExchangeID, 
											CountryID, 
											CreationDate, 
											Symbol_PK, 
											Sector,
											UnderLyingSymbol, 
											ModifiedDate, 
											CurrencyID, 
											EffectiveDate,
											CorpActionID,
											DataSource,
											RoundLot,
											ProxySymbol,
											IsSecApproved,
											ApprovalDate,
											ApprovedBy,
											Comments,
											UDAAssetClassID,
											UDASecurityTypeID,
											UDASectorID,
											UDASubSectorID,
											UDACountryID
										)
										SELECT TOP 1 
											tempCorp.NewSymbol,
											lookup.AssetID,
											lookup.UnderLyingID,
											lookup.AUECID,
											lookup.BloombergSymbol,
											lookup.ISINSymbol,
											lookup.SEDOLSymbol,
											lookup.CUSIPSymbol,
											lookup.ExchangeID,
											lookup.CountryID,
											lookup.CreationDate,
											' +  Cast(@NewSymbol_PK as varchar(100)) +',
											lookup.Sector,
											lookup.UnderLyingSymbol, 
											tempCorp.EffectiveDate,
											lookup.CurrencyID,
											tempCorp.EffectiveDate, 
											tempCorp.CorpActionID,
											lookup.DataSource,
											lookup.RoundLot,
											lookup.ProxySymbol,
											lookup.IsSecApproved,
											lookup.ApprovalDate,
											lookup.ApprovedBy,
											lookup.Comments,
											lookup.UDAAssetClassID,
											lookup.UDASecurityTypeID,
											lookup.UDASectorID,
											lookup.UDASubSectorID,
											lookup.UDACountryID
										FROM		T_SMSymbolLookUpTable lookup 
										INNER JOIN	#TempCorporateActions tempCorp 
													ON lookup.TickerSymbol = tempCorp.OrigSymbol
										ORDER BY	Symbol_PK DESC' 
		
			DECLARE @actualCompanyName VARCHAR(100)
			-- If company name is also changed then save new company name else save old company name with new symbol_pk
			IF(@newCompanyName is not null and @newCompanyName <> '')
				SET @actualCompanyName = 'tempCorp.NewCompanyName'
			ELSE
				SET @actualCompanyName = 'tempCorp.CompanyName'
		
			DECLARE	@equityNonHistoryDataStr VARCHAR(MAX) 
			SET		@equityNonHistoryDataStr = 'INSERT INTO T_SMEquityNonHistoryData
												(
													CompanyName,
													ModifiedAt,
													RoundLot,
													Symbol_PK,
													EffectiveDate,
													CorpActionID,
													Delta
												)
												SELECT DISTINCT
													' + @actualCompanyName + ',
													tempCorp.EffectiveDate,
													NonHistory.RoundLot,
													'  +  Cast(@NewSymbol_PK as varchar(100)) +',
													tempCorp.EffectiveDate,
													tempCorp.CorpActionID,
													NonHistory.Delta 
												FROM		T_SMSymbolLookUpTable lookup
												INNER JOIN	#TempCorporateActions tempCorp 
															ON lookup.TickerSymbol = tempCorp.OrigSymbol
												INNER JOIN	T_SMEquityNonHistoryData NonHistory 
															ON NonHistory.Symbol_PK = lookup.Symbol_PK'

			Execute(@symbolLookupStr)
			Execute(@equityNonHistoryDataStr)

			--Updating DynamicUDAData
			SELECT	TSM.Symbol_PK, TCA.OrigSymbol
			INTO	#NewSymbols 
			FROM	T_SMSymbolLookUpTable TSM 
			INNER JOIN #TempCorporateActions TCA 
					ON TSM.TickerSymbol = TCA.NewSymbol                                             
			ORDER BY Symbol_PK DESC

			DECLARE @dynamicUDAStr VARCHAR(MAX)
			SET		@dynamicUDAStr = 'UPDATE DUDA SET '
			SELECT	@dynamicUDAStr = @dynamicUDAStr + ' DUDA.[' + Tag + '] = TDUDA.[' + Tag  + '],'
			FROM	T_UDA_DynamicUDA 
			SET		@dynamicUDAStr = SUBSTRING(@dynamicUDAStr,0,LEN(@dynamicUDAStr))
			SET		@dynamicUDAStr = @dynamicUDAStr + ' FROM T_UDA_DynamicUDAData DUDA CROSS JOIN T_UDA_DynamicUDAData TDUDA '

			-- Create new record in T_UDA_DynamicUDAData for new symbols with NULL data
			DECLARE	@Symbol_PK BIGINT
			DECLARE	@OldSymbol_PK BIGINT
			DECLARE @OriginalSymbol	VARCHAR(100)
			WHILE EXISTS(SELECT 1 FROM #NewSymbols)
			BEGIN
				SET	@Symbol_PK = (SELECT TOP 1 Symbol_PK FROM #NewSymbols)
				SET @OriginalSymbol = (SELECT TOP 1 OrigSymbol FROM #NewSymbols)
				SET @OldSymbol_PK = (SELECT Symbol_PK FROM T_SMSymbolLookUpTable WHERE UPPER(TickerSymbol) = UPPER(@OriginalSymbol))

				EXEC P_UDA_SaveDynamicUDAData
				@Symbol_PK, NULL, 0   

		
				--Update T_UDA_DynamicUDAData for new symbols
				DECLARE	@dynamicUDAUpdateStr VARCHAR(MAX)
				SET		@dynamicUDAUpdateStr = @dynamicUDAStr + ' WHERE DUDA.Symbol_PK = ' + CONVERT(VARCHAR(MAX),@Symbol_PK) + 'AND TDUDA.Symbol_PK = ' + CONVERT(VARCHAR(MAX),@OldSymbol_PK)
				EXEC (@dynamicUDAUpdateStr)

				DELETE FROM #NewSymbols WHERE Symbol_PK = @Symbol_PK
			END
		     DROP TABLE #NewSymbols
			-- Update T_SMSymbolLookUpTable
			-- Set UnderlyingSymbol = tempCorp.NewSymbol
			-- From T_SMSymbolLookUpTable lookup Inner Join #TempCorporateActions tempCorp on lookup.UnderlyingSymbol = tempCorp.OrigSymbol
			--  Inner Join T_SMOptionData optData on lookup.Symbol_PK = optData.Symbol_PK
			--  Where lookup.AssetID = 2 and DateDiff(d,tempCorp.EffectiveDate,optData.ExpirationDate) >=0-- Only for options

			DECLARE		@Reuters_PK	VARCHAR(100)
			SELECT		@Reuters_PK = Reuters.ReutersPK 
			FROM		T_SMReuters Reuters
			INNER JOIN	T_SMSymbolLookUpTable lookup
						ON lookup.Symbol_PK = Reuters.Symbol_PK
			INNER JOIN	#TempCorporateActions tempCorp 
						ON lookup.TickerSymbol = tempCorp.OrigSymbol
			WHERE		Reuters.IsPrimaryExchange = 1

			DECLARE	@reuterStr VARCHAR(MAX)
			-- Insert a new row in reuters table as primary.
			SET	@reuterStr	= 'INSERT INTO T_SMReuters
								(
									AUECID,
									ExchangeID,
									ReutersSymbol,
									IsPrimaryExchange,
									Symbol_PK
								)
								SELECT 
									Reuters.AUECID,
									Reuters.ExchangeID,
									Reuters.ReutersSymbol,
									1,
									' +  Cast(@NewSymbol_PK as varchar(100)) + ' ' +
								'FROM	T_SMReuters Reuters 
								 WHERE	ReutersPK =' + @Reuters_PK
								-- + ',' +  Cast(@corpActionID as varchar(50))
			EXECUTE(@reuterStr)

			UPDATE	T_SMReuters 
			SET		CorpActionID = @corpActionID
			WHERE	Symbol_PK = @NewSymbol_PK AND ISPrimaryExchange = 1
		END
	END
	ELSE IF((@newCompanyName IS NOT NULL) AND (@newCompanyName <> '')) -- it means only company name change
	BEGIN
		INSERT INTO	T_SMEquityNonHistoryData
		SELECT DISTINCT	
			tempCorp.NewCompanyName,
			tempCorp.EffectiveDate,
			NonHistory.RoundLot,
			NonHistory.Symbol_PK,
			tempCorp.EffectiveDate,
			tempCorp.CorpActionID,
			Delta
		FROM		T_SMSymbolLookUpTable lookup
		INNER JOIN	#TempCorporateActions tempCorp 
					ON lookup.TickerSymbol = tempCorp.OrigSymbol
		INNER JOIN	T_SMEquityNonHistoryData NonHistory 
					ON NonHistory.Symbol_PK = lookup.Symbol_PK
	END

	DROP TABLE #TempCorporateActions
COMMIT TRANSACTION TRAN1
EXEC sp_xml_removedocument @handle

END TRY
BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE()
	SET @ErrorNumber = Error_number()
	DROP TABLE #TempCorporateActions
	ROLLBACK TRANSACTION TRAN1                   
END CATCH 

