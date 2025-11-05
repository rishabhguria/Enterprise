-- =============================================

-- Author:		<Author,,Kashish G.>

-- Create date: <Create Date,,2017-08-28>

-- Description:	<Description,, To delete Journals and Activites for Trading and Revaluation Data>

-- =============================================

CREATE PROCEDURE P_DeleteTradingAndRevaluationActivities	

	@FKID VarChar(max),
	@xml NTEXT

AS

BEGIN	

	SET NOCOUNT ON;

    CREATE TABLE #FKIDList (
    
	FKID varchar(max)
   
    )


    INSERT INTO #FKIDList
    SELECT
      Items AS FKID
    FROM dbo.Split(@FKID, ',')


	DELETE AA FROM T_AllActivity AA
	INNER JOIN #FKIDList FK
	ON FK.FKID = AA.FKID
	WHERE TransactionSource=1;


	DELETE J FROM T_Journal J
	INNER JOIN #FKIDList FK
	ON FK.FKID = J.TaxLotID
	WHERE TransactionSource=1;


	/*---------------------------------------------------------------------------------------
	                      Revaluation Entries Deletion Work
	--------------------------------------------------------------------------------------- */

	DECLARE @handle INT  

    EXEC sp_xml_preparedocument @handle OUTPUT  
    ,@xml


	CREATE TABLE #TempData (  
    FundID INT  

   ,Symbol VARCHAR(50)
     
   ,[Date] DateTime    

  )      


  INSERT INTO #TempData (  

    FundID 
	 
   ,Symbol 
    
   ,[Date]    
  )  

   SELECT AccountID  

   ,Symbol  

   ,CONVERT(varchar(8), [Date], 112) AS [Date]   

   FROM OPENXML(@handle, '/Data/CashActivity', 2) WITH (  
 
    AccountID INT 'AccountID' 
	 
   ,Symbol VARCHAR(50)  

   ,[Date] DATETIME 
   ) 


   DELETE AA FROM T_AllActivity AA
   INNER JOIN #TempData TD
   ON AA.FundID = TD.FundID
   AND AA.Symbol = TD.Symbol
   AND datediff(d,TD.Date,AA.TradeDate)>=0
   Where TransactionSource=9


   DELETE J FROM T_Journal J
   INNER JOIN #TempData TD
   ON J.FundID = TD.FundID
   AND J.Symbol = TD.Symbol
   AND datediff(d,TD.Date,J.TransactionDate)>=0
   Where TransactionSource=9

   Drop Table #FKIDList
   Drop Table #TempData

END
