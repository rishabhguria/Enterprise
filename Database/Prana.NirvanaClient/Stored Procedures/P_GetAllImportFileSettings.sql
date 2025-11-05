-----------------------------------------------------
--Created BY: Bharat raturi
--date: 26/03/14
--Purpose: Get the file settings from the Database

--Modified BY: Bharat raturi
--date: 28/05/14
--Purpose: Get the file settings from the Database with new detail BatchStartDate

--Modified BY: Bhavana
--date: 10/06/14
--Purpose: Get the file settings from the Database with changed column CompanyID

--usage P_GetAllImportFileSettings 1
-------------------------------------------
CREATE procedure [dbo].[P_GetAllImportFileSettings]
@thirdPartyID int 
as
select 
importFileSettingID,
T_ImportFileSettings.IsActive,
FormatName,
ImportTypeID,
CompanyID,
ReleaseID,
FundID,
XSLTPath,
XSDPath,
ImportSPName,
FTPFolderPath,
LocalFolderPath,
--ImportFileName,
FtpID,
EmailID,
EmailLogID,
DecryptionID,
ThirdPartyID,
ISNULL(PriceToleranceColumns,'')as PriceToleranceColumns ,
ISNULL(FormatType,'')as FormatType,
BatchStartDate,  
ImportFormatID
from
T_ImportFileSettings
where ThirdPartyID=@thirdPartyID
