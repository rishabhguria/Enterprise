<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
        <xsl:if test="COL2 != 'Account' and COL2 != '70KV30' and COL2 != '70MA50'">
        <PositionMaster>

          <xsl:variable name="PB_FUND_NAME" select="COL2"/>
          
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          
          <AccountName>
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME != ''">
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountName>

          <BaseCurrency>
            <xsl:value-of select="'USD'"/>
          </BaseCurrency>

          <LocalCurrency>
            <xsl:value-of select="translate(COL4,'&quot;','')"/>
          </LocalCurrency>

          <CashValueBase>
          <xsl:choose>
            <xsl:when test ="number(normalize-space(COL5))">
                <xsl:value-of select="COL5"/>
            </xsl:when >
            <xsl:otherwise>
                <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose >
          </CashValueBase>

          <CashValueLocal>
            <xsl:choose>
            <xsl:when test ="number(normalize-space(COL6))">
                <xsl:value-of select="COL6"/>
            </xsl:when >
            <xsl:otherwise>
                <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose >
        </CashValueLocal>


        <Date>
            <xsl:value-of select="concat(substring(COL1,5,2),'/',substring(COL1,7,2),'/',substring(COL1,1,4))"/>
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
