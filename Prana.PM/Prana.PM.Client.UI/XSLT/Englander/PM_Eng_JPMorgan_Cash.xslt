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


		  <xsl:if test ="substring(COL1,12,1) ='1' ">
        <PositionMaster>

          <!--   Fund -->
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:if test ="substring(COL1,4,8)!= '-OF-F'">
              <xsl:value-of select="substring(COL1,4,8)"/>
            </xsl:if>
          </xsl:variable>
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPMorgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="$PRANA_FUND_NAME=''">
              <FundName>
                <xsl:value-of select="$PB_FUND_NAME"/>
              </FundName>
            </xsl:when>
            <xsl:otherwise>
              <FundName>
                <xsl:value-of select='$PRANA_FUND_NAME'/>
              </FundName>
            </xsl:otherwise>
          </xsl:choose>

          <BaseCurrency>
            <xsl:value-of select="'USD'"/>
          </BaseCurrency>

          <LocalCurrency>
            <xsl:value-of select ="substring(COL1,294,3)"/>
          </LocalCurrency>

          <CashValueBase>
            <xsl:value-of select="0"/>
          </CashValueBase>

          <xsl:variable name ="varCashValLocal">
            <xsl:value-of select ="substring(COL1,91,18)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test ="boolean(number($varCashValLocal))">
              <CashValueLocal>
                <xsl:value-of select="concat(substring($varCashValLocal,1,13),'.',substring($varCashValLocal,14,5))"/>
              </CashValueLocal>
            </xsl:when >
            <xsl:otherwise>
              <CashValueLocal>
                <xsl:value-of select="0"/>
              </CashValueLocal>
            </xsl:otherwise>
          </xsl:choose >

          <Date>
            <xsl:value-of select="''"/>
          </Date>

          <PositionType>
            <xsl:value-of select="'Cash'"/>
          </PositionType>

        </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
