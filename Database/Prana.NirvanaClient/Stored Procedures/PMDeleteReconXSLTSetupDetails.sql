Create proc [dbo].[PMDeleteReconXSLTSetupDetails]  
(  
@reconThirdPartyID int,  
@xsltID int  
)  
  
AS  
  
delete from PM_ReconDataSourceXSLT  
where ReconThirdPartyID  = @reconThirdPartyID  
delete from T_FileData   
where FileId = @xsltID  
  
/*  
select * from PM_ReconDataSourceXSLT  
select * from T_FileData  
*/  