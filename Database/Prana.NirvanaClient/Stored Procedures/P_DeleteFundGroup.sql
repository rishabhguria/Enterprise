/****** Object:  Stored Procedure dbo.P_DeleteFundGroup    Script Date: 11/17/2005 9:50:24 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteFundGroup]  
 (  
  @fundGroupID int  
 )  
AS  
    
Declare @total int  
select @total = COUNT(*) from T_CompanyUserFundGroupMapping where FundGroupID = @fundGroupID  
         
IF(@total = 0)
BEGIN       
DELETE FROM T_FundGroups   
WHERE FundGroupID = @fundGroupID

DELETE FROM T_GroupFundMapping 
WHERE FundGroupID = @fundGroupID
END
