
CREATE PROCEDURE [dbo].[P_UDA_CheckMasterValueAssigned]
	@Tag VARCHAR(MAX),
	@value VARCHAR(MAX)

AS	
BEGIN

Set nocount on
DECLARE @CheckString VARCHAR(MAX)
SET @CheckString = '	DECLARE @res INT 
		SELECT TOP 1 @res = 1 FROM  T_UDA_DynamicUDAData WHERE [' + @Tag + '] = ''' + REPLACE(@value,'''','''''') +'''

		SELECT TOP 1 @res = 1 FROM T_UDA_DynamicUDA WHERE DefaultValue = ''' + REPLACE(@value,'''','''''') + ''' AND Tag = '''+ @Tag 

		+ ''' SELECT @res'

EXEC(@CheckString)
END

