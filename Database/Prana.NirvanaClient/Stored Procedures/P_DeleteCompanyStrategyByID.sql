
CREATE PROCEDURE [dbo].[P_DeleteCompanyStrategyByID] (@companyStrategyID INT, @deleteForceFully INT)
AS
DECLARE @result INT
DECLARE @count INT

SET @result = 0

IF (@deleteForceFully = 1)
BEGIN
	BEGIN TRY
		BEGIN
			BEGIN TRAN

			DELETE T_CompanyStrategy
			WHERE CompanyStrategyID = @companyStrategyID

			DELETE T_CompanyUserStrategies
			WHERE CompanyStrategyID = @companyStrategyID

			DELETE T_CompanyCounterPartyVenueDetails
			WHERE CompanyStrategyID = @companyStrategyID

			COMMIT TRAN

			SET @result = 1
		END
	END TRY

	BEGIN CATCH
		ROLLBACK TRAN

		SET @result = - 1
	END CATCH

	SELECT @result
END
ELSE
BEGIN
	SELECT @count = COUNT(*)
	FROM T_CompanyMasterStrategySubAccountAssociation
	WHERE CompanyStrategyID = @companyStrategyID

	IF @count > 0
	BEGIN
		SET @result = - 1
	END
	ELSE
	BEGIN TRY
		BEGIN
			BEGIN TRAN

			UPDATE T_CompanyStrategy
			SET IsActive = 0
			WHERE CompanyStrategyID = @companyStrategyID

			COMMIT TRAN

			SET @result = 1
		END
	END TRY

	BEGIN CATCH
		ROLLBACK TRAN

		SET @result = - 1
	END CATCH

	SELECT @result
END
