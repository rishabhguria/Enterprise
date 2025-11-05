
--exec PMGetReportsData_RealizedPNL 5, '03-01-2009','04-06-2009'                                                                 
                                                                  
create PROCEDURE [dbo].[PMGetReportsData_RealizedPNL_OLD]                                                
(                                                                                                                        
  @companyID int,                                                
  @startDate datetime,--in Historical All auec date remain same                                                                    
  @endDate datetime                                                                                                                                                                          
                                                                                                     
)                                                                                                                        
As                                                                                                                            
                                                                    
Begin                                                                                                            
                                                                                        
 declare @toAllAUECDatesString varchar(max)                                                
 declare @fromAllAUECDatesString varchar(max)                                                
 set @toAllAUECDatesString = dbo.GetAUECDateString(@endDate)                                                
 set @fromAllAUECDatesString = dbo.GetAUECDateString(@startDate)                                                
                                                 
 Declare @ToAUECDatesTable Table  
 (  
 AUECID int,  
 CurrentAUECDate DateTime  
 )                                                                                                        
                                                                                         
 Insert Into @ToAUECDatesTable   
   Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)     
                                                             
 Declare @FromAUECDatesTable Table  
 (  
   AUECID int,  
   CurrentAUECDate DateTime  
 )                                                                                                        
                                                                                         
 Insert Into @FromAUECDatesTable   
   Select * From dbo.GetAllAUECDatesFromString(@FromAllAUECDatesString)                               
                              
DECLARE @startingDate DateTime                              
Set @startingDate = DateAdd(yy, -20, @endDate)                                                          
 Create Table #TEMPGetConversionFactorForGivenDateRange                                         
 (                                                                                                                                  
  FCID int,                                        
  TCID int,                                        
  ConversionFactor float,                                                 
  ConversionMethod int,                                                                                        
  DateCC DateTime                                                                                                             
 )                                           
 INSERT INTO #TEMPGetConversionFactorForGivenDateRange                                                                                                                                  
 SELECT                                                                       
  FromCurrencyID,                   
  ToCurrencyID,                    
  RateValue,        
  ConversionMethod,                                                                           
  Date                                           
 FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startingDate, @endDate)                   
                                                                                                        
