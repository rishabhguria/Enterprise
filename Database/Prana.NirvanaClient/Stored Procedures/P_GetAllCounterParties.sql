

/****** Object:  Stored Procedure dbo.P_GetAllCounterParties    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllCounterParties]
AS
/*	SELECT   CounterPartyID, FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, Email1,
               ContactName2, Title2, Email2, Acronym, BaseCurrency
FROM         T_CounterParty */


SELECT     CounterPartyID, FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, EMail1, ContactName2, 
			Title2, EMail2, CounterPartyTypeID, Address2, CountryID, StateID, Zip, ContactName1_LastName, 
			ContactName1_WorkPhone, ContactName1_Cell, ContactName2_LastName, ContactName2_WorkPhone, 
			ContactName2_Cell, City, IsAlgoBroker, IsOTDorEMS
FROM         T_CounterParty
--Where IsActive  = 1

