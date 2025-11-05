<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:my1="put-your-namespace-uri-here"
  xmlns:my2="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

		<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL22)"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						 <xsl:variable name="PB_FUND_NAME">
							 <xsl:value-of select="'EMJ Master Fund LP'"/>
						 </xsl:variable>
					     <xsl:variable name="PRANA_FUND_NAME">
					       <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:variable name="PB_SYMBOL_NAME" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="varSymbol" select="normalize-space(COL5)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
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

						<!--20240318-->
						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL12,7,2)"/>
						</xsl:variable>
						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL12,1,4)"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL12,5,2)"/>
						</xsl:variable>
						<xsl:variable name="varCurrentDate">
							<xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
						</xsl:variable>
						<ExDate>
							<xsl:value-of select="$varCurrentDate" />
						</ExDate>

						<RecordDate>
							<xsl:value-of select="$varCurrentDate" />
						</RecordDate>
						
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
							<xsl:value-of select="$varCurrentDate1" />
						</PayoutDate>
						
						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyID"/>
						</xsl:variable>
						<CurrencyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CURRENCY_ID)">
									<xsl:value-of select="$PRANA_CURRENCY_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyID>
					

						<ActivityType>
							<xsl:choose>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'DividendExpense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>
						
						<Description>
							<xsl:choose>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="'Dividend Income'"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'Dividend Expense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>