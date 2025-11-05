<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <!--Transaction Details-->
        <ReceiverBIC>
          <xsl:value-of select ="'Receiver BIC'"/>
        </ReceiverBIC>


        <Action>
          <xsl:value-of select ="'Action'"/>
        </Action>

        <ClientReference>
          <xsl:value-of select ="'Client Reference'"/>
        </ClientReference>

        <PreviousReferenceNo>
          <xsl:value-of select ="'Previous Reference No'"/>
        </PreviousReferenceNo>

        <TradeType>
          <xsl:value-of select ="'Trade Type'"/>
        </TradeType>

        <TransactionType>
          <xsl:value-of select="'Transaction Type'"/>
        </TransactionType>


        <InstrumentType>
          <xsl:value-of select ="'Instrument Type'"/>
        </InstrumentType>

        <BlankColumn>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn>


        <!--Security & Portfolio Details-->
        <SecuritiesAccountno>
          <xsl:value-of select ="'Securities Account no'"/>
        </SecuritiesAccountno>

        <FAPortfolioCode>
          <xsl:value-of select ="'FA Portfolio Code'"/>
        </FAPortfolioCode>


        <SecurityIdentifierType>
          <xsl:value-of select ="'Security Identifier Type'"/>
        </SecurityIdentifierType>

        <LocalSecurityCode>
          <xsl:value-of select ="'Local Security Code'"/>
        </LocalSecurityCode>

        <ISIN>
          <xsl:value-of select ="'ISIN'"/>
        </ISIN>

        <SecurityDescription>
          <xsl:value-of select ="'Security Description'"/>
        </SecurityDescription>

        <BlankColumn1>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn1>


        <!--Broker & Counterparty details-->

        <FABrokerIdentifierType>
          <xsl:value-of select="'FA BrokerIdentifier Type'"/>
        </FABrokerIdentifierType>


        <FABrokerCode>
          <xsl:value-of select="'FA Broker Code'"/>
        </FABrokerCode>

        <FACounterpartyIdentifierType>
          <xsl:value-of select="'FA Counterparty Identifier Type'"/>
        </FACounterpartyIdentifierType>


        <FACounterpartyCode>
          <xsl:value-of select="'FA Counterparty Code'"/>
        </FACounterpartyCode>

        <CounterpartyBIC>
          <xsl:value-of select ="'Counterparty BIC'"/>
        </CounterpartyBIC>

        <CounterpartyDescription>
          <xsl:value-of select ="'Counterparty Description'"/>
        </CounterpartyDescription>

        <CounterpartyAccount>
          <xsl:value-of select ="'Counterparty Account'"/>
        </CounterpartyAccount>

        <BuyerSellerBIC>
          <xsl:value-of select ="'Buyer/Seller BIC'"/>
        </BuyerSellerBIC>

        <BuyerSellerDescription>
          <xsl:value-of select ="'Buyer/Seller Description'"/>
        </BuyerSellerDescription>

        <BuyerSellerAccount>
          <xsl:value-of select ="'Buyer/Seller Account'"/>
        </BuyerSellerAccount>

        <BlankColumn2>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn2>

        <BlankColumn3>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn3>
        <!--Trade Details-->



        <TradeDate>
          <xsl:value-of select ="'Trade Date'"/>
        </TradeDate>


        <SettlementDate>
          <xsl:value-of select ="'Settlement Date'"/>
        </SettlementDate>

        <Quantity>
          <xsl:value-of select ="'Quantity'"/>
        </Quantity>

        <DealPriceCCY>
          <xsl:value-of select ="'Deal Price CCY'"/>
        </DealPriceCCY>

        <DealPrice>
          <xsl:value-of select ="'Deal Price'"/>
        </DealPrice>

        <GrossAmount>
          <xsl:value-of select ="'Gross Amount'"/>
        </GrossAmount>

        <AccruedInterest>
          <xsl:value-of select ="'Accrued Interest'"/>
        </AccruedInterest>

        <PlaceOfTrade>
          <xsl:value-of select ="'Place Of Trade'"/>
        </PlaceOfTrade>

        <BlankColumn4>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn4>

        <BlankColumn5>
          <xsl:value-of select="'Blank Column'"/>
        </BlankColumn5>

        <!--Costs and Taxes-->

        <StampDuty>
          <xsl:value-of select ="'Stamp Duty'"/>
        </StampDuty>

        <ClearingFee>
          <xsl:value-of select ="'Clearing Fee'"/>
        </ClearingFee>

        <OtherFee>
          <xsl:value-of select ="'Other Fee'"/>
        </OtherFee>

        <BrokerCommission>
          <xsl:value-of select ="'Broker Commission'"/>
        </BrokerCommission>

        <SalesTax>
          <xsl:value-of select ="'Sales Tax'"/>
        </SalesTax>

        <Levy>
          <xsl:value-of select ="'Levy'"/>
        </Levy>

        <GSTBrokerage>
          <xsl:value-of select ="'GST Brokerage'"/>
        </GSTBrokerage>

        <GSTClearing>
          <xsl:value-of select ="'GST Clearing'"/>
        </GSTClearing>

        <GSTScanning>
          <xsl:value-of select ="'GST Scanning'"/>
        </GSTScanning>

        <GSTTransaction>
          <xsl:value-of select ="'GST Transaction'"/>
        </GSTTransaction>

        <CDSFees>
          <xsl:value-of select ="'CDS Fees'"/>
        </CDSFees>

        <CSEFees>
          <xsl:value-of select ="'CSE Fees'"/>
        </CSEFees>

        <GOVTCess>
          <xsl:value-of select ="'GOVT Cess'"/>
        </GOVTCess>

        <SECCess>
          <xsl:value-of select ="'SEC Cess'"/>
        </SECCess>

        <SCCP>
          <xsl:value-of select ="'SCCP'"/>
        </SCCP>

        <BoughtAccruedInterest>
          <xsl:value-of select ="'Bought Accrued Interest'"/>
        </BoughtAccruedInterest>

        <ARBIRCBTax>
          <xsl:value-of select ="'AR-BIRCBTax'"/>
        </ARBIRCBTax>

        <PrepaidTaxDiscount>
          <xsl:value-of select ="'Prepaid Tax Discount'"/>
        </PrepaidTaxDiscount>

        <PrepaidTaxPremium>
          <xsl:value-of select ="'Prepaid Tax Premium'"/>
        </PrepaidTaxPremium>

        <WitholdingTaxonSDAACCInt>
          <xsl:value-of select ="'Witholding Tax on SDA ACC Int'"/>
        </WitholdingTaxonSDAACCInt>

        <TransactionFee>
          <xsl:value-of select ="'Transaction Fee'"/>
        </TransactionFee>

        <Reserve>
          <xsl:value-of select ="'Reserve'"/>
        </Reserve>

        <Reserve1>
          <xsl:value-of select ="'Reserve'"/>
        </Reserve1>

        <Reserve2>
          <xsl:value-of select ="'Reserve'"/>
        </Reserve2>

        <Reserve3>
          <xsl:value-of select ="'Reserve'"/>
        </Reserve3>

        <!--Settlement Details-->

        <PSET>
          <xsl:value-of select ="'PSET'"/>
        </PSET>

        <PSAFE>
          <xsl:value-of select ="'PSAFE'"/>
        </PSAFE>

        <SettlementCCY>
          <xsl:value-of select ="'Settlement CCY'"/>
        </SettlementCCY>

        <SettlementAmount>
          <xsl:value-of select ="'Settlement Amount'"/>
        </SettlementAmount>

        <TransferType>
          <xsl:value-of select ="'Transfer Type'"/>
        </TransferType>

        <BlankColumn6>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn6>

        <NarrativeLine1>
          <xsl:value-of select ="'Narrative Line1'"/>
        </NarrativeLine1>

        <NarrativeLine2>
          <xsl:value-of select ="'Narrative Line2'"/>
        </NarrativeLine2>

        <NarrativeLine3>
          <xsl:value-of select ="'Narrative Line3'"/>
        </NarrativeLine3>

        <NarrativeLine4>
          <xsl:value-of select ="'Narrative Line4'"/>
        </NarrativeLine4>

        <RegistrationNarrative>
          <xsl:value-of select ="'Registration Narrative'"/>
        </RegistrationNarrative>

        <BlankColumn7>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn7>

        <BlankColumn8>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn8>

        <BlankColumn9>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn9>

        <BlankColumn10>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn10>

        <BlankColumn11>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn11>

        <BlankColumn12>
          <xsl:value-of select ="'Blank Column'"/>
        </BlankColumn12>

        <EndofLineIndicator>
          <xsl:value-of select ="'End_of_Line_Indicator'"/>
        </EndofLineIndicator>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>


      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'Napean MS 04F104485' or AccountName = 'Napean MS 04F125605' ]">
     
        <xsl:choose>
          <xsl:when test ="TaxLotState != 'Amemded'">
            <ThirdPartyFlatFileDetail>
              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <!--Transaction Details-->
              
                <xsl:variable name="PB_NAME">
                <xsl:value-of select="'EZE'"/>
              </xsl:variable>

              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@RecBIC"/>
              </xsl:variable>
              <ReceiverBIC>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ReceiverBIC>

              <xsl:variable name="varAction">
                <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of select="'NEWM'"/>
                  </xsl:when>
                   <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="'CANC'"/>
                  </xsl:when>
                 
                </xsl:choose>
              </xsl:variable>
              <Action>
                <xsl:value-of select ="$varAction"/>
              </Action>

              <ClientReference>
                <xsl:value-of select ="PBUniqueID"/>
              </ClientReference>

              <PreviousReferenceNo>
                <xsl:value-of select="''"/>
              </PreviousReferenceNo>

              <TradeType>
                <xsl:value-of select ="'F'"/>
              </TradeType>

              <TransactionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TransactionType>

             
              <InstrumentType>
                <xsl:choose>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'E'"/>
                  </xsl:when>

                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </InstrumentType>
              <BlankColumn>
                <xsl:value-of select="''"/>
              </BlankColumn>


              <!--Security & Portfolio Details-->
              <SecuritiesAccountno>
                <xsl:value-of select ="''"/>
              </SecuritiesAccountno>

              <FAPortfolioCode>
                <xsl:value-of select ="'S-114552-0'"/>
              </FAPortfolioCode>
              
              <xsl:variable name="varSecurityIDType">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'ISIN'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="'CUSIP'"/>
                  </xsl:when>

                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="'SEDOL'"/>
                  </xsl:when>
                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="'SYMBOL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <SecurityIdentifierType>
                <xsl:value-of select ="'ISIN'"/>
              </SecurityIdentifierType>
              
              <xsl:variable name="varSecurity">
                <xsl:choose>
                  <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>

                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>

                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <LocalSecurityCode>
                <xsl:value-of select ="ISIN"/>
              </LocalSecurityCode>

              <ISIN>
                <xsl:value-of select ="ISIN"/>
              </ISIN>
              <SecurityDescription>
                <xsl:value-of select ="''"/>
              </SecurityDescription>

              <BlankColumn1>
                <xsl:value-of select="''"/>
              </BlankColumn1>


              <!--Broker & Counterparty details-->

              <FABrokerIdentifierType>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol != 'USD'">
                    <xsl:value-of select ="'BIC_CODE'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTC'"/>
                  </xsl:when>
                </xsl:choose>
              </FABrokerIdentifierType>
			  
              <xsl:variable name="THIRDPARTY_PSET1_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@BICCode"/>
              </xsl:variable>
              <FABrokerCode>
              <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FABrokerCode>

              <FACounterpartyIdentifierType>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol != 'USD'">
                    <xsl:value-of select ="'BIC_CODE'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTC'"/>
                  </xsl:when>
                </xsl:choose>
              </FACounterpartyIdentifierType>


              <FACounterpartyCode>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FACounterpartyCode>

              <CounterpartyBIC>
                <xsl:value-of select ="''"/>
              </CounterpartyBIC>

              <CounterpartyDescription>
                <xsl:value-of select ="''"/>
              </CounterpartyDescription>

              <CounterpartyAccount>
                <xsl:value-of select ="''"/>
              </CounterpartyAccount>

              <BuyerSellerBIC>
                <xsl:value-of select ="''"/>
              </BuyerSellerBIC>

              <BuyerSellerDescription>
                <xsl:value-of select ="''"/>
              </BuyerSellerDescription>

              <BuyerSellerAccount>
                <xsl:value-of select ="''"/>
              </BuyerSellerAccount>

              <BlankColumn2>
                <xsl:value-of select="''"/>
              </BlankColumn2>
              
              <BlankColumn3>
                <xsl:value-of select="''"/>
              </BlankColumn3>
              <!--Trade Details-->

              <xsl:variable name="Date" select="substring-before(TradeDate,'T')"/>
              <xsl:variable name="Day" select="substring($Date,9,2)"/>
              <xsl:variable name="Month" select="substring($Date,6,2)"/>
              <xsl:variable name="Year" select="substring($Date,1,4)"/>

              <TradeDate>
                <xsl:value-of select ="concat($Year,$Month,$Day)"/>
              </TradeDate>

              <xsl:variable name="SDate" select="substring-before(SettlementDate,'T')"/>
              <xsl:variable name="Day1" select="substring($SDate,9,2)"/>
              <xsl:variable name="Month1" select="substring($SDate,6,2)"/>
              <xsl:variable name="Year1" select="substring($SDate,1,4)"/>

              <SettlementDate>
                <xsl:value-of select ="concat($Year1,$Month1,$Day1)"/>
              </SettlementDate>

              <Quantity>
                <xsl:value-of select ="CumQty"/>
              </Quantity>

              <DealPriceCCY>
                <xsl:value-of select ="CurrencySymbol"/>
              </DealPriceCCY>

              <DealPrice>
                <xsl:value-of select ="AvgPrice"/>
              </DealPrice>

              <GrossAmount>
                <xsl:value-of select ="AvgPrice * CumQty"/>
              </GrossAmount>

              <AccruedInterest>
                <xsl:value-of select ="AccruedInterest"/>
              </AccruedInterest>

              <PlaceOfTrade>
                <xsl:value-of select ="''"/>
              </PlaceOfTrade>
              
              <BlankColumn4>
                <xsl:value-of select="''"/>
              </BlankColumn4>
              
              <BlankColumn5>
                <xsl:value-of select="''"/>
              </BlankColumn5>

              <!--Costs and Taxes-->

              <StampDuty>
                <xsl:value-of select ="StampDuty"/>
              </StampDuty>

              <ClearingFee>
                <xsl:value-of select ="ClearingFee"/>
              </ClearingFee>

              <OtherFee>
                <xsl:value-of select ="''"/>
              </OtherFee>

              <BrokerCommission>
                <xsl:value-of select ="''"/>
              </BrokerCommission>

              <SalesTax>
                <xsl:value-of select ="''"/>
              </SalesTax>

              <Levy>
                <xsl:value-of select ="TransactionLevy"/>
              </Levy>
              
              <GSTBrokerage>
                <xsl:value-of select ="''"/>
              </GSTBrokerage>

              <GSTClearing>
                <xsl:value-of select ="''"/>
              </GSTClearing>

              <GSTScanning>
                <xsl:value-of select ="''"/>
              </GSTScanning>

              <GSTTransaction>
                <xsl:value-of select ="''"/>
              </GSTTransaction>

              <CDSFees>
                <xsl:value-of select ="''"/>
              </CDSFees>

              <CSEFees>
                <xsl:value-of select ="''"/>
              </CSEFees>

              <GOVTCess>
                <xsl:value-of select ="''"/>
              </GOVTCess>

              <SECCess>
                <xsl:value-of select ="''"/>
              </SECCess>

              <SCCP>
                <xsl:value-of select ="''"/>
              </SCCP>

              <BoughtAccruedInterest>
                <xsl:value-of select ="''"/>
              </BoughtAccruedInterest>

              <ARBIRCBTax>
                <xsl:value-of select ="''"/>
              </ARBIRCBTax>

              <PrepaidTaxDiscount>
                <xsl:value-of select ="''"/>
              </PrepaidTaxDiscount>

              <PrepaidTaxPremium>
                <xsl:value-of select ="''"/>
              </PrepaidTaxPremium>

              <WitholdingTaxonSDAACCInt>
                <xsl:value-of select ="''"/>
              </WitholdingTaxonSDAACCInt>

              <TransactionFee>
                <xsl:value-of select ="''"/>
              </TransactionFee>

              <Reserve>
                <xsl:value-of select ="''"/>
              </Reserve>
              
               <Reserve1>
                <xsl:value-of select ="''"/>
              </Reserve1>

              <Reserve2>
                <xsl:value-of select ="''"/>
              </Reserve2>
              
              <Reserve3>
                <xsl:value-of select ="''"/>
              </Reserve3>

              <!--Settlement Details-->

              <PSET>
                <xsl:value-of select ="''"/>
              </PSET>
              <PSAFE>
                <xsl:value-of select ="''"/>
              </PSAFE>
              <SettlementCCY>
                <xsl:value-of select ="''"/>
              </SettlementCCY>
              
              <SettlementAmount>
                <xsl:value-of select ="''"/>
              </SettlementAmount>
              
              <TransferType>
                <xsl:value-of select ="''"/>
              </TransferType>
              
              <BlankColumn6>
                <xsl:value-of select ="''"/>
              </BlankColumn6>
           
              <NarrativeLine1>
                <xsl:value-of select ="''"/>
              </NarrativeLine1>

              <NarrativeLine2>
                <xsl:value-of select ="''"/>
              </NarrativeLine2>

              <NarrativeLine3>
                <xsl:value-of select ="''"/>
              </NarrativeLine3>

              <NarrativeLine4>
                <xsl:value-of select ="''"/>
              </NarrativeLine4>

              <RegistrationNarrative>
                <xsl:value-of select ="''"/>
              </RegistrationNarrative>

              <BlankColumn7>
                <xsl:value-of select ="''"/>
              </BlankColumn7>

              <BlankColumn8>
                <xsl:value-of select ="''"/>
              </BlankColumn8>
              
              <BlankColumn9>
                <xsl:value-of select ="''"/>
              </BlankColumn9>

              <BlankColumn10>
                <xsl:value-of select ="''"/>
              </BlankColumn10>

              <BlankColumn11>
                <xsl:value-of select ="''"/>
              </BlankColumn11>

              <BlankColumn12>
                <xsl:value-of select ="''"/>
              </BlankColumn12>
              
              <EndofLineIndicator>
                <xsl:value-of select ="'END'"/>
              </EndofLineIndicator>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>


            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>
                <RowHeader>
                  <xsl:value-of select ="'false'"/>
                </RowHeader>

                <TaxLotState>
                  <xsl:value-of select ="TaxLotState"/>
                </TaxLotState>

                <!--Transaction Details-->
                <xsl:variable name="PB_NAME">
                  <xsl:value-of select="'EZE'"/>
                </xsl:variable>

                <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CurrencySymbol"/>
                <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@RecBIC"/>
              </xsl:variable>
                <ReceiverBIC>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ReceiverBIC>

               <xsl:variable name="varAction">
                <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of select="'NEWM'"/>
                  </xsl:when>
                  <xsl:when test ="TaxLotState = 'Amemded'">
                    <xsl:value-of select="'CANC'"/>
                  </xsl:when>
                  
                  <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="'CANC'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:variable>
                <Action>
                   <xsl:value-of select="$varAction"/>
                </Action>

                <ClientReference>
                  <xsl:choose>
                    
                    <xsl:when test ="TaxLotState = 'Amemded'">
                      <xsl:value-of select ="PBUniqueID"/>
                    </xsl:when>

                    
                  </xsl:choose>
                </ClientReference>

                <PreviousReferenceNo>
                  <xsl:value-of select ="PBUniqueID"/>
                </PreviousReferenceNo>

                <TradeType>
                  <xsl:value-of select ="'F'"/>
                </TradeType>

                <TransactionType>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide = 'Buy to Close'">
                      <xsl:value-of select="'RVP'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell' or OldSide='Sell to Close' or OldSide = 'Sell short' or OldSide = 'Sell to Open'">
                      <xsl:value-of select="'DVP'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </TransactionType>

            
                <InstrumentType>
                  <xsl:choose>
                    <xsl:when test="Asset='Equity'">
                      <xsl:value-of select="'E'"/>
                    </xsl:when>

                    <xsl:when test="Asset='FixedIncome'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </InstrumentType>
                <BlankColumn>
                  <xsl:value-of select="''"/>
                </BlankColumn>
              
                <!--Security & Portfolio Details-->
                <SecuritiesAccountno>
                  <xsl:value-of select ="''"/>
                </SecuritiesAccountno>

                <FAPortfolioCode>
                  <xsl:value-of select ="'S-114552-0'"/>
                </FAPortfolioCode>
                <xsl:variable name="varSecurityIDType">
                  <xsl:choose>
                    <xsl:when test="ISIN!=''">
                      <xsl:value-of select="'ISIN'"/>
                    </xsl:when>
                    <xsl:when test="CUSIP!=''">
                      <xsl:value-of select="'CUSIP'"/>
                    </xsl:when>

                    <xsl:when test="SEDOL!=''">
                      <xsl:value-of select="'SEDOL'"/>
                    </xsl:when>
                    <xsl:when test="Symbol!=''">
                      <xsl:value-of select="'SYMBOL'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <SecurityIdentifierType>
                  <xsl:value-of select ="'ISIN'"/>
                </SecurityIdentifierType>
                <xsl:variable name="varSecurity">
                  <xsl:choose>
                    <xsl:when test="ISIN != ''">
                      <xsl:value-of select ="ISIN"/>
                    </xsl:when>
                    <xsl:when test="CUSIP != ''">
                      <xsl:value-of select ="CUSIP"/>
                    </xsl:when>

                    <xsl:when test="SEDOL != ''">
                      <xsl:value-of select ="SEDOL"/>
                    </xsl:when>

                    <xsl:when test="Symbol!=''">
                      <xsl:value-of select="Symbol"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="Symbol"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <LocalSecurityCode>
                  <xsl:value-of select ="ISIN"/>
                </LocalSecurityCode>

                <ISIN>
                  <xsl:value-of select ="ISIN"/>
                </ISIN>
                <SecurityDescription>
                  <xsl:value-of select ="''"/>
                </SecurityDescription>
                <BlankColumn1>
                  <xsl:value-of select="''"/>
                </BlankColumn1>
         
                <!--Broker & Counterparty details-->

                <FABrokerIdentifierType>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol != 'USD'">
                      <xsl:value-of select ="'BIC_CODE'"/>
                    </xsl:when>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTC'"/>
                    </xsl:when>
                  </xsl:choose>
                </FABrokerIdentifierType>

                
			   <xsl:variable name="THIRDPARTY_PSET1_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@BICCode"/>
              </xsl:variable>
                <FABrokerCode>
                 <xsl:choose>
                    <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </FABrokerCode>

                <FACounterpartyIdentifierType>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol != 'USD'">
                      <xsl:value-of select ="'BIC_CODE'"/>
                    </xsl:when>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTC'"/>
                    </xsl:when>
                  </xsl:choose>
                </FACounterpartyIdentifierType>
                
                


                <FACounterpartyCode>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </FACounterpartyCode>

                <CounterpartyBIC>
                  <xsl:value-of select ="''"/>
                </CounterpartyBIC>

                <CounterpartyDescription>
                  <xsl:value-of select ="''"/>
                </CounterpartyDescription>

                <CounterpartyAccount>
                  <xsl:value-of select ="''"/>
                </CounterpartyAccount>

                <BuyerSellerBIC>
                  <xsl:value-of select ="''"/>
                </BuyerSellerBIC>

                <BuyerSellerDescription>
                  <xsl:value-of select ="''"/>
                </BuyerSellerDescription>

                <BuyerSellerAccount>
                  <xsl:value-of select ="''"/>
                </BuyerSellerAccount>

                <BlankColumn2>
                  <xsl:value-of select="''"/>
                </BlankColumn2>
                
                <BlankColumn3>
                  <xsl:value-of select="''"/>
                </BlankColumn3>
                <!--Trade Details-->

                <xsl:variable name="Date" select="substring-before(OldTradeDate,'T')"/>
                <xsl:variable name="Day" select="substring($Date,9,2)"/>
                <xsl:variable name="Month" select="substring($Date,6,2)"/>
                <xsl:variable name="Year" select="substring($Date,1,4)"/>

                <TradeDate>
                  <xsl:value-of select ="concat($Year,$Month,$Day)"/>
                </TradeDate>

                <xsl:variable name="Date1" select="substring-before(SettlementDate,'T')"/>
              <xsl:variable name="Day1" select="substring($Date1,9,2)"/>
              <xsl:variable name="Month1" select="substring($Date1,6,2)"/>
              <xsl:variable name="Year1" select="substring($Date1,1,4)"/>
                <SettlementDate>
                  <xsl:value-of select ="concat($Year1,$Month1,$Day1)"/>
                </SettlementDate>

                <Quantity>
                  <xsl:value-of select ="OldExecutedQuantity"/>
                </Quantity>

                <DealPriceCCY>
                  <xsl:value-of select ="CurrencySymbol"/>
                </DealPriceCCY>

                <DealPrice>
                  <xsl:value-of select ="OldAvgPrice"/>
                </DealPrice>

                <GrossAmount>
                  <xsl:value-of select ="OldAvgPrice * OldExecutedQuantity"/>
                </GrossAmount>

                <AccruedInterest>
                  <xsl:value-of select ="OldAccruedInterest"/>
                </AccruedInterest>
                <PlaceOfTrade>
                  <xsl:value-of select ="''"/>
                </PlaceOfTrade>

                <BlankColumn4>
                  <xsl:value-of select="''"/>
                </BlankColumn4>

                <BlankColumn5>
                  <xsl:value-of select="''"/>
                </BlankColumn5>


                <!--Costs and Taxes-->

                <StampDuty>
                  <xsl:value-of select ="OldStampDuty"/>
                </StampDuty>

                <ClearingFee>
                  <xsl:value-of select ="OldClearingFee"/>
                </ClearingFee>

                <OtherFee>
                  <xsl:value-of select ="''"/>
                </OtherFee>

                <BrokerCommission>
                  <xsl:value-of select ="''"/>
                </BrokerCommission>

                <SalesTax>
                  <xsl:value-of select ="''"/>
                </SalesTax>

                <Levy>
                  <xsl:value-of select ="OldTransactionLevy"/>
                </Levy>

                <GSTBrokerage>
                  <xsl:value-of select ="''"/>
                </GSTBrokerage>

                <GSTClearing>
                  <xsl:value-of select ="''"/>
                </GSTClearing>

                <GSTScanning>
                  <xsl:value-of select ="''"/>
                </GSTScanning>

                <GSTTransaction>
                  <xsl:value-of select ="''"/>
                </GSTTransaction>

                <CDSFees>
                  <xsl:value-of select ="''"/>
                </CDSFees>

                <CSEFees>
                  <xsl:value-of select ="''"/>
                </CSEFees>

                <GOVTCess>
                  <xsl:value-of select ="''"/>
                </GOVTCess>

                <SECCess>
                  <xsl:value-of select ="''"/>
                </SECCess>

                <SCCP>
                  <xsl:value-of select ="''"/>
                </SCCP>

                <BoughtAccruedInterest>
                  <xsl:value-of select ="''"/>
                </BoughtAccruedInterest>

                <ARBIRCBTax>
                  <xsl:value-of select ="''"/>
                </ARBIRCBTax>

                <PrepaidTaxDiscount>
                  <xsl:value-of select ="''"/>
                </PrepaidTaxDiscount>

                <PrepaidTaxPremium>
                  <xsl:value-of select ="''"/>
                </PrepaidTaxPremium>

                <WitholdingTaxonSDAACCInt>
                  <xsl:value-of select ="''"/>
                </WitholdingTaxonSDAACCInt>

                <TransactionFee>
                  <xsl:value-of select ="''"/>
                </TransactionFee>

                <Reserve>
                  <xsl:value-of select ="''"/>
                </Reserve>

                <Reserve1>
                  <xsl:value-of select ="''"/>
                </Reserve1>

                <Reserve2>
                  <xsl:value-of select ="''"/>
                </Reserve2>

                <Reserve3>
                  <xsl:value-of select ="''"/>
                </Reserve3>

                <!--Settlement Details-->

                <PSET>
                  <xsl:value-of select ="''"/>
                </PSET>
                <PSAFE>
                  <xsl:value-of select ="''"/>
                </PSAFE>
                <SettlementCCY>
                  <xsl:value-of select ="''"/>
                </SettlementCCY>
                <SettlementAmount>
                  <xsl:value-of select ="''"/>
                </SettlementAmount>

                <TransferType>
                  <xsl:value-of select ="''"/>
                </TransferType>

                <BlankColumn6>
                  <xsl:value-of select ="''"/>
                </BlankColumn6>

                <NarrativeLine1>
                  <xsl:value-of select ="''"/>
                </NarrativeLine1>

                <NarrativeLine2>
                  <xsl:value-of select ="''"/>
                </NarrativeLine2>

                <NarrativeLine3>
                  <xsl:value-of select ="''"/>
                </NarrativeLine3>

                <NarrativeLine4>
                  <xsl:value-of select ="''"/>
                </NarrativeLine4>

                <RegistrationNarrative>
                  <xsl:value-of select ="''"/>
                </RegistrationNarrative>

                <BlankColumn7>
                  <xsl:value-of select ="''"/>
                </BlankColumn7>

                <BlankColumn8>
                  <xsl:value-of select ="''"/>
                </BlankColumn8>

                <BlankColumn9>
                  <xsl:value-of select ="''"/>
                </BlankColumn9>

                <BlankColumn10>
                  <xsl:value-of select ="''"/>
                </BlankColumn10>

                <BlankColumn11>
                  <xsl:value-of select ="''"/>
                </BlankColumn11>

                <BlankColumn12>
                  <xsl:value-of select ="''"/>
                </BlankColumn12>
                
                <EndofLineIndicator>
                  <xsl:value-of select ="'END'"/>
                </EndofLineIndicator>


                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>


              </ThirdPartyFlatFileDetail>
            </xsl:if>

            <ThirdPartyFlatFileDetail>
              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <!--Transaction Details-->
               <xsl:variable name="PB_NAME">
                  <xsl:value-of select="'EZE'"/>
                </xsl:variable>

                <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CurrencySymbol"/>
                <xsl:variable name="THIRDPARTY_PSET_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@RecBIC"/>
                </xsl:variable>
                <ReceiverBIC>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ReceiverBIC>

              <xsl:variable name="varAction">
                <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of select="'NEWM'"/>
                  </xsl:when>
                  <xsl:when test ="TaxLotState = 'Amemded'">
                    <xsl:value-of select="'CANC'"/>
                  </xsl:when>
                  
                  <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="'CANC'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:variable>
              <Action>
                <xsl:value-of select ="'NEWM'"/>
              </Action>

              <ClientReference>
                <xsl:value-of select="PBUniqueID"/>
              </ClientReference>

              <PreviousReferenceNo>
                <xsl:value-of select ="''"/>
              </PreviousReferenceNo>

              <TradeType>
                <xsl:value-of select ="'F'"/>
              </TradeType>

              <TransactionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TransactionType>


              <InstrumentType>
                <xsl:choose>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'E'"/>
                  </xsl:when>

                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </InstrumentType>
              <BlankColumn>
                <xsl:value-of select="''"/>             
              </BlankColumn>

              <!--Security & Portfolio Details-->
              <SecuritiesAccountno>
                <xsl:value-of select ="''"/>
              </SecuritiesAccountno>

              <FAPortfolioCode>
                <xsl:value-of select ="'S-114552-0'"/>
              </FAPortfolioCode>
              
              <xsl:variable name="varSecurityIDType">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'ISIN'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="'CUSIP'"/>
                  </xsl:when>

                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="'SEDOL'"/>
                  </xsl:when>
                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="'SYMBOL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <SecurityIdentifierType>
                <xsl:value-of select ="'ISIN'"/>
              </SecurityIdentifierType>
              
              <xsl:variable name="varSecurity">
                <xsl:choose>
                  <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>

                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>

                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <LocalSecurityCode>
                <xsl:value-of select ="ISIN"/>
              </LocalSecurityCode>

              <ISIN>
                <xsl:value-of select ="ISIN"/>
              </ISIN>
              
              <SecurityDescription>
                <xsl:value-of select ="''"/>
              </SecurityDescription>

              <BlankColumn1>
                <xsl:value-of select="''"/>
              </BlankColumn1>

              <!--Broker & Counterparty details-->

              <FABrokerIdentifierType>
                <xsl:choose>                 
                  <xsl:when test="CurrencySymbol != 'USD'">
                    <xsl:value-of select ="'BIC_CODE'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTC'"/>
                  </xsl:when>                 
                </xsl:choose>
              </FABrokerIdentifierType>
              
              <xsl:variable name="THIRDPARTY_PSET1_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_NAME]/@BICCode"/>
              </xsl:variable>
              <FABrokerCode>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FABrokerCode>

              <FACounterpartyIdentifierType>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol != 'USD'">
                    <xsl:value-of select ="'BIC_CODE'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTC'"/>
                  </xsl:when>
                </xsl:choose>
              </FACounterpartyIdentifierType>

             

              <FACounterpartyCode>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET1_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET1_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FACounterpartyCode>

              <CounterpartyBIC>
                <xsl:value-of select ="''"/>
              </CounterpartyBIC>

              <CounterpartyDescription>
                <xsl:value-of select ="''"/>
              </CounterpartyDescription>

              <CounterpartyAccount>
                <xsl:value-of select ="''"/>
              </CounterpartyAccount>

              <BuyerSellerBIC>
                <xsl:value-of select ="''"/>
              </BuyerSellerBIC>

              <BuyerSellerDescription>
                <xsl:value-of select ="''"/>
              </BuyerSellerDescription>

              <BuyerSellerAccount>
                <xsl:value-of select ="''"/>
              </BuyerSellerAccount>

              <BlankColumn2>
                <xsl:value-of select="''"/>
              </BlankColumn2>
              <BlankColumn3>
                <xsl:value-of select="''"/>
              </BlankColumn3>
              <!--Trade Details-->

              <xsl:variable name="Date" select="substring-before(TradeDate,'T')"/>
              <xsl:variable name="Day" select="substring($Date,9,2)"/>
              <xsl:variable name="Month" select="substring($Date,6,2)"/>
              <xsl:variable name="Year" select="substring($Date,1,4)"/>

              <TradeDate>
                <xsl:value-of select ="concat($Year,$Month,$Day)"/>
              </TradeDate>

              <xsl:variable name="SDate" select="substring-before(SettlementDate,'T')"/>
              <xsl:variable name="Day1" select="substring($SDate,9,2)"/>
              <xsl:variable name="Month1" select="substring($SDate,6,2)"/>
              <xsl:variable name="Year1" select="substring($SDate,1,4)"/>

              <SettlementDate>
                <xsl:value-of select ="concat($Year1,$Month1,$Day1)"/>
              </SettlementDate>

              <Quantity>
                <xsl:value-of select ="CumQty"/>
              </Quantity>

              <DealPriceCCY>
                <xsl:value-of select ="CurrencySymbol"/>
              </DealPriceCCY>

              <DealPrice>
                <xsl:value-of select ="AvgPrice"/>
              </DealPrice>

              <GrossAmount>
                <xsl:value-of select ="AvgPrice * CumQty"/>
              </GrossAmount>

              <AccruedInterest>
                <xsl:value-of select ="AccruedInterest"/>
              </AccruedInterest>

              <PlaceOfTrade>
                <xsl:value-of select ="''"/>
              </PlaceOfTrade>

              <BlankColumn4>
                <xsl:value-of select="''"/>
              </BlankColumn4>

              <BlankColumn5>
                <xsl:value-of select="''"/>
              </BlankColumn5>

              <!--Costs and Taxes-->

              <StampDuty>
                <xsl:value-of select ="StampDuty"/>
              </StampDuty>

              <ClearingFee>
                <xsl:value-of select ="ClearingFee"/>
              </ClearingFee>

              <OtherFee>
                <xsl:value-of select ="''"/>
              </OtherFee>

              <BrokerCommission>
                <xsl:value-of select ="''"/>
              </BrokerCommission>

              <SalesTax>
                <xsl:value-of select ="''"/>
              </SalesTax>

              <Levy>
                <xsl:value-of select ="TransactionLevy"/>
              </Levy>

              <GSTBrokerage>
                <xsl:value-of select ="''"/>
              </GSTBrokerage>

              <GSTClearing>
                <xsl:value-of select ="''"/>
              </GSTClearing>

              <GSTScanning>
                <xsl:value-of select ="''"/>
              </GSTScanning>

              <GSTTransaction>
                <xsl:value-of select ="''"/>
              </GSTTransaction>

              <CDSFees>
                <xsl:value-of select ="''"/>
              </CDSFees>

              <CSEFees>
                <xsl:value-of select ="''"/>
              </CSEFees>

              <GOVTCess>
                <xsl:value-of select ="''"/>
              </GOVTCess>

              <SECCess>
                <xsl:value-of select ="''"/>
              </SECCess>

              <SCCP>
                <xsl:value-of select ="''"/>
              </SCCP>

              <BoughtAccruedInterest>
                <xsl:value-of select ="''"/>
              </BoughtAccruedInterest>

              <ARBIRCBTax>
                <xsl:value-of select ="''"/>
              </ARBIRCBTax>

              <PrepaidTaxDiscount>
                <xsl:value-of select ="''"/>
              </PrepaidTaxDiscount>

              <PrepaidTaxPremium>
                <xsl:value-of select ="''"/>
              </PrepaidTaxPremium>

              <WitholdingTaxonSDAACCInt>
                <xsl:value-of select ="''"/>
              </WitholdingTaxonSDAACCInt>

              <TransactionFee>
                <xsl:value-of select ="''"/>
              </TransactionFee>

              <Reserve>
                <xsl:value-of select ="''"/>
              </Reserve>

              <Reserve1>
                <xsl:value-of select ="''"/>
              </Reserve1>

              <Reserve2>
                <xsl:value-of select ="''"/>
              </Reserve2>

              <Reserve3>
                <xsl:value-of select ="''"/>
              </Reserve3>

              <!--Settlement Details-->

              <PSET>
                <xsl:value-of select ="''"/>
              </PSET>
              
              <PSAFE>
                <xsl:value-of select ="''"/>
              </PSAFE>
              
              <SettlementCCY>
                <xsl:value-of select ="''"/>
              </SettlementCCY>
              <SettlementAmount>
                <xsl:value-of select ="''"/>
              </SettlementAmount>

              <TransferType>
                <xsl:value-of select ="''"/>
              </TransferType>

              <BlankColumn6>
                <xsl:value-of select ="''"/>
              </BlankColumn6>
             
              <NarrativeLine1>
                <xsl:value-of select ="''"/>
              </NarrativeLine1>

              <NarrativeLine2>
                <xsl:value-of select ="''"/>
              </NarrativeLine2>

              <NarrativeLine3>
                <xsl:value-of select ="''"/>
              </NarrativeLine3>

              <NarrativeLine4>
                <xsl:value-of select ="''"/>
              </NarrativeLine4>

              <RegistrationNarrative>
                <xsl:value-of select ="''"/>
              </RegistrationNarrative>

              <BlankColumn7>
                <xsl:value-of select ="''"/>
              </BlankColumn7>

              <BlankColumn8>
                <xsl:value-of select ="''"/>
              </BlankColumn8>

              <BlankColumn9>
                <xsl:value-of select ="''"/>
              </BlankColumn9>

              <BlankColumn10>
                <xsl:value-of select ="''"/>
              </BlankColumn10>

              <BlankColumn11>
                <xsl:value-of select ="''"/>
              </BlankColumn11>

              <BlankColumn12>
                <xsl:value-of select ="''"/>
              </BlankColumn12>
              
              <EndofLineIndicator>
                <xsl:value-of select ="'END'"/>
              </EndofLineIndicator>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>


            </ThirdPartyFlatFileDetail>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>