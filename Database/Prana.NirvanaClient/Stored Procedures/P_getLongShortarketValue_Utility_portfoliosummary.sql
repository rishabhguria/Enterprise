/*************************************************                                                                  
Script for Fund wise Long Short market value.                       
This section in the report is to be picked up FROM General Ledger.                      
                      
Please refer doc attached in the JIRA for detailed description                      
--https://jira.nirvanasolutions.com:8443/browse/ONB-4390
--https://jira.nirvanasolutions.com:8443/browse/ONB-7576                  
                               
Execution Statement:                                        
exec [P_GetLongShortarketValue_Utility_PortFolioSummary]
@EndDate='2022-06-01 00:00:00',                                               
*************************************************/
CREATE PROCEDURE [dbo].[P_GetLongShortarketValue_Utility_PortFolioSummary] (
	@EndDate DATETIME
	)
AS
BEGIN

--Declare @EndDate DATETIME
Declare @fund VARCHAR(MAX)
Declare @CompanyID BIGINT
Declare @ReportName VARCHAR(MAX)


--Set @EndDate='2022-06-01 00:00:00'																	
Set @CompanyID=1
Set @ReportName=N'Portfolio Summary'


SET NOCOUNT ON

DECLARE @ReportID INT = 61

/*--------------------------------------------------------------------------------------------------                      
Extract funds out in temp table                    
---------------------------------------------------------------------------------------------------*/

select FundID INTO #Funds from T_W_Funds
						  

/*--------------------------------------------------------------------------------------------------                      
Get Data from T_SubAccountBalances for selected funds for End date        
The data is fetched for end date as Closing Equity for the end date is required only in this case        
---------------------------------------------------------------------------------------------------*/
SELECT CloseDRBalBase
	,CloseCRBalBase
	,SubAccountID
	,SubAccBal.FundID
INTO #filteredT_SubAccountBalances
FROM T_Cash_SubAccountBalances SubAccBal
INNER JOIN #Funds FUNDS ON SubAccBal.FundID = FUNDS.FundID
WHERE TransactionDate = @EndDate
	AND SubAccBal.CompanyID = @CompanyID

IF(@ReportID=61)
BEGIN

--datediff(d,@EndDate,TransactionDate)=0        
CREATE TABLE #tempFinalDataNewPar (
	Statistic VARCHAR(MAX)
	,TotalMktValue FLOAT
	,FundID INT
	)

INSERT INTO #tempFinalDataNewPar (
	Statistic
	,TotalMktValue
	,FundID
)
		SELECT 'Long Market Value',Sum(CloseDRBalBase - CloseCRBalBase),FundID
		FROM #filteredT_SubAccountBalances tempSubAccBal
		WHERE tempSubAccBal.SubAccountID IN (
				SELECT DISTINCT SubAccountID
				FROM T_Cash_SubAccountMappingPSR
				INNER JOIN T_Cash_FieldsPSR ON T_Cash_FieldsPSR.FieldID = T_Cash_SubAccountMappingPSR.FieldID
				INNER JOIN T_Cash_PositionsPSR POS ON T_Cash_FieldsPSR.PositionID = POS.PostionID
					AND T_Cash_FieldsPSR.CompanyID = @CompanyID
					AND T_Cash_SubAccountMappingPSR.CompanyID = @CompanyID
				WHERE T_Cash_FieldsPSR.FieldName = 'Long Market Value'
				AND POS.SubAccountReportID=@ReportID
				)
				Group By FundID


INSERT INTO #tempFinalDataNewPar (
	Statistic
	,TotalMktValue
	,FundID
)
		SELECT 'Short Market Value',Sum(CloseDRBalBase - CloseCRBalBase),FundID
		FROM #filteredT_SubAccountBalances tempSubAccBal
		WHERE tempSubAccBal.SubAccountID IN (
				SELECT DISTINCT SubAccountID
				FROM T_Cash_SubAccountMappingPSR
				INNER JOIN T_Cash_FieldsPSR ON T_Cash_FieldsPSR.FieldID = T_Cash_SubAccountMappingPSR.FieldID
				INNER JOIN T_Cash_PositionsPSR POS ON T_Cash_FieldsPSR.PositionID = POS.PostionID
					AND T_Cash_FieldsPSR.CompanyID = @CompanyID
					AND T_Cash_SubAccountMappingPSR.CompanyID = @CompanyID
				WHERE T_Cash_FieldsPSR.FieldName = 'Short Market Value'
				AND POS.SubAccountReportID=@ReportID
				)
				Group by fundID



/*--------------------------------------------------------------------------------------------------                      
Section Ends                      
---------------------------------------------------------------------------------------------------*/

/***************                      
Final Output of the Procedure         
******************/
SELECT FundName
	,Statistic AS Long_Short_Type
	,isnull(TotalMktValue, 0) AS 'TotalMktValue'
FROM #tempFinalDataNewPar Temp
INNER JOIN
T_W_Funds FUND on FUND.FundID=Temp.FundID
Order By FundName, Long_Short_Type


/***************                      
Drop Temporary Tables For New PAR        
******************/
DROP TABLE #tempFinalDataNewPar,
			#Funds,
			#filteredT_SubAccountBalances

END

END