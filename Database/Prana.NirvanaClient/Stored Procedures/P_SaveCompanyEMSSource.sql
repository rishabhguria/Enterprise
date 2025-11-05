
/*
select * from T_CompanyEMSSource
select * from T_ImportTrade
select * from T_FileData
P_SaveCompanyEMSSource '5', '2'
*/

CREATE PROC [dbo].[P_SaveCompanyEMSSource]
(
@companyID int,
@emsSourceID int
)

AS


if(( @companyID not in (select CompanyID from T_CompanyEMSSource where CompanyID = @companyID) )
OR
( @emsSourceID not in (select EMSSourceID from T_CompanyEMSSource where CompanyID = @companyID) ))
begin
INSERT INTO T_CompanyEMSSource(CompanyID, EMSSourceID)
						Values(@companyID, @emsSourceID)
end



