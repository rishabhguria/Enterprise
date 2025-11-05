/****** Object:  Stored Procedure dbo.P_GetCompanyCVAUECs    Script Date: 04/20/2006 2:25:22 PM ******/

-- P_GetAllCompanyCVAUECsStringName 7   

Create PROCEDURE dbo.P_GetAllCompanyCVAUECsStringName
	(
		@companyID	int		 
	)
AS

	Select distinct CPV.CompanyCounterPartyCVID as CompanyCounterPartyCVID,  CA.AUECID as AUECID,CONCAT(asset.AssetName,'/', underl.UnderLyingName,'/', a.DisplayName,'/', curr.CurrencySymbol) as AUECString,cvauecid
	from T_CVAUEC CVA inner join T_CompanyAUEC CA on CVA.AUECID =CA.AUECID
	inner join T_AUEC A on A.AUECID = CA.AUECID
	inner join T_Asset asset on asset.AssetID=A.AssetID
	inner join T_Exchange excng on excng.ExchangeID=a.ExchangeID
	inner join T_UnderLying underl on underl.UnderLyingID=a.UnderLyingID
	inner join T_Currency curr on curr.CurrencyID=a.BaseCurrencyID
	inner join T_CompanyCounterPartyVenues CPV on CPV.CounterPartyVenueID=CVA.CounterPartyVenueID
	Where CA.CompanyID =@companyID
	order  by cvauecid asc



	
