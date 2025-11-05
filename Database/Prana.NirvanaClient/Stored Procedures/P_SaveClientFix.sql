


CREATE  procedure P_SaveClientFix
(
@SenderCompID varchar(50),
@TargetCompID varchar(50),
@OnBehalfOfCompID varchar(50),
@IP varchar(50),
@Port int ,
@CompanyClientID int
)

as
declare @result int
if ((select count(*) from T_CompanyClientFIX where CompanyClientID=@CompanyClientID)=0)
begin
insert into T_CompanyClientFIX 
(CompanyClientID,
SenderCompID,
OnBehalfOfCompID,
TargetCompID,
IP,
Port)
values(@CompanyClientID,@SenderCompID,@OnBehalfOfCompID,@TargetCompID,@IP,@Port)
end
else

begin
update T_CompanyClientFIX 
set CompanyClientID=@CompanyClientID,
SenderCompID=@SenderCompID,
OnBehalfOfCompID=@OnBehalfOfCompID,
TargetCompID=@TargetCompID,
IP=@IP,
Port=@Port
where CompanyClientID=@CompanyClientID
end



set @result =scope_identity()



