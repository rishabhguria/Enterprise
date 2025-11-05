UPDATE [dbo].[T_AttributeNames]
SET AttributeName = 'Trade Attribute 0' + RIGHT(AttributeValue, 1)
WHERE AttributeValue LIKE 'Trade Attribute [1-9]'
	AND AttributeName = AttributeValue;