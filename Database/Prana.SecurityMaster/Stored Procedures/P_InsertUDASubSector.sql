/*******************************************************************************
created by : omshiv
Date - 11 Jun 2014
Desc- increase the column length of UDA  

********************************************************************************/
create Procedure [dbo].[P_InsertUDASubSector] 
(
@SubSectorName varchar(100),
@SubSectorID  int 
)
AS
 
DELETE FROM T_UDASubSector where SubSectorID =@SubSectorID

INSERT INTO T_UDASubSector(SubSectorName ,SubSectorID)
VALUES(@SubSectorName,@SubSectorID)

