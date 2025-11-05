

CREATE procedure P_GetFundsForTheUser    
(    
  @userID int    
)    
As    
--select FundID    
--from t_userfundmapping    
--where UserID = @userID    
  

 Select CUF.CompanyFundID as FundId   
 From T_CompanyUser as U LEFT OUTER JOIN
 T_CompanyFunds as CF on U.CompanyID = CF.CompanyID LEFT OUTER JOIN
 T_CompanyUserFunds as CUF on CF.CompanyFundID = CUF.CompanyFundID AND U.UserID = CUF.CompanyUserID
Where U.userId = @userID AND CUF.CompanyFundID IS NOT NULL
and CF.IsActive=1
order by Cf.uiorder


-- Based on if user is of default company use fund group concept
-- Select DISTINCT CF.CompanyFundID as FundId   
-- From T_CompanyUser as U LEFT OUTER JOIN
-- T_CompanyFunds as CF LEFT OUTER JOIN
-- T_CompanyUserFunds as CUF on CF.CompanyFundID = CUF.CompanyFundID
--Where U.userId = @userID



-- Select CUF.CompanyFundID as  FundID       
-- From T_CompanyUserFunds CUF          
-- Where CUF.CompanyUserID = @userID 


