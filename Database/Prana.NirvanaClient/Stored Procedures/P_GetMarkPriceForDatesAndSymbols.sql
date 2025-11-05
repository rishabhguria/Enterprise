  
CREATE PROCEDURE [dbo].[P_GetMarkPriceForDatesAndSymbols]        
(        
 @indices varchar(max),        
 @allDates varchar(max)        
)        
As        
        
        
declare @indexTable table(symbols varchar(30))        
declare @datesTable table(durationCode int,date datetime)        
        
insert into @indexTable Select Items from dbo.Split(@indices,',')        
        
insert into @datesTable Select ROW_NUMBER() OVER (ORDER BY cast(Items as datetime) Desc) AS durationCode ,Items from dbo.Split(@allDates,',')        
        
Select dates.durationCode, DM.Symbol, DM.Date, DM.FinalMarkPrice as MarkPrice from PM_DayMarkPrice DM        
Inner Join @indexTable indices on DM.Symbol = indices.symbols        
Inner Join @datesTable dates on DM.Date = dates.date
WHERE DM.FundID = 0   

