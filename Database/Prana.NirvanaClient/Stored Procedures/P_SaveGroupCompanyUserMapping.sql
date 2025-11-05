CREATE PROC [dbo].[P_SaveGroupCompanyUserMapping]          
(      
  @XMLDoc nText
 , @XMLTradingDoc nText
 , @UserID int
 , @FirstName varchar(50)
 , @LastName varchar(50) 
 , @Login varchar(50)  
 , @Password varchar(50)
 , @ShortName varchar(50)
 , @EMail varchar(50) 
 , @CompanyID int
 , @Region varchar(50) 
 , @RoleID int 
 , @IsAllGroupsAccess bit      
 , @ErrorMessage varchar(500) output                        
 , @ErrorNumber int output                        
 )               
AS          
      
SET @ErrorMessage = 'Success'                        
SET @ErrorNumber = 0                                    
      
DECLARE @handle int 
DECLARE @handle1 int  
DECLARE @CmpUserID int                         
exec sp_xml_preparedocument @handle OUTPUT,@XMLDoc 
exec sp_xml_preparedocument @handle1 OUTPUT,@XMLTradingDoc            
      
 CREATE TABLE #TempTableNames                                                                               
  (                                                                               
    CompanyUserId int,      
	GroupId int                   
   )        
      
INSERT INTO #TempTableNames                     
 (                                                                              
    CompanyUserId                        
   ,GroupId                                                       
 )                                                                              
SELECT                                                                               
	CompanyUserId                        
   ,GroupId                                      
    FROM OPENXML(@handle, '/DSGroupCompanyUserMapping/TABGroupCompanyUserMapping', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyUserId int,      
   GroupId int              
 ) 

CREATE TABLE #TempTradingMapping                                                                              
  (                                                                               
    CompanyUserId int,      
	TradingAccountId int                   
   )        
      
INSERT INTO #TempTradingMapping                     
 (                                                                              
    CompanyUserId                        
   ,TradingAccountId                                                       
 )                                                                              
SELECT                                                                               
	CompanyUserId                        
   ,TradingAccountId                                      
    FROM OPENXML(@handle1, '/DSTradingAccountsUserMapping/TABTradingAccountsUserMapping', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyUserId int,      
   TradingAccountId int              
 )                                         

IF (Select count(*) from T_CompanyUser where UserId = @userID ) > 0
BEGIN
UPDATE T_CompanyUser SET FirstName=@FirstName,LastName=@LastName,Login=@Login,Password=@Password,ShortName=@ShortName,
EMail=@EMail,CompanyID=@CompanyID,Region=@Region,RoleID=@RoleID,IsAllGroupsAccess=@IsAllGroupsAccess where UserID = @userID

delete from T_CompanyUserFundGroupMapping 
where CompanyUserId = @UserID

Insert into T_CompanyUserFundGroupMapping (CompanyUserId,FundGroupId) select CompanyUserId,GroupId from #TempTableNames

delete from T_CompanyUserTradingAccounts 
where CompanyUserId = @UserID

Insert into T_CompanyUserTradingAccounts (CompanyUserId,TradingAccountID) select CompanyUserId,TradingAccountID from #TempTradingMapping

Set @ErrorNumber = 1
END
ELSE IF (Select count(*) FROM T_COMPANYUSER WHERE ShortName = @ShortName) > 0
BEGIN
Set @ErrorNumber = -5
END
ELSE IF (Select count(*) FROM T_COMPANYUSER WHERE Login = @Login) > 0
BEGIN
Set @ErrorNumber = -7
END
ELSE
BEGIN
Insert INTO T_CompanyUser(FirstName,LastName,Login,Password,ShortName,EMail,CompanyID,Region,RoleID,IsAllGroupsAccess,
Address1,CountryID,StateID,TelphoneWork,TelphoneMobile,TradingPermission)
VALUES(@FirstName,@LastName,@Login,@Password,@ShortName,@EMail,@CompanyID,@Region,@RoleID,@IsAllGroupsAccess,'NA',0,0,0,0,0)

--select @CmpUserID=MAX(userID) from T_CompanyUser
Insert into T_CompanyUserFundGroupMapping (CompanyUserId,FundGroupId) select @UserID,GroupId from #TempTableNames

Insert into T_CompanyUserTradingAccounts (CompanyUserId,TradingAccountID) select @UserID,TradingAccountID from #TempTradingMapping

END
SELECT @ErrorNumber
EXEC sp_xml_removedocument @handle  
EXEC sp_xml_removedocument @handle1

