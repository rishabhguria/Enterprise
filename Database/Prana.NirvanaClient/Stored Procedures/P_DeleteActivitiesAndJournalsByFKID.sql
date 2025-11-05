-- =============================================
-- Author:		<Author,,Nishant Kumar Jain>
-- Create date: <Create Date,,2015-03-18>
-- Description:	<Description,,To delete the activities and journals on basis of FKIDs>
-- =============================================
CREATE PROCEDURE [dbo].[P_DeleteActivitiesAndJournalsByFKID] (@FKIDs VARCHAR(Max))
AS
BEGIN TRAN Trans

BEGIN TRY
	CREATE TABLE #FKID (FKIDs VARCHAR(50))

	INSERT INTO #FKID
	SELECT Items AS FKIDs
	FROM dbo.Split(@FKIDs, ',')

	CREATE TABLE #Activity (ActivityID VARCHAR(50))

	INSERT INTO #Activity
	SELECT ActivityID
	FROM T_AllActivity
	INNER JOIN #FKID ON T_AllActivity.FKID = #FKID.FKIDs

	DELETE J
	FROM T_Journal J
	INNER JOIN #Activity ON #Activity.ActivityID = J.ActivityId_FK

	DELETE A
	FROM T_AllActivity A
	INNER JOIN #FKID ON A.FKID = #FKID.FKIDs

	DROP TABLE #FKID

	DROP TABLE #Activity

	COMMIT TRANSACTION Trans
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION Trans
END CATCH;
