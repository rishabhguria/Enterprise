/*
select * from T_CompanyEMSSource
select * from T_ImportTrade
select * from T_FileData
P_SaveCompanyEMSSource '5', '3'
*/
CREATE PROC [dbo].[P_DeleteCompanyEMSSource]
(
@companyID int,
@emsSourceID int
)

AS

DELETE FROM T_CompanyEMSSource
WHERE CompanyID = @companyID AND EMSSourceID = @emsSourceID




