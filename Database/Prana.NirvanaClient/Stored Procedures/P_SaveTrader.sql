


/****** Object:  Stored Procedure dbo.P_SaveTrader    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_SaveTrader
(
		@traderID int, 
		@eMail varchar(50), 
		@fax varchar(50), 
		@firstName varchar(50), 
		@lastName varchar(50), 
		@pager varchar(50), 
		@shortName varchar(50), 
		@telephoneCell varchar(50), 
		@telephoneHome varchar(50), 
		@telephoneWork varchar(50), 
		@title varchar(50),
		@companyClientID int
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyClientTrader
Where TraderID = @traderID

if(@total > 0)
begin	
	--Update Trader
	Update T_CompanyClientTrader 
	Set FirstName = @firstName, 
		LastName = @lastName,
		ShortName = @shortName,
		Title = @title,
		EMail = @eMail,
		TelephoneWork = @telephoneWork,
		TelephoneCell = @telephoneCell,
		Pager = @pager,
		TelephoneHome = @telephoneHome,
		Fax = @fax,
		CompanyClientID = @companyClientID
		
	Where TraderID = @traderID
	
	Set @result = @traderID
end
else
--Insert Trader
begin
	INSERT T_CompanyClientTrader(FirstName, LastName, ShortName, Title, EMail, TelephoneWork, TelephoneCell, 
				Pager, TelephoneHome, Fax, CompanyClientID)
	Values(@firstName, @lastName, @shortName, @title, @eMail, @telephoneWork, @telephoneCell, 
			@pager, @telephoneHome, @fax, @companyClientID)
	
	Set @result = scope_identity()
		--	end
end
select @result


