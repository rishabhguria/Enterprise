

/****** Object:  Stored Procedure dbo.P_SaveUserDetail    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE [dbo].[P_SaveUserDetail]
(
		@F_name varchar(50),
		@L_name varchar(50),
		@S_name varchar(50),
		@Title varchar(50),
		@E_mail varchar(50),
		@Tele_work varchar(50),
		@Tele_home varchar(50),
		@Tele_mobile varchar(50),
		@Tele_pager varchar(50),
		@login varchar(20),
		@password varchar(20),
		@address1 varchar(50),
		@address2 varchar(50),
		@countryID int,
		@stateID int,
		@zip varchar(50),
		@fax varchar(50),
		@userID int,
		@city varchar(50),
		@result int 
	)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_User
Where userID = @userID

if(@total > 0)
begin	
	select @count = count(*)
		from T_User
		Where (ShortName = @S_Name OR Login = @login) AND UserID <> @userID
		if(@count = 0)
	begin	
		Update T_User 
		Set FirstName = @F_name, 
			LastName = @L_name, 
			ShortName = @S_name, 
			Title = @Title, 
			Email = @E_mail, 
			TelphoneWork = @Tele_work, 
			TelphoneHome = @Tele_home,
			TelphoneMobile = @Tele_mobile, 
			TelPhonePager = @Tele_pager, 
			Login = @login, 
			Password = @password,
			Address1 = @address1, 
			Address2 = @address2, 
			CountryID = @countryID,
			StateID = @stateID,
			Zip = @zip,
			Fax = @fax,
			City = @city		
		Where userID = @userID
			--and userID <> 37	
		
		Set @result = @userID
	end
	else
	begin
		Set @result = -1
	end
end
else
begin
	select @count = count(*)
	from T_User 
	Where (ShortName = @S_Name OR Login = @login)
		
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_User (FirstName, LastName, ShortName, Title, Email, TelphoneWork, TelphoneHome,
               TelphoneMobile, TelphonePager, Login, Password, Address1, Address2, CountryID, StateID, Zip, Fax, City)
		Values(@F_name, @L_name, @S_name, @Title, @E_mail, @Tele_work,	@Tele_home,	@Tele_mobile,
			@Tele_pager, @login, @password,	@address1, @address2, @countryID, @stateID, @zip, @fax, @city)  
			
		Set @result = scope_identity()
	end
end
select @result

