<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
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

			<xsl:variable name="varCurrency" select="(//Comparision[COL2][4]/COL2[child::node()[1]])"/>

			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//Comparision">

				<xsl:variable name="varCashValueLocal" select="number(COL3)"/>

				<xsl:if test ="$varCashValueLocal and normalize-space(COL2)='PNC GOVT. MONEY MARKET FUND #405'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'PNC'"/>
						</xsl:variable>

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

						<Symbol>
							<xsl:value-of select="$varCurrency"/>
						</Symbol>


						<EndingQuantity>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingQuantity>

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
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
