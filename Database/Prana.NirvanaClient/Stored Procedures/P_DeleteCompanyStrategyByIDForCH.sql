CREATE PROCEDURE [dbo].[P_DeleteCompanyStrategyByIDForCH]  
 (  
  @companyStrategyID int   
 )  
AS  
Declare @result int 
Declare @count int 
Set @result=0 
   select @count=COUNT(*) from T_CompanyMasterStrategySubAccountAssociation where CompanyStrategyID=@companyStrategyID
   IF @count > 0
   begin
   set @result=-1
   end
ELSE
BEGIN TRY  
Begin    
 BEGIN TRAN 
   
   Update T_CompanyStrategy SET IsActive=0 
   Where CompanyStrategyID = @companyStrategyID    

   Delete T_CompanyUserStrategies  
   Where CompanyStrategyID = @companyStrategyID  
     
   Delete T_CompanyCounterPartyVenueDetails  
   Where CompanyStrategyID = @companyStrategyID  
     
COMMIT TRAN 
 
 set @result=1  
 End  
END TRY    
  
BEGIN CATCH  
    ROLLBACK TRAN  
	Set @result=-1  
END CATCH  
 
SELECT @result  
   
