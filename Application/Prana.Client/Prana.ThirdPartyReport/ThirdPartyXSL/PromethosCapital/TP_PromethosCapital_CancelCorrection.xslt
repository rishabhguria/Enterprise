<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before($Date,'-'),substring-before(substring-after($Date,'-'),'-'),substring-before(substring-after(substring-after($Date,'-'),'-'),'T'))"/>
  </xsl:template>
  
 

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        
        
        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>
              
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
                          
               <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
        
             
              <ClientReference>
                <xsl:choose>
                      <xsl:when test="string-length(position())=1">
                        <xsl:value-of select="concat('ABC','0', position())"/>
                      </xsl:when>
                      <xsl:otherwise>
                       <xsl:value-of select="concat('ABC', position())"/>
                      </xsl:otherwise>
                   </xsl:choose>
              </ClientReference>
              
              <CancellationIndicator>
                <xsl:value-of select ="''"/>
              </CancellationIndicator>
              
              <RelatedReference>
                <xsl:value-of select ="''"/>
              </RelatedReference>
              
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <SafekeepingAccount>
               <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SafekeepingAccount>
              
             
              <TradeDate>
                <xsl:value-of select ="$varTradeDate"/>
              </TradeDate>
              
               <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select ="$varSettlementDate"/>
              </SettlementDate>
              
                <xsl:variable name="varSecurityClass">
                  <xsl:choose>                
                    <xsl:when test="Asset = 'Equity'">
                      <xsl:value-of select="'EQ'"/>
                    </xsl:when>
                    <xsl:when test="Asset='FixedIncome'">
                      <xsl:value-of select="'FI'"/>
                    </xsl:when>                 
                                       
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
              <SecurityClass>
                <xsl:value-of select ="$varSecurityClass"/>
              </SecurityClass>
              
              <xsl:variable name="varSecurity">
                <xsl:choose>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="BBCode != ''">
                    <xsl:value-of select ="BBCode"/>
                  </xsl:when>
                   <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecurityID>
                <xsl:value-of select ="$varSecurity"/>
              </SecurityID>
              
              <Quantity>
                <xsl:value-of select ="OrderQty"/>
              </Quantity>
              
              <Price>
                <xsl:value-of select ="AvgPrice"/>
              </Price>
              
              <CCY>
                <xsl:value-of select ="CurrencySymbol"/>
              </CCY>
               <xsl:variable name="varGrossAmount">
                <xsl:value-of select ="OrderQty * AvgPrice * AssetMultiplier"/>
              </xsl:variable>
              <DealAmount>
                <xsl:value-of select ="$varGrossAmount"/>
              </DealAmount>
              
              <Consideration>
                  <xsl:value-of select ="$varNetamount"/>             
              </Consideration>
              
               <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@PSET"/>
              </xsl:variable>
              <SettlementLocationBIC>
                 <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementLocationBIC>
              
              <CountryofTradePlace>
                <xsl:value-of select ="''"/>
              </CountryofTradePlace>
              
              
                <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>
                <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
                </xsl:variable>
                <ExecBrokerIDType>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerIDType>

                <ExecBrokerID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerID>

               <xsl:variable name="THIRDPARTY_ClearerIDType_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerIDType"/>
              </xsl:variable>
              <ClearingAgentIDType>
              <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerIDType_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerIDType_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingAgentIDType>
              
               <xsl:variable name="THIRDPARTY_ClearerID_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerID"/>
              </xsl:variable>
              <ClearingAgentID>
               <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerID_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerID_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingAgentID>
              
              <SecurityType>
                <xsl:value-of select ="''"/>
              </SecurityType>
              
              <SecurityDescription>
                <xsl:value-of select ="CompanyName"/>
              </SecurityDescription>
              
              <BrokerCommission>
               <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </BrokerCommission>
              
               <xsl:variable name="varAllFee">
                   <xsl:value-of select =" CommissionCharged + SoftCommissionCharged + OtherBrokerFees + TransactionLevy +ClearingBrokerFee + ClearingFee + MiscFees + SecFee + OccFee + OrfFee"/>        
        </xsl:variable>
        
              <ChargesFees>
                <xsl:value-of select ="$varAllFee"/>
              </ChargesFees>
              
                <MiscFees>
                  <xsl:value-of select ="''"/>
                </MiscFees>
              
                <Taxes>
                  <xsl:value-of select ="TaxOnCommissions"/>
                </Taxes>
              
                <Interest>
                  <xsl:value-of select ="AccruedInterest"/>
                </Interest>
              
                <ValueAddedTax>
                  <xsl:value-of select ="''"/>
                </ValueAddedTax>
              
                <StampDutyType>
                  <xsl:value-of select ="StampDuty"/>
                </StampDutyType>
              
                <StampDutyAmount>
                   <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="StampDuty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </StampDutyAmount>
              
                <CurrentFace>
                  <xsl:value-of select ="''"/>
                </CurrentFace>
              
                <NetGainorLoss>
                  <xsl:value-of select ="''"/>
                </NetGainorLoss>
              
                <FXInformation>
                  <xsl:value-of select ="''"/>
                </FXInformation>
              
                <FXCancellation>
                  <xsl:value-of select ="''"/>
                </FXCancellation>
              
                <ExecBrokerName>
                  <xsl:value-of select ="''"/>
                </ExecBrokerName>
              
                <ExecBrokerBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ExecBrokerBeneficiaryAccount>
              
              <ClearingAgentName>
                <xsl:value-of select ="''"/>
              </ClearingAgentName>
              
                <ClearingAgentBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ClearingAgentBeneficiaryAccount>
              
                <ReceiverDeliverersCustodiansBIC>
                  <xsl:value-of select ="''"/>
                </ReceiverDeliverersCustodiansBIC>
              
                <ReceiverDeliverersCustodiansAgentsName>
                  <xsl:value-of select ="CounterParty"/>
                </ReceiverDeliverersCustodiansAgentsName>
              
                <ReceiverDeliverersCustodiansAgentsAccountCode>
                  <xsl:value-of select ="CounterParty"/>
                </ReceiverDeliverersCustodiansAgentsAccountCode>
              
                <PlaceofSafekeeping>
                  <xsl:value-of select ="''"/>
                </PlaceofSafekeeping>
              
              <HKSettlementOR>
                <xsl:value-of select ="''"/>
                </HKSettlementOR>
              
                <RegistrationOO>
                <xsl:value-of select ="''"/>
                </RegistrationOO>
              
                <MalaysianTradePlace>
                  <xsl:value-of select ="''"/>
                </MalaysianTradePlace>
              
                <ItalianTaxIdentifier>
                  <xsl:value-of select ="''"/>
                </ItalianTaxIdentifier>
              
                <TransactionTypeIndicator>
                  <xsl:value-of select ="''"/>
                </TransactionTypeIndicator>
              
                <QuantitytobePaired>
                  <xsl:value-of select ="''"/>
                </QuantitytobePaired>
              
                <PairABANumber>
                  <xsl:value-of select ="''"/>
                </PairABANumber>
              
                <PairBeneficiaryAccountName>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountName>
              
                <PairBeneficiaryAccountNumber>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountNumber>
              
              <DirtyClean>
                <xsl:value-of select ="''"/>
              </DirtyClean>
              
                <BookTransferindicator>
                  <xsl:value-of select ="''"/>
                </BookTransferindicator>
              
                <TransactionCondition>
                  <xsl:value-of select ="''"/>
                </TransactionCondition>
              
                <FormofSecurities>
                  <xsl:value-of select ="''"/>
                </FormofSecurities>
              
              <SplitSettlement>
                <xsl:value-of select ="''"/>
              </SplitSettlement>
              
                <ChangeofBeneficialOwnership>
                 <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="'N'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </ChangeofBeneficialOwnership>
              
                <Destination>
                  <xsl:value-of select ="''"/>
                </Destination>
              
              <SpecialInstructions>
                <xsl:value-of select ="''"/>
              </SpecialInstructions>
              
                <EndofRecord>
                  <xsl:value-of select ="'EOR'"/>
                </EndofRecord>

              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>
            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>


                <RowHeader>
                  <xsl:value-of select ="'True'"/>
                </RowHeader>

                <TaxLotState>
                  <xsl:value-of select ="'Deleted'"/>
                </TaxLotState>
                
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

              <xsl:variable name="varTradeDates">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="OldTradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

            
                <ClientReference>
                  <xsl:choose>
                      <xsl:when test="string-length(position())=1">
                        <xsl:value-of select="concat('ABC','0', position())"/>
                      </xsl:when>
                      <xsl:otherwise>
                       <xsl:value-of select="concat('ABC', position())"/>
                      </xsl:otherwise>
                   </xsl:choose>
                
                </ClientReference>

                <CancellationIndicator>
                  <xsl:value-of select ="'Y'"/>
                </CancellationIndicator>

                <RelatedReference>
                  <xsl:value-of select ="''"/>
                </RelatedReference>
                
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <SafekeepingAccount>
               <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SafekeepingAccount>
                 <xsl:variable name="varOldTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <TradeDate>
                  <xsl:value-of select ="$varOldTradeDate"/>
                </TradeDate>

                <xsl:variable name="varOldSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettlementDate>
                  <xsl:value-of select ="$varOldSettlementDate"/>
                </SettlementDate>

              <xsl:variable name="varSecurityClass">
                <xsl:choose>
                  <xsl:when test="Asset = 'Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'FI'"/>
                  </xsl:when>
                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
                <SecurityClass>
                  <xsl:value-of select ="$varSecurityClass"/>
                </SecurityClass>

                  <xsl:variable name="varSecurity">
                    <xsl:choose>
                      <xsl:when test="CUSIP != ''">
                        <xsl:value-of select ="CUSIP"/>
                      </xsl:when>
                      <xsl:when test="SEDOL != ''">
                        <xsl:value-of select ="SEDOL"/>
                      </xsl:when>
                      <xsl:when test="BBCode != ''">
                        <xsl:value-of select ="BBCode"/>
                      </xsl:when>
                      <xsl:when test="ISIN != ''">
                        <xsl:value-of select ="ISIN"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select ="Symbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>
                <SecurityID>
                  <xsl:value-of select ="''"/>
                </SecurityID>

                <Quantity>
                  <xsl:value-of select ="OldExecutedQuantity"/>
                </Quantity>

                <Price>
                  <xsl:value-of select ="OldAvgPrice"/>
                </Price>

                <CCY>
                  <xsl:value-of select ="CurrencySymbol"/>
                </CCY>

               <xsl:variable name="varGrossAmounts">
                  <xsl:value-of select ="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
                </xsl:variable>
                <DealAmount>
                  <xsl:value-of select ="$varGrossAmounts"/>
                </DealAmount>
              <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <Consideration>
                  <xsl:value-of select ="$varOldNetAmount"/>
                </Consideration>

                <SettlementLocationBIC>
                  <xsl:value-of select ="''"/>
                </SettlementLocationBIC>

                <CountryofTradePlace>
                  <xsl:value-of select ="''"/>
                </CountryofTradePlace>

                <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
                <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>
                
                <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>
                <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
                </xsl:variable>
                <ExecBrokerIDType>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerIDType>

                <ExecBrokerID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerID>

                <ClearingAgentIDType>
                  <xsl:value-of select ="''"/>
                </ClearingAgentIDType>

                <ClearingAgentID>
                  <xsl:value-of select ="''"/>
                </ClearingAgentID>

                <SecurityType>
                  <xsl:value-of select ="''"/>
                </SecurityType>

                <SecurityDescription>
                  <xsl:value-of select ="CompanyName"/>
                </SecurityDescription>
                
                <xsl:variable name="varBrokerCommission">
                  <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>        
                </xsl:variable>
                <BrokerCommission>
                 <xsl:choose>
                    <xsl:when test="number($varBrokerCommission)">
                      <xsl:value-of select="$varBrokerCommission"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </BrokerCommission>
                
              <xsl:variable name="varAllFees">
                   <xsl:value-of select =" CommissionCharged + SoftCommissionCharged+ OldOtherBrokerFees + OldTransactionLevy + OldClearingBrokerFee + OldClearingFee  + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>        
              </xsl:variable>
                <ChargesFees>
                  <xsl:value-of select ="$varAllFees"/>
                </ChargesFees>

                <MiscFees>
                  <xsl:value-of select ="''"/>
                </MiscFees>

                <Taxes>
                  <xsl:value-of select ="OldTaxOnCommissions"/>
                </Taxes>

                <Interest>
                  <xsl:value-of select ="OldAccruedInterest"/>
                </Interest>

                <ValueAddedTax>
                  <xsl:value-of select ="''"/>
                </ValueAddedTax>

                <StampDutyType>
                  <xsl:value-of select ="OldStampDuty"/>
                </StampDutyType>

                <StampDutyAmount>
                 <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="OldStampDuty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
                </StampDutyAmount>

                <CurrentFace>
                  <xsl:value-of select ="''"/>
                </CurrentFace>

                <NetGainorLoss>
                  <xsl:value-of select ="''"/>
                </NetGainorLoss>

                <FXInformation>
                  <xsl:value-of select ="''"/>
                </FXInformation>

                <FXCancellation>
                  <xsl:value-of select ="''"/>
                </FXCancellation>

                <ExecBrokerName>
                  <xsl:value-of select ="''"/>
                </ExecBrokerName>

                <ExecBrokerBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ExecBrokerBeneficiaryAccount>

                <ClearingAgentName>
                  <xsl:value-of select ="''"/>
                </ClearingAgentName>

                <ClearingAgentBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ClearingAgentBeneficiaryAccount>

                <ReceiverDeliverersCustodiansBIC>
                  <xsl:value-of select ="''"/>
                </ReceiverDeliverersCustodiansBIC>

                <ReceiverDeliverersCustodiansAgentsName>
                  <xsl:value-of select ="OldCounterparty"/>
                </ReceiverDeliverersCustodiansAgentsName>

                <ReceiverDeliverersCustodiansAgentsAccountCode>
                  <xsl:value-of select ="OldCounterparty"/>
                </ReceiverDeliverersCustodiansAgentsAccountCode>

                <PlaceofSafekeeping>
                  <xsl:value-of select ="''"/>
                </PlaceofSafekeeping>

                <HKSettlementOR>
                  <xsl:value-of select ="''"/>
                </HKSettlementOR>

                <RegistrationOO>
                  <xsl:value-of select ="''"/>
                </RegistrationOO>

                <MalaysianTradePlace>
                  <xsl:value-of select ="''"/>
                </MalaysianTradePlace>

                <ItalianTaxIdentifier>
                  <xsl:value-of select ="''"/>
                </ItalianTaxIdentifier>

                <TransactionTypeIndicator>
                  <xsl:value-of select ="''"/>
                </TransactionTypeIndicator>

                <QuantitytobePaired>
                  <xsl:value-of select ="''"/>
                </QuantitytobePaired>

                <PairABANumber>
                  <xsl:value-of select ="''"/>
                </PairABANumber>

                <PairBeneficiaryAccountName>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountName>

                <PairBeneficiaryAccountNumber>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountNumber>

                <DirtyClean>
                  <xsl:value-of select ="''"/>
                </DirtyClean>

                <BookTransferindicator>
                  <xsl:value-of select ="''"/>
                </BookTransferindicator>

                <TransactionCondition>
                  <xsl:value-of select ="''"/>
                </TransactionCondition>

                <FormofSecurities>
                  <xsl:value-of select ="''"/>
                </FormofSecurities>

                <SplitSettlement>
                  <xsl:value-of select ="''"/>
                </SplitSettlement>

                <ChangeofBeneficialOwnership>
                 <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="'N'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </ChangeofBeneficialOwnership>

                <Destination>
                  <xsl:value-of select ="''"/>
                </Destination>

                <SpecialInstructions>
                  <xsl:value-of select ="''"/>
                </SpecialInstructions>

                <EndofRecord>
                  <xsl:value-of select ="'EOR'"/>
                </EndofRecord>

               

              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select ="'Allocated'"/>
              </TaxLotState>
              
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
                          
               <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
        
              <ClientReference>
               <xsl:choose>
                      <xsl:when test="string-length(position())=1">
                        <xsl:value-of select="concat('ABC','0', position())"/>
                      </xsl:when>
                      <xsl:otherwise>
                       <xsl:value-of select="concat('ABC', position())"/>
                      </xsl:otherwise>
                   </xsl:choose>
              </ClientReference>
              
              <CancellationIndicator>
                <xsl:value-of select ="''"/>
              </CancellationIndicator>
              
              <RelatedReference>
                <xsl:value-of select ="''"/>
              </RelatedReference>
              
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <SafekeepingAccount>
               <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SafekeepingAccount>
              
             
              <TradeDate>
                <xsl:value-of select ="$varTradeDate"/>
              </TradeDate>
              
               <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select ="$varSettlementDate"/>
              </SettlementDate>
              
                <xsl:variable name="varSecurityClass">
                  <xsl:choose>                
                    <xsl:when test="Asset = 'Equity'">
                      <xsl:value-of select="'EQ'"/>
                    </xsl:when>
                    <xsl:when test="Asset='FixedIncome'">
                      <xsl:value-of select="'FI'"/>
                    </xsl:when>                 
                                    
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
              <SecurityClass>
                <xsl:value-of select ="$varSecurityClass"/>
              </SecurityClass>
              
              <xsl:variable name="varSecurity">
                <xsl:choose>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="BBCode != ''">
                    <xsl:value-of select ="BBCode"/>
                  </xsl:when>
                   <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecurityID>
                <xsl:value-of select ="$varSecurity"/>
              </SecurityID>
              
              <Quantity>
                <xsl:value-of select ="OrderQty"/>
              </Quantity>
              
              <Price>
                <xsl:value-of select ="AvgPrice"/>
              </Price>
              
              <CCY>
                <xsl:value-of select ="CurrencySymbol"/>
              </CCY>
               <xsl:variable name="varGrossAmount">
                <xsl:value-of select ="OrderQty * AvgPrice * AssetMultiplier"/>
              </xsl:variable>
              <DealAmount>
                <xsl:value-of select ="$varGrossAmount"/>
              </DealAmount>
              
              <Consideration>
                  <xsl:value-of select ="$varNetamount"/>             
              </Consideration>
              
               <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@PSET"/>
              </xsl:variable>
              <SettlementLocationBIC>
                 <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementLocationBIC>
              
              <CountryofTradePlace>
                <xsl:value-of select ="''"/>
              </CountryofTradePlace>
              
              
                <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>
                <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
                </xsl:variable>
                <ExecBrokerIDType>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerIDType>

                <ExecBrokerID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecBrokerID>

               <xsl:variable name="THIRDPARTY_ClearerIDType_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerIDType"/>
              </xsl:variable>
              <ClearingAgentIDType>
              <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerIDType_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerIDType_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingAgentIDType>
              
               <xsl:variable name="THIRDPARTY_ClearerID_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerID"/>
              </xsl:variable>
              <ClearingAgentID>
               <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerID_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerID_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingAgentID>
              
              <SecurityType>
                <xsl:value-of select ="''"/>
              </SecurityType>
              
              <SecurityDescription>
                <xsl:value-of select ="CompanyName"/>
              </SecurityDescription>
              
               <xsl:variable name="varBrokerCommission">
                  <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>        
                </xsl:variable>
              <BrokerCommission>
               <xsl:choose>
                    <xsl:when test="number($varBrokerCommission)">
                      <xsl:value-of select="$varBrokerCommission"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
              </BrokerCommission>
              
               <xsl:variable name="varAllFee">
                   <xsl:value-of select = "CommissionCharged + SoftCommissionCharged + OtherBrokerFees + TransactionLevy + ClearingBrokerFee + ClearingFee  + MiscFees + SecFee + OccFee + OrfFee"/>        
        </xsl:variable>
        
              <ChargesFees>
                <xsl:value-of select ="$varAllFee"/>
              </ChargesFees>
              
                <MiscFees>
                  <xsl:value-of select ="''"/>
                </MiscFees>
              
                <Taxes>
                  <xsl:value-of select ="TaxOnCommissions"/>
                </Taxes>
              
                <Interest>
                  <xsl:value-of select ="AccruedInterest"/>
                </Interest>
              
                <ValueAddedTax>
                  <xsl:value-of select ="''"/>
                </ValueAddedTax>
              
                <StampDutyType>
                  <xsl:value-of select ="StampDuty"/>
                </StampDutyType>
              
                <StampDutyAmount>
                   <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="StampDuty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </StampDutyAmount>
              
                <CurrentFace>
                  <xsl:value-of select ="''"/>
                </CurrentFace>
              
                <NetGainorLoss>
                  <xsl:value-of select ="''"/>
                </NetGainorLoss>
              
                <FXInformation>
                  <xsl:value-of select ="''"/>
                </FXInformation>
              
                <FXCancellation>
                  <xsl:value-of select ="''"/>
                </FXCancellation>
              
                <ExecBrokerName>
                  <xsl:value-of select ="''"/>
                </ExecBrokerName>
              
                <ExecBrokerBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ExecBrokerBeneficiaryAccount>
              
              <ClearingAgentName>
                <xsl:value-of select ="''"/>
              </ClearingAgentName>
              
                <ClearingAgentBeneficiaryAccount>
                  <xsl:value-of select ="''"/>
                </ClearingAgentBeneficiaryAccount>
              
                <ReceiverDeliverersCustodiansBIC>
                  <xsl:value-of select ="''"/>
                </ReceiverDeliverersCustodiansBIC>
              
                <ReceiverDeliverersCustodiansAgentsName>
                  <xsl:value-of select ="CounterParty"/>
                </ReceiverDeliverersCustodiansAgentsName>
              
                <ReceiverDeliverersCustodiansAgentsAccountCode>
                  <xsl:value-of select ="CounterParty"/>
                </ReceiverDeliverersCustodiansAgentsAccountCode>
              
                <PlaceofSafekeeping>
                  <xsl:value-of select ="''"/>
                </PlaceofSafekeeping>
              
              <HKSettlementOR>
                <xsl:value-of select ="''"/>
                </HKSettlementOR>
              
                <RegistrationOO>
                <xsl:value-of select ="''"/>
                </RegistrationOO>
              
                <MalaysianTradePlace>
                  <xsl:value-of select ="''"/>
                </MalaysianTradePlace>
              
                <ItalianTaxIdentifier>
                  <xsl:value-of select ="''"/>
                </ItalianTaxIdentifier>
              
                <TransactionTypeIndicator>
                  <xsl:value-of select ="''"/>
                </TransactionTypeIndicator>
              
                <QuantitytobePaired>
                  <xsl:value-of select ="''"/>
                </QuantitytobePaired>
              
                <PairABANumber>
                  <xsl:value-of select ="''"/>
                </PairABANumber>
              
                <PairBeneficiaryAccountName>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountName>
              
                <PairBeneficiaryAccountNumber>
                  <xsl:value-of select ="''"/>
                </PairBeneficiaryAccountNumber>
              
              <DirtyClean>
                <xsl:value-of select ="''"/>
              </DirtyClean>
              
                <BookTransferindicator>
                  <xsl:value-of select ="''"/>
                </BookTransferindicator>
              
                <TransactionCondition>
                  <xsl:value-of select ="''"/>
                </TransactionCondition>
              
                <FormofSecurities>
                  <xsl:value-of select ="''"/>
                </FormofSecurities>
              
              <SplitSettlement>
                <xsl:value-of select ="''"/>
              </SplitSettlement>
              
                <ChangeofBeneficialOwnership>
                 <xsl:choose>
                  <xsl:when test="CurrencySymbol= 'HKD'">
                    <xsl:value-of select="'N'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </ChangeofBeneficialOwnership>
              
                <Destination>
                  <xsl:value-of select ="''"/>
                </Destination>
              
              <SpecialInstructions>
                <xsl:value-of select ="''"/>
              </SpecialInstructions>
              
                <EndofRecord>
                  <xsl:value-of select ="'EOR'"/>
                </EndofRecord>

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
