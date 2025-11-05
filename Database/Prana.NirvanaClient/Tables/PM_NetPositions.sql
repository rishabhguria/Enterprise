CREATE TABLE [dbo].[PM_NetPositions] (
    [PositionId]                 UNIQUEIDENTIFIER NOT NULL,
    [ApplicationTaxLotID]        VARCHAR (50)     NULL,
    [ThirdPartyID]               INT              NULL,
    [FundID]                     INT              NULL,
    [Symbol]                     VARCHAR (100)    NULL,
    [AUECID]                     INT              NULL,
    [PositionType]               INT              NULL,
    [PositionStartDate]          DATETIME         NULL,
    [CostBasis]                  FLOAT (53)       NULL,
    [StartQuantity]              INT              NULL,
    [NetPosition]                INT              CONSTRAINT [DF_PM_NetPositions_NetPosition] DEFAULT ((0)) NULL,
    [Commision]                  FLOAT (53)       NULL,
    [Fees]                       FLOAT (53)       NULL,
    [SecFess]                    FLOAT (53)       NULL,
    [LastCloseTradeTime]         DATETIME         NULL,
    [CreatedByID]                INT              NULL,
    [DateCreated]                DATETIME         NULL,
    [ModifiedAt]                 DATETIME         NULL,
    [State]                      INT              NULL,
    [Status]                     INT              NULL,
    [RealizedPNL]                FLOAT (53)       CONSTRAINT [DF_PM_NetPositions_RealizedPNL] DEFAULT ((0)) NULL,
    [FundName]                   VARCHAR (100)    NULL,
    [Multiplier]                 FLOAT (53)       CONSTRAINT [DF_PM_NetPositions_Multiplier] DEFAULT ((1)) NULL,
    [DerivativeRootSymbol]       VARCHAR (50)     NULL,
    [DerivativeUnderlyingSymbol] VARCHAR (50)     NULL,
    [Cusip]                      VARCHAR (50)     NULL,
    [StrikePrice]                FLOAT (53)       NULL,
    [Call_Put]                   VARCHAR (5)      NULL,
    [ExpirationDate]             DATETIME         NULL,
    [AssetID]                    INT              NULL,
    [AssetUnderlyingID]          INT              NULL,
    [IsActive]                   BIT              CONSTRAINT [DF__PM_NetPos__IsAct__04C657A2] DEFAULT ((1)) NULL,
    [IsManuallyCreatedPosition]  BIT              CONSTRAINT [DF__PM_NetPos__IsMan__243F02FB] DEFAULT ((0)) NULL,
    [MonthMarkPrice]             FLOAT (53)       CONSTRAINT [DF_PM_NetPositions_MonthMarkPrice] DEFAULT ((0.00)) NULL,
    [MonthsProfit]               FLOAT (53)       CONSTRAINT [DF_PM_NetPositions_MonthsProfit] DEFAULT ((0)) NULL,
    [SymbolAvgPrice]             FLOAT (53)       NULL,
    [RealizedPLAvgPrice]         FLOAT (53)       NULL,
    [UnRealizedPNL]              FLOAT (53)       NULL,
    CONSTRAINT [PK_PM_NetPositions_1] PRIMARY KEY CLUSTERED ([PositionId] ASC)
);


GO

-- Listing 8-6: Creation Script for the trg_T1_U_Audit Trigger  
CREATE TRIGGER [PM_NetPositionsAudit] ON dbo.PM_NetPositions
FOR INSERT
	,UPDATE
AS
-- If 0 affected rows, do nothing  
IF @@rowcount = 0
	RETURN;

IF EXISTS (
		SELECT *
		FROM inserted
		)
BEGIN
	IF EXISTS (
			SELECT *
			FROM deleted
			)
	BEGIN
		DECLARE @FundName AS VARCHAR(100)
			,@FundID INT
			,@ThirdPartyID INT;

		DECLARE Funds CURSOR FAST_FORWARD
		FOR
		SELECT FundShortName
			,CompanyFundID
			,CompanyThirdPartyID
		FROM dbo.T_CompanyFunds;

		OPEN Funds;

		FETCH NEXT
		FROM Funds
		INTO @FundName
			,@FundID
			,@ThirdPartyID;

		WHILE @@fetch_status = 0
		BEGIN
			UPDATE dbo.PM_NetPositions
			SET dbo.PM_NetPositions.FundID = @FundID
				,dbo.PM_NetPositions.ThirdPartyID = @ThirdPartyID
				,dbo.PM_NetPositions.PositionType = CASE 
					WHEN PMN.NetPosition >= 0
						THEN 0
					ELSE 1
					END
				,dbo.PM_NetPositions.NetPosition = CASE 
					WHEN PMN.NetPosition >= 0
						THEN PMN.NetPosition
					ELSE - PMN.NetPosition
					END
			FROM dbo.PM_NetPositions PMN
			INNER JOIN inserted AS I ON PMN.PositionID = I.PositionID
			WHERE PMN.FundName = @FundName

			FETCH NEXT
			FROM Funds
			INTO @FundName
				,@FundID
				,@ThirdPartyID;
		END

		CLOSE Funds;

		DEALLOCATE Funds;
	END
END
ELSE
BEGIN
	PRINT 'Delete Identified'
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Long[1] or Short[0]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_NetPositions', @level2type = N'COLUMN', @level2name = N'PositionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirty or clean', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_NetPositions', @level2type = N'COLUMN', @level2name = N'State';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Open or Close', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_NetPositions', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The PNL generated as per the remaining quantity in the Position.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_NetPositions', @level2type = N'COLUMN', @level2name = N'RealizedPNL';

