<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	

	<xsl:template match="/">
		<DocumentElement>			

			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//Comparision">

				<xsl:variable name="varCashValueLocal" select="number(COL6)"/>

				<xsl:if test ="number($varCashValueLocal) and not(contains(COL5,'FORWARD'))">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'MapleRock'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
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

						
						
						<xsl:variable name = "PB_CURRENCY_NAME">
						<xsl:value-of select="normalize-space(COL4)"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_CURRENCY_NAME">
						<xsl:value-of select ="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyID"/>
					</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_CURRENCY_NAME!=''">
									<xsl:value-of select ="$PRANA_CURRENCY_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_CURRENCY_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="CashValueLocal" select ="COL6"/>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when  test="number($CashValueLocal)">
									<xsl:value-of select="$CashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CashValueLocal>						
					
						<!--<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
