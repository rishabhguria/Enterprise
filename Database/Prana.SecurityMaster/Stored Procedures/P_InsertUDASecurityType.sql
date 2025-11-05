/*******************************************************************************
created by : omshiv
Date - 11 Jun 2014
Desc- increase the column length of UDA  

********************************************************************************/
create Procedure [dbo].[P_InsertUDASecurityType] 
(
@SecurityTypeName varchar(200),
@SecurityTypeID  int 
)
AS
 


DELETE FROM T_UDASecurityType where SecurityTypeID =@SecurityTypeID 

INSERT INTO T_UDASecurityType(SecurityTypeName ,SecurityTypeID)
VALUES(@SecurityTypeName,@SecurityTypeID)

