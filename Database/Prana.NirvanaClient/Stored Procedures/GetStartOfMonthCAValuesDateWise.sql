/****************************************************************************
Name		:	GetStartOfMonthCAValuesDateWise
Purpose		:	Returns all the Start of Month Capital Account Values date range wise.
Module		:	PM / ExPnL Server
Author		:	Bharat Kumar Jangir
****************************************************************************/
CREATE Procedure [dbo].GetStartOfMonthCAValuesDateWise      
(
@fromDate DateTime
)
As
Select FundID,StartOfMonthCapitalAccount from PM_StartOfMonthCapitalAccount Where Date = @fromDate

