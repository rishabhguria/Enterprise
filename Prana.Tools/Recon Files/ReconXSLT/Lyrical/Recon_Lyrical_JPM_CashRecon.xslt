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

				<xsl:variable name ="PB_NAME">
					<xsl:value-of select ="'JPM'"/>
				</xsl:variable>

				<!--   Fund -->
				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="substring(COL2,1,8)"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JPM']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<!---->

				<xsl:if test ="(COL4 = 'A' or (COL4 = 'B' and COL19 = 'JPMORGAN DEPOSIT ACCT B       ')) and $PRANA_FUND_NAME != ''">
					<PositionMaster>

						

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<!--<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>-->

						<!--<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="substring(COL1,332,2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_CURRENCY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyCode=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
						</xsl:variable>-->

						<Symbol>
							<xsl:value-of select ="'USD'"/>
						</Symbol>

						<xsl:variable name ="varCashValLocal">
							<xsl:choose>
								<xsl:when test ="COL4 = 'A' ">
									<xsl:value-of select="number(COL42)*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(COL20)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<EndingQuantity>
							<xsl:choose>
								<xsl:when test ="number($varCashValLocal)">
									<xsl:value-of select="$varCashValLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >

						</EndingQuantity>

						

					
						<!--<Date>
							<xsl:value-of select="''"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>-->

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
