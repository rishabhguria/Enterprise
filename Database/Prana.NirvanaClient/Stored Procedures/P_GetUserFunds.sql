


/****** Object:  Stored Procedure dbo.P_GetUserFunds    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetUserFunds
	(
		@userID int	
	)
AS
	  SELECT T_CompanyFunds.CompanyFundID,T_CompanyFunds.FundShortName  
from T_CompanyUserFunds join     
T_CompanyFunds     
on    
T_CompanyFunds.CompanyFundID=T_CompanyUserFunds.CompanyFundID    
where     
T_CompanyUserFunds.CompanyUserID=@userID  
  
order by T_CompanyFunds.UIOrder


