GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetCashAccruals]    Script Date: 05/13/2015 16:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
--Delete From P_NT_GetCashAccruals
--Insert Into T_NT_CashAccruals
--Exec P_NT_GetCashAccruals '01/01/2015','05/01/2015'
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetCashAccruals] 
-- Add the parameters for the stored procedure here
@FromDate DateTime,
@ToDate DateTime,
@FundIDSymbol xml = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
Create Table #FundIDSymbol
(FundID int,Fund varchar(Max),Symbol varchar(Max))
Insert Into #FundIDSymbol
(FundID,Fund,Symbol)
Select 
ref.value('FundID[1]','int') As FundID,
ref.value('Fund[1]','varchar(Max)') As Fund,
ref.value('Symbol[1]','varchar(Max)') As Symbol 
From @FundIDSymbol.nodes('/FundIDSymbol') xmlData(ref)

If Not Exists (Select FundID,Fund,Symbol From #FundIDSymbol)
Begin 
	Insert Into #FundIDSymbol 
	(FundID,Fund) 
	Select CompanyFundID,FundName From T_CompanyFunds 
End
--------------------------------------------------------------------------------------------------------- 
Create Table #CashAccruals
(AcctId int,AcctName varchar(Max),RunDate datetime,CashOrAccruals bit,TradeCurrency varchar(Max),BeginningMarketValueLocal float,EndingMarketValueLocal float,BeginningFXRate float,EndingFXRate float)
Insert Into #CashAccruals 
(AcctId,AcctName,RunDate,CashOrAccruals,TradeCurrency,BeginningMarketValueLocal,EndingMarketValueLocal,BeginningFXRate,EndingFXRate) 
Select FS.FundId As AcctId,FS.Fund As AcctName,RunDate,0,TradeCurrency,BeginningMarketValueLocal,EndingMarketValueLocal,BeginningFXRate,EndingFXRate 
From T_MW_GenericPNL A 
Join #FundIDSymbol FS On
(FS.Fund = A.Fund) And 
(FS.Symbol = A.Symbol Or FS.Symbol Is Null) 
Where  
(A.Asset = 'Cash' And A.Open_CloseTag = 'O') And 
(Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0) 
Insert Into #CashAccruals 
(AcctId,AcctName,RunDate,CashOrAccruals,TradeCurrency,BeginningMarketValueLocal,EndingMarketValueLocal,BeginningFXRate,EndingFXRate) 
Select FS.FundId As AcctId,FS.Fund As AcctName,RunDate,1,TradeCurrency,BeginningMarketValueLocal,EndingMarketValueLocal,BeginningFXRate,EndingFXRate 
From T_MW_GenericPNL A 
Join #FundIDSymbol FS On
(FS.Fund = A.Fund) And 
(FS.Symbol = A.Symbol Or FS.Symbol Is Null) 
Where  
(A.Open_CloseTag = 'Accruals') And 
(Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0) 

Select AcctId,AcctName,RunDate,CashOrAccruals,TradeCurrency,BeginningMarketValueLocal,EndingMarketValueLocal,BeginningFXRate,EndingFXRate From #CashAccruals 

Drop Table #CashAccruals
Drop Table #FundIDSymbol

END

GO