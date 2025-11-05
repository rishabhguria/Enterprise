<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Apr'">
				<xsl:value-of select="04"/>
			</xsl:when>
			<xsl:when test="$varMonth=May">
				<xsl:value-of select="05"/>
			</xsl:when>
			<xsl:when test="$varMonth=Jun">
				<xsl:value-of select="06"/>
			</xsl:when>
			<xsl:when test="$varMonth=Jul">
				<xsl:value-of select="07"/>
			</xsl:when>
			<xsl:when test="$varMonth='Aug'">
				<xsl:value-of select="08"/>
			</xsl:when>
			<xsl:when test="$varMonth='Sep'">
				<xsl:value-of select="09"/>
			</xsl:when>
			<xsl:when test="$varMonth='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$varMonth='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$varMonth='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varCashValueLocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL9"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varCashValueLocal) and COL7='Buying Power' and COL8='Cash Only'">
					
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Lyrical'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>




						<CashValueLocal>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CashValueLocal>

						<xsl:variable name ="Day">
							<xsl:value-of select="substring-before(COL5,'-')"/>
						</xsl:variable>
						
						<xsl:variable name ="Month">
							<xsl:call-template name="MonthsCode">
								<xsl:with-param name="varMonth" select="substring-before(substring-after(COL5,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name ="Year">
							<xsl:value-of select="substring-before(substring-after(substring-after(COL5,'-'),'-'),' ')"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
