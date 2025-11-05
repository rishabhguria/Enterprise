<?xml version="1.0" encoding="UTF-8"?>
								<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<!--<InstrumentSubType>
					<xsl:value-of select="'Instrument Sub Type'"/>
				</InstrumentSubType>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<LifeCycle>
					<xsl:value-of select="'Life Cycle'"/>
				</LifeCycle>-->

				<OrdStatus>
					<xsl:value-of select="'OrdStatus'"/>
				</OrdStatus>

				<ExecTransType>
					<xsl:value-of select="'ExecTransType'"/>
				</ExecTransType>

				<ClientOrderID>
					<xsl:value-of select="'ClientOrderID'"/>
				</ClientOrderID>

				<FillID>
					<xsl:value-of select="'Fill ID'"/>
				</FillID>

				<IDofOrderOrFillforAction>
					<xsl:value-of select="'ID of Order Or Fill for Action'"/>
				</IDofOrderOrFillforAction>

				<LotNumber>
					<xsl:value-of select="'LotNumber'"/>
				</LotNumber>

				<Symbol>
					<xsl:value-of select="'Symbol'"/>
				</Symbol>

				<SecurityType>
					<xsl:value-of select="'SecurityType'"/>
				</SecurityType>

				<SecurityCurrency>
					<xsl:value-of select="'Security Currency'"/>
				</SecurityCurrency>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<BuySellShortCover>
					<xsl:value-of select="'BuySellShortCover'"/>
				</BuySellShortCover>

				<OpenClose>
					<xsl:value-of select="'OpenClose'"/>
				</OpenClose>

				<IDSource>
					<xsl:value-of select="'IDSource'"/>
				</IDSource>

				<SecurityID>
					<xsl:value-of select="'SecurityID'"/>
				</SecurityID>

				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<Bloomberg>
					<xsl:value-of select="'Bloomberg'"/>
				</Bloomberg>

				<CINS>
					<xsl:value-of select="'CINS'"/>
				</CINS>

				<WhenIssued>
					<xsl:value-of select="'WhenIssued'"/>
				</WhenIssued>

				<IssueDate>
					<xsl:value-of select="'IssueDate'"/>
				</IssueDate>

				<Maturity>
					<xsl:value-of select="'Maturity'"/>
				</Maturity>

				<Coupon>
					<xsl:value-of select="'Coupon %'"/>
				</Coupon>

				<ExecutionInterestDays>
					<xsl:value-of select="'ExecutionInterestDays'"/>
				</ExecutionInterestDays>

				<AccruedInterest>
					<xsl:value-of select="'AccruedInterest'"/>
				</AccruedInterest>


				<FaceValue>
					<xsl:value-of select="'FaceValue'"/>
				</FaceValue>

				<RollableType>
					<xsl:value-of select="'RollableType'"/>
				</RollableType>

				<RepoLoanCurrency>
					<xsl:value-of select="'RepoLoanCurrency'"/>
				</RepoLoanCurrency>

				<DayCountBase>
					<xsl:value-of select="'DayCountBase'"/>
				</DayCountBase>

				<RepoLoanAmount>
					<xsl:value-of select="'RepoLoanAmount'"/>
				</RepoLoanAmount>

				<Trader>
					<xsl:value-of select="'Trader'"/>
				</Trader>

				<OrderQty>
					<xsl:value-of select="'OrderQty'"/>
				</OrderQty>


				<FillQty>
					<xsl:value-of select="'FillQty'"/>
				</FillQty>


				<CumQty>
					<xsl:value-of select="'CumQty'"/>
				</CumQty>

				<HairCut>
					<xsl:value-of select="'HairCut'"/>
				</HairCut>


				<AvgPx>
					<xsl:value-of select="'AvgPx'"/>
				</AvgPx>

				<FillPrice>
					<xsl:value-of select="'FillPrice'"/>
				</FillPrice>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<TradeTime>
					<xsl:value-of select="'TradeTime'"/>
				</TradeTime>

				<OrigDate>
					<xsl:value-of select="'OrigDate'"/>
				</OrigDate>

				<Unused>
					<xsl:value-of select="'Unused'"/>
				</Unused>

				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>

				<ExecutingUser>
					<xsl:value-of select="'Executing User'"/>
				</ExecutingUser>

				<Comment>
					<xsl:value-of select="'Comment'"/>
				</Comment>

				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>

				<Fund>
					<xsl:value-of select="'Fund'"/>
				</Fund>

				<SubFund>
					<xsl:value-of select="'SubFund'"/>
				</SubFund>

				<AllocationCode>
					<xsl:value-of select="'AllocationCode'"/>
				</AllocationCode>

				<StrategyCode>
					<xsl:value-of select="'StrategyCode'"/>
				</StrategyCode>


				<ExecutionBroker>
					<xsl:value-of select="'Execution Broker'"/>
				</ExecutionBroker>

				<ClearingBroker>
					<xsl:value-of select="'ClearingBroker'"/>
				</ClearingBroker>

				<ContractSize>
					<xsl:value-of select="'ContractSize'"/>
				</ContractSize>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<SpotFXRate>
					<xsl:value-of select="'Spot FX Rate'"/>
				</SpotFXRate>

				<FWDFXpoints>
					<xsl:value-of select="'FWD FX points'"/>
				</FWDFXpoints>

				
				<Fee>
					<xsl:value-of select="'Fee'"/>
				</Fee>

				<CurrencyTraded>
					<xsl:value-of select="'CurrencyTraded'"/>
				</CurrencyTraded>

				<SettleCurrency>
					<xsl:value-of select="'SettleCurrency'"/>
				</SettleCurrency>

				<FXBASErate>
					<xsl:value-of select="'FX/BASE rate'"/>
				</FXBASErate>

				<BASEFXrate>
					<xsl:value-of select="'BASE/FX rate'"/>
				</BASEFXrate>

				<StrikePrice>
					<xsl:value-of select="'StrikePrice'"/>
				</StrikePrice>

				<PutOrCall>
					<xsl:value-of select="'PutOrCall'"/>
				</PutOrCall>

				<DerivativeExpiry>
					<xsl:value-of select="'Derivative Expiry'"/>
				</DerivativeExpiry>

				<SubStrategy>
					<xsl:value-of select="'SubStrategy'"/>
				</SubStrategy>

				<OrderGroup>
					<xsl:value-of select="'OrderGroup'"/>
				</OrderGroup>

				<RepoPenalty>
					<xsl:value-of select="'RepoPenalty'"/>
				</RepoPenalty>

				<CommissionTurn>
					<xsl:value-of select="'CommissionTurn'"/>
				</CommissionTurn>

				<AllocRule>
					<xsl:value-of select="'AllocRule'"/>
				</AllocRule>

				<PaymentFreq>
					<xsl:value-of select="'PaymentFreq'"/>
				</PaymentFreq>


				<RateSource>
					<xsl:value-of select="'RateSource'"/>
				</RateSource>


				<Spread>
					<xsl:value-of select="'Spread'"/>
				</Spread>

				<CurrentFace>
					<xsl:value-of select="'CurrentFace'"/>
				</CurrentFace>


				<CurrentPrincipalFactor>
					<xsl:value-of select="'CurrentPrincipalFactor'"/>
				</CurrentPrincipalFactor>


				<AccrualFactor>
					<xsl:value-of select="'AccrualFactor'"/>
				</AccrualFactor>


				<TaxRate>
					<xsl:value-of select="'Tax Rate'"/>
				</TaxRate>


				<Expenses>
					<xsl:value-of select="'Expenses'"/>
				</Expenses>


				<Fees>
					<xsl:value-of select="'Fees'"/>
				</Fees>


				<PostCommAndFeesOnInit>
					<xsl:value-of select="'PostCommAndFeesOnInit'"/>
				</PostCommAndFeesOnInit>


				<ImpliedCommissionFlag>
					<xsl:value-of select="'Implied Commission Flag'"/>
				</ImpliedCommissionFlag>



				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>



				<MasterConfrimType>
					<xsl:value-of select="'Master Confrim Type'"/>
				</MasterConfrimType>


				<MatrixTerm>
					<xsl:value-of select="'Matrix Term'"/>
				</MatrixTerm>


				<EMInternalSeqNo>
					<xsl:value-of select="'EMInternalSeqNo.'"/>
				</EMInternalSeqNo>


				<ObjectivePrice>
					<xsl:value-of select="'ObjectivePrice'"/>
				</ObjectivePrice>

				<MarketPrice>
					<xsl:value-of select="'MarketPrice'"/>
				</MarketPrice>

				<InitialMargin>
					<xsl:value-of select="'Initial Margin'"/>
				</InitialMargin>


				<NetConsdieration>
					<xsl:value-of select="'NetConsdieration'"/>
				</NetConsdieration>


				<FixingDate>
					<xsl:value-of select="'Fixing Date'"/>
				</FixingDate>


				<DeliveryInstructions>
					<xsl:value-of select="'Delivery Instructions'"/>
				</DeliveryInstructions>

				<ForceMatchID>
					<xsl:value-of select="'Force Match ID'"/>
				</ForceMatchID>


				<ForceMatchType>
					<xsl:value-of select="'Force Match Type'"/>
				</ForceMatchType>


				<ForceMatchNotes>
					<xsl:value-of select="'Force Match Notes'"/>
				</ForceMatchNotes>


				<CommissionRateforAllocation>
					<xsl:value-of select="'Commission Rate for Allocation'"/>
				</CommissionRateforAllocation>


				<CommissionAmountforFill>
					<xsl:value-of select="'Commission Amount for Fill'"/>
				</CommissionAmountforFill>


				<ExpenseAmountforFill>
					<xsl:value-of select="'Expense Amount for Fill'"/>
				</ExpenseAmountforFill>


				<FeeAmountforFill>
					<xsl:value-of select="'Fee Amount for Fill'"/>
				</FeeAmountforFill>


				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>


					<!--<InstrumentSubType>
						<xsl:value-of select="Asset"/>
					</InstrumentSubType>

					<Comments>
						<xsl:value-of select="''"/>
					</Comments>-->

					<!--Exercise / Assign Need To Ask-->
					<xsl:variable name="varLifeCycle">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'New'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'Replace'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'Delete'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Sent'">
								<xsl:value-of select="'Expire'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TaxLotState"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<!--<LifeCycle>
						<xsl:value-of select="$varLifeCycle"/>
					</LifeCycle>-->

					<!--Exercise / Assign Need To Ask-->

					<xsl:variable name="varOrdStatus">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated' or TaxLotState='Sent'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'R'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OrdStatus>
						<xsl:value-of select="$varOrdStatus"/>
					</OrdStatus>

					<xsl:variable name="varExecTransType">
						<xsl:choose>
							<xsl:when test="TaxLotState='Amemded' or TaxLotState='Deleted'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="2"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<ExecTransType>
						<xsl:value-of select="$varExecTransType"/>
					</ExecTransType>

					<ClientOrderID>
						<xsl:value-of select="TradeRefID"/>
					</ClientOrderID>


					<FillID>
						<xsl:value-of select="TradeRefID"/>
					</FillID>

					<IDofOrderOrFillforAction>
						<xsl:value-of select="''"/>
					</IDofOrderOrFillforAction>

					<LotNumber>
						<xsl:value-of select="''"/>
					</LotNumber>

					<!-- For Equity Option OSI Symbology-->


					<xsl:variable name="varOptionUnderlying">
						<xsl:value-of select="substring-after(substring-before(Symbol,' '),':')"/>
					</xsl:variable>

					<xsl:variable name = "BlankCount_Root" >
						<xsl:call-template name="noofBlanks">
							<xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varFormattedStrikePrice">
						<xsl:value-of select="format-number(StrikePrice,'00000.000')"/>
					</xsl:variable>

					<xsl:variable name="varOSIOptionSymbol">
						<xsl:value-of select="concat($varOptionUnderlying,$BlankCount_Root,substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2),substring(PutOrCall,1,1),translate($varFormattedStrikePrice,'.',''))"/>
					</xsl:variable>



					<xsl:variable name ="varUnderlying">
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</xsl:variable>

					<xsl:variable name = "varPreffix" >
						<xsl:value-of select="document('../ReconMappingXml/SymbolPreffixMapping.xml')/SymbolPreffix/PB[@Name='CITCO']/SymbolData[@Underlying=$varUnderlying]/@Preffix"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$varPreffix != ''">
							<Symbol>
								<xsl:value-of select="translate(Symbol,substring-before(Symbol,' '),$varPreffix)"/>
							</Symbol>
						</xsl:when>
						<xsl:when test="Asset='FX' or Asset='FXForward'">
							<Symbol>
								<xsl:value-of select="translate(Symbol,'-','/')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test="Asset='EquityOption'">
							<Symbol>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varOSIOptionSymbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="Symbol"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:variable name="varSecurityType">
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPT'"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'FUT'"/>
							</xsl:when>
							<xsl:when test="Asset='FutureOption'">
								<xsl:value-of select="'IDXFUTOPT'"/>
							</xsl:when>
							<xsl:when test="Asset='FXForward'">
								<xsl:value-of select="'FWDFX'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'COP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecurityType>
						<xsl:value-of select="$varSecurityType"/>
					</SecurityType>

					<xsl:choose>
						<xsl:when test ="Asset='FX' or Asset='FXForward'">
							<SecurityCurrency>
								<xsl:value-of select="LeadCurrencyName"/>
							</SecurityCurrency>
						</xsl:when>
						<xsl:otherwise>
							<SecurityCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</SecurityCurrency>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test="$varPreffix != '' and substring-before(FullSecurityName,' ') = $varUnderlying">
							<SecurityDescription>
								<xsl:value-of select="translate(FullSecurityName,substring-before(FullSecurityName,' '),$varPreffix)"/>
							</SecurityDescription>
						</xsl:when>
						<xsl:when test="Asset='FX' or Asset='FXForward'">
							<SecurityDescription>
								<xsl:value-of select="translate(FullSecurityName,'-','/')"/>
							</SecurityDescription>
						</xsl:when>
						<xsl:otherwise>
							<SecurityDescription>
								<xsl:value-of select="FullSecurityName"/>
							</SecurityDescription>
						</xsl:otherwise>
					</xsl:choose >

					<!--<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>-->


					<!--Need To Confirm Here-->
					<xsl:variable name="varSide">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<BuySellShortCover>
						<xsl:value-of select="$varSide"/>
					</BuySellShortCover>

					<OpenClose>
						<xsl:value-of select="''"/>
					</OpenClose>

					<xsl:choose>
						<xsl:when test ="Asset='Equity'">
							<IDSource>
								<xsl:value-of select="'SEDOL'"/>
							</IDSource>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption'">
							<IDSource>
								<xsl:value-of select="'OCC'"/>
							</IDSource>
						</xsl:when>
						<xsl:when test ="Asset='FixedIncome' and ISIN != ''">
							<IDSource>
								<xsl:value-of select="'ISIN'"/>
							</IDSource>
						</xsl:when>
						<xsl:otherwise>
							<IDSource>
								<xsl:value-of select="'TICKER'"/>
							</IDSource>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:variable name ="varAsset">
						<xsl:value-of select="Asset"/>
					</xsl:variable>

					<xsl:variable name = "varSuffix" >
						<xsl:value-of select="document('../ReconMappingXml/Bloombergsuffix.xml')/Bloombergsuffix/PB[@Name='CITCO']/SymbolData[@Asset=$varAsset] [@Underlying=$varUnderlying]/@Suffix"/>
					</xsl:variable>

					<xsl:variable name ="varFutureOptionSymbol">
						<xsl:choose>
							<xsl:when test ="Asset = 'FutureOption' and $varPreffix != ''">
								<xsl:value-of select="translate(translate(Symbol,substring-before(FullSecurityName,' '),$varPreffix),' ','')"/>
							</xsl:when>
							<xsl:when test ="Asset = 'FutureOption' and $varPreffix = ''">
								<xsl:value-of select="translate(Symbol,' ','')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<!--<xsl:choose>
						<xsl:when test ="Asset = 'FutureOption'">
							<SecurityID>
								<xsl:value-of select="concat(substring($varFutureOptionSymbol,1,5),' ',substring($varFutureOptionSymbol,6),' ',$varSuffix)"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'Future' and $varPreffix != ''">
							<SecurityID>
								<xsl:value-of select="concat(translate(translate(Symbol,substring-before(FullSecurityName,' '),$varPreffix),' ',''),' ',$varSuffix)"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'Future' and $varPreffix = ''">
							<SecurityID>
								<xsl:value-of select="concat(translate(Symbol,' ',''),' ',$varSuffix)"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity'">
							<SecurityID>
								<xsl:value-of select="SEDOL"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'EquityOption'">
							<SecurityID>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varOSIOptionSymbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityID>
						</xsl:when>
						<xsl:when test="Asset='FX' or Asset='FXForward'">
							<SecurityID>
								<xsl:value-of select="concat(translate(Symbol,'-',''),' CURNCY')"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test="Asset='FixedIncome' and ISIN != ''">
							<SecurityID>
								<xsl:value-of select="ISIN"/>
							</SecurityID>
						</xsl:when>
						<xsl:otherwise>
							<SecurityID>
								<xsl:value-of select="concat(Symbol,' ',$varSuffix)"/>
							</SecurityID>
						</xsl:otherwise>
					</xsl:choose>-->

					<xsl:choose>
						<xsl:when test ="Asset = 'FutureOption' or Asset = 'Future'">
							<SecurityID>
								<xsl:value-of select="BBCode"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity'">
							<SecurityID>
								<xsl:value-of select="SEDOL"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test ="Asset = 'EquityOption'">
							<SecurityID>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varOSIOptionSymbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityID>
						</xsl:when>
						<xsl:when test="Asset='FX' or Asset='FXForward'">
							<SecurityID>
								<xsl:value-of select="concat(translate(Symbol,'-',''),' CURNCY')"/>
							</SecurityID>
						</xsl:when>
						<xsl:when test="Asset='FixedIncome' and ISIN != ''">
							<SecurityID>
								<xsl:value-of select="ISIN"/>
							</SecurityID>
						</xsl:when>
						<xsl:otherwise>
							<SecurityID>
								<xsl:value-of select="concat(Symbol,' ',$varSuffix)"/>
							</SecurityID>
						</xsl:otherwise>
					</xsl:choose>


					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>

					<Bloomberg>
						<xsl:value-of select="BBCode"/>
					</Bloomberg>

					<CINS>
						<xsl:value-of select="''"/>
					</CINS>

					<WhenIssued>
						<xsl:value-of select="''"/>
					</WhenIssued>

					<IssueDate>
						<xsl:value-of select="''"/>
					</IssueDate>

					<xsl:variable name="varMaturity">
						<xsl:choose>
							<xsl:when test="ExpirationDate='01/01/1800'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
								<!--<xsl:value-of select="ExpirationDate"/>-->
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Maturity>
						<xsl:value-of select="$varMaturity"/>
					</Maturity>

					<Coupon>
						<xsl:value-of select="''"/>
					</Coupon>

					<ExecutionInterestDays>
						<xsl:value-of select="''"/>
					</ExecutionInterestDays>

					<AccruedInterest>
						<xsl:value-of select="''"/>
					</AccruedInterest>


					<FaceValue>
						<xsl:value-of select="''"/>
					</FaceValue>

					<RollableType>
						<xsl:value-of select="''"/>
					</RollableType>

					<RepoLoanCurrency>
						<xsl:value-of select="''"/>
					</RepoLoanCurrency>

					<DayCountBase>
						<xsl:value-of select="''"/>
					</DayCountBase>

					<RepoLoanAmount>
						<xsl:value-of select="''"/>
					</RepoLoanAmount>


					<Trader>
						<xsl:value-of select="'SAMR'"/>
					</Trader>

					<OrderQty>
						<!--<xsl:value-of select="TotalQty"/>-->
						<xsl:value-of select="AllocatedQty"/>
					</OrderQty>


					<FillQty>
						<!--<xsl:value-of select="AllocatedQty"/>-->
						<xsl:value-of select="''"/>
					</FillQty>


					<CumQty>
						<!--<xsl:value-of select="ExecutedQty"/>-->
						<xsl:value-of select="''"/>
					</CumQty>

					<HairCut>
						<xsl:value-of select="''"/>
					</HairCut>

					<xsl:variable name ="varSymbol">
						<xsl:value-of select ="Symbol"/>
					</xsl:variable>


					<xsl:variable name="varMultiplier">
						<xsl:value-of select="document('../ReconMappingXml/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name='CITCO']/MultiplierData[@PranaSymbol=$varSymbol]/@PBMultiplier"/>
					</xsl:variable>

					<AvgPx>
						<xsl:choose>
							<xsl:when test ="$varMultiplier != '' and number($varMultiplier)">
								<xsl:value-of select="AveragePrice * $varMultiplier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AveragePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvgPx>

					<!--<AvgPx>
						<xsl:value-of select="AveragePrice"/>
					</AvgPx>-->


					<FillPrice>
						<xsl:value-of select="''"/>
					</FillPrice>

					<!--YYYYMMDD-->
					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<TradeTime>
						<xsl:value-of select="''"/>
					</TradeTime>

					<OrigDate>
						<xsl:value-of select="''"/>
					</OrigDate>

					<Unused>
						<xsl:value-of select="''"/>
					</Unused>


					<SettlementDate>
						<xsl:choose>
							<xsl:when test ="Asset='Future'">
								<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>		
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
							</xsl:otherwise>
						</xsl:choose>						
					</SettlementDate>

					<ExecutingUser>
						<xsl:value-of select="''"/>
					</ExecutingUser>

					<Comment>
						<xsl:value-of select="''"/>
					</Comment>

					<Account>
						<xsl:value-of select="''"/>
					</Account>

					<Fund>
						<xsl:value-of select="concat('>','LY_MSR1')"/>
					</Fund>

					<SubFund>
						<xsl:value-of select="''"/>
					</SubFund>

					<AllocationCode>
						<xsl:value-of select="''"/>
					</AllocationCode>

					<StrategyCode>
						<xsl:value-of select="''"/>
					</StrategyCode>

					<ExecutionBroker>
						<xsl:value-of select="CounterParty"/>
					</ExecutionBroker>

					<xsl:choose>
						<xsl:when test ="Asset='Equity' or Asset='FX' or Asset='PrivateEquity'">
							<ClearingBroker>
								<xsl:value-of select="'CS'"/>
							</ClearingBroker>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption' or Asset='Future' or Asset='FutureOption'">
							<ClearingBroker>
								<xsl:value-of select="'CS FUT'"/>
							</ClearingBroker>
						</xsl:when>
						<xsl:otherwise>
							<ClearingBroker>
								<xsl:value-of select="'CS'"/>
							</ClearingBroker>
						</xsl:otherwise>
					</xsl:choose>



					<ContractSize>
						<xsl:value-of select="''"/>
					</ContractSize>

					<!--<xsl:variable name ="varUnderlying">
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</xsl:variable>-->

					<xsl:variable name ="varAssetCategory">
						<xsl:value-of select="Asset"/>
					</xsl:variable>

					<xsl:variable name ="varExchange">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name="varcommissionRate">
						<xsl:value-of select="document('../ReconMappingXml/CommissionRate.xml')/CommissionRateMapping/PB[@Name='CS']/SymbolData[@Asset = $varAssetCategory and @Underlying = $varUnderlying and @Exchange = $varExchange]/@CommRate"/>
					</xsl:variable>

					<!--<Commission>
						<xsl:choose>
							<xsl:when test ="$varcommissionRate != '' and number($varcommissionRate)">
								<xsl:value-of select="AllocatedQty * $varcommissionRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>-->

					<Commission>
						<xsl:value-of select='format-number(CommissionCharged, "###.00")'/>

						<!--<xsl:value-of select="CommissionCharged"/>-->

					</Commission>

					<SpotFXRate>
						<!--<xsl:value-of select="ForexRate_Trade"/>-->
						<xsl:value-of select="1"/>
					</SpotFXRate>

					<FWDFXpoints>
						<xsl:value-of select="''"/>
					</FWDFXpoints>

					
					
					<!--<Fee>
						<xsl:choose>
							<xsl:when test="Asset ='Future' or Asset = 'FutureOption'">
								<xsl:value-of select="StampDuty + MiscFees + ClearingFee + OtherBrokerFee + SecFees + TaxOnCommissions"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fee>-->

					<Fee>
						<xsl:choose>
							<xsl:when test ="$varcommissionRate != '' and number($varcommissionRate)">
								<xsl:value-of select="AllocatedQty * $varcommissionRate"/>
							</xsl:when>
							<!--As per Lynn mail and my discussion with Parveen, there will be no Stamp Duty for Equity-->
							<xsl:when test ="Asset = 'Equity'">
								<xsl:value-of select="MiscFees + ClearingFee + OtherBrokerFee + SecFees + TaxOnCommissions"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="StampDuty + MiscFees + ClearingFee + OtherBrokerFee + SecFees + TaxOnCommissions"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fee>

					<CurrencyTraded>
						<xsl:value-of select="''"/>
					</CurrencyTraded>

					<SettleCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettleCurrency>

					<FXBASErate>
						<xsl:value-of select="''"/>
					</FXBASErate>

					<BASEFXrate>
						<xsl:value-of select="''"/>
					</BASEFXrate>

					<StrikePrice>
						<xsl:value-of select="''"/>
					</StrikePrice>

					<PutOrCall>
						<xsl:value-of select="''"/>
					</PutOrCall>

					<DerivativeExpiry>
						<xsl:value-of select="''"/>
					</DerivativeExpiry>

					<SubStrategy>
						<xsl:value-of select="''"/>
					</SubStrategy>

					<OrderGroup>
						<xsl:value-of select="''"/>
					</OrderGroup>

					<RepoPenalty>
						<xsl:value-of select="''"/>
					</RepoPenalty>

					<CommissionTurn>
						<xsl:value-of select="''"/>
					</CommissionTurn>

					<AllocRule>
						<xsl:value-of select="''"/>
					</AllocRule>

					<PaymentFreq>
						<xsl:value-of select="''"/>
					</PaymentFreq>


					<RateSource>
						<xsl:value-of select="''"/>
					</RateSource>


					<Spread>
						<xsl:value-of select="''"/>
					</Spread>

					<CurrentFace>
						<xsl:value-of select="''"/>
					</CurrentFace>


					<CurrentPrincipalFactor>
						<xsl:value-of select="''"/>
					</CurrentPrincipalFactor>


					<AccrualFactor>
						<xsl:value-of select="''"/>
					</AccrualFactor>

					<TaxRate>
						<xsl:value-of select="''"/>
					</TaxRate>

					<Expenses>
						<xsl:value-of select="''"/>
					</Expenses>

					<Fees>
						<xsl:choose>
							<!--As per Lynn mail and my discussion with Parveen, there will be no Stamp Duty for Equity-->
							<xsl:when test="Asset ='Equity'">
								<xsl:value-of select="MiscFees + ClearingFee + OtherBrokerFee + SecFees + TaxOnCommissions"/>
							</xsl:when>
							<xsl:when test="Asset !='Future' and Asset != 'FutureOption' and Asset != 'Equity'">
								<xsl:value-of select="StampDuty + MiscFees + ClearingFee + OtherBrokerFee + SecFees + TaxOnCommissions"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fees>

					<PostCommAndFeesOnInit>
						<xsl:value-of select="''"/>
					</PostCommAndFeesOnInit>


					<ImpliedCommissionFlag>
						<xsl:value-of select="''"/>
					</ImpliedCommissionFlag>

					<TransactionType>
						<xsl:value-of select="''"/>
					</TransactionType>


					<MasterConfrimType>
						<xsl:value-of select="''"/>
					</MasterConfrimType>

					<MatrixTerm>
						<xsl:value-of select="''"/>
					</MatrixTerm>

					<EMInternalSeqNo>
						<xsl:value-of select="''"/>
					</EMInternalSeqNo>

					<ObjectivePrice>
						<xsl:value-of select="''"/>
					</ObjectivePrice>

					<MarketPrice>
						<xsl:value-of select="''"/>
					</MarketPrice>

					<InitialMargin>
						<xsl:value-of select="''"/>
					</InitialMargin>

					<NetConsdieration>
						<xsl:value-of select="''"/>
					</NetConsdieration>

					<FixingDate>
						<xsl:value-of select="''"/>
					</FixingDate>

					<DeliveryInstructions>
						<xsl:value-of select="''"/>
					</DeliveryInstructions>

					<ForceMatchID>
						<xsl:value-of select="''"/>
					</ForceMatchID>

					<ForceMatchType>
						<xsl:value-of select="''"/>
					</ForceMatchType>

					<ForceMatchNotes>
						<xsl:value-of select="''"/>
					</ForceMatchNotes>


					<CommissionRateforAllocation>
						<xsl:value-of select="''"/>
					</CommissionRateforAllocation>


					<CommissionAmountforFill>
						<xsl:value-of select="''"/>
					</CommissionAmountforFill>


					<ExpenseAmountforFill>
						<xsl:value-of select="''"/>
					</ExpenseAmountforFill>


					<FeeAmountforFill>
						<xsl:value-of select="''"/>
					</FeeAmountforFill>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
