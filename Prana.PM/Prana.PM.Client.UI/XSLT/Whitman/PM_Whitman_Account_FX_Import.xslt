<?xml version="1.0" encoding="UTF-8"?>

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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'Grays'"/>
				</xsl:variable>

				<xsl:variable name="COL12">
					<xsl:choose>
						<xsl:when test="COL2='JAPANESE YEN' or COL2='SWISS FRANC'">
							<xsl:value-of select="1 div COL5"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL5"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="FXRate">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($FXRate)">

					<PositionMaster>


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
							<xsl:value-of select="''"/>
						</BaseCurrency>

						<SettlementCurrency>
							<!--<xsl:value-of select="COL6"/>-->
						</SettlementCurrency>



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



						<!--<xsl:variable name="Date" select="COL1"/>-->

						<Date>
							<!--<xsl:value-of select="''"/>-->

						</Date>

						<FXConversionMethodOperator>
							<xsl:value-of select="'M'"/>
						</FXConversionMethodOperator>

						<!--<FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test="normalize-space(COL2)='MXN'">
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