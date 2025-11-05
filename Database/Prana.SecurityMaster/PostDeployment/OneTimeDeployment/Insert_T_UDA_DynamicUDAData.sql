--Created By: Disha Sharma
--Date: 07-08-2015
--Purpose: Script to add DynamicUDA XML to T_UDA_DynamicUDAData table 

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UDA_DynamicUDAData')
BEGIN
	INSERT INTO T_UDA_DynamicUDAData
	(
		Symbol_PK,
		FundID
	)
	SELECT	SM.Symbol_PK, 0
	FROM	T_SMSymbolLookUpTable SM WHERE SM.Symbol_PK NOT IN (SELECT Symbol_PK FROM T_UDA_DynamicUDAData)
END