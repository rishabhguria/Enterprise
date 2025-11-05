    
/*=============================================    
 Author:  Sumit Kakra    
 Create date: April 23, 2008  
 Description: Search for text in DB. Returns procedures, parameters in procedures, Tables, Table Columns
 Parameters: @text string to be serached. Default value is NULL
			   @Flag flag to specify search criteria
			   'A'  - Search in whole db
			   'P'  - Search in Stored Procedures
			   'U'  - Search in Tables 
			   'V'  - Search in Views 
			   'FN' - Search in Functions
			   Flags can be combined e.g. '''P'',''U'',''V'',''FN'''
 Sample Usage: 
 EXEC FindTextInDatabase1 'Order', '''P'',''U'',''V'',''FN'''
 EXEC FindTextInDatabase1 'Order'
 =======================================================================*/    
CREATE PROCEDURE [dbo].[FindTextInDatabase1]    
 -- Add the parameters for the stored procedure here    
@text varchar(max) = NULL,
@Flag varchar(max) = '''A'''    
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 Declare @newString varchar(202)    
 Declare @SQLString varchar(max)   
 Declare @SQLConditionString varchar(max)   
 Declare @strFlag varchar(max)   
 Declare @tempInt int   

Declare @HelpInfo varchar(max)

Set @HelpInfo =  'Description: Search for text in DB. Returns procedures, parameters in procedures, Tables, Table Columns
 Parameters: @text string to be serached. Default value is NULL
			   @Flag flag to specify search criteria
			   ''A''  - Search in whole db
			   ''P''  - Search in Stored Procedures
			   ''U''  - Search in Tables 
			   ''V''  - Search in Views 
			   ''FN'' - Search in Functions
			   Flags can be combined e.g. ''''''P'''',''''U'''',''''V'''',''''FN''''''

 Executing Stored procedure without any parameters prints this help message
 
 Sample Usage: 
 EXEC FindTextInDatabase1 ''Order'', ''''''P'''',''''U'''',''''V'''',''''FN''''''
 EXEC FindTextInDatabase1 ''Order'''

If @text is NULL 
Begin
	Print @HelpInfo
End
Else
Begin

	 Set @strFlag = LTRIM(RTRIM(@Flag))
	 Set @newString = '%' + @text + '%'    
	 Set @SQLConditionString = ''    


	Select  @tempInt =CHARINDEX('FN', @strFlag)

	If @tempInt  > 0
	Begin
		Set @strFlag = @strFlag + ',''TF'''
	End 

	If LEN(@strFlag) > 0 And EXISTS(Select Items from dbo.Split('''P'',''U'',''V'',''FN'',''TF''',',') 
							Where Items in (Select Items from dbo.Split(@strFlag,',')))
	Begin
	Set @SQLConditionString = ' And sysobjects.xtype in (' + @strFlag + ')'
	End

	 Set @SQLString = 'Select [Column\Parameter Name],[Table\Stored Procedure Name], [Object Type]   
	 from  
	 (  
	  SELECT     
	  syscolumns.name AS [Column\Parameter Name],   
	  sysobjects.Name As [Table\Stored Procedure Name],    
	  Case sysobjects.xtype  
	  When ''P'' Then ''Stored Procedure''  
	  When ''U'' Then ''Table''  
	  When ''V'' Then ''View''  
	  When ''FN'' Then ''Function''  
	  When ''TF'' Then ''Function''  
	  End  As [Object Type]  
	  FROM     sysobjects    
	  Left JOIN syscolumns ON sysobjects.id = syscolumns.id   
	  WHERE (syscolumns.Name like '''+ @newString + ''' or ' + 'sysobjects.Name  like ''' + @newString + ''')'

	Set @SQLString = @SQLString + @SQLConditionString

	Set @SQLString = @SQLString +  
	' UNION  
	  SELECT     
	  '''' AS [Column\Parameter Name],   
	  sysobjects.Name As [Table\Stored Procedure Name],    
	  Case sysobjects.xtype  
	  When ''P'' Then ''Stored Procedure''  
	  When ''U'' Then ''Table''  
	  When ''V'' Then ''View''  
	  When ''FN'' Then ''Function''  
	  When ''TF'' Then ''Function''  
	  End  As [Object Type]  
	  FROM     sysobjects    
	  Left Join syscomments ON  sysobjects.id = syscomments.id   
	  WHERE (sysobjects.Name like ''' + @newString + ''' or ' + 'syscomments.text like ''' + @newString + ''')'

	Set @SQLString = @SQLString +  @SQLConditionString

	Set @SQLString = @SQLString +  
	 ') As DataTable  
	 Order By [Object Type],[Column\Parameter Name] DESC,[Table\Stored Procedure Name]'  

	Select (@SQLString)
	Exec (@SQLString)
End

  
-- Select [Column\Parameter Name],[Table\Stored Procedure Name], [Object Type]   
-- from  
-- (  
--  SELECT     
--  syscolumns.name AS [Column\Parameter Name],   
--  sysobjects.Name As [Table\Stored Procedure Name],    
--  Case sysobjects.xtype  
--  When 'P' Then 'Stored Procedure'  
--  When 'U' Then 'Table'  
--  When 'V' Then 'View'  
--  When 'FN' Then 'Function'  
--  When 'TF' Then 'Function'  
--  End  As [Object Type]  
--  FROM     sysobjects    
--  Left JOIN syscolumns ON sysobjects.id = syscolumns.id   
--  WHERE (syscolumns.Name like @newString or sysobjects.Name  like @newString)  
-- UNION  
--  SELECT     
--  '' AS [Column\Parameter Name],   
--  sysobjects.Name As [Table\Stored Procedure Name],    
--  Case sysobjects.xtype  
--  When 'P' Then 'Stored Procedure'  
--  When 'U' Then 'Table'  
--  When 'V' Then 'View'  
--  When 'FN' Then 'Function'  
--  When 'TF' Then 'Function'  
--  End  As [Object Type]  
--  FROM     sysobjects    
--  Left Join syscomments ON  sysobjects.id = syscomments.id   
--  WHERE (sysobjects.Name like @newString or syscomments.text like @newString)  
-- ) As DataTable  
-- Order By [Object Type],[Column\Parameter Name] DESC,[Table\Stored Procedure Name]  
END    
  