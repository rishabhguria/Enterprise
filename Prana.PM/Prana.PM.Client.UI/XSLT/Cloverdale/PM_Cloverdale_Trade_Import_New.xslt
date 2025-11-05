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

  <xsl:template name="DateFormate">
    <xsl:param name="Date"/>
    <xsl:choose>
      <xsl:when test="contains($Date,'-')">
        <xsl:value-of select="concat(substring-before($Date,'-'),'/',substring-before(substring-after($Date,'-'),'-'),'/',substring-after(substring-after($Date,'-'),'-'))"/>
      </xsl:when>
      <xsl:when test="contains($Date,'/')">
        <xsl:value-of select="concat(substring-before($Date,'/'),'/',substring-before(substring-after($Date,'/'),'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Position)">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>


            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'Buy to Open'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'Buy to Close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varAvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varAvgPrice &gt; 0">
                  <xsl:value-of select="$varAvgPrice"/>
                </xsl:when>
                <xsl:when test="$varAvgPrice &lt; 0">
                  <xsl:value-of select="$varAvgPrice*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommissionRate">
              <xsl:choose>
                <xsl:when test="number(COL9)">
                  <xsl:value-of select="COL9"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:choose>
                        <xsl:when test="COL6='COWR'">
                          <xsl:value-of select="'0.01'"/>
                        </xsl:when>
                        <xsl:when test="COL6='COWNA'">
                          <xsl:value-of select="'0.0225'"/>
                        </xsl:when>
                        <xsl:when test="COL6='COCT'">
                          <xsl:value-of select="'0.0375'"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="0"/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <Commission>
              <xsl:choose>
                    <xsl:when test="$Position &lt; 0">
                      <xsl:value-of select="$Position * (-1) * $varCommissionRate "/>
                    </xsl:when>
                    <xsl:when test="$Position &gt; 0">
                      <xsl:value-of select="$Position * $varCommissionRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
            </Commission>

            <xsl:variable name="varSettlDate">
              <xsl:call-template name="DateFormate">
                <xsl:with-param name="Date" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="$varSettlDate"/>
            </SettlementDate>


            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>
            
            <xsl:variable name="varTradeDate">
              <xsl:call-template name="DateFormate">
                <xsl:with-param name="Date" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varTradeDate"/>
            </PositionStartDate>
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
