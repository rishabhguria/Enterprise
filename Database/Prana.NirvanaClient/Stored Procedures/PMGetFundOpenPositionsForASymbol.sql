
/*                                        
Created By: Sandeep Singh, Date: 22-May-2013                    
Desc: used on Closing to fetch data Symbol and fund wise                      
                    
EXEC PMGetFundOpenPositionsForASymbol '0^5/22/2013 12:00:00 AM~1^5/22/2013 6:32:41 AM~12^5/22/2013 6:32:41 AM~15^5/22/2013 6:32:41 AM~16^5/22/2013 6:32:41 AM~20^5/22/2013 6:32:41 AM~59^5/22/2013 6:32:41 AM~',                    
'',                    
'X',                  
'2'                    
    
Modified By: Sumit  
Date: Dec 18, 2013  
Desc: The better way to stop caching execution plan,   
when you know time to create execution plan < time to execute; is by adding "With compile" to the procedure.    
parametersniffing is commented                              
*/
CREATE PROCEDURE [dbo].[PMGetFundOpenPositionsForASymbol] 
(
	@ToAllAUECDatesString VARCHAR(MAX)
	,@FundIds VARCHAR(MAX)
	,@Symbol VARCHAR(100)
	,@SideSideTagValue VARCHAR(10)
	)
-- WITH RECOMPILE              
AS
  
--Declare @ToAllAUECDatesString VARCHAR(MAX)        
--Declare @FundIds VARCHAR(MAX)        
--Declare @Symbol VARCHAR(100)        
--Declare @SideSideTagValue VARCHAR(10)   
--  
--Set @ToAllAUECDatesString = N'0^7/15/2016 12:00:00 AM~15^7/15/2016 3:07:27 AM~63^7/15/2016 5:07:27 PM~43^7/15/2016 8:07:27 AM~81^7/15/2016 3:07:27 AM~18^7/15/2016 3:07:27 AM~61^7/15/2016 3:07:27 AM~1^7/15/2016 3:07:27 AM~73^7/15/2016 3:07:27 AM~62^7/15/
  
    
      
--2016 3:07:27 AM~59^7/15/2016 9:07:27 AM~77^7/15/2016 3:07:27 PM~11^7/15/2016 3:07:27 AM~80^7/15/2016 3:07:27 AM~74^7/15/2016 3:07:27 AM~'          
--Set @FundIds = N'6'     
--Set @Symbol =  N'CLCD'  
--Set @SideSideTagValue = N'B'  
  
BEGIN
	--http://www.sqlusa.com/bestpractices/training/scripts/parametersniffing/    
	-- Declare @Local_ToAllAUECDatesString VARCHAR(MAX)                                                           
	-- Declare @Local_FundIds VARCHAR(MAX)                 
	-- Declare @Local_Symbol Varchar(100)                
	-- Declare @Local_SideSideTagValue Varchar(10)   
	-- set @Local_ToAllAUECDatesString=@ToAllAUECDatesString                                                        
	-- set @Local_FundIds=@FundIds                   
	-- set @Local_Symbol=@Symbol              
	-- set @Local_SideSideTagValue=@SideSideTagValue  
	-- SET NOCOUNT ON added to prevent extra result sets from                                          
	-- interfering with SELECT statements.                                                           
	SET NOCOUNT ON;

	DECLARE @AUECDatesTable TABLE (
		AUECID INT
		,CurrentAUECDate DATETIME
		)

	INSERT INTO @AUECDatesTable
	SELECT *
	FROM dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)

	-- Create Table #AssetClass (AssetID int)              
	-- if (@AssetIds is NULL or @AssetIds = '')                                                              
	-- Insert into #AssetClass                                                              
	-- Select AssetID from T_Asset                                                              
	-- else                                                               
	-- Insert into #AssetClass                                                              
	-- Select Items as AssetID from dbo.Split(@AssetIds,',')                                                              
	CREATE TABLE #Funds (FundID INT)
	IF (
			@FundIds IS NULL
			OR @FundIds = ''
			)
		INSERT INTO #Funds
		SELECT CompanyFundID AS FundID
		FROM T_CompanyFunds Where IsActive=1
	ELSE
		INSERT INTO #Funds
		SELECT Items AS FundID
		FROM dbo.Split(@FundIds, ',')

	CREATE TABLE #Side (OrderSideTagValue VARCHAR(10))

	IF (
			@SideSideTagValue = '2'
			OR @SideSideTagValue = 'D'
			)
	BEGIN
		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('1')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('A')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('B')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES (@SideSideTagValue) --To get open Positions for sell trades                 
	END
	ELSE IF (@SideSideTagValue = 'B')
	BEGIN
		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('5')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('C')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('2')
		INSERT INTO #Side (OrderSideTagValue)
		VALUES ('D')

		INSERT INTO #Side (OrderSideTagValue)
		VALUES (@SideSideTagValue) --To get open Positions for buy to close trades                   
	END

	-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                    
	SELECT *
	INTO #SecMasterTemp
	FROM V_SecMasterData
	WHERE TickerSymbol = @Symbol

SELECT UDA.*        
 INTO #Temp_UDA        
 FROM V_UDA_DynamicUDA UDA  
