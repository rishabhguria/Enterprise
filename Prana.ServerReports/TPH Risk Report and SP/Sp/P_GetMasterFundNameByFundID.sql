/*********************************************          
[P_GetMasterFundNameByFundID] '40'       
      
Author: Sachin Mishra    
Date: 12 oct 2015    
Decsription: Get Master FundName using FundID    
********************************************/    
Create PROCEDURE [dbo].[P_GetMasterFundNameByFundID]   
(@FundID VARCHAR(max))    
AS    
    
DECLARE @FundS TABLE (FundID INT)    
    
INSERT INTO @FundS    
SELECT Cast(Items AS INT)    
FROM dbo.Split(@FundID, ',')    
    
------Get all Master FundName -------    
    
    
SELECT DISTINCT MasterFundName AS MasterFundName    
FROM T_CompanyMasterFundSubAccountAssociation TCMFSA    
INNER JOIN T_CompanyMasterFunds TCMF     
ON TCMF.CompanyMasterFundID = TCMFSA.CompanyMasterFundID    
WHERE TCMFSA.CompanyFundID IN (SELECT FundID FROM @FundS)