select                                                     
 PTC.PositionalTaxlotID,                                        
 PTC.ClosingTaxlotID,                                                    
 PT.Symbol as Symbol,                                               
 G.OrderSideTagValue as PositionSideID,                                                    
 G1.OrderSideTagValue as ClosingSideID,                                                    
 G.AUECLocalDate as PositionTradeDate,                                            
 --G1.CreationDate as ClosingTradeDate, --now closing taxlot Trade date is cloisng date                                                 
 PTC.AUECLocalDate as ClosingTradeDate, --now closing taxlot Trade date is cloisng date                                                 
 PT.AvgPrice as OpenPrice ,                                                    
 PT1.AvgPrice as ClosingPrice ,                                                    
 PT.FundID as FundID,                                                  
 PT.Level2ID as StrategyID,                                                  
 G.AssetID,                                                  
 G.UnderLyingID,                                                  
 G.ExchangeID,                                                  
 G.CurrencyID,                                                  
 PT.ClosedTotalCommissionandFees as PositionalTaxlotCommission,                                                    
 PT1.ClosedTotalCommissionandFees as ClosingTaxlotCommission,                                                    
 PTC.ClosingMode as ClosingMode,                     
 ISNULL(SM.Multiplier, 0) AS Multiplier,                                                  
 isnull(PT.PositionTag, 0) as OpeiningPositionTag,                                                    
 isnull(PT1.PositionTag, 0) as ClosingPositionTag,                                                    
 PTC.SettlementPrice ,                                                    
 PTC.ClosedQty  ,                                                    
 PT1.TaxLotOpenQty as GeneratedTaxlotQty,                                                    
 isnull(SW.NotionalValue,0) as PositionNotionalValue,                                                              
 isnull(SW.BenchMarkRate,0) as PositionBenchMarkRate,                                                              
 isnull(SW.Differential,0) as PositionDifferential,                                                              
 isnull(SW.OrigCostBasis,0) as PositionOrigCostBasis,                                                                    
 isnull(SW.DayCount,0) as PositionDayCount,                                                              
 SW.FirstResetDate as PositionFirstResetDate,                                                              
 SW.OrigTransDate as PositionOrigTransDate,                                                     
 isnull(SW1.NotionalValue,0) as NotionalValue,                                    
 isnull(SW1.BenchMarkRate,0) as BenchMarkRate,                                                              
 isnull(SW1.Differential,0) as Differential,                                                              
 isnull(SW1.OrigCostBasis,0) as OrigCostBasis,                                                                    
 isnull(SW1.DayCount,0) as DayCount,                                                              
 SW1.FirstResetDate as FirstResetDate,                                                              
 SW1.OrigTransDate as OrigTransDate ,                                                  
 isnull(G.IsSwapped, 0) AS IsOpeningSwapped,                                                
 isnull(G1.IsSwapped, 0) AS IsClosingSwapped,           
 FundName,                                                     
 CMF.MasterFundName,                                            
 TS.Side AS OpeningSide,                                      
 TS1.Side AS ClosingSide,                    
 CS.StrategyName AS StrategyName,                                                        
 ISNULL(UDA.TickerSymbol, 'Undefined') AS UDATickerSymbol,                    
 ISNULL(UDA.AssetName, 'Undefined') AS UDAAssetName,                                                 
 ISNULL(UDA.SecurityTypeName, 'Undefined') AS UDASecurityTypeName,                 
 ISNULL(UDA.SectorName, 'Undefined') AS UDASectorName,                                                        
 ISNULL(UDA.SubSectorName, 'Undefined') AS UDASubSectorName,                                                        
 ISNULL(UDA.CountryName, 'Undefined') AS UDACountryName,                                
 TP.ThirdPartyName AS PrimeBrokerName,                                
 A.AssetName AS AssetName,                                            
                              
 CASE Comp.BaseCurrencyID                                                 
  WHEN G.CurrencyID                                                       
   THEN 1                                                      
   ELSE   
    CASE ISNULL(G.FXRate, 0)                                    
     WHEN 0                                     
     THEN ISNULL(OTConvFactor.ConversionFactor, 0)                          
    ELSE G.FXRate  
    END                                     
 END AS OTConvFactor,  
  
 CASE ISNULL(G.FXRate, 0)                                    
   WHEN 0   
   THEN isnull(OTConvFactor.ConversionMethod, 0)                           
   ELSE                                    
   CASE ISNULL(G.FXConversionMethodOperator, 'M')                                    
     WHEN 'M'   
     THEN 0                                    
     ELSE 1                                    
   END                                  
 END AS OTConvMethod,  
                           
 CASE Comp.BaseCurrencyID                                                 
  WHEN SM.VsCurrencyID                                                       
  THEN 1                                                      
  ELSE                          
	   CASE
			WHEN G.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)                                     
			THEN G.FXRate                          
			ELSE ISNULL(FXOTConvFactor.ConversionFactor, 0)                                  
	   END                                     
 END AS OTFXConvFactor,  
  
 CASE ISNULL(G.FXRate, 0)                                    
  WHEN 0   
  THEN ISNULL(FXOTConvFactor.ConversionMethod, 0)                           
  ELSE                                    
		CASE                                     
			WHEN ISNULL(G.FXConversionMethodOperator, 'M')='M' And G.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)          
			THEN 0 
			WHEN ISNULL(G.FXConversionMethodOperator, 'D')='D' And G.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)          
			THEN 1                                     
		    ELSE ISNULL(FXOTConvFactor.ConversionMethod, 0)                                    
		END                                  
 END AS OTFXConvMethod,                          
                          
 CASE Comp.BaseCurrencyID                                                 
  WHEN G1.CurrencyID                                                       
  THEN 1                                                      
  ELSE                          
   CASE PTC.ClosingMode                    
    WHEN 1                    
    THEN ISNULL(CTConvFactor.ConversionFactor, 0)                          
    WHEN 3                    
    THEN ISNULL(CTConvFactor.ConversionFactor, 0)             
   ELSE                    
    CASE ISNULL(G1.FXRate, 0)                                    
     WHEN 0                                     
     THEN ISNULL(CTConvFactor.ConversionFactor, 0)      
     ELSE G1.FXRate                                    
    END                                     
   END                    
 END AS CTConvFactor,      
                
 CASE PTC.ClosingMode                    
  WHEN 1                    
  THEN isnull(CTConvFactor.ConversionMethod, 0)                 
  WHEN 3                    
  THEN isnull(CTConvFactor.ConversionMethod, 0)                          
  ELSE                          
   CASE ISNULL(G1.FXRate, 0)                                    
    WHEN 0   
    THEN isnull(CTConvFactor.ConversionMethod, 0)                           
   ELSE                                    
    CASE ISNULL(G1.FXConversionMethodOperator, 'M')                           
     WHEN 'M'  
        THEN  0  
     ELSE 1                                    
    END                      
   END                          
 END AS CTConvMethod,                          
                          
 CASE Comp.BaseCurrencyID                                                 
  WHEN SM.VsCurrencyID                                                       
  THEN 1                    
  ELSE                    
	   CASE PTC.ClosingMode                    
		WHEN 1                    
		THEN ISNULL(FXCTConvFactor.ConversionFactor, 0)                          
		WHEN 3                    
		THEN ISNULL(FXCTConvFactor.ConversionFactor, 0)                          
	   ELSE                                                      
			CASE 
				 WHEN G1.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)                                                   
				 THEN G1.FXRate                                                               
				 ELSE ISNULL(FXCTConvFactor.ConversionFactor, 0)
			END                        
	   END                                 
 END AS CTFXConvFactor,                    
  
 CASE PTC.ClosingMode                    
   WHEN 1                    
   THEN isnull(FXCTConvFactor.ConversionMethod, 0)                    
   WHEN 3                    
   THEN isnull(FXCTConvFactor.ConversionMethod, 0)                          
   ELSE                          
     CASE ISNULL(G1.FXRate, 0)                                    
		WHEN 0   
		THEN isnull(FXCTConvFactor.ConversionMethod, 0)                           
		ELSE                                    
			CASE                                     
				 WHEN ISNULL(G1.FXConversionMethodOperator, 'M')='M' And G1.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)          
				 THEN 0 
				 WHEN ISNULL(G1.FXConversionMethodOperator, 'D')='D' And G1.FXRate > 0 And (Comp.BaseCurrencyID = SM.VsCurrencyID OR Comp.BaseCurrencyID = SM.LeadCurrencyID)          
				 THEN 1                                       
				 ELSE ISNULL(FXCTConvFactor.ConversionMethod, 0)                                    
			END                       
		 END                               
 END AS CTFXConvMethod,                          
  
 ISNULL(SM.CompanyName, '') AS CompanyName,                      
 ISNULL(SM.PutOrCall, ' ') AS PutOrCall,              
              
 CASE dbo.GetSideMultiplierForClosing(PTC.ClosingMode, G.OrderSideTagValue, G1.OrderSideTagValue)                                                                        
  WHEN 1                                                                          
  THEN 'Long'               
  WHEN -1               
  THEN 'Short'                                          
  ELSE ''              
 END AS PositionSide              
                
 FROM PM_TaxlotClosing  PTC                              
  INNER JOIN T_Company Comp                                         
  ON Comp.CompanyID = @companyID                                                      
  Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                             
  Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                    
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                      
  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID      
  LEFT JOIN V_SecMasterData SM ON SM.TickerSymbol = PT.Symbol                                                     
  --Inner Join T_AUEC AUEC on AUEC.AUECID=G.AUECID --Now AUECID comes in V_SecMasterData View                                                      
  Inner Join @ToAUECDatesTable ToAUECDatesTable on ToAUECDatesTable.AUECID=SM.AUECID                                                      
  Inner Join @FromAUECDatesTable FromAUECDatesTable on FromAUECDatesTable.AUECID=SM.AUECID                     
  INNER JOIN T_Asset A ON G.AssetID = A.AssetID -- AssetID join was with AUEC.AssetID, but AUEC join has no more, so Now with G.AssetID                                       
  LEFT JOIN T_CompanyStrategy CS                                       
     ON PT.Level2ID = CS.CompanyStrategyID                                  
  LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD                                       
     ON PT.FundID = CTPMD.InternalFundNameID_FK                                
  LEFT JOIN T_CompanyThirdParty CTP                                 
   ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                
  LEFT JOIN T_ThirdParty TP                                 
   ON CTP.ThirdPartyID = TP.ThirdPartyID                                      
  left join V_GetSymbolUDAData as UDA on PT.Symbol=UDA.TickerSymbol                                              
  Left Outer Join  T_SwapParameters SW on SW.GroupID=G.GroupID                                                      
  Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                
     LEFT OUTER JOIN T_CompanyFunds CF ON PT.FundID = CF.CompanyFundID                                                              
     LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                  
     LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                               
     LEFT JOIN T_Side TS on TS.SidetagValue = G.OrderSideTagValue                                            
  LEFT JOIN T_Side TS1 on TS1.SidetagValue = G1.OrderSideTagValue                                        
                                
  LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange OTConvFactor                                         
    ON G.CurrencyID  = OTConvFactor.FCID   
          AND Comp.BaseCurrencyID = OTConvFactor.TCID                              
    AND DATEDIFF(d,OTConvFactor.DateCC, G.AllocationDate) = 0    
  -- the 4 lines given below have been commented                             
  --LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXOTConvFactor                                         
  --   ON (FXOTConvFactor.FCID = SM.LeadCurrencyID                  
  --    And FXOTConvFactor.TCID = SM.VsCurrencyID)                                         
  --    AND DATEDIFF(d,FXOTConvFactor.DateCC,G.AUECLocalDate) = 0    
  --updated 4 new lines                             
  LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXOTConvFactor                                         
     ON (FXOTConvFactor.FCID = SM.VsCurrencyID                  
   And FXOTConvFactor.TCID = Comp.BaseCurrencyID)                                         
   AND DATEDIFF(d,FXOTConvFactor.DateCC,G.AUECLocalDate) = 0                              
                                
  LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange CTConvFactor                                         
     ON G1.CurrencyID  = CTConvFactor.FCID                                         
   AND Comp.BaseCurrencyID = CTConvFactor.TCID                              
   AND DATEDIFF(d,CTConvFactor.DateCC, PTC.AUECLocalDate) = 0    
  -- the 4 lines given below have been commented                            
  --LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXCTConvFactor                                         
  --   ON (FXCTConvFactor.FCID = SM.LeadCurrencyID           
  --    And FXCTConvFactor.TCID = SM.VsCurrencyID)                                         
  --    AND DATEDIFF(d,FXCTConvFactor.DateCC, PTC.AUECLocalDate) = 0                         
  --updated 4 new lines   
  LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXCTConvFactor                                         
     ON (FXCTConvFactor.FCID = SM.VsCurrencyID                                         
   And FXCTConvFactor.TCID = Comp.BaseCurrencyID)                                         
   AND DATEDIFF(d,FXCTConvFactor.DateCC, PTC.AUECLocalDate) = 0     
                           
  --LEFT OUTER JOIN T_FutureMultipliers FM ON SUBSTRING(PT.Symbol, 0, CHARINDEX(' ', PT.Symbol)) = FM.Symbol               
                                                  
  where                         
   DateDiff(d,FromAUECDatesTable.CurrentAUECDate,PTC.AUECLocalDate) >=0                                           
   and  DateDiff(d,PTC.AUECLocalDate,ToAUECDatesTable.CurrentAUECDate)>=0                                  
   and  PTC.ClosingMode<>7                           
--   dbo.GetFormattedDatePart(FromAUECDatesTable.CurrentAUECDate)<= dbo.GetFormattedDatePart(PTC.AUECLocalDate)                                           
--   and  dbo.GetFormattedDatePart(PTC.AUECLocalDate)<=dbo.GetFormattedDatePart(ToAUECDatesTable.CurrentAUECDate)                                  
--   and  PTC.ClosingMode<>7   
                                                                                        
End                                                                  
                                
  DROP TABLE #TEMPGetConversionFactorForGivenDateRange                              
  --select * from T_SwapParameters                                                  
  --select * from PM_taxlotClosinga_RealizedPNL
