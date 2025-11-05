/****** Object:  Stored Procedure dbo.P_GetAllCommonAUECExchanges    Script Date: 05/23/2006 3:20:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllCommonAUECExchanges
AS
	SELECT distinct E.ExchangeID, E.FullName, E.DisplayName, E.TimeZone, E.RegularTradingStartTime, E.RegularTradingEndTime, 
		E.LunchTimeStartTime, E.LunchTimeEndTime, E.Country, E.StateID, E.RegularTime, E.LunchTime, E.CountryFlagID,
		E.LogoID, E.ExchangeIdentifier, E.TimeZoneOffSet
	FROM	T_Exchange E inner join T_AUEC AUEC on E.ExchangeID = AUEC.ExchangeID
	

