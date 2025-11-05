CREATE TABLE [dbo].[T_CompanyTimeInForce]
(
    [CompanyTimeInForceID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CompanyID] INT NOT NULL, 
    [TimeInForceID] INT NOT NULL,
    Foreign Key (CompanyID) REFERENCES T_Company(CompanyID),
    Foreign Key (TimeInForceID) REFERENCES T_TimeInForce(TimeInForceID)
)
