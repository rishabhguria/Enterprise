

CREATE PROCEDURE [dbo].[P_DeleteCompanyByID] (
	@companyID INT,
	@deleteForceFully INT
	)
AS
DECLARE @result INT
DECLARE @IsDelete INT
DECLARE @ErrorMessage varchar(500)
DECLARE @total INT
DECLARE @count INT

SET @count = 0
SET @result = - 1
Set @IsDelete=1
 
/* --Delete Corresponding Company Details     
 if ( @deleteForceFully = 1)    
  begin     
   -- If Company is referenced anywhere and still we want to delete it.    
   --Delete Company and related information.    
   --------------------------- Start: Delete Dependent data -----------------------------------    
     
   ---- Start : Delete Company User dependent data ----------    
   Delete T_CompanyUserTradingAccounts    
   Where CompanyUserID IN (Select UserID From T_CompanyUser Where CompanyID = @companyID)    
       
   Delete T_CompanyUserModule    
   Where CompanyUserID IN (Select UserID From T_CompanyUser Where CompanyID = @companyID)    
       
   Delete T_CompanyUserCounterPartyVenues    
   Where CompanyUserID IN (Select UserID From T_CompanyUser Where CompanyID = @companyID)    
       
   exec P_DeleteCompanyUser @companyID    
       
   ---- End : Delete Company User dependent data ----------    
       
   ---- Start : Delete Company Client dependent data ----------    
       
   Delete T_CompanyClientTrader    
   Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where CompanyID = @companyID)    
       
   Delete T_CompanyClientFund    
   Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where CompanyID = @companyID)    
       
   Delete T_CompanyClient    
   Where CompanyID = @companyID    
       
   ---- End : Delete Company Client dependent data ----------    
       
   ---- Start : Delete Company Counterparty ----------    
   Delete T_CompanyCounterParties    
   Where CompanyID = @companyID     
   ---- End : Delete Company Counterparty ----------    
       
   ---- Start : Delete Company Counterpartyvenues ----------    
   Delete T_CompanyCounterPartyVenues    
   Where CompanyID = @companyID     
   ---- End : Delete Company Counterpartyvenues ----------    
        
   --------------------------- End: Delete Dependent data -----------------------------------    
       
   exec P_DeleteCompanyCounterParties @companyID    
   exec P_DeleteCompanyThirdParty @companyID    
   exec P_DeleteCompanyModules @companyID    
    
   exec P_DeleteCompanyFunds @companyID    
   exec P_DeleteCompanyStrategies @companyID    
   exec P_DeleteCompanyTradingAccounts @companyID    
   exec P_DeleteCompanyClearingFirmsPrimeBrokers @companyID    
       
   exec P_DeleteCompanyCompliance @companyID    
   exec P_DeleteCompanyBorrower @companyID    
    
   Delete T_Company    
   Where CompanyID = @companyID    
  end    
     
 else    
 begin */
 


