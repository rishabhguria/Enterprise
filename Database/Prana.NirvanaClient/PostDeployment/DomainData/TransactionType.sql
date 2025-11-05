--SET IDENTITY_INSERT T_TransactionType ON;
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_TransactionType;

	
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(1,'Non-Trade Transaction','NTRA');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(2,'Accrued Balance','ACB');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(3,'Trade Transaction','TRA');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(4,'Investor Cash Transactions','ICT');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(5,'Fund Transfer','FundTransfer');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(6,'Account Transfer','AccountTransfer');
		INSERT INTO T_TransactionType(TransactionTypeId,TransactionType,TransactionTypeAcronym) VALUES(7,'Cash','Cash');


EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
--SET IDENTITY_INSERT T_TransactionType OFF

