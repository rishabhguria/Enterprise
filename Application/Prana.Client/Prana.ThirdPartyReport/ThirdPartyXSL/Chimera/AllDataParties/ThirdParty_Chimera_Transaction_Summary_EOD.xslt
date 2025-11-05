<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!='CORP' and CounterParty!='Undefined']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>


          <Ticker>
            <xsl:value-of select="Symbol"/>
          </Ticker>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <Account>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
          </Account>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <Side>
            <xsl:value-of select="Side"/>
          </Side>
          
          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <AvgPriceLocal>
            <xsl:value-of select="AveragePrice"/>
          </AvgPriceLocal>


          <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>

          <SoftCommission>
            <xsl:value-of select="SoftCommissionCharged"/>
          </SoftCommission>

          <SECFee>
            <xsl:value-of select="SecFee"/>
          </SECFee>
        
          <ORFFee>
            <xsl:value-of select="OrfFee"/>
          </ORFFee>


          <OtherBrokerFees>
            <xsl:value-of select="OtherBrokerFee"/>
          </OtherBrokerFees>

          <xsl:variable name = "varOthFees">
            <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee + CommissionCharged + SoftCommissionCharged)"/>
          </xsl:variable>

          <TotalCommissionAndFee>
            <xsl:value-of select="$varOthFees"/>
          </TotalCommissionAndFee>
          
          <NetAmount>
            <xsl:value-of select="NetAmount"/>
          </NetAmount>
          
          <UnderlyingSymbol>
            <xsl:value-of select="UnderlyingSymbol"/>
          </UnderlyingSymbol>
          
          <PutOrCall>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="PutOrCall"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </PutOrCall>

          <Strikeprice>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Strikeprice>

          <ExpirationDate>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExpirationDate>
          

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>