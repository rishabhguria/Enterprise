/************************************              
Author: Ankit Misra              
Date:   20 April 2015                
Desc:   Return only the UDAAssets in which the client traded.               
Exec:   P_GetUDACountriesDynamically  
*************************************/              
              
              
CREATE  procedure [dbo].[P_GetUDACountriesDynamically]                          
As               
BEGIN              
 Create Table #TempUDACountry             
  (              
   UDACountry Varchar(100)              
  )              
------------------------------------------------------------------------              
-- FILL UDA ASSETS WHICH ARE PRESENT IN MIDDLEWARE TABLE               
------------------------------------------------------------------------                            
 Insert Into #TempUDACountry              
  Select Distinct              
  UDACountry                          
  From T_MW_GenericPNL PNL                      
  Order by UDACountry              
------------------------------------------------------------------------              
-- IF NO DATA IN MIDDLEWARE SET UDA ASSETS AS "No Data"              
------------------------------------------------------------------------              
 If(Select Count(*) from #TempUDACountry) = 0              
 BEGIN              
  Insert into #TempUDACountry Values('No Data')              
 END              
               
 Select * from #TempUDACountry              
 Drop Table #TempUDACountry              
END 