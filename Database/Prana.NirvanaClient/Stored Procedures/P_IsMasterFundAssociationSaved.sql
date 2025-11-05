   /* Procedure Name: dbo.P_GetAllCompanyClearingFirmsPrimeBrokers
      Date: 07/21/2008 
	  Execution: exec P_IsMasterFundAssociationSaved
      Purpose: To return no of rows depending upon the association saved for master-fund association	
      Name: Bhupesh Bareja   */  

CREATE PROCEDURE dbo.P_IsMasterFundAssociationSaved
AS  
declare @countRows int
 
 set @countRows = (SELECT count(*) FROM T_CompanyMasterFundSubAccountAssociation)   

 select @countRows
  
  
  