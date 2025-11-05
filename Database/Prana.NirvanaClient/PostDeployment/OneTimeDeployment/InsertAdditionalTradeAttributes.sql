DECLARE @Counter INT = 7;
DECLARE @AttrValue VARCHAR(MAX);
DECLARE @AttrName VARCHAR(MAX);

WHILE @Counter <= 45
BEGIN
	-- AttributeValue always without leading zero
	SET @AttrValue = 'Trade Attribute ' + CAST(@Counter AS VARCHAR);

	-- AttributeName with leading zero for 7–9
	IF @Counter < 10
		SET @AttrName = 'Trade Attribute 0' + CAST(@Counter AS VARCHAR);
	ELSE
		SET @AttrName = @AttrValue;

	IF NOT EXISTS (
			SELECT 1
			FROM T_AttributeNames
			WHERE AttributeValue = @AttrValue
			)
	BEGIN
		INSERT INTO [dbo].[T_AttributeNames] (
			[AttributeValue]
			,[AttributeName]
			,[KeepRecord]
			,[DefaultValues]
			)
		VALUES (
			@AttrValue
			,@AttrName
			,1
			,NULL
			);
	END;

	SET @Counter = @Counter + 1;
END;