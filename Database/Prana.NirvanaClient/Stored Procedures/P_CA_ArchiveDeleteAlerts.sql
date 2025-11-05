CREATE PROCEDURE P_CA_ArchiveDeleteAlerts
	@Alerts nvarchar(max), -- CSV values of all candidate keys to be deleted, key given by RuleId+Dimension+validationTime
	@DeletOrArchive int, -- 0 is delete || 1 is archive
	@NoOfRows INT OUTPUT
AS

--------------------------------------------------------------------------
---- insert the alerts into a temp table
--------------------------------------------------------------------------
select * into #TempAlerts from [dbo].[split](@alerts,',')
 

--------------------------------------------------------------------------
---- Copy to #TempAlertsToBeDeleted along with the value deleted or archived. 
--------------------------------------------------------------------------
select
		a.Id,
		a.UserId,
		a.RuleType,
		a.Summary,
		a.CompressionLevel,
		a.Parameters,
		a.OrderId,
		a.Status,
		a.ValidationTime,
		a.RuleId,
		a.Description,
		a.Dimension,
		GETDATE() as ArchiveDate,
		@DeletOrArchive as ArchiveType,
		a.PreTradeType as PreTradeType,
		a.ActionUser as ActionUser,
		a.PreTradeActionType as PreTradeActionType,
		a.ComplianceOfficerNotes as ComplianceOfficerNotes,
		a.UserNotes as UserNotes,
		a.TradeDetails as TradeDetails
	into #TempAlertsToBeDeleted
	from T_CA_AlertHistory as a left outer join
	T_CA_RulesUserDefined as r on a.RuleId = r.RuleId
	where (coalesce(r.RuleName,'N/A') + coalesce(a.Dimension,'') + convert(varchar(50),a.validationTime,20)) in (select * from #TempAlerts)

--------------------------------------------------------------------------
---- Insert into backup table
--------------------------------------------------------------------------
insert into T_CA_ArchivedAlertHistory
select
	UserId,
	RuleType,
	Summary,
	CompressionLevel,
	Parameters,
	OrderId,
	Status,
	ValidationTime,
	RuleId,
	Description,
	Dimension,
	ArchiveDate,
	ArchiveType,
	PreTradeType,
	ActionUser,
	PreTradeActionType,
	ComplianceOfficerNotes,
	UserNotes,
	TradeDetails
from #TempAlertsToBeDeleted


set @NoOfRows = @@ROWCOUNT
--------------------------------------------------------------------------
---- delete all the alerts in given CSV from main alert history table
--------------------------------------------------------------------------
delete from T_CA_AlertHistory  
	where id in (select Id from #TempAlertsToBeDeleted)


--------------------------------------------------------------------------
---- drop temp tables
--------------------------------------------------------------------------
Drop Table #TempAlerts
Drop Table #TempAlertsToBeDeleted

