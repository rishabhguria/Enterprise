CREATE Procedure [dbo].[P_DeleteUDASecurityType] 
(
@SecurityTypeID int
)

as

UPDATE T_SMSymbolLookUpTable
SET UDASecurityTypeID = -2147483648
WHERE UDASecurityTypeID = @SecurityTypeID


DELETE FROM T_UDASecurityType 
WHERE SecurityTypeID = @SecurityTypeID
