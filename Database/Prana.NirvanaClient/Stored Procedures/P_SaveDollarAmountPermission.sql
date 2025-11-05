CREATE PROCEDURE [dbo].[P_SaveDollarAmountPermission]
	(
	@TT bit,
	@PTT bit
	)
AS

if((select count(*) from T_DollarAmountPermission)=0)
BEGIN
INSERT INTO [dbo].[T_DollarAmountPermission]
(
[TT],
[PTT]
)
VALUES
(
@TT,
@PTT
)
END

ELSE 
BEGIN
	UPDATE T_DollarAmountPermission
	SET TT = @TT ,
	PTT=@PTT
END
RETURN 0
