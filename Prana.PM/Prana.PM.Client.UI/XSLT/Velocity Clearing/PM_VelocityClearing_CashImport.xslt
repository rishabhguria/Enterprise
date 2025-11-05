<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template name="varCurrency">
    <xsl:param name="Currency"/>
    <xsl:choose>
      <xsl:when test="$Currency='United States dollar'">
        <xsl:value-of select="'USD'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

        <xsl:variable name ="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varCash)">

          <PositionMaster>

            <xsl:variable name ="PB_NAME">
              <xsl:value-of select ="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <BaseCurrency>
              <xsl:value-of select="normalize-space(COL7)"/>
            </BaseCurrency>

            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_CURRENCY_NAME">
              <xsl:value-of select="document('../ReconMappingXML/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>
            <LocalCurrency>
              <xsl:choose>
                <xsl:when test="$PRANA_CURRENCY_NAME!=''">
                  <xsl:value-of select="$PRANA_CURRENCY_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_CURRENCY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </LocalCurrency>

            <xsl:variable name ="localCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>
            <CashValueLocal>
              <xsl:choose>
                <xsl:when test ="number($localCash)">
                  <xsl:value-of select="$localCash"/>
                </xsl:when >

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CashValueLocal>

            <CashValueBase>
              <xsl:choose>
                <xsl:when test ="number($varCash)">
                  <xsl:value-of select="$varCash"/>
                </xsl:when >

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CashValueBase>

            <PositionType>
              <xsl:value-of select="'Cash'"/>
            </PositionType>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>