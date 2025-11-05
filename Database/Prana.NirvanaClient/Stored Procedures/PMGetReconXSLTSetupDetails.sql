/*          
Modified By: Ankit Gupta on 27 Nov, 2014  
Description: Opening Recon XSLT steup from Admin throws error  
JIRA: http://jira.nirvanasolutions.com:8080/browse/PRANA-5468  
If there are no matching entries in XSLTID & FileID, then FileId & FileNames should not be returned as NULL, instead they should be returned as 0 & String.Empty respectively.  
*/          
CREATE PROCEDURE [dbo].[PMGetReconXSLTSetupDetails]  
  
AS  
  
SELECT  
A.ReconThirdPartyID,  
A.ThirdPartyID,  
A.ReconType,  
A.FormatType,  
ISNULL(B.FileId,0) as FileId,  
ISNULL (B.FileNames,'') as FileNames  
From  
PM_ReconDataSourceXSLT as A  
LEFT OUTER JOIN T_FileData as B on A.XSLTID = B.FileId  
  