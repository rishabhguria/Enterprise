--Usage:
--EXEC P_UDA_SaveDynamicUDAData
--	@Symbol_PK = 81229140420,	
--	@UDAData = '<DynamicUDAs> <Issuer> Issuer1 </Issuer> <RiskCurrency> USD </RiskCurrency> </DynamicUDAs>',
--	@FundID = 2

-- Modified By: Disha Sharma
-- Date: 12/8/2015
-- Description: Removed transaction to avoid rollback error because of nested transaction, PRANA-12365

CREATE PROCEDURE [dbo].[P_UDA_SaveDynamicUDAData] 
	@Symbol_PK		BIGINT,	
	@UDAData		XML,
	@FundID			INT
AS

BEGIN
	IF NOT EXISTS (SELECT 1 FROM dbo.T_UDA_DynamicUDAData WHERE Symbol_PK = @Symbol_PK AND FundID = @FundID)
	BEGIN
		INSERT INTO T_UDA_DynamicUDAData
				(
					Symbol_PK,	
					UDAData,
					FundID
				)
		 VALUES
			   (
					@Symbol_PK,	
					NULL,
					@FundID
				)
	END

	CREATE TABLE #TempXml (UDAData XML)
	INSERT INTO #TempXml(UDAData)VALUES (@UDAData)

	DECLARE @UpdateString VARCHAR(MAX)
	SET		@UpdateString = ' UPDATE DUDA SET '

	SELECT	@UpdateString = @UpdateString + ' DUDA.[' + Tag + '] = TX.UDAData.value(''(/DynamicUDAs/' + Tag  + ')[1]'', ''varchar(100)''),'
	FROM	T_UDA_DynamicUDA 

	SET	@UpdateString = SUBSTRING(@UpdateString,0,LEN(@UpdateString))

	EXEC(@UpdateString + ' FROM T_UDA_DynamicUDAData DUDA CROSS JOIN #TempXml TX WHERE DUDA.Symbol_PK = ' + @Symbol_PK + 'AND DUDA.FundID = ' + @FundID )

	DROP TABLE #TempXml
END

