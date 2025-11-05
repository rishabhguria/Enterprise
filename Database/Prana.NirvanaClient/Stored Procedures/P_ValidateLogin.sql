 /****** Object:  Stored Procedure dbo.P_ValidateLogin    Script Date: 11/17/2005 9:50:20 AM ******/  
  
/********************************************************************  
modified by omshiv  
description: validate user from T_Company user instead of T_user  
exec P_ValidateLogin @login='1',@password='1111'  
  
********************************************************************/  
CREATE PROCEDURE [dbo].[P_ValidateLogin]  
 (  
  @login  varchar(20),  
  @password varchar(20)    
 )  
AS   
--Declare @login  varchar(50)  
--Declare  @password varchar(50)  
--Set @login='1'  
--Set @password='1111'  
  
 Declare @Total int  
 --?? why we checking total again and again  
 Set @Total = 0;  
   
 Select @Total = Count(*) from T_CompanyUser where convert(varbinary, Password) = convert(varbinary, @password)  
    
 if(@Total > 0)  
 begin   
  
  Select @Total = Count(1)  
  From T_CompanyUser  
  Where Login = @login  
   And Password = @password and isActive=1  
    
  if(@total > 0)  
  begin  
            -- modified by omshiv, add role id of user in output  
   Select UserID,isnull(RoleID,0)as RoleID , CompanyID 
   From T_CompanyUser  
   Where Login = @login  
    And Password = @password and isActive=1  
  end   
  else  
  begin  
   Select @Total,0  
  end  
 end  
 else  
 begin  
  Select @Total,0  
 end   