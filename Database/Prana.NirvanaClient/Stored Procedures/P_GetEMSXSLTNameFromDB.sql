CREATE Procedure P_GetEMSXSLTNameFromDB
(
@emsSource varchar(50)
)

as

SELECT
B.FileNames
From T_ImportTrade as A
INNER JOIN T_FileData as B on A.XSLTFileID = B.FileId
Where A.ImportSourceName = @emsSource


-- Select * from T_ImportTrade