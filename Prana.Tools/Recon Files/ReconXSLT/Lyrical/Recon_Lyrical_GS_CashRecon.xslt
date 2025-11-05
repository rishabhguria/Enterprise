<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Currency">
		<xsl:param name="varCurrency"/>
		<xsl:choose>
			<xsl:when test="$varCurrency='CANADIAN DOLLAR'">
				<xsl:value-of select="'CAD'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='NORWEGIAN KRONE'">
				<xsl:value-of select="'NOK'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='SWEDISH KRONA'">
				<xsl:value-of select="'SEK'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='U S DOLLAR'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='UK POUND STERLING'">
				<xsl:value-of select="'GBP'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="Comparision">
				<xsl:if test ="number(COL6)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						

						<xsl:variable name="varCashValueLocal">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:call-template name="Currency">
								<xsl:with-param name="varCurrency" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="$varCurrency"/>
						</Symbol>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test ="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>					

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
