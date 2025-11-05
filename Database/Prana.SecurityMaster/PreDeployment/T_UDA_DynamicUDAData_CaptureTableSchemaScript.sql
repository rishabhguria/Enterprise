/*
Created By: Disha Sharma
Purpose: This script is used to capture the current schema of T_UDA_DynamicUDAData table as it is different for each client based upon dynamic UDA columns 
*/
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UDA_DynamicUDAData_Temp')
BEGIN
	DROP TABLE T_UDA_DynamicUDAData_Temp
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UDA_DynamicUDAData')
BEGIN
	-- Created table T_UDA_DynamicUDAData_Temp to store schema as well as data of T_UDA_DynamicUDAData table before publishing
	SELECT * INTO T_UDA_DynamicUDAData_Temp FROM T_UDA_DynamicUDAData
END