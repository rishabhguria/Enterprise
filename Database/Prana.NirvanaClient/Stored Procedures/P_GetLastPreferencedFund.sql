

CREATE PROCEDURE P_GetLastPreferencedFund  
  
AS  
  
BEGIN  
  
SET NOCOUNT ON  
  
SELECT PreferencedFundID FROM T_ResidualQtyFund  
  
END

