GO

IF not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyFunds' AND COLUMN_NAME = 'StartDate')
BEGIN
	alter table T_CompanyFunds
add  StartDate datetime null
END
GO

IF not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyMasterFunds' AND COLUMN_NAME = 'GroupTypeId')
BEGIN
	alter table T_CompanyMasterFunds
add  GroupTypeId int 
END

GO