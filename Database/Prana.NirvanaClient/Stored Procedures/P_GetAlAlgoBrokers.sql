CREATE PROCEDURE [dbo].[P_GetAlAlgoBrokers]
AS
SELECT CounterPartyID
	,FullName
	,ShortName
FROM T_CounterParty
WHERE IsAlgoBroker = 1
