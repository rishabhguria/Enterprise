


CREATE  procedure
[dbo].[P_GetAssetsByUserID]
	(
		@UserID int	
	)
as
select distinct CompanyID,T_Asset.AssetID,AssetName , 'test ' from T_CompanyUserAUEC

join T_CompanyAUEC on T_CompanyAUEC.CompanyAUECID =T_CompanyUserAUEC.CompanyAUECID
join T_AUEC on T_AUEC.AUECID=T_CompanyAUEC.AUECID 
join T_Asset on T_Asset.AssetID=T_AUEC.AssetID

where T_CompanyUserAUEC.CompanyUserID=@UserID

