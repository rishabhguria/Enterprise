
CREATE PROCEDURE [dbo].[P_GetCompanyUsersAUECs]
(
		@companyUserID int
		
	)

as
SELECT   T_AUEC.AUECID, T_AUEC.AssetID, T_AUEC.UnderLyingID, T_AUEC.ExchangeID, T_AUEC.BaseCurrencyID, T_AUEC.DisplayName


FROM  T_CompanyUserAUEC
join T_CompanyAUEC on T_CompanyAUEC.CompanyAUECID =T_CompanyUserAUEC.CompanyAUECID
join T_AUEC on T_AUEC.AUECID=T_CompanyAUEC.AUECID 

where T_CompanyUserAUEC.CompanyUserID=@companyUserID

order by T_AUEC.AssetID, T_AUEC.UnderLyingID, T_AUEC.ExchangeID, T_AUEC.BaseCurrencyID