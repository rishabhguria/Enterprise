<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
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
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MapleRock'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="BeginningQuantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($BeginningQuantity)">
									<xsl:value-of select="$BeginningQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>


						<xsl:variable name="EndingQuantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<CashValueBase>
							<xsl:choose>
								<xsl:when test="number($EndingQuantity)">
									<xsl:value-of select="$EndingQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueBase>




						<Symbol>
							<xsl:value-of select="normalize-space(COL5)"/>
						</Symbol>

					

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>
