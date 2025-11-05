DELETE FROM T_TradeAudit WHERE Action in (48,49,50)

UPDATE T_TradeAudit
SET Action = Action - 3
WHERE Action > 50