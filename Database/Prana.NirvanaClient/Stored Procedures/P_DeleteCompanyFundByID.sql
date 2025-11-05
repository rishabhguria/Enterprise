
/*********************************************************
Author: Manvendra Prajapati
Creation Date: 17 Feb 2015
Purpose: Did not delete assigned fund from admin
Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-6092

**********************************************************/ 

CREATE PROCEDURE [dbo].[P_DeleteCompanyFundByID] 
(
		@companyFundID int	
)
AS

Declare @result int
Set @result=0

BEGIN TRY
Begin		
	BEGIN TRAN
		if  exists (Select distinct fundid 
			From PM_Taxlots
			Where TaxLotOpenQty<>0 and
			Taxlot_PK in                                                                                           
			(                                                                                                 
			Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                             
			where DateDiff(d,PM_Taxlots.AUECModifiedDate,getdate()) >=0                                                                                                                                    
			group by taxlotid                                                 
			) 
			and fundid=@companyFundID )
		begin
              Set @result=-1
		end

		else
			 begin
			   Delete T_CompanyThirdPartyCVCommissionRules
			   Where CompanyFundID = @companyFundID
			
		    	--Delete T_CompanyThirdPartyMappingDetails
			  --Where SubAccountofID_FK = @companyFundID
			   Delete T_PTTAccountPercentagePreference
				Where AccountId=@companyFundID
			
			  Delete T_CompanyThirdPartyMappingDetails
			  Where InternalFundNameID_FK = @companyFundID
			
			  Delete T_CompanyThirdPartyFileFormats
			  Where CompanyFundID_FK = @companyFundID
			
			  Delete T_CompanyUserFunds
			  Where CompanyFundID = @companyFundID
			
			  Delete T_CompanyCounterPartyVenueDetails
			  Where CompanyFundID = @companyFundID
			
			  Delete T_CompanyFunds
			   Where CompanyFundID = @companyFundID

              set @result=1
		 End

	COMMIT TRAN

	End
END TRY


BEGIN CATCH
   	ROLLBACK TRAN
		Set @result=-1
END CATCH

Select @result

