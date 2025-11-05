    
CREATE Procedure [dbo].[GetXSLTForReconDataSource] as     
Begin    
    
Select     
A.ThirdPartyID,    
A.ReconType,  
A.FormatType,   
B.FileNames     
from     
PM_ReconDataSourceXSLT  A   
INNER JOIN T_FileData B on A.XSLTID = B.FileId order by ThirdPartyID, ReconType  
End    
  
-- select * from  PM_ReconDataSourceXSLT  