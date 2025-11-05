<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <FUND>
            <xsl:value-of select="FundMappedName"/>
          </FUND>

          <SIDE>
            <xsl:value-of select ="Side"/>
          </SIDE>

          <QUANTITY>
            <xsl:value-of select="AllocatedQty"/>
          </QUANTITY>

          <xsl:variable name ="varCheckSymbolUnderlying">
            <xsl:value-of select ="substring-before(Symbol,'-')"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="ISIN != '' and $varCheckSymbolUnderlying != '' and Asset != 'FX'">
              <SYMBOL>
                <xsl:value-of select ="ISIN"/>
              </SYMBOL>
            </xsl:when>
            <xsl:when test="Asset = 'EquityOption'">
              <SYMBOL>
                <xsl:value-of select ="OSIOptionSymbol"/>
              </SYMBOL>
            </xsl:when>
            <xsl:otherwise>
              <SYMBOL>
                <xsl:value-of select="Symbol"/>
              </SYMBOL>
            </xsl:otherwise>
          </xsl:choose>

          <SECURITYNAME>
            <xsl:value-of select ="FullSecurityName"/>
          </SECURITYNAME>

          <AVGPRICE>
            <xsl:value-of select="AveragePrice"/>
          </AVGPRICE>

          <!--For reporting to KNIGHT: Do not include OTherBrokerFees and MISC FEES in the Commission data-->
          <COMMISSION>
            <xsl:value-of select ="CommissionCharged  + TaxOnCommissions + StampDuty + TransactionLevy + ClearingFee "/>
          </COMMISSION>

          <ASSETNAME>
            <xsl:value-of select ="Asset"/>
          </ASSETNAME>

          <xsl:choose>
            <xsl:when test ="CounterParty='NITE'">
              <COUNTERPARTY>
                <xsl:value-of select ="'DTTX'"/>
              </COUNTERPARTY>
            </xsl:when>
            <xsl:otherwise>
              <COUNTERPARTY>
                <xsl:value-of select ="CounterParty"/>
              </COUNTERPARTY>
            </xsl:otherwise>
          </xsl:choose>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
