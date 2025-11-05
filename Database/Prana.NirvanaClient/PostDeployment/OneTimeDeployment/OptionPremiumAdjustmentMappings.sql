/*
	ScriptType: General
	Description: To add the 'OptionPremiumAdjustment' subaccount and its respective mappings
	Created By: Naresh Kumar Sharma
	Dated: 23 May'16
*/

IF NOT EXISTS (SELECT SubAccountID FROM T_SubAccounts WHERE [Name]='OptionPremiumAdjustment' and [Acronym] = 'OptionPremiumAdjustment')
BEGIN

-- Variables
DECLARE @SubAccountID INT
SET @SubAccountID = (SELECT MAX(SubAccountID) FROM T_SubAccounts) + 1

DECLARE @SubCategoryID INT
SET @SubCategoryID = (SELECT SubCategoryID FROM T_SubCategory WHERE SubCategoryName = 'Commissions And Fees')

DECLARE @TransactionTypeID INT
SET @TransactionTypeID = (SELECT TransactionTypeID FROM T_TransactionType WHERE TransactionType = 'Trade Transaction')

-- Inserting 'OptionPremiumAdjustment' a/c
INSERT INTO T_SubAccounts VALUES(@SubAccountID, 'OptionPremiumAdjustment', 'OptionPremiumAdjustment', @SubCategoryID, @TransactionTypeID, 1,null)

END

-- Updating Existed Mappings

UPDATE T_ActivityJournalMapping
SET DebitAccount = (SELECT SubAccountID FROM T_SubAccounts
					WHERE Acronym = 'OptionPremiumAdjustment')
WHERE AmountTypeid_fk IN 
(SELECT AmountTypeId FROM T_ActivityAmountType WHERE AmountType = 'OptionPremiumAdjustment')