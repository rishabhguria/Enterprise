--------------------------------------------------------------------------------------
	-- Adding Domain Data
--------------------------------------------------------------------------------------

EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
:r .\DomainDataScript.sql
EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";


--------------------------------------------------------------------------------------
	-- Scripts to be run everytime after post deployment
--------------------------------------------------------------------------------------

:r .\T_UDA_DynamicUDAData_ResetTableSchemaScript.sql


--------------------------------------------------------------------------------------
	-- Running One time Deployment Scripts.
	-- NOTE : These are data scripts that are just run once in the data base. These should idealy be data snitization scripts. ANd hopefully idempotent :)
--------------------------------------------------------------------------------------

:r .\OneTimeDeploymentScript.sql 


--------------------------------------------------------------------------------------
	-- Validating Database Edition Specific Features.
--------------------------------------------------------------------------------------

:r EnterpriseEditionFeaturesChecker.sql


--------------------------------------------------------------------------------------
	-- Adding Release and SVN Versions in Database
--------------------------------------------------------------------------------------

:r UpdateDBVersion.sql