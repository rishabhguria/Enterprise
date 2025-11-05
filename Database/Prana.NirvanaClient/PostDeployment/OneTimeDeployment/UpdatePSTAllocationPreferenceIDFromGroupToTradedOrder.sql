IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_Group'
			AND COLUMN_NAME = 'TransactionSource'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_TradedOrders'
			AND COLUMN_NAME = 'TransactionSource'
		)
BEGIN
	UPDATE T_TradedOrders
	SET TransactionSource = grp.TransactionSource
	FROM T_Group grp
	JOIN T_TradedOrders orders
		ON grp.GroupID = orders.GroupID
END
