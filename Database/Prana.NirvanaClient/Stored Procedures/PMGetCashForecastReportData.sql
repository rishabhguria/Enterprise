CREATE PROCEDURE [dbo].[PMGetCashForecastReportData] (                                                                                
 @companyID int,                                                                                
 @userID int,                                      
 @date datetime,                                                                                
 @AllAUECDatesString VARCHAR(MAX),                                    
 @ErrorMessage varchar(500) output,                                                                                
 @ErrorNumber int output,            
 @CurrencySymbol varchar(5)                                                                                
)                                                                                
AS                 
/*            
 Declare @Buys numeric(18,4)            
 Declare @Sells numeric(18,4)            
 Declare @ShortSales numeric(18,4)            
 Declare @Covers numeric(18,4)            
 Declare @SettlementDate DateTime            
 Declare @Side varchar(20)        
 Declare @FundName varchar(50)            
 Declare @CurrencyName varchar(50)            
 Declare @CurrencySymbol1 varchar(5)            
 Declare @ProjectedBalanceFinal numeric(18, 4)            
            
 Select            
 @Buys AS Buys            
 ,@Sells AS Sells            
 ,@ShortSales AS ShortSales            
 ,@Covers AS Covers             
 ,@SettlementDate AS SettlementDate              
 ,@Side AS Side            
 ,@FundName AS FundName            
 ,@CurrencyName AS CurrencyName            
 ,@CurrencySymbol1 AS CurrencySymbol            
 ,@ProjectedBalanceFinal AS ProjectedBalanceFinal */            
            
                                                                           
                                                                                
SET @ErrorMessage = 'Success'                                                                                
SET @ErrorNumber = 0             
            
BEGIN TRY            
            
 declare @StartingSettlementDate datetime            
 set @StartingSettlementDate = @date            
 declare @EndingSettlementDate datetime            
 set @EndingSettlementDate = dateadd([day], 3, @StartingSettlementDate)            
            
 DECLARE @auecID int            
    set @auecID=0            
            
            
 DECLARE @AUECSeparator varchar(1)              
 DECLARE @DateSeparator varchar(1)            
 declare @AllAUECDatesStringFinal VARCHAR(MAX)              
              
 SET @AUECSeparator = '~'              
 SET @DateSeparator = '^'            
 Set @AllAUECDatesStringFinal = ' '              
            
--Cursor used to generate the AUEC Date string.            
--DECLARE AUECString_Cursor CURSOR FAST_FORWARD FOR                                                                
--Select                                 
-- AUECID            
-- FROM T_AUEC            
--            
--   Open AUECString_Cursor            
--            
--FETCH NEXT FROM AUECString_Cursor INTO            
--@auecID;            
--             
--WHILE @@fetch_status = 0                                                                
--  BEGIN             
--            
-- Set @AllAUECDatesStringFinal = @AllAUECDatesStringFinal + CONVERT(varchar, @auecID) + @DateSeparator + CONVERT(varchar, @date, 101) + @AUECSeparator            
--            
-- FETCH NEXT FROM AUECString_Cursor INTO             
-- @auecID ;                                                            
--  END               
            
            
             
--CLOSE AUECString_Cursor            
--DEALLOCATE AUECString_Cursor            
            
declare @sellSideMultiplier int            
set @sellSideMultiplier = -1            
declare @futureAssetMultiplier float            
set @futureAssetMultiplier = 0            
            
            
declare @boughtCurrencySymbol varchar(6)            
declare @soldCurrencySymbol varchar(6)            
set @boughtCurrencySymbol = @currencySymbol+'%'             
set @soldCurrencySymbol = '%'+@currencySymbol             
--select @boughtCurrencySymbol AS boughtCurrencySymbol            
--select @soldCurrencySymbol AS soldCurrencySymbol            
--Temp table to hold the positions till date.            
CREATE TABLE #TempFundPositionsForDate            
(            
 TaxlotID varchar(50),            
 TaxlotQty int,            
 FundName varchar(50),            
 AvgPrice float,            
 Symbol varchar(30),            
 OrderSideTagValue varchar(2),            
 AUECID int,            
 AUECLocalDate DateTime,            
 --OrderSideName varchar(20),            
 SettlementDate DateTime,            
 Multiplier float,            
 Commission float,            
 Fees float,          
 OtherFees float,            
 AssetID int,            
 UnderLyingID int,   
 CurrencyName varchar(50),            
 CurrencySymbol varchar(5)            
)            
INSERT INTO #TempFundPositionsForDate            
SELECT             
 TaxlotID,            
 TaxlotQty,            
 FundName,            
 AvgPrice,            
 Symbol,            
 CASE OrderSideTagValue            
  WHEN 'B' THEN            
   'B'       
  WHEN 'A' THEN            
   '1'            
  WHEN 'D' THEN            
   '2'            
  WHEN 'C' THEN            
   '5'            
  ELSE            
   OrderSideTagValue            
 END            
  AS OrderSideTagValue,            
 VT.AUECID,            
 AUECLocalDate,            
 --'a'--OrderSideName,            
 SettlementDate,            
 AUEC.Multiplier,            
 ISNULL(Commission, 0),            
 ISNULL(OtherBrokerFees, 0),          
 ISNULL(ISNULL(StampDuty, 0) + ISNULL(ClearingFee, 0) + ISNULL(TransactionLevy, 0) + ISNULL(TaxOnCommissions, 0) + ISNULL(MiscFees, 0), 0),          
 VT.AssetID,            
 VT.UnderLyingID,            
 CurrencyName,            
 CurrencySymbol            
