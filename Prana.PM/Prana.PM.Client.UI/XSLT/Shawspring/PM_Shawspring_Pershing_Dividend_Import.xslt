<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="'SHAWSPRING PARTNERS QP FUND LP'"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="Symbol" select="COL6"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Amount>
								<xsl:value-of select="$varDividend"/>
						</Amount>

						<PayoutDate>
							<xsl:value-of select="COL2"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL5"/>
						</ExDate>

						<RecordDate>
							<xsl:value-of select="''"/>
						</RecordDate>


						<CurrencyName>
							<xsl:value-of select="translate(COL12,$varSmall,$varCapital)"/>
						</CurrencyName>


						<Description>
						 <xsl:choose>
							<xsl:when test="$varDividend &gt; 0">
								<xsl:value-of select="'Dividend Received'" />
							</xsl:when>
							<xsl:when test="$varDividend &lt; 0">
								<xsl:value-of select="'Dividend Charged'" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0" />
							</xsl:otherwise>
							</xsl:choose>
						</Description>

						<ActivityType>
							<xsl:choose>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
                  <xsl:value-of select ="'DividendExpense'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
						</ActivityType>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>