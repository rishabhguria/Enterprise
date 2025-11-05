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
  Ltrim(Rtrim(DetailRowBGColor))  As DetailRowBGColor,
  Ltrim(Rtrim(Group1FontColor)) As Group1FontColor,    
  Ltrim(Rtrim(Group2FontColor)) As Group2FontColor,    
  Ltrim(Rtrim(Group3FontColor)) As Group3FontColor,    
  Ltrim(Rtrim(Group4FontColor)) As Group4FontColor,    
  Ltrim(Rtrim(DetailRowFontColor))  As DetailRowFontColor,  
  Ltrim(Rtrim(GrandTotalRowFontColor))  As GrandTotalRowFontColor      
 From T_ReportGroupRowColors_New     
END      

