GO 
  print 'Started Executing: ClientDB_P_GetReportGroupRowColors_New.sql'
GO 
                      
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[P_GetReportGroupRowColors_New]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
DROP PROCEDURE [dbo].[P_GetReportGroupRowColors_New]
END
GO
-- =============================================      
-- Author:  Sumit Kakra      
-- Create date: 12-Jan-2009      
-- Description: Returns Row colors from reports    
/*      
 Usage:       
  Exec P_GetReportGroupRowColors]    
      
*/      
-- =============================================      
CREATE PROCEDURE [dbo].[P_GetReportGroupRowColors_New]    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT Ltrim(Rtrim(Group1BGColor)) As Group1BGColor,    
  Ltrim(Rtrim(Group2BGColor)) As Group2BGColor,    
  Ltrim(Rtrim(Group3BGColor)) As Group3BGColor,    
  Ltrim(Rtrim(Group4BGColor)) As Group4BGColor,    
  Ltrim(Rtrim(DetailRowBGColor))  As DetailRowBGColor    
 From T_ReportGroupRowColors_New     
END      

GO 
  print 'Done Executing: ClientDB_P_GetReportGroupRowColors_New.sql'
 GO 
    