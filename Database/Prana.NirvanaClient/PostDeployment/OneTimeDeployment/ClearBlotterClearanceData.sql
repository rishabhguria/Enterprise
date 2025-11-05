DELETE FROM T_CompanyClearanceCommonData;

DELETE FROM T_CompanyAUECClearanceTimeBlotter;

INSERT INTO T_CompanyClearanceCommonData (TimeZone,AutoClearing,BaseTime,CompanyID)
SELECT '(UTC-05:00) Eastern Time (US & Canada)' AS TimeZone, 'True' AS AutoClearing, DATEADD(day, DATEDIFF(Day, 0,  GETDATE()), 0) AS BaseTime, CompanyID FROM T_Company Where CompanyID > 0;

INSERT INTO T_CompanyAUECClearanceTimeBlotter (CompanyAUECID, ClearanceTime)
SELECT CompanyAUECID, DATEADD(day, DATEDIFF(Day, 0,  GETDATE()), 0) AS ClearanceTime FROM T_CompanyAUEC Where CompanyID > 0;

IF EXISTS (select * from sysobjects where name='GetUserTimeZoneString')
BEGIN
Drop Procedure GetUserTimeZoneString;
END

IF EXISTS (select * from sysobjects where name='P_GetBlotterLaunchDataNewest')
BEGIN
Drop Procedure P_GetBlotterLaunchDataNewest;
END

IF EXISTS (select * from sysobjects where name='V_GetAUECandUserBlotterClearanceData')
BEGIN
Drop View V_GetAUECandUserBlotterClearanceData;
END

IF EXISTS (select * from sysobjects where name='P_UpdateClearanceDates')
BEGIN
Drop Procedure P_UpdateClearanceDates;
END

IF EXISTS (select * from sysobjects where name='P_GetOrdersbyDateTemp')
BEGIN
Drop Procedure P_GetOrdersbyDateTemp;
END