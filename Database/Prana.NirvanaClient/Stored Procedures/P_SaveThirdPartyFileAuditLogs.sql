CREATE PROCEDURE [dbo].[P_SaveThirdPartyFileAuditLogs]
    @UserName NVARCHAR(100),
    @ThirdPartyName NVARCHAR(100),
    @JobName NVARCHAR(MAX),
    @DateTime DATETIME
AS
BEGIN
    INSERT INTO T_ThirdPartyFileAuditLogs (DateTime, Type, Action, Details)
    VALUES (
        @DateTime,
        'Manual File',
        CONCAT('File sent manually by ', @UserName),
        CONCAT('File placed on client FTP/Email for broker ', @ThirdPartyName, ' and format ', @JobName)
    );
END