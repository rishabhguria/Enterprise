CREATE TABLE [dbo].[T_RMAUECs] (
    [RM_AUEC_ID]                        INT          IDENTITY (1, 1) NOT NULL,
    [AUECID]                            INT          NOT NULL,
    [AUEC_ExposureLimit_RMBaseCurrency] DECIMAL (18) NOT NULL,
    [AUEC_ExposureLimit_BaseCurrency]   DECIMAL (18) NOT NULL,
    [Maximum_PNLLoss_RMBaseCurrency]    DECIMAL (18) NOT NULL,
    [Maximum_PNLLoss_BaseCurrency]      DECIMAL (18) NOT NULL,
    [CompanyID]                         INT          NOT NULL,
    CONSTRAINT [PK_T_RMAUECs] PRIMARY KEY CLUSTERED ([RM_AUEC_ID] ASC)
);

