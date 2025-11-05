
/*********************************************************
Author: Sachin Mishra
Creation Date: 27 FEB 2015
Purpose: for deletion of company. 

**********************************************************/ 
 
CREATE PROCEDURE [dbo].[P_DeleteCompanyByIDForCH] (  
 @companyID int
)  
AS   
 declare @result int  

Declare @IsDelete int  
Declare @count int  
Set @count=0
Set @IsDelete=1  
  declare  @ErrorMessage varchar(500)
             --------------------------- Start: Check Dependent data -----------------------------------  

             ---- Check for Mapping Details ----
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyFunds where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with funds. Delete association first'
                 SET @IsDelete = 0
                 END
               END
--             IF @IsDelete = 1
--               BEGIN
--               select @count=Count(*) from T_CompanyThirdParty where CompanyID = @companyID 
--               IF @count > 0
--                 BEGIN
--                 select @ErrorMessage = 'Company associated with Third party. Delete association first'
--                 SET @IsDelete = 0
--                 END
--               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyUser where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with company user. Delete association first'
                 SET @IsDelete = 0
                 END
               END
--             IF @IsDelete = 1
--               BEGIN
--               select @count=Count(*) from T_CompanyCounterParties where CompanyID = @companyID 
--               IF @count > 0
--                 BEGIN
--                 select @ErrorMessage = 'Company associated with counter party. Delete association first'
--                 SET @IsDelete = 0
--                 END
--               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyFeederFunds where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with Feeder funds. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyMasterFunds where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with Master funds. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyTradingAccounts where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with Trading Accounts. Delete association first'
                 SET @IsDelete = 0
                 END
               END 
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyStrategy where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with strategies. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyMasterStrategy where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with master strategies. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_ImportFileSettings where CompanyID = @companyID and isactive = 1
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with batch. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             IF @IsDelete = 1
               BEGIN
               select @count=Count(*) from T_CompanyReleaseMapping where CompanyID = @companyID 
               IF @count > 0
                 BEGIN
                 select @ErrorMessage = 'Company associated with Release. Delete association first'
                 SET @IsDelete = 0
                 END
               END
             --IF @IsDelete = 1
             --BEGIN
               --select @count=Count(*) from T_CompanyAUEC where CompanyID = @companyID 
               --IF @count > 0
                -- BEGIN
                 -- @ErrorMessage = 'Company associated with Company AUEC. Delete association first'
                ---- SET @IsDelete = 0
                -- END
               --END--
             
             IF @IsDelete = 1
                BEGIN
                -- set isActive = 0 instead of company deletion

                Update T_Company SET IsActive=0 Where CompanyID = @companyID 
                select @ErrorMessage = 'Company deleted Successfully' 
                END

 select @ErrorMessage
