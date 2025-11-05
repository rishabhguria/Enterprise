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
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'Brandywine'"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
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

						<xsl:variable name="Symbol" select="COL2"/>

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

						<xsl:variable name="varAmount">
							<xsl:choose>
								<xsl:when test="contains(COL12,'GBp') or contains(COL12,'ZAr')">
									<xsl:value-of select="$varDividend div 100"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varDividend"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Amount>
							<xsl:choose>
								<xsl:when test="contains(COL11,'Income')">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$varAmount * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>

						<PayoutDate>
							<xsl:value-of select="COL7"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL6"/>
						</ExDate>

						<RecordDate>
							<xsl:value-of select="COL8"/>
						</RecordDate>


						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="COL12"/>
						</xsl:variable>

						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_Name]/CurrencyData[@PranaCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
						</xsl:variable>

						<CurrencyID>
							<!--<xsl:choose>
								<xsl:when test="$PRANA_CURRENCY_ID !=''">
									<xsl:value-of select="$PRANA_CURRENCY_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:choose>
								<xsl:when test="COL12 ='USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL12 ='GBP' or COL12 ='GBp'">
									<xsl:value-of select="'4'"/>
								</xsl:when>
								<xsl:when test="COL12 ='CAD'">
									<xsl:value-of select="'7'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</CurrencyID>

					
						<Description>
							<xsl:value-of select="'Dividend'"/>
						</Description>

						<ActivityType>
							<xsl:choose>

								<xsl:when test="contains(COL11,'Income')">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test="contains(COL11,'Expense')">
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