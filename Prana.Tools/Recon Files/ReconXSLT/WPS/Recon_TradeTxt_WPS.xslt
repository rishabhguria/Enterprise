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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="DateFormate">
    <xsl:param name="Date"/>
    <xsl:variable name="varDay">
      <xsl:value-of select="substring($Date,3,2)"/>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:value-of select="substring($Date,1,2)"/>
    </xsl:variable>
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($Date,5,2)"/>
    </xsl:variable>

    <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(substring(COL1,52,11))"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and (normalize-space(substring(COL1,10,2))='by' or normalize-space(substring(COL1,10,2))='sl' or normalize-space(substring(COL1,10,2))='ss')">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(substring(COL1,23,8))"/>
            </xsl:variable>
            <xsl:variable name="varCusip">
              <xsl:value-of select="normalize-space(substring(COL1,101,9))"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varCusip!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCusip!=''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(substring(COL1,1,8))"/>

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

            <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>

                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(substring(COL1,32,11))"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <TradeDate>
              <xsl:call-template name="DateFormate">
                <xsl:with-param name="Date" select="normalize-space(substring(COL1,13,6))"/>
              </xsl:call-template>
            </TradeDate>
            
            <SettlementDate>
              <xsl:value-of select="''"/>
            </SettlementDate>

            <xsl:variable name="Side">
              <xsl:value-of select="COL15"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="normalize-space(substring(COL1,10,2))='by'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="normalize-space(substring(COL1,10,2))='ss'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:when test="normalize-space(substring(COL1,10,2))='sl'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

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


