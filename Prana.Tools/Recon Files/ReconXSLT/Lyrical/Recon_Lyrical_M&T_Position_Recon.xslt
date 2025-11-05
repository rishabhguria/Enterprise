<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:variable name = "PB_NAME">
				<xsl:value-of select="'M and T'"/>
			</xsl:variable>
			
			<xsl:for-each select ="//Comparision">

				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="normalize-space(substring(COL1,2,11))"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test ="$PRANA_FUND_NAME!='' and number(normalize-space(substring(COL1,127,16))) and normalize-space(substring(COL1,221,11))!='WGOXX'">

					<PositionMaster>

						

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(substring(COL1,242,35))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="string-length(substring(COL1,221,11) &gt;1)">
									<xsl:value-of select ="normalize-space(substring(COL1,221,11))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						

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
							<xsl:value-of select ="concat(substring(COL1,663,2),'/',substring(COL1,665,2),'/',substring(COL1,659,4))"/>
						</PositionStartDate>


						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="number(normalize-space(substring(COL1,127,16)))"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>

								<xsl:when test ="number($varQuantity)">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>

						<Side>
							<xsl:choose>

								<xsl:when test ="$varQuantity &gt;0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<xsl:variable name ="varAvgPX">
							<xsl:value-of select ="number(normalize-space(substring(COL1,179,14)))"/>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>

								<xsl:when test ="$varAvgPX &lt;0">
									<xsl:value-of select ="$varAvgPX*-1"/>
								</xsl:when>

								<xsl:when test ="$varAvgPX &gt;0">
									<xsl:value-of select ="$varAvgPX"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(substring(COL1,179,14))"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name ="varMarketValue">
							<xsl:value-of select ="number(substring(COL1,644,14))"/>
						</xsl:variable>

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

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
