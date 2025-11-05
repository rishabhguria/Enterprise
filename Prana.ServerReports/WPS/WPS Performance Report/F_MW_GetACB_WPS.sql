-- =============================================                                  
-- Author:  Pankaj Sharma                               
-- Create date: <24/6/2015>                                  
-- Description: <returns the Modified Dietz Return given date range for the entity, note this is for non-linked returns that are less than a month. For linked returns, use F_getLinkedMDReturn>                                  
--    2/3/2011-- expanded to include calculating returns for multi-entities as a table                                 
--    Sample:  select dbo.F_MW_GetMDReturnNOF('7/1/2010','7/2/2010', 'Symbol', 'SPY')                                    
-- exec F_MW_GetACB_WPS        
-- @fromDate = '2015-06-01 00:00:00:000',            
-- @toDate = '2015-06-08 00:00:00:000',            
-- @funds ='ABELS',            
-- @ReportId ='MTM_V0'        
            
-- =============================================                                  
CREATE Procedure [dbo].[F_MW_GetACB_WPS]            
(                                  
 @fromDate datetime,                                    
 @toDate datetime,                                  
 @funds varchar(MAX),                                                            
 @ReportId Varchar(100)            
)                                  
AS                                  
BEGIN                                  
                                  
declare @result float                                  
declare @test float               
                               
create table #T_Funds (entity varchar(max))                                  
insert into #T_Funds select * from dbo.split(@funds, ',')              
                                
--total days in period                                    
declare @D float                                  
set @D = DATEDIFF(d,@fromdate, @toDate) + 1            
print @D                    
                                
--days remaining in period                                    
declare @daysRemain float                                 
set @daysRemain = @D                                  
print @daysRemain             
                                  
--------------------------------------------------------why is to date not adjusted            
declare @LastBusinessDateOfFromdate datetime                                  
set @LastBusinessDateOfFromdate = dbo.AdjustBusinessDays(@fromDate, -1, 1)                                  
print @LastBusinessDateOfFromdate            
                                  
declare @NextBusinessDateIfNotBusinessDay datetime                                  
if dbo.IsBusinessDay(@fromdate,1) = 0                                  
 begin                                   
 select @NextBusinessDateIfNotBusinessDay = dbo.AdjustBusinessDays(@fromDate, 1, 1)                                  
 end                                   
else                                  
 begin                                  
 select @NextBusinessDateIfNotBusinessDay = @fromDate                                  
 end              
print @NextBusinessDateIfNotBusinessDay             
                                  
declare @BaseCurrencyID int                                  
select @BaseCurrencyID = BaseCurrencyID from T_Company              
            
-------------------------------final data            
create table #FinalData            
(            
fundname varchar(max),            
MasterFundName varchar(max),            
ResultantCashEffect float      
)            
insert into #FinalData(fundname) select * from dbo.split(@funds, ',')            
            
Update #FinalData set MasterFundName = NavF.MasterFundName             
From  #FinalData FData                           
inner join                          
(                          
select Funds.FundName, Mfunds.MasterFundName             
from             
T_CompanyFunds AS Funds             
INNER JOIN T_CompanyMasterFundSubAccountAssociation AS MFMappingFunds ON MFMappingFunds.CompanyFundID = Funds.CompanyFundID                  
INNER JOIN T_CompanyMasterFunds AS MFunds ON MFunds.CompanyMasterFundID = MFMappingFunds.CompanyMasterFundID          
) NavF            
on NavF.fundname = FData.fundname            
            
------------------------------------------------               
create table #InvestorCash             
(            
fundname varchar(max),            
ResultantCashEffect float            
)            
            
insert into #InvestorCash                                  
SELECT             
Funds.FundName,            
sum((DateDiff(d,Journal.TransactionDate,@toDate)/@d)            
*            
case when Journal.CurrencyID = @BaseCurrencyID                               
  then             
   Journal.CR- Journal.DR             
  else             
   isnull(CurrencyConversionRate.ConversionRate,1)*(Journal.CR- Journal.DR)             
 end)            
 as ResultantCashEffect            
FROM T_Journal AS Journal                                   
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID                                   
INNER JOIN T_SubCategory SC on SubAccounts.SubCategoryID = SC.SubCategoryID                                  
INNER JOIN T_MasterCategory MC on MC.MasterCategoryID=SC.MasterCategoryID                                
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID                                   
-----for funds and master funds name            
INNER JOIN T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID                                   
INNER JOIN T_CompanyMasterFundSubAccountAssociation AS MFMappingFunds ON MFMappingFunds.CompanyFundID = Funds.CompanyFundID                  
INNER JOIN T_CompanyMasterFunds AS MFunds ON MFunds.CompanyMasterFundID = MFMappingFunds.CompanyMasterFundID                  
-----for currency conversion rate            
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID                                   
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID AND CurrencyConversionRate.Date = Journal.TransactionDate                                   
WHERE              
 Journal.SubAccountId in(47,48)  --for cash deposit,cash withdrawal values are 47 and 48              
and             
 datediff(d,@LastBusinessDateOfFromdate ,Journal.TransactionDate)>=0            
and            
 datediff(d,Journal.TransactionDate,@toDate)>=0            
group by fundname            
            
-----------------------------------------------update resultant cash effect            
Update #FinalData Set ResultantCashEffect  = ISNULL(INVcash.ResultantCashEffect,0)                          
From  #FinalData FData                           
inner join                          
(                          
select fundname,ResultantCashEffect   from  #InvestorCash             
) INVcash            
on INVcash.fundname = FData.fundname            
            
            
--Update #FinalData             
--Set ACB = ISNULL(ResultantCashEffect,0)+ISNULL(NAV,0)            
            
select * from #FinalData            
            
return                                
end 