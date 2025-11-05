CREATE PROC [dbo].[P_GetImportTradeDetails]

AS   
  
SELECT 
ImportSourceID As ID,
ImportSourceName,
XSLTFileID,
B.FileNames
FROM T_ImportTrade A
LEFT OUTER JOIN T_FileData B
ON A.XSLTFileID = B.FileId

