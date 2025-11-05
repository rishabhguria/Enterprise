<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
     
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <PortfolioSystemAccountNumber>
          <xsl:value-of select="'Portfolio system Account Number'"/>
        </PortfolioSystemAccountNumber>

        <TrustAccountingSystemAccountNumber>
          <xsl:value-of select ="'Trust Accounting System Account Number'"/>
        </TrustAccountingSystemAccountNumber>

        <Cusip>
          <xsl:value-of select="'Cusip'"/>
        </Cusip>

        <SecurityName>
          <xsl:value-of select="'Security Name'"/>
        </SecurityName>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <TypeofTradeBuyorSell>
          <xsl:value-of select="'Type of Trade (Buy or Sell)'"/>
        </TypeofTradeBuyorSell>

        <NoofUnitsShares>
          <xsl:value-of select="'No. of Units/Shares'"/>
        </NoofUnitsShares>

        <PricePerShare>
          <xsl:value-of select="'Price Per Share'"/>
        </PricePerShare>

        <GrossPrincipal>
          <xsl:value-of select="'Gross (Principal)'"/>
        </GrossPrincipal>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <ExchangeFees>
          <xsl:value-of select="'Exchange Fees'"/>
        </ExchangeFees>

        <OtherFees>
          <xsl:value-of select="'Other Fees'"/>
        </OtherFees>

        <AccruedInterest>
          <xsl:value-of select="'Accrued Interest'"/>
        </AccruedInterest>

        <NetAmountofTrade>
          <xsl:value-of select="'Net Amount of Trade'"/>
        </NetAmountofTrade>

        <OriginalFaceAmountMBSABSAssets>
          <xsl:value-of select="'Original Face Amount (MBS, ABS Assets)'"/>
        </OriginalFaceAmountMBSABSAssets>

        <BrokerID>
          <xsl:value-of select="'Broker ID'"/>
        </BrokerID>

        <BrokerName>
          <xsl:value-of select="'Broker Name'"/>
        </BrokerName>

        <TaxLotBuyDate>
          <xsl:value-of select="'TaxLotBuyDate'"/>
        </TaxLotBuyDate>

        <TaxLotAverageCost>
          <xsl:value-of select="'TaxLot Average Cost'"/>
        </TaxLotAverageCost>

        <TaxLotShares>
          <xsl:value-of select="'TaxLot Shares'"/>
        </TaxLotShares>

        <!-- system inetrnal use-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="''"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <PortfolioSystemAccountNumber>
            <xsl:value-of select="''"/>
          </PortfolioSystemAccountNumber>

          <TrustAccountingSystemAccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </TrustAccountingSystemAccountNumber>

          <Cusip>
            <xsl:value-of select="CUSIP"/>
          </Cusip>

          <SecurityName>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityName>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <TypeofTradeBuyorSell>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'buy'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'sell'"/>
              </xsl:when>
            </xsl:choose>
          </TypeofTradeBuyorSell>

          <NoofUnitsShares>
            <xsl:value-of select="AllocatedQty"/>
          </NoofUnitsShares>

          <xsl:variable name="Price">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>

          <PricePerShare>
            <xsl:value-of select="format-number($Price,'0.####')"/>
          </PricePerShare>

          <GrossPrincipal>
            <xsl:value-of select="GrossAmount"/>
          </GrossPrincipal>

          <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>

          <ExchangeFees>
            <xsl:value-of select="SecFee"/>
          </ExchangeFees>

          <OtherFees>
            <xsl:value-of select="''"/>
          </OtherFees>

          <AccruedInterest>
            <xsl:value-of select="''"/>
          </AccruedInterest>

          <NetAmountofTrade>
            <xsl:value-of select="NetAmount"/>
          </NetAmountofTrade>

          <OriginalFaceAmountMBSABSAssets>
            <xsl:value-of select="''"/>
          </OriginalFaceAmountMBSABSAssets>

          <BrokerID>
            <xsl:value-of select="'DTC# - 161/443/352'"/>
          </BrokerID>

          <BrokerName>
            <xsl:choose>
              <xsl:when test="CounterParty='JPM'">
                <xsl:value-of select="'JPM Securities LLC'"/>
              </xsl:when>
              <xsl:when test="CounterParty='BGCE'">
                <xsl:value-of select="'Merrill Lynch Broadcort'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMHI'">
                <xsl:value-of select="'Sanders Morris Harris LLC'"/>
              </xsl:when>
            </xsl:choose>
          </BrokerName>

          <TaxLotBuyDate>
            <xsl:value-of select="''"/>
          </TaxLotBuyDate>

          <TaxLotAverageCost>
            <xsl:value-of select="''"/>
          </TaxLotAverageCost>

          <TaxLotShares>
            <xsl:value-of select="''"/>
          </TaxLotShares>

          <!-- system inetrnal use-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>

