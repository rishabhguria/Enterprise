---------------------------------------------------------------------
--Modified by: Bharat Raturi
--date: 07-may-2014
--purpose: get the clients excluding the one with id -1
--usage: P_GetAllClientNames
---------------------------------------------------------------------
/****** Object:  Stored Procedure dbo.P_GetAllClientNames    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllClientNames]

AS
	SELECT   CompanyID, Name
    FROM     T_Company where CompanyID not in(-1) and IsActive = 1

