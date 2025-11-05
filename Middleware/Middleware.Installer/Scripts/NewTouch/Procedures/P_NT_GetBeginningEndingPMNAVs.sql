GO
-- =============================================
/* Usage:
Exec P_NT_GetBeginningEndingPMNAVs '09/01/2014','09/15/2014',0
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetBeginningEndingPMNAVs]
@StartDate datetime,
@EndDate datetime,
@BeginningOrEnding bit
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here

Create Table #AcctValues 
(AcctId int Not Null,AcctName varchar(Max) Not Null,UpdateDate datetime Not Null,Val_PMNAV float Not Null)
Create Table #PreviousEntries 
(AcctId int Not Null,UpdateDate datetime Not Null)
Insert Into #AcctValues 
(AcctId,AcctName,UpdateDate,Val_PMNAV) 
Select B.CompanyFundID,B.FundName,Case @BeginningOrEnding When 1 Then A.Date Else DateAdd(d,1,A.Date) End,A.NAVValue 
From PM_NAVValue A Join T_CompanyFunds B On A.FundID = B.CompanyFundID Where (Case @BeginningOrEnding When 1 Then A.Date Else DateAdd(d,1,A.Date) End) Between @StartDate And @EndDate 
Insert Into #PreviousEntries 
(AcctId,UpdateDate)
Select A.FundID,Max(A.Date) 
From PM_NAVValue A Where (Case @BeginningOrEnding When 1 Then A.Date Else DateAdd(d,1,A.Date) End) < @StartDate Group By A.FundID 
Insert Into #AcctValues
(AcctId,AcctName,UpdateDate,Val_PMNAV) 
Select C.CompanyFundID,C.FundName,IsNull(A.UpdateDate,'01/01/1753'),IsNull(B.NAVValue ,0)
From #PreviousEntries A Join PM_NAVValue B On A.AcctId = B.FundID Right Outer Join T_CompanyFunds C On A.AcctId = C.CompanyFundID 

Select AcctId,AcctName,UpdateDate,Val_PMNAV From #AcctValues
 
Drop Table #AcctValues,#PreviousEntries

END

GO