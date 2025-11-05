Create PROCEDURE [dbo].[P_CA_SaveAlertHistory]
(
@RuleId varchar(50),
--@RuleName varchar(100),
@UserId int,
@RuleType varchar(100),
@Summary varchar(1000),
@CompressionLevel varchar(1000),
@Parameters varchar(1000),
@ValidationTime DateTime,
@OrderId varchar(500),
@Status varchar(50),
@Description varchar(1000),
@Dimension varchar(50),
@PreTradeType varchar(50),
@ActionUser varchar(50),
@PreTradeActionType varchar(50),
@ComplianceOfficerNotes varchar(max),
@UserNotes varchar(max),
@TradeDetails varchar(max),
@AlertPopUpResponse int
)
AS

INSERT INTO T_CA_AlertHistory
           (
			RuleId,
			--RuleName,
			UserId,
			RuleType,
			Summary,
			CompressionLevel,
			Parameters,
			OrderId,
			ValidationTime,
			Status,
			Description,
			Dimension,
			PreTradeType,
			ActionUser,
			PreTradeActionType,
			ComplianceOfficerNotes,
			UserNotes,
			TradeDetails,
			AlertPopUpResponse
            )
     VALUES
           (
			@RuleId,
			--@RuleName,
			@UserId,
			@RuleType,
			@Summary,
			@CompressionLevel,
			@Parameters,
			@OrderId,
			@ValidationTime,
			@Status,
			@Description,
			@Dimension,
			@PreTradeType,
			@ActionUser,
			@PreTradeActionType,
			@ComplianceOfficerNotes,
			@UserNotes,
			@TradeDetails,
			@AlertPopUpResponse
            )

