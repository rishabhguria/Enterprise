-- Adding Security Fields

--SET IDENTITY_INSERT T_SecurityField ON;
--EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_SecurityField;

Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(1,	'Ask',			1,	1,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(2,	'Bid',			1,	1,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(3,	'Last',			1,	1,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(4,	'Mid',			1,	1,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(5,	'Beta',			1,	0,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(6,	'Close',		1,	1,	1,	1);
Insert into T_SecurityField(FieldID,FieldName,IsRealTime,IsHistorical,Esignal,Bloomberg) values(7,	'Avg_AskBid',	1,	1,	1,	1);

--EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
--SET IDENTITY_INSERT T_SecurityField OFF

