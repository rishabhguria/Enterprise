--exec F_ZeroLastPxDBAlert 'Equity'            
            
CREATE FUNCTION [dbo].[F_ZeroLastPxDBAlert]                                      
(                                      
  @assetClass nvarchar(max)                                                     
)                                      
RETURNS nvarchar(max)                                      
AS                                      
BEGIN                
            
declare @Message nvarchar(max),@clientName nvarchar(50)            
            
Declare @allSymbols Table                              
(                              
 SymbolName varchar(50),                                    
 CreatedOn datetime,                 
 [Px Last] varchar(50),                       
 [Asset Class] varchar(100),        
 [Px Selected Feed (Local)] float,        
 [Closing Mark] nvarchar (50),  
 [Pricing Source] nvarchar (50)                      
)             
            
Declare @symbolsWithZeroLastPrice Table                              
(                              
 SymbolName varchar(50),                                              
 CreatedOn datetime,              
 [Px Last]  varchar(50),                          
 [Asset Class] varchar(100),        
 [Px Selected Feed (Local)] float,        
 [Closing Mark] nvarchar (50),  
 [Pricing Source] nvarchar (50)                            
)                
            
Declare @symbolName Table                              
(                              
 SymbolName varchar(50)                               
)            
            
Select top 1 @ClientName= Name from dbo.T_Company             
            
Insert into @allSymbols            
SELECT Symbol , CreatedOn,[Px Last], [Asset Class] , [Px Selected Feed (Local)] , [Closing Mark] , [Pricing Source]       
FROM   (            
   SELECT CreatedOn, Symbol, [Px Last], [Asset Class] , [Px Selected Feed (Local)] , [Closing Mark] , [Pricing Source]        
        , row_number() OVER(PARTITION BY symbol ORDER BY CreatedOn DESC) AS rn            
   FROM   T_PMDataDump            
where [Asset Class] =@assetclass       
and [Closing Mark] != 'Undefined'           
   ) sub            
WHERE  rn = 1;            
            
Insert INTO @symbolsWithZeroLastPrice                              
Select Symbolname, CreatedOn , [Px Last], [Asset Class], [Px Selected Feed (Local)] , [Closing Mark], [Pricing Source] from @allSymbols                                       
where ([Px Last] <= '.001'           
OR [Px Selected Feed (Local)] < CONVERT(float,  [Closing Mark]) - (CONVERT(float,  [Closing Mark]) * .2))  
AND [Pricing Source] = 'LiveFeed'          
            
Insert into @symbolName(SymbolName)                              
Select distinct SymbolName from @symbolsWithZeroLastPrice               
            
declare @msg varchar(max)                              
SELECT @msg = STUFF((SELECT ', ' + CAST(SymbolName AS VARCHAR(30))                              
FROM @symbolName                               
FOR XML PATH(''), TYPE)                              
.value('.','NVARCHAR(MAX)'),1,2,' ')              
            
Begin Set @Message = 'Last Prices in '+@ClientName+' are not updating. E-signal might have dropped. Please check. Symbol Names are : '+ @msg                      
End             
            
 -- Return the result of the function                                      
RETURN @Message              
End