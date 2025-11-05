/*
P_GetMasterFunds '1182,1183,1184,1185'
*/

CREATE Procedure P_GetMasterFunds
(
 @FundID varchar(max)
)
As
Declare @Fund Table                                                                      
(                                                                      
 FundID int                                                                  
)                     
                  
Insert into @Fund                    
Select Cast(Items as int) from dbo.Split(@FundID,',')   

Select
CMF.CompanyMasterFundID as MasterFundID,
CMF.MasterFundName as MasterFundName,
CF.CompanyFundID as CompanyFundID
From T_CompanyFunds CF
Inner JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                      
Inner JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID

Where CF.CompanyFundID in (select FundID from @Fund)

