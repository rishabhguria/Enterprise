
-- =============================================
-- Author:		omshiv
-- Create date: March 2014
-- Description:	Get Prana App level key value pair of preferences
-- Usage - P_GetPranaPreferences
-- =============================================
CREATE procedure [dbo].[P_GetPranaPreferences]          
       
AS          
select PreferenceKey,PreferenceValue from T_PranaKeyValuePreferences

