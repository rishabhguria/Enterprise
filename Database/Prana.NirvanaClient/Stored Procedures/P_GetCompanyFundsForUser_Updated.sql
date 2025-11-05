        
/*********************************************            
P_GetCompanyFundsForUser_Updated 17,'3,4'         
        
Modified Date: 2012-11-09        
Modified By: Rahul Gupta        
Decsription:  Fund should in ascending order in VSR , TSR ,PSR and in other reports        
     http://jira.nirvanasolutions.com:8080/browse/FS-49          
 Added order by fundname.        
*********************************************/        
CREATE PROCEDURE [dbo].[P_GetCompanyFundsForUser_Updated] (        
 @paramUserID INT        
 ,@FundID VARCHAR(max)  
 )        
AS        
DECLARE @MasterFundAssoc TABLE (RecordCount INT)        
DECLARE @permissiontype INT        
        
INSERT INTO @MasterFundAssoc        
EXEC P_IsMasterFundAssociationSaved        
        
DECLARE @count INT        
        
SET @count = (        
  SELECT RecordCount        
  FROM @MasterFundAssoc        
  )        
        
--Select @count         
IF EXISTS (        
  SELECT PreferenceValue        
  FROM T_PranaKeyValuePreferences        
  WHERE PreferenceKey = 'IsFundPermissionByGroup'        
  )        
 SELECT @permissiontype = CONVERT(INT, PreferenceValue)        
 FROM T_PranaKeyValuePreferences        
 WHERE PreferenceKey = 'IsFundPermissionByGroup'        
ELSE        
 SELECT @permissiontype = 0        
        
        
IF (@permissiontype = 0)        
BEGIN        
 IF @count > 0        
 BEGIN        
  DECLARE @MasterFundID TABLE (FundID INT)        
        
  INSERT INTO @MasterFundID        
  SELECT Cast(Items AS INT)        
  FROM dbo.Split(@FundID, ',')        
        
  SELECT CUF.CompanyFundID        
   ,CF.FundName        
  FROM T_CompanyFunds CF        
  INNER JOIN T_CompanyUserFunds CUF ON CF.CompanyFundID = CUF.CompanyFundID        
  INNER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID        
  INNER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID        
  WHERE CUF.CompanyUserID = @paramUserID        
   AND CMF.CompanyMasterFundID IN (        
    SELECT FundID        
    FROM @MasterFundID        
    )        
  ORDER BY CF.UIOrder,CF.Fundname      
 END        
 ELSE        
 BEGIN        
  SELECT CUF.CompanyFundID        
   ,CF.FundName        
  FROM T_CompanyFunds CF        
  INNER JOIN T_CompanyUserFunds CUF ON CF.CompanyFundID = CUF.CompanyFundID        
  WHERE CUF.CompanyUserID = @paramUserID        
  ORDER BY CF.UIOrder,CF.Fundname      
 END        
END        
ELSE        
BEGIN        
 DECLARE @RoleID INT        
        
 SELECT @RoleID = RoleID        
 FROM T_CompanyUser        
 WHERE UserID = @paramUserID        
        
 IF @count > 0        
 BEGIN        
  --DECLARE @MasterFundID TABLE (FundID INT)        
        
  INSERT INTO @MasterFundID        
  SELECT Cast(Items AS INT)        
  FROM dbo.Split(@FundID, ',')        
        
  IF (        
    (@RoleID = 3)        
    OR (@RoleID = 4)        
    )        
  BEGIN        
   SELECT Distinct CF.CompanyFundID        
    ,CF.FundName, CF.UIOrder       
   FROM T_CompanyFunds CF        
   INNER JOIN T_Company CP ON CP.CompanyID = CF.CompanyID        
   INNER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID        
   INNER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID        
   WHERE CF.IsActive = 1        
    AND CMF.CompanyMasterFundID IN (        
     SELECT FundID        
     FROM @MasterFundID        
     )    
   ORDER BY UIOrder,FundName    
  END        
  ELSE        
  BEGIN        
   SELECT distinct CF.CompanyFundID        
    ,CF.FundName,CF.UIOrder        
   FROM T_CompanyFunds CF        
   INNER JOIN T_Company CP ON CP.CompanyID = CF.CompanyID        
   INNER JOIN T_GroupFundMapping GFM ON CF.CompanyFundID = GFM.FundID        
   INNER JOIN T_CompanyUserFundGroupMapping CUFGM ON GFM.FundGroupID = CUFGM.FundGroupID        
   INNER JOIN T_CompanyUser CU ON CUFGM.CompanyUserID = CU.UserID        
   INNER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID        
   INNER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID        
   WHERE CU.UserID = @paramUserID        
    AND CMF.CompanyMasterFundID IN (        
     SELECT FundID        
     FROM @MasterFundID        
     )        
    AND CF.IsActive = 1       
   ORDER BY UIOrder,Fundname         
  END        
 END        
 ELSE        
 BEGIN        
  IF (        
(@RoleID = 3)        
    OR (@RoleID = 4)        
    )        
  BEGIN        
   SELECT distinct CF.CompanyFundID        
    ,CF.FundName,CF.UIOrder        
   FROM T_CompanyFunds CF        
   INNER JOIN T_Company CP ON CP.CompanyID = CF.CompanyID        
   WHERE CF.IsActive = 1    
   ORDER BY UIOrder,Fundname         
  END        
  ELSE        
  BEGIN        
   SELECT distinct CF.CompanyFundID        
    ,CF.FundName,CF.UIOrder        
   FROM T_CompanyFunds CF        
   INNER JOIN T_Company CP ON CP.CompanyID = CF.CompanyID        
   INNER JOIN T_GroupFundMapping GFM ON CF.CompanyFundID = GFM.FundID        
   INNER JOIN T_CompanyUserFundGroupMapping CUFGM ON GFM.FundGroupID = CUFGM.FundGroupID        
   INNER JOIN T_CompanyUser CU ON CUFGM.CompanyUserID = CU.UserID        
   WHERE CU.UserID = @paramUserID        
    AND CF.IsActive = 1   
   ORDER BY UIOrder,FundName            
  END        
 END        
END
