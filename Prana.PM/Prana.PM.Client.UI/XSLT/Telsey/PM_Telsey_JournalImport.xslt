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
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL3)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varCash) and (COL2!='' and COL2!='*')">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            
            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                
                <xsl:when test="$varSymbol!='' and $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="normalize-space(COL11)"/>
            </xsl:variable>
            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

            <xsl:variable name = "varAmount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="concat('Cash', ':', $varAmount , '|', 'WT' ,':', $varAmount)"/>
                </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="concat('WT', ':', $varAmount , '|', 'Cash' ,':', $varAmount)"/>
              </xsl:otherwise>
              </xsl:choose>
            </JournalEntries>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring-after(substring-after(normalize-space(COL1),'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring-before(substring-after(normalize-space(COL1),'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring-before(normalize-space(COL1),'-')"/>
            </xsl:variable>
            
            <Date>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </Date>

            <Description>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>