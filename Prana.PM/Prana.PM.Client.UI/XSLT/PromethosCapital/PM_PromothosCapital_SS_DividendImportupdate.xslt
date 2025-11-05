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

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL21)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'StateStreet'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
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

						<xsl:variable name="varSedol">
							<xsl:value-of select="substring(normalize-space(COL10),2)"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="$varSedol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test ="$varSedol!=''">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name="varEXDate">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<ExDate>
							<xsl:value-of select="$varEXDate" />
						</ExDate>

						<xsl:variable name="varPYDate">
							<xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="$varPYDate" />
						</PayoutDate>

						<Amount>
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>

						<xsl:variable name="varDividend">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<xsl:variable name="varActivityType">
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="'WithholdingTax'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<ActivityType>
							<xsl:value-of select="$varActivityType"/>
						</ActivityType>

						<xsl:variable name="varDecription">
							<xsl:choose>
								<xsl:when test="$varActivityType='DividendIncome'">
									<xsl:value-of select="'Withholding Tax'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Description>
							<xsl:value-of select="$varDecription"/>
						</Description>

						<xsl:variable name="varCurrency">
							<xsl:choose>
								<xsl:when test="normalize-space(COL7)='AUSTRALIAN DOLLAR'">
									<xsl:value-of select="'AUD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7)='BRAZILIAN REAL'">
									<xsl:value-of select="'BRL'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7)='HONG KONG DOLLAR'">
									<xsl:value-of select="'HKD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7)='JAPANESE YEN'">
									<xsl:value-of select="'JPY'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7)='EURO CURRENCY'">
									<xsl:value-of select="'EUR'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$varCurrency"/>
						</CurrencyName>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>