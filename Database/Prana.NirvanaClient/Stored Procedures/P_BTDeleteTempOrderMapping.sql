-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Delete Order Side Mapping for a given Template
-- =============================================
CREATE PROCEDURE P_BTDeleteTempOrderMapping
(
@templateID varchar(200)
)
as
delete T_BTOrderSideMapping
where TemplateID = @TemplateID
