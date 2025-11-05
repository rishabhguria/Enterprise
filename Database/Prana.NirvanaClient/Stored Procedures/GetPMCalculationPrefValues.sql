/****************************************************************************
Name		:	GetPMCalculationPrefValues
Purpose		:	Returns all the User Defined MTD PnL Values date range wise.
Module		:	PM
Author		:	Bharat Kumar Jangir
****************************************************************************/
CREATE Procedure [dbo].[GetPMCalculationPrefValues]      
(          
@companyID int               
)
AS
SELECT FundID,HighWaterMark,Stopout,TraderPayoutPercent FROM T_PMCalculationPrefs WHERE CompanyID=@companyID

