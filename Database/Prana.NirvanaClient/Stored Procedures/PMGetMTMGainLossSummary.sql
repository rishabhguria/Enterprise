
-- Stored Procedure
/*                            
[PMGetMTMGainLossSummary_PRoc] '07-01-2012','12-15-2012','1182,1183,1184','1,2,3,4,5,8,11',0      
*/                            
CREATE Procedure [dbo].[PMGetMTMGainLossSummary]                                                                 
(                                                                                                                                                                                                                               
 @StartDate datetime,                                                                  
 @EndDate datetime,                            
 @Fund Varchar(Max),                            
 @Asset Varchar(Max),                      
 @IncludeCash bit--,                  
-- @UdaAsset varchar(max),                  
-- @UdaCountry varchar(max),                  
-- @UdaSector varchar(max),                  
-- @UdaSecurityType varchar(max),                  
-- @UdaSubSector varchar(max)                  

)                                                                  
AS                                                                                                                                                                                                                                
BEGIN                                                                                                                                                                                                                                
 -- SET NOCOUNT ON added to prevent extra result sets from                                                                                                                                                                                                     

 -- interfering with SELECT statements.                                                                                                                                                                                                                        

 SET NOCOUNT ON;                               

--Declare @StartDate DateTime                            
--Declare @EndDate DateTime                                                               
--                                                                  
--Set @StartDate='07-01-2012'                                                                                                                                                                                                                               
--Set @EndDate='08-06-2012'                            

-- get Security Master Data in a Temp Table                    

/******************************                  
Tables for filtering Uda Data                  
*******************************/                   
--Declare @T_UdaAssetIDs Table                                        
--(                                        
-- UdaAssetID int                                        
--)                                        
--Insert Into @T_UdaAssetIDs                   
--Select * From dbo.Split(@UdaAsset, ',')                  
--                  
--Declare @T_UdaCountryIDs Table                                        
--(                                        
-- UdaCountryID int                                        
--)                                        
--Insert Into @T_UdaCountryIDs                   
--Select * From dbo.Split(@UdaCountry, ',')                  
--                  
--Declare @T_UdaSecurityTypeIDs Table                       
--(                                        
-- UdaSecurityTypeID int                                      --)                                        
--Insert Into @T_UdaSecurityTypeIDs                   
--Select * From dbo.Split(@UdaSecurityType, ',')                  
--              
--Declare @T_UdaSectorIDs Table                                        
--(                                        
-- UdaSectorID int                                        
--)                                        
--Insert Into @T_UdaSectorIDs                   
--Select * From dbo.Split(@UdaSector, ',')                  
--                  
--Declare @T_UdaSubSectorIDs Table                                        
--(                
-- UdaSubSectorID int                                        
--)                                        
--Insert Into @T_UdaSubSectorIDs                   
--Select * From dbo.Split(@UdaSubSector, ',')                  
--                  
--Create Table #T_UDAAsset                  
--(                  
--AssetID int,                  
--AssetName varchar(100)                  
--)                  
--                  
--Insert InTo #T_UDAAsset                                      
-- Select                                       
-- Asset.AssetID,                            
-- Asset.AssetName                                           
-- From T_UDAAssetClass Asset INNER JOIN @T_UdaAssetIDs AssetIDs                   
--ON Asset.AssetID = AssetIDs.UdaAssetId                     
--                  
--Create Table #T_UDACountry                  
--(                  
--CountryID int,                  
--CountryName varchar(100)                  
--)                  
--                  
--Insert InTo #T_UDACountry                                      
-- Select                                       
-- Country.CountryID,                            
-- Country.CountryName                                           
-- From T_UDACountry Country INNER JOIN @T_UdaCountryIDs CountryIDs                   
--ON Country.CountryID = CountryIDs.UdaCountryId                    
--                  
--Create Table #T_UDASecurityType                  
--(                  
--SecurityTypeID int,                  
--SecurityTypeName varchar(100)                  
--)                  
--                  
--Insert InTo #T_UDAsecurityType                                      
-- Select                                       
-- SecurityType.SecurityTypeID,                            
-- SecurityType.SecurityTypeName                                           
-- From T_UDASecurityType SecurityType INNER JOIN @T_UdaSecurityTypeIDs SecurityTypeIDs                   
--ON SecurityType.SecurityTypeID = SecurityTypeIDs.UdaSecurityTypeId                   
--                  
--Create Table #T_UDASector                  
--(                  
--SectorID int,                  
--SectorName varchar(100)                  
--)                  
--                  
--Insert InTo #T_UDASector                                      
-- Select                                       
-- Sector.SectorID,                            
-- Sector.SectorName                                           
-- From T_UDASector Sector INNER JOIN @T_UdaSectorIDs SectorIDs                   
--ON Sector.SectorID = SectorIDs.UdaSectorId                    
--                  
--Create Table #T_UDASubSector                  
--(                  
--SubSectorID int,                  
--SubSectorName varchar(100)                  
--)                  
--                  
--Insert InTo #T_UDASubSector                                      
-- Select                                       
-- SubSector.SubSectorID,                            
-- SubSector.SubSectorName                                           
-- From T_UDASubSector SubSector INNER JOIN @T_UdaSubSectorIDs SubSectorIDs                   
--ON SubSector.SubSectorID = SubSectorIDs.UdaSubSectorId                    

Create Table #SecMasterDataTempTable                                                                                                                                                                             
(                                                                                                                                                            
  AUECID int,                                   
  TickerSymbol Varchar(100),                                                                                                                                       
  CompanyName  VarChar(500),                                                                                              
  AssetName Varchar(100),                                                                                                                                
  SecurityTypeName Varchar(200),                                       
  SectorName Varchar(100),                                       
  SubSectorName Varchar(100),                                               
  CountryName  Varchar(100),                                                                   
  PutOrCall Varchar(5),                                  
  Multiplier Float,                                                           
  LeadCurrencyID int,                                                                                                                                      
  VsCurrencyID int,                                            
  CurrencyID int,                                                          
  UnderlyingSymbol Varchar(100),                            
  AssetID int                                                                   
)                                                               

Insert Into #SecMasterDataTempTable                                            
Select                                
 AUECID ,                                                                                                                                                                                        
 TickerSymbol ,                                                                                                                      
 CompanyName  ,                                                                                                                                        
 AssetName,                                                                                                                                      
 SecurityTypeName,                                                                                                                                                                                                                                       
 SectorName ,                                                                                                                                                                                                                                                 
 SubSectorName ,                                                                                                                                                               
 CountryName ,                                                                                                                                      
 PutOrCall ,                                                                                                                                      
 Multiplier ,                                                                                                      
 LeadCurrencyID ,                                                                                            
 VsCurrencyID,                                                                    
 CurrencyID,                                                          
UnderlyingSymbol,                            
AssetID                                                                                                                                       
  From V_SecMasterData                             

Declare @T_FundIDs Table                                        
(     
 FundId int                                        
)                                        
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                               

Declare @T_AssetIDs Table                                            
(                                            
 AssetId int                                            
)                                            
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')                                  

CREATE TABLE #T_Asset                      
(                                        
 AssetID int,                                        
 AssetName varchar(50)                                 
)                              

Insert InTo #T_Asset                                      
 Select                                       
 T_Asset.AssetID,                            
 T_Asset.AssetName               
  From T_Asset INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetId                                

CREATE TABLE #T_CompanyFunds                                        
(                                        
 CompanyFundID int,                                        
 FundName varchar(50),                                        
 FundShortName varchar(50),                        
 CompanyID int,                                        
 FundTypeID int,                                
 UIOrder int NULL                                       
)                                        
Insert Into #T_CompanyFunds                                        
Select T_CompanyFunds.*                                         
 From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                                       
 Where T_CompanyFunds.IsActive=1

CREATE TABLE #PM_Taxlots                                        
(                                        
 [TaxLot_PK] [bigint] NOT NULL,                                        
 [TaxLotID] [varchar](50) NOT NULL,                                        
 [Symbol] [varchar](100) NOT NULL,                                        
 [TaxLotOpenQty] [float] NOT NULL,                                        
 [AvgPrice] [float] NOT NULL,                        
 [TimeOfSaveUTC] [datetime] NULL,                                  
 [GroupID] [nvarchar](50) NULL,                                        
 [AUECModifiedDate] [datetime] NULL,                                        
 [FundID] [int] NULL,                                        
 [Level2ID] [int] NULL,                                        
 [OpenTotalCommissionandFees] [float] NULL,                                        
 [ClosedTotalCommissionandFees] [float] NULL,                                        
 [PositionTag] [int] NULL,                                   
 [OrderSideTagValue] [nchar](10) NULL,                                        
 [TaxLotClosingId_Fk] [uniqueidentifier] NULL,                                        
 [ParentRow_Pk] [bigint] NULL,                                  
 [AccruedInterest] Float Null                                       
)                                        

Insert Into #PM_Taxlots                                        
 Select     
 TaxLot_PK,                                        
 TaxLotID,                                        
 Symbol,                                        
 TaxLotOpenQty,                                        
 AvgPrice,                        
 TimeOfSaveUTC,          
 GroupID,                                        
 AUECModifiedDate,                                        
 PM_Taxlots.FundID,                                        
 Level2ID,                                        
 OpenTotalCommissionandFees,                                        
 ClosedTotalCommissionandFees,                                        
 PositionTag,                                   
 OrderSideTagValue,     
 TaxLotClosingId_Fk,                                        
 ParentRow_Pk,                                  
 AccruedInterest                                            
 From PM_Taxlots                               
 INNER JOIN @T_FundIDs FundIDs ON PM_Taxlots.FundID = FundIDs.FundID                               
 Inner Join #SecMasterDataTempTable SM On SM.TickerSymbol =  PM_Taxlots.Symbol                              
 INNER JOIN @T_AssetIDs AssetIDs ON SM.AssetID = AssetIDs.AssetID                                      
 Where DateDiff(Day,AUECModifiedDate,@EndDate) >= 0                             


Declare @MinTradeDate DateTime                                                                                                                
Declare @BaseCurrencyID int                                                                                     
Set @BaseCurrencyID=(select top 1 BaseCurrencyID from T_Company where companyId <> -1)  
                                                                                                         
