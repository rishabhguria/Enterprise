


CREATE  PROCEDURE dbo.P_SaveCompanyClientTradingAccount

	(	@CompanyClientTradingAccountID int,
		@CompanyClientTradingAccount varchar(50),
		@CompanyTradingAccountID int,
		@CompanyClientID int,
		@ClientTraderID varchar(50)
	)
AS
declare @result int 
declare @Counter int 
begin
select @Counter=count(*)  from 	T_CompanyClientTradingAccount
where  CompanyClientTradingAccount=@CompanyClientTradingAccount 
and CompanyClientID= @CompanyClientID and ClientTraderID=@ClientTraderID
if( @Counter != 1)
begin
insert into T_CompanyClientTradingAccount(CompanyClientTradingAccount,CompanyTradingAccountID,
CompanyClientID,ClientTraderID)
values(@CompanyClientTradingAccount,@CompanyTradingAccountID,@CompanyClientID,@ClientTraderID)
end
else 
begin

update  T_CompanyClientTradingAccount

set
CompanyClientTradingAccount=@CompanyClientTradingAccount,
CompanyTradingAccountID=@CompanyTradingAccountID,
CompanyClientID=@CompanyClientID,
ClientTraderID=@ClientTraderID

where  CompanyClientTradingAccount=@CompanyClientTradingAccount 
and CompanyClientID= @CompanyClientID and ClientTraderID=@ClientTraderID


end 

Set @result = scope_identity()

Select @result
end	


