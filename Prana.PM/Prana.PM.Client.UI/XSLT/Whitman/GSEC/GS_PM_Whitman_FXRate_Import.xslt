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
					<xsl:value-of select="'GSEC'"/>
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


						
							
					
						<BaseCurrency>
							<xsl:value-of select="COL7"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:choose>
								<xsl:when test="contains(COL2,'JAPANESE')">
									<xsl:value-of select="'JPY'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'SWISS FRANC')">
									<xsl:value-of select="'CHF'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'BRITISH POUND STERLING')">
									<xsl:value-of select="'GBP'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'EURO')">
									<xsl:value-of select="'EUR'"/>
								</xsl:when>
								<xsl:when test="contains(COL2,'US DOLLAR')">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
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

						<!--<FXConversionMethodOperator>
							<xsl:value-of select="'M'"/>
						</FXConversionMethodOperator>-->

						<FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test="normalize-space(COL2)='BRITISH POUND STERLING' or normalize-space(COL2)='EURO'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'M'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXConversionMethodOperator>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>