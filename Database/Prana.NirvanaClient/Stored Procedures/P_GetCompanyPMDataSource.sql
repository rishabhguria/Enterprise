
CREATE PROCEDURE dbo.P_GetCompanyPMDataSource  
 (  
  @companyID int    
 )  
AS  
Declare @PMCompanyID int  
Set @PMCompanyID=0  
  
Select @PMCompanyID=PMCompanyID from PM_Company  
Where NOMSCompanyID=@companyID  
  
If (@PMCompanyID>0)  
Begin  
  
 SELECT     ThirdpartyID  
 FROM         PM_CompanyDataSources  
 Where PMCompanyID = @PMCompanyID  
End 