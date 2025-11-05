<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="varMonth">
		<xsl:param name="MonthName"/>
		<xsl:choose>
			<xsl:when test="$MonthName='June'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$MonthName='Jul'">
				<xsl:value-of select="7"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:variable name="varFund" select="(//Comparision[COL2][2]/COL2[child::node()[1]])"/>
			
			<xsl:variable name="varDate" select="(//Comparision[COL2][3]/COL2[child::node()[1]])"/>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="NetPosition">
					<xsl:value-of select ="COL3"/>
				</xsl:variable>

				<xsl:if test ="number($NetPosition) and COL2!='CASH' and COL10!='I'">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'PNC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL1!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL1!=''">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>
						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="$varFund"/>
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

						<xsl:variable name="MonthNo">
							<xsl:call-template name="varMonth">
								<xsl:with-param name="MonthName" select="substring-before(substring-after($varDate,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="vardateCode" select="substring-before($varDate,'-')"/>

						<xsl:variable name="varYearCode" select="number(substring-after(substring-after($varDate,'-'),'-'))"/>

						<TradeDate>
							<xsl:value-of select="concat($MonthNo,'/',$vardateCode,'/',$varYearCode)"/>
						</TradeDate>

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

						<xsl:variable name="varSide" select="COL3"/>

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

						<xsl:variable name ="MarkPrice">
							<xsl:value-of select="number(COL4)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name ="MarketValue">
							<xsl:value-of select="number(COL6)"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>

								<xsl:when test ="$MarketValue &lt;0">
									<xsl:value-of select ="$MarketValue*-1"/>
								</xsl:when>

								<xsl:when test ="$MarketValue &gt;0">
									<xsl:value-of select ="$MarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

