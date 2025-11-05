
CREATE PROCEDURE [dbo].[P_SaveStrategyMasterStrategyMapping] (
	@XMLDoc NTEXT
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
DECLARE @handle1 INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@XMLDoc

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@XMLMDoc

CREATE TABLE #TempTableNames (
	CompanyMasterStrategyId INT
	,CompanyStrategyId INT
	)

CREATE TABLE #TempTableNames1 (
	CompanyMasterStrategyId INT
	,MasterStrategyName NVARCHAR(100)
	,CompanyId INT
	,QueryType INT
	)

IF (@deleteForceFully = 1)
BEGIN
	BEGIN TRY
		--BEGIN TRAN TRAN1         
		INSERT INTO #TempTableNames (
			CompanyMasterStrategyId
			,CompanyStrategyId
			)
		SELECT CompanyMasterStrategyId
			,CompanyStrategyId
		FROM OPENXML(@handle, '/DSStrategyMasterStrategyMapping/TABStrategyMasterStrategyMapping', 2) WITH (
				CompanyMasterStrategyId INT
				,CompanyStrategyId INT
				)

		INSERT INTO #TempTableNames1 (
			CompanyMasterStrategyId
			,MasterStrategyName
			,CompanyId
			)
		SELECT CompanyMasterStrategyID
			,MasterStrategyName
			,CompanyId
		FROM OPENXML(@handle1, '/NewDataSet/Table1', 2) WITH (
				CompanyMasterStrategyID INT
				,MasterStrategyName NVARCHAR(100)
				,CompanyId INT
				)

		SELECT *
		FROM #TempTableNames

		SELECT *
		FROM #TempTableNames1

		DELETE
		FROM T_CompanyMasterStrategySubAccountAssociation
		WHERE CompanyMasterStrategyID IN (
				SELECT CompanyMasterStrategyId
				FROM T_CompanyMasterStrategy
				WHERE CompanyId = @companyID
				)

		DELETE
		FROM T_CompanyMasterStrategy
		WHERE CompanyID = @companyID

		SET IDENTITY_INSERT T_CompanyMasterStrategy ON

		INSERT INTO T_CompanyMasterStrategy (
			CompanyMasterStrategyId
			,MasterStrategyName
			,CompanyID
			)
		SELECT CompanyMasterStrategyId
			,MasterStrategyName
			,CompanyId
		FROM #TempTableNames1

		--select count(*) from T_CompanyMasterStrategy 
		INSERT INTO T_CompanyMasterStrategySubAccountAssociation (
			CompanyMasterStrategyId
			,CompanyStrategyId
			)
		SELECT CompanyMasterStrategyId
			,CompanyStrategyId
		FROM #TempTableNames

		EXEC sp_xml_removedocument @handle

		EXEC sp_xml_removedocument @handle1
			--COMMIT TRANSACTION TRAN1                        
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();

		PRINT @errormessage

		SET @ErrorNumber = Error_number();
			--ROLLBACK TRANSACTION TRAN1                         
	END CATCH;
END
ELSE
BEGIN
	BEGIN TRY
		--BEGIN TRANSACTION TRAN1

		INSERT INTO #TempTableNames (
			CompanyMasterStrategyId
			,CompanyStrategyId
			)
		SELECT CompanyMasterStrategyId
			,CompanyStrategyId
		FROM OPENXML(@handle, '/DSStrategyMasterStrategyMapping/TABStrategyMasterStrategyMapping', 2) WITH (
				CompanyMasterStrategyId INT
				,CompanyStrategyId INT
				)

		INSERT INTO #TempTableNames1 (
			CompanyMasterStrategyId
			,MasterStrategyName
			,CompanyId
			,QueryType
			)
		SELECT CompanyMasterStrategyID
			,MasterStrategyName
			,CompanyId
			,QueryType
		FROM OPENXML(@handle1, '/NewDataSet/Table1', 2) WITH (
				CompanyMasterStrategyID INT
				,MasterStrategyName NVARCHAR(100)
				,CompanyId INT
				,QueryType INT
				)

		IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE MasterStrategyName IN (
						SELECT T_CompanyMasterStrategy.MasterStrategyName
						FROM T_CompanyMasterStrategy
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
			SET IDENTITY_INSERT T_CompanyMasterStrategy ON

			INSERT INTO T_CompanyMasterStrategy (
				CompanyMasterStrategyId
				,MasterStrategyName
				,CompanyID
				)
			SELECT CompanyMasterStrategyId
				,MasterStrategyName
				,CompanyId
			FROM #TempTableNames1
			WHERE CompanyMasterStrategyId NOT IN (
					SELECT CompanyMasterStrategyId
					FROM T_CompanyMasterStrategy
					WHERE CompanyID = @companyID
					)
		END
		ELSE IF (
				SELECT COUNT(*)
				FROM #TempTableNames1
				WHERE QueryType = 2
				) > 0
		BEGIN
			-- To set isActive to false for deleted master strategy
			UPDATE T_CompanyMasterStrategy
			SET IsActive = 0
			WHERE CompanyMasterStrategyID IN (
					SELECT CompanyMasterStrategyId
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
			UPDATE T_CompanyMasterStrategy
			SET MasterStrategyName = TN.MasterStrategyName
			FROM T_CompanyMasterStrategy TC
			INNER JOIN #TempTableNames1 TN ON TN.CompanyMasterStrategyId = TC.CompanyMasterStrategyID
			WHERE TN.QueryType = 1
		END

		-- Deletion from mapping table for existing master strategy
		--Modified By : sachin mishra 03-02-15 http://jira.nirvanasolutions.com:8080/browse/CHMW-2460 for saving strategyFund 
		DELETE MFA
		FROM T_CompanyMasterStrategySubAccountAssociation AS MFA
		LEFT JOIN T_CompanyMasterStrategy MF ON MF.CompanyMasterStrategyID = MFA.CompanyMasterStrategyID
		WHERE MF.CompanyID = @companyID

		-- Insertion of mapping details of existing master strategy in mapping table
		INSERT INTO T_CompanyMasterStrategySubAccountAssociation (
			CompanyMasterStrategyId
			,CompanyStrategyId
			)
		SELECT CompanyMasterStrategyId
			,CompanyStrategyId
		FROM #TempTableNames

		SELECT @ErrorNumber

		-- Insertion of new master strategy in T_CompanyMasterStrategy
		--SET IDENTITY_INSERT T_CompanyMasterStrategy ON
		--insert into T_CompanyMasterStrategy (CompanyMasterStrategyId,MasterStrategyName,CompanyID) select CompanyMasterStrategyId,MasterStrategyName,CompanyId from #TempTableNames1 
		--where CompanyMasterStrategyId not in (SELECT CompanyMasterStrategyId from T_CompanyMasterStrategy WHERE CompanyID=@companyID)
		-- Insertion of mapping details of existing master strategy in mapping table
		--Insert into T_CompanyMasterStrategySubAccountAssociation (CompanyMasterStrategyId,CompanyStrategyId) select CompanyMasterStrategyId,CompanyStrategyId from #TempTableNames       
		EXEC sp_xml_removedocument @handle

		EXEC sp_xml_removedocument @handle1

		--COMMIT TRANSACTION TRAN1
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();

		PRINT @errormessage

		SET @ErrorNumber = ERROR_NUMBER();

		--ROLLBACK TRANSACTION TRAN1
	END CATCH;
END
