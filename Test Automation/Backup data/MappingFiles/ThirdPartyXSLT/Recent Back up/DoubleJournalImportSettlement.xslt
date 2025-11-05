<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

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

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="number(COL13)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SSC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL20"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>

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

						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="LocalCurrency">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL11)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="BaseCurrency">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL13)"/>
							</xsl:call-template>
						</xsl:variable>



						<Description>
							<xsl:value-of select="'SPOT'"/>
						</Description>


						<CRCurrencyName>
							<xsl:choose>
								<xsl:when test="$LocalCurrency &lt; 0">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$LocalCurrency &gt; 0">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
							</xsl:choose>
						</CRCurrencyName>

						<DRCurrencyName>
							<xsl:choose>
								<xsl:when test="$LocalCurrency &gt; 0">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$LocalCurrency &lt; 0">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
							</xsl:choose>
						</DRCurrencyName>


						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>

						<xsl:variable name="AbsCashLocal">
							<xsl:choose>
								<xsl:when test="$LocalCurrency &gt; 0">
									<xsl:value-of select="$LocalCurrency"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$LocalCurrency * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="AbsCashBase">
							<xsl:choose>
								<xsl:when test="$BaseCurrency &gt; 0">
									<xsl:value-of select="$BaseCurrency"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$BaseCurrency * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CRFXRate>
							<xsl:choose>
								<xsl:when test="$LocalCurrency &lt; 0">
									<xsl:value-of select = 'format-number(($AbsCashBase div $AbsCashLocal),"#.#######")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</CRFXRate>

						<DRFXRate>
							<xsl:choose>
								<xsl:when test="$LocalCurrency &gt; 0">
									<xsl:value-of select='format-number(($AbsCashBase div $AbsCashLocal),"#.#######")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</DRFXRate>

						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$LocalCurrency &lt; 0">
									<xsl:value-of select="concat( 'Cash:', $AbsCashLocal ,'|', 'Cash:', $AbsCashBase)"/>
								</xsl:when>
								<xsl:when test="$LocalCurrency &gt; 0">
									<xsl:value-of select="concat( 'Cash:', $AbsCashBase ,'|', 'Cash:', $AbsCashLocal)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>
