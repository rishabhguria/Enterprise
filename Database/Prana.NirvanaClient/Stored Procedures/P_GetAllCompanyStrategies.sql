-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 22-mar-2014
--Purpose: Get strategy details from DB 
-----------------------------------------------------------------


/****** Object:  Stored Procedure dbo.P_GetAllCompanyStrategies    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllCompanyStrategies]
AS
	SELECT   CompanyStrategyID, StrategyName, StrategyShortName, CompanyID
FROM         T_CompanyStrategy
