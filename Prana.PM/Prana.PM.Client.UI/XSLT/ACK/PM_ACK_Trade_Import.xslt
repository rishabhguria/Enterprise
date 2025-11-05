<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes" />
	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">


		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				
				<xsl:variable name="varNetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL15)"/>
					</xsl:call-template>
				
				</xsl:variable>
					
				
				<xsl:if test="number($varNetPosition)">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL26"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol !='*' or $varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)" />
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>
						
							
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						
						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
						
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide = 'Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide = 'Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide = 'Cover Short'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
							    <xsl:when test="$varSide = 'Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PositionStartDate>
							<xsl:value-of select="normalize-space(COL10)"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="normalize-space(COL11)"/>
						</PositionSettlementDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varNetPosition &gt; 0">
									<xsl:value-of select="$varNetPosition"/>
								</xsl:when>
								<xsl:when test="$varNetPosition &lt; 0">
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL16)"/>
							</xsl:call-template>
						</xsl:variable>
						
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSCurrency">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>
						
						<SettlCurrencyName>
							<xsl:value-of select="$varSCurrency"/>
						</SettlCurrencyName>

						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL18)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL19)"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees" />
								</xsl:when>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						
					    <xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID!='')">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>