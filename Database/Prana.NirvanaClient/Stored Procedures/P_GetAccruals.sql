CREATE Proc P_GetAccruals  
(  
@FromDate datetime,  
@ToDate Datetime  
)  
as  
Select * from T_Accruals where Date>=@FromDate and Date<= @ToDate


