


CREATE PROCEDURE dbo.P_SaveRLVenues
	(
		@RLID	int,
		@TradingAccountID	int,	
		@CounterPartyVenueID	int,
		@Rank int
	)
AS

	
	Declare @total int
	set @total = 0
	
	SELECT      @total = COUNT(*)
	FROM         T_RoutingLogicVenues
	WHERE     (RLID_FK = @RLID) AND (Rank = @Rank)
	
	if(@total = 0)
	Begin
		INSERT INTO T_RoutingLogicVenues
		                      (RLID_FK, CompanyTradingAccountID_FK, CompanyCounterPartyVenueID_FK, Rank)
		VALUES     (@RLID, @TradingAccountID, @CounterPartyVenueID, @Rank)
		
	End
	Else
	Begin	
	
	UPDATE    T_RoutingLogicVenues
	SET      CompanyTradingAccountID_FK = @TradingAccountID, CompanyCounterPartyVenueID_FK = @CounterPartyVenueID        
	WHERE     (RLID_FK = @RLID) AND (Rank = @Rank)
	
	end 


