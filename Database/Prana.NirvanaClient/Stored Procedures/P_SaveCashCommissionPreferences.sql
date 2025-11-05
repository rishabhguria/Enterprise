CREATE PROCEDURE [dbo].[P_SaveCashCommissionPreferences]
	(
	   @csv varchar(max)
	)
AS
BEGIN

CREATE TABLE #Preferences 

 (
    ID int identity(1,1),
	IsChecked bit
 )
	
INSERT INTO #Preferences
SELECT Items AS IsChecked
FROM dbo.Split(@csv, ',')

UPDATE T_CashPreferencesforCommission
SET Ischecked = P.IsChecked
FROM #Preferences P
Inner JOIN T_CashPreferencesforCommission CP
ON P.ID = CP.ID

DROP TABLE #Preferences

END