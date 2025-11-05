<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:value-of select="normalize-space(substring(COL1,103,12))"/>
        </xsl:variable>

        <xsl:variable name="varEntryCheck1">
          <xsl:value-of select="normalize-space(substring(COL1,14,2))"/>
        </xsl:variable>

        <xsl:variable name="varEntryCheck2">
          <xsl:value-of select="normalize-space(substring(COL1,20,4))"/>
        </xsl:variable>
        <xsl:if test="number($varDividend) and $varEntryCheck1='lo' and $varEntryCheck2='cash'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="normalize-space(substring(COL1,20,4))"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(substring(COL1,1,8))"/>
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

            <PBSymbol>
              <xsl:value-of select="$PB_Symbol"/>
            </PBSymbol>

            <Amount>
              <xsl:value-of select="$varDividend"/>
            </Amount>

            <xsl:variable name="varDate">
              <xsl:value-of select="normalize-space(substring(COL1,33,8))"/>
            </xsl:variable>
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring($varDate,1,2)"/>
            </xsl:variable>
            <xsl:variable name="varDay">
              <xsl:value-of select="substring($varDate,3,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring($varDate,5)"/>
            </xsl:variable>
            <PayoutDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </PayoutDate>

            <RecordDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </RecordDate>

            <ExDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </ExDate>

            <Description>
              <xsl:choose>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'Cash Deposit'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
                  <xsl:value-of select ="'Cash Withdraw'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
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
