CREATE PROCEDURE [dbo].[P_DeleteSymbolfromPI]                         
(                          
    @listSymbol varchar(max)                 
)                                                                                                                                                                                     
AS                                                    
BEGIN 

DECLARE @Symbols Table
(
   symbol varchar(100)
)

INSERT INTO @Symbols
Select items from dbo.Split(@listSymbol,',')
Delete from T_UserOptionModelInput 
where symbol in ( Select symbol from @Symbols)
END
