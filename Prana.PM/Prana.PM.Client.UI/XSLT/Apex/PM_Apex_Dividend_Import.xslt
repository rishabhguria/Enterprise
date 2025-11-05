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
            <xsl:with-param name="Number" select="COL19"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend)">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="COL8"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="ExDate">
              <xsl:choose>

                <xsl:when test="contains(normalize-space(COL12),'-')">
                  <xsl:value-of select="concat(substring-before(normalize-space(COL12),'-'),'/',substring-before(substring-after(normalize-space(COL12),'-'),'-'),'/',substring-after(substring-after(normalize-space(COL12),'-'),'-'))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL12)"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PayOutDate">
              <xsl:choose>

                <xsl:when test="contains(normalize-space(COL14),'-')">
                  <xsl:value-of select="concat(substring-before(normalize-space(COL14),'-'),'/',substring-before(substring-after(normalize-space(COL14),'-'),'-'),'/',substring-after(substring-after(normalize-space(COL14),'-'),'-'))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL14)"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="RecordDate">
              <xsl:choose>

                <xsl:when test="contains(normalize-space(COL13),'-')">
                  <xsl:value-of select="concat(substring-before(normalize-space(COL13),'-'),'/',substring-before(substring-after(normalize-space(COL13),'-'),'-'),'/',substring-after(substring-after(normalize-space(COL13),'-'),'-'))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL13)"/>
                </xsl:otherwise>

              </xsl:choose>
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

            <Amount>
              <xsl:value-of select="$varDividend"/>
            </Amount>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </ActivityType>

            <Description>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </Description>

            <PayoutDate>
              <xsl:choose>
                <xsl:when test="string-length(substring-before($PayOutDate,'/'))=1">
                  <xsl:value-of select="concat('0',$PayOutDate)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PayOutDate"/>
                </xsl:otherwise>
              </xsl:choose>
            </PayoutDate>

            <ExDate>
              <xsl:choose>
                <xsl:when test="string-length(substring-before($ExDate,'/'))=1">
                  <xsl:value-of select="concat('0',$ExDate)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$ExDate"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExDate>

            <RecordDate>
              <xsl:choose>
                <xsl:when test="string-length(substring-before($RecordDate,'/'))=1">
                  <xsl:value-of select="concat('0',$RecordDate)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$RecordDate"/>
                </xsl:otherwise>
              </xsl:choose>
            </RecordDate>
            
            <CurrencyName>
              <xsl:value-of select="COL5"/>
            </CurrencyName>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>