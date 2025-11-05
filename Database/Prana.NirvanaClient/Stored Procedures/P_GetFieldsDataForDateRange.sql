CREATE Procedure P_GetFieldsDataForDateRange    
(        
@StartDate Datetime,        
@EndDate Datetime,        
@DeletePrevious Bit        
)        
As        
        
SELECT           
  FundID,           
  SubAccountID,           
  CurrencyID,           
  TransactionDate,           
  Sum(DayDrBase) * (-1) as CurrentDr,    
  Sum(DayCrBase) as CurrentCr    
into #TempDATA        
 FROM DBO.T_SubAccountBalances WITH(NOLOCK)       
 GROUP BY FundID, SubAccountID, CurrencyID, TransactionDate          
order by TransactionDate        
        
        
--UPDATE #TempDATA        
--SET CurrentDr = (-1) * CurrentDr    
    
Declare @MinDate DATETIME          
Declare @MaxDate DATETIME          
          
Select @MinDate=MIN(TransactionDate) from #TempData          
/*          
ASK          
MAX date there is a doubt should it be Current or MAX From T_JOURNAL as following is a froblem here          
Select * FROM DBO.T_Journal where Transactiondate = (Select MAX(TransactionDate) from T_Journal)          
*/          
Select @MaxDate=GETDATE() from #TempData        
        
--Select * from #TempDATA         
        
--------------------------------------Fields        
Select FieldID,FieldName into #TempFinalData1 from T_FieldsPSR        
--Select * from #TempFinalData1        
        
--------------------------------------Funds        
Select Distinct FundID into #TempFinalData2 from #TempDATA         
--Select * from #TempFinalData2        
        
--------------------------------------Dates        
;WITH mycte AS        
(        
  SELECT /*CAST('2011-01-01' AS DATETIME)*/ @MinDate DateValue        
  UNION ALL        
  SELECT  DateValue + 1        
  FROM    mycte           
  WHERE   DateValue + 1 < @MaxDate        
)        
        
SELECT  DateValue into #TempFinalData3        
FROM    mycte        
OPTION (MAXRECURSION 0)        
--Select * from #TempFinalData3        

Create Table #ALL
(
FieldID BigInt,
FieldName Varchar(200),
FundID Int,
DateValue DateTime,
ActivityBase FLOAT Null
)  
       
Select * into #ALL_1        
from #TempFinalData1        
Cross JOIN        
#TempFinalData2        
Cross JOIN        
#TempFinalData3


Insert InTo #ALL
Select
FieldID ,
FieldName,
FundID,
DateValue,
0.0
From #ALL_1          

-- Alter table #ALL add        
--ActivityBase FLOAT Not Null DEFAULT 0.0    
																																							 
 Create Table #Temp_1
 (
 ActivityBaseTemp Float,
 [Date] DateTime,
 FieldID Bigint,
 FundID Int
 )

Insert Into #Temp_1
 Select Sum(CurrentDr+CurrentCr) as ActivityBaseTemp        
,TransactionDate as Date        
,#ALL.FieldID,        
#TempData.FundID 
from #TempData        
Inner Join #ALL on #TempData.TransactionDate = #ALL.DateValue and #TempData.FundID = #ALL.FundID        
Inner Join T_SubAccountMappingPSR on T_SubAccountMappingPSR.FieldID = #ALL.FieldID and T_SubAccountMappingPSR.SubaccountID = #TempData.SubAccountID        
Group By TransactionDate,#TempData.FundID,#ALL.FieldID   
			 
Update FData 
Set FData.ActivityBase = IsNull(T1.ActivityBaseTemp,0)        
From #ALL FData        
inner join #Temp_1 T1 On T1.FundID = FData.FundID and FData.DateValue = T1.[Date] and FData.FieldID = T1.FieldID             
      
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_FundWiseFieldActivites]')) 
BEGIN      
Drop table T_FundWiseFieldActivites      
END      
        
select * into T_FundWiseFieldActivites from #ALL        
        
Drop table #TempDATA,#TempFinalData1,#TempFinalData2,#TempFinalData3,#ALL,#Temp_1,#ALL_1