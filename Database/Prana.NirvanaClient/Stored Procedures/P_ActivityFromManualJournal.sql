
                                             
CREATE procedure P_ActivityFromManualJournal                                             
(      
@StartDate DateTime,                                                                                                                              
@EndDate DateTime         
)                                          
as                                               
     
/*                                                              
Author: SURENDRA BISHT      
                                                            
Usage:      
EXEC [P_ActivityFromManualJournal] '2013-11-14','2014-05-01'         
                                                         
*/      
    
--declare  @StartDate DateTime                                                                                                                              
--declare @EndDate DateTime         
--set  @StartDate='2013-11-14'    
--set  @EndDate='2014-05-01'       
                                                             
---exception transactionids--   
  
SET NOCOUNT ON  
                             
select distinct transactionid into #exceptions from t_journal where ((ActivityId_FK not in (select distinct ActivityID from t_allactivity)  ) or activityid_fk is null)  and datediff(d,transactiondate,@StartDate)<=0 
and   datediff(d,transactiondate,@EndDate)>=0  
and Transactionsource<>9      

-- CHECK IF THERE IS EXCEPTIONS, THEN ONLY EXECUTE FURTHER. 

IF((select count(*) from #exceptions)<>0)

BEGIN
                                                            
-- all exceptions full entry                              
SELECT                                                  
case when                              
accountside=0 then subaccountid                              
end as DRaccount,                                        
case when                               
accountside=1 then subaccountid                              
end as CRaccount,                              
FundID,                              
CurrencyID,                              
Symbol,                              
PBDesc,                              
TransactionDate,                              
TransactionID,                              
DR,                              
cr,                              
TransactionSource,                              
AccountSide,                              
FxRate,                              
FXConversionMethodOperator 
,activitysource  
,taxlotid                                             
into #journal                                                   
from t_journal                                             
where  transactionid in (select transactionid from #exceptions)                              
                                          
                              
                              
--Tempactivity table------------                              
                              
                              
select * into #activity from t_allactivity where activityid is null                        
                        
ALTER TABLE #activity alter column  [ActivityID] [varchar](50)  NULL                        
ALTER TABLE #activity alter column  [ActivityTypeId_FK] [int] NULL                        
ALTER TABLE #activity alter column  [FKID] varchar(50)  NULL                        
ALTER TABLE #activity alter column     [BalanceType] [int] NULL                                          
                              
                                              
                                                       
--activityid for activity to be created                              
declare @activityid varchar(50)                                                        
select @activityid=max(cast(activityid as bigint))+1 from t_allactivity                         
                              
--row                              
declare @row int                              
set @row=1                              
declare @totalrows int                              
select @totalrows=count(transactionid)from #exceptions                              
                              
while ( @row <=@totalrows)                              
begin                              
         
 declare @manualtransactionid varchar(40)                              
                              
 select @manualtransactionid=transactionid  FROM (SELECT  transactionid,ROW_NUMBER()OVER (ORDER BY transactionid)  AS  RowIndex FROM #exceptions)t1 where t1.rowindex=@row                              
                                                         
declare @drcount int                              
declare @crcount int                              
 select @drcount=count(DRaccount) from #journal  where transactionid= @manualtransactionid                                    
 select @crcount=count(CRaccount) from #journal  where transactionid= @manualtransactionid                   
                              
                              
--all mapping having same no of drs and crs as of manual entry will be saved into #tempmapping                                     
select m.* into #tempmapping from (select activitytypeid_fk,count(distinct debitaccount) as d,count(distinct creditaccount) as c from t_activityjournalmapping group by activitytypeid_fk            
having (count(distinct debitaccount)=@drcount and count(distinct creditaccount)=@crcount))as abc inner join  t_activityjournalmapping m                             
on abc.activitytypeid_fk=m.activitytypeid_fk                                            
                                              
                                            
declare @activityTypeid int  -- for found fkid for the journal entry                                              
                 
select @activityTypeid=ActivityTypeId_FK from #tempmapping where ActivityTypeId_FK not in                            
 ((select ActivityTypeId_FK from #tempmapping where debitaccount not in(select DRaccount from #journal where transactionid= @manualtransactionid and DRaccount is not null))                                      
union                             
 (select ActivityTypeId_FK from #tempmapping where creditaccount not in(select CRaccount from #journal where transactionid= @manualtransactionid and CRaccount is not null)))                                 
                              
                              
                            
                              
                                      
--- now either using existing found activityid mapping  or creating new mapping                                              
                                              
-- select @activityTypeid   -- fetched data view                                              
                                              
                                              
if (@activityTypeid is null)                                                
                                              
begin                                               
 -- create new activityid in activitytype                                              
 declare @no varchar(20)                                              
 set @no=1                                              
                                              
    --finding last unknown activity                                              
 declare @activityno int                          
 select @activityno=max(cast(replace(activitytype,'unknown','') as int)) from t_activitytype where activitytype like 'unknown%'                         
 if (@activityno is not null)                                    
 select @no=@activityno+1 
                                              
 insert into t_activitytype(activityType,acronym,BalanceType) values('unknown'+@no,'unknown'+@no,1)                                              
                                          
--Now this is the NEW Activityid for new activity                                              
 select @activityTypeid=ActivityTypeId from  t_activitytype where activityType='unknown'+@no                                              
                            
                                              
 -- now  creatinmg mapping T_activityjournalmapping                                              
 CREATE TABLE #mapping(                                              
 [ActivityTypeId_FK] int ,                                              
  [AmountTypeId_FK] int ,                                              
  [DebitAccount] int ,                                              
  [CreditAccount] int ,                                               
  [CashValueType] bit           --[ActivityDateType]                                              
 )                                      
                                              
  -- inserting drs                                    
insert  into #mapping([DebitAccount])                           
   select DRaccount from #journal where  transactionid= @manualtransactionid and DRaccount is not null                                        
                                                                   
  -- inserting crs                                              
 insert into  #mapping([CreditAccount])                                               
   select CRaccount from  #journal where  transactionid= @manualtransactionid and CRaccount is not null                                 
                                
                                            
  --updating cashvaluectype                                               
   update #mapping set cashvaluetype=0   --later need to change                  
                                              
                         
 -- updating the  ActivityTypeId_FK                                        
 update #mapping set [ActivityTypeId_FK]=@activityTypeid                                              
             
    -- updating the amountypeId_fk accordingly for the default subaccount                                              
  update #mapping set [AmountTypeId_FK]=amt.AmountTypeId                                              
 from #mapping m                                               
 inner join t_subaccounts acc on m.debitaccount=acc.subaccountid                         
 inner join t_activityamounttype amt on amt.defaultSubaccountAcronym=acc.Acronym                                              
                                              
    -- if still null it will be set as amount .                                              
 update #mapping set [AmountTypeId_FK]=(select AmountTypeId from t_activityamounttype where AmountType='Amount') where [AmountTypeId_FK] is null                                              
                                              
                                              
                     
                                        
                                              
 -- inserting new mapping                                              
 insert into T_ActivityJournalMapping(ActivityTypeId_FK,AmountTypeId_FK,DebitAccount,CreditAccount,CashValueType)                                              
 select * from #mapping                      
                  
                
    drop table #mapping                                              
end                                              
                                              
---- Activity journal mapping done.-----------                                              
-----------------------------------------------                                              
                                              
--Now activity 
   
--getting Qty & SettlementDate

--select   @CloseQty=taxlotqty From t_level2allocation where taxlotid=(select taxlotid from  #journal where transactionid=@manualtransactionid )  
declare  @CloseQty int
declare  @settlementDate datetime
declare  @GroupId varchar(50)

select @GroupId=max(substring(taxlotid,1,13)) from  #journal where transactionid=@manualtransactionid group by  transactionid

select @CloseQty=quantity From t_group where groupid=@GroupId  

select @settlementDate=settlementdate From t_group where groupid=@GroupId  


--------------------------------------------------------------------                                                                                        
-- inserting from t_journal to temp activity table      
                  
insert into #activity(fkId,Sidemultiplier,TransactionSource,FundId,Symbol,TradeDate,SettlementDate,CurrencyId,LeadCurrencyID,VsCurrencyID,ClosedQty,Description,Subactivity,Fxrate,FXConversionMethodOperator,activitysource)      
SELECT distinct 
CASE 
WHEN #journal.TransactionSource=2 OR #journal.TransactionSource=11 
THEN transactionid
ELSE
taxlotid END ,
CASE 
WHEN #journal.TransactionSource in(2,11,4)
THEN 1
ELSE  dbo.[GetSideMultiplier] (T_Group.OrderSideTagValue)
 END
,#journal.TransactionSource,FundId,#journal.Symbol,TransactionDate,@settlementDate,#journal.CurrencyId,0,0,@CloseQty,pbdesc,'',#journal.Fxrate,#journal.FXConversionMethodOperator,activitysource                                              
from #journal INNER JOIN T_Group ON T_Group.groupid=substring(#journal.taxlotid,1,13)
where transactionid=@manualtransactionid   

--------------------------------------------------------------

update #activity set sidemultiplier=dbo.[GetSideMultiplier] (T_Group.OrderSideTagValue) 
from #activity
INNER JOIN pm_taxlots ON #activity.taxlotid=pm_taxlots.taxlotid
INNER JOIN 	T_Group ON T_Group.groupid=pm_taxlots.groupid
WHERE T_Group.TransactionSource not in (2,11)



------------------------------------------------
                              
                              
                              
                               
--this table used for updating defaultsubaccountacronym which needed to be joined with subaccounts                              
                                                       
                            
                          
select dr,defaultsubaccountacronym into #temp2 from #journal 
inner join t_subaccounts sub on #journal.DRaccount=sub.subaccountid                          
inner join t_activityamounttype amt on sub.acronym=amt.defaultsubaccountacronym where #journal.transactionid=@manualtransactionid                          
                          
                                              
             
                                              
                                              
--- updating amount and fees------------------------------------                                              
update #activity                              
set stampduty=isnull((select dr from #temp2  where defaultsubaccountacronym='Stamp_Duty' ),0)                            
,Commission=isnull((select dr from #temp2  where defaultsubaccountacronym='Commission'),0)                         
,OtherBrokerFees=isnull((select dr from #temp2  where defaultsubaccountacronym='Other_Broker_Fees'),0)                             
,TransactionLevy=isnull((select dr from #temp2  where defaultsubaccountacronym='Transaction_Levy'),0)                             
,ClearingFee=isnull((select dr from #temp2  where defaultsubaccountacronym='Clearing_Fee'),0)                            
,TaxOnCommissions=isnull((select dr from #temp2  where defaultsubaccountacronym='Tax_On_Commissions'),0)
,MiscFees=isnull((select dr from #temp2  where defaultsubaccountacronym='Misc_Fees'),0)   
,SecFee=isnull((select dr from #temp2  where defaultsubaccountacronym='Sec_Fee'),0) 
,OccFee=isnull((select dr from #temp2  where defaultsubaccountacronym='Occ_Fee'),0) 
,OrfFee=isnull((select dr from #temp2  where defaultsubaccountacronym='Orf_Fee'),0) 
,ClearingBrokerFee=isnull((select dr from #temp2  where defaultsubaccountacronym='Clearing_Broker_Fee'),0) 
,SoftCommission=isnull((select dr from #temp2  where defaultsubaccountacronym='Soft_Commission'),0)                              
where fkid=@manualtransactionid                                                   
--------------------------------------------------------------------                                              
                                              
-- updating all other details needed-------------                                              
                                              
declare @total float    --total dr                                              
declare @commission float  --total commission                                              
select @total=sum(dr) from #journal where transactionid=@manualtransactionid                                                        
select @commission=(select sum(dr)from #temp2)                                              
                              
                                   
set   @activityid=cast(@activityid as bigint)+1                                                
                                              
                                              
update #activity set                                              
amount=@total-isnull(@commission,0)                                             
,balancetype=1                                              
,activitytypeid_fk=@activityTypeid                                              
,activityid=isnull(@activityid,@row)                                              
,activitynumber=1                                              
 from #activity  where fkid=@manualtransactionid                                   
    
--unique key                                   
update #activity set UniqueKey=FKID+cast (DATEPART(m,tradedate)as varchar(5) )+'/'+                                              
cast (DATEPART(d,tradedate)as varchar(5) ) +'/'+                                              
cast (DATEPART(yyyy,tradedate)as varchar(5) )+TransactionSource+cast(ActivityNumber as varchar(5)) from #activity   where fkid=@manualtransactionid                            
                                              
                                             
-------------------------------------------------------------                                                        
                                                                             
                                                  
drop table #tempmapping                                                                                                             
drop table #temp2          
                                
set @activityTypeid=null                              
set @row =@row+1                              
                              
END         
                        
SET NOCOUNT OFF     
                                                   
 insert into t_allactivity                                           
select * from #activity  

-------------------------------------------------------------                                           
 SET NOCOUNT ON                                                                                                                     
-- updating journal ActivityId_FK column                                             
update j  set ActivityId_FK=activityid 
from #activity activity inner join t_journal as j on activity.fkid=j.taxlotid
where j.transactionsource not in(2,11)

update j set ActivityId_FK=activityid 
from #activity activity inner join t_journal as j on activity.fkid=j.transactionid
where j.transactionsource in(2,11)
                                          
                                             
-------------------------------------------------------------                           
                      
                                                  
drop table #journal                               
drop table #activity                               

END


drop table #exceptions 


