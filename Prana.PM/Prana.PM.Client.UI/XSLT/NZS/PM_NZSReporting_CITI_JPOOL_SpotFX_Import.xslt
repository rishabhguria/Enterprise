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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varFXRate">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:variable name = "PB_Currency" >
							<xsl:value-of select="substring-before(COL1,'')"/>
						</xsl:variable>
						<xsl:variable name = "varCurrFirst" >
							<xsl:value-of select="substring(COL1,1,3)"/>
						</xsl:variable>
						<xsl:variable name = "varCurrSecond" >
							<xsl:value-of select="substring(COL1,4,3)"/>
						</xsl:variable>
				<xsl:if test="number($varFXRate)">
				
					<PositionMaster>
					
					
						<xsl:variable name="varBaseCurrencyAUD">
							<xsl:choose>
								<xsl:when test="contains($varCurrFirst,'AUD')">
									<xsl:value-of select="$varCurrSecond"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varCurrFirst"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<BaseCurrency>
								<xsl:value-of select="'AUD'"/>
						</BaseCurrency>


					
						<SettlementCurrency>
							<xsl:value-of select="$varBaseCurrencyAUD"/>
						</SettlementCurrency>


						<ForexPrice>
								
							<xsl:choose>
								<xsl:when test="$varCurrFirst='EUR' or $varCurrFirst='GBP' or $varCurrFirst='USD'">
									<xsl:value-of select=" 1 div $varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFXRate"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>

						<xsl:variable name="varDate">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<Date>
							<xsl:value-of select="$varDate"/>
						</Date>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>