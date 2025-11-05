







/****** Object:  Stored Procedure dbo.P_GETCLEARANCEFORUSER 6 
	Script Date: 09/20/2006 4:00:23 PM ******/
	
CREATE PROCEDURE [dbo].[P_GetAllocationClearanceForUser]	
(
	@USERID int		
)

AS 



SELECT 
	  A.AssetName + '\' + U.UnderLyingName + '\' + AUEC.DisplayName + '\' + C.CurrencySymbol AS [AUEC]
	 , AUEC.AUECID AS [AUECID]
	--, U.UnderLyingName
	--, AE.FullName
	--, AE.DisplayName
	--, C.CurrencyName
	--, CA.T_CompanyAUEC
	, AUEC.RegularTradingStartTime AS [Start Time]
	, AUEC.RegularTradingEndTime	AS [End Time]
	, ISNULL(CUACT.ClearanceTime , '')As [ Clearance Time]
	, ISNULL(CUACT.CompanyUserAUECCLearanceTimeID, 0) AS [Clearance Time ID]
	, CUA.CompanyUserAUECID AS [CompanyUserAUECID]	
FROM 
	T_CompanyUser CU
	INNER JOIN T_CompanyUserAUEC CUA ON CU.USERID = CUA.CompanyUserID
	LEFT OUTER JOIN T_CompanyUserAUECClearanceTimeAllocation CUACT ON CUA.CompanyUserAUECID  = CUACT.CompanyUserAUECID
	--LEFT OUTER JOIN  T_CompanyUserAUEC CUA 
	--INNER JOIN T_CompanyUser CU 
	INNER JOIN T_CompanyAUEC CA ON CA.CompanyAUECID = CUA.CompanyAUECID
	INNER JOIN T_AUEC AUEC ON AUEC.AUECID = CA.AUECID
	INNER JOIN T_ASSET A ON AUEC.AssetID = A.ASSETID
	INNER JOIN T_UNDERLYING U ON AUEC.UNDERLYINGID = U.UNDERLYINGID 
	INNER JOIN T_CURRENCY C ON AUEC.BaseCurrencyID = C.CurrencyID
WHERE 
	CU.USERID = @USERID



set ANSI_NULLS OFF 
set QUOTED_IDENTIFIER OFF 