IF (@deleteForceFully = 1)
BEGIN
	SELECT @count = Count(1)
	FROM PM_Company
	WHERE NOMSCompanyID = @companyID

	IF (@count = 0)
	BEGIN
		SELECT @total = Count(1)
		FROM T_RMCompanyOverallLimit
		WHERE CompanyID = @companyID

		IF (@total = 0)
		BEGIN
			SELECT @total = Count(1)
			FROM T_RMCompanyUsersOverall
			WHERE CompanyID = @companyID

			IF (@total = 0)
			BEGIN
				SELECT @total = Count(1)
				FROM T_RMCompanyUsersOverall
				WHERE CompanyID = @companyID

				IF (@total = 0)
				BEGIN
					SELECT @total = Count(1)
					FROM T_RMCompanyUsersOverall
					WHERE CompanyID = @companyID

					IF (@total = 0)
					BEGIN
						SELECT @total = Count(1)
						FROM T_RMCompanyUsersOverall
						WHERE CompanyID = @companyID

						IF (@total = 0)
						BEGIN
							SELECT @total = Count(1)
							FROM T_RMCompanyUsersOverall
							WHERE CompanyID = @companyID

							IF (@total = 0)
							BEGIN
								SELECT @total = Count(1)
								FROM T_RMCompanyUsersOverall
								WHERE CompanyID = @companyID

								IF (@total = 0)
								BEGIN
									-- If Company is not referenced anywhere.    
									--Delete Company.    
									--------------------------- Start: Delete Dependent data -----------------------------------    
									---- Start : Delete Company ThirdParties dependent data ----------    
									---- Delete CommissionRules Details ----    
									DELETE T_CompanyThirdPartyCVCommissionRules
									WHERE CompanyFundID IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyCVCommissionRules
									WHERE CompanyCounterPartyCVID IN (
											SELECT CompanyCounterPartyCVID
											FROM T_CompanyCounterPartyVenues
											WHERE CompanyID = @companyID
											)

									---- Delete CVIdentifier Details ----    
									DELETE T_CompanyThirdPartyCVIdentifier
									WHERE CompanyThirdPartyID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyCVIdentifier
									WHERE CompanyCounterPartyVenueID_FK IN (
											SELECT CompanyCounterPartyCVID
											FROM T_CompanyCounterPartyVenues
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyFlatFileSaveDetails
									WHERE CompanyThirdPartyID IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									---- Delete FileFormats Details ----    
									DELETE T_CompanyThirdPartyFileFormats
									WHERE CompanyFundID_FK IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyFileFormats
									WHERE CompanyPrimeBrokerClearerID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyFileFormats
									WHERE CompanyCustodianID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyFileFormats
									WHERE CompanyAdministratorID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									---- Delete Mapping Details ----    
									DELETE T_CompanyThirdPartyMappingDetails
									WHERE CompanyThirdPartyID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyThirdPartyMappingDetails
									WHERE InternalFundNameID_FK IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_ThirdPartyFFRunFundDetails
									WHERE CompanyFundID IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_ThirdPartyPermittedFunds
									WHERE CopanyFundID IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_ThirdPartyFFRunFunds
									WHERE CompanyFundID IN (
											SELECT CompanyFundID
											FROM T_CompanyFunds
											WHERE CompanyID = @companyID
											)

									DELETE T_ThirdPartyFFRunFunds
									WHERE CompanyThirdPartyID_FK IN (
											SELECT CompanyThirdPartyID
											FROM T_CompanyThirdParty
											WHERE CompanyID = @companyID
											)

									DELETE T_ThirdPartyFFRunReport
									WHERE CompanyID = @companyID

									-- Delete T_CompanyThirdPartyMappingDetails    
									-- Where SubAccountofID_FK IN (Select CompanyFundID From T_CompanyFunds Where CompanyID = @companyID)    
									---- Start : Delete Company User dependent data ----------    
									DELETE T_CompanyUserTradingAccounts
									WHERE CompanyUserID IN (
											SELECT UserID
											FROM T_CompanyUser
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyUserModule
									WHERE CompanyUserID IN (
											SELECT UserID
											FROM T_CompanyUser
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyUserCounterPartyVenues
									WHERE CompanyUserID IN (
											SELECT UserID
											FROM T_CompanyUser
											WHERE CompanyID = @companyID
											)

									EXEC P_DeleteCompanyUsers @companyID

									---- End : Delete Company User dependent data ----------    
									---- Start : Delete Company Client dependent data ----------    
									DELETE T_CompanyClientTradingAccount
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientTrader
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientFund
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientAUEC
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientClearer
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientFix
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClientIdentifier
									WHERE CompanyClientID IN (
											SELECT CompanyClientID
											FROM T_CompanyClient
											WHERE CompanyID = @companyID
											)

									DELETE T_CompanyClient
									WHERE CompanyID = @companyID

									---- End : Delete Company Client dependent data ----------    
									---- Start : Delete Company Counterparty ----------    
									DELETE T_CompanyCounterParties
									WHERE CompanyID = @companyID

									---- End : Delete Company Counterparty ----------    
									---- Start : Delete Company Counterpartyvenues ----------    
									DELETE T_CompanyCounterPartyVenues
									WHERE CompanyID = @companyID

									---- End : Delete Company Counterpartyvenues ----------    
									--------------------------- End: Delete Dependent data -----------------------------------    
									---- Start : Delete Company Counterpartyvenue Details ----------   
									DELETE T_CompanyCVCMTAIdentifier
									WHERE (
											CompanyCounterPartyVenueID IN (
												SELECT CompanyCounterPartyVenueID
												FROM T_CompanyCounterPartyVenues
												WHERE CompanyID = @companyID
												)
											)

									DELETE T_CompanyCVGiveUpIdentifier
									WHERE (
											CompanyCounterPartyVenueID IN (
												SELECT CompanyCounterPartyVenueID
												FROM T_CompanyCounterPartyVenues
												WHERE CompanyID = @companyID
												)
											)

									DELETE T_CompanyCounterPartyVenueDetails
									WHERE (
											CompanyCounterPartyVenueID IN (
												SELECT CompanyCounterPartyVenueID
												FROM T_CompanyCounterPartyVenues
												WHERE CompanyID = @companyID
												)
											)

									EXEC P_DeleteCompanyCounterParties @companyID

									EXEC P_DeleteCompanyThirdParty @companyID

									--exec P_DeleteCompanyModules @companyID  --  
									DELETE T_CompanyModule
									WHERE CompanyID = @companyID

									EXEC P_DeleteCompanyFunds @companyID

									EXEC P_DeleteCompanyStrategies @companyID

									--exec P_DeleteCompanyTradingAccounts @companyID  --  
									DELETE T_CompanyTradingAccounts
									WHERE CompanyID = @companyID

									EXEC P_DeleteCompanyClearingFirmsPrimeBrokers @companyID

									EXEC P_DeleteCompanyAUECs @companyID

									EXEC P_DeleteCompanyCompliance @companyID

									EXEC P_DeleteCompanyBorrower @companyID

									EXEC P_DeleteCompanyComplianceCurrencies @companyID

									--Deleting Company Contracts  
									DELETE T_SetUpContracts
									WHERE CompanyID = @companyID

									--Deleting Company mpid  
									DELETE T_CompanyMPID
									WHERE CompanyID = @companyID

									DELETE T_Company
									WHERE CompanyID = @companyID

									SET @result = 1
								END
							END
						END
					END
				END
			END
					--end    
			ELSE
			BEGIN
				SET @result = - 1
			END
		END
	END
	ELSE
	BEGIN
		SET @result = - 1
	END
	SELECT @result
END
ELSE
BEGIN
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

END
