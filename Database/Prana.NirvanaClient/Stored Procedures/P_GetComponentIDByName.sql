

/****************************************************************************                                                                                            
Name :   [P_GetComponentIDByName]                                                                                            
Purpose:  Gets the layout id against the given layout name.  
Author : Bhupesh Bareja  
Usage: exec P_GetComponentIDByName 'Blotter'  
Module: PranaLayout                           
****************************************************************************/  
Create PROCEDURE [dbo].[P_GetComponentIDByName] (  
@componentName varchar(50)  
)  
AS  
declare @id int   
Set @id = 0  
  
IF @componentName='PranaAnalysisUI'
BEGIN
SET @componentName='Risk Analysis'
END
IF @componentName='SymbolLookUp'
BEGIN
SET @componentName='Security Master'
END
Select   
@id = ComponentID   
From T_LayoutComponents  
  
Where ComponentName = @componentName  
  
Select @id
