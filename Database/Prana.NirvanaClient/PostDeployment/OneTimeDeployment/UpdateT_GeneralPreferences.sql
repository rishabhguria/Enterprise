IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_GeneralPreferences')
BEGIN 

INSERT INTO T_GeneralPreferences (UserID, IsShowServiceIcons)
SELECT UserID,
CASE
WHEN Login like 'Support%' THEN 1
    ELSE 0
END AS IsShowServiceIcons
 FROM T_CompanyUser

End