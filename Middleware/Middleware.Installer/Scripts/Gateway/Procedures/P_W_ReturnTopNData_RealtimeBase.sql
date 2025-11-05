/****** Object:  StoredProcedure [dbo].[P_W_ReturnTopNData_RealtimeBase]    Script Date: 02/27/2014 10:05:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[P_W_ReturnTopNData_RealtimeBase] 
@GroupBy varchar(Max),@Client varchar(Max),@Entity varchar(Max),
@SortDirection varchar(max),@Content varchar(max),
@SymbolLevel varchar(max) = 'Symbol',
@NAVOnOff int = 1,
@ReportID Varchar(100) = 'Touch'
AS
BEGIN

Declare @SectorLevel_SubSectorOrSector int 
Select @SectorLevel_SubSectorOrSector = SectorLevel_SubSectorOrSector 
From T_W_BatchPreferences Where ReportID = @ReportID

Declare @IntradayExposure_NotDeltaAdjustedOrDeltaAdjusted bit  
Select @IntradayExposure_NotDeltaAdjustedOrDeltaAdjusted = IntradayExposure_NotDeltaAdjustedOrDeltaAdjusted From T_W_BatchPreferences Where ReportID = @ReportID 

Declare 
@NegativePerformanceBase_AbsoluteOrNull int
Select 
@NegativePerformanceBase_AbsoluteOrNull = NegativePerformanceBase_AbsoluteOrNull 
From T_W_BatchPreferences Where ReportID = @ReportID

Declare @Sql nvarchar(Max) 
Select @Sql = 
N'Select [Account],
[Symbol],
[Security Name],
[Asset Class],'+
(Case @SectorLevel_SubSectorOrSector When 0 Then '[Sub Sector]' Else '[Sector]' End)+',
[Closing Mark],'+
(Case When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Px Selected Feed (Local)') Then '[Px Selected Feed (Local)]' When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Px Last') Then '[Px Last]' Else 'Null' End)+',
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],'+
(Case When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Underlying Price') Then '[Underlying Price]' When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Underlying Price') Then '[Underlying Price]' Else 'Null' End)+',
[Delta Adj. Position],
[Day P&L (Base)],
[Market Value (Base)],'+
(Case @IntradayExposure_NotDeltaAdjustedOrDeltaAdjusted When 0 Then '[Market Value (Base)]' Else '[Net Exposure (Base)]' End)+',
[Cost Basis P&L (Base)],
[Beta Adj. Exposure (Base)] 
From T_PMDataDump Where CreatedOn =
(Select Max(CreatedOn) As Expr2
From T_PMDataDump
Where (CreatedOn <
(Select Max(CreatedOn) As Expr1
From T_PMDataDump)))' 

Create Table #DataDump_1 
([Fund] varchar(Max),
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[Closing Mark] varchar(Max),
[Px Last] varchar(Max),
[% Change] varchar(Max),
[Cost Basis] varchar(Max),
[Position] varchar(Max),
[Underlying Symbol] varchar(Max),
[Underlying Price] varchar(Max),
[Delta Adj Position] varchar(Max),
[Day P&L Base] varchar(Max),
[Market Value Base] varchar(Max),
[Exposure Base] varchar(Max),
[Cost Basis PNL Base] varchar(Max),
[Beta Adj Exposure (Base)] varchar(Max)) 

Insert Into #DataDump_1 
([Fund],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[Closing Mark],
[Px Last],
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],
[Underlying Price],
[Delta Adj Position],
[Day P&L Base],
[Market Value Base],
[Exposure Base],
[Cost Basis PNL Base],
[Beta Adj Exposure (Base)])
Exec sp_executesql @Sql 

Create Table #DataDump 
([Fund] varchar(Max),
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[Closing Mark] float,
[Px Last] float,
[% Change] float,
[Cost Basis] float,
[Position] float,
[Underlying Symbol] varchar(Max),
[Underlying Price] float,
[Delta Adj Position] float,
[Day P&L Base] float,
[Market Value Base] float,
[Exposure Base] float,
[Cost Basis PNL Base] float,
[Beta Adj Exposure (Base)] float) 

Insert Into #DataDump 
([Fund],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[Closing Mark],
[Px Last],
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],
[Underlying Price],
[Delta Adj Position],
[Day P&L Base],
[Market Value Base],
[Exposure Base],
[Cost Basis PNL Base],
[Beta Adj Exposure (Base)])
Select 
[Fund],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
Case When IsNumeric([Closing Mark]) = 1 And [Closing Mark] <> '-' Then Cast(Replace([Closing Mark],',','') As float) Else Null End,
Case When IsNumeric([Px Last]) = 1 And [Px Last] <> '-' Then Cast(Replace([Px Last],',','') As float) Else Null End,
Case When IsNumeric([% Change]) = 1 And [% Change] <> '-' Then Cast(Replace([% Change],',','') As float) Else Null End,
Case When IsNumeric([Cost Basis]) = 1 And [Cost Basis] <> '-' Then Cast(Replace([Cost Basis],',','') As float) Else Null End,
Case When IsNumeric([Position]) = 1 And [Position] <> '-' Then Cast(Replace([Position],',','') As float) Else Null End,
[Underlying Symbol],
Case When IsNumeric([Underlying Price]) = 1 And [Underlying Price] <> '-' Then Cast(Replace([Underlying Price],',','') As float) Else Null End,
Case When IsNumeric([Delta Adj Position]) = 1 And [Delta Adj Position] <> '-' Then Cast(Replace([Delta Adj Position],',','') As float) Else Null End,
Case When IsNumeric([Day P&L Base]) = 1 And [Day P&L Base] <> '-' Then Cast(Replace([Day P&L Base],',','') As float) Else Null End,
Case When IsNumeric([Market Value Base]) = 1 And [Market Value Base] <> '-' Then Cast(Replace([Market Value Base],',','') As float) Else Null End,
Case When IsNumeric([Exposure Base]) = 1 And [Exposure Base] <> '-' Then Cast(Replace([Exposure Base],',','') As float) Else Null End,
Case When IsNumeric([Cost Basis PNL Base]) = 1 And [Cost Basis PNL Base] <> '-' Then Cast(Replace([Cost Basis PNL Base],',','') As float) Else Null End,
Case When IsNumeric([Beta Adj Exposure (Base)]) = 1 And [Beta Adj Exposure (Base)] <> '-' Then Cast(Replace([Beta Adj Exposure (Base)],',','') As float) Else Null End
From #DataDump_1

Create Table #ClientFundValue
([ClientName] varchar(Max),
[FundName] varchar(Max),
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[Closing Mark] float,
[Px Last] float,
[% Change] float,
[Cost Basis] float,
[Position] float,
[Underlying Symbol] varchar(Max),
[Underlying Price] float,
[Delta Adj Position] float,
[Day P&L Base] float,
[Market Value Base] float,
[Exposure Base] float,
[Cost Basis PNL Base] float,
[Beta Adj Exposure (Base)] float)

If @GroupBy = 'Aggregate' 
Begin 
	Insert Into #ClientFundValue
	([ClientName],
	[FundName],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)])
	Select 
	C.[ClientName],
	CF.[FundName],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)]
	From T_W_Clients As C Inner Join
	T_W_ClientFundMapping As M On M.ClientID = C.ClientID Inner Join
	T_W_Funds As F On F.TouchFundID = M.TouchFundID Inner Join
	T_CompanyFunds As CF On CF.CompanyFundID = F.PranaFundID Left Outer Join
	#DataDump As D On D.Fund = CF.FundName 
	Where C.ClientName = @Client
End 
Else If @GroupBy = 'Fund' 
Begin 
	Insert Into #ClientFundValue
	([ClientName],
	[FundName],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)])
	Select 
	C.[ClientName],
	CF.[FundName],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)]
	From T_W_Clients As C Inner Join
	T_W_ClientFundMapping As M On M.ClientID = C.ClientID Inner Join
	T_W_Funds As F On F.TouchFundID = M.TouchFundID Inner Join
	T_CompanyFunds As CF On CF.CompanyFundID = F.PranaFundID Left Outer Join
	#DataDump As D On D.Fund = CF.FundName 
	Where C.ClientName = @Client And CF.FundName = @Entity
End

Create Table #EntitySymbolValueTemp(GroupBy varchar(Max),Client varchar(Max),Entity varchar(Max),
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[Closing Mark] float,
[Px Last] float,
[% Change] float,
[Cost Basis] float,
[Position] float,
[Underlying Symbol] varchar(Max),
[Underlying Price] float,
[Delta Adj Position] float,
[Day P&L Base] float,
[Market Value Base] float,
[Exposure Base] float,
[Cost Basis PNL Base] float,
[Beta Adj Exposure (Base)] float)

Insert Into #EntitySymbolValueTemp 
(GroupBy,Client,Entity,
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[Closing Mark],
[Px Last],
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],
[Underlying Price],
[Delta Adj Position],
[Day P&L Base],
[Market Value Base],
[Exposure Base],
[Cost Basis PNL Base],
[Beta Adj Exposure (Base)])
Select 'Fund',ClientName,FundName,
Symbol,[Security Name],[Asset Class],[Sector],
Max([Closing Mark]),Max([Px Last]),Max([% Change]),Sum([Position]*[Cost Basis])/NullIf(Sum([Position]),0),Sum([Position]),
[Underlying Symbol],Max([Underlying Price]),Sum([Delta Adj Position]),
Sum([Day P&L Base]),Sum([Market Value Base]),Sum([Exposure Base]),Sum([Cost Basis PNL Base]),Sum([Beta Adj Exposure (Base)]) 
From #ClientFundValue Group By ClientName,FundName,Symbol,[Security Name],[Asset Class],[Sector],[Underlying Symbol]

If @GroupBy = 'Aggregate' 
Begin 
	Insert Into #EntitySymbolValueTemp 
	(GroupBy,Client,Entity,
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)])
	Select 'Aggregate',Client,Client,
	Symbol,[Security Name],[Asset Class],[Sector],
	Max([Closing Mark]),Max([Px Last]),Max([% Change]),Sum([Position]*[Cost Basis])/NullIf(Sum([Position]),0),Sum([Position]),
	[Underlying Symbol],Max([Underlying Price]),Sum([Delta Adj Position]),
	Sum([Day P&L Base]),Sum([Market Value Base]),Sum([Exposure Base]),Sum([Cost Basis PNL Base]),Sum([Beta Adj Exposure (Base)]) 
	From #EntitySymbolValueTemp Where GroupBy = 'Fund' Group By Client,Symbol,[Security Name],[Asset Class],[Sector],[Underlying Symbol]
End 

Alter Table #EntitySymbolValueTemp Add PreviousNAV float,PreviousCash float,PreviousPosition float,PreviousDeltaAdjPosition float 

Update D Set D.PreviousNAV = S.NAV,D.PreviousCash = S.Cash 
From #EntitySymbolValueTemp D Left Outer Join [dbo].[T_W_RiskExp] S On D.GroupBy = S.GroupBy And D.Client = S.Client And D.Entity = S.Entity 
Where S.AlignDataType = 'Delta Exposure'

Update D Set D.PreviousPosition = S.Position,D.PreviousDeltaAdjPosition = S.DeltaAdjPosition 
From #EntitySymbolValueTemp D Left Outer Join [dbo].[T_W_HoldingSymbolGroup] S On D.GroupBy = S.GroupBy And D.Client = S.Client And D.Entity = S.Entity And D.[Symbol] = S.Symbol And D.[Underlying Symbol] = S.UnderlyingSymbol 
Where S.TimeFrame = 'Day' 

Create Table #EntitySymbolValue(GroupBy varchar(Max),Client varchar(Max),Entity varchar(Max),
[Entity NAV] float,
[Entity Start of Day NAV] float,
[Entity P&L] float,
[Entity Market Value] float,
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[Closing Mark] float,
[Px Last] float,
[% Change] float,
[Cost Basis] float,
[Position] float,
[Underlying Symbol] varchar(Max),
[Underlying Price] float,
[Delta Adj Position] float,
[Day P&L Base] float,
[Market Value Base] float,
[Exposure Base] float,
[Cost Basis PNL Base] float,
[Beta Adj Exposure (Base)] float,
PreviousPosition float,
PreviousDeltaAdjPosition float)

Declare @IntradayNAV_AnalyticsOrDataDumpOrGrossExposure int 
Select @IntradayNAV_AnalyticsOrDataDumpOrGrossExposure = IntradayNAV_AnalyticsOrDataDumpOrGrossExposure From T_W_BatchPreferences Where ReportID = @ReportID 

-- Analytics
If @IntradayNAV_AnalyticsOrDataDumpOrGrossExposure = 0 
Begin
	Insert Into #EntitySymbolValue
	(GroupBy,Client,Entity,
	[Entity NAV],
	[Entity Start of Day NAV],
	[Entity P&L],
	[Entity Market Value],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition)
	Select GroupBy,Client,Entity,
	IsNull(PreviousNAV,0),
	IsNull(PreviousNAV,0),
	Sum(IsNull([Day P&L Base],0)) Over(Partition By GroupBy,Client,Entity),
	Sum(IsNull([Market Value Base],0)) Over(Partition By GroupBy,Client,Entity),
	Symbol,
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition 
	From #EntitySymbolValueTemp
End
-- DataDump
Else If @IntradayNAV_AnalyticsOrDataDumpOrGrossExposure = 1 
Begin
	Insert Into #EntitySymbolValue
	(GroupBy,Client,Entity,
	[Entity NAV],
	[Entity Start of Day NAV],
	[Entity P&L],
	[Entity Market Value],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition)
	Select GroupBy,Client,Entity,
	IsNull(PreviousNAV,0)+Sum(IsNull([Day P&L Base],0)) Over(Partition By GroupBy,Client,Entity),
	IsNull(PreviousNAV,0),
	Sum(IsNull([Day P&L Base],0)) Over(Partition By GroupBy,Client,Entity),
	Sum(IsNull([Market Value Base],0)) Over(Partition By GroupBy,Client,Entity),
	Symbol,
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition 
	From #EntitySymbolValueTemp

	Create Table #ClientNAV(Client varchar(Max),[Entity NAV] float,[Entity Start of Day NAV] float)
	Insert Into #ClientNAV(Client,[Entity NAV],[Entity Start of Day NAV])
	Select Client,Sum([Entity NAV]),Sum([Entity Start of Day NAV]) From (Select Client,Entity,Max([Entity NAV]) As [Entity NAV],Max([Entity Start of Day NAV]) As [Entity Start of Day NAV] From #EntitySymbolValue Where GroupBy = 'Fund' Group By Client,Entity) S  Group By Client

	Update D Set [Entity NAV] = S.[Entity NAV],[Entity Start of Day NAV] = S.[Entity Start of Day NAV] From #EntitySymbolValue D Left Outer Join #ClientNAV S On D.Client=S.Client
	Where D.GroupBy = 'Aggregate' 
	
	Drop Table #ClientNAV 
End
-- GrossExposure
Else If @IntradayNAV_AnalyticsOrDataDumpOrGrossExposure = 2 
Begin
	Insert Into #EntitySymbolValue
	(GroupBy,Client,Entity,
	[Entity NAV],
	[Entity Start of Day NAV],
	[Entity P&L],
	[Entity Market Value],
	[Symbol],
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition)
	Select GroupBy,Client,Entity,
	Sum(Abs(IsNull([Exposure Base],0))) Over(Partition By GroupBy,Client,Entity),
	Sum(Abs(IsNull([Exposure Base],0))) Over(Partition By GroupBy,Client,Entity),
	Sum(IsNull([Day P&L Base],0)) Over(Partition By GroupBy,Client,Entity),
	Sum(IsNull([Market Value Base],0)) Over(Partition By GroupBy,Client,Entity),
	Symbol,
	[Security Name],
	[Asset Class],
	[Sector],
	[Closing Mark],
	[Px Last],
	[% Change],
	[Cost Basis],
	[Position],
	[Underlying Symbol],
	[Underlying Price],
	[Delta Adj Position],
	[Day P&L Base],
	[Market Value Base],
	[Exposure Base],
	[Cost Basis PNL Base],
	[Beta Adj Exposure (Base)],
	PreviousPosition,
	PreviousDeltaAdjPosition 
	From #EntitySymbolValueTemp
End 

Declare @PNLContribution_PNLOrACB int  
Select @PNLContribution_PNLOrACB = PNLContribution_PNLOrACB From T_W_BatchPreferences Where ReportID = @ReportID 

Create Table #AllPositions
(GroupBy varchar(max),Client varchar(Max),Entity varchar(Max),Symbol varchar(max),[% Change] float,[Exposure Base] float,[Day P&L Base] float,[% Contribution] float)

If @SymbolLevel = 'Symbol'
Begin
	Insert Into #AllPositions(GroupBy,Client,Entity,Symbol,[% Change],[Exposure Base],[Day P&L Base],[% Contribution])
	Select GroupBy,Client,Entity,Symbol,[% Change],[Exposure Base],[Day P&L Base],100*[Day P&L Base]/NullIf(Case @PNLContribution_PNLOrACB When 1 Then Case Sign([Entity Start of Day NAV]) When 1 Then [Entity Start of Day NAV] When -1 Then (Case @NegativePerformanceBase_AbsoluteOrNull When 0 Then -[Entity Start of Day NAV] Else Null End) Else Null End Else [Entity P&L] End,0) From #EntitySymbolValue Where GroupBy = @GroupBy And Symbol Is Not Null
End
Else If @SymbolLevel = 'UnderlyingSymbol'
Begin
	Insert Into #AllPositions(GroupBy,Client,Entity,Symbol,[% Change],[Exposure Base],[Day P&L Base],[% Contribution])
	Select GroupBy,Client,Entity,[Underlying Symbol],Max([% Change]),Sum([Exposure Base]),Sum([Day P&L Base]),Sum(100*[Day P&L Base]/NullIf(Case @PNLContribution_PNLOrACB When 1 Then Case Sign([Entity Start of Day NAV]) When 1 Then [Entity Start of Day NAV] When -1 Then (Case @NegativePerformanceBase_AbsoluteOrNull When 0 Then -[Entity Start of Day NAV] Else Null End) Else Null End Else [Entity P&L] End,0)) From #EntitySymbolValue Where GroupBy = @GroupBy And [Underlying Symbol] Is Not Null Group By GroupBy,Client,Entity,[Underlying Symbol]
End

If @Content = 'Exposure Base' And @SortDirection = 'Desc'
Begin	
	Select Top 10 [Symbol],[Exposure Base],[% Change],[% Contribution] From #AllPositions Where [Exposure Base] > 0 Order By [Exposure Base] Desc
End

If @Content = 'Exposure Base' And @SortDirection = 'Asc'
Begin	
	Select Top 10 [Symbol],[Exposure Base],[% Change],[% Contribution] From #AllPositions Where [Exposure Base] < 0 Order By [Exposure Base] Asc
End

If @Content = 'Day P&L Base' And @SortDirection = 'Desc'
Begin 
	Select Top 10 [Symbol],[Day P&L Base],[% Change],[% Contribution] From #AllPositions Where [Day P&L Base] > 0 Order By [Day P&L Base] Desc
End

If @Content = 'Day P&L Base' And @SortDirection = 'Asc'
Begin	
	Select Top 10 [Symbol],[Day P&L Base],[% Change],[% Contribution] From #AllPositions Where [Day P&L Base] < 0 Order By [Day P&L Base] Asc
End

Drop Table #DataDump_1,#DataDump
Drop Table #ClientFundValue,#EntitySymbolValueTemp,#EntitySymbolValue 
Drop Table #AllPositions

END