INSERT INTO T_Asset (AssetNAme) 
VALUES ('Forward') 

update T_AUEC set 
UnderlyingID=12 
where AssetID=5 

update T_AUEC set 
AssetID=11 
where AUECID=33


-- Migrate Thirdparty taxlot state to new structure

Insert Into T_PBWisetaxlotstate (PBID, TaxlotID, TaxlotState, PBTaxlotID)
Select	T_CompanyThirdPartyMappingDetails.CompanyThirdPartyID_FK As PBID, 
		TaxlotID, 
		TaxlotState, 
		Convert(varchar(50), T_CompanyThirdPartyMappingDetails.CompanyThirdPartyID_FK) + TaxlotID As PBTaxlotID
From  T_Level2Allocation
Inner Join T_FundAllocation On T_Level2Allocation.Level1AllocationID =  T_FundAllocation.AllocationID
Inner Join T_CompanyThirdPartyMappingDetails On T_CompanyThirdPartyMappingDetails.InternalFundNameID_FK = T_FundAllocation.FundID

Update T_Level2Allocation
Set TaxlotState = 0