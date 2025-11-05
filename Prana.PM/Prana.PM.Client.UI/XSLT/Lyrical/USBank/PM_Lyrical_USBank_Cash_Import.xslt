<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and normalize-space(COL2)='CASH EQUIVALENTS'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <AccountName>
              <xsl:value-of select ="'Lyrical International Equity Fund: 194552'"/>
            </AccountName>

            <xsl:variable name="LocalCurrency"/>
            <LocalCurrency>
              <xsl:value-of select="'USD'"/>
            </LocalCurrency>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <CashValueLocal>
              <xsl:choose>
                <xsl:when test ="number($Cash)">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </CashValueLocal>


            <xsl:variable name="Date" select="''"/>
            <Date>
              <xsl:value-of select="$Date"/>
            </Date>

            <PositionType>
              <xsl:value-of select="'Cash'"/>
            </PositionType>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>