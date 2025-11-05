<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="normalize-space(COL1) != 'branch_cd' and number(COL7)">
          <PositionMaster>

            <xsl:variable name="varBaseCurrency">
              <xsl:value-of select="'USD'"/>
            </xsl:variable>

            <xsl:variable name="varLocalCurrency">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varCashValueLocal">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varCashValueBase">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="concat(COL1,'-',COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Lazard']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <BaseCurrency>
              <xsl:value-of select="$varBaseCurrency"/>
            </BaseCurrency>

            <LocalCurrency>
              <xsl:value-of select="$varLocalCurrency"/>
            </LocalCurrency>

            <CashValueBase>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCashValueBase))">
                  <xsl:value-of select="$varCashValueBase"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CashValueBase>

            <CashValueLocal>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCashValueLocal))">
                  <xsl:value-of select="$varCashValueLocal"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CashValueLocal>

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
