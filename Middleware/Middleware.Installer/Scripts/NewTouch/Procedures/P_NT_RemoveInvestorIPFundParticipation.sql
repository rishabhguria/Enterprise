GO
/****** Object:  StoredProcedure [dbo].[P_NT_RemoveInvestorIPFundParticipation]    Script Date: 05/13/2015 16:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_RemoveInvestorIPFundParticipation '05/04/2014','Investor 3','RJ OASIS','Investor'
Exec P_NT_RemoveInvestorIPFundParticipation '05/04/2014','Fund IP 1','RJ OASIS','Fund Independent Party'
Exec P_NT_RemoveInvestorIPFundParticipation '05/04/2014','Account IP 3','Account Proxy of Account 3','Account Proxy Independent Party' 

Parameter @GroupTypeName refers to the one for investorip.
*/
/* Notes:
When a Fund's Participation by an Investor is being inserted/updated, that Fund's Participations by other Investors should be inserted/updated at the same time, so that at retrieval missing relationships means zero participation. 
Besides, we will need to ensure 100% Participation.
While a FundIP's Participation in a Fund is being inserted/updated, that FundIP's Participations in other Funds should be inserted/updated at the same time, so that at retrieval missing relationships means zero participation.
While a AcctProxyIP's Participation in a Fund is being inserted/updated, that AcctProxyIP's Participations in other Funds should be inserted/updated at the same time, so that at retrieval missing relationships means zero participation.

After a batch of inserts, the whole batch can be verified by: 
Exec P_NT_ValidateInvestorIPFundParticipation 
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RemoveInvestorIPFundParticipation] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@InvestorIPName varchar(Max),
	@FundName varchar(Max),
	@GroupTypeName varchar(Max)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
Declare @InvestorIPGroupTypeId int 
Select @InvestorIPGroupTypeId = Id 
From T_NT_GroupType Where [Name] = @GroupTypeName 
Select Id,[Name] 
Into #InvestorIPPeers From T_NT_InvestorIP 
Where GroupTypeId = @InvestorIPGroupTypeId 
Declare @InvestorIPId int
Select @InvestorIPId = Id 
From #InvestorIPPeers 
Where [Name] = @InvestorIPName 
Declare @FundGroupTypeId int 
If @InvestorIPGroupTypeId = 4
Begin 
	Select @FundGroupTypeId = 1 
End
Else If @InvestorIPGroupTypeId = 5 
Begin 
	Select @FundGroupTypeId = 1 
End 
Else If @InvestorIPGroupTypeId = 6  
Begin 
	Select @FundGroupTypeId = 2 
End
Select CompanyMasterFundID As Id,MasterFundName As [Name] 
Into #FundPeers From T_CompanyMasterFunds 
Where GroupTypeId = @FundGroupTypeId 
Declare @FundId int
Select @FundId = Id 
From #FundPeers 
Where [Name] = @FundName 

If (@InvestorIPId Is Not Null) And (@FundId Is Not Null) 
Begin
	Delete From T_NT_InvestorIPFundPart Where InvestorIPId = @InvestorIPId And FundId = @FundId And UpdateDate = @UpdateDate And IsNull(Approved,0) = 0
End

Drop Table #InvestorIPPeers,#FundPeers

END


GO