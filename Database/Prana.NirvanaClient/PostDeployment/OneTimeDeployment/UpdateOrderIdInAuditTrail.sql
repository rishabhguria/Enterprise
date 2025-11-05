IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_TradeAudit' AND COLUMN_NAME = 'ParentOrderId')
	BEGIN
	Update T_TradeAudit 
		set ParentOrderId = GroupId ,
		OrderId = TaxlotId 
		where Source in (2,9,10) and ParentOrderId is null

	Update T_TradeAudit 
		set GroupId = '',
		TaxlotId = ''
		where Source in (2,9,10)
	END