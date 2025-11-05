/*                
Created By : Pranay Deep on 04 Sep, 2015        
Description : Fetch saved data T_ReconGroupingCriteria.        
JIRA  : http://jira.nirvanasolutions.com:8080/browse/PRANA-10911             
*/
CREATE PROCEDURE [dbo].[P_ReconGroupingCriteria]
AS
BEGIN
	SELECT RT.ReconTypeName
		, GC.GroupingColumns
	FROM T_ReconGroupingCriteria AS GC
	LEFT JOIN T_ReconType AS RT ON RT.ReconTypeID = GC.ReconTypeID_FK
END
