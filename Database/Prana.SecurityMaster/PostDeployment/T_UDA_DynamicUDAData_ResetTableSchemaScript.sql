/*
Created By: Disha Sharma
Purpose: This script is used to set the schema of T_UDA_DynamicUDAData table from the temp table created before deployment
*/

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UDA_DynamicUDAData')
BEGIN
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UDA_DynamicUDAData_Temp')
	BEGIN
		DROP TABLE T_UDA_DynamicUDAData

		-- Created table T_UDA_DynamicUDAData_Temp to store schema as well as data of T_UDA_DynamicUDAData table before publishing
		SELECT * INTO T_UDA_DynamicUDAData FROM T_UDA_DynamicUDAData_Temp

		-- Drop temp table
		DROP TABLE T_UDA_DynamicUDAData_Temp
	END
END

