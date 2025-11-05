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

        <xsl:variable name="NetAmount">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL14,'$','')"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetAmount)">
          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            
            <AccountName>
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select='$PB_FUND_NAME'/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
              </xsl:otherwise>
            </xsl:choose>
            </AccountName>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            
            <Amount>
              <xsl:value-of select="$NetAmount"/>
            </Amount>
            <PayoutDate>
              <xsl:value-of select="normalize-space(COL8)"/>
            </PayoutDate>
            <ExDate>
              <xsl:value-of select="normalize-space(COL6)"/>
            </ExDate>

            <RecordDate>
              <xsl:value-of select="normalize-space(COL7)"/>
            </RecordDate>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$NetAmount &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test="$NetAmount &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ActivityType>

            <Description>
              <xsl:choose>
                <xsl:when test="$NetAmount &gt; 0">
                  <xsl:value-of select="'Dividend Received'"/>
                </xsl:when>
                <xsl:when test="$NetAmount &lt; 0">
                  <xsl:value-of select="'Dividend Charged'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>

          </PositionMaster>

        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
