/*************************************************                                                  
Author : Ankit Misra                                                 
Creation Date : 11th June , 2015                                                    
Description : Script to calculate Top N,N,n Long/Short Exposure Expo                                   
                                     
Modified By: Sandeep Singh
Date: 10 August 2015
Desc: http://jira.nirvanasolutions.com:8080/browse/PRANA-10294 (No FX Spot or Forward positions required in the Positions by Size area)                                  
                                       
Execution Statement:                                                 
P_MW_TopLongShortExposure_DailyReport @EndDate='7/2/2015',@Fund=N'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310',@TopExposures='5,10,20,25',@PTHFund='1270'                          
*************************************************/                         
ALTER Procedure [dbo].[P_MW_TopLongShortExposure_DailyReport]                                
(                        
 @EndDate datetime,                                
 @Fund Varchar(max),                                
 @TopExposures Varchar(20),  
 @PTHFund Varchar(max)                              
)                                
AS  
      
--Declare @EndDate datetime                                
--Declare @Fund Varchar(2000)      
--Declare @TopExposures Varchar(20)                                    
--Declare @PTHFund Varchar(max)   
--                       
--Set @EndDate = '7/2/2015'                            
--Set @Fund = '1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'       
--Set @TopExposures = '5,10,20,25'  
--Set @PTHFund='1270'      
                        
BEGIN                        
                        
Declare @T_FundIDs Table                                                                                                                                              
(                                
 FundId int                                
)                                
                                
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                                                                  
    
---- PTH Funds  
Declare @T_PTHFundIDs Table                                                                                                                                                            
(                                                                                                                                                            
 FundId int                                                                                                                                                            
)                                                                                                                                                            
Insert Into @T_PTHFundIDs Select * From dbo.Split(@PTHFund, ',')    
          
Declare @PTHFundCount Int  
Set @PTHFundCount = (Select Count(FundID) from @T_PTHFundIDs)  
      
If ( @PTHFundCount > 0)  
Begin  
Delete From @T_FundIDs Where FundID In ( Select FUndID From @T_PTHFundIDs)  
End   
                                                                                
                                                                                  
CREATE TABLE #T_CompanyFunds                                                                                                                                              
(                                                                                                                   
 CompanyFundID int,                                
 FundName varchar(50)                                
)                                
                                                    
Insert Into #T_CompanyFunds                                                          
Select                                                                                                                           
CompanyFundID,                                                                                    
FundName                              
From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                         
                        
Declare @T_TopExposures Table                                                                                                                                              
(                                
 TopN int                                
)                                
                                
Insert Into @T_TopExposures Select * From dbo.Split(@TopExposures, ',')     
    
Select        
UnderlyingSymbol,  
Sum(CASE  
WHEN Asset NOT IN ('FX','FXForward')  
THEN ISNULL(DeltaExposureBase,0)  
ELSE 0.0  
END) AS Exposure   
InTo #TempExp     
FROM T_MW_GenericPNL PNL                                
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                   
WHERE DATEDIFF(Day,Rundate,@EndDate)=0 And Open_Closetag = 'O' And Asset Not In ('Cash','FX','FXForward')
----Asset <> 'Cash'                
Group By UnderlyingSymbol     
        
Alter Table #TempExp    
Add Side Varchar(20)    
    
Update #TempExp    
Set Side =     
Case    
 When Exposure >= 0    
 Then 'Long'    
 Else 'Short'    
End                    
                        
CREATE  Table  #ExposureTable                              
(                                    
 Category  VARCHAR(50),                                    
 TopExposure VARCHAR(50),                        
 Exposure Float                                    
)                        
                        
Insert Into #ExposureTable                        
Select                        
'Long',                        
'Total Long',                         
Sum(Exposure)    
FROM #TempExp                              
WHERE Side = 'Long'    
        
Insert Into #ExposureTable                        
Select                        
'Short',                        
'Total Short',                         
Sum(Exposure)    
FROM #TempExp                              
WHERE Side = 'Short'    
                   
Declare @I Int                        
Declare @DynamicQuery nvarchar(500)                       
Declare @TopPosition  varchar(5)                      
DECLARE @Exposure Float                      
Set @I = (Select Count(*) from @T_TopExposures)                        
                        
While ( @I>0)                      
BEGIN                    
SET @TopPosition = (Select Top 1 * from @T_TopExposures )                    
DELETE TOP (1) FROM @T_TopExposures                    
SET @DynamicQuery ='INSERT INTO #ExposureTable SELECT ''Long'',''TOP '+ @TopPosition +  
''',SUM(EXPOSURE.DEB) From (Select TOP '+@TopPosition+' Sum(IsNull(Exposure,0)) as DEB From #TempExp Where Side = ''Long''   
Group By UnderlyingSymbol Order By DEB DESC) AS EXPOSURE'                   
  
EXEC sp_executesql @DynamicQuery                    
SET @DynamicQuery ='INSERT INTO #ExposureTable SELECT ''Short'',''TOP '+ @TopPosition +  
''',SUM(EXPOSURE.DEB) From (Select TOP '+@TopPosition+' Sum(IsNull(Exposure,0)) as DEB From #TempExp Where Side = ''Short''  
Group By UnderlyingSymbol   
Order By DEB ASC) AS EXPOSURE'                
                   
EXEC sp_executesql @DynamicQuery                    
Set @I = @I-1                      
END     
                       
Select Category,TopExposure,ISNULL(Exposure,0) As Exposure  from  #ExposureTable     
                     
DROP TABLE #T_CompanyFunds,#ExposureTable ,#TempExp                     
END 