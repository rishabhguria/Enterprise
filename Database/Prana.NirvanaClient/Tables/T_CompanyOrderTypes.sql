CREATE TABLE [dbo].[T_CompanyOrderTypes]
(
    [CompanyOrderTypeID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CompanyID] INT NOT NULL, 
    [OrderTypeID] INT NOT NULL,
    Foreign Key (CompanyID) REFERENCES T_Company(CompanyID),
    Foreign Key (OrderTypeID) REFERENCES T_OrderType(OrderTypesID)
)
