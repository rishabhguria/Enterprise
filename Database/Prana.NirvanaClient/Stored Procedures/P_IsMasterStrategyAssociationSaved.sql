   /* Procedure Name: dbo.P_IsMasterStrategyAssociationSaved  
      Date: 08/07/2009   
      Execution: exec P_IsMasterStrategyAssociationSaved  
      Purpose: To return no of rows depending upon the association saved for master-Strategy association   
      Name: Sandeep  */    
  
CREATE PROCEDURE dbo.P_IsMasterStrategyAssociationSaved  
AS    
declare @countRows int  
set @countRows = (SELECT count(*) FROM T_CompanyMasterFundSubAccountAssociation)     
select @countRows  
    
    