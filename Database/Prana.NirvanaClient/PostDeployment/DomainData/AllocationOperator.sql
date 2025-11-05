EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_AL_Operator;

	
INSERT INTO T_AL_Operator(Id, Operator, Description)	VALUES(1,'All',		'All of the check list will be included');
INSERT INTO T_AL_Operator(Id, Operator, Description)	VALUES(2,'Include',	'Only specified data will be included');
INSERT INTO T_AL_Operator(Id, Operator, Description)	VALUES(3,'Exclude',	'Only specified data will be excluded');


EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";