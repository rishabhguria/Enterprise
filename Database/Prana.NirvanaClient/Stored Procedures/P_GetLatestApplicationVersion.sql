CREATE PROC P_GetLatestApplicationVersion
as
begin
SELECT TOP 1 Version
 FROM T_DBVersion where version is not null

ORDER BY date
 DESC;
end
