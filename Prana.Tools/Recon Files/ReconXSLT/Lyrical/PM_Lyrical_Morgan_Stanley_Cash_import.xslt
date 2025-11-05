<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash" select="substring(COL1,347,8)"/>

        <xsl:if test="number($Cash) and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')='true'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'LYRICAL'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="substring(COL1,7,9)"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <CashValueBase>
              <xsl:value-of select="0"/>
            </CashValueBase>

            <xsl:variable name="Year" select="substring(COL1,378,4)"/>
            <xsl:variable name="Month" select="substring(COL1,383,2)"/>
            <xsl:variable name="Day" select="substring(COL1,386,2)"/>

            <xsl:variable name="Date" select="substring(COL1,378,10)"/>

            <Date>

              <xsl:choose>
                <xsl:when test="contains(substring(COL1,378,10),'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>


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