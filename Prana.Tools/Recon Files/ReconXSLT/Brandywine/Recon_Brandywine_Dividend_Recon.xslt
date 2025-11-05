<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

				<xsl:variable name ="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL27"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend) and (COL77!= '' or COL79!='' ) and (contains(COL6,'60-002344')='true' or contains(COL6,'60-002417')='true' or contains(COL6,'60-000472')='true' or contains(COL6,'60-210146')='true' or contains(COL6,'60-000474')='true' or contains(COL6,'60-000437')='true' or contains(COL6,'60-840047')='true' or contains(COL6,'60-000437')='true' or contains(COL6,'60-000533')='true')">
					<!--and (COL77!= '' or COL79!='' )-->
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'State Street'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="PB_SYMBOL_NAME" select="normalize-space(COL28)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL6!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<Dividend>
								<xsl:choose>
								<xsl:when test="number($varDividend)">
									<xsl:value-of select="$varDividend"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Dividend>

						<PayoutDate>
							<xsl:value-of select="concat(substring(COL79,5,2),'/',substring(COL79,7,2),'/',substring(COL79,1,4))"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="concat(substring(COL77,5,2),'/',substring(COL77,7,2),'/',substring(COL77,1,4))"/>
						</ExDate>

						<CashValueLocal>
							<xsl:value-of select="(COL27 - COL48) * -1"/>
						</CashValueLocal>

						<Description>
							<xsl:choose>
								<xsl:when test="$varDividend &gt;0">
									<xsl:value-of select="'Dividend Income'"/>
								</xsl:when>
								<xsl:when test="$varDividend &lt;0">
									<xsl:value-of select="'Dividend Expense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						<Currency>
							<xsl:value-of select="COL7"/>

						</Currency>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>