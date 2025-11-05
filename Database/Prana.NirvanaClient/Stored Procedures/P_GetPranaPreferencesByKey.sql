
-- =============================================
-- Author:		omshiv
-- Create date: March 2014
-- Description:	Get Prana App level key value pair of preferences by key
-- Usage - P_GetPranaPreferences
-- =============================================
CREATE procedure [dbo].[P_GetPranaPreferencesByKey]          
(
@PreferenceKey varchar(100)
)    
AS          
select PreferenceValue from T_PranaKeyValuePreferences
where PreferenceKey = @PreferenceKey

