/*
https://jira.nirvanasolutions.com:8443/browse/PRANA-28123
This query is used to update the existing data in tabel T_Journal.
If the user doesn't enter any symbol or description with manual journal entry, it should be saved as blank in database and not as NULL.
*/

UPDATE T_Journal
SET Symbol = ''
WHERE Symbol IS NULL

UPDATE T_Journal
SET PBDesc = ''
WHERE PBDesc IS NULL