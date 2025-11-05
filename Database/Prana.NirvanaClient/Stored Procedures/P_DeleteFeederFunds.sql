
-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Delete the feeder funds of the company
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_DeleteFeederFunds] (
	@xmlDoc NTEXT
	,@deleteForceFully INT
	)
AS
DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@xmlDoc

CREATE TABLE #deletefeeder (feederfundId INT)

INSERT INTO #deletefeeder (feederfundid)
SELECT FeederFundID
FROM openxml(@handle, '/dsFeederFunds/dtFeederFunds', 2) WITH (FeederFundID INT)

IF (@deleteForceFully = 1)
BEGIN
	DELETE T_CompanyFeederFunds
	WHERE FeederFundID IN (
			SELECT FeederFundID
			FROM #deletefeeder
			)
END
ELSE
BEGIN
	UPDATE T_CompanyFeederFunds
	SET IsActive = 0
	WHERE FeederFundID IN (
			SELECT FeederFundID
			FROM #deletefeeder
			)
END

EXEC sp_xml_removedocument @handle
