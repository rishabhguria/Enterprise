GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetTradingLevels]    Script Date: 05/13/2015 16:36:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetTradingLevels '09/01/2014','09/15/2014',1
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetTradingLevels] 
@StartDate datetime,
@EndDate datetime,
@Approved bit = 1
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here 

Create Table #AcctValues 
(AcctId int Not Null,AcctName varchar(Max) Not Null,UpdateDate datetime Not Null,Val_TradingLevel decimal(38,8) Not Null,Val_InvestedCash decimal(38,8) Not Null) 
Create Table #PreviousEntries 
(AcctId int Not Null,UpdateDate datetime Not Null)
If @Approved = 1 
Begin 
	Insert Into #AcctValues 
	(AcctId,AcctName,UpdateDate,Val_TradingLevel,Val_InvestedCash) 
	Select B.CompanyFundID,B.FundName,A.UpdateDate,A.Val_TradingLevel,A.Val_InvestedCash
	From T_NT_AcctVal A Join T_CompanyFunds B On A.AcctId = B.CompanyFundID Where IsNull(A.Approved,0) = 1 And A.UpdateDate Between @StartDate And @EndDate 
	Insert Into #PreviousEntries 
	(AcctId,UpdateDate)
	Select A.AcctId,Max(A.UpdateDate) 
	From T_NT_AcctVal A Where IsNull(A.Approved,0) = 1 And A.UpdateDate < @StartDate Group By A.AcctId 
	Insert Into #AcctValues
	(AcctId,AcctName,UpdateDate,Val_TradingLevel,Val_InvestedCash) 
	Select C.CompanyFundID,C.FundName,IsNull(A.UpdateDate,'01/01/1753'),IsNull(B.Val_TradingLevel,0),IsNull(B.Val_InvestedCash,0)
	From #PreviousEntries A Join T_NT_AcctVal B On A.AcctId = B.AcctId Right Outer Join T_CompanyFunds C On A.AcctId = C.CompanyFundID 
End 
Else 
Begin 
	Insert Into #AcctValues 
	(AcctId,AcctName,UpdateDate,Val_TradingLevel,Val_InvestedCash) 
	Select B.CompanyFundID,B.FundName,A.UpdateDate,A.Val_TradingLevel,A.Val_InvestedCash
	From T_NT_AcctVal A Join T_CompanyFunds B On A.AcctId = B.CompanyFundID Where A.UpdateDate Between @StartDate And @EndDate 
	Insert Into #PreviousEntries 
	(AcctId,UpdateDate)
	Select A.AcctId,Max(A.UpdateDate) 
	From T_NT_AcctVal A Where A.UpdateDate < @StartDate Group By A.AcctId 
	Insert Into #AcctValues
	(AcctId,AcctName,UpdateDate,Val_TradingLevel,Val_InvestedCash) 
	Select C.CompanyFundID,C.FundName,IsNull(A.UpdateDate,'01/01/1753'),IsNull(B.Val_TradingLevel,0),IsNull(B.Val_InvestedCash,0)
	From #PreviousEntries A Join T_NT_AcctVal B On A.AcctId = B.AcctId Right Outer Join T_CompanyFunds C On A.AcctId = C.CompanyFundID 
End 

Select AcctId,AcctName,UpdateDate,Val_TradingLevel,Val_InvestedCash From #AcctValues
 
Drop Table #AcctValues,#PreviousEntries

END

GO