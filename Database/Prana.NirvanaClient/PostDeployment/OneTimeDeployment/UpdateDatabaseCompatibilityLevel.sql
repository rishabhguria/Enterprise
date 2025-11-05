-- Upgrade database compatibility level to SQL Server 2022 (level 160)
DECLARE @sql NVARCHAR(MAX);
SET @sql = 'ALTER DATABASE [' + DB_NAME() + '] SET COMPATIBILITY_LEVEL = 160;';
EXEC sp_executesql @sql;
