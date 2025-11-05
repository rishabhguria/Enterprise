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
						<xsl:with-param name="Number" select="COL24"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend) and COL27 = 'DIVIDEND ACCRUAL POSTING'">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
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
						<Amount>
								<xsl:value-of select="$varDividend"/>
						</Amount>
						 <xsl:variable name="varDay1">
                                                 <xsl:value-of select="substring(COL14,7,2)"/>
                                         </xsl:variable>
                                         <xsl:variable name="varYear1">
                                                 <xsl:value-of select="substring(COL14,1,4)"/>
                                         </xsl:variable>

                                         <xsl:variable name="varMonth1">
                                                 <xsl:value-of select="substring(COL14,5,2)"/>
                                         </xsl:variable>
                                         <xsl:variable name="varCurrentDate1">
                                                 <xsl:value-of select="concat($varMonth1,'/',$varDay1, '/', $varYear1)"/>
                                          </xsl:variable>
						<PayoutDate>
							<xsl:value-of select="$varCurrentDate1"/>
						</PayoutDate>
											 <xsl:variable name="varDay">
                                                        <xsl:value-of select="substring(COL13,7,2)"/>
                                                </xsl:variable>
                                                <xsl:variable name="varYear">
                                                        <xsl:value-of select="substring(COL13,1,4)"/>
                                                </xsl:variable>

                                                <xsl:variable name="varMonth">
                                                        <xsl:value-of select="substring(COL13,5,2)"/>
                                                </xsl:variable>
                                                <xsl:variable name="varCurrentDate">
                                                        <xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
                                                </xsl:variable>
						<ExDate>
							<xsl:value-of select="$varCurrentDate"/>
						</ExDate>

						<RecordDate>
							<xsl:value-of select="''"/>
						</RecordDate>


						<CurrencyName>
							<xsl:value-of select="COL10"/>
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