<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <xsl:variable name="PB_NAME" select="'BAML'"/>
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="PortfolioName"/>
          </xsl:variable>
          <xsl:variable name="THIRDPARTY_FUND_CODE">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <PortfolioName>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </PortfolioName>

          <Security>
            <xsl:value-of select="Security"/>
          </Security>

          <Position>
            <xsl:value-of select="Position"/>
          </Position>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>