
/*       
EXEC [P_GetDataForAutomailer_Newtyn_PFPI]  '03-16-2023'
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-5900 
DESC: Data is required only for selected accounts and different accounts have to merged, so here we have hardcoded the accounts ID.
Initially we used account name but let's say account name is renamed, in this case the functionality will break. so we have used accountId here.
*/
              
CREATE PROCEDURE [dbo].[P_GetDataForAutomailer_Newtyn_PFPI]                                                                                       
(                                                                                                      
 @inputDate datetime                                                                                                  
)                                                                                                         
AS          
     
 --Declare @inputDate datetime 
 --Set @inputDate = '03-16-2023'

--1199	BNP NP 491-00040
--1211	BNP NTE 491-00041
--1214	Citi NP 522-91K57
--1215	Citi NTE 522-91K58

--1274	Fidelity NP 015479
--1275	Fidelity NTE 015468
--1212	GS NP 002486561
--1213	GS NTE 002486553

 Declare @FundId Varchar(500)
 Set @FundId = ('1199,1211,1214,1215,1274,1275,1212,1213')

 Declare @FundIdTable Table
 (
 FundId Int
 )

 Insert InTo @FundIdTable 
 Select Cast(LTRIM(RTRIM(Items)) As Int) As FundId
 From DBO.Split(@FundId,',')

 --Select *
 --From @FundIdTable

 Select CF.CompanyFundID, CF.FundName
 InTo #TempAccountDetails
 From T_CompanyFunds CF With(NoLock)
 Inner Join @FundIdTable F On F.FundId = CF.CompanyFundID


Select                 

	Case 
	When CF.CompanyFundID In (1199,1211)
	Then 'NEWTYN BNP'
	When CF.CompanyFundID In (1214,1215)
	Then 'NEWTYN CITI'
	When CF.CompanyFundID In (1274, 1275)
	Then 'NEWTYN FEDO'
	When CF.CompanyFundID In (1212, 1213)
	Then 'NEWTYN GSCO'
	Else ''
	End As AccountName,
	S.Side As Side,                                                                                                 
	VT.Symbol,                                                                                                       
	IsNull(CP.ShortName,'Undefined') As CounterParty ,
	VT.AvgPrice As AveragePrice,
	VT.TaxLotQty As AllocatedQty,     
	Cast(VT.AUECLocalDate As Date) As TradeDate,
	(VT.AvgPrice * VT.TaxLotQty) As WTDPrice
      
InTo #TempGroupedData               
From V_TaxLots VT With(NoLock)               
Inner Join T_Side S With(NoLock) On S.SideTagValue = VT.OrderSideTagValue
Inner Join #TempAccountDetails CF On CF.CompanyFundID=VT.FundID 
Left Outer Join T_CounterParty CP With(NoLock) On CP.CounterPartyID=VT.CounterPartyID 
Where DateDiff(DAY,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 
 

Select 
AccountName, 
Side,
Symbol,
CounterParty,
CONVERT(Decimal(18,8),Sum(AllocatedQty)) as AllocatedQty,    
Round(CONVERT(Decimal(18,8),
	Case 
		When Sum(AllocatedQty) > 0
		Then Sum(WTDPrice)/ Sum(AllocatedQty)
		Else 0
	End 
),4) 
As AveragePrice,
TradeDate

From #TempGroupedData
Group by AccountName, Side,Symbol,CounterParty,TradeDate 

FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')                                                             

Drop Table #TempGroupedData, #TempAccountDetails      
