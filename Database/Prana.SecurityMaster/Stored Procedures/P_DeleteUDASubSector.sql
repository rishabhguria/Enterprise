CREATE Procedure [dbo].[P_DeleteUDASubSector]
(
 @SubSectorID int
)

as

UPDATE T_SMSymbolLookUpTable
SET UDASubSectorID = -2147483648
WHERE UDASubSectorID = @SubSectorID

DELETE FROM T_UDASubSector 
WHERE SubSectorID=@SubSectorID


-- P_DeleteUDASubSector 8
