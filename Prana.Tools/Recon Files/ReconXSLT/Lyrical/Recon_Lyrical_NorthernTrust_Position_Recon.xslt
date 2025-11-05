<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="varCurrency">
		<xsl:param name="Currency"/>
		<xsl:choose>
			<xsl:when test="$Currency='United States dollar'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL10) and substring(COL18,1,4) != 'Cash'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME" >
							<xsl:value-of select ="'NorthernTrust'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select="'SEDOL'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="substring(COL24,2)"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="substring(COL24,2)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="COL10"/>
						</xsl:variable>

						<Quantity>

							<xsl:choose>
								<xsl:when test ="number($NetPosition)">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Quantity>


						<Side>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name ="varCostBasis" select ="number(COL4)"/>

						<MarkPrice>
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
						</MarkPrice>

						<TradeDate>
							<xsl:value-of select ="COL3"/>
						</TradeDate>

						<xsl:variable name ="varMarketValue" select ="number(COL5)"/>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="$varMarketValue &lt;0">
									<xsl:value-of select ="$varMarketValue*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarketValue &gt;0">
									<xsl:value-of select ="$varMarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="varCurrency">
							<xsl:call-template name="varCurrency">
								<xsl:with-param name="Currency" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<CurrencySymbol>
							<xsl:value-of select="$varCurrency"/>
						</CurrencySymbol>


						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<SMRequest>
							<xsl:value-of select ="'true'"/>
						</SMRequest>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
