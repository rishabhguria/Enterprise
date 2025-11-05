
/*       
EXEC [P_GetData_Newtyn_PFPI_EOD]  0,'','03-16-2023',0,0,0,0,0
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-6328
DESC: Data is required only for selected accounts and different accounts have to merged, so here we have hardcoded the accounts ID.
Initially we used account name but let's say account name is renamed, in this case the functionality will break. so we have used accountId here.
*/
              
CREATE PROCEDURE [dbo].[P_GetData_Newtyn_JPMS_EOD]                                                                                       
(                                                                                                      
@ThirdPartyID int,                                                                                                                                                                      
 @CompanyFundIDs varchar(max),                                                                                                                                                                      
 @InputDate datetime,                                                                                                                                                                  
 @CompanyID int,                                                                                                                                  
 @AuecIDs varchar(max),                                                                        
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                       
 @DateType int ,          
 @fileFormatID int                                                                                                
)                                                                                                         
AS   

--Declare @ThirdPartyID int,                                                                                                                                                                      
-- @CompanyFundIDs varchar(max),                                                                                                                                                                      
-- @InputDate datetime,                                                                                                                                                                  
-- @CompanyID int,                                                                                                                                  
-- @AuecIDs varchar(max),                                                                        
-- @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                       
-- @DateType int ,          
-- @fileFormatID int    
 
-- Set @ThirdPartyID   = 0                                                                                                                                                                   
-- Set @CompanyFundIDs =''                                                                                                                                                                      
-- Set @InputDate  = '03-16-2023'                                                                                                                                                              
-- Set @CompanyID  = 0                                                                                                                               
-- Set @AuecIDs   = 0                                                                      
-- Set @TypeID   = 0                                                           
-- Set @DateType  = 0      
-- Set @fileFormatID   = 0
     


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
	Then 'BNP'
	When CF.CompanyFundID In (1214,1215)
	Then 'CITI'
	When CF.CompanyFundID In (1274, 1275)
	Then 'FIDO'
	When CF.CompanyFundID In (1212, 1213)
	Then 'GSCO'
	Else ''
	End As AccountName,
	S.Side As Side,                                                                                                 
	VT.Symbol,                                                                                                       
	IsNull(CP.ShortName,'Undefined') As CounterParty ,
	VT.AvgPrice As AveragePrice,
	VT.TaxLotQty As AllocatedQty,     
	CONVERT(VARCHAR(10),VT.AUECLocalDate,101) As TradeDate,
	CONVERT(VARCHAR(10),VT.SettlementDate,101) As SettlmentDate,
	(VT.AvgPrice * VT.TaxLotQty) As WTDPrice,
	ISNULL((VT.SoftCommission + VT.Commission), 0) As TotalCommission
	
	
      
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
TradeDate,
SettlmentDate,
   Case 
		When Sum(AllocatedQty) > 0
		Then Sum(TotalCommission)/ Sum(AllocatedQty)
		Else 0
	End 
	As CommissionPerShare

From #TempGroupedData
Group by AccountName, Side,Symbol,CounterParty,TradeDate,SettlmentDate                                                   

Drop Table #TempGroupedData, #TempAccountDetails      

