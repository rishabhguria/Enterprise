<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>

      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <TaxlotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxlotState>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Close'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <!--<AveragePrice>
            <xsl:value-of select="format-number(AvgPrice,'########.####')"/>
          </AveragePrice>-->

          <ActionCode>
            <xsl:choose>
              <xsl:when test="CustomOrderBy = 1">
                <xsl:value-of select ="concat('EH,',TransmissionDate,',','08290598,',$varSide, ',',Symbol,',',AvgPrice, ',',TradeDate,',',SEDOL)"/>
              </xsl:when>
              <xsl:when test="CustomOrderBy = 2">
                <xsl:value-of select ="concat('EA,',FundAccntNo,',',TaxLotQty)"/>
              </xsl:when>
              <xsl:when test="CustomOrderBy = 3">
                <xsl:value-of select ="concat('ET,',RecordCount,',',TotalQty)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ActionCode>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
