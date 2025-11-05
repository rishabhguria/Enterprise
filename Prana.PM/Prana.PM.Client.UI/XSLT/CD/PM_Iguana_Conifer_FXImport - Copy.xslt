<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:if test="normalize-space(COL2) = 'Cash and Equivalents'">
          <PositionMaster>

            <xsl:variable name="varDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBaseCurrency">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varSettlementCurrency">
              <xsl:value-of select="'USD'"/>
            </xsl:variable>

            <xsl:variable name="varForexPrice">
              <xsl:value-of select="COL17 div COL16"/>
            </xsl:variable>

            <BaseCurrency>
              <xsl:value-of select ="$varBaseCurrency"/>
            </BaseCurrency>

            <SettlementCurrency>
              <xsl:value-of select="$varSettlementCurrency"/>
            </SettlementCurrency>

            <xsl:choose>
              <xsl:when test ="number($varForexPrice)">
                <ForexPrice>
                  <xsl:value-of select="$varForexPrice"/>
                </ForexPrice>
              </xsl:when >
              <xsl:otherwise>
                <ForexPrice>
                  <xsl:value-of select="0"/>
                </ForexPrice>
              </xsl:otherwise>
            </xsl:choose >

            <Date>
              <xsl:value-of select="''"/>
            </Date>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
