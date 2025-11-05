<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

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
              <xsl:when test="Side='Buy to Close'">
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


          <CUSIP>
            <xsl:value-of select="concat('C.',CUSIP)"/>
          </CUSIP>

          <PRICE>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="format-number(AveragePrice,'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PRICE>

          <ACCRUEDINTEREST>
            <xsl:value-of select="''"/>
          </ACCRUEDINTEREST>

          <NETMONEY>
            <xsl:value-of select="''"/>
          </NETMONEY>

          <xsl:variable name="varPershare">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
          <COMMISSION>
            <xsl:choose>
              <xsl:when test="CommissionCharged &lt;1">
                <xsl:value-of select="format-number($varPershare,'##.##')"/>
              </xsl:when>
              <xsl:when test="CommissionCharged &gt;1">
                <xsl:value-of select="concat('c',$varCommission)"/>
              </xsl:when>
            </xsl:choose>            
          </COMMISSION>

          <ACCOUNT>
            <xsl:value-of select="AccountNo"/>
          </ACCOUNT>

          <FIXYIELD>
            <xsl:value-of select="''"/>
          </FIXYIELD>

          <FIXBLOTTER>
            <xsl:value-of select="''"/>
          </FIXBLOTTER>

          <FIXAVEPRICEIND>
            <xsl:value-of select="''"/>
          </FIXAVEPRICEIND>

          <FIXTIME>
            <xsl:value-of select="''"/>
          </FIXTIME>

          <BROKER>
            <xsl:value-of select="'0501'"/>
          </BROKER>


          <TRADEDATEYEAR>
            <xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
          </TRADEDATEYEAR>

          <TRADEDATEMONTH>
            <xsl:value-of select="substring-before(TradeDate,'/')"/>
          </TRADEDATEMONTH>

          <TRADEDATEDAY>
            <xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
          </TRADEDATEDAY>

          <SETTLEDATEYEAR>
            <xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
          </SETTLEDATEYEAR>

          <SETTLEDATEMONTH>
            <xsl:value-of select="substring-before(SettlementDate,'/')"/>
          </SETTLEDATEMONTH>

          <SETTLEDATEDAY>
            <xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
          </SETTLEDATEDAY>

        
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>