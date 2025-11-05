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

          <ACCOUNT>
            <xsl:value-of select="AccountNo"/>
          </ACCOUNT>

          <SYMBOL>
            <xsl:value-of select="UnderlyingSymbol"/>
          </SYMBOL>

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
                <xsl:value-of select="format-number($varPershare,'##.##')"/>
              </xsl:when>
              <xsl:when test="CommissionCharged &gt;1">
                <xsl:value-of select="concat('c',$varCommission)"/>
              </xsl:when>
            </xsl:choose>
          </COMMISSION>
          
          <BROKER>
            <xsl:value-of select="''"/>
          </BROKER>
        
          <EXPYEAR>
            <xsl:value-of select="substring(substring-after(substring-after(ExpirationDate,'/'),'/'),3,2)"/>
          </EXPYEAR>

          <EXPMONTH>
            <xsl:value-of select="substring-before(ExpirationDate,'/')"/>
          </EXPMONTH>

          <EXPDAY>
            <xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
          </EXPDAY>

          <PUTCALL>
            <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </PUTCALL>


          <STRIKEPRICE>
            <xsl:value-of select="StrikePrice"/>
          </STRIKEPRICE>

          <EXCHANGE>
            <xsl:value-of select="''"/>
          </EXCHANGE>

          <CMTAOPENCLOSE>
            <xsl:choose>
              <xsl:when test="Side='Sell short' or Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'O'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
            </xsl:choose>
          </CMTAOPENCLOSE>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>