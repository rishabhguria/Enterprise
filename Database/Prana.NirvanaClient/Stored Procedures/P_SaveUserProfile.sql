CREATE PROCEDURE [dbo].[P_SaveUserProfile]
(
  @userID int,    
  @firstName varchar(50),    
  @lastName varchar(50),  
  @eMail varchar(50),   
  @address1 varchar(50),    
  @address2 varchar(50),   
  @countryID int,    
  @stateID int,
  @zip varchar(50),  
  @telphoneWork varchar(50),
  @telphoneMobile varchar(50)
)
AS

Declare @result int    
Declare @total int  
Set @total = 0 

Select @total = Count(*)    
From T_CompanyUser    
Where UserID = @userID 

if(@total > 0) 
begin
--Update CompanyUser
 Update T_CompanyUser     
   Set  FirstName = @firstName,
        LastName = @lastName,     
		EMail = @eMail, 
	    Address1 = @address1,    
        Address2 = @address2,  
	    CountryID = @countryID, 
		StateID=@stateID,
		Zip = @zip,
	    TelphoneWork = @telphoneWork, 
		TelphoneMobile = @telphoneMobile  
	   where UserID = @userID 
	   Set @result = @userID 
  end  
   else    
   begin    
    Set @result = -1    
   end    
select @result
