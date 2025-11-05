

CREATE  PROCEDURE dbo.P_SaveClientIdentifiers

	(
		@CompanyClientIdentifierID varchar(100),
		@CompanyClientID int,
		@IdentifierID int ,
		@Identifier varchar(50)
	)

AS
if ((select count(*) from T_CompanyClientIdentifier where CompanyClientIdentifierID=@CompanyClientIdentifierID)=0)
begin
insert into T_CompanyClientIdentifier
(CompanyClientIdentifierID,CompanyClientID,IdentifierID,Identifier)
values(@CompanyClientIdentifierID,@CompanyClientID,@IdentifierID,@Identifier)
end
else
begin
update T_CompanyClientIdentifier
set Identifier=@Identifier
where CompanyClientIdentifierID=@CompanyClientIdentifierID
end
