Update T_UserTTGeneralPreferences set QuantityType= 0 where QuantityType is NULL
Update T_CompanyTTGeneralPreferences set QuantityType=0 where QuantityType is NULL
ALTER table T_UserTTGeneralPreferences  ALTER COLUMN QuantityType TINYINT NOT NULL
ALTER table T_CompanyTTGeneralPreferences  ALTER COLUMN QuantityType TINYINT NOT NULL