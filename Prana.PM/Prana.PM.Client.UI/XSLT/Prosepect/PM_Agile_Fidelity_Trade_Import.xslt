<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
						<xsl:with-param name="Number" select="COL19"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and normalize-space(COL15)!='CASH'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Agile'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="normalize-space(COL10)"/>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<!--<xsl:variable name="AssetType" select="normalize-space(COL12)"/>-->
						<xsl:variable name="Currency" select="normalize-space(COL17)"/>

						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="translate($Symbol,$varSmall,$varCapital)"/>
								</xsl:when>							

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>						

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>

								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name="Side" select="normalize-space(COL7)"/>

						<SideTagValue>
							<xsl:choose>

								<xsl:when test="$Side='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$Side='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="$Side='SellShort'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="$Side='CoverShort'">
									<xsl:value-of select="'B'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="Comm">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Comm &gt; 0">
									<xsl:value-of select="$Comm"/>

								</xsl:when>
								<xsl:when test="$Comm &lt; 0">
									<xsl:value-of select="$Comm * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name="StampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$StampDuty &gt; 0">
									<xsl:value-of select="$StampDuty"/>

								</xsl:when>
								<xsl:when test="$StampDuty &lt; 0">
									<xsl:value-of select="$StampDuty * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</StampDuty>

						<xsl:variable name="PB_CounterParty" select="normalize-space(COL33)"/>

						<xsl:variable name="PRANA_CounterParty">
							<xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>

								<xsl:when test ="$PRANA_CounterParty!=''">
									<xsl:value-of select ="$PRANA_CounterParty"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CounterPartyID>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<PositionStartDate>
							<xsl:value-of select="normalize-space(COL5)"/>
						</PositionStartDate>

						<!--<CurrencySymbol>
							<xsl:value-of select="substring-before($Currency,' ')"/>
						</CurrencySymbol>-->

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>