<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


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

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL21"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and (normalize-space(COL18)='settle' or normalize-space(COL18)='cleanup' or normalize-space(COL18)='reclaim')">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'ClearStreet'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="concat(COL6,' ',COL4)"/>
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

            <CounterPartyID>
              <xsl:value-of select ="'1'"/>
            </CounterPartyID>

            <xsl:variable name="Symbol">
              <xsl:value-of select="COL9"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>
            </PBSymbol>

            <xsl:variable name="AvgPX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL32 div COL21"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$AvgPX &gt; 0">
                  <xsl:value-of select="$AvgPX"/>
                </xsl:when>
                <xsl:when test="$AvgPX &lt; 0">
                  <xsl:value-of select="$AvgPX * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varDate">
              <xsl:value-of select ="concat(substring(COL20,5,2),'/',substring(COL20,7,2),'/',substring(COL20,1,4))"/>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="$varDate"/>
            </PositionStartDate>
            
            <PositionSettlementDate>
              <xsl:value-of select="$varDate"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL13='Common Equity'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL13='Composite Unit'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL13='Exchange Traded Fund'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL13='Mutual Fund/Unit Investment Trust'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL13='Preferred Equity'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL13='Right'">
                  <xsl:value-of select="'PrivateEquity'"/>
                </xsl:when>
                <xsl:when test="COL13='Warrant'">
                  <xsl:value-of select="'PrivateEquity'"/>
                </xsl:when>
                <xsl:when test="COL13='Government/Agency Bond'">
                  <xsl:value-of select="'fixedIncome'"/>
                </xsl:when>
                <xsl:when test="COL13='Corporate Bond'">
                  <xsl:value-of select="'fixedIncome'"/>
                </xsl:when>
                <xsl:when test="COL13='Barrier Option'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="Side">
              <xsl:value-of select="COL16"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side = 'buy' and (normalize-space(COL18)='settle' or normalize-space(COL18)='cleanup')">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$Side = 'sell' and(normalize-space(COL18)='settle' or normalize-space(COL18)='cleanup')">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Side = 'buy' and normalize-space(COL18)='reclaim'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Side = 'sell' and normalize-space(COL18)='reclaim'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


