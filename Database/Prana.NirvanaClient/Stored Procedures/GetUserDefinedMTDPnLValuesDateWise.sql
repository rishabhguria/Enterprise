/****************************************************************************
Name		:	GetUserDefinedMTDPnLValuesDateWise
Purpose		:	Returns all the User Defined MTD PnL Values date range wise.
Module		:	PM / ExPnL Server
Author		:	Bharat Kumar Jangir
****************************************************************************/
CREATE Procedure [dbo].GetUserDefinedMTDPnLValuesDateWise      
(
@fromDate DateTime
)
As
Select FundID,UserDefinedMTDPnL from PM_UserDefinedMTDPnL Where Date = @fromDate

