CREATE Procedure [dbo].[P_GetPositions_New]                                                                            
(                                                                                                                                                      
 @ToAllAUECDatesString VARCHAR(MAX)                                                                                                                                  
)                                               
As                                                                                                                                   
Begin                                                                                                                                          
                                                                                                                       
Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                                      
                                                                                                                       
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)                                                                                                                                      
                                                                                                                       
-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable 
Create TABLE #Final (
	[TaxLot_PK] [bigint]  NOT NULL,
	[TaxLotID] [varchar](50) NOT NULL,
	[Symbol] [varchar](100) NOT NULL,
	[TaxLotOpenQty] [float] NOT NULL,
	[AvgPrice] [float] NOT NULL,
	[TimeOfSaveUTC] [datetime] NULL,
	[GroupID] [nvarchar](50) NULL,
	[AUECModifiedDate] [datetime] NOT NULL,
	[FundID] [int] NULL,
	[Level2ID] [int] NULL,
	[OpenTotalCommissionandFees] [float] NULL,
	[ClosedTotalCommissionandFees] [float] NULL,
	[PositionTag] [int] NULL,
	[OrderSideTagValue] [nchar](10) NULL,
	[TaxLotClosingId_Fk] [uniqueidentifier] NULL,
	[ParentRow_Pk] [bigint] NULL,
	[AccruedInterest] [float] NULL)

INSERT INTO #Final                                                                                                                                    
 
Select TaxLot_PK,TaxLotID,Symbol,TaxLotOpenQty,AvgPrice,TimeOfSaveUTC,GroupID,AUECModifiedDate,FundID,Level2ID,OpenTotalCommissionandFees
,ClosedTotalCommissionandFees,PositionTag,OrderSideTagValue,TaxLotClosingId_Fk,ParentRow_Pk,AccruedInterest From PM_Taxlots                                                                                               
Where                                          
taxlot_PK in                                                                    
(                                                            
  Select max(taxlot_PK) from PM_Taxlots                                                                                 
  Inner join  T_Group G on G.GroupID=PM_Taxlots.GroupID                                                    
  inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                              
  inner join @AUECDatesTable AUECDates on AUEC.AUECID = AUECDates.AUECID                                                                       
  where Datediff(d,PM_Taxlots.AUECModifiedDate,AUECDates.CurrentAUECDate) >= 0  AND
        Datediff(d,PM_Taxlots.AUECModifiedDate,(Select SpecifiedDate FROM T_PositionDate)) < 0      
  group by taxlotid                                      
)                                                             
UNION 
SELECT TaxLot_PK,TaxLotID,Symbol,TaxLotOpenQty,AvgPrice,TimeOfSaveUTC,GroupID,AUECModifiedDate,FundID,Level2ID,OpenTotalCommissionandFees
,ClosedTotalCommissionandFees,PositionTag,OrderSideTagValue,TaxLotClosingId_Fk,ParentRow_Pk,AccruedInterest FROM PM_SnapShot


SELECT --PT.AUECModifiedDate,
PT.TaxLotID as TaxLotID,                                                                                                                                                                            
 G.AUECLocalDate as TradeDate,                                                                            
 PT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                                                                           
           
 PT.Symbol as Symbol ,                                                                              
 PT.TaxLotOpenQty as TaxLotOpenQty ,                                                                                
 PT.AvgPrice as AvgPrice ,                                                                   
 PT.FundID as FundID,                                                                                           
 G.AssetID as AssetID,                                       
 G.UnderLyingID as UnderLyingID,                                                                                                    
 G.ExchangeID as ExchangeID,                                                   
 G.CurrencyID as CurrencyID,                                                                                                                          
 G.AUECID as AUECID ,                                                                            
 PT.OpenTotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                                                  
 isnull(V_SecMasterData.Multiplier,1) as Multiplier,                                                                                                                              
 G.SettlementDate as SettlementDate,                                                                                                                      
V_SecMasterData.LeadCurrencyID,          
V_SecMasterData.VsCurrencyID,        
isnull(V_SecMasterData.ExpirationDate,'1/1/1800') as ExpirationDate,                                        
 G.Description as Description,                                                                                           
 PT.Level2ID as Level2ID,                                                                              
 isnull( (PT.TaxLotOpenQty * SW.NotionalValue / G.Quantity) ,0) as NotionalValue,                                                                                          
 isnull(SW.BenchMarkRate,0) as BenchMarkRate,                                                         
 isnull(SW.Differential,0) as Differential,                                            
 isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                       
 isnull(SW.DayCount,0) as DayCount,                                                                                          
 isnull(SW.SwapDescription,'') as SwapDescription,                                                                                          
 SW.FirstResetDate as FirstResetDate,                                                                 
 SW.OrigTransDate as OrigTransDate,                                                                      
 G.IsSwapped as IsSwapped,                                                                   
 G.AllocationDate as AUECLocalDate,                                                                  
 G.GroupID,                                                                
 PT.PositionTag,                                                          
 G.FXRate,                                                          
 G.FXConversionMethodOperator,                                                     
 isnull(V_SecMasterData.CompanyName,'') as CompanyName,                                                                    
 isnull(V_SecMasterData.UnderlyingSymbol,'') as UnderlyingSymbol,                                  
 IsNull(V_SecMasterData.Delta,1) as Delta,                                
 IsNull(V_SecMasterData.PutOrCall,'') as PutOrCall,                              
 G.IsPreAllocated,                              
 G.CumQty,                              
 G.AllocatedQty,                              
 G.Quantity,                    
 PT.taxlot_Pk,                    
 PT.ParentRow_Pk ,      
 IsNull(V_SecMasterData.StrikePrice,0) as StrikePrice,    
 G.UserID,    
 G.CounterPartyID,
 CATaxlots.CorpActionID,
 V_SecMasterData.Coupon,
V_SecMasterData.IssueDate,
V_SecMasterData.MaturityDate,
V_SecMasterData.FirstCouponDate,
V_SecMasterData.CouponFrequencyID,
V_SecMasterData.AccrualBasisID,
V_SecMasterData.BondTypeID,
V_SecMasterData.IsZero                                                 
 FROM #Final PT                                                                                               
 Inner join  T_Group G on G.GroupID=PT.GroupID                             
 Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                            
 LEFT OUTER JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol 
 Left Outer Join PM_CorpActionTaxlots CATaxlots on  PT.Taxlot_PK = CATaxlots.FKId
where taxlot_PK in                                    
(                                                            
	Select max(taxlot_PK) from #Final group by taxlotid                                      
) and TaxLotOpenQty<>0  order by taxlotid
                                            
RETURN;                                                                              
End 



