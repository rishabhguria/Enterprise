
 -- Fectching all fields from T_ImportTag table
     --Author  Aman Seth     

CREATE PROCEDURE [dbo].[P_GetImportTag]

AS
BEGIN
	SELECT   Acronym, ImportTagName
    FROM     T_ImportTag 
END
