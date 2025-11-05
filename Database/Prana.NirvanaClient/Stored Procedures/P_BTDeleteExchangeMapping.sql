
-- =============================================
-- Author:		<harshkumar>
-- Create date: <18/10/2006>
-- Description:	<Deletes Exchange Mapping corresponding to a given Template>
-- =============================================
CREATE PROCEDURE [dbo].[P_BTDeleteExchangeMapping]
(
@templateID varchar(200)
)
as
delete T_BTExchangeMapping
where TemplateID = @TemplateID
