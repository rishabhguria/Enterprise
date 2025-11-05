
/****** Object:  Stored Procedure dbo.P_DeleteCompany    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompany
(
	@companyID int
)
AS	
	Declare @total int
	Select @total = Count(1) 
		From T_RMCompanyUsersOverall
		Where CompanyID = @companyID
	
			if ( @total = 0)
			begin
			Select @total = Count(1) 
				From T_RMCompanyUsersOverall
				Where CompanyID = @companyID
	
				if ( @total = 0)
				begin
				
					Select @total = Count(1) 
						From T_RMCompanyUsersOverall
						Where CompanyID = @companyID
			
						if ( @total = 0)
						begin
						
							Select @total = Count(1) 
							From T_RMCompanyUsersOverall
							Where CompanyID = @companyID
					
								if ( @total = 0)
								begin
								
									Select @total = Count(1) 
										From T_RMCompanyUsersOverall
										Where CompanyID = @companyID
							
										if ( @total = 0)
										begin
										
											Select @total = Count(1) 
												From T_RMCompanyUsersOverall
												Where CompanyID = @companyID
									
											if ( @total = 0)
											begin
												exec P_DeleteCompanyUsers @companyID
												
												exec P_DeleteCompanyCVDetails @companyID		--Have to delete the CounterParty b4 deleting it.
												
												exec P_DeleteCompanyClient @companyID
												
												exec P_DeleteCompanyModules @companyID
												
												exec P_DeleteCompanyThirdParty @companyID
												
												exec P_DeleteSetUpCompany @companyID	
												
												Delete T_Company
												Where CompanyID = @companyID
											end
										end
									end
								end
							end
						end