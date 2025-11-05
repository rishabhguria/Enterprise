Create Procedure [dbo].[P_getSize]
as
SET NOCOUNT ON 
  DBCC UPDATEUSAGE(0)

  SELECT sysobjects.[name] AS [TableName],
    SUM(sysindexes.reserved) * 8 AS [Size(KB)],
    SUM(sysindexes.dpages) * 8 AS [Data(KB)],
    (SUM(sysindexes.used) - SUM(sysindexes.dpages)) * 8 AS [Indexes(KB)],
    (SUM(sysindexes.reserved) - SUM(sysindexes.dpages)) * 8 AS [Unused(KB)]
  FROM dbo.sysindexes AS sysindexes
    JOIN dbo.sysobjects AS sysobjects ON sysobjects.id = sysindexes.id
  WHERE sysobjects.[type] = 'U'
  GROUP BY sysobjects.[name]
  ORDER BY [Size(KB)] DESC
