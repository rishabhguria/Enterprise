/************************************                
Author: Ankit Misra                
Date:   16 Jan. 2015                  
Desc:   Return only the UDASubSector that were traded by the client.                 
Exec:   P_UDAGetAllSubSectorDynamically_Activity                
*************************************/                
                
                
CREATE  procedure [dbo].[P_UDAGetAllSubSectorDynamically_Activity]                            
As                 
BEGIN                
 Create Table #TempUDASubSector                
  (                
   SubSectorName Varchar(100)                
  )                
------------------------------------------------------------------------------                
-- FILL UDASUBSECTOR WHICH ARE PRESENT IN T_MW_Transactions TABLE                
------------------------------------------------------------------------------                              
 Insert Into #TempUDASubSector                
  Select Distinct                
  UDASubSector                            
  From T_MW_Transactions Transactions                     
  Order by UDASubSector                
-------------------------------------------------------------------------------                
-- IF NO DATA IN T_MW_Transactions SET UDASUBSECTOR AS 'No Data'                
-------------------------------------------------------------------------------                
 If(Select Count(*) from #TempUDASubSector) = 0                
 BEGIN                
  Insert into #TempUDASubSector Values('No Data')                
 END                
                 
 Select * from #TempUDASubSector                
 Drop Table #TempUDASubSector                
END 