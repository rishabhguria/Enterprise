--Created By: Disha Sharma
--Date: 11-07-2016
--Purpose: Script to update corrupted Fixed Allocation Schemes

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_AllocationScheme')
BEGIN
	UPDATE T_AllocationScheme
	SET AllocationScheme = cast( replace( replace( cast(AllocationScheme as nvarchar(max)), '<AccountName>', '<FundName>'), '</AccountName>', '</FundName>') as xml)
	WHERE AllocationScheme IS NOT NULL AND LTRIM(RTRIM(cast(AllocationScheme as nvarchar(max)))) <> ''

	UPDATE T_AllocationScheme
	SET AllocationScheme = cast( replace( replace( cast(AllocationScheme as nvarchar(max)), '<AccountID>', '<FundID>'), '</AccountID>', '</FundID>') as xml)
	WHERE AllocationScheme IS NOT NULL AND LTRIM(RTRIM(cast(AllocationScheme as nvarchar(max)))) <> ''
END