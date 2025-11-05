CREATE PROCEDURE [dbo].[P_UpdateShortLocateData] (
	@NirvanaLocateID INT
	,@BorrowerId VARCHAR(50)
	,@BorrowSharesAvailable FLOAT
	,@BorrowedShare INT
	,@BorrowedRate VARCHAR(50)
	,@ClientMasterFund VARCHAR(50)
	)
AS
	DECLARE @result INT
	if(@ClientMasterFund='')
	BEGIN
	UPDATE T_ShortLocateDetails
	SET BorrowSharesAvailable = @BorrowSharesAvailable
		,BorrowedShare = @BorrowedShare
		,BorrowedRate = @BorrowedRate
	WHERE NirvanaLocateID = @NirvanaLocateID AND BorrowerId = @BorrowerId AND SLImportDate=CONVERT(date, getdate())
	END
	ELSE
	BEGIN
	UPDATE T_ShortLocateDetails
	SET BorrowSharesAvailable = @BorrowSharesAvailable
		,BorrowedShare = @BorrowedShare
		,BorrowedRate = @BorrowedRate
	WHERE NirvanaLocateID = @NirvanaLocateID AND BorrowerId = @BorrowerId AND SLImportDate=CONVERT(date, getdate()) AND ClientMasterfundId=(Select CompanyMasterFundID from T_CompanyMasterFunds where MasterFundName=@ClientMasterFund)
	END
	
