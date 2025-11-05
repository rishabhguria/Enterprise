
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: When user tries to save a new third party, and it is present in the DB but in INACTIVE state,  set value of 'result' so that appropriate message can be given to the 
user.
*/        
CREATE PROCEDURE [dbo].[P_SaveThirdPartyDetail] 
(      
  @thirdPartyName varchar(50),      
  @description varchar(50),      
  @thirdPartyTypeID int,      
  @shortName varchar(50),      
  @contactPerson varchar(50),      
  @address1 varchar(50),      
  @address2 varchar(50),      
  @cellPhone varchar(50),      
  @workTelephone varchar(50),      
  @fax varchar(50),      
  @email varchar(20),      
  @countryID int,      
  @stateID int,      
  @zip varchar(50),      
  @contactPersonLastName varchar(50),      
  @contactPersonTitle varchar(50),      
  @contactPersonFax varchar(50),      
  @contactPersonWorkPhone varchar(50),      
  @thirdPartyID int,    
  @securityIdentifierTypeID  int,    
--  @delimiter varchar(5),      
--  @delimiterName varchar(20),       
--  @fileExtension varchar(20),       
  @result int,
  @brokerCode varchar(50),  
  @counterPartyID int
 )      
AS       
      
Declare @total int       
Set @total = 0      
declare @count int      
set @count = 0      
declare @count1 int      
set @count1 = 0      
      
Select @total = Count(*)      
From T_ThirdParty      
Where ThirdPartyID = @thirdPartyID      
      
if(@total > 0)      
begin       
 select @count = count(*)      
  from T_ThirdParty      
  Where ShortName = @shortName AND ThirdPartyID <> @thirdPartyID AND ThirdPartyTypeID = @thirdPartyTypeID      
  if(@count = 0)      
  begin      
   Update T_ThirdParty       
   Set ThirdPartyName = @thirdPartyName,       
    Description = @description,       
    ThirdPartyTypeID = @thirdPartyTypeID,       
    ShortName = @shortName,       
    ContactPerson = @contactPerson,       
    Address1 = @address1,       
    Address2 = @address2,       
    CellPhone = @cellPhone,      
    WorkTelephone = @workTelephone,       
    Fax = @fax,       
    Email = @email,      
    CountryID = @countryID,      
    StateID = @stateID,      
    Zip = @zip,      
    ContactPersonLastName = @contactPersonLastName,      
    ContactPersonTitle = @contactPersonTitle,      
    ContactPersonFax = @contactPersonFax,      
    ContactPersonWorkPhone = @contactPersonWorkPhone,     
    SecurityIdentifierTypeID=@securityIdentifierTypeID,
    BrokerCode = @brokerCode,     
    CounterPartyID = @counterPartyID
--    Delimiter = @delimiter,     
--    DelimiterName = @delimiterName,  
--    FileExtension=@fileExtension  
           
   Where ThirdPartyID = @thirdPartyID      
   Set @result = @thirdPartyID      
  end      
  else      
  begin      
   Set @result = -1      
  end      
end      
else      
begin      
 select @count = count(*)      
 from T_ThirdParty       
 Where ShortName = @shortName AND ThirdPartyTypeID = @thirdPartyTypeID  AND isActive = 1

select @count1 = count(*)      
 from T_ThirdParty       
 Where ShortName = @shortName AND ThirdPartyTypeID = @thirdPartyTypeID  AND isActive = 0 
      
 if(@count > 0)      
 begin      
  Set @result = -1      
 end 
    

 else  if(@count1 > 0)      
 begin      
  Set @result = -2
 end

 else      
 begin      
  INSERT T_ThirdParty(ThirdPartyName, Description, ThirdPartyTypeID, ShortName, ContactPerson, Address1,      
    Address2, CellPhone, WorkTelephone, Fax, Email, CountryID, StateID, Zip, ContactPersonLastName,       
    ContactPersonTitle, ContactPersonFax, ContactPersonWorkPhone,SecurityIdentifierTypeID,BrokerCode,CounterPartyID)--,Delimiter,DelimiterName,FileExtension)      
  Values(@thirdPartyName, @description, @thirdPartyTypeID, @shortName, @contactPerson, @address1, @address2,      
   @cellPhone, @workTelephone, @fax, @email, @countryID, @stateID, @zip, @contactPersonLastName,       
   @contactPersonTitle, @contactPersonFax, @contactPersonWorkPhone,@securityIdentifierTypeID,@brokerCode, @counterPartyID)--,@delimiter,@delimiterName,@fileExtension)        
         
  Set @result = scope_identity()      
 end      
end      
select @result 

