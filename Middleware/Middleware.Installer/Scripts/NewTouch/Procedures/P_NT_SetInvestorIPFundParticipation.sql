GO
/****** Object:  StoredProcedure [dbo].[P_NT_SetInvestorIPFundParticipation]    Script Date: 05/13/2015 16:36:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_SetInvestorIPFundParticipation '05/04/2014','Investor 3','RJ OASIS','Investor',0.22,0.22
Exec P_NT_SetInvestorIPFundParticipation '05/04/2014','Fund IP 1','RJ OASIS','Fund Independent Party',0.22,0.22
Exec P_NT_SetInvestorIPFundParticipation '05/04/2014','Account IP 3','Account Proxy of Account 3','Account Proxy Independent Party',0.22,0.22 

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
CREATE PROCEDURE [dbo].[P_NT_SetInvestorIPFundParticipation] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@InvestorIPName varchar(Max),
	@FundName varchar(Max),
	@GroupTypeName varchar(Max),
	@Part_TradingLevel decimal(9,8),
	@Part_InvestedCash decimal(9,8)
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
		Select B.InvestorIPId,B.UpdateDate,B.Part_TradingLevel,B.Part_InvestedCash 
		Into #InvestorSnapshot_1 From #InvestorIPPeers A Join T_NT_InvestorIPFundPart B On A.Id = B.InvestorIPId And FundId = @FundId And UpdateDate <= @UpdateDate
		Select InvestorIPId,Part_TradingLevel,Part_InvestedCash 
		Into #InvestorSnapshot From #InvestorSnapshot_1 Where UpdateDate = (Select Max(UpdateDate) From #InvestorSnapshot_1)

		Delete From #InvestorSnapshot Where InvestorIPId = @InvestorIPId
		Insert Into #InvestorSnapshot 
		(InvestorIPId,Part_TradingLevel,Part_InvestedCash)
		Select @InvestorIPId,@Part_TradingLevel,@Part_InvestedCash

		Delete From T_NT_InvestorIPFundPart Where FundId = @FundId And UpdateDate = @UpdateDate
		Insert Into T_NT_InvestorIPFundPart 
		(InvestorIPId,FundId,UpdateDate,Part_TradingLevel,Part_InvestedCash)
		Select InvestorIPId,@FundId,@UpdateDate,Part_TradingLevel,Part_InvestedCash From #InvestorSnapshot

		--	Select InvestorIPId,Part_TradingLevel,Part_InvestedCash From #InvestorSnapshot
		Drop Table #InvestorSnapshot_1,#InvestorSnapshot
	End
	Else If @InvestorIPGroupTypeId = 5 And @FundGroupTypeId = 1 
	Begin 
		Select B.FundId,B.UpdateDate,B.Part_TradingLevel,B.Part_InvestedCash 
		Into #FundIPSnapshot_1 From #FundPeers A Join T_NT_InvestorIPFundPart B On A.Id = B.FundId And InvestorIPId = @InvestorIPId And UpdateDate <= @UpdateDate
		Select FundId,Part_TradingLevel,Part_InvestedCash 
		Into #FundIPSnapshot From #FundIPSnapshot_1 Where UpdateDate = (Select Max(UpdateDate) From #FundIPSnapshot_1)

		Delete From #FundIPSnapshot Where FundId = @FundId
		Insert Into #FundIPSnapshot 
		(FundId,Part_TradingLevel,Part_InvestedCash)
		Select @FundId,@Part_TradingLevel,@Part_InvestedCash

		Delete From T_NT_InvestorIPFundPart Where InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		Insert Into T_NT_InvestorIPFundPart 
		(InvestorIPId,FundId,UpdateDate,Part_TradingLevel,Part_InvestedCash)
		Select @InvestorIPId,FundId,@UpdateDate,Part_TradingLevel,Part_InvestedCash From #FundIPSnapshot

		--	Select FundId,Part_TradingLevel,Part_InvestedCash From #FundIPSnapshot
		Drop Table #FundIPSnapshot_1,#FundIPSnapshot 
	End 
	Else If @InvestorIPGroupTypeId = 6 And @FundGroupTypeId = 2 
	Begin 
		Select B.FundId,B.UpdateDate,B.Part_TradingLevel,B.Part_InvestedCash 
		Into #AcctProxySnapshot_1 From #FundPeers A Join T_NT_InvestorIPFundPart B On A.Id = B.FundId And InvestorIPId = @InvestorIPId And UpdateDate <= @UpdateDate
		Select FundId,Part_TradingLevel,Part_InvestedCash 
		Into #AcctProxySnapshot From #AcctProxySnapshot_1 Where UpdateDate = (Select Max(UpdateDate) From #AcctProxySnapshot_1)

		Delete From #AcctProxySnapshot Where FundId = @FundId
		Insert Into #AcctProxySnapshot 
		(FundId,Part_TradingLevel,Part_InvestedCash)
		Select @FundId,@Part_TradingLevel,@Part_InvestedCash

		Delete From T_NT_InvestorIPFundPart Where InvestorIPId = @InvestorIPId And UpdateDate = @UpdateDate
		Insert Into T_NT_InvestorIPFundPart 
		(InvestorIPId,FundId,UpdateDate,Part_TradingLevel,Part_InvestedCash)
		Select @InvestorIPId,FundId,@UpdateDate,Part_TradingLevel,Part_InvestedCash From #AcctProxySnapshot

		--	Select FundId,Part_TradingLevel,Part_InvestedCash From #AcctProxySnapshot
		Drop Table #AcctProxySnapshot_1,#AcctProxySnapshot 
	End 
End
Drop Table #InvestorIPPeers,#FundPeers

END


GO