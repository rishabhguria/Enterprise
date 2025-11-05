  
   /****** Object:  Stored Procedure dbo.P_SaveLogo    Script Date: 02/10/2006 4:30:22 PM ******/  
CREATE PROCEDURE [dbo].[P_SaveLogo]  
 (  
  @logoID    int,   
  @logoName    varchar(50),   
  @logoImage    image   
 )  
AS  
  
  
Declare @result int  
Declare @total int   
Set @total = 0  
  
Select @total = Count(*)  
From T_Logo  
Where LogoID = @logoID  
if(@total > 0)   
begin  
 --Update  
 Update T_Logo  
 Set LogoName = @logoName,   
  LogoImage = @logoImage  
    
 Where LogoID = @logoID  
 Set @result = @logoID  
end  
else  
begin  
 --Insert  
 --Insert into T_NirvanaLogo
--	values(@logoName, @logoImage)	
 Insert into T_Logo(LogoName, LogoImage)  
 Values(@logoName, @logoImage)  
                      
   Set @logoID = scope_identity()  
end  
 Select @logoID  
   
  
  
  