-- get company default AUECID                                          
Declare @DefaultAUECID int                                                                                         
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                                                                                                                              
-- get Mark Price for Start Date                                                                                                            
Create Table #MarkPriceForStartDate                                                          
(                        
 Finalmarkprice float ,                                                                                                                                                             
 Symbol varchar(50)                                                                                                                          
)                                                              

-- get Mark Price for End Date                                                                                                                                   
Create Table #MarkPriceForEndDate                                                          
(                                                                                                                                                        
Finalmarkprice float ,                                
Symbol varchar(50)                                                                            
)                                                                              
-- get forex rates for 2 date ranges                                                                                                                                                                                        
Create Table #FXConversionRates                                                                        
(                               
 FromCurrencyID int,             
ToCurrencyID int,                                                                                               
 RateValue float,                                                                                            
 ConversionMethod int,                                                                                                                                                      
 Date DateTime,                                                                                                 
 eSignalSymbol varchar(max)                                                                                                                               
)                                                                                                                                                                      

-- get yesterday business day AUEC wise                                                                                                                                      
Create Table #AUECYesterDates                                                                                                                                                                                     
(                                                                            
   AUECID INT,                                                                                                                  
   YESTERDAYBIZDATE DATETIME                                                                                                                                                
)                                                                               
-- get business day AUEC wise for End Date                                                                                                    
Create Table #AUECBusinessDatesForEndDate                                                                                                                    
(                                                                               
   AUECID INT,                                                                                                                   
   YESTERDAYBIZDATE DATETIME                                                                                                                                                                              
)                                                                                                                               


Set @MinTradeDate =(Select MIN(PT.AUECModifiedDate) as TradeDate                                                                                                     
 from #PM_Taxlots PT  Where Datediff(d,PT.AUECModifiedDate,@EndDate) >=0                                                                                                      
    And TaxlotClosingID_FK is null)                                                                                                                                                                       

If ( @MinTradeDate is not null And (DateDiff(d,@StartDate,@MinTradeDate)) > 0)                                                                                                  
  Begin                                                                                                                
   Set @MinTradeDate = @StartDate                                                                                        
  End                                        
Else If ( @MinTradeDate is null)       
 Begin                                      
  Set @MinTradeDate = @StartDate                                       
 End                                                                                          

Set @MinTradeDate =  dbo.AdjustBusinessDays(@MinTradeDate,-1, @DefaultAUECID)                             

Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @MinTradeDate,@EndDate                              
--  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@MinTradeDate,@EndDate) as A                                                               

 Update #FXConversionRates                                                               
  Set RateValue = 1.0/RateValue                                                                                                                                                                        
  Where RateValue <> 0 and ConversionMethod = 1                                                                                         

Update #FXConversionRates                                                                                            
  Set RateValue = 0                                                         
  Where RateValue is Null                                                                   

--code for international future                                                                                                 
Declare @SelectedSymbol table                                                                      
(                                                                    
 FutSymbol varchar(100)                                                            
)                                                                             
Insert InTo @SelectedSymbol                                                                                                
Select                                   
TickerSymbol                                                                    
From #SecMasterDataTempTable SM                                                                                                                
Inner Join T_AUEC on SM.AUECID=T_AUEC.AUECID                                                                                                                
Where T_AUEC.AssetID=3 and SM.CurrencyID<>@BaseCurrencyID                                                                         

CREATE TABLE [dbo].#TempMP                                                                                                                           
(                                                                               
 Symbol varchar(200) ,                                                                                                
 Date datetime,                                                                                                
 Markprice float,                                                                                                
 CF float                                                                                                   
)                                                                                                                                                         

INSERT INTO #TempMP                                                                                                                                   
(                  
Symbol,                                                                                                          
Date,                                                                                               
MarkPrice ,                                                                                                          
CF                                          
)                                                                                                          
Select                         
P.Symbol,                                                                                   
P.Date,                                                                                  
Isnull(PMDMP.FinalMarkPrice,0),                                                                                  
Isnull(FXDayRatesForTradeDate.RateValue,0)                                                                                  
from                                     
(Select FutTab.FutSymbol as Symbol,Items as Date,T_Group.CurrencyID                                                 
 from @SelectedSymbol FutTab                                                                                    
 Cross Join dbo.GetDateRange(DATEADD(day,-3,@StartDate), @EndDate)                                                                                                           
 Inner Join T_Group on FutTab.FutSymbol=T_Group.Symbol                                                                              
 Inner Join #PM_Taxlots on #PM_Taxlots.GroupID = T_Group.GroupID                                                                                                          
 Left Outer Join PM_TaxlotClosing on PM_TaxlotClosing.TaxlotClosingID = #PM_Taxlots.TaxlotClosingID_Fk                                                           
 Where DateDiff(day,T_Group.ProcessDate,@EndDate) >= 0                                      
  And (DateDiff(day,@StartDate,PM_TaxlotClosing.AuecLocalDate) >= 0 OR PM_TaxlotClosing.TaxlotClosingID is null)) As p                                                                              
  left outer join PM_DayMarkPRice PMDMP                                                                                   
    On (PMDMP.Symbol=P.Symbol and Datediff(d,PMDMP.Date,P.Date)=0)                                   
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                            
    On (FXDayRatesForTradeDate.FromCurrencyID = P.CurrencyID                                                                                                                                     
  And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID            
  And DateDiff(d,P.Date,FXDayRatesForTradeDate.Date)=0)                                                                      

--to adjust holidays mark price and CF                                                                                                    
Update #TempMP                                                                                                    
Set Markprice = S.MarkPrice,                                                                                                    
 CF=S.CF                                                                                             
From                                                                                                    
(                                                                                  
 Select                                                                     
 T1.Symbol,                                                                                  
 dbo.AdjustBusinessDays(DateAdd(day,1,T1.date),-1,SM.AUECID) as LastDate ,                                        
 T1.Date as Date ,                       
 T2.MarkPrice as MarkPrice,                                                                                  
 T2.CF                                                                                           
 From #TempMP T1                                                                               
 Inner Join #SecMasterDataTempTable SM on SM.TickerSymbol = T1.Symbol                                                                                                    
 Inner Join #TempMP T2 on T1.Symbol=T2.Symbol                  
 Where T1.Date <> dbo.AdjustBusinessDays(DateAdd(day,1,T1.Date),-1,SM.AUECID)                                                                                                
 and T2.date= dbo.AdjustBusinessDays(DateAdd(day,1, T1.date),-1,SM.AUECID)                                                                                                    
) As S                                                                                                     
  Where S.symbol=#tempMP.Symbol and S.date=#tempMP.date                                                                                            

Create Table #Temp2                                                                     
(                                                                                                
 TaxlotId varchar(100),                                        
 TradeDate datetime,                                                       
 AvgPrice float,                                                  
 Symbol varchar(100),                                                     
 TaxLotOpenQty float,                                                                                                
 FundID int,                                                                
 CommissionandFees float,                                                                            
 Level2ID int,                                                                     
 OrderSideTagValue varchar(1),                                                                    
 FXRate float,                                                                                  
 FXConversionMethodOperator  Varchar(5)                                  
)                                                                                                
Insert Into #Temp2                                     
(                                                                                            
 TaxlotId ,                                                                                                
 TradeDate,                                                                                                
 AvgPrice,                                                                                                
 Symbol,                 
 TaxLotOpenQty,                                                                                                
 FundID ,                                                       
 CommissionandFees ,                                                                                                
 Level2ID ,                                                                                                
 OrderSideTagValue ,                                                                    
 FXRate ,                                                                  
 FXConversionMethodOperator                                                                                       
)                                                                                            

Select                                                                            
 TaxlotID,                                                                                                
 G.ProcessDate,                                                                                    
 PT.AvgPrice,                                                                                                
 PT.Symbol,                                                                                
 TaxLotOpenQty ,                                                                                                
 FundID,                                                                                                
 PT.OpenTotalCommissionandFees,     
 PT.Level2ID,                                                                                                
 PT.OrderSideTagValue,                                                                    
 G.FXRate,                                                                                  
 G.FXConversionMethodOperator                                                                        
 From #PM_Taxlots PT                                                                                                
 Inner Join T_Group G                                                                     
  On G.GroupID=PT.GroupID                                                                                       
 Where  TaxLotOpenQty<>0                                                                                                  
        And Taxlot_PK in                                                                                                                                                                                                                                      




      (                           
   Select Max(Taxlot_PK) from #PM_Taxlots                                                                                            
   Inner join T_Group on T_Group.GroupID=#PM_Taxlots.GroupID                                                                                                     
   Inner join @SelectedSymbol SSymbol on SSymbol.FutSymbol= T_Group.Symbol                                                                                                                                                            




   Where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                                                                
   Group By Taxlotid                                            
   )                                         

 -- cross between mark price and Positions                                                                                           
Create table #Temp3                                                                                  
(                                                                                                
TaxlotId varchar(100),                                                                                                
TradeDate datetime,                                                                                                
AvgPrice float,                                                                                     
Symbol varchar(100),                                                                                                
Markprice float,                                                                                              
CF float,                                                                                                
MPDate datetime,                                                                                         
TaxLotOpenQty float ,                                                                                                
FundID int,                                                     
OpenCommissionandFees float,                                                                  
Level2ID int,                                                                 
OrderSideTagValue varchar(5),                                                                    
FXRate float,                                                                                  
FXConversionMethodOperator  Varchar(5)                                                                                 
)                                                                                                

Insert Into #Temp3                                                             
(                                                                                                                
TaxlotId ,                                                                                                                
TradeDate,                                                                                          
AvgPrice,                                                                        
Symbol,                                                                                                                
markprice,                                                                                                                
CF,                                         
MPDate,                                                               
TaxLotOpenQty,                                                                                                                
FundID ,                                                                                                                
OpenCommissionandFees,                                                                                                          
--ClosedCommissionandFees,                                                                                                                
Level2ID ,                                                                          
OrderSideTagValue,                                                                                  
FXRate ,                      
FXConversionMethodOperator                                                                                    
)                                                                                                       

