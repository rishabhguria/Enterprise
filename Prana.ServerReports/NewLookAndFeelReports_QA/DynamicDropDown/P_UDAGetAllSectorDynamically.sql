/************************************            
Author: Ankit Misra            
Date:   15 Jan. 2015              
Desc:   Return only the UDASector that were traded by the client.             
Exec:   P_UDAGetAllSectorDynamically            
*************************************/            
            
            
CREATE procedure [dbo].[P_UDAGetAllSectorDynamically]            
As             
BEGIN            
 Create Table #TempUDASector            
  (            
 SectorName Varchar(100)          
  )            
------------------------------------------------------------------------            
-- FILL UDASECTOR WHICH ARE PRESENT IN MIDDLEWARE TABLE           
------------------------------------------------------------------------                          
 Insert Into #TempUDASector            
  Select Distinct            
  UDASector          
  From T_MW_GenericPNL PNL                    
  Order by UDASector          
------------------------------------------------------------------------            
-- IF NO DATA IN MIDDLEWARE SET UDASECTOR AS "No Data"            
------------------------------------------------------------------------            
 If(Select Count(*) from #TempUDASector) = 0            
 BEGIN            
  Insert into #TempUDASector Values('No Data')            
 END            
             
 Select * from #TempUDASector            
 Drop Table #TempUDASector            
END 