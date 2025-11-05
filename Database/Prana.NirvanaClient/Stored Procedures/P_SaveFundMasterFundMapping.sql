CREATE PROCEDURE [dbo].[P_SaveFundMasterFundMapping] (
	 @XMLDoc NTEXT
	,@XMLTADoc NTEXT
	,@XMLMDoc NTEXT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	,@companyID INT
	,@deleteForceFully INT
	)
AS
SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@XMLDoc

DECLARE @handle1 INT

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@XMLMDoc

DECLARE @handle2 INT

EXEC sp_xml_preparedocument @handle2 OUTPUT
	,@XMLTADoc

CREATE TABLE #TempTableNames (
	CompanyMasterFundId INT
	,CompanyFundId INT
	)

CREATE TABLE #TempTableNames1 (
	CompanyMasterFundId INT
	,MasterFundName NVARCHAR(200)
	,CompanyID INT
	,QueryType INT
	)
	
CREATE TABLE #TempTableNames2 (
	CompanyMasterFundId INT
	,CompanyTradingAccountId INT
	)

IF (@deleteForceFully = 1)
BEGIN
	BEGIN TRY
		BEGIN TRAN TRAN1         
		INSERT INTO #TempTableNames (
			CompanyMasterFundId
			,CompanyFundId
			)
		SELECT CompanyMasterFundId
			,CompanyFundId
		FROM OPENXML(@handle, '/DSFundMasterFundMapping/TABFundMasterFundMapping', 2) WITH (
				CompanyMasterFundId INT
				,CompanyFundId INT
				)

		INSERT INTO #TempTableNames1 (
			CompanyMasterFundId
			,MasterFundName
			,CompanyID
			,QueryType
			)
		SELECT CompanyMasterFundID
			,MasterFundName
			,CompanyID
			,QueryType
		FROM OPENXML(@handle1, '/NewDataSet/Table1', 2) WITH (
				CompanyMasterFundID INT
				,MasterFundName NVARCHAR(200)                                                                            
				,CompanyID INT
				,QueryType INT
				)
       
		INSERT INTO #TempTableNames2 (
			CompanyMasterFundId
			,CompanyTradingAccountId
			)
		SELECT CompanyMasterFundId
			,CompanyTradingAccountId
		FROM OPENXML(@handle2, '/DSMasterFundTradingAccountMapping/TABMasterFundTradingAccountMapping', 2) WITH (
				CompanyMasterFundId INT
				,CompanyTradingAccountId INT
				)

		IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE MasterFundName IN (
						SELECT T_CompanyMasterFunds.MasterFundName
						FROM T_CompanyMasterFunds
						)
				) > 0
        BEGIN
			SET @ErrorNumber = - 11

			SELECT @ErrorNumber
		END

		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 0
				) > 0
		BEGIN
			SET IDENTITY_INSERT T_CompanyMasterFunds ON

			INSERT INTO T_CompanyMasterFunds (
				CompanyMasterFundId
				,MasterFundName
				,CompanyID
				)
			SELECT CompanyMasterFundId
				,MasterFundName
				,CompanyID
			FROM #TempTableNames1
			WHERE CompanyMasterFundId NOT IN (
					SELECT CompanyMasterFundID
					FROM T_CompanyMasterFunds
					WHERE CompanyID = @companyID
					)
		END

		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 2
				) > 0
		BEGIN
			
		-- Deletion from mapping table for existing master funds
		DELETE MFA
		FROM T_CompanyMasterFundSubAccountAssociation AS MFA
		LEFT JOIN T_CompanyMasterFunds MF ON MF.CompanyMasterFundID = MFA.CompanyMasterFundID
		WHERE MF.CompanyID = @companyID

		DELETE 
		FROM T_CompanyMasterFunds
		WHERE CompanyMasterFundID IN (
					SELECT CompanyMasterFundId
					FROM #TempTableNames1
					WHERE QueryType = 2
					)
		END

		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 1
				) > 0
		BEGIN
			UPDATE T_CompanyMasterFunds
			SET MasterFundName = TN.MasterFundName
			FROM T_CompanyMasterFunds TC
			INNER JOIN #TempTableNames1 TN ON TN.CompanyMasterFundId = TC.CompanyMasterFundID
			WHERE TN.QueryType = 1
		END

		-- Deletion from mapping table for existing master funds
		DELETE MFA
		FROM T_CompanyMasterFundSubAccountAssociation AS MFA
		LEFT JOIN T_CompanyMasterFunds MF ON MF.CompanyMasterFundID = MFA.CompanyMasterFundID
		WHERE MF.CompanyID = @companyID

		-- Insertion of mapping details of existing master funds in mapping table	
		INSERT INTO T_CompanyMasterFundSubAccountAssociation (
			CompanyMasterFundId
			,CompanyFundId
			)
		SELECT CompanyMasterFundId
			,CompanyFundId
		FROM #TempTableNames

		-- Deletion from Trading Account mapping table for existing master funds
		DELETE MFTA
		FROM T_CompanyMasterFundTradingAccountAssociation AS MFTA
		LEFT JOIN T_CompanyMasterFunds MF ON MF.CompanyMasterFundID = MFTA.CompanyMasterFundID
		WHERE MF.CompanyID = @companyID

		-- Insertion of mapping details of existing master funds in mapping table	
		INSERT INTO T_CompanyMasterFundTradingAccountAssociation (
			CompanyMasterFundId
			,CompanyTradingAccountID
			)
		SELECT CompanyMasterFundId
			,CompanyTradingAccountId
		FROM #TempTableNames2

		SELECT @ErrorNumber

		EXEC sp_xml_removedocument @handle

		EXEC sp_xml_removedocument @handle1
			COMMIT TRANSACTION TRAN1                        
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();

		PRINT @errormessage

		SET @ErrorNumber = Error_number();
			ROLLBACK TRANSACTION TRAN1                         
	END CATCH;
