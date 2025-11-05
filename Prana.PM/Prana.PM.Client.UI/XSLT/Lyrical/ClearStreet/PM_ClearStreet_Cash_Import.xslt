<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
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
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varCashlocal">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varCashlocal)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

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

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <LocalCurrency>
              <xsl:value-of select ="'USD'"/>
            </LocalCurrency>

            <CashValueLocal>
              <xsl:choose>
                <xsl:when test ="$varCashlocal &gt; 0">
                  <xsl:value-of select ="$varCashlocal"/>
                </xsl:when>

                <xsl:when test ="$varCashlocal &lt; 0">
                  <xsl:value-of select ="$varCashlocal * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueLocal>

            <!--<CashValueBase>
              <xsl:choose>
                <xsl:when test ="$varCashlocal &gt; 0">
                  <xsl:value-of select ="$varCashlocal"/>
                </xsl:when>

                <xsl:when test ="$varCashlocal &lt; 0">
                  <xsl:value-of select ="$varCashlocal * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueBase>-->

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL1,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL1,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL1,1,4)"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </Date>


            <PositionType>
              <xsl:value-of select="'Cash'"/>
            </PositionType>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>