IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_AssetSide'
			AND COLUMN_NAME = 'AssetID'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_AssetSide'
			AND COLUMN_NAME = 'SideID'
		)
BEGIN
     TRUNCATE table T_AssetSide

	DECLARE @assetID INT
	DECLARE @sideID INT

	---Equity Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Equity'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell short'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---Equity Sides End
	---EquityOption Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'EquityOption'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---EquityOption Sides End
	---Futures Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Future'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---Futures Sides End
	---FutureOption Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'FutureOption'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---FutureOption Sides End
	---FX Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'FX'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---FX Sides End
	---Equity Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Indices'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell short'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---Equity Sides End
	---FX Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'FixedIncome'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---FX Sides End
	---PrivateEquity Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'PrivateEquity'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell short'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---PrivateEquity Sides End
	---FX Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'FXForward'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---FX Sides End
	---CreditDefaultSwap Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'CreditDefaultSwap'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell short'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---CreditDefaultSwap Sides End
	---CreditDefaultSwap Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'ConvertibleBond'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell short'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)
		---CreditDefaultSwap Sides End

		---Futures Sides End
	---FutureOption Sides Begin
	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'FXOption'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Open'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell to Close'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	---FutureOption Sides End

	SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Cash'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

    SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Forex'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)

	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Sell'
				)
			)

	INSERT INTO T_AssetSide (
		AssetID
		,SideID
		)
	VALUES (
		@assetID
		,@sideID
		)
END
