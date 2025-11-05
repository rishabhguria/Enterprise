-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	deletes Template from DB/ takes parameter TemplateID
-- =============================================
CREATE PROCEDURE P_BTDeleteTemplate
(
@templateID varchar(200)
)
as
delete T_BTTemplateList 
where TemplateID = @templateID