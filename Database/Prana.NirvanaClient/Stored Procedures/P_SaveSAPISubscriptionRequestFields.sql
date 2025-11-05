CREATE PROCEDURE [dbo].[P_SaveSAPISubscriptionRequestFields]
(
@xml XML
)
AS
BEGIN
	DECLARE @handle INT
	EXEC sp_xml_preparedocument @handle OUTPUT, @xml
	CREATE TABLE #Temp_SAPISubscriptionData
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
	INSERT INTO #Temp_SAPISubscriptionData
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

	UPDATE T_SAPISubscriptionRequestField
	SET
		T_SAPISubscriptionRequestField.Equity = T2.Equity,
		T_SAPISubscriptionRequestField.EquityOption = T2.EquityOption,
		T_SAPISubscriptionRequestField.Future = T2.Future,
		T_SAPISubscriptionRequestField.FutureOption = T2.FutureOption,
		T_SAPISubscriptionRequestField.FX = T2.FX,
		T_SAPISubscriptionRequestField.FixedIncome = T2.FixedIncome,
		T_SAPISubscriptionRequestField.FXForward = T2.FXForward

	FROM #Temp_SAPISubscriptionData AS T2
	where T_SAPISubscriptionRequestField.NirvanaFields = T2.NirvanaFields;

	EXEC sp_xml_removedocument @handle
END
