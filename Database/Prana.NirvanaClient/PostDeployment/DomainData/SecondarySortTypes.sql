SET IDENTITY_INSERT T_SecondarySortTypes ON;
delete from T_SecondarySortTypes

		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(0,'None');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(1,'Avg Px ASC');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(2,'Avg Px DESC');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(3,'Same Px Avg Px ASC');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(4,'Same Px Avg Px DESC');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(5,'Order Sequence ASC');
		INSERT INTO T_SecondarySortTypes(SecondarySortId, SecondarySortName) Values(6,'Order Sequence DESC');

SET IDENTITY_INSERT T_SecondarySortTypes OFF
