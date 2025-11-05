


/****** Object:  Stored Procedure dbo.P_DeleteVendor    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteVendor
	(
		@vendorID int	
	)
AS
Delete T_Vendor
Where VendorID = @vendorID




