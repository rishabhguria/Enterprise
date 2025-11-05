/************************************      
Author: Ankit Misra      
Date:   15 Jan. 2015        
Desc:   Return only the UDASector that were traded by the client       
Exec:   P_UDAGetAllSectorDynamically_Activity      
*************************************/      
      
      
CREATE  procedure [dbo].[P_UDAGetAllSectorDynamically_Activity]        
As       
BEGIN      
 Create Table #TempUDASector      
  (      
   SectorName Varchar(100)      
  )      
----------------------------------------------------------------------------      
-- FILL UDASECTOR WHICH ARE PRESENT IN T_MW_Transactions TABLE   
----------------------------------------------------------------------------                    
 Insert Into #TempUDASector      
  Select Distinct      
  UDASector                  
  From T_MW_Transactions Transactions              
  Order by UDASector      
----------------------------------------------------------------------------      
-- IF NO DATA IN T_MW_Transactions SET UDASECTOR AS 'No Data'      
----------------------------------------------------------------------------      
 If(Select Count(*) from #TempUDASector) = 0      
 BEGIN      
  Insert into #TempUDASector Values('No Data')      
 END      
       
 Select * from #TempUDASector      
 Drop Table #TempUDASector      
END 