/****    
    
P_MW_NAV '2013-09-20','63,68'    
    
*****/    
    
Create Proc P_MW_GetFundNAV    
(    
 @Date datetime,    
 @FundID varchar(max)    
)    
As    
Begin    
    
select * into #Funds    
from dbo.Split(@FundID, ',')    
    
select NAVValue as NAV from    
PM_NAVValue     
where datediff(d,@Date,Date)=0      
and     
FundID in(select * from #Funds)    
    
Drop table #Funds    
    
End    
--select * from T_CompanyFunds