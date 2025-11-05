

CREATE Proc [dbo].[PMGetAllDataSourceFundByCompanyFund]
As
Select 
	distinct CompanyFundID,
	DataSourceCompanyFundID,
	DataSourceFundName
From
	PM_DataSourceCompanyFund
