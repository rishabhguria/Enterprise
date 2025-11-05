-- =============================================      
-- Modified By: om shiv
-- Modification Date: NOV, 2013
-- Description: Get UDA details from T_SMSymbolLookUpTable 
-- =============================================      
CREATE view  [dbo].[V_GetSymbolUDAData] 
as  
  
SELECT		T_SM.TickerSymbol, 
			T_SM.UDAAssetClassID, 
			ISNULL(T_UDAAssetClass.AssetName, 'Undefined') AS AssetName,   
            T_SM.UDASecurityTypeID, 
			ISNULL(T_UDASecurityType.SecurityTypeName, 'Undefined') AS SecurityTypeName,   
            T_SM.UDASectorID, ISNULL(T_UDASector.SectorName, 'Undefined') AS SectorName, 
			T_SM.UDASubSectorID,   
            ISNULL(T_UDASubSector.SubSectorName, 'Undefined') AS SubSectorName, 
			T_SM.UDACountryID,   
            ISNULL(T_UDACountry.CountryName, 'Undefined') AS CountryName
            --SecData.CompanyName
FROM        [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable AS T_SM LEFT OUTER JOIN  
			[$(SecurityMaster)].dbo.T_UDAAssetClass  as T_UDAAssetClass ON T_UDAAssetClass.AssetID = T_SM.UDAAssetClassID LEFT OUTER JOIN  
			[$(SecurityMaster)].dbo.T_UDASecurityType as T_UDASecurityType ON T_UDASecurityType.SecurityTypeID = T_SM.UDASecurityTypeID LEFT OUTER JOIN  
			[$(SecurityMaster)].dbo.T_UDASector as T_UDASector ON T_UDASector.SectorID = T_SM.UDASectorID LEFT OUTER JOIN  
			[$(SecurityMaster)].dbo.T_UDASubSector as T_UDASubSector  ON T_UDASubSector.SubSectorID = T_SM.UDASubSectorID LEFT OUTER JOIN  
			[$(SecurityMaster)].dbo.T_UDACountry as T_UDACountry ON T_SM.UDACountryID = T_UDACountry.CountryID       --  left outer join 
                 --V_SecMasterData as SecData on SecData.TickerSymbol=UDA.TickerSymbol