END
ELSE
BEGIN
	BEGIN TRAN TRAN1

	BEGIN TRY
		INSERT INTO #TempTableNames (
			CompanyMasterFundId
			,CompanyFundId
			)
		SELECT CompanyMasterFundId
			,CompanyFundId
		FROM OPENXML(@handle, '/DSFundMasterFundMapping/TABFundMasterFundMapping', 2) WITH (
				CompanyMasterFundId INT
				,CompanyFundId INT
				)

		INSERT INTO #TempTableNames1 (
			CompanyMasterFundId
			,MasterFundName
			,CompanyID
			,QueryType
			)
		SELECT CompanyMasterFundID
			,MasterFundName
			,CompanyID
			,QueryType
		FROM OPENXML(@handle1, '/NewDataSet/Table1', 2) WITH (
				CompanyMasterFundID INT
				,MasterFundName NVARCHAR(100)
				,CompanyID INT
				,QueryType INT
				)

		IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE MasterFundName IN (
						SELECT T_CompanyMasterFunds.MasterFundName
						FROM T_CompanyMasterFunds
						)
				) > 0
		BEGIN
			SET @ErrorNumber = - 11

			SELECT @ErrorNumber
		END
		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 0
				) > 0
		BEGIN
			SET IDENTITY_INSERT T_CompanyMasterFunds ON

			INSERT INTO T_CompanyMasterFunds (
				CompanyMasterFundId
				,MasterFundName
				,CompanyID
				)
			SELECT CompanyMasterFundId
				,MasterFundName
				,CompanyID
			FROM #TempTableNames1
			WHERE CompanyMasterFundId NOT IN (
					SELECT CompanyMasterFundID
					FROM T_CompanyMasterFunds
					WHERE CompanyID = @companyID
					)
		END
		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 2
				) > 0
		BEGIN
			-- To set isActive to false for deleted master funds
			UPDATE T_CompanyMasterFunds
			SET IsActive = 0
			WHERE CompanyMasterFundID IN (
					SELECT CompanyMasterFundId
					FROM #TempTableNames1
					WHERE QueryType = 2
					)
		END
		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 1
				) > 0
		BEGIN
			UPDATE T_CompanyMasterFunds
			SET MasterFundName = TN.MasterFundName
			FROM T_CompanyMasterFunds TC
			INNER JOIN #TempTableNames1 TN ON TN.CompanyMasterFundId = TC.CompanyMasterFundID
			WHERE TN.QueryType = 1
		END

		-- Deletion from mapping table for existing master funds
		DELETE MFA
		FROM T_CompanyMasterFundSubAccountAssociation AS MFA
		LEFT JOIN T_CompanyMasterFunds MF ON MF.CompanyMasterFundID = MFA.CompanyMasterFundID
		WHERE MF.CompanyID = @companyID

		-- Insertion of mapping details of existing master funds in mapping table
		INSERT INTO T_CompanyMasterFundSubAccountAssociation (
			CompanyMasterFundId
			,CompanyFundId
			)
		SELECT CompanyMasterFundId
			,CompanyFundId
		FROM #TempTableNames

		SELECT @ErrorNumber

		-- Insertion of new master funds in T_CompanyMasterFunds
		--SET IDENTITY_INSERT T_CompanyMasterFunds ON
		--insert into T_CompanyMasterFunds (CompanyMasterFundId, MasterFundName, CompanyID) select CompanyMasterFundId,MasterFundName, CompanyID from #TempTableNames1
		--where CompanyMasterFundId not in (SELECT CompanyMasterFundID from T_CompanyMasterFunds WHERE CompanyID=@companyID)
		-- Insertion of mapping details of existing master funds in mapping table
		--Insert into T_CompanyMasterFundSubAccountAssociation (CompanyMasterFundId,CompanyFundId) select CompanyMasterFundId,CompanyFundId from #TempTableNames       
		EXEC sp_xml_removedocument @handle

		EXEC sp_xml_removedocument @handle1

		COMMIT TRANSACTION TRAN1
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();

		PRINT @errormessage

		SET @ErrorNumber = Error_number();

		ROLLBACK TRANSACTION TRAN1
	END CATCH;
END


BEGIN TRY
BEGIN TRANSACTION PTTTRANS
		DELETE  pref 
	    FROM T_PTTMasterFundPreference pref
		LEFT JOIN T_CompanyMasterFundSubAccountAssociation mapping
		ON pref.MasterFundId=mapping.CompanyMasterFundID
		WHERE mapping.CompanyMasterFundID IS NULL

	   CREATE TABLE #Temp
		(
			PreferenceType int
		)

		INSERT INTO #Temp (PreferenceType) 
		select 0
		UNION ALL
		select 1
		UNION ALL
		select 2

		INSERT INTO T_PTTMasterFundPreference (MasterFundId,PreferenceType)
		SELECT Distinct mapping.CompanyMasterFundID,Tmp.PreferenceType FROM T_CompanyMasterFundSubAccountAssociation mapping 
		LEFT JOIN T_PTTMasterFundPreference pref on mapping.CompanyMasterFundID=pref.MasterFundId
		CROSS JOIN #Temp Tmp
		WHERE pref.MasterFundId IS NULL
		drop table #Temp

		COMMIT TRANSACTION PTTTRANS
END TRY
BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();

		PRINT @errormessage

		SET @ErrorNumber = Error_number();

		ROLLBACK TRANSACTION PTTTRANS
	END CATCH;