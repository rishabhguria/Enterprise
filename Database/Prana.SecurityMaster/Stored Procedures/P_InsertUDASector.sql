/*******************************************************************************
created by : omshiv
Date - 11 Jun 2014
Desc- increase the column length of UDA  

********************************************************************************/
create Procedure [dbo].[P_InsertUDASector] 
(
 @SectorName varchar(100),
 @SectorID int 
)
AS
DELETE FROM T_UDASector where SectorID = @SectorID

INSERT INTO T_UDASector(SectorName ,SectorID)
VALUES(@SectorName,@SectorID)
