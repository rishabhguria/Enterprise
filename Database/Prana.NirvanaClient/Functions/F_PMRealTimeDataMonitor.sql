CREATE FUNCTION [dbo].[F_PMRealTimeDataMonitor]                
(                
  @symbol nvarchar(max),        
  @assetClass nvarchar(max),        
  @usercount nvarchar(max)                
)                
RETURNS nvarchar(max)                
AS                
BEGIN          
        
-- Declare the local variable and intermediate tables here.           
        
declare @count int, @DumpTime datetime,@Message nvarchar(max),@clientName nvarchar(50)        
             
Declare @Index1 table    
(    
[Symbol] nvarchar(50),     
[Px Last] float,     
[avg PX] float,    
[CreatedOn] datetime,    
[Asset Class] nvarchar(50)    
)                
        
Declare @symbolsWithMaxChange Table        
(        
 SymbolName varchar(50),        
 maxChange float,        
 createdon datetime,        
 [Asset Class] varchar(100)        
)        
        
Declare @symbolsWithMaxChange_updated Table        
(        
 SymbolName varchar(50),        
 maxChange float,        
 [Asset Class] varchar(100)        
)         
        
Declare @symbolName Table        
(        
 SymbolName varchar(50)         
)        
        
Declare @symbolNameTemp Table        
(        
 TempSymbol varchar(50)         
)         
        
declare @assetClassTemp Table        
(        
 TempAssetClass varchar(50)         
);         
        
-- Declaration section ends here.        
        
-- preparing temporary table for the symbol(s) and asset class(es) provided by end-user.        
Insert into @symbolNameTemp         
Select Cast(Items as varchar(50)) from dbo.Split(@symbol,',')          
        
Insert into @assetClassTemp         
Select Cast(Items as varchar(50)) from dbo.Split(@assetClass,',')          
        
Select top 1 @ClientName= Name from dbo.T_Company                               
                
-- Inserting the required data in temp table for further calculations.        
Insert into @Index1                
Select symbol,[Px Last], null, createdon,[Asset Class] from T_PMdatadump          
where [Asset Class] not in (select TempAssetClass from @assetClassTemp)  
       
Declare @Index1AvgPrice Table    
(    
 Symbol Varchar(100),        
 [Asset Class] Varchar(100),        
 [avg px] Float    
)    
    
Insert Into @Index1AvgPrice    
Select     
symbol,    
[Asset Class],     
avg([Px Last]) as [avg px]     
from @Index1 group by symbol, [Asset Class]        
            
-- Calculating avg price based on Last Px for the same symbols, so that it can be used further.                 
Insert into @Index1                
Select A.symbol,    
A.[Px Last],     
B.[avg px],     
A.createdon,     
A.[Asset Class]     
from @Index1 A     
left outer join @Index1AvgPrice B               
--(Select symbol,[Asset Class], avg([Px Last]) as [avg px] from @Index1 group by symbol, [Asset Class]) B                
on A.symbol = B.symbol               
        
Delete from @Index1 where [avg px] is null                
           
-- Here we are checking the max change in the price of symbol that is being calculated based on Avg Px calculated previously.         
-- Now there are possibility that a symbol is there multiple times like follows:-        
-- Symbol  Px_Last  Avg Px  AssetClass        
-- AAPL    50  51  Equity        
-- AAPL    52  51  Equity        
-- AAPL    51  51  Equity                
Insert into @symbolsWithMaxChange        
Select Symbol,  avg(abs([Px Last]-[avg Px])),[createdon],max([Asset Class]) as [Asset Class]  from @Index1                 
group by Symbol, [createdon]         
      
      
        
-- Now still there are possibility that a symbol is there multiple times like follows:-        
-- Symbol  MaxChange         
-- AAPL    1        
-- AAPL    1        
-- AAPL    0        
-- So we can't go with maxchange as in above case for last symbol price has changed but still the maxchange calculated is 0.        
        
-- So we sum up all the maxchange value for a symbol and if it is 0 then we consider that prices are not updating (actually in this case prices        
-- prices did not update in last 3 minutes at least).        
Insert INTO @symbolsWithMaxChange_updated        
Select Symbolname, SUM(Maxchange),max([Asset Class]) from @symbolsWithMaxChange                 
group by Symbolname       
      
Delete from @symbolsWithMaxChange_updated      
where symbolname in(select distinct symbolname from @symbolsWithMaxChange where maxchange>0)      
      
      
        
-- getting total count and list of symbols for which prices are not updating.        
--Select @count=count(DISTINCT Symbolname)  from @symbolsWithMaxChange_updated where MaxChange =0 and MaxChange is not null            
        
Insert into @symbolName(SymbolName)        
Select distinct SymbolName from @symbolsWithMaxChange_updated where MaxChange =0 and MaxChange is not null        
and SymbolName not in (select TempSymbol from @symbolNameTemp)         
and [Asset Class] not in (select TempAssetClass from @assetClassTemp)        
        
Select top 1 @DumpTime = max(createdon) from @Index1      
    
-- Ideally Symbol Count should be after filtering the data i.e. data is not in @symbolNameTemp and @assetClassTemp    
Select @count=count(DISTINCT SymbolName)  from @symbolName               
              
-- Getting list of symbols in string format for which prices are not updating.        
declare @msg varchar(max)        
SELECT @msg = STUFF((SELECT ', ' + CAST(SymbolName AS VARCHAR(10))        
FROM @symbolName         
FOR XML PATH(''), TYPE)        
.value('.','NVARCHAR(MAX)'),1,2,' ')        
        
-- Raising message.        
If @count >0 and @count >= @usercount        
   Begin Set @Message = 'Last Prices in '+@ClientName+' are not updating. E-signal might have dropped. Please check. Symbol Names are : '+ @msg  End                 
If datediff(minute,@DumpTime, getutcdate()) > 3                 
   Begin Set @Message=@Message +'DataDump process might not be runing. Please check. '                
  End                
 -- Return the result of the function                
RETURN @Message                
                
END 