/****************************************************************************                                                                            
Name :   PMGetServerReports                                    
Date Created: 28-Apr-2008                                                                             
Purpose:  Get the server reports links from the DB.                                    
Module Name: PortfolioReports/Transaction Summary Report                                    
Author: Bhupesh Bareja                                                                            
Parameters:                                                                             
  @companyID int,                                                                            
  @userID int,                                                                            
  @ErrorMessage varchar(500)                                                                             
  , @ErrorNumber int                                                                              
Execution StateMent:                                                                             
   EXEC [PMGetServerReports] 4, 1, ' ' , 0                                   
      
Date Modified:  15-Dec-2009                                                                           
Description:  PM_ReportSections table added                                                                             
Modified By:  Abhilash Katiyar                                                                             
****************************************************************************/                                      
CREATE PROCEDURE [dbo].[PMGetServerReports]       
(                                                                
  @companyID int,                                                                
  @userID int                                                  
                                                                  
 )                                                                
AS                                                                
SELECT 
ReportID,
ReportName, 
ReportLink, 
Flag, 
PMR.SectionID,
PMRS.SectionName
FROM PM_Reports AS PMR
JOIN PM_ReportSections AS PMRS ON PMR.SectionID = PMRS.SectionID
WHERE Flag = 'true'  
Order by PMR.SectionID    