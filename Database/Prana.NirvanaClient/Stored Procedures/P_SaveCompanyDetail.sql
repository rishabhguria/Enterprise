-----------------------------------------------------------------
--Modified BY: Manvendra
--Date: 4-JUNE-2015
--Purpose: Remove Secondary and Technology Contact fields
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_SaveCompanyDetail]
	(
		@CompanyID				int, 
		@Address1				varchar(100), 
		@Address2				varchar(100), 
		@CompanyTypeID			int,	
		@Fax					varchar(20), 
		@Name					varchar(50), 
		@PrimaryContactCell		 varchar(50), 
		@PrimaryContactEMail	 varchar(50), 
		@PrimaryContactFirstName varchar(50), 
		@PrimaryContactLastName	 varchar(50), 
		@PrimaryContactTelephone varchar(50), 
		@PrimaryContactTitle	 varchar(50), 
		
		@Telephone				varchar(50),
		@ShortName				varchar(50),
		@Login				varchar(50),
		@Password				varchar(50),
		@CountryID				int,
		@StateID				int,
		@Zip				varchar(50),
		@BaseCurrencyID				int,
		@SupportsMultipleCurrencies				int,
		@City				varchar(50),
		@Region varchar(50),
        @TimeZone int,
        @EmailAlert				varchar(500),
		@SendAllocationsViaFix	BIT
	)
AS
Declare @result int
Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Company 
Where CompanyID = @CompanyID

if(@total > 0) 
begin
	select @count = count(*)
		from T_Company
		Where Name = @Name AND CompanyID <> @companyID
		if(@count = 0)
		begin
			--Update
			Update T_Company
			Set Name = @Name, 
				Address1 = @Address1, 
				Address2 = @Address2, 
				CompanyTypeID = @CompanyTypeID, 
				Telephone = @Telephone, 
				Fax = @Fax, 
				PrimaryContactFirstName = @PrimaryContactFirstName, 
				PrimaryContactLastName = @PrimaryContactLastName, 
				PrimaryContactTitle = @PrimaryContactTitle, 
				PrimaryContactEMail = @PrimaryContactEMail, 
				PrimaryContactTelephone = @PrimaryContactTelephone, 
				PrimaryContactCell = @PrimaryContactCell, 
				
				
				ShortName = @ShortName,
				Login = @Login,
				Password = @Password,
				CountryID = @CountryID,
				StateID = @StateID,
				Zip = @Zip,
				BaseCurrencyID = @BaseCurrencyID,
				SupportsMultipleCurrencies = @SupportsMultipleCurrencies,
				City = @City,
                region=@region,
                TimeZone = @TimeZone,
                AlertEmails = @EmailAlert,
				SendAllocationsViaFix = @SendAllocationsViaFix
			Where CompanyID = @CompanyID
			Set @result = @CompanyID
			end
			else
			begin
				Set @result = -1
			end
end
else
begin
	select @count = count(*)
		from T_Company 
		Where Name = @Name
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
			--Insert
			Insert into T_Company(Name, Address1, Address2, CompanyTypeID, Telephone, Fax, 
							PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle, 
							PrimaryContactEMail, PrimaryContactTelephone, PrimaryContactCell,
							ShortName, Login, Password, CountryID, StateID, Zip, BaseCurrencyID, SupportsMultipleCurrencies, City, region, TimeZone, AlertEmails, SendAllocationsViaFix)
			Values(@Name, @Address1, @Address2, @CompanyTypeID, @Telephone, @Fax, 
							@PrimaryContactFirstName, @PrimaryContactLastName, @PrimaryContactTitle, 
							@PrimaryContactEMail, @PrimaryContactTelephone, @PrimaryContactCell,
							@ShortName, @Login, @Password, @CountryID, @StateID, @Zip, @BaseCurrencyID, @SupportsMultipleCurrencies, @City, @region, @TimeZone, @EmailAlert, @SendAllocationsViaFix)
		                    
		   Set @result = scope_identity()
		end
		
end
	Select @result
