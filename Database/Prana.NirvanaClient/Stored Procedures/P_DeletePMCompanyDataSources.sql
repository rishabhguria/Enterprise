

/****** Object:  Stored Procedure dbo.P_DeletePMCompanyDataSources    Script Date: 25/01/2007 7:11:24 PM ******/
CREATE PROCEDURE [dbo].[P_DeletePMCompanyDataSources]
(
	@companyID int
)
AS
Declare @PMCompanyID int 
Set @PMCompanyID = 0

Select @PMCompanyID = PMCompanyID
From PM_Company
Where NOMSCompanyID = @companyID
	
	Delete PM_CompanyDataSources
	Where PMCompanyID = @PMCompanyID



