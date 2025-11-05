-- =============================================    
-- Author:  Divya Bansal   
-- Create date: 2 april 2013   
-- Description: Attribute names can be changed by user. 
-- =============================================    
CREATE PROCEDURE [dbo].[P_GetAttributeNames]
AS
BEGIN
    SELECT 
        AttributeValue, 
        AttributeName, 
        KeepRecord, 
        DefaultValues 
    FROM 
        T_AttributeNames
    ORDER BY 
        TRY_CAST(SUBSTRING(AttributeValue, LEN('Trade Attribute ') + 1, LEN(AttributeValue)) AS INT);
END