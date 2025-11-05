




CREATE    procedure
[dbo].[P_GetUnderLyingsByAssetAndUserID]
	(
		@UserID int,
		@AssetID int
	)
as
select distinct  

T_AUEC.UnderlyingID,T_Underlying.UnderLyingName,T_AUEC.AssetID,'test' 
from T_CompanyUserAUEC

join T_CompanyAUEC
on 
T_CompanyAUEC.CompanyAUECID =T_CompanyUserAUEC.CompanyAUECID

join T_AUEC
on T_AUEC.AUECID=T_CompanyAUEC.AUECID
join T_Underlying
on T_Underlying.UnderLyingID=T_AUEC.UnderLyingID
where T_CompanyUserAUEC.CompanyUserID =@UserID
and T_AUEC.AssetID = @AssetID

--select * from T_CompanyAUEC
--select * from T_Underlying
--select * from T_AUEC

