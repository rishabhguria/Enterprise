
--http://jira.nirvanasolutions.com:8080/browse/PRANA-11049
--Desh Bandhu:- Do this change to select all the symbols, which was changed earlier to select top 1 because of memory issues.
--I have added new Project "Prana.SecuritySearch" in "Prana.Server" solution implementing Lucene text search related to this. So, it should not lead to memory issues now.

CREATE procedure [dbo].[P_GetAllSymbol]
as
select TickerSymbol,  BloombergSymbol, FactSetSymbol, ActivSymbol from T_SMSymbolLookUpTable  option (fast 1)

