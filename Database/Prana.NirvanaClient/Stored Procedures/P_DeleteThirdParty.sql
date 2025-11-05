
CREATE PROCEDURE [dbo].[P_DeleteThirdParty] (
	@thirdPartyID INT
	,@deleteForceFully INT
	)
AS
DECLARE @total INT
DECLARE @thirdPartyName VARCHAR(100)
DECLARE @msg VARCHAR(250)

IF (@deleteForceFully = 1)
BEGIN
	SELECT @total = Count(*)
	FROM T_CompanyThirdParty
	WHERE ThirdPartyID = @thirdPartyID

	IF (@total = 0)
	BEGIN
		-- If ThirdPartyID is not referenced anywhere.  
		--Delete ThirdPartyFileFormat that is depanded on Third Party Table.  
		DELETE T_ThirdPartyFileFormat
		WHERE ThirdPartyID = @thirdPartyID

		--Delete ThirdParty.  
		DELETE T_ThirdParty
		WHERE ThirdPartyID = @thirdPartyID
	END
	ELSE
	BEGIN
		SET @msg = 'Third Party is referenced in Client Third Party.You Please remove references first to delete it.'
	END
END
ELSE
BEGIN
	-- To check if thirdparty exist in batch setup
	SELECT @thirdPartyName = isnull(ShortName, '')
	FROM T_ThirdParty
	WHERE ThirdPartyID = @thirdPartyID

	SELECT @total = Count(*)
	FROM T_CompanyThirdParty
	WHERE ThirdPartyID = @thirdPartyID

	IF (@total > 0)
	BEGIN
		SET @msg = @thirdPartyName + ' is associated with Company' + '. Delete association first.'
	END

	-- To check if thirdparty exist in batch setup
	IF (@total = 0)
	BEGIN
		SELECT @total = Count(*)
		FROM T_ImportFileSettings
		WHERE ThirdPartyID = @thirdPartyID

		IF (@total > 0)
			SET @msg = @thirdPartyName + ' is associated with Batch' + '. Delete association first.'
	END

	-- To check if thirdparty exist in T_CompanyFunds
	IF (@total = 0)
	BEGIN
		SELECT @total = Count(*)
		FROM T_CompanyFunds
		WHERE CompanyThirdPartyID = @thirdPartyID

		IF (@total > 0)
			SET @msg = @thirdPartyName + ' is associated with company funds. Delete association first.'
	END

	--Delete ThirdParty.	
	IF (@total = 0)
	BEGIN
		--Delete T_ThirdParty
		--Where ThirdPartyID = @thirdPartyID
		UPDATE T_ThirdParty
		SET isActive = 0
		WHERE ThirdPartyID = @thirdPartyID;
	END
END
SELECT @msg

