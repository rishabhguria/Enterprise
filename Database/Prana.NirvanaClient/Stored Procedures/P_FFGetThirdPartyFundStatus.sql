
/*
Name : <P_FFGetThirdPartyFundStatus>
Created by :<Kanupriya>
Dated : <11/23/2006>
Purpose : <To fetch the status of the selected Third Party permitted fund.>
*/
/*
Modified By : <Kanupriya>
Date : </12/05/2006>
Purpose : <To change the column name as per the changed in the Flat File Tables.>
*/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundStatus]
	
	(
	@companyThirdPartyID int,
	@companyFundID int,
	@dateTime Datetime,
	@FormatId int
	)
	
AS 
	SELECT     T_ThirdPartyFFRunReport.ThirdPartyFFRunID, T_ThirdPartyFFRunReport.CompanyThirdPartyID, T_ThirdPartyFFRunReport.TradeDate, 
	                      T_ThirdPartyFFRunReport.StatusID, T_ThirdPartyFFRunStatus.Status, T_ThirdPartyFFRunFunds.CompanyFundID
	FROM         T_ThirdPartyFFRunReport INNER JOIN
	                      T_ThirdPartyFFRunFunds ON T_ThirdPartyFFRunReport.ThirdPartyFFRunID = T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK INNER JOIN
	                      T_ThirdPartyFFRunStatus ON T_ThirdPartyFFRunReport.StatusID = T_ThirdPartyFFRunStatus.StatusID
	WHERE     (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @companyThirdPartyID) AND (T_ThirdPartyFFRunReport.TradeDate = @dateTime) AND 
	                      (T_ThirdPartyFFRunFunds.CompanyFundID = @companyFundID)AND (T_ThirdPartyFFRunReport.FormatId=@FormatId)


