
CREATE  procedure
P_GetUnderLyingsByUserID
	(
		@UserID int	
	)
as
select distinct  T_Underlying.UnderlyingID,UnderLyingName,T_Underlying.AssetID,'test' from T_CompanyUserAUEC

join T_CompanyAUEC
on 
T_CompanyAUEC.CompanyAUECID =T_CompanyUserAUEC.CompanyAUECID

join T_AUEC
on T_AUEC.AUECID=T_CompanyAUEC.AUECID
join T_Underlying
on T_Underlying.UnderLyingID=T_AUEC.UnderLyingID
where T_CompanyUserAUEC.CompanyUserID=@UserID
