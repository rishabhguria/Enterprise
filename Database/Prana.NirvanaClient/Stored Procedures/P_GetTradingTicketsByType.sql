

CREATE   PROCEDURE [dbo].[P_GetTradingTicketsByType] (
--@SettingType int ,
@CompanyUserID int
)
AS

Select 
--	 SettingType  ,
--	 Name ,
--	 Description ,
--	 DefaultTicket ,
--	 ActionButton ,
--	 DisplayColor ,
--	 DisplayName ,
--	 Side ,
--	 FundID ,
--	 StrategyID ,
--	 ClientComp ,	
--	 ClientTrade ,
--	 ClientFund ,
--	 Principal ,
--	 ShortExempt ,
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
--	 TicketSettingsID,
--	 CompanyUserID,
--	 DisplayPosition,
--	 LimitType,
--	 LimitOffset,
--	 Display,
--OpenClose
 

* From T_TicketSettings

Where --(SettingType = @SettingType or SettingType = 3 ) and  
CompanyUserID = @CompanyUserID
 --and Display = 'True'

--select * from T_TicketSettings
