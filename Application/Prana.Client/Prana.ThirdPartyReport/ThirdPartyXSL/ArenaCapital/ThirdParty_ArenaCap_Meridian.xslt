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

          <Company>
            <xsl:value-of select ="''"/>
          </Company>

          <TradeDate>
            <xsl:value-of select ="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select ="SettlementDate"/>
          </SettlementDate>

          <Counterpart>
            <xsl:value-of select ="CounterParty"/>
          </Counterpart>

          <Security>
            <xsl:value-of select="Symbol"/>
          </Security>

          <Number>
            <xsl:value-of select ="position()"/>
          </Number>

          <Price>
            <xsl:value-of select ="AveragePrice"/>
          </Price>

          <Commission>
            <xsl:value-of select ="CommissionCharged + TaxOnCommissions"/>
          </Commission>

          <OtherCharges>
            <xsl:value-of select ="StampDuty + TransactionLevy + ClearingFee + OtherBrokerFee + MiscFees"/>
          </OtherCharges>

          <TradeType>
            <xsl:value-of select="''"/>
          </TradeType>

          <MISCode1>
            <xsl:value-of select ="''"/>
          </MISCode1>

          <MISCode2>
            <xsl:value-of select ="''"/>
          </MISCode2>

          <RealisedProfit>
            <xsl:value-of select ="0"/>
          </RealisedProfit>

          <SettlementType>
            <xsl:value-of select ="''"/>
          </SettlementType>

          <CompanyCustodian>
            <xsl:value-of select ="''"/>
          </CompanyCustodian>

          <CompanyCustodianAccount>
            <xsl:value-of select ="''"/>
          </CompanyCustodianAccount>

          <CounterpartCustodian>
            <xsl:value-of select ="''"/>
          </CounterpartCustodian>

          <CounterpartCustodianAccount>
            <xsl:value-of select ="''"/>
          </CounterpartCustodianAccount>

          <CloseAgainst>
            <xsl:value-of select ="''"/>
          </CloseAgainst>

          <Description>
            <xsl:value-of select ="FullSecurityName"/>
          </Description>


          <SettlementAccruedInterest>
            <xsl:value-of select ="''"/>
          </SettlementAccruedInterest>
          
          <FOFGrossAmount>
            <xsl:value-of select ="''"/>
          </FOFGrossAmount>

          <MFPricingDate>
            <xsl:value-of select ="''"/>
          </MFPricingDate>

          <FullyRedeem_Y_N>
            <xsl:value-of select ="''"/>
          </FullyRedeem_Y_N>

          <LockShares_Y_N>
            <xsl:value-of select ="''"/>
          </LockShares_Y_N>

          <LockPrice_Y_N>
            <xsl:value-of select ="''"/>
          </LockPrice_Y_N>

          <LockAmount_Y_N>
            <xsl:value-of select ="''"/>
          </LockAmount_Y_N>

          <Deferredpayment_Y_N>
            <xsl:value-of select ="''"/>
          </Deferredpayment_Y_N>

          <Isorder_Y_N>
            <xsl:value-of select ="''"/>
          </Isorder_Y_N>

          <Orderdate>
            <xsl:value-of select ="''"/>
          </Orderdate>

          <Prospective_Y_N>
            <xsl:value-of select ="''"/>
          </Prospective_Y_N>

          <Newissue_Y_N>
            <xsl:value-of select ="''"/>
          </Newissue_Y_N>

          <Equityattributiongroup>
            <xsl:value-of select ="''"/>
          </Equityattributiongroup>

          <Equityattributiongroup>
            <xsl:value-of select ="''"/>
          </Equityattributiongroup>

          <Externalreference>
            <xsl:value-of select ="''"/>
          </Externalreference>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
