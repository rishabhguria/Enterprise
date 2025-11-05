/*
GetFundListCommaSeparated '1182,1185,1183,1184'
*/
CREATE Procedure GetFundListCommaSeparated
(
 @FundID varchar(max)
)
As

SET NOCOUNT ON 

Declare @Fund Table                                                                      
(                                                                      
 FundID int                                                                  
)                     
                  
Insert into @Fund                    
Select Cast(Items as int) from dbo.Split(@FundID,',') 

CREATE TABLE #TempFunds
( 
    FundName VARCHAR(2000) 
) 

INSERT into #TempFunds 
Select 
CF.FundName  
 From T_CompanyFunds CF--, 
Where CF.CompanyFundID in (select FundID from @Fund)
--T_CompanyUserFunds CUF  
-- Where CF.CompanyFundID = CUF.CompanyFundID  
-- And CUF.CompanyUserID = 17 
 
DECLARE @fundNames VARCHAR(2000) 
 
SELECT 
    @fundNames = COALESCE(@fundNames + ', ', '') + FundName 
FROM 
    #TempFunds 
 
Delete #TempFunds

Insert into #TempFunds
Select 
@fundNames

Select * from #TempFunds
 
DROP TABLE #TempFunds 
