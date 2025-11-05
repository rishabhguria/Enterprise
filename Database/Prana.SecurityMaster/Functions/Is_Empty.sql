



CREATE function [dbo].[Is_Empty]
(
@param1 varchar(100),
@param2 varchar(100)

)
RETURNS  varchar(100)
as
Begin
declare @result varchar(100)

if(@param1='')
set @result= @param2
else
set @result=@param1

return @result
end





