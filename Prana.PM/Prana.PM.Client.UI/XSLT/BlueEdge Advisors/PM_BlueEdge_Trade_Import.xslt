<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL4)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varQuantity)" >
          <PositionMaster>
            
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="GS"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>
              

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>
            <xsl:variable name="varBloomberg">
              <xsl:value-of select="COL6"/>
            </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varBloomberg!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>
           
            <Bloomberg>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varBloomberg!=''">
                  <xsl:value-of select="$varBloomberg"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Bloomberg>
              <xsl:variable name="PB_FUND_NAME" select="''"/>

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
            
              <xsl:variable name="varSide">
                <xsl:value-of select="COL3" />
              </xsl:variable>
              <SideTagValue>
                <xsl:choose>
                  <xsl:when test="$varSide='SOLD'">
                    <xsl:value-of select="'2'"/>
                  </xsl:when>

                  <xsl:when test="$varSide='BOT'">
                    <xsl:value-of select="'1'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SideTagValue>
            
              <NetPosition>
                <xsl:choose>
                  <xsl:when test="$varQuantity &gt; 0">
                    <xsl:value-of select="$varQuantity"/>
                  </xsl:when>
                  <xsl:when test="$varQuantity &lt; 0">
                    <xsl:value-of select="$varQuantity * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetPosition>


            
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="'COL7'"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>
            <xsl:variable name="varStrategy">
              <xsl:value-of select="COL11" />
            </xsl:variable>
            <Strategy>
              <xsl:value-of select="$varStrategy" />
            </Strategy>
            <xsl:variable name="varTradeDate">
              <xsl:value-of select="''" />
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="$varTradeDate"/>
            </TradeDate>
            <xsl:variable name="varBroker">
              <xsl:value-of select="0" />
            </xsl:variable>
            <CounterPartyID>
              <xsl:value-of select="$varBroker"/>
            </CounterPartyID>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  
</xsl:stylesheet>
