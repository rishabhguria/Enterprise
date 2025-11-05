<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my"> public int RoundOff(double Qty) { return (int)Math.Round(Qty,0); } </msxsl:script>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        
        <FileHeader>
         <xsl:value-of select="'true'"/>
        </FileHeader>
        
        <FileFooter>
         <xsl:value-of select="'true'"/>
        </FileFooter>

        <PrimeBrokerAccountNumber>
          <xsl:value-of select="'Prime Broker Account Number'"/>
        </PrimeBrokerAccountNumber>

        <Reserved1>
          <xsl:value-of select="'Reserved'"/>
        </Reserved1>

        <TradingUnit>
          <xsl:value-of select="'Trading Unit'"/>
        </TradingUnit>

        <TradingSubUnit>
          <xsl:value-of select="'Trading Sub-Unit (Deal Id)'"/>
        </TradingSubUnit>

        <RecordType>
          <xsl:value-of select="'Record Type'"/>
        </RecordType>

        <TransactionType>
          <xsl:value-of select="'Transaction Type'"/>
        </TransactionType>

        <ClientTransactionID>
          <xsl:value-of select="'Client Transaction ID'"/>
        </ClientTransactionID>

        <ClientBlockId>
          <xsl:value-of select="'Client Block Id'"/>
        </ClientBlockId>

        <ClientOriginalTransactionId>
          <xsl:value-of select="'Client Original Transaction Id'"/>
        </ClientOriginalTransactionId>

        <ClientAssetType>
          <xsl:value-of select="'Client Asset Type'"/>
        </ClientAssetType>

        <ClientProductIdType>
          <xsl:value-of select="'Client Product Id Type'"/>
        </ClientProductIdType>

        <ClientProductId>
          <xsl:value-of select="'Client Product Id'"/>
        </ClientProductId>

        <CountryofTrading>
          <xsl:value-of select="'Country of Trading'"/>
        </CountryofTrading>

        <ClientProductDescription>
          <xsl:value-of select="'Client Product Description'"/>
        </ClientProductDescription>

        <ClientExecutingBroker>
          <xsl:value-of select="'Client Executing Broker'"/>
        </ClientExecutingBroker>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <ContractualSettlementDate>
          <xsl:value-of select="'Contractual Settlement Date'"/>
        </ContractualSettlementDate>

        <SpotDate>
          <xsl:value-of select="'Spot Date'"/>
        </SpotDate>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <IssueCurrency>
          <xsl:value-of select="'Issue Currency'"/>
        </IssueCurrency>

        <SettlementCurrency>
          <xsl:value-of select="'Settlement Currency'"/>
        </SettlementCurrency>

        <CostBasisFXRate>
          <xsl:value-of select="'Cost Basis FX Rate'"/>
        </CostBasisFXRate>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <CommissionAmount>
          <xsl:value-of select="'Commission Amount'"/>
        </CommissionAmount>

        <CommissionRate>
          <xsl:value-of select="'Commission Rate'"/>
        </CommissionRate>

        <CommissionType>
          <xsl:value-of select="'CommissionType'"/>
        </CommissionType>

        <SECFee>
          <xsl:value-of select="'SEC Fee'"/>
        </SECFee>

        <FeeType1>
          <xsl:value-of select="'Fee Type 1'"/>
        </FeeType1>

        <FeeAmount1>
          <xsl:value-of select="'Fee Amount 1'"/>
        </FeeAmount1>

        <FeeType2>
          <xsl:value-of select="'Fee Type 2'"/>
        </FeeType2>

        <FeeAmount2>
          <xsl:value-of select="'Fee Amount 2'"/>
        </FeeAmount2>

        <FeeType3>
          <xsl:value-of select="'Fee Type 3'"/>
        </FeeType3>

        <FeeAmount3>
          <xsl:value-of select="'Fee Amount 3'"/>
        </FeeAmount3>

        <FeeType4>
          <xsl:value-of select="'Fee Type 4'"/>
        </FeeType4>

        <FeeAmount4>
          <xsl:value-of select="'Fee Amount 4'"/>
        </FeeAmount4>

        <FeeType5>
          <xsl:value-of select="'Fee Type 5'"/>
        </FeeType5>

        <FeeAmount5>
          <xsl:value-of select="'Fee Amount 5'"/>
        </FeeAmount5>

        <AccruedInterest>
          <xsl:value-of select="'Accrued Interest'"/>
        </AccruedInterest>

        <NetAmount>
          <xsl:value-of select="'Net Amount'"/>
        </NetAmount>

        <OptionType>
          <xsl:value-of select="'Option Type'"/>
        </OptionType>

        <StrikePrice>
          <xsl:value-of select="'Strike Price'"/>
        </StrikePrice>

        <ExpirationDate>
          <xsl:value-of select="'Expiration Date'"/>
        </ExpirationDate>

        <ClientUnderlyingProductIdType>
          <xsl:value-of select="'Client Underlying Product Id Type'"/>
        </ClientUnderlyingProductIdType>

        <ClientUnderlyingProductId>
          <xsl:value-of select="'Client Underlying Product Id'"/>
        </ClientUnderlyingProductId>

        <ClientComments>
          <xsl:value-of select="'Client Comments'"/>
        </ClientComments>

        <BlotterCode>
          <xsl:value-of select="'Blotter Code'"/>
        </BlotterCode>

        <ExecutingService>
          <xsl:value-of select="'Executing Service'"/>
        </ExecutingService>

        <ExchangeCode>
          <xsl:value-of select="'Exchange Code'"/>
        </ExchangeCode>

        <SettlementCode>
          <xsl:value-of select="'Settlement Code'"/>
        </SettlementCode>

        <TaxLotCloseout>
          <xsl:value-of select="'Tax Lot Closeout'"/>
        </TaxLotCloseout>

        <SpecialHandling>
          <xsl:value-of select="'Special Handling'"/>
        </SpecialHandling>

        <BOMMarker>
          <xsl:value-of select="'BOM Marker'"/>
        </BOMMarker>

        <InternationalTransferFlag>
          <xsl:value-of select="'International Transfer Flag'"/>
        </InternationalTransferFlag>

        <StampAmountCurrency>
          <xsl:value-of select="'Stamp Amount Currency'"/>
        </StampAmountCurrency>

        <StampAmount>
          <xsl:value-of select="'Stamp Amount'"/>
        </StampAmount>

        <StampableConsiderationCurrency>
          <xsl:value-of select="'Stampable Consideration Currency'"/>
        </StampableConsiderationCurrency>

        <StampConsideration>
          <xsl:value-of select="'Stamp Consideration'"/>
        </StampConsideration>

        <TransactionStampStatus>
          <xsl:value-of select="'Transaction Stamp Status'"/>
        </TransactionStampStatus>

        <CustomerID>
          <xsl:value-of select="'Customer ID'"/>
        </CustomerID>

        <FTTexemptionreason>
          <xsl:value-of select="'FTT exemption reason'"/>
        </FTTexemptionreason>

        <FTThighlowindicator>
          <xsl:value-of select="'FTT high-low indicator'"/>
        </FTThighlowindicator>

        <TransactionTypeindicator>
          <xsl:value-of select="'Transaction Type indicator'"/>
        </TransactionTypeindicator>

        <Reserved2>
          <xsl:value-of select="'Reserved'"/>
        </Reserved2>

        <ExecutionReference>
          <xsl:value-of select="'Execution Reference'"/>
        </ExecutionReference>

        <Reserved3>
          <xsl:value-of select="'Reserved'"/>
        </Reserved3>
        
        <Reserved4>
          <xsl:value-of select="'Reserved'"/>
        </Reserved4>

        <OriginalValue>
          <xsl:value-of select="'Original Value'"/>
        </OriginalValue>

        <ExternalFFCAccount1>
          <xsl:value-of select="'External FFC Account1'"/>
        </ExternalFFCAccount1>

        <ExternalFFCAccount2>
          <xsl:value-of select="'External FFC Account2'"/>
        </ExternalFFCAccount2>

        <ExternalFFCAccount3>
          <xsl:value-of select="'External FFC Account3'"/>
        </ExternalFFCAccount3>

        <ABARoutingTransitNumber>
          <xsl:value-of select="'ABA Routing Transit Number'"/>
        </ABARoutingTransitNumber>

        <NewPrimeBrokerAccountNumber>
          <xsl:value-of select="'New Prime Broker Account Number'"/>
        </NewPrimeBrokerAccountNumber>

        <NewTradingUnit>
          <xsl:value-of select="'New Trading Unit'"/>
        </NewTradingUnit>

        <NewTradingSubUnit>
          <xsl:value-of select="'New Trading Sub-Unit (Deal Id)'"/>
        </NewTradingSubUnit>

        <OffsetCode>
          <xsl:value-of select="'Offset Code'"/>
        </OffsetCode>

        <Trailer1>
          <xsl:value-of select="'Trailer 1'"/>
        </Trailer1>
        
        <Trailer2>
          <xsl:value-of select="'Trailer 2'"/>
        </Trailer2>

        <Trailer3>
          <xsl:value-of select="'Trailer 3'"/>
        </Trailer3>

        <OpenDate>
          <xsl:value-of select="'Open Date'"/>
        </OpenDate>

        <CloseDate>
          <xsl:value-of select="'Close Date'"/>
        </CloseDate>

        <OpenStartPrice>
          <xsl:value-of select="'Open (Start) Price'"/>
        </OpenStartPrice>

        <OpenStartCashCcy>
          <xsl:value-of select="'Open (Start) Cash Ccy'"/>
        </OpenStartCashCcy>
    
        <OpenStartCashQty>
          <xsl:value-of select="'Open (Start) Cash Qty'"/>
        </OpenStartCashQty>

        <CloseEndPrice>
          <xsl:value-of select="'Close (End) Price'"/>
        </CloseEndPrice>

        <CloseEndCashCcy>
          <xsl:value-of select="'Close (End) Cash Ccy'"/>
        </CloseEndCashCcy>

        <CloseEndCashQty>
          <xsl:value-of select="'Close (End) Cash Qty'"/>
        </CloseEndCashQty>

        <RepoRate>
          <xsl:value-of select="'Repo Rate'"/>
        </RepoRate>

        <RepoRateEffectiveDate>
          <xsl:value-of select="'Repo Rate Effective Date'"/>
        </RepoRateEffectiveDate>

        <TransactionReportMarker>
          <xsl:value-of select="'Transaction Report Marker'"/>
        </TransactionReportMarker>

        <GrossAmountCurrency>
          <xsl:value-of select="'Gross Amount Currency'"/>
        </GrossAmountCurrency>

        <GrossAmount>
          <xsl:value-of select="'Gross Amount'"/>
        </GrossAmount>

        <AgentIndicator>
          <xsl:value-of select="'Agent Indicator'"/>
        </AgentIndicator>

        <OrderReference>
          <xsl:value-of select="'Order Reference'"/>
        </OrderReference>

        <MarketClientIndicatorField>
          <xsl:value-of select="'Market Client Indicator Field'"/>
        </MarketClientIndicatorField>

        <ClientMarketLinkField>
          <xsl:value-of select="'Client Market Link Field'"/>
        </ClientMarketLinkField>

        <SpecialCondition>
          <xsl:value-of select="'Special Condition / Special Price'"/>
        </SpecialCondition>

        <TransactionReportBIC>
          <xsl:value-of select="'Transaction Report BIC'"/>
        </TransactionReportBIC>

        <Reserved5>
          <xsl:value-of select="'Reserved'"/>
        </Reserved5>

        <RisklessPrincipalMarker>
          <xsl:value-of select="'Riskless Principal Marker'"/>
        </RisklessPrincipalMarker>

        <ETFTransactionReportflag>
          <xsl:value-of select="'ETF Transaction Report flag'"/>
        </ETFTransactionReportflag>

        <SettleReport>
        <xsl:value-of select="'Settle/Report only flag'"/>
        </SettleReport>
        
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
    
    </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          
          <FileHeader>
           <xsl:value-of select="'true'"/>
          </FileHeader>
        
          <FileFooter>
           <xsl:value-of select="'true'"/>
          </FileFooter>

          <xsl:variable name ="varAllocationState">
            <xsl:choose>
              <xsl:when test="TaxLotState ='Allocated'">
                <xsl:value-of select ="'N'"/>
              </xsl:when>

              <xsl:when test="TaxLotState ='Amended'">
                <xsl:value-of select ="'A'"/>
              </xsl:when>

              <xsl:when test="TaxLotState ='Deleted'">
                <xsl:value-of select ="'C'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'N'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <PrimeBrokerAccountNumber>
            <xsl:value-of select="AccountNo"/>
          </PrimeBrokerAccountNumber>

          <Reserved1>
            <xsl:value-of select="''"/>
          </Reserved1>

          <TradingUnit>
            <xsl:value-of select="AccountName"/>
          </TradingUnit>

          <TradingSubUnit>
            <xsl:value-of select="AccountName"/>
          </TradingSubUnit>

          <RecordType>
            <xsl:value-of select="$varAllocationState"/>
          </RecordType>

          <TransactionType>
            <xsl:choose>
              <xsl:when test="Side ='Buy'">
                <xsl:value-of select ="'BY'"/>
              </xsl:when>
			  
              <xsl:when test="Side ='Buy to Open'">
                <xsl:value-of select ="'BY'"/>
              </xsl:when>

              <xsl:when test="Side ='Sell'">
                <xsl:value-of select ="'SL'"/>
              </xsl:when>
			  
			  <xsl:when test="Side ='Sell to Close'">
                <xsl:value-of select ="'SL'"/>
              </xsl:when>

              <xsl:when test="Side ='Sell short'">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
			  
			  <xsl:when test="Side ='Sell to Open'">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
              
              <xsl:when test="Side ='Buy to Close'">
                <xsl:value-of select ="'CS'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </TransactionType>

          <ClientTransactionID>
            <xsl:value-of select="TradeRefID"/>
          </ClientTransactionID>

          <ClientBlockId>
            <xsl:value-of select="EntityID"/>
          </ClientBlockId>

          <ClientOriginalTransactionId>
            <xsl:value-of select="TradeRefID"/>
          </ClientOriginalTransactionId>

          <ClientAssetType>
            <xsl:value-of select="Asset"/>
          </ClientAssetType>

          <ClientProductIdType>
            <xsl:choose>
              <xsl:when test="Asset ='Equity'">
                <xsl:value-of select ="'SEDOL'"/>
              </xsl:when>

              <xsl:when test="Asset ='EquityOption'">
                <xsl:value-of select ="'OCCCode'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClientProductIdType>

          <ClientProductId>
            <xsl:choose>
              <xsl:when test="Asset ='Equity'">
                <xsl:value-of select ="SEDOL"/>
              </xsl:when>

              <xsl:when test="Asset ='EquityOption'">
                <xsl:value-of select ="OSIOptionSymbol"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClientProductId>

          <CountryofTrading>
            <xsl:value-of select="CurrencySymbol"/>
          </CountryofTrading>

          <ClientProductDescription>
            <xsl:value-of select="''"/>
          </ClientProductDescription>

          <ClientExecutingBroker>
            <xsl:value-of select="CounterParty"/>
          </ClientExecutingBroker>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <ContractualSettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </ContractualSettlementDate>

          <SpotDate>
            <xsl:value-of select="''"/>
          </SpotDate>

          <xsl:variable name="SingleQuote">,</xsl:variable>
          <Price>
            <xsl:value-of select="translate(AveragePrice,'$SingleQuote','')"/>
          </Price>

          <IssueCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </IssueCurrency>

          <SettlementCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SettlementCurrency>

          <CostBasisFXRate>
            <xsl:value-of select="format-number(ForexRate_Trade,'#.######')"/>
          </CostBasisFXRate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <CommissionAmount>
            <xsl:value-of select="CommissionCharged"/>
          </CommissionAmount>

          <CommissionRate>
            <xsl:value-of select="CommissionCharged"/>
          </CommissionRate>

          <CommissionType>
             <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select ="'S'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'A'"/>
              </xsl:otherwise>
            </xsl:choose>
          </CommissionType>

          <SECFee>
            <xsl:value-of select="SecFees * ForexRate_Trade"/>
          </SECFee>

          <FeeType1>
            <xsl:value-of select="'Stamp Duty'"/>
          </FeeType1>

          <FeeAmount1>
            <xsl:choose>
              <xsl:when test="StampDuty!= 0">
                <xsl:value-of select ="StampDuty"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FeeAmount1>

          <FeeType2>
            <xsl:value-of select="'Other Broker Fee'"/>
          </FeeType2>

          <FeeAmount2>
            <xsl:choose>
              <xsl:when test="OtherBrokerFee!= 0">
                <xsl:value-of select ="OtherBrokerFee"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
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
              <xsl:when test="PutOrCall= 'CALL'">
                <xsl:value-of select ="'CL'"/>
              </xsl:when>
              
              <xsl:when test="PutOrCall= 'PUT'">
                <xsl:value-of select ="'PT'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OptionType>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <ExpirationDate>
            <xsl:value-of select="ExpirationDate"/>
          </ExpirationDate>

          <ClientUnderlyingProductIdType>
            <xsl:value-of select="'TICKER'"/>
          </ClientUnderlyingProductIdType>

          <ClientUnderlyingProductId>
            <xsl:value-of select="UnderlyingSymbol"/>
          </ClientUnderlyingProductId>

          <ClientComments>
            <xsl:value-of select="''"/>
          </ClientComments>

          <BlotterCode>
            <xsl:choose>
              <xsl:when test="Asset= 'EquityOption'">
                <xsl:choose>
                  <xsl:when test="PutOrCall= 'PUT'">
                    <xsl:value-of select ="'OP'"/>
                  </xsl:when>
                
                  <xsl:when test="PutOrCall= 'CALL'">
                    <xsl:value-of select ="'OC'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              
              <xsl:when test="CounterParty= 'JONES'">
                <xsl:value-of select ="'PO'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'ID'"/>
              </xsl:otherwise>
            </xsl:choose>
          </BlotterCode>

          <ExecutingService>
            <xsl:value-of select="''"/>
          </ExecutingService>

          <ExchangeCode>
            <xsl:value-of select="Exchange"/>
          </ExchangeCode>
          
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'BAML'"/>
          </xsl:variable>
          <xsl:variable name="PB_CURRENCY_NAME">
            <xsl:value-of select="SettlCurrency"/>
          </xsl:variable>
          <xsl:variable name="PRANA_CURRENCY_NAME">
            <xsl:value-of select="document('../ReconMappingXML/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
          </xsl:variable>

          <SettlementCode>
            <xsl:choose>
              <xsl:when test="$PRANA_CURRENCY_NAME!=''">
                <xsl:value-of select="$PRANA_CURRENCY_NAME"/>
              </xsl:when>
            
              <xsl:otherwise>
                <xsl:value-of select="$PB_CURRENCY_NAME"/>
              </xsl:otherwise>  
            </xsl:choose>
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

          <FTThighlowindicator>
            <xsl:value-of select="''"/>
          </FTThighlowindicator>

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

          <NewTradingSubUnit>
            <xsl:value-of select="''"/>
          </NewTradingSubUnit>

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

          <SpecialCondition>
            <xsl:value-of select="''"/>
          </SpecialCondition>

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

          <SettleReport>
            <xsl:value-of select="''"/>
          </SettleReport>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>