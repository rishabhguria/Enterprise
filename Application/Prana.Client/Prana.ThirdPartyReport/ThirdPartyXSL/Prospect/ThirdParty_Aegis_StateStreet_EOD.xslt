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

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <Account>
          <xsl:value-of select="'SSB Account#'"/>
        </Account>

        <AccountName>
          <xsl:value-of select="'SSB Account Name'"/>
        </AccountName>
        
        <Transaction>
          <xsl:value-of select="'Transaction Type'"/>
        </Transaction>

        <TransactionDesc>
          <xsl:value-of select="'Transaction Description'"/>
        </TransactionDesc>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettleDate>

		  <ShareAmount>
			  <xsl:value-of select="'ShareAmount'"/>
		  </ShareAmount>

        <Asset>
          <xsl:value-of select="'Asset Identifier'"/>
        </Asset>

        <SecDescription>
          <xsl:value-of select="'Securities Description'"/>
        </SecDescription>

        <TradeCurrency>
          <xsl:value-of select="'Trade Currency'"/>
        </TradeCurrency>

        <PrincipalAmount>
          <xsl:value-of select="'Principal Amount'"/>
        </PrincipalAmount>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <Tax>
          <xsl:value-of select="'Tax'"/>
        </Tax>

        <SecFee>
          <xsl:value-of select="'Securities Fee'"/>
        </SecFee>

        <NetSettlement>
          <xsl:value-of select="'Net Settlement'"/>
        </NetSettlement>

        <SettlementCurrency>
          <xsl:value-of select="'Settlement Currency'"/>
        </SettlementCurrency>


        <SettlementLocation>
          <xsl:value-of select="'Settlement Location'"/>
        </SettlementLocation>

        <TradingBroker>
          <xsl:value-of select="'Trading Broker'"/>
        </TradingBroker>

        <ClearingBroker>
          <xsl:value-of select="'Clearing Broker'"/>
        </ClearingBroker>

        <ParticipantNo>
          <xsl:value-of select ="'Participant#'"/>
        </ParticipantNo>

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
            <xsl:value-of select="'JTFM'"/>
          </Account>

          <AccountName>
            <xsl:value-of select="'JTFM'"/>
          </AccountName>

          <Transaction>
            <xsl:value-of select="Side"/>
          </Transaction>

          <TransactionDesc>
            <xsl:value-of select="''"/>
          </TransactionDesc>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

			<ShareAmount>
				<xsl:value-of select="AllocatedQty"/>
			</ShareAmount>

          <Asset>
            <xsl:value-of select="Symbol"/>
          </Asset>

          <SecDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecDescription>

          <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <PrincipalAmount>
            <xsl:value-of select="GrossAmount"/>
          </PrincipalAmount>

          <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>

          <Tax>
            <xsl:value-of select="TaxOnCommissions"/>
          </Tax>

          <SecFee>
            <xsl:value-of select="SecFees"/>
          </SecFee>

          <NetSettlement>
            <xsl:value-of select="NetAmount"/>
          </NetSettlement>

          <SettlementCurrency>
            <xsl:value-of select="'USD'"/>
          </SettlementCurrency>


          <SettlementLocation>
            <xsl:value-of select="''"/>
          </SettlementLocation>

          <TradingBroker>
            <xsl:value-of select="CounterParty"/>
          </TradingBroker>

          <ClearingBroker>
            <xsl:value-of select="''"/>
          </ClearingBroker>

          <ParticipantNo>
            <xsl:value-of select ="'0355'"/>
          </ParticipantNo>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
