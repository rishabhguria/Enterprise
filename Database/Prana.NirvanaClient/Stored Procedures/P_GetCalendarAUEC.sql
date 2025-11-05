    
CREATE PROCEDURE [dbo].[P_GetCalendarAUEC]    
(    
@calendarname nvarchar(max),
@year int  
)    
AS    
SELECT     
AUEC.auecid,    
AUEC.assetID,    
AUEC.UnderlyingID,    
AUEC.ExchangeID,    
AUEC.basecurrencyID,    
AUEC.displayname     
FROM T_auec AUEC right outer join T_calendarauec calAUEC ON AUEC.auecid = calAUEC.auecid    
WHERE calAUEC.calendarid =  (SELECT calendarid FROM T_calendar WHERE calendarname = @calendarname and calendaryear = @year)