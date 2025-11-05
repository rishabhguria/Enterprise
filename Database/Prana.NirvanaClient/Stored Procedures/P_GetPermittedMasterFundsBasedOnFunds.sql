CREATE PROCEDURE [dbo].[P_GetPermittedMasterFundsBasedOnFunds]
	@companyID INT,
    @userID INT
AS
BEGIN
    SET NOCOUNT ON;

   /*---------------------------------------------------
      1. Get permitted funds for the user into a temp table
    ---------------------------------------------------*/
    IF OBJECT_ID('tempdb..#PermittedFunds') IS NOT NULL DROP TABLE #PermittedFunds;
    SELECT CF.CompanyFundID
    INTO #PermittedFunds
    FROM T_CompanyUserFunds CUF
    INNER JOIN T_CompanyFunds CF 
        ON CF.CompanyFundID = CUF.CompanyFundID
    WHERE CUF.CompanyUserID = @userID
      AND CF.IsActive = 1;

    /*---------------------------------------------------
      2. Get MasterFund -> Fund mapping into temp table
    ---------------------------------------------------*/
    IF OBJECT_ID('tempdb..#MasterFundMapping') IS NOT NULL DROP TABLE #MasterFundMapping;
    SELECT MFSA.CompanyMasterFundID, MFSA.CompanyFundID
    INTO #MasterFundMapping
    FROM T_CompanyMasterFundSubAccountAssociation MFSA
    INNER JOIN T_CompanyMasterFunds MF 
        ON MF.CompanyMasterFundID = MFSA.CompanyMasterFundID
	INNER JOIN 
    T_CompanyFunds 
        ON T_CompanyFunds.CompanyFundID = MFSA.CompanyFundID
WHERE 
    MF.CompanyID = @companyID
    AND T_CompanyFunds.IsActive = 1;

    /*---------------------------------------------------
      3. Validate which MasterFunds are fully permitted
    ---------------------------------------------------*/
    IF OBJECT_ID('tempdb..#MasterFundValidation') IS NOT NULL DROP TABLE #MasterFundValidation;
    SELECT 
        M.CompanyMasterFundID,
        COUNT(DISTINCT M.CompanyFundID) AS TotalFunds,
        SUM(CASE WHEN P.CompanyFundID IS NOT NULL THEN 1 ELSE 0 END) AS PermittedFundsCount
    INTO #MasterFundValidation
    FROM #MasterFundMapping M
    LEFT JOIN #PermittedFunds P 
        ON M.CompanyFundID = P.CompanyFundID
    GROUP BY M.CompanyMasterFundID;

    /*---------------------------------------------------
      4. First result set → MasterFunds where all funds permitted
    ---------------------------------------------------*/
    SELECT 
        MV.CompanyMasterFundID
    FROM #MasterFundValidation MV
    WHERE MV.TotalFunds = MV.PermittedFundsCount;

END
