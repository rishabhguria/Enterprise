<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
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
      <xsl:for-each select="//Comparision">
        <xsl:variable name="CashDr">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL8)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($CashDr)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'JP Morgan'"/>
              </xsl:variable>

             

				<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME !=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>

              <OpeningBalanceCR>
				  <xsl:value-of select="0"/>
              </OpeningBalanceCR>

				<xsl:variable name="CashValueBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<OpeningBalanceDR>
					<xsl:choose>
						<xsl:when test="number($CashValueBase)">
							<xsl:value-of select="$CashValueBase"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</OpeningBalanceDR>

              <Currency>
                <xsl:value-of select="normalize-space(COL5)"/>
              </Currency>

              

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <PortfolioAccount>
                <xsl:value-of select="''"/>
              </PortfolioAccount>

              <OpeningBalanceDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceDR>

				<OpeningBalanceCR>
					<xsl:value-of select="0"/>
				</OpeningBalanceCR>

              <Currency>
                <xsl:value-of select="''"/>
              </Currency>

              
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>