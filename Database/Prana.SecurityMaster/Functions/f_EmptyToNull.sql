
create function f_EmptyToNull
(
@input varchar(20)
)
RETURNS  varchar(20)
as
Begin
declare @result varchar(20)

if(@input='')
set @result= null
else
set @result=@input

return @result
end

