-- =============================================
-- Created By: Bhavana
-- Creation Date: 11 July, 2014
-- Description: Get pricing data from T_SMPricingData
-- =============================================

create VIEW [dbo].[V_SMPricingData]
AS

WITH PricingData
     AS (SELECT Symbol_PK,
                Date,
                Source,
                SecondarySource,
                NirvanaSymbol,
                PricingType = C.value('local-name(.)', 'VARCHAR(50)'),
                [Price] = C.value('./.', 'FLOAT')
         FROM   [$(SecurityMaster)].dbo.T_SMPricingData
                CROSS APPLY PricingXml.nodes('/Fields/*') AS T(C))

SELECT 
Symbol_PK,
Date,
Source, 
SecondarySource,
NirvanaSymbol as Symbol, 
PricingType, 
[Price]
FROM PricingData;

