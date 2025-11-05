<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>
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
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Cashlocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL9"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($Cashlocal)">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Velocity'"/>
						</xsl:variable>

						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="concat(COL2,'-',COL3,'-',COL4,'-',COL6)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>


						<xsl:variable name ="varCurrency">
							<xsl:value-of select ="normalize-space(COL8)"/>
						</xsl:variable>

						<Currency>
							<xsl:value-of select="$varCurrency"/>
						</Currency>						

						<xsl:variable name ="varCashLocal">
							<xsl:value-of select ="normalize-space(COL9)"/>
						</xsl:variable>
						
						

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test ="$varCashLocal &lt;0">
									<xsl:value-of select ="$varCashLocal*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varCashLocal &gt;0">
									<xsl:value-of select ="$varCashLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CashValueLocal>
						
						

						<Symbol>
							<xsl:value-of select="COL8"/>
						</Symbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>