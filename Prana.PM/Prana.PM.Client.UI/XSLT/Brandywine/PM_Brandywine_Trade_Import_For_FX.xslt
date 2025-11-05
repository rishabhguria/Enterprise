<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varEntryTest">
          <xsl:value-of select="COL18"/>
        </xsl:variable>

        <xsl:variable name="PositionForLeadUSD">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:variable name="PositionForNonLeadUSD">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:if test="$varEntryTest='CURRENCY ON ACCOUNT PROCESSING' and (number($PositionForLeadUSD) or number($PositionForNonLeadUSD))">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varTestSymbol">
              <xsl:value-of select="normalize-space(COL26)"/>
            </xsl:variable>

            <xsl:variable name="varAvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varCostBasic">
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
            </xsl:variable>
            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="normalize-space('')"/>
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

            <Symbol>
              <xsl:choose>
                <xsl:when test="contains($varTestSymbol,'AUSTRALIAN DOLLAR SPOT')">
                  <xsl:value-of select="'AUD/USD'"/>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'GREAT BRITISH POUNDS')">
                  <xsl:value-of select="'GBP/USD'"/>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'EUROPEAN')">
                  <xsl:value-of select="'EUR/USD'"/>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'HONG KONG DOLLARS')">
                  <xsl:value-of select="'USD/HKD'"/>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'SINGAPORE DOLLAR')">
                  <xsl:value-of select="'USD/SGD'"/>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'SOUTH AFRICAN RAND')">
                  <xsl:value-of select="'USD/ZAR'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="contains($varTestSymbol,'AUSTRALIAN DOLLAR SPOT') or contains($varTestSymbol,'GREAT BRITISH POUNDS') or contains($varTestSymbol,'EUROPEAN') ">
                  <xsl:choose>
                    <xsl:when test="$PositionForNonLeadUSD &lt; 0">
                      <xsl:value-of select="$PositionForNonLeadUSD * (-1)"/>
                    </xsl:when>
                    <xsl:when test="$PositionForNonLeadUSD &gt; 0">
                      <xsl:value-of select="$PositionForNonLeadUSD"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="contains($varTestSymbol,'HONG KONG DOLLARS') or contains($varTestSymbol,'SINGAPORE DOLLAR') or contains($varTestSymbol,'SOUTH AFRICAN RAND') ">
                  <xsl:choose>
                    <xsl:when test="$PositionForLeadUSD &lt; 0">
                      <xsl:value-of select="$PositionForLeadUSD * (-1)"/>
                    </xsl:when>
                    <xsl:when test="$PositionForLeadUSD &gt; 0">
                      <xsl:value-of select="$PositionForLeadUSD"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
                
              <SideTagValue>
                <xsl:choose>
                  <xsl:when test="contains($varTestSymbol,'AUSTRALIAN DOLLAR SPOT') or contains($varTestSymbol,'GREAT BRITISH POUNDS') or contains($varTestSymbol,'EUROPEAN') ">
                    <xsl:choose>
                      <xsl:when test="contains($varSide,'PURCHASES')">
                        <xsl:value-of select="'2'"/>
                      </xsl:when>
                      <xsl:when test="contains($varSide,'SALE')">
                        <xsl:value-of select="'1'"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="contains($varTestSymbol,'HONG KONG DOLLARS') or contains($varTestSymbol,'SINGAPORE DOLLAR') or contains($varTestSymbol,'SOUTH AFRICAN RAND') ">
                    <xsl:choose>
                      <xsl:when test="contains($varSide,'PURCHASES')">
                        <xsl:value-of select="'1'"/>
                      </xsl:when>
                      <xsl:when test="contains($varSide,'SALE')">
                        <xsl:value-of select="'2'"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                  </xsl:choose>
              </SideTagValue>
              
              <CostBasis>
                <xsl:choose>
                  <xsl:when test="contains($varTestSymbol,'AUSTRALIAN DOLLAR SPOT') or contains($varTestSymbol,'GREAT BRITISH POUNDS') or contains($varTestSymbol,'EUROPEAN') ">
                    <xsl:value-of select="1 div $varAvgPrice"/>
                  </xsl:when>
                  <xsl:when test="contains($varTestSymbol,'HONG KONG DOLLARS') or contains($varTestSymbol,'SINGAPORE DOLLAR') or contains($varTestSymbol,'SOUTH AFRICAN RAND') ">
                    <xsl:value-of select="$varAvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CostBasis>
          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>