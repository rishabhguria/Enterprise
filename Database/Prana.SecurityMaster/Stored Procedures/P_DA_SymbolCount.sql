
/* =============================================      
-- Modified By: Sumit Kakra
-- Modification Date: May 27 2015
-- Description: Nirvana Data access webservice proedure for webmethod SymbolCount
-- EXEC [P_DA_SymbolCount] 1,'',0
-- Modified By: Sandeep Singh
-- Modification Date: OCT 11, 2019
-- Description: Input parameters changed
-- =============================================  */    
CREATE Procedure [dbo].[P_DA_SymbolCount]
(
	@AddOnId INT,
	@errorMessage varchar(max) output,                                      
	@errorNumber int output                               
)                            
As  

--Declare @errorMessage varchar(max)                                      
--Declare @errorNumber int  

SET @ErrorMessage = 'Success'                                      
SET @ErrorNumber = 0  

--Declare @AddOnId INT
--Set @AddOnId = 1                               
                            
BEGIN TRY                        
      -- To ensure no locking, it allows dirty reads, so check for blank symbols and Qty>0
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 

	Declare @SymbolsAUECIDsList varchar(max)
	Declare @SpecificSymbolsList varchar(max)

	Set @SymbolsAUECIDsList = (SELECT SymbolsAUECIDs  FROM T_DA_AddOns WHERE SymbolsAUECIDs Is Not Null And AddOnId = @AddOnId)

	Set @SpecificSymbolsList = (SELECT SpecificSymbols FROM T_DA_AddOns WHERE SpecificSymbols Is Not Null And AddOnId = @AddOnId)

	Declare @T_AUECIDs Table                                              
	(                                              
	 AUECID int       
	)                                              
	Insert Into @T_AUECIDs Select RTRIM(LTRIM(Items)) From dbo.Split(@SymbolsAUECIDsList, ',')  

	Declare @T_SpecificSymbolsList Table                                              
	(                                              
	 SpecificSymbol Varchar(max)       
	)                                              
	Insert Into @T_SpecificSymbolsList Select RTRIM(LTRIM(Items)) From dbo.Split(@SpecificSymbolsList, ',') 

	Select 
	COUNT(*)
	FROM T_SMSymbolLookUpTable 
	WHERE T_SMSymbolLookUpTable.AUECID in (Select AUECID from @T_AUECIDs)  OR TickerSymbol in (Select SpecificSymbol from @T_SpecificSymbolsList)

END TRY                                      
BEGIN CATCH                              
                                        
 SET @ErrorMessage = ERROR_MESSAGE();                                      
 SET @ErrorNumber = Error_number();          
                                       
END CATCH;            

