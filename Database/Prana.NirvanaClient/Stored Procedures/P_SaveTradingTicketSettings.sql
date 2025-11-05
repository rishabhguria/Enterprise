

CREATE PROCEDURE [dbo].[P_SaveTradingTicketSettings] (
	@AssetID int,
	@AuecID int,
	@ButtonColor varchar(50),
	@DisplayPosition varchar(20),
	@CompanyUserID int,
	@CounterpartyID int,
	@Description varchar(50),
	@Discr varchar(20),
	@DisplayQuantity int,
	@ExecutionInstructionID varchar(10),
	@FundID int,
	@HandlingInstructionID varchar(10),
	@IsHotButton varchar(10),
	@LimitOffset varchar(20),
	@LimitType int,
	@ButtonName varchar(50),
	@OrderTypeID varchar(10),
	@Peg varchar(20),
	@PNP int,
	@Quantity int,
	@Random int,
	@SideID varchar(10),
	@StrategyID int,
	@DefaultTicketID varchar(200),
	@TIF varchar(10),
	@TradingAccountID int,
	@UnderLyingID int,
	@VenueID int,
	@ClearingFirmID int


--	@SettingType  int,
--	@ActionButton int,
--	@DisplayName varchar(50),
--	@ClientCompanyID int,
--	@ClientTraderID int,
--	@ClientFundID int,
--	@Principal int,
--	@ShortExempt int,
--	@ClearingFirmID int,
--
--	@TicketSettingsID varchar(200),
--
--	@Display varchar(10),
--@OpenClose varchar(10)
)
AS
Declare @result int

if ((select count(*) from T_TicketSettings where DefaultTicketID =@DefaultTicketID)=0)
begin


	--Insert
	Insert into T_TicketSettings
(
	 AssetID ,
	 AUECID,
	 DisplayColor ,
	 DisplayPosition,
	 CompanyUserID,
	 CounterpartyID ,
	 Description ,
	 Discr ,
	 DisplayQuantity ,
	 ExecutionInstructionID ,
	 FundID ,
	 HandlingInstructionID ,
	 IsHotButton,
	 LimitOffset,
	 LimitType,
	 ButtonName ,
	 OrderTypeID ,
	 PegOffSet ,
	 PNP ,
	 Quantity ,
	 Random ,
	 Side ,
	 StrategyID ,
	 DefaultTicketID,
	 TimeInForceID ,
	 TradingAccountID ,
	 UnderLyingID ,
	 VenueID ,
	 ClearingFirmID
)
	Values
(
	@AssetID ,
	@AuecID ,
	@ButtonColor ,
	@DisplayPosition ,
	@CompanyUserID ,
	@CounterpartyID ,
	@Description ,
	@Discr ,
	@DisplayQuantity ,
	@ExecutionInstructionID ,
	@FundID ,
	@HandlingInstructionID ,
	@IsHotButton ,
	@LimitOffset ,
	@LimitType ,
	@ButtonName ,
	@OrderTypeID,
	@Peg ,
	@PNP ,
	@Quantity ,
	@Random ,
	@SideID ,
	@StrategyID ,
	@DefaultTicketID ,
	@TIF,
	@TradingAccountID ,
	@UnderLyingID ,
	@VenueID,
	@ClearingFirmID
)
	
	Set @result = scope_identity()
Select @result

end 
 else 
begin
Update  T_TicketSettings
Set
	 --SettingType = @SettingType  ,
	 AssetID =@AssetID ,
	 AUECID = @AuecID,
	 DisplayColor =@ButtonColor ,
	 DisplayPosition = @DisplayPosition,
	 CompanyUserID=@CompanyUserID,
	 CounterpartyID= @CounterpartyID ,
	 Description =@Description ,
	 Discr =@Discr ,
	 DisplayQuantity =@DisplayQuantity ,
	 ExecutionInstructionID =@ExecutionInstructionID ,
	 FundID=@FundID ,
	 HandlingInstructionID =@HandlingInstructionID ,
	 IsHotButton = @IsHotButton,
	 LimitOffset = @LimitOffset,
	 LimitType = @LimitType,
	 ButtonName =@ButtonName ,
	 OrderTypeID =@OrderTypeID ,
	 PegOffSet =@Peg ,
	 PNP =@PNP ,
	 Quantity =@Quantity ,
	 Random =@Random ,
	 Side =@SideID ,
	 StrategyID= @StrategyID ,
	 TimeInForceID= @TIF ,
	 TradingAccountID =@TradingAccountID ,
	 UnderLyingID= @UnderLyingID ,
	 VenueID= @VenueID ,
	 ClearingFirmID =@ClearingFirmID
	
Where DefaultTicketID = @DefaultTicketID
Set @result = scope_identity()
Select @result

end
