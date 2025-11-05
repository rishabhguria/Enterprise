<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash" select="COL4"/>

        <xsl:if test="number($Cash) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GSEC'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL3"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="COL2='US DOLLAR'">
						  <xsl:value-of select="'USD'"/>
					  </xsl:when>
					  <xsl:when test="COL2='SWISS FRANC'">
						  <xsl:value-of select="'CHF'"/>
					  </xsl:when>
					  <xsl:when test="COL2='JAPANESE YEN'">
						  <xsl:value-of select="'JPY'"/>
					  </xsl:when>
					  <xsl:when test="COL2='EURO'">
						  <xsl:value-of select="'EUR'"/>
					  </xsl:when>
				  </xsl:choose>


			  </Symbol>

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
              <xsl:value-of select="'COL2'"/>
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