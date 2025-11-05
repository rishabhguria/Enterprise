GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetGroupTypeWiseAcctAssociations]    Script Date: 05/13/2015 16:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Exec [P_NT_GetGroupTypeWiseAcctAssociations] @GroupTypeId = 1
--Exec [P_NT_GetGroupTypeWiseAcctAssociations] @GroupTypeName = 'Account Proxy'
-- Select * From T_NT_GroupType
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetGroupTypeWiseAcctAssociations] 
-- Add the parameters for the stored procedure here
@GroupTypeId int = Null,@GroupTypeName varchar(Max) = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here 

Create Table #AcctAssociations 
(FundGroupTypeId int Not Null,FundGroupTypeName varchar(Max) Not Null,FundId int Not Null,FundName varchar(Max) Not Null,AcctId int Not Null,AcctName varchar(Max) Not Null)

Declare @FundGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
Select @FundGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
Select @FundGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 
If @FundGroupTypeId Is Not Null 
Begin 
Insert Into #AcctAssociations 
(FundGroupTypeId,FundGroupTypeName,FundId,FundName,AcctId,AcctName)
Select D.Id,D.Name,C.CompanyMasterFundID,C.MasterFundName,A.CompanyFundID,A.FundName
From T_CompanyFunds A Join T_CompanyMasterFundSubAccountAssociation B Join T_CompanyMasterFunds C Join T_NT_GroupType D 
On D.Id = C.GroupTypeId On C.CompanyMasterFundID = B.CompanyMasterFundID On B.CompanyFundID = A.CompanyFundID 
Where D.Id = @FundGroupTypeId 
End 

Select FundGroupTypeId,FundGroupTypeName,FundId,FundName,AcctId,AcctName From #AcctAssociations

Drop Table #AcctAssociations

END



GO