FROM V_Taxlots VT INNER JOIN T_AUEC AUEC ON AUEC.AUECID = VT.AUECID INNER JOIN T_CompanyFunds CF ON            
 VT.FundID = CF.CompanyFundID INNER JOIN T_Currency C ON VT.CurrencyID = C.CurrencyID            
 AND (C.CurrencySymbol = @CurrencySymbol OR Symbol LIKE @boughtCurrencySymbol OR Symbol LIKE @soldCurrencySymbol)            
 WHERE VT.AssetID <> 3 --Leaving Future trades out as per requirement.            
  AND VT.AssetID <> 4 --Leaving Future options trades out as per requirement.            
            
            
 --declare @auecID int            
-- set @auecID = (Select min(auecID) FROM T_AUEC AUEC INNER JOIN T_Currency C ON AUEC.BaseCurrencyID = C.CurrencyID            
--     WHERE C.CurrencySymbol = @CurrencySymbol)            
            
-- declare @assetName varchar(20)            
-- declare @underLyingName varchar(20)            
-- declare @exchangeName varchar(20)            
-- declare @auecName varchar(50)            
-- set @assetName = (Select AssetName FROM T_AUEC AUEC INNER JOIN T_Asset A ON AUEC.AUECID = A.AssetID WHERE AUECID = @auecID)            
-- set @underLyingName = (Select UnderLyingName FROM T_AUEC AUEC INNER JOIN T_UnderLying U ON AUEC.AUECID = U.UnderLyingID WHERE AUECID = @auecID)            
-- set @exchangeName = (Select E.DisplayName FROM T_AUEC AUEC INNER JOIN T_Exchange E ON AUEC.AUECID = E.ExchangeID WHERE AUECID = @auecID)            
             
-- select @assetName            
-- select @underLyingName            
-- select @exchangeName            
-- select @CurrencySymbol            
-- set @auecName = @assetName + '\' + @underLyingName + '\' + @exchangeName + '\' + @CurrencySymbol            
 --select @auecName            
            
 --select @auecID            
            
 --Table to hold the next three settlement dates along with the current date.            
 CREATE TABLE [dbo].#TempSettlementDateTable            
    (            
  SettlementDate datetime            
    )            
-- INSERT INTO [dbo].#TempSettlementDateTable            
--      (            
--       SettlementDate              
--      )            
--      select * from dbo.GetDateRange(@StartingSettlementDate, @EndingSettlementDate)            
 INSERT INTO [dbo].#TempSettlementDateTable            
      (            
       SettlementDate              
      )            
      select * from dbo.GetNextAskedDaysWithoutExchangeANDBusinessHolidays(@StartingSettlementDate, 4)            
