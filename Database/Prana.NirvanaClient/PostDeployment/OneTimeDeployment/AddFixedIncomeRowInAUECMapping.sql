IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_AUECMapping')
BEGIN
	IF (SELECT COUNT(*) FROM T_AUECMapping M INNER JOIN T_AUEC A ON A.AUECID = M.AUECID WHERE AssetID = 8) = 0
		INSERT INTO T_AUECMapping (AUECID, ExchangeIdentifier, TranslateRoot, TranslateType, EsignalExchangeCode)
		SELECT AUECID, ExchangeIdentifier, 0, 0, 'OTC' from T_AUEC WHERE AssetID = 8
	ELSE
		UPDATE AM
		SET AM.EsignalExchangeCode = 'OTC'
		FROM T_AUECMapping AM
		INNER JOIN T_AUEC A ON A.AUECID = AM.AUECID AND A.AssetID = 8
END
