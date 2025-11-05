Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg=''

Select CF.FundName As [Fund Name]
InTo #Temp_ThirdPartyPermittedFunds
From T_CompanyFunds CF
Where CompanyFundID Not In
(
	Select CopanyFundID From T_ThirdPartyPermittedFunds
)

Select CF.FundName As [Fund Name] 
InTo #Temp_ThirdPartyMappingFunds
From T_CompanyFunds CF
Where CompanyFundID Not In
(
	Select InternalFundNameID_FK From T_CompanyThirdPartyMappingDetails
)

Declare @CountPermittedFunds Int
Set  @CountPermittedFunds = (Select Count(*) From #Temp_ThirdPartyPermittedFunds)

Declare @CountMappedFunds Int
Set  @CountMappedFunds = (Select Count(*) From #Temp_ThirdPartyMappingFunds)

--If Exists (Select * from #Temp_ThirdPartyPermittedFunds)
If (@CountPermittedFunds > 0)
	Begin
		Select * from #Temp_ThirdPartyPermittedFunds
		Set @ErrorMsg = @ErrorMsg + 'Account Mapping is missing in ThirdParty'
	End
Else  If (@CountMappedFunds > 0)
	Begin
		Select * from #Temp_ThirdPartyMappingFunds
		Set @ErrorMsg = @ErrorMsg + 'Account Mapping is missing in ThirdParty'
	End

Select @ErrorMsg as ErrorMsg

Drop Table #Temp_ThirdPartyPermittedFunds,#Temp_ThirdPartyMappingFunds
