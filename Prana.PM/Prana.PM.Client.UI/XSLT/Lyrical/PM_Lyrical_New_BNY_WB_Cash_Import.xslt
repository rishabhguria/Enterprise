<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash" select="COL28"/>

        <xsl:if test="((number($Cash) and COL1 != 'RLPF10300002') or (number($Cash) and COL1 = 'RLPF10300002' and contains(COL9,'DREYFUS'))) and contains(COL12,'CASH')">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNY_WB'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

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


            <xsl:variable name="LocalCurrency" select="COL16"/>
            <LocalCurrency>
              <xsl:value-of select="COL16"/>
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

            <xsl:variable name="Date" select="COL5"/>

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