/*    
Exec P_ImportPerformanceOnBoard    
@filepath = 'C:\Users\Administrator\Desktop\Agile_V1.7.1\Agile_PerformanceonBoard\Book1.xlsx'    
*/    
    
CREATE PROCEDURE [dbo].[P_ImportPerformanceOnBoard]    
(    
 @FilePath varchar(max)    
)                      
AS                      
BEGIN                      
-- Transaction start-----------                      
BEGIN TRANSACTION [Tran1]                      
--Start try block statement ---                      
BEGIN TRY                      
                      
---- Create a temp table                      
CREATE TABLE #TempDataTable                       
(                      
 Fund VARCHAR(300) Not Null -- COL A                      
 ,Field VARCHAR(100) Null -- COL B                      
 ,Period VARCHAR(100) Null -- COL C                      
 ,Definition VARCHAR(100) Null -- COL D                      
 ,[Value] VARCHAR(100) Null -- COL E                      
)               
     
DECLARE @bulkinsert NVARCHAR(2000)    
SET @bulkinsert = N'insert INTO #TempDataTable (Fund,Field,Period,Definition,[Value])                      
SELECT * FROM OPENROWSET(''Microsoft.ACE.OLEDB.12.0'',''Excel 8.0;HDR=no;Database='+@filepath+''', ''select * from [Sheet1$]'''+')'                    
  
EXEC sp_executesql @bulkinsert    
            
--select * from #TempDataTable            
                      
------ we are getting spaces left and reght sider, so updating before saving in the database                      
UPDATE #TempDataTable                       
SET                       
 Fund = RTRIM(LTRIM(Fund)),                      
 Field = RTRIM(LTRIM(Field)),                      
 Period = RTRIM(LTRIM(Period)),                      
 Definition = RTRIM(LTRIM(Definition)),                      
 [Value] = RTRIM(LTRIM([Value]))                      
                      
DELETE from #TempDataTable where Fund='Fund'                      
                      
                  
                    
update #TempDataTable                    
SET Period='' where Period is null                    
                  
--select * from #TempDataTable                      
                      
SELECT                       
Fund as Fund                      
,CAST([Value] as float) as Performance                      
,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,Definition)+1,0)) as asOfDate                      
,dbo.TRIM(Case                       
   when Field LIKE '%Gross%'                       
   THEN 'GROSS'                      
   When Field Like '%Net%'                      
   Then 'NET'                      
   WHEN Field LIKE '%Index%'                      
   THEN left(Field, charindex('Index', Field) - 2)                     
   Else Field                     
   End                      
+' '+Period) as DataType                      
,CASE                       
 WHEN Field LIKE '%$%' and Period in ('MTD','QTD','YTD','ITD') and Field Not LIKE '%Gross%' and Field Not LIKE '%Net%'      
 THEN 1                      
 ELSE 0                      
 End as IsBenchMark into #Temp                      
                      
from #TempDataTable                      
                      
update #Temp set asofdate = dbo.AdjustBusinessDays(asOfDate,-1,11) where dbo. isbusinessday(asOfDate,1)=0 --set date to previous business day.                        
                
               
------------Temp Table to store comman values means need to update only----                    
                
-- SELECT T.Fund,T.AsofDate,T.DataType,T.Isbenchmark,T.Performance into #Comman                     
--  FROM T_MW_PerformanceOnBoard PB                      
--  INNER JOIN #Temp T ON T.Fund=PB.Fund and T.AsofDate=PB.AsOfDate and                      
--  T.DataType =PB.DataType and PB.IsBenchmark=T.IsBenchmark                
--              
-----------Update data to store info----------              
              
--UPDATE T_MW_PerformanceOnBoard                      
-- SET Performance = T.Performance                      
-- FROM #Temp T         
-- INNER JOIN T_MW_PerformanceOnBoard PB ON T.Fund=PB.Fund AND T.AsofDate=PB.AsOfDate and                       
-- T.DataType =PB.DataType and PB.IsBenchmark=T.IsBenchmark       
              
---------------now delete comman rows------              
              
--delete t from #Temp T               
--inner join #comman PB               
--ON T.Fund=PB.Fund and T.AsofDate=PB.AsOfDate and                      
--T.DataType =PB.DataType and PB.IsBenchmark=T.IsBenchmark                
        
-----Delete all values from table----------------------              
delete from T_MW_PerformanceOnBoard        
              
-----Rest insert ----------------------              
INSERT INTO T_MW_PerformanceOnBoard (                      
  Fund                      
  ,AsOfDate                      
  ,DataType                      
  ,Performance                      
  ,IsBenchmark                      
  )                      
 SELECT Fund                      
  ,AsOfDate                      
  ,DataType                      
  ,Performance                      
  ,IsBenchmark                      
 FROM #Temp               
                 
--select * from T_MW_PerformanceOnBoard                   
                     
          
Drop TABLE #Temp--,#comman                      
Drop Table #TempDataTable                      
                      
--if all records saved successfully then commit                       
Commit TRANSACTION [Tran1]                      
END TRY                      
BEGIN CATCH                      
-- roll back all record if anyone query failed                      
  ROLLBACK TRANSACTION [Tran1]                      
END CATCH                        
                      
End 