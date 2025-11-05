

/****** Object:  Stored Procedure dbo.P_SaveCompanyLogo    Script Date: 01/23/2008 5:38:22 PM ******/    
CREATE PROCEDURE [dbo].[P_SaveCompanyLogo] (    
  @logoID    int,     
  @logoName    varchar(50),     
  @logoImage    image     
 )    
AS    
    
    
Declare @result int    
Declare @total int     
Set @total = 0    
    
Select @total = Count(*)    
From T_CompanyLogo    
Where LogoID = @logoID    
if(@total > 0)     
begin    
 --Update    
 Update T_CompanyLogo    
 Set LogoName = @logoName,     
  Logo = @logoImage    
      
 Where LogoID = @logoID    
 Set @result = @logoID    
end    
else    
begin    
 --Insert    
 --Insert into T_NirvanaLogo  
-- values(@logoName, @logoImage) 
 Delete FROM T_CompanyLogo Where LogoID <> 0  
 Insert into T_CompanyLogo(LogoName, Logo)    
 Values(@logoName, @logoImage)    
                        
   Set @logoID = scope_identity()    
end    
 Select @logoID
