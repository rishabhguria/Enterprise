/****** Object:  Stored Procedure dbo.P_GetCounterPartyTypes    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCounterPartyTypes
AS
	Select CounterPartyTypeID, CounterPartyType
	From T_CounterPartyType Order by CounterPartyType ASC