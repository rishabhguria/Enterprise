/*   -- Fectching data based on User ID
     -- Author : bhavana 
     -- Dated : 17 April 2014  
*/

/****** Object:  Stored Procedure dbo.P_GroupFundAssociation    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GroupFundAssociation]
(
@groupID int
)
AS
	select Tfg.FundGroupID, TM.FundID from T_GroupFundMapping TM 
    inner JOIN T_FundGroups Tfg on TM.FundGroupID = Tfg.FundGroupID
    inner JOIN T_CompanyFunds TF on TF.Companyfundid = TM.FundID
    where Tfg.FundGroupID = @groupID and TF.IsActive = 1 
