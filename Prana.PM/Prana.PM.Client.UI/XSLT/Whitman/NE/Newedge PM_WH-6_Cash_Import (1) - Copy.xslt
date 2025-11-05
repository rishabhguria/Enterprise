<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash" select="COL122"/>

        <xsl:if test="number($Cash) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'NewEdge'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


            <xsl:variable name="LocalCurrency"/>
            <LocalCurrency>
              <xsl:value-of select="COL14"/>
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

            <CashValueBase>
              <xsl:value-of select="0"/>
            </CashValueBase>

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