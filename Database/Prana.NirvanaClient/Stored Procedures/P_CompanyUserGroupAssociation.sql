/*   -- Fectching data CompanyUser Group Mapping on User ID
     -- Author : bhavana 
     -- Dated : 14 April 2014  
*/

/****** Object:  Stored Procedure dbo.P_CompanyUserGroupAssociation    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_CompanyUserGroupAssociation]
(
@userID int
)
AS
BEGIN
	select TU.UserID, TM.FundGroupID from T_CompanyUserFundGroupMapping TM 
    inner JOIN T_CompanyUser TU on TM.CompanyUserID = TU.UserID
    where TU.UserID = @userID
END
