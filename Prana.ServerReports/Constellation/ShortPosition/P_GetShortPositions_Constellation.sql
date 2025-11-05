/***************************************************************                    
Author : Pooja Porwal                                                                                                               
Creation Date: September 18,2015                          
Description : Get Australian short positions  
Jira Link : http://jira.nirvanasolutions.com:8080/browse/PRANA-10354                
Usage:                  
[P_GetShortPositions_Constellation] '2014-09-01','1213','','Symbol','USD'                  
*******************************************************************/ 

PRINT 'Started Executing: P_GetShortPositions_Constellation.sql'
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE id = object_id(N'[dbo].[P_GetShortPositions_Constellation]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
BEGIN
	DROP PROCEDURE [dbo].[P_GetShortPositions_Constellation]
END
GO

Create Procedure P_GetShortPositions_Constellation         
(               
@EndDate datetime,                          
@Funds Varchar(max),                         
@SearchString Varchar(5000),                          
@SearchBy Varchar(100),            
@Currency varchar(10)                          
)                             
As                 
              
Begin  
         
select * into #FundIDs                              
  from dbo.Split(@Funds, ',')                
              
Select * InTo #TempSymbol                 
From dbo.split(@SearchString , ',')               
                
Select Distinct * InTo #Symbol                 
From #TempSymbol        
          
Select               
Symbol              
,UnderlyingSymbol              
,LEFT(BloomBergSymbol, 
	CASE 
		WHEN CharIndex(' ', BloomBergSymbol) = 0 
		THEN LEN(BloomBergSymbol) 
		ELSE CharIndex(' ', BloomBergSymbol) - 1 
	END) 
As BloomBergSymbol              
,CUSIPSymbol              
,SedolSymbol              
,OSISymbol              
,IDCOSymbol              
,Fund              
,MasterFund              
,CONVERT(VARCHAR(10),Rundate,101) as Rundate             
,PNL.BeginningQuantity  as Quantity              
,Side              
,Multiplier              
,SideMultiplier              
,TradeCurrency              
,UDASector              
,UDACountry              
,UDASubSector              
,UDASecurityType              
,UDAAssetClass              
,Exchange              
,Currency              
              
 Into #TempConstellation          
from T_MW_GenericPNL  PNL      
INNER JOIN  T_CompanyFunds CF on PNL.Fund =CF.FundName              
INNER JOIN #FundIDs on CF.CompanyFundID = #FundIDs.Items              
Where Side = 'Short' And DATEDIFF(Day,RunDate,@EndDate)=0 
And PNL.BeginningQuantity <> 0and PNL.Currency in (select * from dbo.Split(@Currency,',')) 
End


If(@SearchString <> '')                             
 Begin                           
  if (@searchby='Symbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.Symbol                  
  Order by symbol                  
  end                  
  else if (@searchby='underlyingSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.underlyingSymbol                  
  Order by symbol                  
  end                    
  else if (@searchby='BloombergSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.BloombergSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='SedolSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.SedolSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='OSISymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.OSISymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='IDCOSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.IDCOSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='ISINSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.ISINSymbol                  
  Order by symbol                  
  end                     
  else if (@searchby='CUSIPSymbol')                  
  begin                  
  SELECT * FROM #TempConstellation                  
  Inner Join #Symbol on #Symbol.items = #TempConstellation.CUSIPSymbol                  
  Order by Symbol                  
  end                               
 End                              
Else                              
 Begin                              
  Select * from #TempConstellation Order By symbol                    
 End 

Drop TABLE #TempConstellation,#TempSymbol,#Symbol

GO

PRINT 'Done Executing: P_GetShortPositions_Constellation.sql'
GO