<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <ACCOUNT>
          <xsl:value-of select="'ACCOUNT'"/>
        </ACCOUNT>


        <SYMBOL>
          <xsl:value-of select="'SYMBOL'"/>
        </SYMBOL>

        <SUFFIX>
          <xsl:value-of select="'SUFFIX'"/>
        </SUFFIX>

        <SIDE>
          <xsl:value-of select="'SIDE'"/>
        </SIDE>

        <QUANTITY>
          <xsl:value-of select="'QUANTITY'"/>
        </QUANTITY>


        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <COMMISSION>
          <xsl:value-of select="'COMMISSION'"/>
        </COMMISSION>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='Equity') and CurrencySymbol='USD' and (CounterParty='VCGO' or CounterParty='VONE' or CounterParty='VTFX')]">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <ACCOUNT>
            <xsl:value-of select="AccountNo"/>
          </ACCOUNT>

          <xsl:variable name="varSymbol1">
            <xsl:choose>
              <xsl:when test="(substring-after(Symbol,'/') = 'W') or (substring-after(Symbol,'/') = 'WS')">
                <xsl:value-of select="concat(substring-before(Symbol,'/'),'WS')"/>
              </xsl:when>
              <xsl:when test="(substring-after(Symbol,'.') = 'W') or (substring-after(Symbol,'.') = 'WS')">
                <xsl:value-of select="concat(substring-before(Symbol,'.'),'WS')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <!--<xsl:variable name="varSuffix">
            <xsl:choose>
              <xsl:when test="contains(Symbol,'/')">
                <xsl:value-of select="substring-after(Symbol,'/')"/>
              </xsl:when>
              <xsl:when test="contains(Symbol,'.')">
                <xsl:value-of select="substring-after(Symbol,'.')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>-->

          <SYMBOL>
            <xsl:value-of select="$varSymbol1"/>
          </SYMBOL>

          <SUFFIX>
            <xsl:value-of select="''"/>
          </SUFFIX>

          <SIDE>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SIDE>

          <QUANTITY>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </QUANTITY>


          <PRICE>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PRICE>



          <xsl:variable name="varPershare">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
          <COMMISSION>
            <xsl:choose>
              <xsl:when test="CommissionCharged &lt;1">
                <xsl:value-of select="format-number($varPershare,'##.####')"/>
              </xsl:when>
              <xsl:when test="CommissionCharged &gt;1">
                <xsl:value-of select="concat('c',$varCommission)"/>
              </xsl:when>
              <xsl:when test="CommissionCharged='1'">
                <xsl:value-of select="concat('c',$varCommission)"/>
              </xsl:when>
            </xsl:choose>
          </COMMISSION>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>