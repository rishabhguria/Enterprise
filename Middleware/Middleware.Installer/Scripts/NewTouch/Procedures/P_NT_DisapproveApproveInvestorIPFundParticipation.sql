GO
/****** Object:  StoredProcedure [dbo].[P_NT_DisapproveApproveInvestorIPFundParticipation]    Script Date: 05/13/2015 16:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_DisapproveApproveInvestorIPFundParticipation '05/04/2014','Investor 3','RJ OASIS','Investor',1
Exec P_NT_DisapproveApproveInvestorIPFundParticipation '05/04/2014','Fund IP 1','RJ OASIS','Fund Independent Party',1
Exec P_NT_DisapproveApproveInvestorIPFundParticipation '05/04/2014','Account IP 3','Account Proxy of Account 3','Account Proxy Independent Party',1

Exec P_NT_DisapproveApproveInvestorIPFundParticipation '5/4/2014',
'Investor 2','Access OASIS 1-2-3 (Fund)','Investor',0

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
CREATE PROCEDURE [dbo].[P_NT_DisapproveApproveInvestorIPFundParticipation] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@InvestorIPName varchar(Max),
	@FundName varchar(Max),
	@GroupTypeName varchar(Max),
	@DisapproveApprove bit
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
	If @InvestorIPGroupTypeId = 4 And @FundGroupTypeId = 1 
	Begin 
		Declare @Sum_Part_TradingLevel decimal(9,8),@Sum_Part_InvestedCash decimal(9,8)
		Select @Sum_Part_TradingLevel = Sum(Part_TradingLevel),@Sum_Part_InvestedCash = Sum(Part_InvestedCash)
		From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
		Where GroupTypeId = @InvestorIPGroupTypeId And FundId = @FundId And UpdateDate = @UpdateDate 
		If @Sum_Part_TradingLevel = 1 And @Sum_Part_InvestedCash = 1 
		Begin 
			If @DisapproveApprove = 0 
			Begin 
				Update A Set Approved = 0 
				From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
				Where GroupTypeId = @InvestorIPGroupTypeId And FundId = @FundId And UpdateDate = @UpdateDate 
			End 
			Else If @DisapproveApprove = 1 
			Begin 
				Update A Set Approved = 1 
				From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
				Where GroupTypeId = @InvestorIPGroupTypeId And FundId = @FundId And UpdateDate = @UpdateDate 
			End 
		End 
	End 
	Else If @InvestorIPGroupTypeId = 5 And @FundGroupTypeId = 1 
	Begin
		If @DisapproveApprove = 0 
		Begin 
			Update A Set Approved = @DisapproveApprove 
			From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
			Where GroupTypeId = @InvestorIPGroupTypeId And InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		End 
		Else If @DisapproveApprove = 1 
		Begin 
			Update A Set Approved = @DisapproveApprove 
			From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
			Where GroupTypeId = @InvestorIPGroupTypeId And InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		End 
	End 
	Else If @InvestorIPGroupTypeId = 6 And @FundGroupTypeId = 2 
	Begin 
		If @DisapproveApprove = 0 
		Begin 
			Update A Set Approved = @DisapproveApprove 
			From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
			Where GroupTypeId = @InvestorIPGroupTypeId And InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		End
		Else If @DisapproveApprove = 1 
		Begin 
			Update A Set Approved = @DisapproveApprove 
			From T_NT_InvestorIPFundPart A Join T_NT_InvestorIP B On A.InvestorIPId = B.Id 
			Where GroupTypeId = @InvestorIPGroupTypeId And InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		End 
	End
End
Drop Table #InvestorIPPeers,#FundPeers

END


GO