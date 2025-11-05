
/*        
Modified By: Ankit Gupta on 24 Oct, 2014
Description: If user deletes a Fund, it should not be visible on Release set up UI.
*/    
    
CREATE PROCEDURE [dbo].[P_GetAllCompanywiseFunds]
(
 @xmlClient nText
)
AS

declare @handle1 int
exec sp_xml_preparedocument @handle1 output, @xmlClient

CREATE TABLE #TempCompany                                                                              
(                                                                               
   CompanyID int,      
   CompanyName varchar(100)                 
)  
      
insert INTO #TempCompany                                                                               
(CompanyID , CompanyName)
SELECT                                                                                
CompanyId,
CompanyName                                   
FROM OPENXML(@handle1, '/dsClient/dtClient', 2)                                                                                 
WITH                                                                               
(                                                         
   CompanyID int,      
   CompanyName varchar(100)           
)                          

SELECT CompanyFundID, FundName, FundShortName, CompanyID
FROM  T_CompanyFunds where companyID in (SELECT CompanyID from #TempCompany) AND IsActive = '1'
exec sp_xml_removedocument @handle1

