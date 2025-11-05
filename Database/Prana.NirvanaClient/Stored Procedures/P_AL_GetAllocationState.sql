 
CREATE Procedure [P_AL_GetAllocationState]
(                          
	@FromDate DateTime                          
)                          
As                          

Begin try

DECLARE @intErrorCode INT

BEGIN TRAN

--DECLARE @ToDate DateTime
--Set @ToDate=GetDate()
--DECLARE @FromDate DateTime
--Set @FromDate='2014-03-25 08:00:00.000'

-----------------------------------------------------------------------------
-----------------------------------------------------------------------------
-- Creating Temp table from PM_Taxlots
-----------------------------------------------------------------------------
-----------------------------------------------------------------------------

Create Table #TEMPAllocationState
(
	TaxlotID Varchar(50),
	GroupID Varchar(50),
	Symbol Varchar(200),
	TaxlotOpenQty Float,
	FundID Int,
	OrderSideTagValue Varchar(10),
	AvgPrice float,
	OpenTotalCommissionandFees float,
	IsSwapped bit
)

Select Max(Taxlot_PK) As Taxlot_PK 
Into #TempTaxlot_PK
 from PM_Taxlots  
 Where   
	Datediff(d, PM_Taxlots.AUECModifiedDate,GetDate()) >= 0 AND  
	Datediff(d, @FromDate, PM_Taxlots.AUECModifiedDate) >= 0  
	Group By TaxlotID


--Get Open Taxlots
Insert Into #TEMPAllocationState
Select T.TaxlotID, T.GroupID, T.Symbol, T.TaxlotOpenQty, T.FundID, T.OrderSideTagValue, T.AvgPrice, T.OpenTotalCommissionandFees,G.IsSwapped
From PM_Taxlots  as T 
Inner JOIN #TempTaxlot_PK Temp On Temp.Taxlot_PK = T.Taxlot_PK
Inner JOIN T_group as G on G.GroupID=T.GroupID
Where TaxLotOpenQty <> 0 
--Where Taxlot_PK in
--(
--	Select Max(Taxlot_PK) 
--	from PM_Taxlots
--	Where 
--		Datediff(d, PM_Taxlots.AUECModifiedDate,GetDate()) >= 0 AND
--		Datediff(d, @FromDate, PM_Taxlots.AUECModifiedDate) >= 0
--	Group By TaxlotID
--) And TaxLotOpenQty<>0


-----------------------------------------------------------------------------
-----------------------------------------------------------------------------
-- Creating Temp table from V_Secmaster
-----------------------------------------------------------------------------
-----------------------------------------------------------------------------
Create Table #TEMPVsecData
(
	TickerSymbol varchar(100),
	AssetId int,
	Multiplier float
)




Insert Into #TEMPVsecData
Select TickerSymbol, AssetId, Multiplier
From V_SecMasterData
Where TickerSymbol in
(
	Select DISTINCT Symbol
	from #TEMPAllocationState	
)




-----------------------------------------------------------------------------
-----------------------------------------------------------------------------
-- Selecting required data
-----------------------------------------------------------------------------
-----------------------------------------------------------------------------

SELECT 
	case when TAX.IsSwapped = 1 THEN TAX.Symbol+'-Swap'
	else TAX.Symbol end as Symbol,
	TAX.FundID,
	SUM(TAX.TaxlotOpenQty * dbo.GetSideMultiplier(TAX.OrderSideTagValue)) AS CumQuantity,
	SUM(
		CASE 
			WHEN VSEC.AssetId in (8,13) THEN
				(TAX.TaxlotOpenQty * IsNull(TAX.AvgPrice,0) * VSEC.Multiplier * dbo.GetSideMultiplier(TAX.OrderSideTagValue))/100 
			ELSE
				TAX.TaxlotOpenQty * IsNull(TAX.AvgPrice,0) * VSEC.Multiplier * dbo.GetSideMultiplier(TAX.OrderSideTagValue) 
		END
		) as NetNotional
from 
#TEMPAllocationState AS TAX Left Outer Join 
#TEMPVsecData AS VSEC ON TAX.Symbol = VSEC.TickerSymbol
GROUP BY Symbol,FundID,IsSwapped

--Select * from PM_TAXLOTS

DROP TABLE #TEMPAllocationState,#TEMPVsecData,#TempTaxlot_PK
COMMIT

END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	

    -- Use RAISERROR inside the CATCH block to return 
    -- error information about the original error that 
    -- caused execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH

