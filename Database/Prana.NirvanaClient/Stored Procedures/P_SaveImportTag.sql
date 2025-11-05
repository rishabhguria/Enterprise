
CREATE procedure [dbo].[P_SaveImportTag]  
@xmlDoc ntext  
as  
declare @handle int  
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc  
CREATE TABLE #TempBatch                                                                                 
(                                                                                 
Acronym varchar(max),  
ImportTagName varchar(max)  
)          
insert INTO #TempBatch  
(Acronym,ImportTagName)  
SELECT   
Acronym, ImportTagName  
from openxml (@handle, '/NewDataSet/Table1', 2)  
with  
(  
Acronym varchar(max),  
ImportTagName varchar(max)  
)  

DELETE FROM T_ImportTag

INSERT INTO T_ImportTag ( 
      Acronym, 
      ImportTagName
       ) 
SELECT 
       Acronym, 
       ImportTagName
FROM #TempBatch
  

exec sp_xml_removedocument @handle   
drop TABLE #TempBatch  

