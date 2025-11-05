<?xml version="1.0" encoding="UTF-8"?>

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

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<portfolio>
					<xsl:value-of select="'Portfolio Code'"/>
				</portfolio>

				<Tran_Code>
					<xsl:value-of select="'Tran. Code'"/>
				</Tran_Code>

				<Comment>
					<xsl:value-of select="'Comment'"/>
				</Comment>

				<Sec_Type>
					<xsl:value-of select="'Sec. Type'"/>
				</Sec_Type>

				<Security_Symbol>
					<xsl:value-of select="'Security Symbol'"/>
				</Security_Symbol>

				<Trade_Date>
					<xsl:value-of select="'Trade Date'"/>
				</Trade_Date>

				<Settle_Date>
					<xsl:value-of select="'Settle Date'"/>
				</Settle_Date>

				<Original_Cost_Date>
					<xsl:value-of select="'Original Cost Date'"/>
				</Original_Cost_Date>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Close_Meth>
					<xsl:value-of select="'Close Meth.'"/>
				</Close_Meth>

				<Versus_Date>
					<xsl:value-of select="'Versus Date'"/>
				</Versus_Date>

				<SrcDst_Type>
					<xsl:value-of select="'Src/Dst Type'"/>
				</SrcDst_Type>

				<SrcDstSymbol>
					<xsl:value-of select="'Src/Dst Symbol'"/>
				</SrcDstSymbol>

				<TradeDate_FXRate>
					<xsl:value-of select="'Trade Date FX Rate'"/>
				</TradeDate_FXRate>

				<SetteDate_FXRate>
					<xsl:value-of  select="'Sette Date FX Rate'"/>
				</SetteDate_FXRate>

				<Original_FXRate>
					<xsl:value-of select="'Original FX Rate'"/>
				</Original_FXRate>

				<MarktoMarket>
					<xsl:value-of select="'Mark to Market'"/>
				</MarktoMarket>

				<TradeAmount>
					<xsl:value-of select="'Trade Amount'"/>
				</TradeAmount>

				<OriginalCost>
					<xsl:value-of select="'Original Cost'"/>
				</OriginalCost>

				<reserved>
					<xsl:value-of select="'Reserved'"/>
				</reserved>

				<WithholdingTax>
					<xsl:value-of select="'Withholding Tax'"/>
				</WithholdingTax>

				<Exchange>
					<xsl:value-of select="'Exchange'"/>
				</Exchange>

				<ExchangeFee>
					<xsl:value-of select="'Exchange Fee'"/>
				</ExchangeFee>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<ImpliedComm>
					<xsl:value-of select="'Implied Comm'"/>
				</ImpliedComm>

				<OtherFees>
					<xsl:value-of select="'Other Fees'"/>
				</OtherFees>

				<CommPurpose>
					<xsl:value-of select="'Comm Purpose'"/>
				</CommPurpose>

				<Pledge>
					<xsl:value-of select="'Pledge'"/>
				</Pledge>

				<LotLocation>
					<xsl:value-of select="'Lot Location'"/>
				</LotLocation>

				<DestPledge>
					<xsl:value-of select="'Dest Pledge'"/>
				</DestPledge>

				<DestLotLocation>
					<xsl:value-of select="'Dest Lot Location'"/>
				</DestLotLocation>

				<OriginalFace>
					<xsl:value-of select="'Original Face'"/>
				</OriginalFace>

				<YieldonCost>
					<xsl:value-of select="'Yield on Cost'"/>
				</YieldonCost>

				<DurationonCost>
					<xsl:value-of select="'Durationon Cost'"/>
				</DurationonCost>

				<UserDef1>
					<xsl:value-of select="'User Def. 1'"/>
				</UserDef1>

				<UserDef2>
					<xsl:value-of select="'User Def. 2'"/>
				</UserDef2>

				<UserDef3>
					<xsl:value-of select="'User Def. 3'"/>
				</UserDef3>

				<TranID>
					<xsl:value-of select="'Tran ID'"/>
				</TranID>

				<IPCounter>
					<xsl:value-of select="'IP Counter'"/>
				</IPCounter>

				<Repl.>
					<xsl:value-of select="'Repl.'"/>
				</Repl.>

				<Source>
					<xsl:value-of select="'Source'"/>
				</Source>

				<Reserved1>
					<xsl:value-of select="'Reserved1'"/>
				</Reserved1>

				<OmniAcct>
					<xsl:value-of select="'Omni Acct'"/>
				</OmniAcct>

				<Recon.>
					<xsl:value-of select="'Recon.'"/>
				</Recon.>

				<Post>
					<xsl:value-of select="'Post'"/>
				</Post>

				<LabelName>
					<xsl:value-of select="'Label Name'"/>
				</LabelName>

				<LabelDefinitionNumber>
					<xsl:value-of select="'Label Definition (Number)'"/>
				</LabelDefinitionNumber>

				<LabelDefinitionDate>
					<xsl:value-of select="'Label Definition (Date)'"/>
				</LabelDefinitionDate>

				<LabelDefinitionString>
					<xsl:value-of select="'Label Definition (String)'"/>
				</LabelDefinitionString>

				<Reserved2>
					<xsl:value-of select="'Reserved2'"/>
				</Reserved2>


				<RecordDate>
					<xsl:value-of select="'Record Date'"/>
				</RecordDate>

				<ReclaimAmount>
					<xsl:value-of select="'Reclaim Amount'"/>
				</ReclaimAmount>

				<Strategy>
					<xsl:value-of select="'Strategy'"/>
				</Strategy>

				<Reserved3>
					<xsl:value-of select="'Reserved3'"/>
				</Reserved3>

				<IncomeAccount>
					<xsl:value-of select="'Income Account'"/>
				</IncomeAccount>

				<AccrualAccount>
					<xsl:value-of select="'Accrual Account'"/>
				</AccrualAccount>

				<DivAccrualMethod>
					<xsl:value-of select="'Div Accrual Method'"/>
				</DivAccrualMethod>

				<PerfContributionorWithdrawal>
					<xsl:value-of select="'Perf. Contribution or Withdrawal'"/>
				</PerfContributionorWithdrawal>

				<!--<TradeOrderedBy>
					<xsl:value-of select="'Trade OrderedBy'"/>
				</TradeOrderedBy>


				<TradeEnteredBy>
					<xsl:value-of select="'Trade EnteredBy'"/>
				</TradeEnteredBy>-->

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test ="contains(AccountName, 'Three Arch') != false">
					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'false'"/>
						</RowHeader>

						<!--for system internal use-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="true"/>
						</IsCaptionChangeRequired>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<portfolio>
							<xsl:value-of select ="AccountNo"/>
						</portfolio>

						<Tran_Code>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'by'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'cs'"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side= 'Sell to Close'">
									<xsl:value-of select="'sl'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side = 'Sell to Open'">
									<xsl:value-of select="'ss'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Tran_Code>


						<Comment>
							<xsl:value-of select="''"/>
						</Comment>

						<Sec_Type>
							<xsl:choose>
								<xsl:when test="Asset = 'Equity'">
									<xsl:value-of select="'csus'"/>
								</xsl:when>
								<xsl:when test="Asset = 'EquityOption' and PutOrCall = 'CALL'">
									<xsl:value-of select="'clus'"/>
								</xsl:when>
								<xsl:when test="Asset = 'EquityOption' and PutOrCall = 'PUT'">
									<xsl:value-of select="'ptus'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Sec_Type>

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

						<Security_Symbol>
							<xsl:choose>
								<xsl:when test="Asset != 'EquityOption'">
									<xsl:value-of select="translate(Symbol,$varLower,$varUpper)"/>
								</xsl:when>
								<xsl:when test ="Asset = 'FixedIncome'">
									<xsl:value-of select ="CUSIP"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varOSIOptionSymbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Security_Symbol>

						<Settle_Date>
							<xsl:value-of select="translate(SettlementDate,'/','')"/>
						</Settle_Date>

						<Original_Cost_Date>
							<xsl:value-of select="''"/>
						</Original_Cost_Date>

						<Trade_Date>
							<xsl:value-of select="translate(TradeDate,'/','')"/>
						</Trade_Date>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<Close_Meth>
							<xsl:value-of select="'h'"/>
						</Close_Meth>

						<Versus_Date>
							<xsl:value-of select="''"/>
						</Versus_Date>

						<SrcDst_Type>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side ='Sell'">
									<xsl:value-of select="'caus'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side ='Buy to Close'">
									<xsl:value-of select="'mcus'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SrcDst_Type>


						<SrcDstSymbol>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side ='Sell'">
									<xsl:value-of select="'CASH'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side ='Buy to Close'">
									<xsl:value-of select="'SHORT'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SrcDstSymbol>

						<TradeDate_FXRate>
							<xsl:choose>
								<xsl:when test="CurrencySymbol = 'USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="ForexRate_Trade"/>
								</xsl:otherwise>
							</xsl:choose>
						</TradeDate_FXRate>

						<SetteDate_FXRate>
							<xsl:choose>
								<xsl:when test="CurrencySymbol = 'USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="ForexRate"/>
								</xsl:otherwise>
							</xsl:choose>
						</SetteDate_FXRate>

						<Original_FXRate>
							<xsl:choose>
								<xsl:when test="CurrencySymbol = 'USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="ForexRate_Trade"/>
								</xsl:otherwise>
							</xsl:choose>
						</Original_FXRate>

						<MarktoMarket>
							<xsl:value-of select="''"/>
						</MarktoMarket>

						<TradeAmount>
							<xsl:value-of select="concat('@',AveragePrice)"/>
						</TradeAmount>

						<OriginalCost>
							<xsl:value-of select="''"/>
						</OriginalCost>

						<reserved>
							<xsl:value-of select="''"/>
						</reserved>

						<WithholdingTax>
							<xsl:value-of select="''"/>
						</WithholdingTax>

						<Exchange>
							<xsl:value-of select="1"/>
						</Exchange>

						<ExchangeFee>
							<xsl:choose>
								<xsl:when test="Side='Sell short' or Side ='Sell'">
									<xsl:value-of select="'y'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExchangeFee>

						<Commission>
							<xsl:value-of select="CommissionCharged"/>
						</Commission>

						<xsl:variable name="varCounterPartyID" select="CounterPartyID"/>

						<xsl:variable name="varCounterParty">
							<xsl:value-of select="document('..\ReconMappingXml\ExecBrokerMapping.xml')/BrokerMapping/PB[Name = 'BTIG']/BrokerData[PranaBrokerID = $varCounterPartyID]/@PranaBroker"/>
						</xsl:variable>

						<Broker>
							<xsl:value-of select="CounterParty"/>
						</Broker>

						<ImpliedComm>
							<xsl:value-of select="'n'"/>
						</ImpliedComm>

						<OtherFees>
							<xsl:choose>
								<xsl:when test="CounterParty = 'BAYT'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="OtherBrokerFee"/>
								</xsl:otherwise>
							</xsl:choose>
						</OtherFees>

						<CommPurpose>
							<xsl:value-of select="''"/>
						</CommPurpose>

						<Pledge>
							<xsl:value-of select="'n'"/>
						</Pledge>

						<LotLocation>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side = 'Sell short'">
									<xsl:value-of select="'253'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</LotLocation>

						<DestPledge>
							<xsl:value-of select="''"/>
						</DestPledge>

						<DestLotLocation>
							<xsl:value-of select="''"/>
						</DestLotLocation>

						<OriginalFace>
							<xsl:value-of select="''"/>
						</OriginalFace>

						<YieldonCost>
							<xsl:value-of select="''"/>
						</YieldonCost>

						<DurationonCost>
							<xsl:value-of select="''"/>
						</DurationonCost>

						<UserDef1>
							<xsl:value-of select="''"/>
						</UserDef1>

						<UserDef2>
							<xsl:value-of select="''"/>
						</UserDef2>

						<UserDef3>
							<xsl:value-of select="''"/>
						</UserDef3>

						<TranID>
							<xsl:value-of select="''"/>
						</TranID>

						<IPCounter>
							<xsl:value-of select="''"/>
						</IPCounter>

						<Repl.>
							<xsl:value-of select="''"/>
						</Repl.>

						<Source>
							<xsl:value-of select="'1'"/>
						</Source>

						<Reserved1>
							<xsl:value-of select="''"/>
						</Reserved1>

						<OmniAcct>
							<xsl:value-of select="''"/>
						</OmniAcct>

						<Recon.>
							<xsl:value-of select="''"/>
						</Recon.>

						<Post>
							<xsl:value-of select="'y'"/>
						</Post>

						<LabelName>
							<xsl:value-of select="''"/>
						</LabelName>

						<LabelDefinitionNumber>
							<xsl:value-of select="''"/>
						</LabelDefinitionNumber>

						<LabelDefinitionDate>
							<xsl:value-of select="''"/>
						</LabelDefinitionDate>

						<LabelDefinitionString>
							<xsl:value-of select="''"/>
						</LabelDefinitionString>

						<Reserved2>
							<xsl:value-of select="''"/>
						</Reserved2>

						<RecordDate>
							<xsl:value-of select="''"/>
						</RecordDate>

						<ReclaimAmount>
							<xsl:value-of select="''"/>
						</ReclaimAmount>

						<Strategy>
							<xsl:value-of select="''"/>
						</Strategy>

						<Reserved3>
							<xsl:value-of select="''"/>
						</Reserved3>

						<IncomeAccount>
							<xsl:value-of select="''"/>
						</IncomeAccount>

						<AccrualAccount>
							<xsl:value-of select="''"/>
						</AccrualAccount>

						<DivAccrualMethod>
							<xsl:value-of select="''"/>
						</DivAccrualMethod>

						<PerfContributionorWithdrawal>
							<xsl:value-of select="''"/>
						</PerfContributionorWithdrawal>

						<!--<TradeOrderedBy>
						<xsl:value-of select="'Charles Frumberg'"/>
					</TradeOrderedBy>-->


						<!--<TradeEnteredBy>
						<xsl:value-of select="'Chris Wallace'"/>
					</TradeEnteredBy>-->

						<!-- system use only-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

	<xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>