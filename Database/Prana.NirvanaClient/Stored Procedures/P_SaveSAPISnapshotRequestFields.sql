CREATE PROCEDURE [dbo].[P_SaveSAPISnapshotRequestFields]
(
@xml XML
)
AS
BEGIN
	DECLARE @handle INT
	EXEC sp_xml_preparedocument @handle OUTPUT, @xml
	CREATE TABLE #Temp_SAPISnapshotData
	(
		NirvanaFields VARCHAR(255),
		BBGMnemonic VARCHAR(MAX),
		Equity BIT,
		EquityOption BIT,
		Future BIT,
		FutureOption BIT,
		FX BIT,
		FixedIncome BIT,
		FXForward BIT
	)
	INSERT INTO #Temp_SAPISnapshotData
	(
		NirvanaFields,
		BBGMnemonic,
		Equity,
		EquityOption,
		Future,
		FutureOption,
		FX,
		FixedIncome,
		FXForward
	)
	SELECT
		NirvanaFields,
		BBGMnemonic,
		Equity,
		EquityOption,
		Future,
		FutureOption,
		FX,
		FixedIncome,
		FXForward
	FROM
	OPENXML(@handle, '//Table1', 2)
	with
	(
		NirvanaFields VARCHAR(255),
		BBGMnemonic VARCHAR(MAX),
		Equity BIT,
		EquityOption BIT,
		Future BIT,
		FutureOption BIT,
		FX BIT,
		FixedIncome BIT,
		FXForward BIT
	)

	UPDATE T_SAPISnapshotRequestField
	SET
		T_SAPISnapshotRequestField.Equity = T2.Equity,
		T_SAPISnapshotRequestField.EquityOption = T2.EquityOption,
		T_SAPISnapshotRequestField.Future = T2.Future,
		T_SAPISnapshotRequestField.FutureOption = T2.FutureOption,
		T_SAPISnapshotRequestField.FX = T2.FX,
		T_SAPISnapshotRequestField.FixedIncome = T2.FixedIncome,
		T_SAPISnapshotRequestField.FXForward = T2.FXForward

	FROM #Temp_SAPISnapshotData AS T2
	where T_SAPISnapshotRequestField.NirvanaFields = T2.NirvanaFields;

	EXEC sp_xml_removedocument @handle
END