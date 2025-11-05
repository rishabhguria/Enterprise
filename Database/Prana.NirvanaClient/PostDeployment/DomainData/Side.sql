SET IDENTITY_INSERT T_SIDE ON;
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_SIDE;


INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(1,	'Buy',				'1',1)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(2,	'Sell',				'2',1)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(3,	'Buy minus',		'3',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(4,	'Sell plus',		'4',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(5,	'Sell short',		'5',1)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(6,	'Sell short exempt','6',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(7,	'Cross',			'8',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(8,	'Cross short',		'9',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(9,	'Buy to Open',		'A',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(10,	'Buy to Close',		'B',1)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(11,	'Sell to Open',		'C',0)
INSERT INTO T_SIDE(SideID, Side, SideTagValue, IsBasicSide) VALUES(12,	'Sell to Close',	'D',0)



EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
SET IDENTITY_INSERT T_SIDE OFF

