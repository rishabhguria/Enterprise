--------------------------------------------------------------------------------------
	-- Adding Domain Data
--------------------------------------------------------------------------------------

:r .\DomainDataScript.sql


--------------------------------------------------------------------------------------
	-- Running One time Deployment Scripts.
	-- NOTE : These are data scripts that are just run once in the data base. These should idealy be data snitization scripts.
--------------------------------------------------------------------------------------

:r .\OneTimeDeploymentScript.sql 


--------------------------------------------------------------------------------------
	-- Running Dummy Data Scripts.
	-- NOTE : These scripts must run if there is no data already in the table (when creating a fresh database instance)
--------------------------------------------------------------------------------------

:r .\DummyDataScript.sql 


--------------------------------------------------------------------------------------
	-- Validating Database Edition Specific Features.
--------------------------------------------------------------------------------------

:r EnterpriseEditionFeaturesChecker.sql


--------------------------------------------------------------------------------------
	-- Adding Release and SVN Versions in Database
--------------------------------------------------------------------------------------

:r UpdateDBVersion.sql