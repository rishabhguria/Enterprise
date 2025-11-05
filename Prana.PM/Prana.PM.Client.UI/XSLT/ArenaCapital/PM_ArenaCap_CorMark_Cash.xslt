<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
        <PositionMaster>

          <!--<xsl:variable name = "varPortfolioID">
							<xsl:value-of select="translate(COL1,' ','')"/>
						</xsl:variable>-->
          <!--   Fund -->
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:value-of select="translate(COL1,'&quot;','')"/>
          </xsl:variable>
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$PRANA_FUND_NAME=''">
              <FundName>
                <xsl:value-of select='$PB_FUND_NAME'/>
              </FundName>
            </xsl:when>
            <xsl:otherwise>
              <FundName>
                <xsl:value-of select='$PRANA_FUND_NAME'/>
              </FundName>
            </xsl:otherwise>
          </xsl:choose >

          <BaseCurrency>
            <xsl:value-of select="'USD'"/>
          </BaseCurrency>

          <LocalCurrency>
            <xsl:value-of select="'USD'"/>
          </LocalCurrency>

          <xsl:choose>
            <xsl:when test ="COL23 &lt; 0 or COL23 &gt; 0 or COL23 = 0">
              <CashValueLocal>
                <xsl:value-of select="COL23"/>
              </CashValueLocal>
              <CashValueBase>
                <xsl:value-of select="COL23"/>
              </CashValueBase>
            </xsl:when >
            <xsl:otherwise>
              <CashValueLocal>
                <xsl:value-of select="0"/>
              </CashValueLocal>
              <CashValueBase>
                <xsl:value-of select="0"/>
              </CashValueBase>
            </xsl:otherwise>
          </xsl:choose >

          <xsl:choose>
            <xsl:when test="COL22 != 'ACTIVE_DATE'">
              <Date>
                <xsl:value-of select="concat(substring(COL22,3,2),'/',substring(COL22,5,2),'/','20',substring(COL22,1,2))"/>
              </Date>
            </xsl:when>
            <xsl:otherwise>
              <Date>
                <xsl:value-of select="''"/>
              </Date>
            </xsl:otherwise>
          </xsl:choose>

          <PositionType>
            <xsl:value-of select="'Cash'"/>
          </PositionType>

        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
