  
  
/****** Object:  Stored Procedure dbo.P_SaveCompanyUser    Script Date: 11/17/2005 9:50:23 AM ******/  
CREATE PROCEDURE [dbo].[P_SaveCompanyUser]    
(    
  @userID varchar(50),    
  @lastName varchar(50),    
  @firstName varchar(50),    
  @shortName varchar(50),    
  @title varchar(50),    
  @eMail varchar(50),    
  @factSetUsernameAndSerialNumber varchar(200),
  @telphoneWork varchar(50),    
  @telphoneHome varchar(50),    
  @telphoneMobile varchar(50),    
  @fax varchar(50),    
  @login varchar(50),    
  @password varchar(100),    
  @telphonePager varchar(50),    
  @address1 varchar(50),    
  @address2 varchar(50),    
  @countryID int,    
  @stateID int,    
  @zip varchar(50),    
  @companyID int,    
  @tradingPermission int,    
  @city varchar(50),
  @isFactSetSupportUser bit,
  @activUsername varchar(200),
  @activPassword varchar(200),
  @samsaraAzureId varchar(200),
  @sapiUsername varchar(200)
)    
AS     
Declare @result int    
Declare @total int     
Set @total = 0    
declare @count int    
set @count = 0    
Declare @oldlogin varchar(50)

select @oldlogin = login
From T_CompanyUser
Where UserID = @userID    
    
Select @total = Count(*)    
From T_CompanyUser    
Where UserID = @userID    
    
if(@total > 0)    
begin     
 select @count = count(*)    
  from T_CompanyUser    
  Where ShortName = @shortName AND UserID <> @userID AND CompanyID = @companyID    
  if(@count = 0)    
  begin    
   --Update CompanyUser    
   Update T_CompanyUser     
   Set LastName = @lastName,     
    FirstName = @firstName,    
    ShortName = @shortName,    
    Title = @title,    
    EMail = @eMail,    
	[FactSetUsernameAndSerialNumber] = @factSetUsernameAndSerialNumber,
    TelphoneWork = @telphoneWork,    
    TelphoneHome = @telphoneHome,    
    TelphoneMobile = @telphoneMobile,    
    Fax = @fax,    
    Login = @login,     
    TelphonePager = @telphonePager,    
    Address1 = @address1,    
    Address2 = @address2,    
    CountryID = @countryID,    
    StateID = @stateID,    
    Zip = @zip,    
    CompanyID = @companyID,    
    TradingPermission = @tradingPermission,    
    City = @city,
	IsFactSetSupportUser = @isFactSetSupportUser,
	ActivUsername = @activUsername,
	ActivPassword = @activPassword,
    SamsaraAzureId = @samsaraAzureId,
    SapiUsername = @sapiUsername
   Where UserID = @userID     
   Set @result = @userID    
   if(@password != '')     
	begin
		Update T_CompanyUser     
		set Password = @password
		Where UserID = @userID   
	end    
  end    
  else    
   begin    
    Set @result = -1    
   end    
end    
else    
--Insert CompanyUser    
begin    
 select @count = count(*)    
  from T_CompanyUser    
  Where ShortName = @shortName AND CompanyID = @companyID    
      
  if(@count > 0)    
  begin    
       
   Set @result = -1    
  end    
  else    
  begin    
   INSERT T_CompanyUser(LastName, FirstName, ShortName, Title, EMail, [FactSetUsernameAndSerialNumber], TelphoneWork, TelphoneHome,    
         TelphoneMobile, Fax, Login, Password, TelphonePager, Address1, Address2,    
         CountryID, StateID, Zip, CompanyID, TradingPermission, City, IsFactSetSupportUser, ActivUsername, ActivPassword, SamsaraAzureId, RoleID, SapiUsername) --RoleID=4 hardcoded because all users in Prana mode should be made as super admin
   Values(@lastName, @firstName, @shortName, @title, @eMail, @factSetUsernameAndSerialNumber, @telphoneWork, @telphoneHome,    
     @telphoneMobile, @fax, @login, @password, @telphonePager, @address1, @address2, @countryID,     
     @stateID, @zip, @companyID, @tradingPermission, @city, @isFactSetSupportUser, @activUsername, @activPassword, @samsaraAzureId, 4, @sapiUsername)    
       
   Set @result = scope_identity()   
   
  end     
end    
-- Check Whether a Default Layout Exists for a new User in T_Layout  
IF not exists(SELECT * FROM T_Layout where UserID=@result AND LayoutName='DEFAULT')  
BEGIN  
 INSERT T_Layout(LayoutName , UserID )  
   VALUES ('DEFAULT',@result)  
end  
select @result,@oldlogin
