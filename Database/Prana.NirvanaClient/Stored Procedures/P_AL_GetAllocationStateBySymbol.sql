/*
Modified by : Sachin Misra
Modification Date: 05-08-2022
Desc: 
1) Excluded current trade from calculation of prorata state by sending groupid and removed those groupid from taxlots
2) Also, removed Max(Taxlot_PK) from main calculation as that will optimize the performance of query
3) Sp optimizations
*/  
      
Create Procedure [dbo].[P_AL_GetAllocationStateBySymbol]
(                                
 @ToDate DateTime,                           
 @Symbol nvarchar(50),    
 @GroupIds Varchar(max)      
)                                
As                                
      
Begin try      
      
DECLARE @intErrorCode INT      
      
BEGIN TRAN      
      
--DECLARE @ToDate DateTime      
--Set @ToDate=GetDate()      
--DECLARE @ToDate DateTime      
--Set @ToDate='2014-03-25 08:00:00.000'      
      
-----------------------------------------------------------------------------      
-----------------------------------------------------------------------------      
-- Creating Temp table from PM_Taxlots      
-----------------------------------------------------------------------------      
-----------------------------------------------------------------------------      
SELECT  * INTO #TempGroupID            
FROM dbo.Split(@GroupIds, ',')     
        
select  distinct G.groupid,G.isswapped into #selectedGroups
from T_Group As G  with (nolock)        
Where @Symbol= Case WHEN G.IsSwapped = 1 then G.Symbol+'-Swap' else G.Symbol end  

  
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
 from PM_Taxlots   with (nolock)  
Where       
  Datediff(d, PM_Taxlots.AUECModifiedDate,GetDate()) >= 0 AND      
  Datediff(d, @ToDate, PM_Taxlots.AUECModifiedDate) <= 0  
  and groupid in (select groupid from #selectedGroups)    
 Group By TaxlotID     
  
		
--Get Open Taxlots   

Insert Into #TEMPAllocationState      
Select   
T.TaxlotID,   
T.GroupID,   
T.Symbol,   
T.TaxlotOpenQty,   
T.FundID,   
T.OrderSideTagValue,   
T.AvgPrice,   
T.OpenTotalCommissionandFees,  
G.IsSwapped      
From PM_Taxlots As T   with (nolock) 
Inner JOIN #TempTaxlot_PK Temp On Temp.Taxlot_PK = T.Taxlot_PK   
Inner Join #selectedGroups As G  with (nolock)  on G.GroupID = T.GroupID       
Where TaxLotOpenQty <> 0 


---- deleting data is fast operation as compare to sub query    
Delete #TEMPAllocationState  
Where GroupID In (Select * from #TempGroupID)  

    
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
  Declare @Uniquesymbol varchar(200)
  
SELECT @Uniquesymbol = x.Symbol
FROM 
(
  Select DISTINCT Symbol      
 from #TEMPAllocationState 
) AS x 

Insert Into #TEMPVsecData      
Select   
TickerSymbol,   
AssetId,   
Multiplier      
From V_SecMasterData  
where TickerSymbol =@Uniquesymbol

    

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
      
DROP TABLE #TEMPAllocationState ,#TempGroupID,#TempTaxlot_PK  ,#TEMPVsecData ,#selectedGroups  
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