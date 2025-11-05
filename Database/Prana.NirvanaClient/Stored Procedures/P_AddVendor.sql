


/****** Object:  Stored Procedure dbo.P_AddVendor    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_AddVendor
	(
		@Ven_name varchar(30)
	)	
AS 
INSERT T_Vendor (VendorName)
  Values(@Ven_name)


