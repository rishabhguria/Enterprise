@ECHO OFF
SET ReleaseName=%~1
SET GitHash=%~2

if exist %cd%\Prana.NirvanaClient\PostDeployment\UpdateDBVersion.sql @ECHO Insert into T_DBVersion(Revision,Version,Product) values('%GitHash%','%ReleaseName%','Nirvana Enterprise')> "Prana.NirvanaClient\PostDeployment\UpdateDBVersion.sql"

if exist %cd%\Prana.SecurityMaster\PostDeployment\UpdateDBVersion.sql @ECHO Insert into T_DBVersion(Revision,Version,Product) values('%GitHash%','%ReleaseName%','Nirvana Enterprise')> "Prana.SecurityMaster\PostDeployment\UpdateDBVersion.sql"