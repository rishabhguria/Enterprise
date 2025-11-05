<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position" select="COL16"/>

				<xsl:if test="number($Position) and not(contains(COL50,'CASH'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BNY_WB'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL8)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL31!=''">
									<xsl:value-of select="COL31"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

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

						<Side>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<Quantity>
							<xsl:value-of select="$Position"/>
						</Quantity>

						<xsl:variable name="Cost" select="number(COL24)"/>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>

								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<TradeDate>
							<xsl:value-of select="COL28"/>
						</TradeDate>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="MarketValue" select="number(COL42)"/>

						<MarketValue>
							<xsl:choose>
								
								<xsl:when test="$MarketValue">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<!--<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="MarketValue * (-1)"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
								
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="MarketValueBase" select="number(COL41)"/>

						<MarketValueBase>
							<xsl:choose>

								<xsl:when test="$MarketValueBase">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>

								<!--<xsl:when test="$MarketValueBase &lt; 0">
									<xsl:value-of select="MarketValueBase * (-1)"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>