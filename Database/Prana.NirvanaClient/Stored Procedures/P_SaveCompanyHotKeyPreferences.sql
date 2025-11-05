CREATE PROCEDURE [dbo].[P_SaveCompanyHotKeyPreferences]
    @companyUserID int
AS
BEGIN  
    INSERT INTO T_CompanyUserHotKeyPreferences    
    SELECT CU.UserID,  
        'Allocation^Broker^Venue^TIF^Order Type^Execution Type',  
        0,  
        0,
		0,
        GETDATE()
    FROM T_CompanyUserHotKeyPreferences HP  
    RIGHT JOIN T_CompanyUser CU ON CU.UserID = HP.CompanyUserID  
    WHERE CU.UserID=@companyUserID and HP.CompanyUserID IS NULL   
    END
