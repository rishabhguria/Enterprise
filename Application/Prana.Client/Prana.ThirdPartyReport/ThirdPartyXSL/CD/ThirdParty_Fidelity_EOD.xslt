<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <FileHeader>
          <xsl:value-of select ="'false'"/>
        </FileHeader>

        <FileFooter>
          <xsl:value-of select ="'false'"/>
        </FileFooter>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <Account>
          <xsl:value-of select="'Account Number '"/>
        </Account>

        <AccountType>
          <xsl:value-of select="'Account Type'"/>
        </AccountType>


        <OrderAction>
          <xsl:value-of select="'Order Action'"/>
        </OrderAction>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>


        <PriceType>
          <xsl:value-of select="'Price Type'"/>
        </PriceType>

        <TimeInForce>
          <xsl:value-of select="'Time In Force '"/>
        </TimeInForce>

        <StopPrice>
          <xsl:value-of select="'Stop Price'"/>
        </StopPrice>

        <QuantityType>
          <xsl:value-of select="'Quantity Type'"/>
        </QuantityType>

        <ExchangetoSymbol>
          <xsl:value-of select="'Exchange to Symbol'"/>
        </ExchangetoSymbol>

        <BlockIndicator>
          <xsl:value-of select="'Block Indicator'"/>
        </BlockIndicator>

        <BlockID>
          <xsl:value-of select="'Block ID'"/>
        </BlockID>

        <CommissionAmount>
          <xsl:value-of select="'Commission Amount'"/>
        </CommissionAmount>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <FileHeader>
            <xsl:value-of select ="'false'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select ="'false'"/>
          </FileFooter>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <Account>
            <xsl:value-of select="AccountName"/>
          </Account>

          <AccountType>
            <xsl:value-of select="'Account Type'"/>
          </AccountType>

          <OrderAction>
            <xsl:choose>
              <xsl:when test="Side = 'Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell short'">
                <xsl:value-of select="'SH'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Open' and Asset = 'EquityOption' and PutOrCall = 'Put'">
                <xsl:value-of select="'BPO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Open' and Asset = 'EquityOption' and PutOrCall = 'Call'">
                <xsl:value-of select="'BPO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close' and Asset = 'EquityOption' and PutOrCall = 'Put'">
                <xsl:value-of select="'BPC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close' and Asset = 'EquityOption' and PutOrCall = 'Call'">
                <xsl:value-of select="'BCC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption' and PutOrCall = 'Put'">
                <xsl:value-of select="'SPO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption' and PutOrCall = 'Call'">
                <xsl:value-of select="'SPO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Close' and Asset = 'EquityOption' and PutOrCall = 'Put'">
                <xsl:value-of select="'SPC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Close' and Asset = 'EquityOption' and PutOrCall = 'Call'">
                <xsl:value-of select="'SCC'"/>
              </xsl:when>
            </xsl:choose>
          </OrderAction>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>


          <PriceType>
            <xsl:value-of select="''"/>
          </PriceType>

          <TimeInForce>
            <xsl:value-of select="''"/>
          </TimeInForce>

          <StopPrice>
            <xsl:value-of select="''"/>
          </StopPrice>

          <QuantityType>
            <xsl:value-of select="''"/>
          </QuantityType>

          <ExchangetoSymbol>
            <xsl:value-of select="''"/>
          </ExchangetoSymbol>

          <BlockIndicator>
            <xsl:value-of select="''"/>
          </BlockIndicator>

          <BlockID>
            <xsl:value-of select="''"/>
          </BlockID>

          <CommissionAmount>
            <xsl:value-of select="CommissionCharged"/>
          </CommissionAmount>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
