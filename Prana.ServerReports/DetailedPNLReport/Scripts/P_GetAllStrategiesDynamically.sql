/************************************            
Author: Ankit Misra            
Date:   20 April 2015              
Desc:   Return only the Strategy in which the client traded.             
Exec:   P_GetAllStrategiesDynamically
*************************************/            
            
            
CREATE  procedure [dbo].[P_GetAllStrategiesDynamically]                        
As             
BEGIN            
 Create Table #TempStrategy           
  (            
   Strategy Varchar(100)            
  )            
------------------------------------------------------------------------            
-- FILL STRATEGY WHICH ARE PRESENT IN MIDDLEWARE TABLE             
------------------------------------------------------------------------                          
 Insert Into #TempStrategy            
  Select Distinct            
  Strategy                        
  From T_MW_GenericPNL PNL                    
  Order by Strategy            
------------------------------------------------------------------------            
-- IF NO DATA IN MIDDLEWARE SET STRATEGY AS "No Data"            
------------------------------------------------------------------------            
 If(Select Count(*) from #TempStrategy) = 0            
 BEGIN            
  Insert into #TempStrategy Values('No Data')            
 END            
             
 Select * from #TempStrategy            
 Drop Table #TempStrategy            
END 