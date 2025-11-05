CREATE proc [dbo].[P_GetCompanyEMSSources]  
(  
@companyID int  
)  
  
as  
  
SELECT   
EMSSourceID ,  
B.ImportSourceName,  
B.XSLTFileID,  
C.FileNames  
FROM T_CompanyEMSSource A  
LEFT OUTER JOIN T_ImportTrade B ON A.EMSSourceID = B.ImportSourceID  
LEFT OUTER JOIN T_FileData C ON B.XSLTFileID = C.FileId  

where A.CompanyID  =  @companyID
