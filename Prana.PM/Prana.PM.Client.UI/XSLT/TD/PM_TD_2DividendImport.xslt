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
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varDividend)">
          <PositionMaster>
            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
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

          
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

           
			 <xsl:variable name="varActivityType" select="COL9"/>
			 
            <Amount>
              <xsl:choose>
                <xsl:when test="number($varDividend) and $varActivityType = 'DividendExpense'">
                  <xsl:value-of select="$varDividend * (-1)"/>
                </xsl:when>
				<xsl:when test="number($varDividend) and $varActivityType = 'DividendIncome'">
                  <xsl:value-of select="$varDividend"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>
            <PayoutDate>
              <xsl:value-of select="concat(substring(COL6,5,2),'/',substring(COL6,7,2),'/',substring(COL6,1,4))"/>
            </PayoutDate>
            <ExDate>
              <xsl:value-of select="concat(substring(COL5,5,2),'/',substring(COL5,7,2),'/',substring(COL5,1,4))"/>
            </ExDate>
			 
            <RecordDate>
              <xsl:value-of select="concat(substring(COL7,5,2),'/',substring(COL7,7,2),'/',substring(COL7,1,4))"/>
            </RecordDate>
			
			<xsl:variable name="PB_Name_Curr">
              <xsl:value-of select="'SG'"/>
            </xsl:variable>
			
            <xsl:variable name="PB_CURRENCY_NAME" select="translate(COL10,$varSmall,$varCapital)"/>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_Name_Curr]/CurrencyData[@PranaCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>
            <CurrencyID>
              <xsl:choose>
                <xsl:when test="$PRANA_CURRENCY_ID !=''">
                  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>
            
            <Description>
			  <xsl:value-of select="$varActivityType"/>
            </Description>
            <ActivityType>
              <xsl:value-of select="$varActivityType"/>
            </ActivityType>
            <PBSymbol>
              <xsl:value-of select="$PB_Symbol"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>