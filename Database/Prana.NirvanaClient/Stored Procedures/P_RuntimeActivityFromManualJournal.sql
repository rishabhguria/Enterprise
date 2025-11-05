
CREATE PROCEDURE [dbo].[P_RuntimeActivityFromManualJournal] (
	@xml NTEXT = NULL
	,@StartDate DATETIME = NULL
	,@EndDate DATETIME = NULL
	,@FundIDs VARCHAR(max) = NULL
	)
AS
/*                                                                  
Author: SURENDRA BISHT       
  
This Sp Is made keeping in mind that Activity should be made using key of Currency,fxrate,transactiontype                                                                  
                                                             
*/
-----------------------------making temporary #NonTradejournal---------------------------------------------------------------------------------------------  
BEGIN TRY
	DECLARE @handle INT

	SET NOCOUNT ON

	EXEC sp_xml_preparedocument @handle OUTPUT
		,@xml
	CREATE TABLE #NonTradejournal(
			[TaxLotID] [varchar](50)
			,[FundID] [int]
			,[SubAccountID] [int]
			,[CurrencyID] [int]
			,[Symbol] [varchar](100)
			,[PBDesc] [varchar](3000)
			,[TransactionDate] [datetime]
			,[TransactionID] [varchar](50)
			,[DR] [money]
			,[CR] [money]
			,[TransactionSource] [varchar](100)
			,[TransactionEntryID] [varchar](50)
			,[TransactionNumber] [int]
			,[AccountSide] [varchar](2)
			,[ActivityId_FK] [varchar](50)
			,[ActivitySource] [varchar](50)
			,[FxRate] [float]
			,[FXConversionMethodOperator] [varchar](3)
			,[ModifyDate] [datetime]
			,[EntryDate] [datetime]
			,[UserId] [int]
			)
	INSERT INTO #NonTradejournal
	SELECT TaxLotID
		,FundID
		,SubAccountID
		,CurrencyID
		,Symbol
		,PBDesc
		,TransactionDate
		,TransactionID
		,DR
		,CR
		,TransactionSource
		,TransactionEntryID
		,TransactionNumber
		,AccountSide
		,ActivityId_FK
		,ActivitySource
		,FxRate
		,FXConversionMethodOperator
		,ModifyDate
		,EntryDate
		,UserId
	FROM openXML(@handle, 'DS/Transactions', 2) WITH (
			[TaxLotID] [varchar](50)
			,[FundID] [int]
			,[SubAccountID] [int]
			,[CurrencyID] [int]
			,[Symbol] [varchar](100)
			,[PBDesc] [varchar](3000)
			,[TransactionDate] [datetime]
			,[TransactionID] [varchar](50)
			,[DR] [money]
			,[CR] [money]
			,[TransactionSource] [varchar](100)
			,[TransactionEntryID] [varchar](50)
			,[TransactionNumber] [int]
			,[AccountSide] [varchar](2)
			,[ActivityId_FK] [varchar](50)
			,[ActivitySource] [varchar](50)
			,[FxRate] [float]
			,[FXConversionMethodOperator] [varchar](3)
			,[ModifyDate] [datetime]
			,[EntryDate] [datetime]
			,[UserId] [int]
			)

	IF (@xml IS NULL)
	BEGIN
		----------------Create fund Table for fundwise journals---------------------  
		CREATE TABLE #Funds (colFundID INT)

		IF (@FundIDs IS NOT NULL)
		BEGIN
			INSERT INTO #Funds
			SELECT Items AS colFundID
			FROM dbo.Split(@FundIDs, ',')
		END
		ELSE
		BEGIN
			INSERT INTO #Funds
			SELECT CompanyFundID
			FROM T_CompanyFunds
			Where IsActive=1 
		END

		----------------------------------------------------------------------------  
		SELECT DISTINCT transactionid
		INTO #exceptions
		FROM t_journal
		INNER JOIN #Funds ON colFundID = FundID
		WHERE activityid_fk IS NULL
			AND datediff(d, transactiondate, @StartDate) <= 0
			AND datediff(d, transactiondate, @EndDate) >= 0
			AND Transactionsource NOT IN (
				9
				,4
				,5
				)

		--------------For Picking Dividend Payout Journal for the Dividends having Exdate in the given date range-------------------------------------------------------------------------------------------------  
		SELECT *
		INTO #CashDivTransactions
		FROM T_CashDivTransactions
		WHERE fkid IN (
				SELECT cashtransactionid
				FROM T_CashTransactions
				INNER JOIN #Funds ON colFundID = FundID
				WHERE (
						datediff(d, exdate, @StartDate) <= 0
						AND datediff(d, exdate, @EndDate) >= 0
						)
					OR (
						datediff(d, payoutdate, @StartDate) <= 0
						AND datediff(d, payoutdate, @EndDate) >= 0
						)
				)

		INSERT INTO #exceptions
		SELECT DISTINCT t_journal.transactionid
		FROM t_journal
		INNER JOIN #CashDivTransactions ON t_journal.transactionid = #CashDivTransactions.transactionid
		INNER JOIN T_SubAccounts subacc ON subacc.subaccountid = t_journal.subaccountid
		INNER JOIN T_Transactiontype acctype ON subacc.TransactionTypeId = acctype.TransactionTypeId
		WHERE (activityid_fk IS NULL)
			AND transactiontype = 'Cash'

		---------------------------------------------------------------------------------------------------------------  
		IF (
				(
					SELECT count(*)
					FROM #exceptions
					) = 0
				)
			RETURN

		INSERT INTO #NonTradejournal
		SELECT TaxLotID
			,FundID
			,SubAccountID
			,CurrencyID
			,Symbol
			,PBDesc
			,TransactionDate
			,TransactionID
			,DR
			,CR
			,TransactionSource
			,TransactionEntryID
			,TransactionNumber
			,AccountSide
			,ActivityId_FK
			,ActivitySource
			,FxRate
			,FXConversionMethodOperator
			,ModifyDate
			,EntryDate
			,UserId
		FROM t_journal
		WHERE transactionid IN (
				SELECT transactionid
				FROM #exceptions
				)
	END

	----------------------------------------------------------------------------------------------------------------------  
	--Tempactivity table------------                                
	SELECT *
	INTO #activity
	FROM t_allactivity
	WHERE activityid IS NULL

	ALTER TABLE #activity

	ALTER COLUMN [ActivityID] [varchar] (50) NULL

	ALTER TABLE #activity

	ALTER COLUMN [ActivityTypeId_FK] [int] NULL

	ALTER TABLE #activity

	ALTER COLUMN [FKID] VARCHAR(50) NULL

	ALTER TABLE #activity

	ALTER COLUMN [BalanceType] [int] NULL

	-- variable to know whether new activity type created.      
	DECLARE @isNewActivityTypeCreated BIT

	SET @isNewActivityTypeCreated = 0

	--activityid for activity to be created                                
	DECLARE @activityid VARCHAR(50)

    SELECT @activityid = Max(cast(isnull(round(ActivityID,0), 0) AS BIGINT))
	FROM t_allactivity

	DECLARE @LastNewActivityType INT

	SELECT @LastNewActivityType = max(Activitytypeid)
	FROM T_ActivityType

	DECLARE @activityTypeid INT -- for finding activityTypeid_fk  for each NonTradejournal entry .  
	DECLARE @descriptions VARCHAR(3000)
	DECLARE @entryDate DATETIME
	DECLARE @modifyDate DATETIME
	DECLARE @userId INT
	--row--                                
	DECLARE @row INT

	SET @row = 1

	DECLARE @totalrows INT

	SELECT @totalrows = count(DISTINCT transactionid)
	FROM #NonTradejournal

	DECLARE @FeesAdjust BIT

	SET @FeesAdjust = 0

	WHILE (@row <= @totalrows)
	BEGIN
		DECLARE @manualtransactionid VARCHAR(40)

		SELECT @manualtransactionid = transactionid
		FROM (
			SELECT transactionid
				,ROW_NUMBER() OVER (
					ORDER BY transactionid
					) AS RowIndex
			FROM #NonTradejournal
			GROUP BY transactionid
			) t1
		WHERE t1.rowindex = @row

		SELECT nontradingJournal.*
			,subacc.NAME
			,subacc.Acronym
			,acctype.TransactionType
		INTO #SingleJournal
		FROM #NonTradejournal nontradingJournal
		INNER JOIN T_SubAccounts subacc ON subacc.subaccountid = nontradingJournal.subaccountid
		INNER JOIN T_Transactiontype acctype ON subacc.TransactionTypeId = acctype.TransactionTypeId
			AND transactionid = @manualtransactionid

		DECLARE @verify INT

		--  @verify is null            -> no activity for such Non- Trade  
		--  @verify is 1               -> Activity type /mapping created if not and then activity  
		--  @verify is greater than 1  -> if activity to be made for this trade of activity Type 'UnknownActivity'  
		SELECT @verify = COUNT(*)
		FROM (
			SELECT currencyid
				,transactiontype
			FROM #SingleJournal
			WHERE transactiontype = 'Cash'
				OR transactiontype = 'Accrued Balance'
			GROUP BY currencyid
				,transactiontype
			) AS splits

		IF (@verify = 1)
			SELECT @verify = COUNT(*)
			FROM (
				SELECT currencyid
				FROM #SingleJournal
				GROUP BY currencyid
					,fxrate
				) AS splits

		DECLARE @drcount INT
		DECLARE @crcount INT

		SELECT @drcount = count(DISTINCT subaccountid)
		FROM #NonTradejournal
		WHERE transactionid = @manualtransactionid
			AND accountside = 0

		SELECT @crcount = count(DISTINCT subaccountid)
		FROM #NonTradejournal
		WHERE transactionid = @manualtransactionid
			AND accountside = 1

		---------------------------- Identify Dividend Activity (Special Handling)-----------------------------------------------------------------------------------------------------------  
		IF (@verify = 2)
		BEGIN
			DECLARE @DiffCurrency INT

			SELECT @DiffCurrency = count(DISTINCT CurrencyID)
			FROM #NonTradejournal
			WHERE transactionid = @manualtransactionid

			SELECT *
			INTO #dividendsMapping
			FROM T_ActivityJournalMapping
			WHERE ActivityDateType = 2
				AND @drcount = 1
				AND @crcount = 1

			SELECT @activityTypeid = ActivityTypeId_FK
			FROM #dividendsMapping
			WHERE ActivityTypeId_FK NOT IN (
					(
						SELECT ActivityTypeId_FK
						FROM #dividendsMapping
						WHERE debitaccount NOT IN (
								SELECT subaccountid
								FROM #NonTradejournal
								WHERE transactionid = @manualtransactionid
									AND accountside = 0
								)
						)
					
					UNION
					
					(
						SELECT ActivityTypeId_FK
						FROM #dividendsMapping
						WHERE creditaccount NOT IN (
								SELECT subaccountid
								FROM #NonTradejournal
								WHERE transactionid = @manualtransactionid
									AND accountside = 1
								)
						)
					)

			IF (
					@activityTypeid IS NOT NULL
					AND @DiffCurrency = 1
					)
				SET @verify = 1

			DROP TABLE #dividendsMapping
		END

		------------------------START OF SEARCHING EXISTING ACTIVITYTYPE-------------------------------------------------------------  
		IF (@verify = 1)
		BEGIN
			---this is to identify unique transactiontype in #SingleJournal----  
			DECLARE @transactiontype VARCHAR(40)

			SET @transactiontype = (
					SELECT TOP 1 transactiontype
					FROM #SingleJournal
					WHERE transactiontype = 'Cash'
						OR transactiontype = 'Accrued Balance'
					)

			IF (@activityTypeid IS NULL)
			BEGIN
				--all mapping having same no of drs and crs as of manual entry will be saved into #tempmapping                                       
				SELECT m.*
				INTO #tempmapping
				FROM (
					SELECT activitytypeid_fk
						,count(DISTINCT debitaccount) AS d
						,count(DISTINCT creditaccount) AS c
					FROM t_activityjournalmapping
					WHERE ActivityTypeid_fk NOT IN (
							SELECT ActivityTypeId_FK
							FROM T_ActivityJournalMapping
							WHERE ActivityDateType = 2
							)
					GROUP BY activitytypeid_fk
					HAVING (
							count(DISTINCT debitaccount) = @drcount
							AND count(DISTINCT creditaccount) = @crcount
							)
					) AS abc
				INNER JOIN t_activityjournalmapping m ON abc.activitytypeid_fk = m.activitytypeid_fk

				SELECT @activityTypeid = ActivityTypeId_FK
				FROM #tempmapping
				WHERE ActivityTypeId_FK NOT IN (
						(
							SELECT ActivityTypeId_FK
							FROM #tempmapping
							WHERE debitaccount NOT IN (
									SELECT subaccountid
									FROM #NonTradejournal
									WHERE transactionid = @manualtransactionid
										AND accountside = 0
									)
							)
						
						UNION
						
						(
							SELECT ActivityTypeId_FK
							FROM #tempmapping
							WHERE creditaccount NOT IN (
									SELECT subaccountid
									FROM #NonTradejournal
									WHERE transactionid = @manualtransactionid
										AND accountside = 1
									)
							)
						)

				IF (@activityTypeid IS NULL)
				BEGIN
					SELECT @activityTypeid = ActivityTypeId_FK
					FROM #tempmapping
					WHERE ActivityTypeId_FK NOT IN (
							(
								SELECT ActivityTypeId_FK
								FROM #tempmapping
								WHERE creditaccount NOT IN (
										SELECT subaccountid
										FROM #NonTradejournal
										WHERE transactionid = @manualtransactionid
											AND accountside = 0
										)
								)
							
							UNION
							
							(
								SELECT ActivityTypeId_FK
								FROM #tempmapping
								WHERE debitaccount NOT IN (
										SELECT subaccountid
										FROM #NonTradejournal
										WHERE transactionid = @manualtransactionid
											AND accountside = 1
										)
								)
							)
				END

				DROP TABLE #tempmapping
			END

			--Handling  THE CASE when trading journal not including any or all of the fees ,then need different way to map this journal to Activity---------------  
			IF (@activityTypeid IS NULL)
			BEGIN
				IF EXISTS (
						SELECT *
						FROM #NonTradejournal
						WHERE transactionid = @manualtransactionid
							AND accountside = 0
							AND subaccountid = 17
							AND transactionsource = 1
						)
				BEGIN
					IF EXISTS (
							SELECT *
							FROM #NonTradejournal
							WHERE transactionid = @manualtransactionid
								AND accountside = 1
								AND subaccountid = 17
							)
					BEGIN
						SELECT @crcount = count(DISTINCT subaccountid)
						FROM #NonTradejournal
						WHERE transactionid = @manualtransactionid
							AND accountside = 1
							AND subaccountid NOT IN (
								SELECT subaccountid
								FROM T_subaccounts
								WHERE acronym IN (
										'Stamp_Duty'
										,'Clearing_Fee'
										,'Misc_Fees'
										,'Tax_On_Commissions'
										,'Transaction_Levy'
										,'Other_Broker_Fees'
										,'Commission'
										,'Sec_Fee'
										,'Occ_Fee'
										,'Orf_Fee'
										,'Clearing_Broker_Fee'
										,'Soft_Commission'
										,'cash'
										)
								)

						SELECT @drcount = count(DISTINCT subaccountid)
						FROM #NonTradejournal
						WHERE transactionid = @manualtransactionid
							AND accountside = 0
							AND subaccountid NOT IN (
								SELECT subaccountid
								FROM T_subaccounts
								WHERE acronym IN (
										'Stamp_Duty'
										,'Clearing_Fee'
										,'Misc_Fees'
										,'Tax_On_Commissions'
										,'Transaction_Levy'
										,'Other_Broker_Fees'
										,'Commission'
										,'Sec_Fee'
										,'Occ_Fee'
										,'Orf_Fee'
										,'Clearing_Broker_Fee'
										,'Soft_Commission'
										,'cash'
										)
								)

						IF (@crcount = 1)
						BEGIN
							DELETE
							FROM #NonTradejournal
							WHERE transactionid = @manualtransactionid
								AND accountside = 1
								AND subaccountid = 17

							SET @FeesAdjust = 1
						END

						IF (@drcount = 1)
						BEGIN
							DELETE
							FROM #NonTradejournal
							WHERE transactionid = @manualtransactionid
								AND accountside = 0
								AND subaccountid = 17

							SET @FeesAdjust = 1
						END
					END
				END

				SELECT @drcount = count(DISTINCT subaccountid)
				FROM #NonTradejournal
				WHERE transactionid = @manualtransactionid
					AND accountside = 0
					AND subaccountid NOT IN (
						SELECT subaccountid
						FROM T_subaccounts
						WHERE acronym IN (
								'Stamp_Duty'
								,'Clearing_Fee'
								,'Misc_Fees'
								,'Tax_On_Commissions'
								,'Transaction_Levy'
								,'Other_Broker_Fees'
								,'Commission'
								,'Sec_Fee'
								,'Occ_Fee'
								,'Orf_Fee'
								,'Clearing_Broker_Fee'
								,'Soft_Commission'
								)
						)

				SELECT @crcount = count(DISTINCT subaccountid)
				FROM #NonTradejournal
				WHERE transactionid = @manualtransactionid
					AND accountside = 1
					AND subaccountid NOT IN (
						SELECT subaccountid
						FROM T_subaccounts
						WHERE acronym IN (
								'Stamp_Duty'
								,'Clearing_Fee'
								,'Misc_Fees'
								,'Tax_On_Commissions'
								,'Transaction_Levy'
								,'Other_Broker_Fees'
								,'Commission'
								,'Sec_Fee'
								,'Occ_Fee'
								,'Orf_Fee'
								,'Clearing_Broker_Fee'
								,'Soft_Commission'
								)
						)

				--all mapping having same no of drs and crs as of manual entry will be saved into #tempmapping    
				SELECT m.*
				INTO #tempmapping2
				FROM (
					SELECT activitytypeid_fk
						,count(DISTINCT debitaccount) AS d
						,count(DISTINCT creditaccount) AS c
					FROM t_activityjournalmapping
					WHERE AmountTypeId_FK IN (
							SELECT AmountTypeId
							FROM t_ActivityAmountType
							WHERE AmountType = 'Amount'
								OR AmountType = 'ClosedQty'
							)
						AND ActivityTypeid_fk NOT IN (
							SELECT ActivityTypeId_FK
							FROM T_ActivityJournalMapping
							WHERE ActivityDateType = 2
							)
					GROUP BY activitytypeid_fk
					HAVING (
							count(DISTINCT debitaccount) = @drcount
							AND count(DISTINCT creditaccount) = @crcount
							)
					) AS abc
				INNER JOIN t_activityjournalmapping m ON abc.activitytypeid_fk = m.activitytypeid_fk
				WHERE m.AmountTypeId_FK IN (
						SELECT AmountTypeId
						FROM t_ActivityAmountType
						WHERE AmountType = 'Amount'
							OR AmountType = 'ClosedQty'
						)

				SELECT @activityTypeid = ActivityTypeId_FK
				FROM #tempmapping2
				WHERE ActivityTypeId_FK NOT IN (
						(
							SELECT ActivityTypeId_FK
							FROM #tempmapping2
							WHERE debitaccount NOT IN (
									SELECT subaccountid
									FROM #NonTradejournal
									WHERE transactionid = @manualtransactionid
										AND accountside = 0
										AND subaccountid NOT IN (
											SELECT subaccountid
											FROM T_subaccounts
											WHERE acronym IN (
													'Stamp_Duty'
													,'Clearing_Fee'
													,'Misc_Fees'
													,'Tax_On_Commissions'
													,'Transaction_Levy'
													,'Other_Broker_Fees'
													,'Commission'
													,'Sec_Fee'
													,'Occ_Fee'
													,'Orf_Fee'
													,'Clearing_Broker_Fee'
													,'Soft_Commission'
													)
											)
									)
							)
						
						UNION
						
						(
							SELECT ActivityTypeId_FK
							FROM #tempmapping2
							WHERE creditaccount NOT IN (
									SELECT subaccountid
									FROM #NonTradejournal
									WHERE transactionid = @manualtransactionid
										AND accountside = 1
										AND subaccountid NOT IN (
											SELECT subaccountid
											FROM T_subaccounts
											WHERE acronym IN (
													'Stamp_Duty'
													,'Clearing_Fee'
													,'Misc_Fees'
													,'Tax_On_Commissions'
													,'Transaction_Levy'
													,'Other_Broker_Fees'
													,'Commission'
													,'Sec_Fee'
													,'Occ_Fee'
													,'Orf_Fee'
													,'Clearing_Broker_Fee'
													,'Soft_Commission'
													)
											)
									)
							)
						)

				IF (@activityTypeid IS NULL)
				BEGIN
					SELECT @activityTypeid = ActivityTypeId_FK
					FROM #tempmapping2
					WHERE ActivityTypeId_FK NOT IN (
							(
								SELECT ActivityTypeId_FK
								FROM #tempmapping2
								WHERE creditaccount NOT IN (
										SELECT subaccountid
										FROM #NonTradejournal
										WHERE transactionid = @manualtransactionid
											AND accountside = 0
											AND subaccountid NOT IN (
												SELECT subaccountid
												FROM T_subaccounts
												WHERE acronym IN (
														'Stamp_Duty'
														,'Clearing_Fee'
														,'Misc_Fees'
														,'Tax_On_Commissions'
														,'Transaction_Levy'
														,'Other_Broker_Fees'
														,'Commission'
														,'Sec_Fee'
														,'Occ_Fee'
														,'Orf_Fee'
														,'Clearing_Broker_Fee'
														,'Soft_Commission'
														)
												)
										)
								)
							
							UNION
							
							(
								SELECT ActivityTypeId_FK
								FROM #tempmapping2
								WHERE debitaccount NOT IN (
										SELECT subaccountid
										FROM #NonTradejournal
										WHERE transactionid = @manualtransactionid
											AND accountside = 1
											AND subaccountid NOT IN (
												SELECT subaccountid
												FROM T_subaccounts
												WHERE acronym IN (
														'Stamp_Duty'
														,'Clearing_Fee'
														,'Misc_Fees'
														,'Tax_On_Commissions'
														,'Transaction_Levy'
														,'Other_Broker_Fees'
														,'Commission'
														,'Sec_Fee'
														,'Occ_Fee'
														,'Orf_Fee'
														,'Clearing_Broker_Fee'
														,'Soft_Commission'
														)
												)
										)
								)
							)
				END

				DROP TABLE #tempmapping2
			END

			--End of Special Handling -------------------        
			IF (@activityTypeid IS NOT NULL)
			BEGIN
				SET @transactiontype = CASE 
						WHEN (
								SELECT BalanceType
								FROM T_ActivityType
								WHERE ActivityTypeId = @activityTypeid
								) = 1
							THEN 'Cash'
						ELSE 'Accrued Balance'
						END
			END

			------------------------END OF SEARCHING EXISTING ACTIVITYTYPE------------------------------------------------------------------------------------------------------------     
			--- now either using existing found ActivityId mapping  or creating new mapping                                                
			-- select @activityTypeid   -- fetched data view            
			IF (@activityTypeid IS NULL)
			BEGIN
				-- create new activityid in activitytype                                
				DECLARE @no VARCHAR(20)

				SET @no = 1

				--finding last unknown activity                            
				DECLARE @activityno INT

				SELECT @activityno = max(cast(replace(activitytype, 'unknown', '') AS INT))
				FROM t_activitytype
				WHERE activitytype LIKE 'unknown%'
					AND isnumeric(replace(activitytype, 'unknown', '')) = 1

				IF (@activityno IS NOT NULL)
					SELECT @no = @activityno + 1

				INSERT INTO t_activitytype (
					activityType
					,acronym
					,BalanceType
					)
				VALUES (
					'unknown' + @no
					,'unknown' + @no
					,CASE 
						WHEN @transactiontype LIKE 'Cash'
							THEN 1
						ELSE 2
						END
					)

				--Now this is the NEW Activitytypeid for new activity                                
				SELECT @activityTypeid = ActivityTypeId
				FROM t_activitytype
				WHERE activityType = 'unknown' + @no

				-- now  creatinmg mapping T_activityjournalmapping                                
				CREATE TABLE #mapping (
					[ActivityTypeId_FK] INT
					,[AmountTypeId_FK] INT
					,[DebitAccount] INT
					,[CreditAccount] INT
					,[CashValueType] BIT --[ActivityDateType]                                
					)

				-- inserting drs   and cashvaluetype                    
				INSERT INTO #mapping (
					[DebitAccount]
					,[CashValueType]
					)
				SELECT DISTINCT subaccountid
					,0
				FROM #NonTradejournal
				WHERE accountside = 0
					AND transactionid = @manualtransactionid

				-- inserting crs  and cashvaluetype                               
				INSERT INTO #mapping (
					[CreditAccount]
					,[CashValueType]
					)
				SELECT DISTINCT subaccountid
					,0
				FROM #NonTradejournal
				WHERE accountside = 1
					AND transactionid = @manualtransactionid

				-- updating the  ActivityTypeId_FK                          
				UPDATE #mapping
				SET [ActivityTypeId_FK] = @activityTypeid

				-- updating the amountypeId_fk accordingly for the default debit subaccount                                
				UPDATE #mapping
				SET [AmountTypeId_FK] = amt.AmountTypeId
				FROM #mapping m
				INNER JOIN t_subaccounts acc ON m.debitaccount = acc.subaccountid
				INNER JOIN t_activityamounttype amt ON amt.defaultSubaccountAcronym = acc.Acronym

				-- updating the amountypeId_fk accordingly for the default credit subaccount                                
				UPDATE #mapping
				SET [AmountTypeId_FK] = amt.AmountTypeId
				FROM #mapping m
				INNER JOIN t_subaccounts acc ON m.creditaccount = acc.subaccountid
				INNER JOIN t_activityamounttype amt ON amt.defaultSubaccountAcronym = acc.Acronym

				DECLARE @drmapp INT
				DECLARE @crmapp INT

				SELECT @drmapp = count(DebitAccount)
				FROM #mapping
				WHERE [AmountTypeId_FK] IS NULL

				SELECT @crmapp = count(CreditAccount)
				FROM #mapping
				WHERE [AmountTypeId_FK] IS NULL

				IF (@drmapp = 1)
					UPDATE #mapping
					SET AmountTypeId_FK = (
							SELECT AmountTypeId
							FROM t_activityamounttype
							WHERE AmountType = 'Amount'
							)
					WHERE [AmountTypeId_FK] IS NULL
						AND creditaccount IS NULL

				IF (@crmapp = 1)
					UPDATE #mapping
					SET AmountTypeId_FK = (
							SELECT AmountTypeId
							FROM t_activityamounttype
							WHERE AmountType = 'Amount'
							)
					WHERE [AmountTypeId_FK] IS NULL
						AND DebitAccount IS NULL

				-- if still null all will be set to fees and then only one draccount/craccount will be set to 'amount' amountype.  
				UPDATE #mapping
				SET AmountTypeId_FK = (
						SELECT AmountTypeId
						FROM t_activityamounttype
						WHERE AmountType = 'ClearingFee'
						)
				WHERE [AmountTypeId_FK] IS NULL

				-- if in the mapping there is still only one or NO AmountType then it will be a problem making journal with this mapping. So handling it -----   
				SELECT subaccountid
					,max(dr - cr) AS amount
				INTO #drcramountsubaccount
				FROM #NonTradejournal
				WHERE transactionid = @manualtransactionid
				GROUP BY subaccountid

				DECLARE @AmountSubaccount INT -- subaccount which is to be amount type in activityType  

				IF NOT EXISTS (
						SELECT *
						FROM #mapping
						WHERE [AmountTypeId_FK] = (
								SELECT AmountTypeId
								FROM t_activityamounttype
								WHERE AmountType = 'Amount'
								)
							AND debitaccount IS NULL
						)
				BEGIN
					SELECT TOP 1 @AmountSubaccount = subaccountid
					FROM #drcramountsubaccount
					ORDER BY amount

					UPDATE #mapping
					SET [AmountTypeId_FK] = (
							SELECT AmountTypeId
							FROM t_activityamounttype
							WHERE AmountType = 'Amount'
							)
					WHERE creditaccount = @AmountSubaccount
				END

				IF NOT EXISTS (
						SELECT *
						FROM #mapping
						WHERE [AmountTypeId_FK] = (
								SELECT AmountTypeId
								FROM t_activityamounttype
								WHERE AmountType = 'Amount'
								)
							AND creditaccount IS NULL
						)
				BEGIN
					SELECT TOP 1 @AmountSubaccount = subaccountid
					FROM #drcramountsubaccount
					ORDER BY amount DESC

					UPDATE #mapping
					SET [AmountTypeId_FK] = (
							SELECT AmountTypeId
							FROM t_activityamounttype
							WHERE AmountType = 'Amount'
							)
					WHERE debitaccount = @AmountSubaccount
				END

				------------------------------------------------------------------------------------------------------------------------------------------                                
				--updating CashvalueType             
				UPDATE #mapping
				SET [CashValueType] = CASE 
						WHEN (
								SELECT sum(dr) - sum(cr)
								FROM #SingleJournal
								WHERE transactiontype = 'Cash'
									OR transactiontype = 'Accrued Balance'
								) < 0
							THEN 1
						ELSE 0
						END
				WHERE [AmountTypeId_FK] = (
						SELECT AmountTypeId
						FROM t_activityamounttype
						WHERE AmountType = 'Amount'
						)

				-- inserting new mapping                                
				INSERT INTO T_ActivityJournalMapping (
					ActivityTypeId_FK
					,AmountTypeId_FK
					,DebitAccount
					,CreditAccount
					,CashValueType
					)
				SELECT *
				FROM #mapping

				SET @isNewActivityTypeCreated = 1

				DROP TABLE #mapping

				DROP TABLE #drcramountsubaccount
			END

			---- Activity journal mapping done.-----------                                
			-----------------------------------------------                                
			--Now activity                                 
			-- inserting from #NonTradejournal to temp activity table       
			-- Will not insert settlement date,closeQty which doesn't exist for non-trading                   
			INSERT INTO #activity (
				fkId
				,TransactionSource
				,FundId
				,Symbol
				,TradeDate
				,SettlementDate
				,CurrencyId
				,LeadCurrencyID
				,VsCurrencyID
				,ClosedQty
				,Subactivity
				,Fxrate
				,FXConversionMethodOperator
				,activitysource
				,sidemultiplier
				,SettlCurrency
				)
			SELECT DISTINCT transactionid
				,TransactionSource
				,FundId
				,isnull(Symbol, '')
				,TransactionDate
				,TransactionDate
				,CurrencyId
				,0
				,0
				,0
				,''
				,Fxrate
				,FXConversionMethodOperator
				,ActivitySource
				,1
				,0
			FROM #NonTradejournal
			WHERE transactionid = @manualtransactionid

			-----------------multiple Descriptions-----------------------------------------------------                        
			SET @descriptions = ''

			SELECT @descriptions = COALESCE(@descriptions + ', ', '') + DistinctDesc
			FROM (
				SELECT DISTINCT pbdesc AS DistinctDesc
				FROM #NonTradejournal
				WHERE transactionid = @manualtransactionid
					AND pbdesc IS NOT NULL
				) AS tableofdescripts

			SET @descriptions = substring(@descriptions, 2, len(@descriptions))

			UPDATE #activity
			SET description = @descriptions
			WHERE fkid = @manualtransactionid

			SET @modifyDate = GETDATE()
			SET @entryDate = GETDATE()
			SET @userId = 0

			SELECT @modifyDate = ModifyDate
				,@entryDate = EntryDate
				,@userId = UserID
			FROM (
				SELECT TOP (1) ModifyDate
					,EntryDate
					,UserID
				FROM #NonTradejournal
				WHERE transactionid = @manualtransactionid
				) AS tableofuser

			UPDATE #activity
			SET ModifyDate = @modifyDate
				,EntryDate = @entryDate
				,UserID = @userId
			WHERE fkid = @manualtransactionid

			----------------------------------------------------------------------------------------------  
			SELECT dr
				,defaultsubaccountacronym
				,mapp.debitaccount
			INTO #temp2
			FROM t_activityjournalmapping mapp
			INNER JOIN t_activityamounttype amt ON amt.amounttypeid = mapp.amounttypeid_fk
			INNER JOIN #NonTradejournal j ON j.subaccountid = mapp.debitaccount
			INNER JOIN t_subaccounts acc ON acc.acronym = amt.defaultsubaccountacronym
			WHERE activitytypeid_fk = @activityTypeid
				AND j.accountside = 0
				AND j.transactionid = @manualtransactionid
				AND mapp.debitaccount NOT IN (
					SELECT CASE 
							WHEN debitaccount IS NULL
								THEN creditaccount
							ELSE debitaccount
							END AS subaccountid
					FROM t_activityjournalmapping
					WHERE t_activityjournalmapping.amounttypeid_fk = (
							SELECT AmountTypeId
							FROM t_activityamounttype
							WHERE AmountType = 'Amount'
							)
						AND activitytypeid_fk = @activityTypeid
					)

			--- updating amount and fees------------------------------------       
			UPDATE #activity
			SET stampduty = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Stamp_Duty'
						), 0)
				,Commission = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Commission'
						), 0)
				,OtherBrokerFees = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Other_Broker_Fees'
						), 0)
				,TransactionLevy = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Transaction_Levy'
						), 0)
				,ClearingFee = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Clearing_Fee'
						), 0)
				,TaxOnCommissions = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Tax_On_Commissions'
						), 0)
				,MiscFees = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Misc_Fees'
						), 0)
				,SecFee = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Sec_Fee'
						), 0)
				,OccFee = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Occ_Fee'
						), 0)
				,OrfFee = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Orf_Fee'
						), 0)
				,ClearingBrokerFee = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Clearing_Broker_Fee'
						), 0)
				,SoftCommission = isnull((
						SELECT sum(dr)
						FROM #temp2
						WHERE defaultsubaccountacronym = 'Soft_Commission'
						), 0)
			WHERE fkid = @manualtransactionid

			--------------------------------------------------------------------                                
			-- updating all other details needed-------------                                
			DECLARE @total [varchar] (50) --total dr                                                
			DECLARE @commission DECIMAL --total commission                              

			SELECT @total = sum(dr - cr)
			FROM #SingleJournal
			WHERE transactiontype = 'Cash'
				OR (
					transactiontype = 'Accrued Balance'
					AND transactionsource NOT IN (
						4
						,5
						)
					)

			-- ---Special Handling  for Dividend Entries------------------------------------------------------------------------  
			-- select @total=sum(dr-cr) from #SingleJournal where transactiontype='Cash'  and transactionsource in (4,5)        
			-- -------------------------------------------------------------------------------------------------------------------                 
			SELECT @commission = (
					SELECT sum(dr)
					FROM #temp2
					)

			IF (@FeesAdjust = 1)
			BEGIN
				SET @total = @total - @commission
				SET @FeesAdjust = 0
			END

			-- .01 is added for uniqueness as there Non trading saved from this SP.
            SET @activityid = Max(cast(isnull(round(@activityid,0), 0) AS BIGINT)) + 1 + .01

			UPDATE #activity
			SET amount = @total
				,balancetype = CASE 
					WHEN @transactiontype LIKE 'Cash'
						THEN 1
					ELSE 2
					END
				,activitytypeid_fk = @activityTypeid
				,activityid = @activityid
				,activitynumber = 1
			FROM #activity
			WHERE fkid = @manualtransactionid

			--unique key                     
			UPDATE #activity
			SET UniqueKey = FKID + cast(DATEPART(m, tradedate) AS VARCHAR(5)) + '/' + cast(DATEPART(d, tradedate) AS VARCHAR(5)) + '/' + cast(DATEPART(yyyy, tradedate) AS VARCHAR(5)) + TransactionSource + cast(ActivityNumber AS VARCHAR(5))
			FROM #activity
			WHERE fkid = @manualtransactionid

			-------------------------------------------------------------                             
			DROP TABLE #temp2
		END

		--------New Handling for (Accrual + Cash) or having Multiple Currency or Both of them in Journal-------------------------------------------------------  
		IF (@verify > 1)
		BEGIN
			--------------- activity type would be 'UnknownActivity'which have no mapping -------------------------------------------------------  
			SELECT @activityTypeid = ActivityTypeId
			FROM T_ActivityType
			WHERE acronym LIKE 'UnknownActivity'

			-----------Here Making key of different splits on whose behalf we could no of activities of this single journal--------------------------------------------------  
			SELECT ROW_NUMBER() OVER (
					ORDER BY currencyid
						,transactiontype
						,fxrate
					) AS id
				,currencyid
				,transactiontype
				,fxrate
			INTO #journalsplit
			FROM #SingleJournal
			WHERE transactiontype = 'Cash'
				OR transactiontype = 'Accrued Balance'
			GROUP BY currencyid
				,transactiontype
				,fxrate

			-----------------------------------------------------------------------------------------------   
			DECLARE @NoOfSplits INT
			DECLARE @currentsplit INT

			SET @currentsplit = 1

			SELECT @NoOfSplits = count(*)
			FROM #journalsplit

			WHILE (@currentsplit <= @NoOfSplits)
			BEGIN
				INSERT INTO #activity (
					fkId
					,TransactionSource
					,FundId
					,Symbol
					,TradeDate
					,SettlementDate
					,CurrencyId
					,LeadCurrencyID
					,VsCurrencyID
					,ClosedQty
					,Subactivity
					,Fxrate
					,FXConversionMethodOperator
					,ActivitySource
					,sidemultiplier
					,activitynumber
					,balancetype
					,SettlCurrency
					)
				SELECT DISTINCT transactionid
					,TransactionSource
					,FundId
					,isnull(Symbol, '')
					,TransactionDate
					,TransactionDate
					,#SingleJournal.CurrencyId
					,0
					,0
					,0
					,''
					,#SingleJournal.Fxrate
					,FXConversionMethodOperator
					,ActivitySource
					,1
					,@currentsplit
					,CASE 
						WHEN #SingleJournal.transactiontype LIKE 'Cash'
							THEN 1
						ELSE 2
						END
					,0
				FROM #SingleJournal
				INNER JOIN #journalsplit ON #SingleJournal.currencyid = #journalsplit.currencyid
					AND #SingleJournal.fxrate = #journalsplit.fxrate
					AND #SingleJournal.transactiontype = #journalsplit.transactiontype
				WHERE #journalsplit.id = @currentsplit

                SET @activityid = Max(cast(isnull(round(@activityid,0), 0) AS BIGINT)) + 1 + .01

				UPDATE #activity
				SET activityid = @activityid
				WHERE activityid IS NULL -- this check is used so that the other activities of same journal dont modify.  

				----------------- multiple Descriptions,activityType-----------------------------------------------------                        
				SET @descriptions = ''

				SELECT @descriptions = COALESCE(@descriptions + ', ', '') + DistinctDesc
				FROM (
					SELECT DISTINCT pbdesc AS DistinctDesc
					FROM #NonTradejournal
					WHERE transactionid = @manualtransactionid
						AND pbdesc IS NOT NULL
					) AS tableofdescripts

				SET @descriptions = substring(@descriptions, 2, len(@descriptions))

				UPDATE #activity
				SET description = @descriptions
					,activitytypeid_fk = @activityTypeid
				WHERE activityid = @activityid

				SET @modifyDate = GETDATE()
				SET @entryDate = GETDATE()
				SET @userId = 0

				SELECT @modifyDate = ModifyDate
					,@entryDate = EntryDate
					,@userId = UserID
				FROM (
					SELECT TOP (1) ModifyDate
						,EntryDate
						,UserID
					FROM #NonTradejournal
					WHERE transactionid = @manualtransactionid
					) AS tableofuser

				UPDATE #activity
				SET ModifyDate = @modifyDate
					,EntryDate = @entryDate
					,UserID = @userId
				WHERE activityid = @activityid

				----------------------------------------------------------------------------------------------                      
				DECLARE @amount VARCHAR(50)

				---- setting amount for cash /accrual activity accordingly  
				SELECT @amount = sum(dr) - sum(cr)
				FROM #SingleJournal
				INNER JOIN #journalsplit ON #SingleJournal.currencyid = #journalsplit.currencyid
					AND #SingleJournal.transactiontype = #journalsplit.transactiontype
				WHERE #journalsplit.id = @currentsplit

				UPDATE #activity
				SET amount = @amount
				WHERE activityid = @activityid

				--- updating amount and fees------------------------------------                                
				SELECT dr
					,defaultsubaccountacronym
				INTO #temp3
				FROM #SingleJournal j
				INNER JOIN t_activityamounttype amt ON amt.defaultsubaccountacronym = j.acronym
				WHERE j.accountside = 0
					AND currencyid = (
						SELECT currencyid
						FROM #activity
						WHERE activityid = @activityid
						)

				UPDATE #activity
				SET stampduty = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Stamp_Duty'
							), 0)
					,Commission = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Commission'
							), 0)
					,OtherBrokerFees = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Other_Broker_Fees'
							), 0)
					,TransactionLevy = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Transaction_Levy'
							), 0)
					,ClearingFee = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Clearing_Fee'
							), 0)
					,TaxOnCommissions = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Tax_On_Commissions'
							), 0)
					,MiscFees = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Misc_Fees'
							), 0)
					,SecFee = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Sec_Fee'
							), 0)
					,OccFee = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Occ_Fee'
							), 0)
					,OrfFee = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Orf_Fee'
							), 0)
					,ClearingBrokerFee = isnull((
							SELECT sum(dr)
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Clearing_Broker_Fee'
							), 0)
					,SoftCommission = isnull((
							SELECT sum(dr) r
							FROM #temp3
							WHERE defaultsubaccountacronym = 'Soft_Commission'
							), 0)
				WHERE activityid = @activityid
					AND BalanceType = 1

				UPDATE #activity
				SET stampduty = 0
					,Commission = 0
					,OtherBrokerFees = 0
					,TransactionLevy = 0
					,ClearingFee = 0
					,TaxOnCommissions = 0
					,MiscFees = 0
					,SecFee = 0
					,OccFee = 0
					,OrfFee = 0
					,ClearingBrokerFee = 0
					,SoftCommission = 0
				WHERE activityid = @activityid
					AND BalanceType = 2

				-------------------------------------------------------------------------------------------------   
				SET @currentsplit = @currentsplit + 1

				DROP TABLE #temp3
			END

			DROP TABLE #journalsplit
		END --  END OF ELSE  

		DROP TABLE #SingleJournal

		------------------------------------------------------------------------------------------------------------  
		SET @activityTypeid = NULL
		SET @row = @row + 1
	END

	-- updating #NonTradejournal ActivityId_FK column   
	UPDATE #NonTradejournal
	SET ActivityId_FK = ActivityID
	FROM #activity
	INNER JOIN #NonTradejournal ON #activity.fkid = #NonTradejournal.transactionid
	WHERE activitynumber = 1

	-- !!!!!  Now As we know there can be journals whose activityid_fk can be null as no activity made for journals having no cash or accrual part.  
	--so have to delete the foreign constraint.  
	IF (@xml IS NULL)
	BEGIN
		-- updating journal ActivityId_FK column   
		UPDATE t_journal
		SET ActivityId_FK = ActivityID
		FROM #activity
		INNER JOIN t_journal ON #activity.fkid = t_journal.transactionid
		WHERE activitynumber = 1

		-------------------------update of Activityid_fk For Dividend Entries in exdate entries------------------------------------------------------------------------------------------  
		SELECT ActivityId_FK AS ActivityId_FK
			,T_CashDivTransactions.fkid AS fkid
		INTO #temporary
		FROM T_CashDivTransactions
		INNER JOIN t_journal ON T_CashDivTransactions.transactionid = t_journal.transactionid
		WHERE ActivityId_FK IS NOT NULL

		UPDATE t_journal
		SET ActivityId_FK = #temporary.ActivityId_FK
		FROM t_journal j
		INNER JOIN T_CashDivTransactions div ON j.transactionid = div.transactionid
		INNER JOIN #temporary ON #temporary.fkid = div.fkid
		WHERE j.ActivityId_FK IS NULL

		------------------------------------------------------------------------------------------------------  
		--Setting side multiplier,closedQty,SettlementDate of trading taxlots  
		UPDATE #activity
		SET sidemultiplier = dbo.[GetSideMultiplier](T_Group.OrderSideTagValue)
			,#activity.ClosedQty = cast(T_Group.quantity AS DECIMAL)
			,#activity.settlementDate = T_Group.settlementdate
		FROM #activity
		INNER JOIN t_journal ON #activity.Fkid = t_journal.transactionid
		INNER JOIN pm_taxlots ON t_journal.taxlotid = pm_taxlots.taxlotid
		INNER JOIN T_Group ON T_Group.groupid = pm_taxlots.groupid
		WHERE #activity.TransactionSource NOT IN (
				2
				,8
				,9
				,4
				,5
				)

		--Setting fkid to taxlotid of trading journals----  
		UPDATE #activity
		SET Fkid = #NonTradejournal.taxlotid
		FROM #NonTradejournal
		INNER JOIN #activity ON #activity.Fkid = #NonTradejournal.transactionid
		WHERE #NonTradejournal.transactionsource IN (
				1
				,3
				,7
				)

		--dividend Journals  --  
		UPDATE #activity
		SET fkid = div.fkid
		FROM #activity act
		INNER JOIN T_CashDivTransactions div ON act.fkid = div.transactionid
		WHERE act.Transactionsource IN (
				4
				,5
				)

		UPDATE #activity
		SET TradeDate = Exdate
		FROM #activity act
		INNER JOIN T_CashTransactions cashdiv ON act.fkid = cashdiv.cashtransactionid
		WHERE act.Transactionsource IN (
				4
				,5
				)

		-------------in doubt it is needed or not------------------------------------------------   
		UPDATE #activity
		SET SettlementDate = PayoutDate
		FROM #activity act
		INNER JOIN T_CashTransactions cashdiv ON act.fkid = cashdiv.cashtransactionid
		WHERE act.Transactionsource IN (
				4
				,5
				)
			-----------------------------------------------------------------------------------------  
	END

	--unique key                     
	UPDATE #activity
	SET UniqueKey = FKID + cast(DATEPART(m, tradedate) AS VARCHAR(5)) + '/' + cast(DATEPART(d, tradedate) AS VARCHAR(5)) + '/' + cast(DATEPART(yyyy, tradedate) AS VARCHAR(5)) + TransactionSource + cast(ActivityNumber AS VARCHAR(5))

	INSERT INTO t_allactivity
	SELECT *
	FROM #activity

	-------------------------------------Updating Cash Transactions UI Activity Type------------------------------------------------------------------------  
	UPDATE T_CashTransactions
	SET ActivityTypeID = #Activity.ActivityTypeId_FK
	FROM T_CashTransactions
	INNER JOIN #activity ON #activity.FKID = T_CashTransactions.CashTransactionID
	WHERE #Activity.TransactionSource IN (
			4
			,5
			)

	--------------------------------------------------------------------------------------------------------------------------------------------------------    
	IF (@xml IS NOT NULL)
	BEGIN
		----------Insert---------------
		CREATE TABLE #Transactions (TransactionID VARCHAR (50))

		INSERT INTO #Transactions SELECT TransactionID
		FROM #NonTradejournal NTJ
		INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = NTJ.SubAccountID
		INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeId = TransType.TransactionTypeId
		WHERE TransType.TransactionType = 'Accrued Balance' 
		and NTJ.TransactionDate>=(select SymbolWiseRevaluationDate from T_CashPreferences where FundID=NTJ.FundID)

		IF (EXISTS(select TransactionID from #Transactions))
		BEGIN
		INSERT INTO T_SymbolLevelAccrualsJournal
		SELECT * FROM #NonTradejournal where TransactionID in (select T.TransactionID from #Transactions T) 
		END

		INSERT INTO t_journal (
			TaxLotID
			,FundID
			,SubAccountID
			,CurrencyID
			,Symbol
			,PBDesc
			,TransactionDate
			,TransactionID
			,DR
			,CR
			,TransactionSource
			,TransactionEntryID
			,TransactionNumber
			,AccountSide
			,ActivityId_FK
			,ActivitySource
			,FxRate
			,FXConversionMethodOperator
			,ModifyDate
			,EntryDate
			,UserId
			)
		SELECT *
		FROM #NonTradejournal
	END

	----------Sending data back to Code--------------------------------------------  
	IF (@isNewActivityTypeCreated = 1)
	BEGIN
		SELECT *
		FROM t_activitytype
		WHERE ActivityTypeId > @LastNewActivityType

		SELECT *
		FROM t_activityjournalmapping
		WHERE ActivityTypeId_FK > @LastNewActivityType
	END

	SELECT *
	FROM #activity

	-------------------------------------------------------------------------------  
	DROP TABLE #activity

	DROP TABLE #NonTradejournal
	
	DROP TABLE #Transactions

	EXEC sp_xml_removedocument @handle
END TRY

BEGIN CATCH
	SELECT ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
