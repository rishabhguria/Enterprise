CREATE Procedure [dbo].[P_DeleteUDASector] 
(
@SectorID int
)


as

UPDATE T_SMSymbolLookUpTable
SET UDASectorID = -2147483648
WHERE UDASectorID = @SectorID

DELETE FROM T_UDASector 
WHERE SectorID = @SectorID
