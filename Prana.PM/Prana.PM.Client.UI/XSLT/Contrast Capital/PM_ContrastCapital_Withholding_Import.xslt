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
						<xsl:with-param name="Number" select="normalize-space(COL19)"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
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
						
                        <xsl:variable name = "varSymbol" >
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<SEDOL>
							<xsl:choose>
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>
						
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
						
						<xsl:variable name="varCurrency">
							<xsl:choose>
								<xsl:when test="normalize-space(COL7) = 'BRAZIL REAL'">
									<xsl:value-of select="'BRL'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'EURO CURRENCY'">
									<xsl:value-of select="'EUR'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'JAPANESE YEN'">
									<xsl:value-of select="'JPY'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'POUND STERLING'">
									<xsl:value-of select="'GBP'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'US DOLLAR'">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'SINGAPORE DOLLAR'">
									<xsl:value-of select="'SGD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'DANISH KRONE'">
									<xsl:value-of select="'DKK'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'AUSTRALIAN DOLLAR'">
									<xsl:value-of select="'AUD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL7) = 'CANADIAN DOLLAR'">
									<xsl:value-of select="'CAD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>	

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL6)"/>
							</xsl:call-template>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="normalize-space(COL7)='AUSTRALIAN DOLLAR' or normalize-space(COL7)='POUND STERLING' or normalize-space(COL7)='EURO CURRENCY'">
									<xsl:value-of select="COL23 div COL19"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL19 div COL23"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>
						
						<xsl:variable name="varPayoutDate">
							<xsl:value-of select="normalize-space(COL15)"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="$varPayoutDate"/>
						</PayoutDate>

						<xsl:variable name="varEXDate">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>
																					
						<ExDate>
							<xsl:value-of select="$varEXDate"/>
						</ExDate>												

						<CurrencyName>
							<xsl:value-of select="$varCurrency"/>
						</CurrencyName>

						<Description>
							<xsl:value-of select="'Dividend Received'"/>
						</Description>

						<ActivityType>							
								<xsl:value-of select="'DividendIncome'"/>
						</ActivityType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>