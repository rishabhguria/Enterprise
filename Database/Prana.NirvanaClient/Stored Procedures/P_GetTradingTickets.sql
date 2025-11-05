

CREATE  PROCEDURE [dbo].[P_GetTradingTickets] (
		@companyUserID int
)
AS

Select 
--	 --SettingType  ,
--	 ButtonName ,
--	 Description ,
--	 --DefaultTicket ,
--	 --ActionButton ,
--	 DisplayColor ,
--	 --DisplayName ,
--	 Side ,
--	 FundID ,
--	 StrategyID ,
--	 --ClientComp ,
--	 --ClientTrade ,
--	 --ClientFund ,
--	 --Principal ,
--	 --ShortExempt ,
--	 ClearingFirmID ,
--	 AssetID ,
--	 UnderLyingID ,
--	 CounterpartyID ,
--	 VenueID ,
--	 Quantity ,
--	 OrderTypeID ,
--	 TimeInForceID ,
--	 ExecutionInstructionID ,
--	 HandlingInstructionID ,
--	 TradingAccountID ,
--	 PegOffSet ,
--	 Discr ,
--	 DisplayQuantity ,
--	 Random ,
--	 PNP ,
--	 --TicketSettingsID,
--	 CompanyUserID,
--	 -DisplayPosition,
--	 LimitType,
--	 LimitOffset


* from T_TicketSettings
where CompanyUserID = @companyUserID
--Where SettingType = @ID or SettingType = 3
