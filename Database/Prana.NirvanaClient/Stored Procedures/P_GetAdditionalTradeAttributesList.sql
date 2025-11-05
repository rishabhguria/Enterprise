CREATE PROCEDURE [dbo].[P_GetAdditionalTradeAttributesList]
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Results (
        AttributeNumber INT,
        DataItem NVARCHAR(MAX)
    );

    -- Step 1: Extract and insert distinct TradeAttribute(7-45) values from T_Group.AdditionalTradeAttributes into the #Results table
    INSERT INTO #Results (AttributeNumber, DataItem)
    SELECT DISTINCT
        CAST(TRIM(REPLACE(A.AttributeValue, 'Trade Attribute', '')) AS INT) AS AttributeNumber,
        JSON_VALUE(Parsed.[value], '$.Value') AS DataItem
    FROM T_Group
    CROSS APPLY OPENJSON(AdditionalTradeAttributes) AS Parsed
    INNER JOIN T_AttributeNames A
        ON REPLACE(A.AttributeValue, ' ', '') = REPLACE(JSON_VALUE(Parsed.[value], '$.Name'), ' ', '')
    WHERE JSON_VALUE(Parsed.[value], '$.Value') IS NOT NULL
      AND JSON_VALUE(Parsed.[value], '$.Value') <> '';

    -- Step 2: Return one row per unique AttributeNumber and DataItem pair
    -- Returned columns:
    --   AttributeNumber (INT): the numeric part of the attribute name (e.g., 7 to 45)
    --   DataItem (NVARCHAR): an individual, trimmed value from either T_Group
    SELECT DISTINCT AttributeNumber, DataItem
    FROM #Results
    ORDER BY AttributeNumber;

    DROP TABLE #Results;
END;