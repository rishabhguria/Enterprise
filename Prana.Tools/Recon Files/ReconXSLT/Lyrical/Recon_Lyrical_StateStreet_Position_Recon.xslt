<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position" select="COL18"/>

				<xsl:if test="number($Position) and normalize-space(COL7)='EQUITY'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Statestreet'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL12)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="Cusip" select="substring(COL11,2)"/>

						<CUSIP>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Cusip!=''">
									<xsl:value-of select="$Cusip"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</CUSIP>

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

						<xsl:variable name="Cost" select="number(COL22)"/>

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

						<xsl:variable name="MarketValue" select="COL23"/>

						<MarketValue>
							<xsl:choose>

								<xsl:when test="number($MarketValue) ">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<!--<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (-1)"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<TradeDate>
							<xsl:value-of select="COL4"/>
						</TradeDate>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>