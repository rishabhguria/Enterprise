-- Adding Assets

SET IDENTITY_INSERT t_asset ON;
--EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";


delete from t_asset;

	
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(1,	'Equity',			NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(2,	'EquityOption',		NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(3,	'Future',			NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(4,	'FutureOption',		NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(5,	'FX',				NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(6,	'Cash',				NULL);
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(7,	'Indices',			NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(8,	'FixedIncome',		NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(9,	'PrivateEquity',	NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(10,	'FXOption',			NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(11,	'FXForward',		NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(12,	'Forex',			NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(13,	'ConvertibleBond',	NULL)
INSERT INTO T_ASSET(AssetId, AssetName, Comment)	VALUES(14,	'CreditDefaultSwap',NULL)



--EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
SET IDENTITY_INSERT t_asset OFF



-- Adding Assets to UDA Assets