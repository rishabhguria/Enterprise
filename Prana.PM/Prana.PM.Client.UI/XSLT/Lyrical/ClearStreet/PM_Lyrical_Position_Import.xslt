<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

       
        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL16)"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:if test="number($varNetPosition)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'JP Morgan'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol" >
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$Symbol !=''">
                  <xsl:value-of select ="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select ="COL4"/>

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
            
            <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL14)"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varMarkPrice &gt; 0">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="$varMarkPrice &lt; 0">
                  <xsl:value-of select="$varMarkPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
              
            </CostBasis>
            
            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
           
            
            <xsl:variable name="varCUSIP">
              <xsl:value-of select="(normalize-space(COL9))" />
            </xsl:variable>
            <CUSIP>
              <xsl:value-of select="$varCUSIP" />
            </CUSIP>
            <xsl:variable name="varISIN">
              <xsl:value-of select="(normalize-space(COL10))" />
            </xsl:variable>
            <ISIN>
              <xsl:value-of select="$varISIN" />
            </ISIN>
            <xsl:variable name="varSEDOL">
              <xsl:value-of select="(normalize-space(COL11))" />
            </xsl:variable>
            <SEDOL>
              <xsl:value-of select="$varSEDOL" />
            </SEDOL>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL1,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL1,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL1,1,4)"/>
            </xsl:variable>
            
            <PositionStartDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </PositionStartDate>

          
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>