-- =============================================  
-- Created by: Faisal Shah
-- Date: 26 Dec 2014
-- Purpose: Get User permitted Companies on the basis of Fund Groups
-- =============================================
CREATE PROCEDURE [dbo].[P_UserPermittedCompanies]   
(  
@UserID int  
)  
AS  

Declare @RoleID int  
select @RoleID=RoleID from T_CompanyUser where UserID = @UserID  
--For admin and system admin show data for all the funds  
IF ((@RoleID = 3) or (@RoleID = 4))  
BEGIN  
 
SELECT T_Company.CompanyID AS CompanyID,T_Company.Name as CompanyName
      FROM  T_CompanyFunds INNER JOIN    
    T_Company ON T_Company.CompanyID = T_CompanyFunds.CompanyID    where  T_CompanyFunds.IsActive = 1 

END  
ELSE  
BEGIN  
 SELECT Distinct T_Company.CompanyID AS CompanyID,T_Company.Name as CompanyName
    FROM  T_CompanyUserFundGroupMapping INNER JOIN    
    T_CompanyUser ON T_CompanyUserFundGroupMapping.CompanyUserID = T_CompanyUser.UserID INNER JOIN    
    T_GroupFundMapping ON T_CompanyUserFundGroupMapping.FundGroupID = T_GroupFundMapping.FundGroupID INNER JOIN    
    T_CompanyFunds ON T_GroupFundMapping.FundID = T_CompanyFunds.CompanyFundID INNER JOIN    
    T_Company ON T_Company.CompanyID = T_CompanyFunds.CompanyID    
    where  T_CompanyUser.UserID = @UserID and  T_CompanyFunds.IsActive = 1
	
	
END
