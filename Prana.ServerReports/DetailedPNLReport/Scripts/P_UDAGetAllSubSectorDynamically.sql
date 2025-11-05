/************************************            
Author: Ankit Misra            
Date:   15 Jan. 2015              
Desc:   Return only the UDASubSector that were traded by the client.             
Exec:   P_UDAGetAllSubSectorDynamically            
*************************************/            
            
            
CREATE  procedure [dbo].[P_UDAGetAllSubSectorDynamically]                        
As             
BEGIN            
 Create Table #TempUDASubSector            
  (            
   SubSectorName Varchar(100)            
  )            
------------------------------------------------------------------------            
-- FILL UDASUBSECTOR WHICH ARE PRESENT IN MIDDLEWARE TABLE             
------------------------------------------------------------------------                          
 Insert Into #TempUDASubSector            
  Select Distinct            
  UDASubSector                        
  From T_MW_GenericPNL PNL                    
  Order by UDASubSector            
------------------------------------------------------------------------            
-- IF NO DATA IN MIDDLEWARE SET UDASUBSECTOR AS "No Data"            
------------------------------------------------------------------------            
 If(Select Count(*) from #TempUDASubSector) = 0            
 BEGIN            
  Insert into #TempUDASubSector Values('No Data')            
 END            
             
 Select * from #TempUDASubSector            
 Drop Table #TempUDASubSector            
END 