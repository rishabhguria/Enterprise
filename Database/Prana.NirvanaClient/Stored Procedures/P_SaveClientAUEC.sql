


CREATE  PROCEDURE dbo.P_SaveClientAUEC

	(

@CompanyClientID int ,
@AUECID int
	)

AS
declare @CompanyAUECID int

select @CompanyAUECID = CompanyAUECID from T_CompanyAUEC where AUECID=@AUECID
if((select count(*) from T_CompanyClientAUEC where CompanyClientID=@CompanyClientID and CompanyAUECID=@CompanyAUECID)=0)
insert into T_CompanyClientAUEC(CompanyClientID,CompanyAUECID)  values(@CompanyClientID,@CompanyAUECID)





