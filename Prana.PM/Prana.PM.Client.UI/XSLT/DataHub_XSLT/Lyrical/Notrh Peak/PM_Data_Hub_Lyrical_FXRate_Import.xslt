<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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


	<xsl:template name="ScientificToNumber">
		<xsl:param name="ScientificN"/>
		<xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
		<xsl:variable name="vFactor"
					select="substring('100000000000000000000000000000000000000000000',
                              1, substring($vExponent,2) + 1)"/>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="FXRate">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL35"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($FXRate)">

					<PositionMaster>


						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<xsl:variable name="PRANA_CURRENCY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSettleCurrencyName=$PB_CURRENCY_NAME]/@SettleCurrency"/>
						</xsl:variable>
						<SettlementCurrency>

							<xsl:choose>
								<xsl:when test="$PRANA_CURRENCY_NAME!=''">
									<xsl:value-of select="$PRANA_CURRENCY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_CURRENCY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrency>


						<AccountName>
							<xsl:value-of select="COL6"/>
						</AccountName>
						

						<ForexPrice>
							<xsl:choose>
								<xsl:when test="$FXRate &gt; 0">
									<xsl:value-of select="$FXRate"/>

								</xsl:when>
								<xsl:when test="$FXRate &lt; 0">
									<xsl:value-of select="$FXRate * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>

							</xsl:choose>
						</ForexPrice>



						<xsl:variable name="Date" select="''"/>

						<Date>
							<xsl:value-of select="''"/>

						</Date>

						<FXConversionMethodOperator>
							<xsl:value-of select="'D'"/>
						</FXConversionMethodOperator>

						<!--<FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test="normalize-space(COL6)='CAD' or normalize-space(COL6)='MXN'">
                  <xsl:value-of select="'M'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'D'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXConversionMethodOperator>-->

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>