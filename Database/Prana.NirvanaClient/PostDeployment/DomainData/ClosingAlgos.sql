--SET IDENTITY_INSERT T_ClosingAlgos ON;
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_ClosingAlgos;

	
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(0,	'NONE',		'NONE');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(1,	'LIFO',		'Last In First Out');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(2,	'FIFO',		'First In First Out');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(3,	'HIFO',		'Highest In First Out');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(4,	'MFIFO',	'Modified First In First Out');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(5,	'PRESET',	'Algo according to closing preferences');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(6,	'ETM',		'Efficient Tax Methodology');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(7,	'LOWCOST',	'Low Cost First');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(8,	'ACA',		'ACA');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(9,	'HIHO',		'Highest In Highest Out');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(10,	'BTAX',		'Best Tax Methodology');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(11,	'TAXADV',	'Tax Advantage');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(12,	'MANUAL',	'MANUAL');
		INSERT INTO T_ClosingAlgos(AlgorithmId, AlgorithmAcronym, AlgorithmDesc) VALUES(13,	'HCST',		'High cost short term');



EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";
--SET IDENTITY_INSERT T_ClosingAlgos OFF