Inner Join #SecMasterTemp SM ON SM.Symbol_PK = UDA.Symbol_PK         
  
  
SELECT max(Taxlot_PK) As Taxlot_PK     
InTo #TempTaxlotPK     
   FROM PM_Taxlots        
   INNER JOIN #Funds On #Funds.FundID =  PM_Taxlots.FundId             
   INNER JOIN T_Group G ON G.GroupID = PM_Taxlots.GroupID        
   --  inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                   
   INNER JOIN @AUECDatesTable AUECDates ON G.AUECID = AUECDates.AUECID        
   WHERE Datediff(d, PM_Taxlots.AUECModifiedDate, AUECDates.CurrentAUECDate) >= 0        
 And PM_Taxlots.Symbol = @Symbol             
   GROUP BY taxlotid           
        
	SELECT PT.TaxLotID AS TaxLotID
		,G.AUECLocalDate AS AUECLocalDate
		,PT.OrderSideTagValue AS SideID
		,-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                              
		PT.Symbol AS Symbol
		,PT.TaxLotOpenQty AS OpenQuantity
		,PT.AvgPrice AS AvgPX
		,PT.FundID AS FundID
		,G.AssetID AS AssetID
		,G.UnderLyingID AS UnderLyingID
		,G.ExchangeID AS ExchangeID
		,G.CurrencyID AS CurrencyID
		,G.AUECID AS AUECID
		,PT.OpenTotalCommissionandFees AS TotalCommissionandFees
		,--this is open commission and closed commission sum is not necessarily equals to total commission                                                                  
		isnull(SM.Multiplier, 1) AS Multiplier
		,G.SettlementDate AS SettlementDate
		,SM.LeadCurrencyID
		,SM.VsCurrencyID
		,isnull(SM.ExpirationDate, '1/1/1800') AS ExpirationDate
		,G.Description AS Description
		,PT.Level2ID AS Level2ID
		,isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty), 0) AS NotionalValue
		,isnull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW.Differential, 0) AS Differential
		,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW.DayCount, 0) AS DayCount
		,isnull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,G.IsSwapped AS IsSwapped
		,G.AllocationDate AS AUECLocalDate
		,G.GroupID
		,PT.PositionTag
		,IsNull(PT.FXRate, G.FXRate) AS FXRate
		,IsNull(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) AS FXConversionMethodOperator
		,isnull(SM.CompanyName, '') AS CompanyName
		,isnull(SM.UnderlyingSymbol, '') AS UnderlyingSymbol
		,IsNull(SM.Delta, 1) AS Delta
		,IsNull(SM.PutOrCall, '') AS PutOrCall
		,G.IsPreAllocated AS IsGrPreAllocated
		,G.CumQty AS GrCumQty
		,G.AllocatedQty AS GrAllocatedQty
		,G.Quantity AS GrQuantity
		,PT.taxlot_Pk AS Taxlot_Pk
		,PT.ParentRow_Pk AS ParentRow_Pk
		,IsNull(SM.StrikePrice, 0) AS StrikePrice
		,G.UserID AS UserID
		,G.CounterPartyID
		,CATaxlots.CorpActionID AS CorpActionID
		,SM.Coupon
		,SM.IssueDate AS IssueDate
		,SM.MaturityDate AS MaturityDate
		,SM.FirstCouponDate AS FirstCouponDate
		,SM.CouponFrequencyID AS CouponFrequencyID
		,SM.AccrualBasisID AS AccrualBasisID
		,SM.BondTypeID
		,SM.IsZero AS IsZero
		,G.ProcessDate
		,G.OriginalPurchaseDate
		,SM.IsNDF
		,SM.FixingDate
		,SM.IDCOSymbol
		,SM.OSISymbol
		,SM.SEDOLSymbol
		,SM.CUSIPSymbol
		,SM.BloombergSymbol
		,MF.MasterFundName AS MasterFund
		,SM.UnderlyingDelta
		,SM.ISINSymbol
		,PT.LotId
		,PT.ExternalTransId
		,PT.TradeAttribute1
		,PT.TradeAttribute2
		,PT.TradeAttribute3
		,PT.TradeAttribute4
		,PT.TradeAttribute5
		,PT.TradeAttribute6
		,SM.ProxySymbol
		,SM.IsCurrencyFuture
		,UDA.*
		,PT.SettlCurrency
		,PT.AdditionalTradeAttributes
	FROM PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK      
	INNER JOIN T_Group G ON G.GroupID = PT.GroupID
	LEFT OUTER JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID
 INNER JOIN #SecMasterTemp SM ON PT.Symbol = SM.TickerSymbol              
	LEFT OUTER JOIN PM_CorpActionTaxlots CATaxlots ON PT.Taxlot_PK = CATaxlots.FKId
	INNER JOIN #Funds ON PT.FundID = #Funds.FundID
	INNER JOIN #Side ON G.OrderSideTagValue = #Side.OrderSideTagValue
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = #Funds.FundID
	LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
 LEFT OUTER JOIN #Temp_UDA UDA ON SM.Symbol_PK = UDA.Symbol_PK        
 WHERE   
TaxLotOpenQty <> 0        
		AND PT.Symbol = @Symbol
	ORDER BY TaxlotId

 DROP TABLE #Funds,#SecMasterTemp ,#Side,#TempTaxlotPK ,#Temp_UDA       
END
