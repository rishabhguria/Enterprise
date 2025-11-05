-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	checks if the given template is in use by a basket
-- =============================================
CREATE PROCEDURE P_BTCheckIfTemplateInUse
(
@templateID varchar(200)
)
AS
Begin 
Declare @Exists bit  -- return value
if((select count(*) from T_BTSavedBaskets where TemplateID = @templateID)= 0)
	select @Exists = 0
else 
	select @Exists = 1

return @Exists
end

