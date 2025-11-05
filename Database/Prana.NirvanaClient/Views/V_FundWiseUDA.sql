

/*=============================================
-- Modified By: om shiv
-- Modification Date: Nov, 2014
-- Description: Get Fund wise UDA
-- =============================================*/

CREATE VIEW [dbo].[V_FundWiseUDA]          
AS          
SELECT 
F.FundID,
F.Symbol_pk,
F.UDAAssetClassID,
F.UDASecurityTypeID,
F.UDASectorID,
F.UDASubSectorID,
F.UDACountryID,
F.IsApproved,
F.PrimarySymbol,
ISNULL(T_UDAAssetClass.AssetName, 'Undefined') AS AssetName,
ISNULL(T_UDASecurityType.SecurityTypeName, 'Undefined') AS SecurityTypeName,
ISNULL(T_UDASector.SectorName, 'Undefined') AS SectorName,
ISNULL(T_UDASubSector.SubSectorName, 'Undefined') AS SubSectorName,
ISNULL(T_UDACountry.CountryName, 'Undefined') AS CountryName

from [$(SecurityMaster)].dbo.T_FundWiseUDAData F
INNER JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable AS SM ON SM.Symbol_PK = F.Symbol_pk
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDAAssetClass as T_UDAAssetClass ON T_UDAAssetClass.AssetID = SM.UDAAssetClassID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASecurityType as T_UDASecurityType ON T_UDASecurityType.SecurityTypeID = SM.UDASecurityTypeID 
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASector as T_UDASector ON T_UDASector.SectorID = SM.UDASectorID 
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASubSector as T_UDASubSector ON T_UDASubSector.SubSectorID = SM.UDASubSectorID 
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDACountry as T_UDACountry ON SM.UDACountryID = T_UDACountry.CountryID -- left outer join
WHERE F.IsApproved ='TRUE'    
 
