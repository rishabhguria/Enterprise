<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
            
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varSide1">
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell short' ">
                <xsl:value-of select ="'SELL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <TransactionType>
            <xsl:value-of select ="$varSide1"/>
          </TransactionType>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEWM'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'AMEND'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CAN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <MessageFunction>
            <xsl:value-of select ="$varTaxlotState"/>
          </MessageFunction>
          
          <TransactionReference>
            <xsl:value-of select ="PBUniqueID"/>
          </TransactionReference>
          
          <RelatedReferenceNumber>
            <xsl:value-of select ="''"/>
          </RelatedReferenceNumber>
          
          <FundID>
            <xsl:value-of select ="''"/>
          </FundID>
          
          <TradeDate>
            <xsl:value-of select ="TradeDate"/>
          </TradeDate>
          
          <SettlementDate>
            <xsl:value-of select ="SettleDate"/>
          </SettlementDate>
          
          <LateDeliveryDate>
            <xsl:value-of select ="''"/>
          </LateDeliveryDate>
          
          <SecurityIDType>
            <xsl:value-of select ="'US'"/>
          </SecurityIDType>

          <xsl:variable name="varSecurityID">
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
            <xsl:value-of select ="$varSecurityID"/>
          </SecurityID>
          
          <SecurityDescription>
            <xsl:value-of select ="CompanyName"/>
          </SecurityDescription>
          
          <SecurityType>
            <xsl:value-of select ="'CS'"/>
          </SecurityType>
          
          <CurrencyOfDenomination>
            <xsl:value-of select ="''"/>
          </CurrencyOfDenomination>
          
          <OptionStyle>
            <xsl:value-of select ="''"/>
          </OptionStyle>
          
          <OptionType>
            <xsl:value-of select ="''"/>
          </OptionType>
          
          <ContractSize>
            <xsl:value-of select ="''"/>
          </ContractSize>
          
          <StrikePrice>
            <xsl:value-of select ="''"/>
          </StrikePrice>
          
          <ExpirationDate>
            <xsl:value-of select ="''"/>
          </ExpirationDate>
          
          <UnderlyingSecurityIDType>
            <xsl:value-of select ="''"/>
          </UnderlyingSecurityIDType>
          
          <UnderlyingSecurityID>
            <xsl:value-of select ="''"/>
          </UnderlyingSecurityID>
          
          <UnderlyingSecurityDesc>
            <xsl:value-of select ="''"/>
          </UnderlyingSecurityDesc>
          
          <MaturityDate>
            <xsl:value-of select ="''"/>
          </MaturityDate>
          
          <IssueDate>
            <xsl:value-of select ="''"/>
          </IssueDate>
          
          <InterestRate>
            <xsl:value-of select ="''"/>
          </InterestRate>
          <OriginalFace>
            <xsl:value-of select ="''"/>
          </OriginalFace>
          
          <Quantity>
            <xsl:value-of select ="AllocatedQty"/>
          </Quantity>
          
          <TradeCurrency>
            <xsl:value-of select ="Currency"/>
          </TradeCurrency>
          
          <DealPriceCode>
            <xsl:value-of select ="'ACTU'"/>
          </DealPriceCode>
          
          <DealPrice>
            <xsl:value-of select ="format-number(AveragePrice,'#.####')"/>
          </DealPrice>
          
          <PrincipalAmount>
            <xsl:value-of select ="format-number(GrossAmount,'#.####')"/>
          </PrincipalAmount>

          <xsl:variable name ="varTotalCommission">
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.####')"/>
          </xsl:variable>
                        
          <CommissionsAmount>
            <xsl:value-of select ="$varTotalCommission"/>
          </CommissionsAmount>
          
          <ChargesFeesAmount>
            <xsl:value-of select ="SecFee"/>
          </ChargesFeesAmount>
          
          <OtherAmount>
            <xsl:value-of select ="OtherFee"/>
          </OtherAmount>
          
          <AccruedInterestAmount>
            <xsl:value-of select ="AccruedInterestAmount"/>
          </AccruedInterestAmount>

          <xsl:variable name ="varTaxesAmount">
            <xsl:value-of select="format-number(TaxOnCommissions,'#.##')"/>
          </xsl:variable>
          <TaxesAmount>
            <xsl:value-of select ="$varTaxesAmount"/>
          </TaxesAmount>
          
          <StampDutyExemptionAmount>
            <xsl:value-of select ="StampDutyFee"/>
          </StampDutyExemptionAmount>
          
          <SettlementCurrency>
            <xsl:value-of select ="SettlementCurrency"/>
          </SettlementCurrency>


          <xsl:variable name ="varSettlementAmount">
            <xsl:value-of select="format-number(NetAmount,'#.##')"/>
          </xsl:variable>
          <SettlementAmount>
            <xsl:value-of select ="$varSettlementAmount"/>
          </SettlementAmount>
          
          <TransactionSubType>
            <xsl:value-of select ="'TRAD'"/>
          </TransactionSubType>
          
          <SettlementTransactionConditionIndicator>
            <xsl:value-of select ="''"/>
          </SettlementTransactionConditionIndicator>
          
          <SettlementTransactionConditionIndicator2>
            <xsl:value-of select ="''"/>
          </SettlementTransactionConditionIndicator2>
          
          <ProcessingIndicator>
            <xsl:value-of select ="''"/>
          </ProcessingIndicator>
          
          <TrackingIndicator>
            <xsl:value-of select ="''"/>
          </TrackingIndicator>
          
          <SettlementLocation>
            <xsl:value-of select ="'DTCYUS33'"/>
          </SettlementLocation>
          
          <PlaceOfTrade>
            <xsl:value-of select ="''"/>
          </PlaceOfTrade>
          
          <PlaceOfSafekeeping>
            <xsl:value-of select ="''"/>
          </PlaceOfSafekeeping>
          
          <FXContraCurrency>
            <xsl:value-of select ="''"/>
          </FXContraCurrency>
          
          <FXOrderCXLIndicator>
            <xsl:value-of select ="''"/>
          </FXOrderCXLIndicator>
          
          <ExecutingBrokerIDType>
            <xsl:value-of select ="'DTCYID'"/>
          </ExecutingBrokerIDType>


          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>
          
          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
          <xsl:variable name="THIRDPARTY_BROKER">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
          </xsl:variable>
          <ExecutingBrokerID>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER!= ''">
                <xsl:value-of select="$THIRDPARTY_BROKER"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExecutingBrokerID>
          
          <ExecutingBrokerAcct>
            <xsl:value-of select ="''"/>
          </ExecutingBrokerAcct>
          
          <ClearingBrokerAgentIDType>
            <xsl:value-of select ="'DTCYID'"/>
          </ClearingBrokerAgentIDType>
          
          <ClearingBrokerAgentID>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER!= ''">
                <xsl:value-of select="$THIRDPARTY_BROKER"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClearingBrokerAgentID>
          
          <ExposureTypeIndicator>
            <xsl:value-of select ="''"/>
          </ExposureTypeIndicator>
          
          <NetMovementIndicator>
            <xsl:value-of select ="''"/>
          </NetMovementIndicator>
          
          <NetMovementAmount>
            <xsl:value-of select ="''"/>
          </NetMovementAmount>
          
          <IntermediaryIDType>
            <xsl:value-of select ="''"/>
          </IntermediaryIDType>
          
          <IntermediaryID>
            <xsl:value-of select ="''"/>
          </IntermediaryID>
          
          <AcctWithInstitutionIDType>
            <xsl:value-of select ="''"/>
          </AcctWithInstitutionIDType>
          
          <AcctWithInstitutionID>
            <xsl:value-of select ="''"/>
          </AcctWithInstitutionID>
          
          <PayingInstitution>
            <xsl:value-of select ="''"/>
          </PayingInstitution>
          
          <BeneficiaryOfMoney>
            <xsl:value-of select ="''"/>
          </BeneficiaryOfMoney>
          
          <CashAcct>
            <xsl:value-of select ="''"/>
          </CashAcct>
          
          <CBO>
            <xsl:value-of select ="''"/>
          </CBO>
          
          <StampDutyExemption>
            <xsl:value-of select ="''"/>
          </StampDutyExemption>
          
          <StampCode>
            <xsl:value-of select ="''"/>
          </StampCode>
          
          <TRADDETNarrative>
            <xsl:value-of select ="''"/>
          </TRADDETNarrative>
          
          <FIANarrative>
            <xsl:value-of select ="''"/>
          </FIANarrative>
          
          <ProcessingReference>
            <xsl:value-of select ="''"/>
          </ProcessingReference>
          
          <ClearingBrokerAccount>
            <xsl:value-of select ="''"/>
          </ClearingBrokerAccount>
          
          <Restrictions>
            <xsl:value-of select ="''"/>
          </Restrictions>
          
          <RepoTermOpenInd>
            <xsl:value-of select ="''"/>
          </RepoTermOpenInd>
          
          <RepoTermDate>
            <xsl:value-of select ="''"/>
          </RepoTermDate>
          
          <RepoRateType>
            <xsl:value-of select ="''"/>
          </RepoRateType>
          
          <RepoRate>
            <xsl:value-of select ="''"/>
          </RepoRate>
          
          <RepoReference>
            <xsl:value-of select ="''"/>
          </RepoReference>
          
          <RepoTotalTermAmt>
            <xsl:value-of select ="''"/>
          </RepoTotalTermAmt>
          
          <RepoAccrueAmt>
            <xsl:value-of select ="''"/>
          </RepoAccrueAmt>
          
          <RepoTotalCollCnt>
            <xsl:value-of select ="''"/>
          </RepoTotalCollCnt>
          
          <RepoCollNumb>
            <xsl:value-of select ="''"/>
          </RepoCollNumb>
          
          <RepoTypeInd>
            <xsl:value-of select ="''"/>
          </RepoTypeInd>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>