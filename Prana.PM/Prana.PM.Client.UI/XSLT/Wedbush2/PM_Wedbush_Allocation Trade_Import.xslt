<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test ="number($Position) and COL8='U'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Wedbush'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL55"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Touradji']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select ="COL1"/>
						</PositionStartDate>

						<!--<PositionSettlementDate>
							<xsl:value-of select ="COL48"/>
						</PositionSettlementDate>-->

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$Position &lt;0">
									<xsl:value-of select ="$Position*-1"/>
								</xsl:when>

								<xsl:when test ="$Position &gt;0">
									<xsl:value-of select ="$Position"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<!--<xsl:variable name="Asset" select="normalize-space(COL7)"/>-->

						<xsl:variable name="varside" select="COL2"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varside='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varside='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<!--<xsl:when test="$Asset='EquityOption' and $varside='Buy to Open'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption' and $varside='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Asset='Equity' and $varside='Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption' and $varside='Sell to Open'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption' and $varside='Sell to Close'">
									<xsl:value-of select="'D'"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>
						
						<xsl:variable name ="varCommision">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>

								<xsl:when test ="$varCommision &lt;0">
									<xsl:value-of select ="$varCommision*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommision &gt;0">
									<xsl:value-of select ="$varCommision"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name="StampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>

								<xsl:when test ="$StampDuty &lt;0">
									<xsl:value-of select ="$StampDuty*-1"/>
								</xsl:when>

								<xsl:when test ="$StampDuty &gt;0">
									<xsl:value-of select ="$StampDuty"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</StampDuty>


						<!--<xsl:variable name="varFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>
							<xsl:choose>

								<xsl:when test ="$varFee &lt;0">
									<xsl:value-of select ="$varFee*-1"/>
								</xsl:when>

								<xsl:when test ="$varFee &gt;0">
									<xsl:value-of select ="$varFee"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Fees>-->

						<xsl:variable name = "PB_BROKER_NAME">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_BROKER_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>

								<xsl:when test ="number($PRANA_BROKER_NAME)">
									<xsl:value-of select ="$PRANA_BROKER_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CounterPartyID>

						<xsl:variable name = "PB_ACCOUNT_NAME">
							<xsl:value-of select="normalize-space(COL11)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_ACCOUNT_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/TradingAccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/AccountData[@PBAccount=$PB_ACCOUNT_NAME]/@PranaAccountCode"/>
						</xsl:variable>

						<TradingAccountID>
							<xsl:choose>

								<xsl:when test ="number($PRANA_ACCOUNT_NAME)">
									<xsl:value-of select ="$PRANA_ACCOUNT_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="122"/>
								</xsl:otherwise>
							</xsl:choose>

						</TradingAccountID>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
