
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: When user re-enables a deleted or disabled third party, set the value of the column 'isActive' to '1' for that third party.
*/        
CREATE PROCEDURE [dbo].[P_SaveItemsToBeEnabled]          
(      
  @XMLDoc nText                                            
 )               
AS               
                                                             
BEGIN TRY  
BEGIN TRANSACTION TRAN1    
DECLARE @handle int                           
exec sp_xml_preparedocument @handle OUTPUT,@XMLDoc 
         
      
 CREATE TABLE #TempTableNames                                                                               
  (                                                                               
    ID int,      
	ItemID int                
   )        
      
INSERT INTO #TempTableNames                     
 (                                                                              
    ID                        
   ,ItemID                                                         
 )                                                                              
SELECT                                                                               
	ID                        
   ,IDToBeEnabled                                        
    FROM OPENXML(@handle, '/dsEnableDisabledItems/dtEnableDisabledItems', 2)                                                                                 
 WITH                                                                               
 (                                                         
   ID int,      
IDToBeEnabled int                  
 )                          
 
Declare @ItemType int

Set @ItemType = (SELECT TOP 1 ID from #TempTableNames)

IF @ItemType = 1
BEGIN
Update T_Company SET IsActive = 1 where CompanyID in (SELECT ItemID from #TempTableNames)
END
ELSE IF @ItemType = 2
BEGIN
Update T_CompanyMasterFunds SET IsActive = 1 where CompanyMasterFundID in (SELECT ItemID from #TempTableNames)
END 

ELSE IF @ItemType = 3
BEGIN
Update T_CompanyFunds SET IsActive = 1 where CompanyFundID in (SELECT ItemID from #TempTableNames)
END 

ELSE IF @ItemType = 4
BEGIN
Update T_CompanyMasterStrategy SET IsActive = 1 where CompanyMasterStrategyID in (SELECT ItemID from #TempTableNames)
END 

ELSE IF @ItemType = 5
BEGIN
Update T_CompanyStrategy SET IsActive = 1 where CompanyStrategyID in (SELECT ItemID from #TempTableNames)
END                                                                                                           

ELSE IF @ItemType = 6
BEGIN
Update T_CompanyUser SET IsActive = 1 where UserID in (SELECT ItemID from #TempTableNames) 
END     

ELSE IF @ItemType = 7
BEGIN
Update T_ThirdParty SET IsActive = 1 where ThirdPartyID in (SELECT ItemID from #TempTableNames) 
END

EXEC sp_xml_removedocument @handle                            
                       
                       
COMMIT TRANSACTION TRAN1 
END TRY       
                     
BEGIN CATCH                                         
ROLLBACK TRANSACTION TRAN1                      
END CATCH;

