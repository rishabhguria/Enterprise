<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
          <ThirdPartyFlatFileDetail>
            <!--for system internal use-->
            <RowHeader>
              <xsl:value-of select ="'false'"/>
            </RowHeader>

            <FileHeader>
              <xsl:value-of select="'true'"/>
            </FileHeader>

            <FileFooter>
              <xsl:value-of select="'true'"/>
            </FileFooter>

            <TaxLotState>
              <xsl:value-of select="TaxLotState"/>
            </TaxLotState>
            
           <xsl:variable name="PB_NAME" select="'BAML'"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

            <PrimeBrokerAccountNumber>
             <xsl:choose>
              <xsl:when test ="$PB_FUND_NAME != ''">
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
            </PrimeBrokerAccountNumber>

            <Reserved>
              <xsl:value-of select="''"/>
            </Reserved>

            <TradingUnit>
              <xsl:value-of select="''"/>
            </TradingUnit>

            <TradingSub-UnitDealId>
              <xsl:value-of select="''"/>
            </TradingSub-UnitDealId>

            <RecordType>
              <xsl:choose>
                <xsl:when test="TaxLotState='Allocated'">
                  <xsl:value-of select="'N'"/>
                </xsl:when>
                <xsl:when test="TaxLotState='Amemded'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="TaxLotState='Deleted'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </RecordType>

            <TransactionType>
              <xsl:choose>
                <xsl:when test="Side='Buy'">
                  <xsl:value-of select="'BY'"/>
                </xsl:when>
                <xsl:when test="Side='Sell'">
                  <xsl:value-of select="'SL'"/>
                </xsl:when>
                <xsl:when test="Side='Sell short'">
                  <xsl:value-of select="'SS'"/>
                </xsl:when>
                <xsl:when test="Side='Cover short'">
                  <xsl:value-of select="'CS'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionType>

            <ClientTransactionID>
              <xsl:value-of select="EntityID"/>
            </ClientTransactionID>

            <ClientBlockId>
              <xsl:value-of select="''"/>
            </ClientBlockId>

            <ClientOriginalTransactionId>
              <xsl:value-of select="''"/>
            </ClientOriginalTransactionId>
            <!--EQ: Equities BO: Bonds OP: Options WAR: Warrants RT: Rights-->

            <ClientAssetType>
              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="'OP'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'EQ'"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClientAssetType>

            <ClientProductIdType>
              <xsl:value-of select="''"/>
            </ClientProductIdType>

          <xsl:variable name="varTDates" select="concat(substring(substring-after(substring-after(TradeDate,'/'),'/'),3,2), substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>

            <ClientProductId>
                <xsl:value-of select="concat(Symbol,$varTDates, position())"/>
            </ClientProductId>
            
            
            <xsl:variable name="PRANA_COUNTRY_CODE" select="Country"/>
            <xsl:variable name="PB_COUNTRY_NAME">
              <xsl:value-of select="document('../ReconMappingXml/CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$PRANA_COUNTRY_CODE]/@PBCountryName"/>
            </xsl:variable>

            <CountryofTrading>
              <xsl:choose>
                <xsl:when test="$PB_COUNTRY_NAME!=''">
                  <xsl:value-of select="$PB_COUNTRY_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="Country"/>
                </xsl:otherwise>
              </xsl:choose>
            </CountryofTrading>

            <ClientProductDescription>
              <xsl:value-of select="FullSecurityName"/>
            </ClientProductDescription>
            
            <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
            <xsl:variable name="PB_COUNTERPARTY_NAME">
              <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
            </xsl:variable>
            <ClientExecutingBroker>
              <xsl:choose>
                <xsl:when test="$PB_COUNTERPARTY_NAME!=''">
                  <xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'BAML'"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClientExecutingBroker>

             <xsl:variable name="varTDate" select="concat(substring-after(substring-after(TradeDate,'/'),'/'), substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>

            <TradeDate>
              <xsl:value-of select="$varTDate"/>
            </TradeDate>

            <ContractualSettlementDate>
              <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'), substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
            </ContractualSettlementDate>

            <SpotDate>
              <xsl:value-of select="''"/>
            </SpotDate>

            <Price>
              <xsl:value-of select="format-number(AveragePrice,'0.######')"/>
            </Price>

            <IssueCurrency>
              <xsl:value-of select="''"/>
            </IssueCurrency>

            <SettlementCurrency>
              <xsl:value-of select="SettlCurrency"/>
            </SettlementCurrency>

            <CostBasisFXRate>
              <xsl:value-of select="'0'"/>
            </CostBasisFXRate>

            <Quantity>
              <xsl:value-of select="AllocatedQty"/>
            </Quantity>

            <CommissionAmount>             
                  <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>           
            </CommissionAmount>

            <CommissionRate>
              <xsl:value-of select="'0'"/>
            </CommissionRate>

            <CommissionType>
              <xsl:choose>
                <xsl:when test="CurrencySymbol ='USD'">
                  <xsl:value-of select="'S'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'A'"/>
                </xsl:otherwise>
              </xsl:choose>
            </CommissionType>

            <SECFee>
              <xsl:value-of select="SecFee"/>
            </SECFee>

            <FeeType1>
              <xsl:value-of select="''"/>
            </FeeType1>

            <FeeAmount1>
              <xsl:value-of select="''"/>
            </FeeAmount1>

            <FeeType2>
              <xsl:value-of select="''"/>
            </FeeType2>

            <FeeAmount2>
              <xsl:value-of select="''"/>
            </FeeAmount2>


            <FeeType3>
              <xsl:value-of select="''"/>
            </FeeType3>

            <FeeAmount3>
              <xsl:value-of select="''"/>
            </FeeAmount3>

            <FeeType4>
              <xsl:value-of select="''"/>
            </FeeType4>

            <FeeAmount4>
              <xsl:value-of select="''"/>
            </FeeAmount4>

            <FeeType5>
              <xsl:value-of select="''"/>
            </FeeType5>

            <FeeAmount5>
              <xsl:value-of select="''"/>
            </FeeAmount5>

            <AccruedInterest>
              <xsl:value-of select="AccruedInterest"/>
            </AccruedInterest>

            <NetAmount>
              <xsl:value-of select="NetAmount"/>
            </NetAmount>

            <OptionType>
              <xsl:choose>
                <xsl:when test="PutOrCall='Call'">
                  <xsl:value-of select="'CL'"/>
                </xsl:when>
                <xsl:when test="PutOrCall='Put'">
                  <xsl:value-of select="'PT'"/>
                </xsl:when>
              </xsl:choose>
            </OptionType>

            <StrikePrice>
              <xsl:value-of select="''"/>
            </StrikePrice>

            <ExpirationDate>
              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'), substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

            <ClientUnderlyingProductIdType>
              <xsl:value-of select="''"/>
            </ClientUnderlyingProductIdType>

            <ClientUnderlyingProductId>
              <xsl:value-of select="''"/>
            </ClientUnderlyingProductId>

            <ClientComments>
              <xsl:value-of select="''"/>
            </ClientComments>

            <BlotterCode>
              <xsl:value-of select="''"/>
            </BlotterCode>

            <ExecutingService>
              <xsl:value-of select="''"/>
            </ExecutingService>

            <ExchangeCode>
              <xsl:value-of select="''"/>
            </ExchangeCode>

            <SettlementCode>
              <xsl:value-of select="''"/>
            </SettlementCode>

            <TaxLotCloseout>
              <xsl:value-of select="''"/>
            </TaxLotCloseout>

            <SpecialHandling>
              <xsl:value-of select="''"/>
            </SpecialHandling>

            <BOMMarker>
              <xsl:value-of select="''"/>
            </BOMMarker>

            <InternationalTransferFlag>
              <xsl:value-of select="''"/>
            </InternationalTransferFlag>

            <StampAmountCurrency>
              <xsl:value-of select="''"/>
            </StampAmountCurrency>

            <StampAmount>
              <xsl:value-of select="''"/>
            </StampAmount>

            <StampableConsiderationCurrency>
              <xsl:value-of select="''"/>
            </StampableConsiderationCurrency>

            <StampConsideration>
              <xsl:value-of select="''"/>
            </StampConsideration>

            <TransactionStampStatus>
              <xsl:value-of select="''"/>
            </TransactionStampStatus>

            <CustomerID>
              <xsl:value-of select="''"/>
            </CustomerID>

            <FTTexemptionreason>
              <xsl:value-of select="''"/>
            </FTTexemptionreason>

            <FTThigh-lowindicator>
              <xsl:value-of select="''"/>
            </FTThigh-lowindicator>

            <TransactionTypeindicator>
              <xsl:value-of select="''"/>
            </TransactionTypeindicator>

            <Reserved2>
              <xsl:value-of select="''"/>
            </Reserved2>

            <ExecutionReference>
              <xsl:value-of select="''"/>
            </ExecutionReference>

            <Reserved3>
              <xsl:value-of select="''"/>
            </Reserved3>

            <Reserved4>
              <xsl:value-of select="''"/>
            </Reserved4>

            <OriginalTradeDate>
              <xsl:value-of select="''"/>
            </OriginalTradeDate>

            <OriginalValue>
              <xsl:value-of select="''"/>
            </OriginalValue>

            <ExternalFFCAccount1>
              <xsl:value-of select="''"/>
            </ExternalFFCAccount1>

            <ExternalFFCAccount2>
              <xsl:value-of select="''"/>
            </ExternalFFCAccount2>

            <ExternalFFCAccount3>
              <xsl:value-of select="''"/>
            </ExternalFFCAccount3>

            <ABARoutingTransitNumber>
              <xsl:value-of select="''"/>
            </ABARoutingTransitNumber>

            <NewPrimeBrokerAccountNumber>
              <xsl:value-of select="''"/>
            </NewPrimeBrokerAccountNumber>

            <NewTradingUnit>
              <xsl:value-of select="''"/>
            </NewTradingUnit>

            <NewTradingSub-UnitDealId>
              <xsl:value-of select="''"/>
            </NewTradingSub-UnitDealId>

            <OffsetCode>
              <xsl:value-of select="''"/>
            </OffsetCode>

            <Trailer1>
              <xsl:value-of select="''"/>
            </Trailer1>

            <Trailer2>
              <xsl:value-of select="''"/>
            </Trailer2>

            <Trailer3>
              <xsl:value-of select="''"/>
            </Trailer3>

            <OpenDate>
              <xsl:value-of select="''"/>
            </OpenDate>

            <CloseDate>
              <xsl:value-of select="''"/>
            </CloseDate>

            <OpenStartPrice>
              <xsl:value-of select="''"/>
            </OpenStartPrice>

            <OpenStartCashCcy>
              <xsl:value-of select="''"/>
            </OpenStartCashCcy>

            <OpenStartCashQty>
              <xsl:value-of select="''"/>
            </OpenStartCashQty>

            <CloseEndPrice>
              <xsl:value-of select="''"/>
            </CloseEndPrice>

            <CloseEndCashCcy>
              <xsl:value-of select="''"/>
            </CloseEndCashCcy>

            <CloseEndCashQty>
              <xsl:value-of select="''"/>
            </CloseEndCashQty>

            <RepoRate>
              <xsl:value-of select="''"/>
            </RepoRate>

            <RepoRateEffectiveDate>
              <xsl:value-of select="''"/>
            </RepoRateEffectiveDate>

            <TransactionReportMarker>
              <xsl:value-of select="''"/>
            </TransactionReportMarker>

            <GrossAmountCurrency>
              <xsl:value-of select="''"/>
            </GrossAmountCurrency>

            <GrossAmount>
              <xsl:value-of select="''"/>
            </GrossAmount>

            <AgentIndicator>
              <xsl:value-of select="''"/>
            </AgentIndicator>

            <OrderReference>
              <xsl:value-of select="''"/>
            </OrderReference>

            <MarketClientIndicatorField>
              <xsl:value-of select="''"/>
            </MarketClientIndicatorField>

            <ClientMarketLinkField>
              <xsl:value-of select="''"/>
            </ClientMarketLinkField>

            <SpecialConditionSpecialPrice>
              <xsl:value-of select="''"/>
            </SpecialConditionSpecialPrice>

            <TransactionReportBIC>
              <xsl:value-of select="''"/>
            </TransactionReportBIC>

            <Reserved5>
              <xsl:value-of select="''"/>
            </Reserved5>

            <RisklessPrincipalMarker>
              <xsl:value-of select="''"/>
            </RisklessPrincipalMarker>

            <ETFTransactionReportflag>
              <xsl:value-of select="''"/>
            </ETFTransactionReportflag>

            <SettleReportonlyflag>
              <xsl:value-of select="''"/>
            </SettleReportonlyflag>

            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

          </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>