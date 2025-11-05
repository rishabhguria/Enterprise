    
    
/***********************************************************************************      
Modified : Praveen    
    
P_BalanceSheetReport_WithMergeFunds      
'2014/08/01','1213,1214,1233,1234,1235,1236,1237',1,0      
      
********************************************************************/    
          
CREATE PROCEDURE [dbo].[P_BalanceSheetReport_WithMergeFunds]            
(            
@date datetime,              
@fund varchar(max),          
@InvestmentsAccountMerge bit,          
@HideTransfersAccount bit        
)            
As          
           
declare @MinDate datetime              
Select @MinDate= CashMgmtStartDate From T_CashPreferences              
                
declare @BaseCurrencyID int              
Select @BaseCurrencyID= BaseCurrencyID From T_Company              
             
Select * Into #Funds                                              
from dbo.Split(@fund, ',')                   
                                                                                                                       
                
            
SELECT            
SubBal.SubAccountID,            
            
CASE            
WHEN (MastCat.MasterCategoryName IN ('Income','Expenses'))            
THEN 'Retained Earnings-Current Period'            
-----------------Transfers Account---------------------            
WHEN (SubAcc.Acronym = 'CASHTRA' AND @HideTransfersAccount=1)            
THEN 'Retained Earnings-Current Period'           
-----------------Investments Account---------------------            
WHEN (SubAcc.Acronym = 'EquityLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Long Equity'          
WHEN (SubAcc.Acronym = 'FixedIncomeLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Long Warrants'          
WHEN (SubAcc.Acronym = 'EquityOptionLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Long Equity Option'          
WHEN (SubAcc.Acronym = 'BondLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Long Bond'          
WHEN (SubAcc.Acronym = 'EquityShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Short Equity'      
      
WHEN (SubAcc.Acronym = 'Invt_CDS_Short' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Short Equity'       
WHEN (SubAcc.Acronym = 'CDSShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Short Equity'       
        
WHEN (SubAcc.Acronym = 'EquityOptionShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Short Equity Option'          
WHEN (SubAcc.Acronym = 'BondShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Short Bond'          
WHEN (SubAcc.Acronym = 'PrivateEquityLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Long Private Equity'          
WHEN (SubAcc.Acronym = 'PrivateEquityShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'Investment - Private Short Equity'       
         
--------------------------------------              
          
ELSE SubAcc.Name             
End as SubAccName,            
            
SubCat.SubCategoryID,            
            
CASE            
WHEN (MastCat.MasterCategoryName IN ('Income','Expenses'))            
THEN 'Retained Earnings-Current Period'            
-----------------Transfers Account---------------------            
WHEN (SubAcc.Acronym = 'CASHTRA' AND @HideTransfersAccount=1)           
THEN  'Retained Earnings-Current Period'--Cash Transfers          
-----------------Investments Account---------------------            
WHEN (SubAcc.Acronym = 'EquityLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermAsset'          
WHEN (SubAcc.Acronym = 'FixedIncomeLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermAsset'          
WHEN (SubAcc.Acronym = 'EquityOptionLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermAsset'          
WHEN (SubAcc.Acronym = 'BondLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'LongTermAsset'          
WHEN (SubAcc.Acronym = 'EquityShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermLiability'       
      
      
WHEN (SubAcc.Acronym = 'Invt_CDS_Short' AND @InvestmentsAccountMerge=1)       
THEN  'ShortTermLiability'       
WHEN (SubAcc.Acronym = 'CDSShortRevaluation' AND @InvestmentsAccountMerge=1)       
THEN  'ShortTermLiability'      
      
      
WHEN (SubAcc.Acronym = 'EquityOptionShortRevaluation'  AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermLiability'          
WHEN (SubAcc.Acronym = 'BondShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'LongTermLiability'          
WHEN (SubAcc.Acronym = 'PrivateEquityLongRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermAsset'          
WHEN (SubAcc.Acronym = 'PrivateEquityShortRevaluation' AND @InvestmentsAccountMerge=1)           
THEN  'ShortTermLiability'          
--------------------------------------            
ELSE SubCat.SubCategoryName             
End as SubCategoryName,            
            
MastCat.MasterCategoryID,            
            
CASE             
WHEN (MastCat.MasterCategoryName IN ('Cash'))            
THEN 'Asset'           
WHEN (MastCat.MasterCategoryName IN ('Equity','Income','Expenses'))            
THEN 'Owner''s Equity'          
WHEN (SubBal.SubAccountID = 67)            
THEN 'Owner''s Equity'          
ELSE MastCat.MasterCategoryName             
End as MasterCategoryName,            
            
CASE             
WHEN (MastCat.MasterCategoryName IN ('Cash'))            
THEN 1           
ELSE 2             
End as SortOrder,              
              
SubBal.FundId,            
Funds.FundName as FundName,            
SubBal.CurrencyId,            
Curr.CurrencySymbol,            
SubBal.OpenBalDate,            
SubBal.TransactionDate,                  
            
(SubBal.CloseDrBal - SubBal.CloseCrBal) as BalanceLocal,              
(SubBal.CloseDrBalBase - SubBal.CloseCrBalBase) as BalanceBase          
            
            
from T_SubAccountBalances SubBal            
INNER JOIN T_SubAccounts SubAcc on SubBal.SubAccountID = SubAcc.SubAccountID            
INNER JOIN T_CompanyFunds Funds on SubBal.FundID = Funds.CompanyFundID            
INNER JOIN T_Currency Curr on SubBal.CurrencyID = Curr.CurrencyID            
INNER JOIN T_SubCategory SubCat on SubAcc.SubCategoryID = SubCat.SubCategoryID            
INNER JOIN T_MasterCategory MastCat on SubCat.MasterCategoryID = MastCat.MasterCategoryID            
            
WHERE             
DateDiff(d,TransactionDate,@date) = 0            
and            
SubBal.FundId in (Select * from #Funds)         
Order by SortOrder,MasterCategoryName,SubCategoryName,SubAccName     