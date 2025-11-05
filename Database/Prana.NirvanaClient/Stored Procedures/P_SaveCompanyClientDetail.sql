
/****** Object:  Stored Procedure dbo.P_SaveCompanyClientDetail    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyClientDetail
	(
		@companyID int,								--0
		@clientName varchar(50),					--1
		@mailingAddress1 varchar(50),				--2
		@mailingAddress2 varchar(50),				--3
		@companyType int,							--4
		@telephone varchar(50),						--5
		@fax varchar(50),							--6		
		@primaryContactFirstName varchar(50),		--7
		@primaryContactLastName varchar(50),		--8
		@primaryContactTitle varchar(50),			--9
		@primaryContactEmail varchar(50),			--10
		@primaryContactTelephone varchar(50),		--11
		@primaryContactCell varchar(50),			--12			
		@secondaryContactFirstName varchar(50),		--13
		@secondaryContactLastName varchar(50),		--14
		@secondaryContactTitle varchar(50),			--15
		@secondaryContactEmail varchar(50),			--16
		@secondaryContactTelephone varchar(50),		--17
		@secondaryContactCell varchar(50),			--18.
		
		@shortName varchar(50),			--19.
		@countryID int,			--20.
		@stateID int,			--21.
		@zip varchar(50),			--22.
		
		@companyClientID int						--23
	)
AS

Declare @result int
Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_CompanyClient
Where CompanyClientID = @companyClientID

if(@total > 0)
begin
	select @count = count(*)
		from T_CompanyClient
		Where ClientName = @clientName AND CompanyClientID <> @CompanyClientID AND CompanyID = @companyID
		if(@count = 0)
		begin
			--Update CompanyClient
			Update T_CompanyClient
			Set ClientName = @clientName,
				MailingAddress1 = @mailingAddress1, 
				MailingAddress2 = @mailingAddress2,
				CompanyType = @companyType,
				Telephone = @telephone,
				Fax = @fax,
				
				PrimaryContactFirstName = @primaryContactFirstName,
				PrimaryContactLastName = @primaryContactLastName,
				PrimaryContactTitle = @primaryContactTitle,
				PrimaryContactEmail = @primaryContactEmail,
				PrimaryContactTelephone = @primaryContactTelephone,
				PrimaryContactCell = @primaryContactCell,
				
				SecondaryContactFirstName = @secondaryContactFirstName,
				SecondaryContactLastName = @secondaryContactLastName,
				SecondaryContactTitle = @secondaryContactTitle,
				SecondryContactEmail = @secondaryContactEmail,
				SecondaryContactTelephone = @secondaryContactTelephone,
				SecondaryContactCell = @secondaryContactCell,
				CompanyID = @companyID,
				
				ShortName = @shortName,
				CountryID = @countryID,
				StateID = @stateID,
				Zip = @zip
				
				where CompanyClientID = @companyClientID
				Set @result = @companyClientID
			end
			else
			begin
				Set @result = -1
			end
end
else
begin
	select @count = count(*)
		from T_CompanyClient
		Where ClientName = @clientName AND CompanyID = @companyID
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
			--Insert
			Insert into T_CompanyClient(ClientName, MailingAddress1, MailingAddress2, CompanyType, Telephone, Fax,
			PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle, PrimaryContactEmail,
			PrimaryContactTelephone, PrimaryContactCell, SecondaryContactFirstName, SecondaryContactLastName, 
			SecondaryContactTitle, SecondryContactEmail, SecondaryContactTelephone, SecondaryContactCell, CompanyID,
			ShortName, CountryID, StateID, Zip)
			Values(@clientName, @mailingAddress1, @mailingAddress1, @companyType, @telephone, @fax, 
					@primaryContactFirstName, @primaryContactLastName, @primaryContactTitle, @primaryContactEmail, 
					@primaryContactTelephone, @primaryContactCell, @secondaryContactFirstName, @secondaryContactLastName, 
					@secondaryContactTitle, @secondaryContactEmail,	@secondaryContactTelephone, @secondaryContactCell, 
					@companyID, @shortName, @countryID, @stateID, @zip)
			
			Set @result = scope_identity()
		end
end
	Select @result
