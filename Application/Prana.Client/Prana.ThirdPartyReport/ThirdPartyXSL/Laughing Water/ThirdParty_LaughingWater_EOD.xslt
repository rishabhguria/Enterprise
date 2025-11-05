<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[CurrencySymbol='CAD' or CurrencySymbol='USD']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <SIDE>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SIDE>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name="PB_NAME" select="'JPM'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<Account>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Account>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Qty>
            <xsl:value-of select="AllocatedQty"/>
          </Qty>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>
          
          <xsl:variable name="varPrincipal">
            <xsl:value-of select="(AllocatedQty * AveragePrice)"/>
          </xsl:variable>
          
          <Principalamount>
            <xsl:value-of select="$varPrincipal"/>
          </Principalamount>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <xsl:variable name = "varOthFees">
            <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>            
          </xsl:variable>
          <Othercommissionandfees>
            <xsl:value-of select="$varOthFees"/>
          </Othercommissionandfees>

          <Interest>
            <xsl:value-of select="AccruedInterest"/>
          </Interest>

          <Netamount>
            <xsl:value-of select="NetAmount"/>
          </Netamount>

          <xsl:variable name = "PRANA_SETTLE_CURRENCY">
            <xsl:value-of select="SettlCurrency"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_COUNTRY_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CustomColumnMapping.xml')/PBCustomColumnMapping/PB[@Name=$PB_NAME]/PBData[@SettlCurrency=$PRANA_SETTLE_CURRENCY]/@Country"/>
          </xsl:variable>

          <Country>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTRY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTRY_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Country>
		  
		       <LocalCurrency>
			      <xsl:value-of select="CurrencySymbol"/>
		      </LocalCurrency>

          <SetteledCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SetteledCurrency>
          
          <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name = 'JPM']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@DTCCode"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = 'JPM'">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <DTCCode>
            <xsl:value-of select="$varCounterParty"/>
          </DTCCode>
          
          <ExecutingBroker>
            <xsl:value-of select="CounterParty"/>
          </ExecutingBroker>

          <xsl:variable name ="THIRDPARTY_SSI_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CustomColumnMapping.xml')/PBCustomColumnMapping/PB[@Name=$PB_NAME]/PBData[@SettlCurrency=$PRANA_SETTLE_CURRENCY]/@SSI"/>
          </xsl:variable>

          <SSIInstructions>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_SSI_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_SSI_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SSIInstructions>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>