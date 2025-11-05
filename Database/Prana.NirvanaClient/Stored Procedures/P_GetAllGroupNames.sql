/* -- Fectching data based on User ID
     --Author : bhavana 
    --Dated : 14 April 2014  
*/

/****** Object:  Stored Procedure dbo.P_GetAllGroupNames    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllGroupNames]

AS
BEGIN
	SELECT   FundGroupID, GroupName
    FROM     T_FundGroups
END
