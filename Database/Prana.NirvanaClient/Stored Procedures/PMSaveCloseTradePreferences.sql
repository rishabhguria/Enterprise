  
/****************************************************************************  
Name :   PMSaveCloseTradePreferences  
Date Created: 04-dec-2006   
Purpose:  To save preferences for the close trader filter for the last trade.  
Author: Bhupesh Bareja  
Parameters:   
   @UserID int  
   @CloseTradeReportDate datetime  
   @ThirdPartyID int  
   @ReportMethodology tinyint  
   @ReportAlgorithm tinyint  
   @Comments ntext  
   @AssetID int  
   @ErrorNumber int output  
   @ErrorMessage varchar(100) output  
  
Execution StateMent:      
   EXEC PMSaveCloseTradePreferences 2, '11/11/2006', 1, 'Maunal', 'LIFO', ' ', 0, ' '  
  
  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMSaveCloseTradePreferences]  
 (  
    @UserID int  
  , @CloseTradeReportDate datetime  
  , @ThirdPartyID int  
  , @ReportMethodology tinyint  
  , @ReportAlgorithm tinyint  
  , @Comments ntext  
  , @AUECIDList ntext  
  , @FundIDList ntext  
  , @AssetID int  
  , @ErrorNumber int output  
  , @ErrorMessage varchar(100) output  
    
 )  
AS   
  
  
  
Declare @CloseTradeReportID int  
set @CloseTradeReportID = 0  
  
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'  
BEGIN TRY  
  
BEGIN TRAN  
  
INSERT  INTO  
  PM_CloseTradeReportRuns   
   (  
      UserID  
    , CloseTradeReportDate  
    , ThirdPartyID  
    , ReportMethodology  
    , ReportAlgorithm  
    , Comments  
    , AssetID  
   )  
VALUES  
   (  
      @UserID  
    , @CloseTradeReportDate  
    , @ThirdPartyID  
    , @ReportMethodology  
    , @ReportAlgorithm  
    , @Comments  
    , @AssetID  
   )  
  Set @CloseTradeReportID = Scope_Identity()  
    
  --INSERT INTO PM_CloseTradeReportRunsAUEC Select * from   
    
    
  CREATE TABLE  
   #TempFundIDList(FundID int, CloseTradeReportID int)  
insert into   
 #TempFundIDList  
  (  
   FundID  
  )    
  (  
   Select   
    CAST(ITEMS as INT)  
   FROM   
    SPLIT(@FundIDList, ',')  
  )  
UPDATE  
 #TempFundIDList  
    Set CloseTradeReportID = @CloseTradeReportID  
    
  
 CREATE TABLE  
   #TempAUECIDList(AUECID int, CloseTradeReportID int)  
insert into   
 #TempAUECIDList  
  (  
   AUECID  
  )    
  (  
   Select   
    CAST(ITEMS as INT)  
   FROM   
    SPLIT(@AUECIDList, ',')  
  )   
  
UPDATE   
 #TempAUECIDList  
  Set CloseTradeReportID = @CloseTradeReportID  
    
    
  INSERT INTO PM_CloseTradeReportRunsAUEC(AUECID, CloseTradeReportID)   
  --Values(1, 1)  
   Select A.AUECID, A.CloseTradeReportID from #TempAUECIDList A  
    
  INSERT INTO PM_CloseTradeReportRunsFunds(CompanyFundID, CloseTradeReportID)   
  --Values(1, 1)  
  Select F.FundID, F.CloseTradeReportID from #TempFundIDList F  
    
    
COMMIT TRAN  
Select @CloseTradeReportID  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
END CATCH;  
  
  
  
  
  