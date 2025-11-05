
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date, ,>  
-- Description: <Description, ,>  
-- =============================================  
CREATE FUNCTION [dbo].[GetPaddedSymbol] (  
 @Symbol varchar(max)
)  
RETURNS varchar(max)  
AS  
BEGIN  
 RETURN CASE LEN(RTRIM(LTRIM(@Symbol)))
		When 1
		Then '000' + @Symbol
		When 2 
		Then '00' + @Symbol
		When 3 
		Then '0' + @Symbol
		Else @Symbol
		End 
END