--      select * from dbo.GetNextAskedBusinessDaysWithoutExchangeHolidays(@StartingSettlementDate, 4)            
            
            
 CREATE TABLE [dbo].#TempCashTable                                    
    (                                    
  StartingBalance numeric(18,4),                                    
  Buys numeric(18,4),                                
  Sells numeric(18,4),                                
  ShortSales numeric(18,4),                                
  Covers numeric(18,4),              SettlementDate datetime,            
  --Side varchar(20),            
  ProjectedBalance numeric(18,4),            
  FundName varchar(50),            
  CurrencyName varchar(50),            
  CurrencySymbol varchar(5)            
    )            
            
 INSERT INTO [dbo].#TempCashTable            
      (            
       Buys,            
       Sells,            
       ShortSales,            
       Covers            
       ,SettlementDate            
       --,Side            
       ,FundName            
       ,CurrencyName            
       ,CurrencySymbol              
      )            
 SELECT             
  CASE min(AssetID)            
   WHEN '5' THEN            
    CASE TFPDT.OrderSideTagValue            
     WHEN '1' THEN            
      CASE @currencySymbol            
       WHEN min(substring(Symbol, 1, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum((TaxlotQty * Multiplier) + (Commission + Fees + OtherFees))            
         WHEN '2' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '5' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN 'B' THEN            
          sum(((TaxlotQty * Multiplier) + (Commission + Fees + OtherFees)))            
         ELSE            
          0            
        END            
       WHEN min(substring(Symbol, 5, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)) * @sellSideMultiplier)            
         WHEN '2' THEN            
          sum(((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN '5' THEN            
          sum(((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN 'B' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         ELSE            
          0            
        END            
       ELSE            
        0            
      END            
     ELSE            
      0            
    END           
   ELSE            
    CASE TFPDT.OrderSideTagValue            
     WHEN '1' THEN             
      sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)) * @sellSideMultiplier)             
     ELSE            
      0            
    END            
  END ,             
              
  CASE min(AssetID)            
   WHEN '5' THEN            
    CASE TFPDT.OrderSideTagValue            
     WHEN '2' THEN            
      CASE @currencySymbol            
       WHEN min(substring(Symbol, 1, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)))            
         WHEN '2' THEN            
          sum((((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '5' THEN            
          sum((((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN 'B' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)))            
         ELSE            
          0            
        END            
       WHEN min(substring(Symbol, 5, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '2' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN '5' THEN     
          sum(((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN 'B' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         ELSE            
          0            
        END            
       ELSE            
        0            
      END            
     ELSE            
      0            
    END            
   ELSE            
    CASE TFPDT.OrderSideTagValue            
     WHEN '2' THEN             
      sum((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))             
     ELSE            
      0            
    END            
  END ,         
            
            
            
  CASE min(AssetID)            
   WHEN '5' THEN            
    CASE TFPDT.OrderSideTagValue            
     WHEN '5' THEN            
      CASE @currencySymbol            
       WHEN min(substring(Symbol, 1, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)))            
         WHEN '2' THEN            
          sum((((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '5' THEN            
          sum((((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN 'B' THEN         
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)))            
         ELSE            
          0            
        END            
       WHEN min(substring(Symbol, 5, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '2' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN '5' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN 'B' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         ELSE            
          0            
        END            
       ELSE            
        0            
      END            
     ELSE            
      0            
    END            
   ELSE            
    CASE TFPDT.OrderSideTagValue            
     WHEN '5' THEN             
      sum((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))             
     ELSE            
      0            
    END            
  END ,            
            
            
            
  CASE min(AssetID)            
   WHEN '5' THEN            
    CASE TFPDT.OrderSideTagValue            
     WHEN 'B' THEN            
      CASE @currencySymbol            
       WHEN min(substring(Symbol, 1, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum((TaxlotQty * Multiplier) + (Commission + Fees + OtherFees))            
         WHEN '2' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN '5' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) - (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         WHEN 'B' THEN            
          sum(((TaxlotQty * Multiplier) + (Commission + Fees + OtherFees)))            
         ELSE            
          0            
        END            
       WHEN min(substring(Symbol, 5, 3)) THEN            
        CASE TFPDT.OrderSideTagValue            
         WHEN '1' THEN            
          sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)) * @sellSideMultiplier)            
         WHEN '2' THEN            
          sum(((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN '5' THEN            
          sum(((TaxlotQty * Multiplier) - (Commission + Fees + OtherFees)))            
         WHEN 'B' THEN            
          sum((((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees))) * @sellSideMultiplier)            
         ELSE            
          0            
        END            
       ELSE            
        0            
      END            
     ELSE            
      0            
    END            
   ELSE            
    CASE TFPDT.OrderSideTagValue            
     WHEN 'B' THEN             
      sum(((TaxlotQty * AvgPrice * Multiplier) + (Commission + Fees + OtherFees)) * @sellSideMultiplier)             
     ELSE            
      0            
    END            
  END ,            
              
  TSDT.SettlementDate           
  --min(TFPDT.OrderSideName)            
  ,min(FundName)            
  ,min(CurrencyName)            
  ,min(CurrencySymbol)            
            
  FROM #TempSettlementDateTable TSDT INNER JOIN             
  #TempFundPositionsForDate TFPDT ON            
  datediff(d,TSDT.SettlementDate,TFPDT.SettlementDate) = 0    
  WHERE /*TFPDT.AssetID <> 3 --Leaving Future trades out as per requirement.            
  AND TFPDT.AssetID <> 4 --Leaving Future option trades out as per requirement. */            
  TFPDT.UnderLyingID <> 5 --Leaving Brazilian trades out as per requirement.            
  GROUP BY TFPDT.OrderSideTagValue, TSDT.SettlementDate, Symbol            
            
 --Select * from #TempCashTable            
            
 --Select * from #TempSettlementDateTable            
 --select * from dbo.GetDateRange(@StartingSettlementDate, @EndingSettlementDate)            
            
             
 --As now no business day and exchange day is taken into conisderation to get the next day. So the following            
 --lines are commented.            
-- --To get the date passed as the business day and assign it to @date.            
-- declare @tempDate datetime            
-- set @tempDate = dateadd([day], -1, @StartingSettlementDate)              
-- set @date = (Select [dbo].AdjustBusinessDaysWithoutExchangeHolidays(@tempDate, 1))             
            
         
 --set @date = dateadd([day], 1, @StartingSettlementDate)            
            
            
        
UPDATE [dbo].#TempCashTable            
                  
  Set StartingBalance = (select Sum(CashValueLocal) FROM T_DayEndBalances where BalanceType=1 AND             
       datediff(d,Date,@date) = 0 AND             
       LocalCurrencyID = (Select CurrencyID FROM T_Currency WHERE CurrencySymbol = @CurrencySymbol)            
       group by Date)             
       Where datediff(d,#TempCashTable.SettlementDate,@date) = 0    
            
            
 DECLARE @startBalance numeric(18,4)            
 Set @startBalance = (select Sum(CashValueLocal) FROM T_DayEndBalances where BalanceType=1 AND             
       datediff(d,Date,@date) = 0 AND             
       LocalCurrencyID = (Select CurrencyID FROM T_Currency WHERE CurrencySymbol = @CurrencySymbol)            
       group by Date )            
            
             
-- SELECT @startBalance            
                   
                  
             
            
            
  CREATE TABLE [dbo].#TempCashTableFinal                                    
  (                                    
   StartingBalance numeric(18,4),                                    
   Buys numeric(18,4),                                
   Sells numeric(18,4),                                
   ShortSales numeric(18,4),                                
   Covers numeric(18,4),            
   SettlementDate datetime,            
   --Side varchar(20),            
   ProjectedBalanceFinal numeric(18,4),            
   FundName varchar(50),            
   CurrencyName varchar(50),            
   CurrencySymbol varchar(5),            
   AUECName varchar(50)            
  )            
          
  INSERT INTO [dbo].#TempCashTableFinal            
  (            
   Buys,            
   Sells,            
   ShortSales,            
   Covers            
   ,SettlementDate            
   --,Side            
   ,FundName            
   ,CurrencyName            
   ,CurrencySymbol            
   ,StartingBalance              
   ,ProjectedBalanceFinal            
   ,AUECName            
  )            
  SELECT              
   ISNULL(sum(Buys), 0) AS Buys,            
   ISNULL(sum(Sells), 0) AS Sells,            
   ISNULL(sum(ShortSales), 0) AS ShortSales,            
   ISNULL(sum(Covers), 0) AS Covers            
   ,CONVERT(VARCHAR(10), TSDT.SettlementDate, 101) AS SettlementDate              
   --,min(Side) AS Side            
   ,min(FundName) AS FundName            
   ,min(CurrencyName) AS CurrencyName            
   ,min(CurrencySymbol) AS CurrencySymbol            
   ,ISNULL(min(StartingBalance), 0) AS StartingBalance            
   ,CASE TSDT.SettlementDate            
    WHEN @date Then            
     ISNULL(min(ISNULL(StartingBalance, 0)) + sum(ISNULL(Buys, 0)) + sum(ISNULL(Sells, 0)) + sum(ISNULL(ShortSales, 0)) + sum(ISNULL(Covers, 0)), 0)            
    ELSE            
     ISNULL(sum(ISNULL(Buys, 0)) + sum(ISNULL(Sells, 0)) + sum(ISNULL(ShortSales, 0)) + sum(ISNULL(Covers, 0)), 0)            
    END AS ProjectedBalanceFinal            
   ,'' AS AUECName--@auecName            
   --,min(ISNULL(ProjectedBalance, 0)) AS ProjectedBalanceFinal            
  FROM #TempCashTable TCT            
  RIGHT OUTER JOIN             
  #TempSettlementDateTable TSDT ON            
  datediff(d,TCT.SettlementDate,TSDT.SettlementDate) = 0            
  group by TSDT.SettlementDate            
              
            
  DECLARE @startBalanceTemp numeric(18,4)             
  Set @startBalanceTemp = (Select StartingBalance FROM [dbo].#TempCashTableFinal WHERE             
    datediff(d,SettlementDate,@date) = 0)            
  If(@startBalanceTemp <= 0)            
  begin            
   UPDATE [dbo].#TempCashTableFinal            
   Set StartingBalance = ISNULL(@startBalance, 0) WHERE             
   datediff(d,[dbo].#TempCashTableFinal.SettlementDate,@date) = 0            
            
   UPDATE [dbo].#TempCashTableFinal            
   Set ProjectedBalanceFinal = (ISNULL(@startBalance, 0) + ProjectedBalanceFinal) WHERE             
   datediff(d,[dbo].#TempCashTableFinal.SettlementDate,@date) = 0            
  end            
            
            
            
   UPDATE [dbo].#TempCashTableFinal            
   Set StartingBalance = (Select ProjectedBalanceFinal FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,@date) = 0)            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 1, @date))) = 0            
            
   UPDATE [dbo].#TempCashTableFinal            
   Set ProjectedBalanceFinal = (Select ProjectedBalanceFinal + StartingBalance FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,(Select dateadd([day], 1, @date))) = 0  )            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 1, @date))) = 0              
            
   UPDATE [dbo].#TempCashTableFinal            
   Set StartingBalance = (Select ProjectedBalanceFinal FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,(Select dateadd([day], 1, @date))) = 0  )            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 1, @date))) = 0              
            
   UPDATE [dbo].#TempCashTableFinal            
   Set ProjectedBalanceFinal = (Select ProjectedBalanceFinal + StartingBalance FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,(Select dateadd([day], 2, @date))) = 0  )            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 2, @date))) = 0              
            
            
   UPDATE [dbo].#TempCashTableFinal            
   Set StartingBalance = (Select ProjectedBalanceFinal FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,(Select dateadd([day], 2, @date))) = 0  )            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 3, @date))) = 0              
            
   UPDATE [dbo].#TempCashTableFinal            
   Set ProjectedBalanceFinal = (Select ProjectedBalanceFinal + StartingBalance FROM #TempCashTableFinal Where             
    datediff(d,SettlementDate,(Select dateadd([day], 3, @date))) = 0  )            
    WHERE datediff(d,SettlementDate,(Select dateadd([day], 3, @date))) = 0              
                      
            
            
   SELECT              
   Buys,            
   Sells,            
   ShortSales,            
   Covers,            
   CONVERT(VARCHAR(10), /*TSDT.*/SettlementDate, 101) AS SettlementDate,              
   --Side,            
   FundName,            
   CurrencyName,            
   CurrencySymbol,            
   StartingBalance,            
   ProjectedBalanceFinal,            
   AUECName            
   --ISNULL(StartingBalance, 0) + Buys + Sells + ShortSales + Covers            
  FROM #TempCashTableFinal       
    
 DROP TABLE #TempFundPositionsForDate            
 DROP TABLE #TempSettlementDateTable            
 DROP TABLE #TempCashTable            
 DROP TABLE #TempCashTableFinal            
             
            
END TRY                                                                      
BEGIN CATCH              
 --CLOSE AUECString_Cursor            
 --DEALLOCATE AUECString_Cursor                                                                               
                                                  
 SET @ErrorMessage = ERROR_MESSAGE();                                                                                
 SET @ErrorNumber = Error_number();                                                                  
 Print @ErrorMessage            
 Print @ErrorNumber            
END CATCH; 

        
        
/****************************************************************************                              
Name :   [PMGetDailyCash]          
Purpose:  Returns all the Cash Values currency and date wise.          
Module: DailyCash/PM          
Author: Bhupesh Bareja    
Parameters:                               
@Date datetime      
@ErrorMessage varchar(500)                               
  , @ErrorNumber int                                
Execution StateMent:                               
   EXEC [PMGetDailyCash] '10-07-2008', ' ', 0          
                              
Date Modified:                               
Description:                                 
Modified By:                                 
****************************************************************************/              
