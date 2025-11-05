/************************************              
Author: Ankit Misra              
Date:   20 April 2015                
Desc:   Return only the UDAAssets in which the client traded.               
Exec:   P_GetUDAAssetsDynamically  
*************************************/              
              
              
CREATE  procedure [dbo].[P_GetUDAAssetsDynamically]                          
As               
BEGIN              
 Create Table #TempUDAAssets             
  (              
   UDAAssetClass Varchar(100)              
  )              
------------------------------------------------------------------------              
-- FILL UDA ASSETS WHICH ARE PRESENT IN MIDDLEWARE TABLE               
------------------------------------------------------------------------                            
 Insert Into #TempUDAAssets              
  Select Distinct              
  UDAAssetClass                          
  From T_MW_GenericPNL PNL                      
  Order by UDAAssetClass              
------------------------------------------------------------------------              
-- IF NO DATA IN MIDDLEWARE SET UDA ASSETS AS "No Data"              
------------------------------------------------------------------------              
 If(Select Count(*) from #TempUDAAssets) = 0              
 BEGIN              
  Insert into #TempUDAAssets Values('No Data')              
 END              
               
 Select * from #TempUDAAssets              
 Drop Table #TempUDAAssets              
END 