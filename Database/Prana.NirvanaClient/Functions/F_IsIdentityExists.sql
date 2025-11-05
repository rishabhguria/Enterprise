       

--===========================================================
-- Author: Rahul Gupta
-- Date: 2013,Feb'07
-- Description: Check whether a table has an identity column or not in order to set identity_insert on/off.
--===========================================================
CREATE FUNCTION F_IsIdentityExists(@tableName varchar(100))
Returns bit as

BEGIN
Declare @Identity bit

-- Return 1 if identity column exists else 0
Declare @count int
SET @count = (SELECT COUNT(name) AS HasIdentity
FROM syscolumns
WHERE OBJECT_NAME(id) = @tableName
AND COLUMNPROPERTY(id, name, 'IsIdentity') = 1)

IF @count > 0
SET @Identity = 1
ELSE
SET @Identity = 0

RETURN @Identity
END

