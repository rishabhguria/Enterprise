GO
/****** Object:  StoredProcedure [dbo].[P_NT_RealtimeBase]    Script Date: 05/13/2015 16:36:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
--Exec [P_NT_RealtimeBase]
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RealtimeBase]
-- Add the parameters for the stored procedure here
AS
BEGIN

--Set NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;

Declare @Sql nvarchar(Max) 
Select @Sql = 
N'Select [Account],
[NAV (Touch)],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[Sub Sector],
[Country],
[Trade Currency],
[Closing Mark],
[Px Selected Feed (Local)],
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],'+
(Case When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Underlying Price') Then '[Underlying Price]' When Exists (Select Object_Id From Sys.Columns Where Object_Id = Object_Id('dbo.T_PMDataDump','U') And Name = 'Underlying Price') Then '[Underlying Price]' Else 'Null' End)+',
[Delta Adj. Position],
[Day P&L (Base)],
[Market Value (Base)],
[Net Exposure (Base)],
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
[Fund NAV] varchar(Max),
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[SubSector] varchar(Max),
[Country] varchar(Max),
[Trade Currency] varchar(Max),
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
[Fund NAV],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[SubSector],
[Country],
[Trade Currency],
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
[Fund NAV] float,
[Symbol] varchar(Max),
[Security Name] varchar(Max),
[Bloomberg Symbol] varchar(Max),
[Asset Class] varchar(Max),
[Sector] varchar(Max),
[SubSector] varchar(Max),
[Country] varchar(Max),
[Trade Currency] varchar(Max),
[Closing Mark] float,
[Px Last] float,
[% Change] float,
[Cost Basis] float,
[Position] float,
[Underlying Symbol] varchar(Max),
[Underlying Security Name] varchar(Max),
[Underlying Price] float,
[Underlying Closing Mark] float,
[Delta Adj Position] float,
[Day P&L Base] float,
[Market Value Base] float,
[Exposure Base] float,
[Cost Basis PNL Base] float,
[Beta Adj Exposure (Base)] float) 

Insert Into #DataDump 
([Fund],
[Fund NAV],
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[SubSector],
[Country],
[Trade Currency],
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
Case When IsNumeric([Fund NAV]) = 1 And [Fund NAV] <> '-' Then Cast(Replace([Fund NAV],',','') As float) Else Null End,
[Symbol],
[Security Name],
[Asset Class],
[Sector],
[SubSector],
[Country],
[Trade Currency],
Case When IsNumeric([Closing Mark]) = 1 And [Closing Mark] <> '-' Then Cast(Replace([Closing Mark],',','') As float) Else Null End,
Case When IsNumeric([Px Last]) = 1 And [Px Last] <> '-' Then Cast(Replace([Px Last],',','') As float) Else Null End,
--Case When IsNumeric([% Change]) = 1 And [% Change] <> '-' Then Cast(Replace([% Change],',','') As float) Else Null End,
100*((Case When IsNumeric([Px Last]) = 1 And [Px Last] <> '-' Then Cast(Replace([Px Last],',','') As float) Else Null End)-(Case When IsNumeric([Closing Mark]) = 1 And [Closing Mark] <> '-' Then Cast(Replace([Closing Mark],',','') As float) Else Null End))/(Case When IsNumeric([Closing Mark]) = 1 And [Closing Mark] <> '-' Then Cast(Replace([Closing Mark],',','') As float) Else Null End),
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

--Create Table #SecMasterDataTempTable
--(TickerSymbol varchar(Max),UnderlyingDelta float,
--SectorName varchar(Max),SubSectorName varchar(Max),CountryName varchar(Max),CompanyName varchar(Max),BloombergSymbol varchar(Max),PutOrCall varchar(Max),ExpirationDate datetime,StrikePrice float)
--Insert Into #SecMasterDataTempTable 
--(TickerSymbol,UnderlyingDelta,
--SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate,StrikePrice) 
--Select TickerSymbol,UnderlyingDelta,
--SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate,StrikePrice From V_SecMasterData 

--Update DataDump Set 
--[Underlying Security Name] = USM.CompanyName,
--[Bloomberg Symbol] = SM.BloombergSymbol,
--[Underlying Closing Mark] = MPS.FinalMarkPrice
--From #DataDump DataDump
--Left Outer Join #SecMasterDataTempTable SM On (DataDump.[Symbol] = SM.TickerSymbol)
--Left Outer Join #SecMasterDataTempTable USM On (DataDump.[Underlying Symbol] = USM.TickerSymbol) 
--Left Outer Join PM_DayMarkPrice MPS On DateDiff(d,MPS.Date,dbo.AdjustBusinessDays(GetDate(),-1,(Select DefaultAUECID From T_Company))) = 0 And MPS.Symbol = DataDump.[Underlying Symbol]

Select 
[Fund],
[Fund NAV],
[Symbol],
[Security Name],
[Bloomberg Symbol],
[Asset Class],
[Sector],
[SubSector],
[Country],
[Trade Currency],
[Closing Mark],
[Px Last],
[% Change],
[Cost Basis],
[Position],
[Underlying Symbol],
[Underlying Security Name],
[Underlying Price],
[Underlying Closing Mark],
[Delta Adj Position],
[Day P&L Base],
[Market Value Base],
[Exposure Base],
[Cost Basis PNL Base],
[Beta Adj Exposure (Base)] 
From #DataDump

Drop Table #DataDump_1,#DataDump
END

GO