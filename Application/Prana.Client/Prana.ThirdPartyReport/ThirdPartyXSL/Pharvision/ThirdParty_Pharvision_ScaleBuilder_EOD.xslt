<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail [AccountName = 'Scalebuilder - 53279315']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="i" select="position()" />

          <OrderID>
            <xsl:choose>
              <xsl:when test="$i &lt; 10">
                <xsl:value-of select="concat('100000',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 100">
                <xsl:value-of select="concat('10000',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 1000">
                <xsl:value-of select="concat('1000',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 10000">
                <xsl:value-of select="concat('100',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 100000">
                <xsl:value-of select="concat('10',$i)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat('1',$i)"/>
              </xsl:otherwise>
            </xsl:choose>
          </OrderID>

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <Account>
            <xsl:choose>
              <xsl:when test ="$PB_FUND_NAME != ''">
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="AccountNo"/>
              </xsl:otherwise>
            </xsl:choose>
          </Account>
          <!--executing broker-->
          <ClearingBroker>
            <xsl:value-of select="CounterParty"/>
          </ClearingBroker>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <OrderQuantity>
            <xsl:value-of select="((AllocatedQty div ExecutedQty) * TotalQty)"/>
          </OrderQuantity>

          <ExecutedQuantity>
            <xsl:value-of select="AllocatedQty"/>
          </ExecutedQuantity>

          <Side>
            <xsl:value-of select="Side"/>
          </Side>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.000')"/>
          </Commission>

          <Tradedate>
            <xsl:value-of select="TradeDate"/>
          </Tradedate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>