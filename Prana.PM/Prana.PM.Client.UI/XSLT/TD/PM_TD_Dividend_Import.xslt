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
              <xsl:value-of select="'TD Securities'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select="normalize-space(COL3)"/>
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

            <xsl:variable name="varSymbol" select="COL2"/>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <Amount>
              <xsl:choose>
                <xsl:when test="number($varDividend)">
                  <xsl:value-of select="$varDividend"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>

            <xsl:variable name="varPayoutDate">
              <xsl:choose>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) ">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD')">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <PayoutDate>
              <xsl:value-of select="$varPayoutDate"/>
            </PayoutDate>

            <xsl:variable name="varExDate">
              <xsl:choose>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) ">
                  <xsl:value-of select="COL4"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD')">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <ExDate>
              <xsl:value-of select="$varExDate"/>
            </ExDate>
            
            <xsl:variable name="varRecordDate">
              <xsl:choose>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) ">
                  <xsl:value-of select="COL5"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD')">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <RecordDate>
              <xsl:value-of select="$varRecordDate"/>
            </RecordDate>
            <Currency>
              <xsl:value-of select="'USD'"/>
            </Currency>
			<CurrencyID>
              <xsl:value-of select="1"/>
            </CurrencyID>


            <xsl:variable name="varDescription">
              <xsl:choose>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) and $varDividend &lt; 0">
                  <xsl:value-of select="'Dividend expense'"/>
                </xsl:when>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) and $varDividend &gt; 0">
                  <xsl:value-of select="'Dividend Income'"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD') and $varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD') and $varDividend &gt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            
            
            <Description>
              <xsl:value-of select="$varDescription"/>
            </Description>
            

            <xsl:variable name="varActivity">
              <xsl:choose>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) and $varDividend &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="not(contains(COL3,'WITHHOLD')) and $varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD') and $varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                <xsl:when test="contains(COL3,'WITHHOLD') and $varDividend &gt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <ActivityType>
              <xsl:value-of select="$varActivity"/>
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