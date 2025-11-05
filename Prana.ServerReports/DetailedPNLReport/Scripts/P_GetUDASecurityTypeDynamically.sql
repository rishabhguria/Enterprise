/************************************            
Author: Ankit Misra            
Date:   20 April 2015              
Desc:   Return only the UDAAssets in which the client traded.             
Exec:   P_GetUDASecurityTypeDynamically
*************************************/            
            
            
CREATE  procedure [dbo].[P_GetUDASecurityTypeDynamically]                        
As             
BEGIN            
 Create Table #TempUDASecurityType           
  (            
   UDASecurityType Varchar(100)            
  )            
------------------------------------------------------------------------            
-- FILL UDA ASSETS WHICH ARE PRESENT IN MIDDLEWARE TABLE             
------------------------------------------------------------------------                          
 Insert Into #TempUDASecurityType            
  Select Distinct            
  UDASecurityType                        
  From T_MW_GenericPNL PNL                    
  Order by UDASecurityType            
------------------------------------------------------------------------            
-- IF NO DATA IN MIDDLEWARE SET UDA ASSETS AS "No Data"            
------------------------------------------------------------------------            
 If(Select Count(*) from #TempUDASecurityType) = 0            
 BEGIN            
  Insert into #TempUDASecurityType Values('No Data')            
 END            
             
 Select * from #TempUDASecurityType            
 Drop Table #TempUDASecurityType            
END 