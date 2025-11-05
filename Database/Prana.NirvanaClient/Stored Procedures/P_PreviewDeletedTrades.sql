/*
P_PreviewDeletedTrades '08-17-2010'
drop proc P_PreviewDeletedTrades
*/
CREATE Proc P_PreviewDeletedTrades
(
	@DeclaredDate DateTime=null
)
As

set nocount on

-- Check Day whether Sunday or Not
-- If Sunday then below query returns 7

--Declare @DeclaredDate datetime 
if(@DeclaredDate is null)
Set @DeclaredDate=GetDate()    
  
Declare @weekDay tinyInt
SET @weekDay =((@@dateFirst+datePart(dw,@DeclaredDate)-2) % 7) + 1    

--Select @weekDay as WeekDay

Declare @CutOffHour int
Declare @CutOffMin int

Declare @SubTable Table
(
	Symbol Varchar(100),
	ClOrderID varchar(100),
	ParentClOrderID varchar(100),
	AUECLocalDate DateTime,
--	TradeDate DateTime,
	ProcessDate DateTime
)

Declare @GroupTable Table
(
	Symbol Varchar(100),
	Side varchar(20),
	AvgPrice float,
	AllocatedQty float,
	GroupID varchar(100),
	AUECLocalDate DateTime,
	ProcessDate DateTime,
	InfoTag Varchar(20)
)

Declare  
@Symbol varchar(100),     
@SundayCutOffHour int,      
@SundayCutOffMIn int,      
@WeekCutOffHour int,
@WeekCutOffMin int
      
DECLARE FutureSymbol_Cursor CURSOR FAST_FORWARD FOR                                  
Select Distinct       
  G.Symbol                    
 ,SundayCutOffHour                      
 ,SundayCutOffMIn                 
 ,WeekCutOffHour                 
 ,WeekCutOffMin                   
From  T_FutureSymbolwiseCutoffTime FM
Inner Join T_Group G ON SUBSTRING(G.Symbol, 0, CHARINDEX(' ', G.Symbol)) = FM.Symbol    
      
Open FutureSymbol_Cursor;                                
                                
FETCH NEXT FROM FutureSymbol_Cursor INTO         
@Symbol,     
@SundayCutOffHour,      
@SundayCutOffMIn,      
@WeekCutOffHour,
@WeekCutOffMin  

WHILE @@fetch_status = 0                                  
BEGIN 
 

--Select @Symbol

if @weekDay = 7
	Begin
		Set @CutOffHour =@SundayCutOffHour
		Set @CutOffMin = @SundayCutOffMIn
	End
Else
	Begin
		Set @CutOffHour =@WeekCutOffHour
		Set @CutOffMin = @WeekCutOffMin
	End

If(@CutOffHour>0)
Begin

	Insert into @SubTable
	SELECT 
	T_Order.Symbol,
	ClOrderID, 
	T_Sub.ParentClOrderID,
	AUECLocalDate,
	Case
		When [dbo].[IsGreaterThan](AUECLocalDate,@CutOffHour,@CutOffMin)=1
		Then DateAdd(d,1,AUECLocalDate)
	Else AUECLocalDate
	End as ProcessDate
	FROM T_Sub
	Inner Join T_Order On T_Order.ParentClOrderID=T_Sub.ParentClOrderID
	Where T_Order.Symbol=@Symbol 
	order by T_Order.Symbol,AUECLocalDate

	Insert into @GroupTable
	SELECT 
	Symbol,
	T_Side.Side ,
	AvgPrice,
	AllocatedQty,
	GroupID, 
	AUECLocalDate,
	Case
		When [dbo].[IsGreaterThan](AUECLocalDate,@CutOffHour,@CutOffMin)=1
		Then DateAdd(d,1,AUECLocalDate)
		Else AUECLocalDate
	End as ProcessDate,
    ''
	FROM T_Group
    Inner Join T_Side On T_Side.SideTagValue=T_Group.OrderSideTagValue
	Where Symbol=@Symbol 
	order by Symbol,AUECLocalDate

End


FETCH NEXT FROM FutureSymbol_Cursor INTO                                   
      
@Symbol,     
@SundayCutOffHour,      
@SundayCutOffMIn,      
@WeekCutOffHour,
@WeekCutOffMin   ;      
      
END                                  
CLOSE FutureSymbol_Cursor;                                  
DEALLOCATE FutureSymbol_Cursor; 


--Select * from @SubTable Where DateDiff(day,ProcessDate,@DeclaredDate)>0
--order by Symbol, AUECLocalDate

update @GroupTable
Set InfoTag=
Case
When DateDiff(day,ProcessDate,@DeclaredDate)>0
Then 'To Be Deleted'
Else 'Not To Be Deleted'
End
from @GroupTable

Select 
* from @GroupTable 
Order By Symbol, AUECLocalDate,InfoTag

