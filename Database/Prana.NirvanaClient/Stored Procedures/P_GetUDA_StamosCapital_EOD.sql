/*    
    
Author: Sandeep Singh    
Date: 1 April 2025    
Desc: Send an EOD file to customers detailing open securities accountwise, including dynamic User-Defined Attributes (UDA).    
    
*/    
    
CREATE Procedure [dbo].[P_GetUDA_StamosCapital_EOD]    
(    
 @thirdPartyID INT    
 ,@companyFundIDs VARCHAR(max)    
 ,@inputDate DATETIME    
 ,@companyID INT    
 ,@auecIDs VARCHAR(max)    
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties     
 ,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                
 ,@fileFormatID INT    
 ,@includeSent INT = 0    
)    
As    
    
SET NOCOUNT ON;    
    
--Declare @inputDate Date    
--Set @inputDate = GetDate()    
    
Select CF.FundName As Account, PT.Symbol    
InTo #Temp_Account_OpenSymbol    
From PM_Taxlots PT    
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID     
Where PT.TaxLot_PK IN    
(    
Select Max(Taxlot_PK) From PM_Taxlots    
Where DateDiff(Day,AUECModifiedDate, @inputDate) >= 0    
Group By TaxLotID     
)    
And TaxLotOpenQty > 0    
Group By CF.FundName, PT.Symbol     
    
Select     
Convert(varchar(10),@inputDate,110) As EffectiveDate,     
OP.Account,    
OP.Symbol,     
SM.BloombergSymbol,    
RiskCurrency As [RiskCurrency],    
Issuer As [Issuer],    
CountryOfRisk As [CountryOfRisk],    
Region As [Region],    
Analyst As [Analyst],    
UCITSEligibleTag As [UCITSEligibleTag],    
LiquidTag As [LiquidTag],    
MarketCap As[MarketCap],    
CustomUDA1 As [CustomUDA1],    
CustomUDA2 As [GICS],    
CustomUDA3 As [CustomUDA3],    
CustomUDA4 As [CustomUDA4],    
CustomUDA5 As [AOValue],    
CustomUDA6 As [DVPValue],    
CustomUDA7 As [LRAValue],    
CustomUDA8 As [AOWWHAssessment],    
CustomUDA9 As [DVPWWHAssessment],    
CustomUDA10 As [LRAWWHAssessment],    
CustomUDA11 As [AOSCPSizeBand],    
CustomUDA12 As [DVPSCPSizeBand],    
CustomUDA13 As [LRASCPSizeBand],    
CustomUDA14 As [AOSector],    
CustomUDA15 As [DVPSector],    
CustomUDA16 As [LRASector],    
CustomUDA17 As [AOSubSector],    
CustomUDA18 As [DVPSubSector],    
CustomUDA19 As [LRASubSector],    
CustomUDA20 As [CustomUDA20],    
CustomUDA21 As [AOAspirationalSCPSizeBand],    
CustomUDA22 As [AOMinSize],    
CustomUDA23 As [AOMidPointSize],    
CustomUDA24 As [AOMaxSize],    
CustomUDA25 As [AOLatestTargetSize],    
CustomUDA26 As [AOModelTargetSize],    
CustomUDA27 As [AORawIVPrice],    
CustomUDA28 As [AOConfAdjIVPrice],    
CustomUDA29 As [DVPAspirationalSCPSizeBand],    
CustomUDA30 As [DVPMinSize],    
CustomUDA31 As [DVPMidPointSize],    
CustomUDA32 As [DVPMaxSize],    
CustomUDA33 As [DVPLatestTargetSize],    
CustomUDA34 As [DVPModelTargetSize],    
CustomUDA35 As [DVPRawIVPrice],    
CustomUDA36 As [DVPConfAdjIVPrice],    
CustomUDA37 As [LRAAspirationalSCPSizeBand],    
CustomUDA38 As [LRAMinSize],    
CustomUDA39 As [LRAMidPointSize],    
CustomUDA40 As [LRAMaxSize],    
CustomUDA41 As [LRALatestTargetSize],    
CustomUDA42 As [LRAModelTargetSize],    
CustomUDA43 As [LRARawIVPrice],    
CustomUDA44 As [LRAConfAdjIVPrice],    
CustomUDA45 As [CustomUDA45]    
From #Temp_Account_OpenSymbol OP    
Inner Join V_SecMasterData SM On SM.TickerSymbol = OP.Symbol     
Inner Join V_UDA_DynamicUDA UDA On UDA.Symbol_PK = SM.Symbol_PK    
    
    
Drop Table #Temp_Account_OpenSymbol