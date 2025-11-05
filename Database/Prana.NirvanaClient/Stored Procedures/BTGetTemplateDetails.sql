
-- =============================================
-- Author:		<Harsh Kumar>
-- Create date: <12/10/2006>
-- Description:	Returs Template Details assetID,underlyingID,columnList 
-- =============================================
CREATE PROCEDURE [dbo].[BTGetTemplateDetails] (
@templateID varchar(200)
)
as
select TemplateName,Columns,OSM.SideMappingString,
isnull(IsDefaultTemplate,'False'),EM.ExchangeMappingList
from 
(T_BTTemplateList as TL inner join T_BTOrderSideMapping as OSM
on TL.TemplateID=OSM.TemplateID) left join T_BTExchangeMapping as EM
on TL.TemplateID=EM.TemplateID

where TL.TemplateID=@templateID
