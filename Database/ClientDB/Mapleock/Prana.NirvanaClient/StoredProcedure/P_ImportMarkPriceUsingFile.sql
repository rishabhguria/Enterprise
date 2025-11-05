 CREATE Procedure [dbo].[P_ImportMarkPriceUsingFile]        
 (            
@Path varchar(255) ,        
@Sheet varchar(50) ,        
@InputDate DateTime          
)         
As                                        
Begin                                      
        
SET NOCOUNT off         
        
DECLARE @IndexInputQuery VARCHAR(MAX)        
        
CREATE TABLE #TempDataTable(        
 Account [nvarchar](255) NULL,        
 GSRef [nvarchar](255) NULL,        
 ProductType [nvarchar](255) NULL,        
 Protection [nvarchar](255) NULL,        
 [Description] [nvarchar](255) NULL,        
 ProductIdentifier [nvarchar](255) NULL,        
 CCY [nvarchar](255) NULL,        
 OriginalNotional [float] NULL,        
 CurrentNotional [float] NULL,        
 FixedCpn [float] NULL,        
 TradeDate [nvarchar](255) NULL,        
 EffectiveDate [nvarchar](255) NULL,        
 MaturityDate [nvarchar](255) NULL,        
 BlankN [nvarchar](255) NULL,        
 Price1 [nvarchar](255) NULL,        
 BlankP [nvarchar](255) NULL,        
 Price2 [float] NULL,        
 ApproxCurrentRate [nvarchar](255) NULL,        
 AccruedLocal [nvarchar](255) NULL,        
 NPVLessAccured [float] NULL,        
 NPVLocal [float] NULL,        
 NPVUSD [float] NULL        
)         
        
SET @IndexInputQuery ='        
insert INTO #TempDataTable         
SELECT *               
FROM OPENROWSET(''Microsoft.Jet.OLEDB.4.0'',                      
''Excel 8.0;Database=' + @Path +';HDR=YES'', ''select * from [' + @Sheet+'$A7:V]'');  '         
-----------------------------------------------------------------------------------------------------        
exec (@IndexInputQuery)        
-------------------------------------------------------------------------------------------------------------------------        
-- Delete Unnecessary Data on TempDataTable        
delete #TempDataTable        
where Isnumeric(NPVLessAccured)= 0        
-------------------------------------------------------------------------------------------------        
    
---------------------------------------------------------------------------------------------        
Create table #TempConnectionTable        
(        
 Symbol varchar(100),        
 DescriptionOnFile varchar(100),        
 Price Decimal(19,9)        
)        
        
        
Insert Into #TempConnectionTable Values ('NAVI CDS USD SR 5Y D14','NAVIENT CORPORATION',0)        
Insert Into #TempConnectionTable Values ('NAVI CDS USD SR 5Y D14_A','NAVIENT CORPORATION',0)        
Insert Into #TempConnectionTable Values ('XRX CDS USD SR 5Y D14','XEROX CORPORATION',0)        
Insert Into #TempConnectionTable Values ('XRX CDS USD SR 5Y D14_A','XEROX CORPORATION',0)        
        
Select * Into #TempfinalTable from #TempDataTable        
full outer join #TempConnectionTable        
On #TempConnectionTable.DescriptionOnFile = #TempDataTable.[Description]        
     
        
Update  #TempfinalTable        
Set Price = Case        
    When (Symbol='XRX CDS USD SR 5Y D14_A' and NPVLessAccured < 0) Then '0.00000000001'        
    When (Symbol='XRX CDS USD SR 5Y D14' and NPVLessAccured < 0) Then (NPVLessAccured*(-1)/CurrentNotional*100)        
        
    When (Symbol='XRX CDS USD SR 5Y D14_A' and NPVLessAccured > 0) Then (NPVLessAccured/CurrentNotional*100)        
    When (Symbol='XRX CDS USD SR 5Y D14' and NPVLessAccured > 0) Then '0.00000000001'        
        
    When (Symbol='NAVI CDS USD SR 5Y D14_A' and NPVLessAccured < 0) Then '0.00000000001'        
    When (Symbol='NAVI CDS USD SR 5Y D14' and NPVLessAccured < 0) Then (NPVLessAccured*(-1)/CurrentNotional*100)        
        
    When (Symbol='NAVI CDS USD SR 5Y D14_A' and NPVLessAccured > 0) Then (NPVLessAccured/CurrentNotional*100)        
    When (Symbol='NAVI CDS USD SR 5Y D14' and NPVLessAccured > 0) Then '0.00000000001'        
   End        
        
        
Select * from #TempfinalTable        
        
INSERT INTO PM_DayMarkPrice(Date,Symbol,ApplicationMarkPrice,PrimeBrokerMarkPrice,FinalMarkPrice,IsActive,ForwardPoints,FundID,PriceTypeID,SourceID,IsApproved,AmendedMarkPrice)        
SELECT @InputDate, F.Symbol,0,0,F.Price,1,0,0,0,1,1,F.Price          
FROM #TempfinalTable F         
INNER JOIN V_SecMasterData SM WIth (NoLock) ON F.Symbol = SM.TickerSymbol        
WHERE F.Price IS NOT NULL AND F.Price <> 0        
        
---------------------------------------------------------------------------------------------        
-- Drop Table        
Drop Table #TempDataTable, #TempfinalTable,#TempConnectionTable        
        
END