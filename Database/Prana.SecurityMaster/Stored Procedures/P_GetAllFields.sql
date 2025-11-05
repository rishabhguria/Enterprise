
/* -- Fectching all fields from T_SecurityField table
     --Author : bhavana 
    --Dated : 15 July 2014  
*/
CREATE PROCEDURE [dbo].[P_GetAllFields]

AS
BEGIN
	SELECT   FieldID, FieldName, IsRealTime, IsHistorical, Esignal, Bloomberg
    FROM     T_SecurityField where IsHistorical=1
    order by FieldName
END