Select Distinct                                                                                                                 
TaxlotID,                                    
Tradedate,                                                                                                          
AvgPrice,                                                                                                                
#temp2.Symbol,                                                                                                                
MarkPrice,                                                                                                                
CF,                                                                                                                
#TempMP.date,                                                               
TaxLotOpenQty,                                                              
FundID ,                                                                                              
CommissionandFees ,                                                                     
--0,--0 for ClosedComissionAndFees                                                                                    
Level2ID ,                                                                                                                
OrderSideTagValue,                                                                               
FXRate ,   
FXConversionMethodOperator                                                                                                            
From #Temp2                                                                                                   
Cross Join #TempMP                                                                                                                  
where DateDiff(day,#Temp2.TradeDate,#TempMP.Date) >=0                     
      And #Temp2.Symbol = #TempMP.Symbol                                                                                              

Union                                                                                                

Select Distinct                                                                             
TaxlotID,                                                                                                                
T_Group.ProcessDate as Tradedate ,                                                 
#PM_Taxlots.AvgPrice as AvgPrice,                                                                                          
T_Group.Symbol,                                                                                          
#TempMP.MarkPrice,                                                                                                                
CF,                                                                                                                
#TempMP.Date,                                          
#PM_Taxlots.TaxLotOpenQty,                                                                                                                
#PM_Taxlots.FundID ,                                                   
#PM_Taxlots.OpenTotalCommissionandFees,                                                                      
--0,--0 for ClosedComissionAndFees                                                                                                               
#PM_Taxlots.Level2ID ,       
#PM_Taxlots.OrderSideTagValue ,                                                                                  
T_Group.FXRate,                                             
T_Group.FXConversionMethodOperator                                                                                                                 
From #PM_Taxlots                                                                 
Cross join #TempMP                                                                                
Inner join PM_taxlotClosing on PM_taxlotClosing.TaxlotClosingID = #PM_Taxlots.TaxlotClosingID_FK         
Inner join T_Group on T_Group.GroupID=#PM_Taxlots.GroupID                                                                                                     
Inner Join @SelectedSymbol SSymbol on SSymbol.FutSymbol=T_Group.Symbol                                                                                                                
Where DateDiff(day,T_Group.ProcessDate,#TempMP.Date) >= 0                                                                  
 And T_Group.Symbol=#TempMP.Symbol                                                                            
 And DateDiff(day,#TempMP.Date,PM_TaxlotClosing.AUECLocalDate) >= 0                                                                
 And DateDiff(day,@StartDate,PM_TaxlotClosing.AUECLocalDate) >= 0                                                                                   
 And DateDiff(day,PM_taxlotClosing.AUECLocalDate,@EndDate) >= 0                                                                                                                
 Order by TaxlotId,Date                                                                                              
--for modifying quantity for previous date              
Update #Temp3                                                                     
Set #Temp3.TaxlotopenQty = #PM_Taxlots.TaxlotopenQty                                                                      
, #Temp3.OpenCommissionandFees =#PM_Taxlots.OpenTotalCommissionandFees                                                                                                            
From #PM_Taxlots                                                                                                             
Inner Join #Temp3 On #PM_Taxlots.TaxlotID=#temp3.TaxlotID                              
Where Taxlot_pk in                                                   
(Select Max(Taxlot_pk) from #PM_Taxlots                                                                                                            
 Where Datediff(d,#Temp3.MPdate,#PM_Taxlots.AUECModifiedDate) < 0                        
 And TaxlotID=#Temp3.TaxlotID)                                                                                                  

Update #Temp3                                                                                                            
Set #Temp3.TaxlotopenQty = #PM_Taxlots.TaxlotopenQty                                                                       
, #Temp3.OpenCommissionandFees =#PM_Taxlots.OpenTotalCommissionandFees                                                                                                           
from #PM_Taxlots                                                                                                             
Inner join #Temp3                                                                                             
on #PM_Taxlots.TaxlotID=#Temp3.TaxlotID                                                                                                            
where Datediff(day,#Temp3.MPdate,#PM_Taxlots.AUECModifiedDate)=0                                                                                                  
and Datediff(day,#Temp3.MPdate,#Temp3.TradeDate)= 0                                                                

 INSERT INTO #MarkPriceForStartDate Exec P_GetMarkPriceForBusinessDay @StartDate                                                        
-- Select * From dbo.GetMarkPriceForBusinessDay(@StartDate)                                                                                                          

Declare @MarkEndDate DateTime                                                                  
Set @MarkEndDate=DateAdd(d,1,@EndDate)                                                                                             
  INSERT INTO #MarkPriceForEndDate Exec P_GetMarkPriceForBusinessDay @MarkEndDate                                                        
--Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@EndDate))                                                                                                               

-- Yesterday business date                                                                                                                                      
   INSERT INTO #AUECYesterDates                                                                                                    
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(@StartDate,-1, V_SymbolAUEC.AUECID)                                                                                                                                                        
from V_SymbolAUEC                                                                                                     

   INSERT INTO #AUECBusinessDatesForEndDate                                                                                                                                                                                                        
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1, V_SymbolAUEC.AUECID)                                                                                                                       
from V_SymbolAUEC                                 


Create Table #TempSplitFactorForOpen                                                            
(                                                            
TaxlotID varchar(50),                                                            
Symbol varchar(100),                                                            
SplitFactor float                                                       
)                                                            

Insert InTo #TempSplitFactorForOpen                                          

select NA.TaxlotID, NA.Symbol, IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor  from                                                             
(                                                            
 Select A.Taxlotid,A.symbol, A.StartDate, A.Enddate, VCA.SplitFactor from                                                            
 (                                          
  Select                                            
  TaxlotId,                                     
  PT.Symbol as Symbol,                                                            
  Case                                                                                                                                 
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                    
   Then G.ProcessDate                                                          
   Else @StartDate                                                          
  End as StartDate,                                                            
  @EndDate  as EndDate                                                                        
   from #PM_Taxlots PT                                                                                              
    Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                                   
    Where TaxLotOpenQty <> 0                                                                                                                                 
     and Taxlot_PK in                                                                                                                                           
     (                                                                                                                             
   Select max(Taxlot_PK) from #PM_Taxlots                                  
   Where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                             
   Group by TaxlotId                                                                                                                                                                                                           
     )                                  
 ) as A                     
 Inner Join V_CorpActionData VCA on A.Symbol = VCA.Symbol and                                                            
 Datediff(d,A.StartDate, VCA.Effectivedate) >= 0 and Datediff(d, VCA.Effectivedate, A.Enddate) >= 0 and VCA.IsApplied = 1 and VCA.CorpActionTypeID=6                                                             
) as NA                                                             
Group by NA.TaxlotId,NA.symbol                                                          

Create Table #TempSplitFactorForClosed_1                                                            
(                                                            
TaxlotID varchar(50),                                                            
Symbol varchar(100),                                                            
SplitFactor float,                                    
CorpActionID varchar(100),                                  
Effectivedate DateTime                                 
)                                                            

Insert InTo #TempSplitFactorForClosed_1                                                            

 Select A.Taxlotid,A.symbol, VCA.SplitFactor as SplitFactor,VCA.CorpActionID,VCA.Effectivedate as Effectivedate from                                                           
 (                                                            
  Select                                                             
  PT.TaxlotId,                                                            
  PT.Symbol as Symbol,                                                            
  Case                                              
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                        
   Then G.ProcessDate                                                          
   Else @StartDate                                                         
  End as StartDate,                                                            
  PTC.AUECLocalDate  as EndDate                                                             

  from PM_TaxlotClosing  PTC                                                                                                     
     Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                  
     Inner Join T_Group G on G.GroupID = PT.GroupID                                                             
     Where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                         
      and DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                                                                                                                             
      and PTC.ClosingMode<>7                                                            
 ) as A                                                            
 Inner Join V_CorpActionData VCA on A.Symbol = VCA.Symbol and                                                            
 Datediff(d,A.StartDate, VCA.Effectivedate) >= 0 and Datediff(d, VCA.Effectivedate, A.Enddate) >= 0 and VCA.IsApplied = 1 and VCA.CorpActionTypeID=6                                                            



Select Distinct TaxlotID, CorpActionID,SplitFactor,Symbol,Effectivedate into #TempSplitFactorForClosed_2  from #TempSplitFactorForClosed_1                                    

Create Table #TempSplitFactorForClosed                                                   
(                                        
TaxlotID varchar(50),                                                    
Symbol varchar(100),                                             
SplitFactor float,                                  
Effectivedate DateTime                                     
)                                    

Insert into #TempSplitFactorForClosed                                    
Select                                     
NA.TaxlotID,                                     
NA.Symbol,                                     
IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor,                                  
Max(Effectivedate) as Effectivedate                                       
from #TempSplitFactorForClosed_2 NA Group by NA.TaxlotID,NA.symbol                                                            

Create Table #PositionActivityTable              
(                                                                      
FundID int,                                                                      
Symbol Varchar(100),                                                                      
TaxLotOpenQty float,                                                                     
AvgPrice float,                                                            
ClosingPrice float,                               
AssetID int,                                                                      
CurrencyID int,                                                                      
AUECID int,                                                                      
TotalOpenCommission float,                                                                      
TotalClosedCommission float,                                 
AssetMultiplier float,                                                                      
TradeDate DateTime,                                                                      
ClosingDate DateTime,                                                                      
Mark1 float,                                                                      
Mark2 float,                                                                      
MarketValue1 float,                                                                      
MarketValue2 float,                                         
PositionFrom Varchar(5),                                                                      
ConversionRateTrade float,                                                                      
ConversionRateStart float,                                                                      
ConversionRateEnd float,                                                                      
CompanyName varchar(200),                                                                      
FundName varchar(100),                            
StrategyName varchar(100),                                                                      
Side varchar(10),                                                                      
Asset varchar(50),                                                                      
UDAAsset varchar(100),                                                                      
UDASecurityTypeName varchar(100),                                                                      
UDASectorName varchar(100),                                                                      
UDASubSectorName varchar(100),                                                                      
UDACountryName varchar(100),                                                                      
PutOrCall varchar(5),                                                      
MasterFundName varchar(100),                                                                      
Dividend float,                                                                      
MasterStrategyName varchar(100),                                                                    
UnderlyingSymbol Varchar(100),                                        
CashFXUnrealizedPNL float,                            
NotionalValueForOpenPositions Float,                            
IsSwapped bit,                    
MarketValueStartDate float                                                    
)                                                                   

DECLARE @StartDateForOpenPositions datetime                    
SET @StartDateForOpenPositions = (Select dbo.AdjustBusinessDays(@StartDate,-1, @DefaultAUECID))                           

/******************************************************************                            
      OPEN POSITIONS HANDLING ON START DATE                         
*******************************************************************/                              
Insert Into #PositionActivityTable                                               
Select                                                                                                                   
  PT.FundID    as FundID,                                             
  PT.Symbol    as Symbol,                                                                                                                   
  PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue) as TaxLotOpenQty ,                                                                                                                             
  PT.AvgPrice    as AvgPrice ,                                                                                                                                                                                                                  
  0  as ClosingPrice,                                                             
  G.AssetID as AssetID,                                                                                                                                                                 
  G.CurrencyID As CurrencyID,                                                                                                                                                                          
  G.AUECID As AUECID ,                             

Case                                                                                                                                                               
 When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                                            
 Then IsNull(PT.OpenTotalCommissionAndFees,0)                    
 Else  --When Company and Traded Currency are different                                                                     
  Case                                                                                   
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                             
   Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                                                                                                                                                                 
   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                                                
   Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                             
   When G.FXRate <= 0 OR G.FXRate is null                                                     
   Then  IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                  
  End                                                                                    
End as TotalOpenCommission,                                                                                                                                                                         

 0 as TotalClosedCommission,                                                                                                                    
 SM.Multiplier as AssetMultiplier,                             
 G.ProcessDate   as TradeDate,                                                                                                                                                                                                
 null as ClosingDate,                                           
IsNull(MPS.Finalmarkprice,0) As Mark1,                                                                                                 
IsNull(MPE.Finalmarkprice,0) As Mark2,                                                                               

0 as MarketValue1,                                                          
0 as MarketValue2,                                                              

'OS' as PositionFrom,                             

IsNull(FXDayRatesForTradeDate.RateValue,0) As ConversionRateTrade,                                                                                                 
IsNull(FXDayRatesForStartDate.RateValue,0) As ConversionRateStart,                     
IsNull(FXDayRatesForEndDate.RateValue,0) As ConversionRateEnd,                                                                                                                                    
IsNull(SM.CompanyName,'') as CompanyName,                                                                                                                                                                 
#T_CompanyFunds.FundName as FundName,                                                                         
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                             

Case dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                                                                                              









  When  1                                    
  Then  'Long'                                                                                                                                                                     
  Else  'Short'                                                                                                                             
End as Side,                                 

#T_Asset.AssetName as Asset,                                                                                                                                      
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                                                                              
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                           
IsNUll(SM.SectorName,'Undefined') as UDASectorName,                                   
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                      
IsNull(SM.CountryName,'Undefined') as UDACountryName ,                                                                                                                                                                              
IsNull(SM.PutOrCall,'') as PutOrCall,                                                                                                                            
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                                                                                             
0 as Dividend,                                     
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                          
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,         
0 as CashFXUnrealizedPNL,                            

Case                            
 When (G.AssetID=1 And G.IsSwapped = 1) Or (G.AssetID=5 OR G.AssetID=11)                             
 Then                                                                         
  Case                                                       
   When G.CurrencyID = @BaseCurrencyID                                                                                                                                                                
   Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                                         










   Else                                                                                                                                                                    
    Case                                                                                                
     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                   
     Then IsNull(PT.AvgPrice * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                             










     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                          
     Then IsNull(PT.AvgPrice * 1/G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                           










     Else IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                   



    End                                                                                                                                                  
  End                                                                                                                                                    
 Else 0                                  
End As NotionalValueForOpenPositions,                            

G.IsSwapped as IsSwapped,                    

Case                                                                                              
 When G.CurrencyID =  @BaseCurrencyID                                                     
 Then IsNull((IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1))* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                         









 Else IsNull((IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * IsNull(FXDayRatesForStartDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                             

End as MarketValueStartDate                                                                                         

 from #PM_Taxlots PT                                                                                            
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                             
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                 
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                                               





  Left outer join  T_SwapParameters SW on G.GroupID=SW.GroupID                        
  Inner Join #T_CompanyFunds ON  PT.FundID= #T_CompanyFunds.CompanyFundID             
 Inner Join #T_Asset On #T_Asset.AssetId=G.AssetID                                                                                                                                         
  -- join to get yesterday business day                                                                                                                                    
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                                                                                                      
  LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON G.AUECID = AUECBusinessDatesForEndDate.AUECID                                                                                       

-- Forex Price for Trade Date                                                                      
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                     
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                                                                     
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                         
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                                                 

 -- Forex Price for Start Date                                                                             
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                          
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                                                                 
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                                                    

-- Forex Price for End Date                                                                                                                   
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                         
 on (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                                                                                                                                     
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                                                  
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDate.Date)=0)                                                                                                                                                 
-- Security Master DB Join                                                                
  Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                                                              
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                          

  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                  
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                       
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                      
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                        
  Inner Join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                            
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol                                                           
  Left Outer Join #TempSplitFactorForOpen SplitTab on SplitTab.TaxlotID=PT.TaxlotID                                                                                                  
  where TaxLotOpenQty<>0 and SSymbol.FutSymbol is null                                                                                                                              
   and Taxlot_PK in                                                                                                                      
   (                                       
    select max(Taxlot_PK) from #PM_Taxlots                                                                                                                                                                                                                  
    where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@StartDateForOpenPositions) >=0                                                                                                                                                         
    group by TaxlotId                                                                                                                                                                                                     
  )                                     

/******************************************************************                            
      OPEN POSITIONS HANDLING ON END DATE                           
*******************************************************************/                                                              
Insert Into #PositionActivityTable                      
Select                                                                                                                                                                         
  PT.FundID    as FundID,                                             
  PT.Symbol    as Symbol,                                                                                                                   
  PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue) as TaxLotOpenQty ,                                                                
  PT.AvgPrice    as AvgPrice ,                                                                                                                                                                                                                  
  0  as ClosingPrice,                            
  G.AssetID as AssetID,                                                                                                        
  G.CurrencyID As CurrencyID,                                                                                                                                                                          
  G.AUECID As AUECID ,                             

Case                                                                                                                                                               
 When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                
 Then IsNull(PT.OpenTotalCommissionAndFees,0)                                                    
 Else  --When Company and Traded Currency are different                                                                     
  Case                                                                                   
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                             
   Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                                                                                                                            
   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                                                
   Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                             
   When G.FXRate <= 0 OR G.FXRate is null                                                                                                                        
   Then  IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                  
  End                                                                                                                                                                
End as TotalOpenCommission,                                                                                                                                                                         

 0 as TotalClosedCommission,                                                                                                                    
 SM.Multiplier as AssetMultiplier,                                                                                                                           
 G.ProcessDate   as TradeDate,                                                                                                                                                                                                
 '1800-01-01 00:00:00.000' as ClosingDate,                                                                                                
IsNull(MPS.Finalmarkprice,0) As Mark1,                                                                                                 
IsNull(MPE.Finalmarkprice,0) As Mark2,                                                                               

Case                                                               
 When G.CurrencyID =  @BaseCurrencyID                                                                                                 
 Then                                        
  Case                                                                
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                      
   Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                       









   Else IsNull((IsNull(MPS.Finalmarkprice,0) / IsNull(SplitTab.SplitFactor,1))* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                       









  End                                                                                                                                     
 Else                                                                                                                                                                                            
  Case                                                                                  
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                                                                 
   Then                                                                                                                                          
    Case                                                                                                         
     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                                                                 
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)       
     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                     
     Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/G.FXRate) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                       









     When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                         









    End                                                                                                              
   Else IsNull((IsNull(MPS.Finalmarkprice,0) / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForStartDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                   


  End                                                                                                              
End  as  MarketValue1,                                                             

Case       
 When G.CurrencyID =  @BaseCurrencyID                                                                                                                                                          
 Then IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                     




 Else IsNull(IsNull(MPE.Finalmarkprice,0) * FXDayRatesForEndDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                          







End as MarketValue2,                                                                  

Case                                                                                                 
 When G.AssetID=3                                                                   
 Then 'I'                                                                                                                    
 Else 'O'                                                                                          
End as PositionFrom,                             

IsNull(FXDayRatesForTradeDate.RateValue,0) As ConversionRateTrade,                                                                                                 
IsNull(FXDayRatesForStartDate.RateValue,0) As ConversionRateStart,                          
IsNull(FXDayRatesForEndDate.RateValue,0) As ConversionRateEnd,                                                                                                                                    
IsNull(SM.CompanyName,'') as CompanyName,                                                                                                                    
#T_CompanyFunds.FundName as FundName,                                                                         
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                             

Case dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                                                                                               








  When  1                                    
  Then  'Long'                                                                                                                                                                                                                                
  Else  'Short'                                                                     
End as Side,                                 

#T_Asset.AssetName as Asset,                                                                                          
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                                                                              
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                                                                                                                   
IsNUll(SM.SectorName,'Undefined') as UDASectorName,                                   
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                      
IsNull(SM.CountryName,'Undefined') as UDACountryName ,                                                                                                                                                                              
IsNull(SM.PutOrCall,'') as PutOrCall,                    
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                                                                                             
0 as Dividend,                                                                                      
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                          
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                        
0 as CashFXUnrealizedPNL,                            

Case                            
When (G.AssetID=1 And G.IsSwapped = 1) Or (G.AssetID=5 OR G.AssetID=11)                             
Then                                                                                                                                                                      
 Case                                                       
  When G.CurrencyID = @BaseCurrencyID                                                                                                                                                                
  Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                                          









  Else                                                                                         
   Case                                                                                                                   
    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                   
    Then IsNull(PT.AvgPrice * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                          
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                       
    Then IsNull(PT.AvgPrice * 1/G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                            









    Else IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                            










   End                                                                                                                                                  
 End                                                                                                                                                    
Else 0                                  
End As NotionalValueForOpenPositions,                            

G.IsSwapped as IsSwapped,                    
0 as MarketValueStartDate                                                                                          

 from #PM_Taxlots PT                                                                                            
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                                                           
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                 
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                                               





  Left outer join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                                                                                                        
  Inner Join #T_CompanyFunds ON  PT.FundID= #T_CompanyFunds.CompanyFundID              
  Inner Join #T_Asset On #T_Asset.AssetId=G.AssetID                                                      
  -- join to get yesterday business day                                                                                                                                    
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                                                                                                      
  LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON G.AUECID = AUECBusinessDatesForEndDate.AUECID                                                                                       
  -- Forex Price for Trade Date                                                                            
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                     
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                                                                     
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                     
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                                                 
  -- Forex Price for Start Date                                                                              
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                                                                     
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                        
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                                          
-- Forex Price for End Date                                                                                                                                                                     
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                         
 on (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                                                                                                                                     
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                     
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDate.Date)=0)                                                                                                          
-- Security Master DB Join                                                                                                                                      
  Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                                                                              

  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                          

  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                       

 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                       
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                        

  Inner Join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                                                 
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol                                                           
  Left Outer Join #TempSplitFactorForOpen SplitTab on SplitTab.TaxlotID=PT.TaxlotID                                                                                                  
  where TaxLotOpenQty<>0 and SSymbol.FutSymbol is null                                                                                                                              
   and Taxlot_PK in                                                                                                                      
   (                                                                                                                             
    select max(Taxlot_PK) from #PM_Taxlots                                        
    where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                                                          
    group by TaxlotId                                                                                                                        
  )                                 



/******************************************************************                            
      CLOSED POSITIONS HANDLING - REALIZED PNL                            
*******************************************************************/                                                                                         
Insert Into #PositionActivityTable                                                           

 Select                                                                                                                          
  PT.FundID as FundID,                                                                                                          
  PT.Symbol as Symbol,                                                                                                                                                   
  PTC.ClosedQty * dbo.GetSideMultiplier(G1.OrderSideTagValue)  as ClosedQty ,  -- PTC.ClosedQty * dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)  as ClosedQty ,                                                                   









  PT.AvgPrice as AvgPrice ,                                                                                                                                   
  IsNull(PT1.AvgPrice,0)as ClosingPrice ,                                                                                                                                     
  G.AssetID    as AssetID,                                                                                                                                                   
  G.CurrencyID   as CurrencyID,                                                                                                              
  AUEC.AUECID    as AUECID,                              

   --Open Commission                                                                                                                                             
 Case                                 
  When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                                               

  Then IsNull(PT.ClosedTotalCommissionandFees,0)                                                                                                                   
  Else  --When Company and Traded Currency are different                                                                           
   Case                                                                                                               
    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                  
    Then IsNull(PT.ClosedTotalCommissionandFees * G.FXRate,0)                                                       
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                       
    Then IsNull(PT.ClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                  
    When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                                  
    Then  IsNull(PT.ClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                             
   End                                                                                                                                            
 End as TotalOpenCommission,                             

 --Closed Commission                                                                                                                                   
Case                                       
 When G.CurrencyID =  @BaseCurrencyID --When Company and Traded Currency both are same                                              
 Then IsNull(PT1.ClosedTotalCommissionandFees,0)                                                                                                
 Else  --When Company and Traded Currency are different                                                                          
  Case                             
   When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                                                            
   Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                           
   When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                                                                                
   Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                                                                                
   When G1.FXRate <= 0 OR G1.FXRate is null                                                                                                                                    
   Then  IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDate.RateValue,0),0)                                              
  End                                                                                                                                                       
End as TotalClosedCommission,                                                                                                                                          

SM.Multiplier as AssetMultiplier,                                                                                                                                                                                         
G.ProcessDate  as TradeDate,                                               
IsNull(PTC.AUECLocalDate,'1800-01-01 00:00:00.000') as ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                                                                    
IsNull(MPS.FinalMarkPrice,0) As Mark1,                                                                                                             
IsNull(MPE.FinalMarkPrice,0) As Mark2,                                                                                                

Case                                             
 When G.CurrencyID =  @BaseCurrencyID                                 
 Then                              
  Case                                                                                                                                            
   When DateDiff(d,G.ProcessDate,@StartDate) >0                                            
   Then (IsNull(MPS.FinalMarkPrice,0)/IsNull(SplitTab.SplitFactor,1)) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                 









   Else (G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                  









  End                                                            
 Else                                                                                                                                    
  Case                                                                               
   When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                                                                   
   Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForStartDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                        


   Else                                                                                                                 
    Case                                                                               
     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                                                                
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                          









     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                                                                      
     Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                 

     When G.FXRate <= 0 OR G.FXRate is null                                                                    
     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                   









    End                                                                                                                                                                         
  End                                                                                                        
End as  MarketValue1 ,                                                                                                            

Case                                  
 When G.CurrencyID <> @BaseCurrencyID                                                                
 Then                                                                                                                     
  Case                                                                                         
   When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                             
   Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                        



   When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                         
   Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                   









   Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                   









  End                                                
 Else ISNULL(PT1.AvgPrice,0)* PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                         
End as  MarketValue2,                                                              

'C' as PositionFrom,                                                                                                                               
IsNull(FXDayRatesForTradeDate.RateValue,0) as ConversionRateTrade,                                                                                                                          
IsNull(FXDayRatesForStartDate.RateValue,0) As ConversionRateStart,                                                                                                                            
IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                                                                                                                                   
IsNull(SM.CompanyName,'') as CompanyName,                                                                                                                      
#T_CompanyFunds.FundName  as FundName,                                                                                                                                                                            
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                               

Case dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                                  
 When  1                                                                                                    
 Then  'Long'                                                                                              
 When  -1                                                                                                                                                                     
 Then  'Short'                                                                                            
Else  ''                                                                                                                         
End as Side,                                  

  #T_Asset.AssetName as Asset,                                                                                                                      
  IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                           
  IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                                                                                                                              
  IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                                                                     
  IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                                                    
  IsNull(SM.CountryName,'Undefined') as UDACountryName,                                                                                                                              
  IsNUll(SM.PutOrCall,'') as PutOrCall,                                                                                                            
  IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                                                                                       
  0 as Dividend,                                                                                      
  IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,        
  IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                        
0 as CashFXUnrealizedPNL,                            
0 As NotionalValueForOpenPositions,                            
G.IsSwapped as IsSwapped,                    
0 as MarketValueStartDate                                                                                                                

  from PM_TaxlotClosing PTC                                                                         
  Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                           
  Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                  
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                                                              







  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                              
  Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID                                                                                                                                   
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                            
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                 
  --get yesterday business day                                                                                                                                    
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID              
  Inner Join #T_CompanyFunds ON  PT.FundID= #T_CompanyFunds.CompanyFundID              
 Inner Join #T_Asset On #T_Asset.AssetId=G.AssetID                                                                                                                                      
  -- Security Master DB join                                                                                   
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol          
  -- Forex Price for Trade Date                                                               
  Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                         
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                                                                     
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                    
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                                                                                                     
  -- Forex Price for Start Date                            
  Left outer  join #FXConversionRates FXDayRatesForStartDate                                       
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                                                              
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                               
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                 
  -- Forex Price for Closing Date                            
  Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                                                                                               
 on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                                                                                        
 And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                     
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                                                                                        
  Left Outer Join  T_SwapParameters SW on SW.GroupID=G.GroupID                                                    
  Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                                                                 

  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                                                       









  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                    











  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                           
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                                        




  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                        
  LEFT OUTER join @SelectedSymbol SSymbol on SSymbol.FutSymbol= G.Symbol                                                          
  -- Split Corp Action                                                          
  Left Outer Join #TempSplitFactorForClosed SplitTab on SplitTab.TaxlotID=PT.TaxlotID And DateDiff(day,PT.AUECModifiedDate,SplitTab.Effectivedate) <= 0                                                                                                   
 where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                              
  and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                                          
  and  PTC.ClosingMode<>7 and  SSymbol.FutSymbol is null --7 means CoperateAction!                                                                                                                 


/******************************************************************                            
     INTERNATIONAL FUTURES HANDLING                            
*******************************************************************/                           
Insert Into #PositionActivityTable                                                                 
select                                                                                                 
T1.FundID,                                                                                                                    
T1.Symbol,                                                                                
T1.TaxlotOpenQty * dbo.GetSideMultiplier(T1.OrderSideTagValue) as TaxlotOpenQty,                                                                                                                    
T1.AvgPrice,                                                                                                         
0 as ClosingPrice,                                      
#T_Asset.AssetID as AssetID,                                                                                                                    
0 as CurrencyID,                                                                                 
AUEC.AUECID as AUECID,                                                                        
Case                                                                                                          
  When DateDiff(day,T2.MPdate, @StartDate)=0                
  Then T1.OpenCommissionandFees * T2.CF                                                                                                          
  Else 0                                                                        
End as TotalOpenCommission,                                                                                            
0  as TotalClosedCommission,                                                                                                  
SM.Multiplier  as AssetMultiplier,                                                                                                                                                               
T1.tradedate as TradeDate,                                                                                                                                                                             
'1800-01-01 00:00:00.000' as ClosingDate,                                                                                                 
T1.MarkPrice as Mark1,                                                                                                                                   
T2.MarkPrice as Mark2,                                                                                    
T1.MarkPrice * T2.TaxlotOPenQty * T2.CF * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) as Marketvalue1,                                                                     
T2.MarkPrice * T2.TaxlotOPenQty * T2.CF * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) as Marketvalue2,                                                                                                                              






 Case datediff(day,T2.MPdate, @EndDate)              
  When 0                                                                                       
  Then 'I'                                                                     
  Else 'C'                                                                                                    
End as PositionFrom,                                                             
   0 as ConversionRateTrade,                                                                                                                                                                                                                  
T2.CF as ConversionRateStart,                                                                        
T2.CF as ConversionRateEnd,                                                                                                             
IsNull(SM.CompanyName,'') as CompanyName,                       
#T_CompanyFunds.FundName   as FundName,                                                                                          
'Strategy Unallocated' as StrategyName,                          
Case dbo.GetSideMultiplier(t2.OrderSideTagValue)                                                                                                                                                                                                               









  When  1                                                                                                                                                                                                                                 
  Then  'Long'                                                                       
  Else  'Short'                                                 
End as Side,                                                                                                                                                                                   
IsNull(#T_Asset.AssetName,'Undefined') as Asset,                                                             
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                                                 
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                               
IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                                                          
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                     
IsNull(SM.CountryName,'Undefined') as UDACountryName,                                                                                        
IsNull(SM.PutOrCall,'') as PutOrCall,                                                                     
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName ,                                                                                                                                       
0 as Dividend,                                                               
'Unassigned' as MasterStrategyName,                              
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                        
0 as CashFXUnrealizedPNL,                            
0 As NotionalValueForOpenPositions,                        
'' as IsSwapped,                    
0 as MarketValueStartDate                                            

From #Temp3 T1                                                                                                
 Inner Join #Temp3 T2                                                                                         
 on (T1.TaxlotID=T2.TaxlotID                                                                                         
 And DateDiff(day,(DATEADD(day, DATEDIFF(day, 0, T1.MPdate),1)),T2.MPDate)=0 )                                                                                                                   
 Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = T2.Symbol                                                                                                 
 Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=t1.Level2ID                                                                                                                                                        









 Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                      
Inner Join #T_Asset On #T_Asset.AssetID=AUEC.AssetId                                                                                                   
Inner Join #T_CompanyFunds on #T_CompanyFunds.CompanyFundID= t1.FundID                                                                                                                                     
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                
 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                             
Where DateDiff(d,@StartDate,T2.MPdate) >= 0                                                                                            
And DateDiff(d,T2.MPdate,@Enddate) >=0                                                                                                      


/******************************************************************                            
      INTERNATIONAL FUTURES HANDLING                            
*******************************************************************/                                                           
Insert Into #PositionActivityTable                                                                                                 
Select                                                                                                 
 T1.FundID,                                                                                     
 T1.Symbol,                                            
 T1.TaxlotOpenQty * dbo.GetSideMultiplier(T1.OrderSideTagValue) as TaxlotOpenQty,                                                           
 T1.AvgPrice,                                                                                  
 0 as ClosingPrice,                                                                                                
 #T_Asset.AssetID as AssetID,                                                
 0 as CurrencyID,                                                                                                 
 AUEC.AUECID as AUECID,                                                                         
Case                                                                                                         
   When DateDiff(day,T1.MPdate, T1.TradeDate)=0                                        
   Then T1.OpenCommissionandFees *                                              
  Case                                                                                
    When T1.FXRate is null Or T1.FXRate=0                                                                                       
    Then T1.CF                                                                                             
    Else                                                                                      
  Case  T1.FXConversionMethodOperator                                                                                                          
     When 'M'                                                                                
Then T1.FXRate                                                                                    
     When 'D'                                                                                                         
     Then 1/T1.FXRate                                                                                  
     Else 0                                                                     
  End                                                                                                           
   End                                                                     
 Else 0                                                                                                         
 End As TotalOpenCommission,                                                                     
 0  as TotalClosedCommission,                                                                             
 SM.Multiplier as AssetMultiplier,                                                                                                                                                           
 T1.tradedate as TradeDate,                                                
'1800-01-01 00:00:00.000' as ClosingDate,                                                                                                 
 T1.AvgPrice as Mark1,                           
 T1.MarkPrice as Mark2,                                                                                                                                                
 T1.AvgPrice * T1.TaxlotOPenQty*                                                                                                             
Case                   
  When T1.FXRate is null Or T1.FXRate=0                                                                                       
  Then T1.CF                                                                                          
  Else                                                                                                           
   Case  T1.FXConversionMethodOperator                                                                                           
     When 'M'                                                                                                         
     Then T1.FXRate                                                                                    
     When 'D'                                                                                                         
     Then 1/T1.FXRate                                                                                  
     Else 0                                                                                                         
   End                   
 End                                                                                         
 *IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) As Marketvalue1,                                     
 T1.MarkPrice * T1.TaxlotOPenQty *t1.CF*IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(T1.OrderSideTagValue) As Marketvalue2,                                                                                                                                









 Case datediff(day,t1.MPdate, @EndDate)                                                                                           
  When 0                                                                                         
  Then  'I'           
  Else 'C'                                                                       
 End As PositionFrom,                                                                              
 0 as ConversionRateTrade,                                                                                                  
 T1.CF as ConversionRateStart,                                                                                                                                                                                                                  
 T1.CF as ConversionRateEnd,                             
 IsNull(SM.CompanyName,'') as CompanyName,                                                                                                                                                
 #T_CompanyFunds.FundName   as FundName,                                                                                                                                                                                  
 'Strategy Unallocated' as StrategyName,                                                                                           
 Case dbo.GetSideMultiplier(T1.OrderSideTagValue)                                                                               
  When  1                                                                                                                                                                                                                                 
   Then  'Long'                                               
   Else  'Short'                                                                                                                
 End as Side,                                                                                                                
 IsNull(#T_Asset.AssetName,'Undefined') as Asset,                                                                       
 IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                                       
 IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                                                                 
 IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                
 IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                
 IsNull(SM.CountryName,'Undefined') as UDACountryName,                                                          
 IsNull(SM.PutOrCall,'') as PutOrCall,                                                                                                                                                                                
 IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName ,                                                                                                                                       
 0 as Dividend,                                                                                               
 'Unassigned' As MasterStrategyName,                                                          
 IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                        
0 as CashFXUnrealizedPNL,                            
0 As NotionalValueForOpenPositions,                            
'' as IsSwapped,                    
0 as MarketValueStartDate                                

From #Temp3 T1                                                                                             
--Left Outer  join V_taxlots on V_taxlots.taxlotID =   t1.TaxlotID                                                    
Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = T1.Symbol                                                                
Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=t1.Level2ID                                                                                                                                                          









Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                               
Inner Join #T_Asset On #T_Asset.AssetID=AUEC.AssetId                                                                                               
Inner Join #T_CompanyFunds on #T_CompanyFunds.CompanyFundID=T1.FundID                                                                                 
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                 

LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                 
Where DateDiff(d,T1.MPdate,T1.TradeDate)=0                                                                                                                
      And DateDiff(d,@StartDate,T1.Tradedate) >= 0                                                                          
      And DateDiff(d,T1.Tradedate,@Enddate) >= 0                                                    


/******************************************************************                            
     NAME CHANGE HANDLING                            
*******************************************************************/                                                                           
Insert Into #PositionActivityTable                                                    

Select                                                                                                          
  PT.FundID    as FundID,                                                                                       
  PT.Symbol    as Symbol,                                                                                                                                            
  PTC.ClosedQty * dbo.GetSideMultiplier(G1.OrderSideTagValue) as ClosedQty , --* dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                                                
  PT.AvgPrice as AvgPrice ,                                                                                                                                     
  IsNull(PT1.AvgPrice,0)as ClosingPrice ,                                                                                                                                                       
  G.AssetID    as AssetID,                                                                                                                                                  
  G.CurrencyID   as CurrencyID,                                      
  AUEC.AUECID    as AUECID,                                                                                                                
   --Open Commission                                                                                      
Case                              
When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                                                       









Then IsNull(PT.OpenTotalCommissionandFees,0)                                                                                                 
Else  --When Company and Traded Currency are different                                                                                                         
 Case                                                                    
  When G.FXRate > 0 And G.FXConversionMethodOperator='M'                              
  Then IsNull(PT.OpenTotalCommissionandFees * G.FXRate,0)                                                                                                                                                             
  When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                                                  
  Then IsNull(PT.OpenTotalCommissionandFees * 1/G.FXRate,0)                                                              
  When G.FXRate <= 0 OR G.FXRate is null                                                           
  Then  IsNull(PT.OpenTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                                              
 End                                                                                 
End as TotalOpenCommission,                                                                                               
--Closed Commission        
0 as TotalClosedCommission,                                                                                    
SM.Multiplier as AssetMultiplier,                                                                                                                                                                                           
G.ProcessDate as TradeDate,                                                                                                                                                                                      
PTC.AUECLocalDate as ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                        
IsNull(MPS.FinalMarkPrice,0) as Mark1,                                                                                      
IsNull(MPE.FinalMarkPrice,0) As Mark2,                                                                                                                       
Case                                                                                                                                                     
When G.CurrencyID =  @BaseCurrencyID                                                           
Then                                       
Case                                                                                                                                                                 
  When DateDiff(d,G.ProcessDate,@StartDate) >0                                                                
  Then IsNull(MPS.FinalMarkPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                 
  Else PT.AvgPrice * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                            
 End                                                                   
Else                                
 Case                                                                                  
 When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                                                                                                                                                       
 Then IsNull(MPS.FinalMarkPrice,0) * FXDayRatesForStartDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                  







 Else                                                                                                         
  Case                                        
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                                  
   Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                              









   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                               
   Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                     
   When G.FXRate <= 0 OR G.FXRate is null                                                                                        
   Then IsNull(PT.AvgPrice * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                   
  End                                                  
 End                                                                                                          
End as  MarketValue1 ,                                                        

Case                                                                                                                                                                                                                 
When G.CurrencyID <> @BaseCurrencyID                                                                                                                 
Then                                                                                                                       
 Case                                                                                        
  When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                                        
  Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                       









  When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                  
  Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                     









  Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                    









 End                                                                                                              
Else ISNULL(PT1.AvgPrice,0)* PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                                     





End as  MarketValue2,                                                     
'O' as PositionFrom,                                                                                                                             
IsNull(FXDayRatesForTradeDate.RateValue,0) As ConversionRateTrade,                                            
IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                                                                                  
IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                                                                              
IsNull(SM.CompanyName,'') as CompanyName,                                                                                                                                                    

#T_CompanyFunds.FundName  as FundName,                                                                                                                                                                              
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                                                                                                                                             
Case dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                  
  When  1                                                                                   
  Then  'Long'                                                                                                                                                                       
  When  -1                                                                                              
  Then  'Short'                                                   
  Else  ''                                                               
End as Side,                                                                                                                                                                 
#T_Asset.AssetName as Asset,                                                                                                                       
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                      
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                
IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                     
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                                                                
IsNull(SM.CountryName,'Undefined') as UDACountryName,                                     
IsNUll(SM.PutOrCall,'') as PutOrCall,                                                                                                                                                                              
IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                                
0 as Dividend,                                                                                     
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                       
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                        
0 as CashFXUnrealizedPNL,                            
0 As NotionalValueForOpenPositions,                      
G.IsSwapped as IsSwapped,                    
0 as MarketValueStartDate                                                                                                                  

  from PM_TaxlotClosing  PTC                                                                                  
  Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                               
  Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                             
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                   
  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                                                                                                                                                
  Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID                                                                            
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                                                   
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                    
  --get yesterday business day                                                                                                                                      
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID              
  Inner Join #T_CompanyFunds ON  PT.FundID= #T_CompanyFunds.CompanyFundID            
  Inner Join #T_Asset On #T_Asset.AssetId=G.AssetID                                      
  -- Security Master DB join                                                                                                                                        
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                   

  -- Forex Price for Trade Date                                                                                                                                       
  Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                         
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                  
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                       
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                   

  -- Forex Price for Start Date                                                                                                                                           
  Left outer  join #FXConversionRates FXDayRatesForStartDate                                                                                                                                 
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                                                                    
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                       
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                   

  -- Forex Price for Closing Date                                                                         
 Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                            
 on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                                                                                         
 And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                       
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                      


 Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                                                                        








 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                     









 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                              
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                         
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                   
  Where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                       
   and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                             
   and  PTC.ClosingMode=7 --7 means CoperateAction!                               


/******************************************************************                            
     DIVIDEND HANDLING                            
*******************************************************************/                                                                                             
Insert Into #PositionActivityTable                                                                            
 Select                     
  IsNull(CashDiv.FundId,0) as FundID,                                                                                                                
   CashDiv.Symbol,                                                                                                                                                                                        
   0  as TaxLotOpenQty ,                                                                                                                                                         
   0  as AvgPrice ,                         
   0  as ClosingPrice,                                                                             
   min(Isnull(AUEC.AssetID,0)) as AssetID,                                                                                     
   Min(SM.CurrencyID) as CurrencyID,                                                            
   Min(Isnull(AUEC.AUECID,0)) as AUECID ,                                                                                                                                                                                                    
   0  as TotalOpenCommission,                                                             
   0  as TotalClosedCommission,                                                                       
   0  as AssetMultiplier,                                             
   min(CashDiv.ExDate) as TradeDate,                                                                                                        
   '1800-01-01 00:00:00.000' as ClosingDate,                                                         
   0  as Mark1,                                                                                                                                                                                                    
   0  as Mark2,                                                                  
   0  as  MarketValue1,                                                                                                                                                          
   0  as  MarketValue2 ,                                                    
   'D' as PositionFrom,                                                                                                                                                                                                     
  Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) as ConversionRateTrade,                                                                                                 
   0 as ConversionRateStart,                                                                                                                                        
   0 as ConversionRateEnd,                         
   Min(IsNull(SM.CompanyName,'')) as CompanyName,                                                                                                                                             
   Min(#T_CompanyFunds.FundName)   as FundName,                                                                                                                                               
   'Strategy Unallocated' as StrategyName,                                            
Case                                             
When Sum(CashDiv.Amount) >= 0                                            
Then 'Long'                                            
Else 'Short'                                            
End as Side,                                                                                                                                                                              
--   'Undefined' as Side,                                                                                                     
   Min(IsNull(#T_Asset.AssetName,'Undefined')) as Asset,                                                                                                 
   Min(IsNull(SM.AssetName,'Undefined')) as UDAAsset,                                            
   Min(IsNull(SM.SecurityTypeName,'Undefined')) as UDASecurityTypeName,                                                                                                                                                                              
   Min(IsNull(SM.SectorName,'Undefined')) as UDASectorName,                                                                                         
   Min(IsNull(SM.SubSectorName,'Undefined')) as UDASubSectorName,                                                                                           
   Min(IsNull(SM.CountryName,'Undefined')) as UDACountryName,                                                                                                                                                           
   Min(IsNull(SM.PutOrCall,'')) as PutOrCall,                                                                                                                                
   Min(IsNull(CMF.MasterFundName,'Unassigned')) as MasterFundName,                                                          
 Case                                                                 
  When Min(SM.CurrencyID) <>  @BaseCurrencyID                                 
  Then Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) * Sum(CashDiv.Amount)                                              
  Else Sum(CashDiv.Amount)                                             
End as Dividend,                                                                                       
   'Unassigned' as MasterStrategyName,                                                  
   MIn(IsNull(SM.UnderlyingSymbol,'')) as UnderlyingSymbol,                                        
   0 as CashFXUnrealizedPNL,                            
0 As NotionalValueForOpenPositions,                   
'' as IsSwapped,                    
0 as MarketValueStartDate                                  

  from  T_CashTransactions CashDiv
   inner JOIN T_ActivityType on (T_ActivityType.ActivityTypeId = CashDiv.ActivityTypeId and ActivitySource = 2)                                                                                
   Inner Join #T_CompanyFunds ON  CashDiv.FundID= #T_CompanyFunds.CompanyFundID                                                      
   LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = CashDiv.Symbol                                                                                                                                                                            
   Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                                            
   Inner Join #T_Asset On #T_Asset.AssetID=AUEC.AssetId                                                                                                  
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                  









   LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                          
   Left outer  join #FXConversionRates FXDayRatesForDiviDate On (FXDayRatesForDiviDate.FromCurrencyID = SM.CurrencyID                                                                  
   And FXDayRatesForDiviDate.ToCurrencyID = @BaseCurrencyID And DateDiff(d,CashDiv.ExDate,FXDayRatesForDiviDate.Date)=0)                                                                           
 Where DateDiff(d,@StartDate,CashDiv.ExDate) >=0                                                                                                        
       and DateDiff(d,CashDiv.ExDate,@EndDate)>=0                                                                                                                      
       Group By CashDiv.FundId,CashDiv.Symbol,CashDiv.ExDate                   


Declare @RecentEndDateforNonZeroCash datetime                      
Set @RecentEndDateforNonZeroCash = (Select dbo.[GetRecentDateForNonZeroCash](@EndDate))                         


Declare @BusinessAdjStartDateforCash datetime                       
Set @BusinessAdjStartDateforCash = dbo.AdjustBusinessDays(@StartDate,-1, @DefaultAUECID)          
Declare @RecentStartDateforNonZeroCash datetime                      
Set @RecentStartDateforNonZeroCash = (Select dbo.[GetRecentDateForNonZeroCash](@BusinessAdjStartDateforCash))                                                         

/******************************************************************                            
     CASH HANDLING ON START DATE                            
*******************************************************************/                        
IF @IncludeCash = 1                      
BEGIN                      
Insert into #PositionActivityTable                                                                                                                                          
Select                                                                                                                                                                              
  CFCC.FundId as FundID,                                                                                                                
  MIN(CLocal.CurrencySymbol) As Symbol,                                                                                                                                                                                        
  SUM(CashValueLocal) As TaxLotOpenQty ,                                              
   0  as AvgPrice ,                                                      
   0  as ClosingPrice,                                                                                               
   6  as AssetID,                                                                                                                   
  MIN(CFCC.LocalCurrencyID) As CurrencyID,                                                                                                                                                                                                                     





   0  as AUECID ,                                                                                                                                                                                                      
   0  as TotalOpenCommission,                                                                                                                                             
   0  as TotalClosedCommission,                                                                                                                    
   1  as AssetMultiplier,                                                                                                       
  Min(CFCC.Date) as TradeDate,                                                                                                        
  '1800-01-01 00:00:00.000' As ClosingDate,                                                                                       
   0  as Mark1,                               
   0  as Mark2,                                                           
   0  as  MarketValue1,                                                                                                          
   0 as MarketValue2 ,                                                                                                                                                             
   'CASH' as PositionFrom,                                                                                                                                                                                           
  0 as ConversionRateTrade,                                                                   
  0 as ConversionRateStart,                                                          
  0 as ConversionRateEnd,                                                
  '' as CompanyName,                                                                         
  Min(#T_CompanyFunds.FundName) As FundName,                                                                                                                             
   'Strategy Unallocated' as StrategyName,                                                                       
  Case                                                                     
When SUM(CashValueBase) >= 0                                        
 Then 'Long'                                                                    
 Else 'Short'                                                                    
  End as Side,                                                 
 'Cash' as Asset,                                                                                                                                                                                                       
'Undefined' as UDAAsset,                                                          
'Undefined' as UDASecurityTypeName,                                                                                                                                                         
'Undefined' as UDASectorName,                                                                
'Undefined' as UDASubSectorName,                                                                                                                                                                
'Undefined' as UDACountryName,                                                                                   
'' as PutOrCall,                                                                                                                                                  
  Min(IsNull(CMF.MasterFundName,'Unassigned')) As MasterFundName,                                                                                                        
0 as Dividend,                                                                                                                
 'Unassigned' as MasterStrategyName,                                                                            
'' as UnderlyingSymbol,                                                                      
0 as CashFXUnrealizedPNL,                                                
0 As NotionalValueForOpenPositions,                            
'' as IsSwapped,                    
Sum(IsNull(CFCC.CashValueBase,0)) as MarketValueStartDate                                                                 

  from PM_CompanyFundCashCurrencyValue  CFCC                                                                                                             
   INNER JOIN T_Currency CLocal ON CFCC.LocalCurrencyID = CLocal.CurrencyID --AND CFCC.BalanceType=1                                                                       
   INNER JOIN T_Currency CBase ON BaseCurrencyID = CBase.CurrencyID                                                                                          
   Inner Join #T_CompanyFunds ON  CFCC.FundID= #T_CompanyFunds.CompanyFundID                                                
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                            
   LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                   
  -- Forex Price for Start Date other than FX Trade                                                                
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                      
   on (FXDayRatesForStartDate.FromCurrencyID = CFCC.LocalCurrencyID                                               
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                                               
 And DateDiff(d,dbo.AdjustBusinessDays(@StartDate,-1,@DefaultAUECID),FXDayRatesForStartDate.Date)=0)                                                                                             
-- Forex Price for End Date other than FX Trade                                                                                                                                                                
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                                                                              
   on (FXDayRatesForEndDate.FromCurrencyID = CFCC.LocalCurrencyID                                                                                                                                          
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                                                    
 And DateDiff(d,dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1,@DefaultAUECID),FXDayRatesForEndDate.Date)=0)                                         
   WHERE DateDiff(Day,CFCC.Date,@RecentStartDateforNonZeroCash)=0 --And CFCC.BalanceType=1                                                                                      
    Group By CFCC.FundID,CFCC.LocalCurrencyID                 

 END                       


/******************************************************************                            
     CASH HANDLING ON END DATE                           
*******************************************************************/                 
IF @IncludeCash = 1                      
BEGIN                                                                                                                         
Insert Into #PositionActivityTable                                                                                  
 Select                                                                                                                                                    
  CFCC.FundId as FundID,                                                                                                                
  MIN(CLocal.CurrencySymbol) As Symbol,                                                                                     
  SUM(CashValueLocal) As TaxLotOpenQty ,                                                                                                                                                         
  0  as AvgPrice ,                                                                                                                                  
  0  as ClosingPrice,                                                                                            
  6  as AssetID,                                                                                                                                                                                                                                              









  MIN(CFCC.LocalCurrencyID) As CurrencyID,                                                                                              

  0  as AUECID ,                                                                                                                                                                                         
  0  as TotalOpenCommission,                                                                                                                                                                                                    
  0 as TotalClosedCommission,                                                                         
  1  as AssetMultiplier,                                                                            
  Min(CFCC.Date) as TradeDate,                                                                                                        
  '1800-01-01 00:00:00.000' As ClosingDate,                                                                                       
  0  as Mark1,                                                                                                 
  0  as Mark2,                                                                                                                                                                                                
  0  as MarketValue1,                        

 SUM(Isnull(CFCC.CashValueBase,0))  As MarketValue2 ,                       

  'CASH' as PositionFrom,                                                                                                                   
  0 as ConversionRateTrade,                                                                                                                                                               
  Min(IsNull(FXDayRatesForStartDate.RateValue,0)) as ConversionRateStart,                                              
  Min(IsNull(FXDayRatesForEndDate.RateValue,0)) as ConversionRateEnd,                                                                                                                                                                                    
  '' as CompanyName,                                                                                                                                                             
  Min(#T_CompanyFunds.FundName) As FundName,                                 
  'Strategy Unallocated' as StrategyName,                                        
Case                          
When SUM(CFCC.CashValueBase) >= 0                                        
 Then 'Long'                                        
 Else 'Short'                                        
End as Side,                                                                                              
--  'Undefined' as Side,                                                                                                     
  'Cash' as Asset,                                                                      
  'Undefined' as UDAAsset,                                                                                                                                                             
  'Undefined' as UDASecurityTypeName,                                                                                                                                                                              
  'Undefined' as UDASectorName,                                                                                         
  'Undefined' as UDASubSectorName,                                                                                                                                      
  'Undefined' as UDACountryName,                                                                                                                                                                              
  '' as PutOrCall,                                                                                                                                
  Min(IsNull(CMF.MasterFundName,'Unassigned')) As MasterFundName,                                                                              
  0 as Dividend,                                                                                      
  'Unassigned' as MasterStrategyName,                                                          
  '' as UnderlyingSymbol,                       
Case                                             
  When CFCC.LocalCurrencyID <> @BaseCurrencyID                                            
  Then (Min(IsNull(FXDayRatesForEndDate.RateValue,0)) - Min(IsNull(FXDayRatesForStartDate.RateValue,0))) * Sum(CFCC.CashValueLocal)                           
  Else 0                                            
End as CashFXUnrealizedPNL,                        

0 As NotionalValueForOpenPositions,                            
'' as IsSwapped,                    
0 as MarketValueStartDate                                           

  from PM_CompanyFundCashCurrencyValue  CFCC                                                                                                             
   INNER JOIN T_Currency CLocal ON CFCC.LocalCurrencyID = CLocal.CurrencyID                                                                        
   INNER JOIN T_Currency CBase ON BaseCurrencyID = CBase.CurrencyID                                                                                                               
   Inner Join #T_CompanyFunds ON  CFCC.FundID= #T_CompanyFunds.CompanyFundID                                                                   
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                            
   LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                         
   -- Forex Price for Start Date other than FX Trade                                                                                        
   Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                                                                  
   on (FXDayRatesForStartDate.FromCurrencyID = CFCC.LocalCurrencyID                                                                                                              
   And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                         
   And DateDiff(d,dbo.AdjustBusinessDays(@StartDate,-1,@DefaultAUECID),FXDayRatesForStartDate.Date)=0)                                                                     
   -- Forex Price for End Date other than FX Trade                                                      
   Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                       
   on (FXDayRatesForEndDate.FromCurrencyID = CFCC.LocalCurrencyID                                                                                                                                          
   And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                              
   And DateDiff(d,dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1,@DefaultAUECID),FXDayRatesForEndDate.Date)=0)                                                                     
   WHERE DateDiff(Day,CFCC.Date,@RecentEndDateforNonZeroCash)=0  --AND CFCC.BalanceType=1                                                                                      
    Group By CFCC.FundID,CFCC.LocalCurrencyID                

End                             


/*************************************************************                            
      ADDING PNL COLUMNS TO TABLE                          
**************************************************************/                         
Alter Table #PositionActivityTable                            
Add                   
UnRealizedPNL Float Null,                            
RealizedPNL Float Null                            

Update #PositionActivityTable                                                                 
Set UnRealizedPNL=0,                           
RealizedPNL=0                            


/******************************************************************                            
      FIXED INCOME UPDATE                            
*******************************************************************/                            
Update #PositionActivityTable                            
set RealizedPNL =                             
case                             
 when PositionFrom = 'C'                            
 then (MarketValue2 - MarketValue1)/100                            
 else (MarketValue2 - MarketValue1)                            
end,                            

UnRealizedPNL =                             
case                             
 when  PositionFrom = 'O'                            
 then (MarketValue2 - MarketValue1)/100                            
 else (MarketValue2 - MarketValue1)                            
end                            
where Asset = 'FixedIncome'                            



/******************************************************************                            
      CLOSED POSITIONS UPDATE                            
*******************************************************************/                            
Update #PositionActivityTable                                                              
Set RealizedPNL =                             
Case                             
 when DateDiff(day,@StartDate,TradeDate)>=0                            
 then (MarketValue2 - MarketValue1 - TotalOpenCommission - TotalClosedCommission)                             
 else  (MarketValue2 - MarketValue1 - TotalClosedCommission)                             
end                             
where (PositionFrom = 'C' or PositionFrom = 'I')                            



/******************************************************************                            
      OPEN POSITIONS UPDATE                            
*******************************************************************/                                                              
Update #PositionActivityTable                           
Set UnRealizedPNL =                             
Case                             
 When DateDiff(day,@StartDate,TradeDate)>=0                                
 then (MarketValue2 - MarketValue1 - TotalOpenCommission)                              
 else (MarketValue2 - MarketValue1)                            
end                                                       
Where PositionFrom = 'O'                            

/******************************************************************                            
     FUTURES UPDATE -- i.e. no unrealized P&L for Futures                      
*******************************************************************/                            
Update #PositionActivityTable                            
Set UnRealizedPNL = 0                            
Where PositionFrom = 'I'                            


/******************************************************************                            
     FX AND FXFORWARD, SWAPS UPDATE                            
*******************************************************************/                            
Update #PositionActivityTable                  
Set MarketValue2 =                    
case                   
when PositionFrom = 'O'                  
then MarketValue2 - (NotionalValueForOpenPositions + TotalOpenCommission)                   
else MarketValue2                           
end,                  

MarketValueStartDate =                   
case                   
when PositionFrom = 'OS'                  
then MarketValueStartDate - (NotionalValueForOpenPositions + TotalOpenCommission)                   
else 0                   
end                       
where ((Asset = 'FX' or Asset = 'FXForward') or (Asset = 'Equity' and IsSwapped = 1))                    


Update #PositionActivityTable                                                              
Set MarketValue1 = 0,                      
MarketValue2 = 0 ,                          
TaxLotOpenQty = 0                           
where (PositionFrom = 'C')             


Create Table #TempOutput                                                     
(                                                      
Symbol varchar(100),                             
--TaxLotOpenQty Float,                                                     
MarketValueStart float,                                                      
MarketValueEnd float,                                                       
--PositionFrom varchar(5),                                    
Side Varchar(10),                                                      
RealizedPNL float,                                                      
UnRealizedPNL float,                            
CashFXUnrealizedPNL Float,                            
Dividend Float                                               
)                              

Insert into #TempOutput                         
Select                                                       
Symbol,                                                                
Sum(MarketValueStartDate) as MarketValueStart,                                                      
Sum(MarketValue2) as MarketValueEnd,                                                      
Side,                                                      
Sum(RealizedPNL) as RealizedPNL,                                                      
Sum(UnRealizedPNL) as UnRealizedPNL,                            
Sum(CashFXUnrealizedPNL) As CashFXUnrealizedPNL,                            
Sum(Dividend) As Dividend                                                       
from #PositionActivityTable Activity                   
--inner join #T_UdaAsset Asset on Asset.AssetName = Activity.UDAAsset                     
--inner join #T_UdaSecurityType SecurityType on SecurityType.SecurityTypeName = Activity.UDASecurityTypeName                   
--inner join #T_UdaSector Sector on Sector.SectorName = Activity.UDASectorName                   
--inner join #T_UdaSubSector SubSector on SubSector.SubSectorName = Activity.UDASubSectorName                   
--inner join #T_UdaCountry Country on Country.CountryName = Activity.UDACountryName                                                  
Group By Symbol,Side                            


Select * from #TempOutput Order By Symbol                                                                    

Drop Table #TempMP,#Temp2,#Temp3,#MarkPriceForStartDate,#MarkPriceForEndDate,#FXConversionRates,#AUECYesterDates,#AUECBusinessDatesForEndDate,#SecMasterDataTempTable,#PositionActivityTable                                                           
Drop Table #T_CompanyFunds,#T_Asset,#PM_Taxlots,#TempOutput,#TempSplitFactorForClosed,#TempSplitFactorForOpen,#TempSplitFactorForClosed_1,#TempSplitFactorForClosed_2                            

END 
