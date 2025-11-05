<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public int RoundOff(double Qty)
    {

    return (int)Math.Round(Qty,0);
    }
  </msxsl:script>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>


        <TypeOfRequest>
          <xsl:value-of select="'Type Of Request'"/>
        </TypeOfRequest>

        <OriginalCustomerReferenceNumber>
          <xsl:value-of select="'Original Customer Reference Number'"/>
        </OriginalCustomerReferenceNumber>

        <PreviousReferenceNumber>
          <xsl:value-of select="'Previous Reference Number'"/>
        </PreviousReferenceNumber>



        <SafekeepingAccount>
          <xsl:value-of select="'Safekeeping Account'"/>
        </SafekeepingAccount>

        <CustomerReferenceNumber>
          <xsl:value-of select="'Customer Reference Number'"/>
        </CustomerReferenceNumber>

        <InstructionType>
          <xsl:value-of select="'Instruction Type'"/>
        </InstructionType>

        <TransactionType>
          <xsl:value-of select="'Transaction Type'"/>
        </TransactionType>

        <SecurityIdType>
          <xsl:value-of select="'Security Id Type'"/>
        </SecurityIdType>

        <SecurityId>
          <xsl:value-of select="'Security Id'"/>
        </SecurityId>

        <SecurityId2>
          <xsl:value-of select="'Security Id 2'"/>
        </SecurityId2>


        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>

        <ReceiveAccount>
          <xsl:value-of select="'Receive Account'"/>
        </ReceiveAccount>


        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <SettlementLocation>
          <xsl:value-of select="'Settlement Location'"/>
        </SettlementLocation>

        <PlaceofSafekeeping>
          <xsl:value-of select="'Place of Safekeeping'"/>
        </PlaceofSafekeeping>

        <SettlementCurrency>
          <xsl:value-of select="'Settlement Currency'"/>
        </SettlementCurrency>


        <SettlementQuantity>
          <xsl:value-of select="'Settlement Quantity'"/>
        </SettlementQuantity>

        <SettlementAmount>
          <xsl:value-of select="'Settlement Amount'"/>
        </SettlementAmount>

        <Freetext>
          <xsl:value-of select="'Free text'"/>

        </Freetext>

        <CashAccountNumber>
          <xsl:value-of select="'Cash Account Number'"/>
        </CashAccountNumber>

        <CashAccountcurrency>
          <xsl:value-of select="'Cash Account currency'"/>
        </CashAccountcurrency>

        <Shortform>
          <xsl:value-of select="'Short form'"/>
        </Shortform>

        <ClearingAgentCodeType>
          <xsl:value-of select="'Clearing Agent Code Type'"/>
        </ClearingAgentCodeType>


        <ClearingAgentCode>
          <xsl:value-of select="'Clearing Agent Code'"/>
        </ClearingAgentCode>

        <ClearingAgentName>
          <xsl:value-of select="'Clearing Agent Name'"/>
        </ClearingAgentName>

        <Acatdepository>
          <xsl:value-of select="'A/c at depository'"/>
        </Acatdepository>

        <BuyerSellerCodeType>
          <xsl:value-of select="'Buyer/Seller Code Type'"/>
        </BuyerSellerCodeType>


        <BuyerSellercode>
          <xsl:value-of select="'Buyer/Seller code'"/>
        </BuyerSellercode>

        <BuyerSellerName>
          <xsl:value-of select="'Buyer/Seller Name'"/>
        </BuyerSellerName>

        <Acatcustodianclearingagt>
          <xsl:value-of select="'A/c at custodian/clearing agt'"/>
        </Acatcustodianclearingagt>

        <IntermediaryCustodianCodeType>
          <xsl:value-of select="'Intermediary Custodian Code Type'"/>
        </IntermediaryCustodianCodeType>

        <IntermediaryCustodianCode>
          <xsl:value-of select="'Intermediary Custodian Code'"/>
        </IntermediaryCustodianCode>

        <IntermediaryCustodianName>
          <xsl:value-of select="'Intermediary Custodian Name'"/>
        </IntermediaryCustodianName>

        <Acatclearingagent>
          <xsl:value-of select="'A/c at clearing agent'"/>
        </Acatclearingagent>

        <NationalityofInvestor>
          <xsl:value-of select="'Nationality of Investor'"/>
        </NationalityofInvestor>

        <PriceTypeCode>
          <xsl:value-of select="'Price Type Code'"/>
        </PriceTypeCode>

        <Pricecurrency>
          <xsl:value-of select="'Price currency'"/>
        </Pricecurrency>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Dealamountcurrency>
          <xsl:value-of select="'Deal amount currency'"/>
        </Dealamountcurrency>

        <Dealamount>
          <xsl:value-of select="'Deal amount'"/>
        </Dealamount>

        <Brokercommissioncurrency>
          <xsl:value-of select="'Broker commission currency'"/>
        </Brokercommissioncurrency>

        <Brokercommission>
          <xsl:value-of select="'Broker commission'"/>
        </Brokercommission>

        <Taxcurrency>
          <xsl:value-of select="'Tax currency'"/>
        </Taxcurrency>

        <Tax>
          <xsl:value-of select="'Tax'"/>
        </Tax>

        <Stampdutycurrency>
          <xsl:value-of select="'Stamp duty currency'"/>
        </Stampdutycurrency>

        <Stampduty>
          <xsl:value-of select="'Stamp duty'"/>
        </Stampduty>

        <Amortisedamount>
          <xsl:value-of select="'Amortised amount'"/>
        </Amortisedamount>

        <Miscellaneousfeescurrency>
          <xsl:value-of select="'Miscellaneous fees currency'"/>
        </Miscellaneousfeescurrency>

        <Miscellaneousfees>
          <xsl:value-of select="'Miscellaneous fees'"/>
        </Miscellaneousfees>

        <PaymentLevytaxcurrency>
          <xsl:value-of select="'Payment  Levy tax currency'"/>
        </PaymentLevytaxcurrency>

        <Paymentlevytax>
          <xsl:value-of select="'Payment levy tax'"/>
        </Paymentlevytax>

        <Countrytaxcurrency>
          <xsl:value-of select="'Country tax currency'"/>
        </Countrytaxcurrency>

        <Countrytax>
          <xsl:value-of select="'Country tax'"/>

        </Countrytax>

        <Localtaxcurrency>
          <xsl:value-of select="'Local tax currency'"/>
        </Localtaxcurrency>



        <Localtax>
          <xsl:value-of select="'Local tax'"/>

        </Localtax>

        <LocalCountryTaxcurrency>
          <xsl:value-of select="'Local Country Tax currency'"/>

        </LocalCountryTaxcurrency>


        <LocalCountryTax>
          <xsl:value-of select="'Local Country Tax'"/>
        </LocalCountryTax>

        <Localbrokercommissioncurrency>
          <xsl:value-of select="'Local broker commission currency'"/>
        </Localbrokercommissioncurrency>

        <Localbrokercommission>
          <xsl:value-of select="'Local broker commission'"/>
        </Localbrokercommission>


        <Regulatoryamountcurrency>
          <xsl:value-of select="'Regulatory amount currency'"/>
        </Regulatoryamountcurrency>

        <Regulatoryamount>
          <xsl:value-of select="'Regulatory amount'"/>
        </Regulatoryamount>

        <Shippingamountcurrency>
          <xsl:value-of select="'Shipping amount currency'"/>
        </Shippingamountcurrency>

        <Shippingamount>
          <xsl:value-of select="'Shipping amount'"/>
        </Shippingamount>

        <Stockexchangetaxcurrency>
          <xsl:value-of select="'Stock exchange tax currency'"/>
        </Stockexchangetaxcurrency>

        <Stockexchangetax>
          <xsl:value-of select="'Stock exchange tax'"/>
        </Stockexchangetax>

        <Transfertaxcurrency>
          <xsl:value-of select="'Transfer tax currency'"/>
        </Transfertaxcurrency>

        <Transfertax>
          <xsl:value-of select="'Transfer tax'"/>
        </Transfertax>

        <Transactiontaxcurrency>
          <xsl:value-of select="'Transaction tax currency'"/>
        </Transactiontaxcurrency>

        <Transactiontax>
          <xsl:value-of select="'Transaction tax'"/>
        </Transactiontax>

        <WithholdingTaxcurrency>
          <xsl:value-of select="'Withholding Tax currency'"/>
        </WithholdingTaxcurrency>

        <WithholdingTax>
          <xsl:value-of select="'Withholding Tax'"/>
        </WithholdingTax>

        <ConsumptionTaxcurrency>
          <xsl:value-of select="'Consumption Tax currency'"/>
        </ConsumptionTaxcurrency>

        <ConsumptionTax>
          <xsl:value-of select="'Consumption Tax'"/>
        </ConsumptionTax>

        <Accruedinterestcurrency>
          <xsl:value-of select="'Accrued interest currency'"/>
        </Accruedinterestcurrency>

        <Accruedinterest>
          <xsl:value-of select="'Accrued interest'"/>
        </Accruedinterest>

        <Marginamountcurrency>
          <xsl:value-of select="'Margin amount currency'"/>
        </Marginamountcurrency>

        <Marginamount>
          <xsl:value-of select="'Margin amount'"/>
        </Marginamount>

        <NetGainLosscurrency>
          <xsl:value-of select="'Net Gain/Loss currency'"/>
        </NetGainLosscurrency>

        <NetGainLoss>
          <xsl:value-of select="'Net Gain/Loss'"/>
        </NetGainLoss>

        <Accruedcapitalizationcurrency>
          <xsl:value-of select="'Accrued capitalization currency'"/>
        </Accruedcapitalizationcurrency>

        <Accruedcapitalization>
          <xsl:value-of select="'Accrued capitalization'"/>
        </Accruedcapitalization>

        <IssueDisctAllowancecurrency>
          <xsl:value-of select="'Issue Disct/Allowance currency'"/>
        </IssueDisctAllowancecurrency>


        <IssueDisctAllowance>
          <xsl:value-of select="'Issue Disct/Allowance'"/>
        </IssueDisctAllowance>

        <ConcessionAmountcurrency>
          <xsl:value-of select="'Concession Amount currency'"/>
        </ConcessionAmountcurrency>

        <ConcessionAmount>
          <xsl:value-of select="'Concession Amount'"/>
        </ConcessionAmount>

        <ResultingAmtcurrency>
          <xsl:value-of select="'Resulting Amt currency'"/>
        </ResultingAmtcurrency>

        <ResultingAmt>
          <xsl:value-of select="'Resulting Amt'"/>
        </ResultingAmt>


        <ExchangeRate>
          <xsl:value-of select="'Exchange Rate'"/>

        </ExchangeRate>

        <ExchangeRateFirstcurrency>
          <xsl:value-of select="'Exchange Rate First currency'"/>
        </ExchangeRateFirstcurrency>

        <ExchangeRateSecondcurrency>
          <xsl:value-of select="'Exchange Rate Second currency'"/>
        </ExchangeRateSecondcurrency>

        <OriginalOrderedAmtcurrency>
          <xsl:value-of select="'Original/Ordered Amt currency'"/>
        </OriginalOrderedAmtcurrency>


        <OriginalOrderedAmt>
          <xsl:value-of select="'Original/Ordered Amt'"/>
        </OriginalOrderedAmt>


        <OtherAmtcurrency>
          <xsl:value-of select="'Other Amt currency'"/>
        </OtherAmtcurrency>


        <OtherAmt>
          <xsl:value-of select="'Other Amt'"/>
        </OtherAmt>

        <MaturityDate>
          <xsl:value-of select="'Maturity Date'"/>
        </MaturityDate>

        <InterestRate>
          <xsl:value-of select="'Interest Rate'"/>

        </InterestRate>

        <IssueDate>
          <xsl:value-of select="'Issue Date'"/>

        </IssueDate>

        <ExpiryDate>
          <xsl:value-of select="'Expiry Date'"/>
        </ExpiryDate>

        <DatedDate>
          <xsl:value-of select="'Dated Date'"/>
        </DatedDate>

        <CouponDate>
          <xsl:value-of select="'Coupon Date'"/>
        </CouponDate>

        <AccruedIntDays>
          <xsl:value-of select="'Accrued Int Days'"/>

        </AccruedIntDays>

        <AccountWithInstitutionCodeType>
          <xsl:value-of select="'Account With Institution Code Type'"/>
        </AccountWithInstitutionCodeType>

        <AccountWithInstitutionCode >
          <xsl:value-of select="'Account With Institution Code'"/>
        </AccountWithInstitutionCode>

        <AccountWithInstitutionName>
          <xsl:value-of select="'Account With Institution Name'"/>
        </AccountWithInstitutionName>

        <ACCWCashAccount>
          <xsl:value-of select="'ACCW Cash Account'"/>

        </ACCWCashAccount>

        <CashBeneficiaryCodeType>
          <xsl:value-of select="'Cash Beneficiary Code Type'"/>
        </CashBeneficiaryCodeType>

        <CashBeneficiaryCode>
          <xsl:value-of select="'Cash Beneficiary Code '"/>
        </CashBeneficiaryCode>

        <CashBeneficiaryName>
          <xsl:value-of select="'Cash Beneficiary Name'"/>
        </CashBeneficiaryName>

        <BENMCashAccount>
          <xsl:value-of select="'BENM Cash Account'"/>
        </BENMCashAccount>

        <PayeeCodeType>
          <xsl:value-of select="'Payee Code Type'"/>
        </PayeeCodeType>

        <PayeeCode>
          <xsl:value-of select="'Payee Code'"/>
        </PayeeCode>

        <PayeeName>
          <xsl:value-of select="'Payee Name'"/>
        </PayeeName>

        <PAYECashAccount>
          <xsl:value-of select="'PAYE Cash Account'"/>
        </PAYECashAccount>

        <TransactionReference>
          <xsl:value-of select="'Transaction Reference'"/>
        </TransactionReference>


        <SecondLegReference>
          <xsl:value-of select="'Second Leg Reference'"/>
        </SecondLegReference>

        <ClosingDate>
          <xsl:value-of select="'Closing Date'"/>
        </ClosingDate>

        <RepurchaseTypeIndicator>
          <xsl:value-of select="'Repurchase Type Indicator'"/>
        </RepurchaseTypeIndicator>

        <TerminationCCY>
          <xsl:value-of select="'Termination CCY'"/>
        </TerminationCCY>


        <TerminationAmt>
          <xsl:value-of select="'Termination Amt'"/>
        </TerminationAmt>

        <TerminationAmtperPieceofCollateral>
          <xsl:value-of select="'Termination Amt per Piece of Collateral'"/>
        </TerminationAmtperPieceofCollateral>

        <PremiumCCY>
          <xsl:value-of select="'Premium CCY'"/>
        </PremiumCCY>

        <PremiumAmt>
          <xsl:value-of select="'Premium Amt'"/>
        </PremiumAmt>

        <RepurchaseRate>
          <xsl:value-of select="'Repurchase Rate'"/>
        </RepurchaseRate>

        <RateType>
          <xsl:value-of select="'Rate Type'"/>
        </RateType>

        <SpreadRate>
          <xsl:value-of select="'Spread Rate'"/>

        </SpreadRate>

        <CallDelay>
          <xsl:value-of select="'Call Delay'"/>
        </CallDelay>

        <TotalCollateral>
          <xsl:value-of select="'Total Collateral'"/>
        </TotalCollateral>

        <InstructionSequenceNumber>
          <xsl:value-of select="'Instruction Sequence Number'"/>
        </InstructionSequenceNumber>

        <SecondLegNarrative>
          <xsl:value-of select="'Second Leg Narrative'"/>
        </SecondLegNarrative>

        <SecuritiesAlternateId>
          <xsl:value-of select="'Securities Alternate Id'"/>

        </SecuritiesAlternateId>

        <RelatedReference>
          <xsl:value-of select="'Related Reference'"/>
        </RelatedReference>

        <CollateralReference1>
          <xsl:value-of select="'Collateral Reference 1'"/>
        </CollateralReference1>

        <CollateralReference2>
          <xsl:value-of select="'Collateral Reference 2'"/>
        </CollateralReference2>

        <CollateralReference3>
          <xsl:value-of select="'Collateral Reference 3'"/>
        </CollateralReference3>

        <CollateralReference4>
          <xsl:value-of select="'Collateral Reference 4'"/>
        </CollateralReference4>

        <CollateralReference5>
          <xsl:value-of select="'Collateral Reference 5'"/>
        </CollateralReference5>

        <CollateralReference6>
          <xsl:value-of select="'Collateral Reference 6'"/>
        </CollateralReference6>

        <CollateralReference7>
          <xsl:value-of select="'Collateral Reference 7'"/>
        </CollateralReference7>

        <CollateralReference8>
          <xsl:value-of select="'Collateral Reference 8'"/>
        </CollateralReference8>

        <CollateralReference9>
          <xsl:value-of select="'Collateral Reference 9'"/>
        </CollateralReference9>

        <CollateralReference10>
          <xsl:value-of select="'Collateral Reference 10'"/>
        </CollateralReference10>

        <SpecialInstructions1>
          <xsl:value-of select="'Special Instructions 1'"/>
        </SpecialInstructions1>

        <SpecialInstructionsvalue1>
          <xsl:value-of select="'Special Instructions value 1'"/>
        </SpecialInstructionsvalue1>

        <SpecialInstructions2>
          <xsl:value-of select="'Special Instructions 2'"/>
        </SpecialInstructions2>

        <SpecialInstructionsvalue2>
          <xsl:value-of select="'Special Instructions value 2'"/>
        </SpecialInstructionsvalue2>

        <SpecialInstructions3>
          <xsl:value-of select="'Special Instructions 3'"/>
        </SpecialInstructions3>

        <SpecialInstructionsvalue3>
          <xsl:value-of select="'Special Instructions value 3'"/>
        </SpecialInstructionsvalue3>

        <SpecialInstructions4>
          <xsl:value-of select="'Special Instructions 4'"/>
        </SpecialInstructions4>

        <SpecialInstructionsvalue4>
          <xsl:value-of select="'Special Instructions value 4'"/>
        </SpecialInstructionsvalue4>

        <SpecialInstructions5>
          <xsl:value-of select="'Special Instructions 5'"/>

        </SpecialInstructions5>

        <SpecialInstructionsvalue5>
          <xsl:value-of select="'Special Instructions value 5'"/>
        </SpecialInstructionsvalue5>


        <LoanDate>
          <xsl:value-of select="'Loan Date'"/>
        </LoanDate>

        <PledgeeCode>
          <xsl:value-of select="'Pledgee Code'"/>
        </PledgeeCode>

        <ABANumber>
          <xsl:value-of select="'ABA Number'"/>
        </ABANumber>

        <ABADescription>
          <xsl:value-of select="'ABA Description'"/>
        </ABADescription>

        <PledgeInstruction1>
          <xsl:value-of select="'Pledge Instruction 1'"/>
        </PledgeInstruction1>

        <PledgeInstruction2>
          <xsl:value-of select="'Pledge Instruction 2'"/>
        </PledgeInstruction2>

        <SegAccount>
          <xsl:value-of select="'Seg Account'"/>

        </SegAccount>

        <Margin>
          <xsl:value-of select="'Margin'"/>
        </Margin>


        <UnderlyingSecurityIdType>
          <xsl:value-of select="'Underlying Security Id Type'"/>
        </UnderlyingSecurityIdType>

        <UnderlyingSecurityId>
          <xsl:value-of select="'Underlying Security Id'"/>
        </UnderlyingSecurityId>

        <SecurityId3>
          <xsl:value-of select="'Security Id 3'"/>
        </SecurityId3>

        <UnderlyingSecurityDescription>
          <xsl:value-of select="'Underlying Security Description'"/>
        </UnderlyingSecurityDescription>

        <OptionsType>
          <xsl:value-of select="'Options Type'"/>
        </OptionsType>

        <OpenCloseIndicator>
          <xsl:value-of select="'Open /Close Indicator'"/>
        </OpenCloseIndicator>

        <OptionsStyle>
          <xsl:value-of select="'Options Style'"/>
        </OptionsStyle>

        <NumberofContracts>
          <xsl:value-of select="'Number of Contracts'"/>
        </NumberofContracts>

        <ContractSize>
          <xsl:value-of select="'Contract Size'"/>
        </ContractSize>

        <ExercisePriceCurrency>
          <xsl:value-of select="'Exercise Price Currency'"/>
        </ExercisePriceCurrency>

        <ExercisePriceAmount>
          <xsl:value-of select="'Exercise Price Amount'"/>
        </ExercisePriceAmount>

        <OCCMemberCodeType>
          <xsl:value-of select="'OCC Member Code Type'"/>
        </OCCMemberCodeType>

        <OCCMemberIndicator>
          <xsl:value-of select="'OCC Member Indicator'"/>
        </OCCMemberIndicator>

        <OCCMemberCode>
          <xsl:value-of select="'OCC Member Code'"/>
        </OCCMemberCode>

        <OCCMemberName>
          <xsl:value-of select="'OCC Member Name'"/>
        </OCCMemberName>

        <OCCMemberAcatBroker>
          <xsl:value-of select="'OCC Member A/c at Broker'"/>
        </OCCMemberAcatBroker>

        <OCCParticipantCodeType>
          <xsl:value-of select="'OCC Participant Code Type'"/>
        </OCCParticipantCodeType>

        <OCCParticipantCode>
          <xsl:value-of select="'OCC Participant Code'"/>
        </OCCParticipantCode>

        <OCCParticipantName>
          <xsl:value-of select="'OCC Participant Name'"/>
        </OCCParticipantName>

        <OCCParticipantAcatBroker>
          <xsl:value-of select="'OCC Participant A/c at Broker'"/>
        </OCCParticipantAcatBroker>

        <ReasonCode>
          <xsl:value-of select="'Reason Code'"/>
        </ReasonCode>

        <ContractDate>
          <xsl:value-of select="'Contract Date'"/>
        </ContractDate>

        <RecordDate>
          <xsl:value-of select="'Record Date'"/>
        </RecordDate>

        <PayDate>
          <xsl:value-of select="'Pay Date'"/>
        </PayDate>

        <OldPriceCurrency>
          <xsl:value-of select="'Old Price Currency'"/>
        </OldPriceCurrency>

        <OldPriceAmount>
          <xsl:value-of select="'Old Price Amount'"/>
        </OldPriceAmount>

        <NewPriceCurrency>
          <xsl:value-of select="'New Price Currency'"/>
        </NewPriceCurrency>

        <NewPriceAmount>
          <xsl:value-of select="'New Price Amount'"/>
        </NewPriceAmount>


        <BranchCode>
          <xsl:value-of select="'Branch Code'"/>
        </BranchCode>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'Jupiter NZS Glo EQ GR Unconstr SICAV'][CounterParty !='Transfer' and CounterParty !='Corporate action' and CounterParty != 'Cost Adjustment']">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="OrderQty * AvgPrice * AssetMultiplier"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="OrderQty * AvgPrice * AssetMultiplier"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'False'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <xsl:variable name ="varAllocationState">
    
                <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="'New'"/>
                  </xsl:when>
				  
			      	   <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of  select="'Cancel'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of  select="'SENT'"/>
                  </xsl:otherwise>
                </xsl:choose>
          </xsl:variable>

              <TypeOfRequest>
                <xsl:value-of select="$varAllocationState"/>
              </TypeOfRequest>

              <OriginalCustomerReferenceNumber>
                      <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="''"/>
                  </xsl:when>
				  
			      	   <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of  select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
          </OriginalCustomerReferenceNumber>

              <PreviousReferenceNumber>
                <xsl:value-of select="''"/>
              </PreviousReferenceNumber>



             <SafekeepingAccount>
            <xsl:value-of select="FundAccntNo"/>
          </SafekeepingAccount>

              <CustomerReferenceNumber>
             <xsl:choose>
				<xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="concat(PBUniqueID,position())"/>
                  </xsl:when>
                  <!-- <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:when> -->		  
			      	   

                  <xsl:otherwise>
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:otherwise>
                </xsl:choose>
          </CustomerReferenceNumber>

              <InstructionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>

                </xsl:choose>
              </InstructionType>

              <TransactionType>
                <xsl:value-of select="'Standard'"/>
              </TransactionType>

              <SecurityIdType>
                <xsl:value-of select="'ISIN'"/>
              </SecurityIdType>

              <SecurityId>
                <xsl:value-of select="ISIN"/>
              </SecurityId>

              <SecurityId2>
                <xsl:value-of select="SEDOL"/>
              </SecurityId2>


              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <ReceiveAccount>
                <xsl:value-of select="''"/>
              </ReceiveAccount>


             <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate">
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <TradeDate>
            <!-- <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/> -->
			
			<xsl:value-of select="$varTradeDate"/>
          </TradeDate>

          <xsl:variable name="varSettlementDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate">
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <SettlementDate>
            <xsl:value-of select="$varSettlementDate"/>
          </SettlementDate>

              <xsl:variable name="PB_NAME">
            <xsl:value-of select="'CITI'"/>
          </xsl:variable>
          
		   <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
              </xsl:variable>
			  
			  <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
              </xsl:variable>
			  
			  <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
              </xsl:variable>

          <SettlementLocation>
            <xsl:choose>
		   
		          <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'DE'"/>
              </xsl:when>
			       <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'NL'"/>
              </xsl:when>
             
			 
              <xsl:otherwise>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementLocation>
              <PlaceofSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceofSafekeeping>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>


              <SettlementQuantity>
                <xsl:value-of select="OrderQty"/>
              </SettlementQuantity>
			  
			  <xsl:variable name="varSetNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

              <SettlementAmount>
                <xsl:value-of select="format-number($varSetNetamount,'#.##')"/>
              </SettlementAmount>

              <Freetext>
                <xsl:value-of select="''"/>

              </Freetext>

              <CashAccountNumber>
                <xsl:value-of select="''"/>
              </CashAccountNumber>

              <CashAccountcurrency>
                <xsl:value-of select="''"/>
              </CashAccountcurrency>

              <Shortform>
                <xsl:value-of select="''"/>
              </Shortform>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
							</xsl:variable>
          <ClearingAgentCodeType>
            <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
          </ClearingAgentCodeType>



            
              <xsl:variable name="PRANA_COUNTERPARTY">
                <xsl:value-of select="CounterParty"/>
              </xsl:variable>

              <xsl:variable name="PB_COUNTERPARTY">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name = $PB_NAME]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@DTCCode"/>
              </xsl:variable>

              <xsl:variable name="varDTCCode">
                <xsl:choose>
                  <xsl:when test="$PB_COUNTERPARTY = ''">
                    <xsl:value-of select="$PRANA_COUNTERPARTY"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_COUNTERPARTY"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>
          <ClearingAgentCode>
            <xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'PARBDEFFXXX'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ClearingBrokerBIC != ' '">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClearingAgentCode>

              <ClearingAgentName>
                <xsl:value-of select="''"/>
              </ClearingAgentName>

              <Acatdepository>
                <xsl:value-of select="''"/>
              </Acatdepository>

              <BuyerSellerCodeType>
            <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'LOCAL'"/>
              </xsl:when>
              			 
              <xsl:otherwise>
                <xsl:value-of select="'BIC'"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuyerSellerCodeType>


              <BuyerSellercode>
            <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>            
          </BuyerSellercode>

              <BuyerSellerName>
                <xsl:value-of select="CounterParty"/>
              </BuyerSellerName>

              <Acatcustodianclearingagt>
            <xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'6547070004'"/>
              </xsl:when>
			  <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'ESES 611'"/>
              </xsl:when>
             
			 
              <xsl:otherwise>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerLocalCode"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Acatcustodianclearingagt>

              <IntermediaryCustodianCodeType>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianCodeType>

              <IntermediaryCustodianCode>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianCode>

              <IntermediaryCustodianName>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianName>

              <Acatclearingagent>
                <xsl:value-of select="''"/>
              </Acatclearingagent>

              <NationalityofInvestor>
                <xsl:value-of select="''"/>
              </NationalityofInvestor>

              <PriceTypeCode>
                <xsl:value-of select="''"/>
              </PriceTypeCode>

              <Pricecurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Pricecurrency>

              <Price>
			   <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                            </Price>

              <Dealamountcurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Dealamountcurrency>

              <Dealamount>
			  <xsl:value-of select="format-number($varNetamount,'#.##')"/>
            
              </Dealamount>

              <Brokercommissioncurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Brokercommissioncurrency>

              <Brokercommission>
               <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </Brokercommission>

              <Taxcurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Taxcurrency>

              <Tax>
                <xsl:value-of select="OtherBrokerFees"/>
              </Tax>

              <Stampdutycurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Stampdutycurrency>

              <Stampduty>
                <xsl:value-of select="StampDuty"/>
              </Stampduty>

              <Amortisedamount>
                <xsl:value-of select="''"/>
              </Amortisedamount>

              <Miscellaneousfeescurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Miscellaneousfeescurrency>

              <Miscellaneousfees>
			  <xsl:value-of select="format-number((ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee  ),'0.######')"/>
                <!-- <xsl:value-of select="format-number(SecFee +TaxOnCommissions,'#.######')"/> -->
              </Miscellaneousfees>

              <PaymentLevytaxcurrency>
                <xsl:value-of select="''"/>
              </PaymentLevytaxcurrency>

              <Paymentlevytax>
                <xsl:value-of select="''"/>
              </Paymentlevytax>

              <Countrytaxcurrency>
                <xsl:value-of select="''"/>
              </Countrytaxcurrency>

              <Countrytax>
                <xsl:value-of select="''"/>

              </Countrytax>

              <Localtaxcurrency>
                <xsl:value-of select="''"/>
              </Localtaxcurrency>



              <Localtax>
                <xsl:value-of select="''"/>

              </Localtax>

              <LocalCountryTaxcurrency>
                <xsl:value-of select="''"/>

              </LocalCountryTaxcurrency>


              <LocalCountryTax>
                <xsl:value-of select="''"/>
              </LocalCountryTax>

              <Localbrokercommissioncurrency>
                <xsl:value-of select="''"/>
              </Localbrokercommissioncurrency>

              <Localbrokercommission>
                <xsl:value-of select="''"/>
              </Localbrokercommission>


              <Regulatoryamountcurrency>
                <xsl:value-of select="''"/>
              </Regulatoryamountcurrency>

              <Regulatoryamount>
                <xsl:value-of select="''"/>
              </Regulatoryamount>

              <Shippingamountcurrency>
                <xsl:value-of select="''"/>
              </Shippingamountcurrency>

              <Shippingamount>
                <xsl:value-of select="''"/>
              </Shippingamount>

              <Stockexchangetaxcurrency>
                <xsl:value-of select="''"/>
              </Stockexchangetaxcurrency>

              <Stockexchangetax>
                <xsl:value-of select="''"/>
              </Stockexchangetax>

              <Transfertaxcurrency>
                <xsl:value-of select="''"/>
              </Transfertaxcurrency>

              <Transfertax>
                <xsl:value-of select="''"/>
              </Transfertax>

              <Transactiontaxcurrency>
                <xsl:value-of select="''"/>
              </Transactiontaxcurrency>

              <Transactiontax>
                <xsl:value-of select="''"/>
              </Transactiontax>

              <WithholdingTaxcurrency>
                <xsl:value-of select="''"/>
              </WithholdingTaxcurrency>

              <WithholdingTax>
                <xsl:value-of select="''"/>
              </WithholdingTax>

              <ConsumptionTaxcurrency>
                <xsl:value-of select="''"/>
              </ConsumptionTaxcurrency>

              <ConsumptionTax>
                <xsl:value-of select="''"/>
              </ConsumptionTax>

              <Accruedinterestcurrency>
                <xsl:value-of select="''"/>
              </Accruedinterestcurrency>

              <Accruedinterest>
                <xsl:value-of select="''"/>
              </Accruedinterest>

              <Marginamountcurrency>
                <xsl:value-of select="''"/>
              </Marginamountcurrency>

              <Marginamount>
                <xsl:value-of select="''"/>
              </Marginamount>

              <NetGainLosscurrency>
                <xsl:value-of select="''"/>
              </NetGainLosscurrency>

              <NetGainLoss>
                <xsl:value-of select="''"/>
              </NetGainLoss>

              <Accruedcapitalizationcurrency>
                <xsl:value-of select="''"/>
              </Accruedcapitalizationcurrency>

              <Accruedcapitalization>
                <xsl:value-of select="''"/>
              </Accruedcapitalization>

              <IssueDisctAllowancecurrency>
                <xsl:value-of select="''"/>
              </IssueDisctAllowancecurrency>


              <IssueDisctAllowance>
                <xsl:value-of select="''"/>
              </IssueDisctAllowance>

              <ConcessionAmountcurrency>
                <xsl:value-of select="''"/>
              </ConcessionAmountcurrency>

              <ConcessionAmount>
                <xsl:value-of select="''"/>
              </ConcessionAmount>

              <ResultingAmtcurrency>
                <xsl:value-of select="''"/>
              </ResultingAmtcurrency>

              <ResultingAmt>
                <xsl:value-of select="''"/>
              </ResultingAmt>


              <ExchangeRate>
                <xsl:value-of select="''"/>

              </ExchangeRate>

              <ExchangeRateFirstcurrency>
                <xsl:value-of select="''"/>
              </ExchangeRateFirstcurrency>

              <ExchangeRateSecondcurrency>
                <xsl:value-of select="''"/>
              </ExchangeRateSecondcurrency>

              <OriginalOrderedAmtcurrency>
                <xsl:value-of select="''"/>
              </OriginalOrderedAmtcurrency>


              <OriginalOrderedAmt>
                <xsl:value-of select="''"/>
              </OriginalOrderedAmt>


              <OtherAmtcurrency>
                <xsl:value-of select="''"/>
              </OtherAmtcurrency>


              <OtherAmt>
                <xsl:value-of select="''"/>
              </OtherAmt>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <InterestRate>
                <xsl:value-of select="''"/>

              </InterestRate>

              <IssueDate>
                <xsl:value-of select="''"/>

              </IssueDate>

              <ExpiryDate>
                <xsl:value-of select="''"/>
              </ExpiryDate>

              <DatedDate>
                <xsl:value-of select="''"/>
              </DatedDate>

              <CouponDate>
                <xsl:value-of select="''"/>
              </CouponDate>

              <AccruedIntDays>
                <xsl:value-of select="''"/>

              </AccruedIntDays>

              <AccountWithInstitutionCodeType>
                <xsl:value-of select="''"/>
              </AccountWithInstitutionCodeType>

              <AccountWithInstitutionCode >
                <xsl:value-of select="''"/>
              </AccountWithInstitutionCode>

              <AccountWithInstitutionName>
                <xsl:value-of select="''"/>
              </AccountWithInstitutionName>

              <ACCWCashAccount>
                <xsl:value-of select="''"/>

              </ACCWCashAccount>

              <CashBeneficiaryCodeType>
                <xsl:value-of select="''"/>
              </CashBeneficiaryCodeType>

              <CashBeneficiaryCode>
                <xsl:value-of select="''"/>
              </CashBeneficiaryCode>

              <CashBeneficiaryName>
                <xsl:value-of select="''"/>
              </CashBeneficiaryName>

              <BENMCashAccount>
                <xsl:value-of select="''"/>
              </BENMCashAccount>

              <PayeeCodeType>
                <xsl:value-of select="''"/>
              </PayeeCodeType>

              <PayeeCode>
                <xsl:value-of select="''"/>
              </PayeeCode>

              <PayeeName>
                <xsl:value-of select="''"/>
              </PayeeName>

              <PAYECashAccount>
                <xsl:value-of select="''"/>
              </PAYECashAccount>

              <TransactionReference>
                <xsl:value-of select="''"/>
              </TransactionReference>


              <SecondLegReference>
                <xsl:value-of select="''"/>
              </SecondLegReference>

              <ClosingDate>
                <xsl:value-of select="''"/>
              </ClosingDate>

              <RepurchaseTypeIndicator>
                <xsl:value-of select="''"/>
              </RepurchaseTypeIndicator>

              <TerminationCCY>
                <xsl:value-of select="''"/>
              </TerminationCCY>


              <TerminationAmt>
                <xsl:value-of select="''"/>
              </TerminationAmt>

              <TerminationAmtperPieceofCollateral>
                <xsl:value-of select="''"/>
              </TerminationAmtperPieceofCollateral>

              <PremiumCCY>
                <xsl:value-of select="''"/>
              </PremiumCCY>

              <PremiumAmt>
                <xsl:value-of select="''"/>
              </PremiumAmt>

              <RepurchaseRate>
                <xsl:value-of select="''"/>
              </RepurchaseRate>

              <RateType>
                <xsl:value-of select="''"/>
              </RateType>

              <SpreadRate>
                <xsl:value-of select="''"/>

              </SpreadRate>

              <CallDelay>
                <xsl:value-of select="''"/>
              </CallDelay>

              <TotalCollateral>
                <xsl:value-of select="''"/>
              </TotalCollateral>

              <InstructionSequenceNumber>
                <xsl:value-of select="''"/>
              </InstructionSequenceNumber>

              <SecondLegNarrative>
                <xsl:value-of select="''"/>
              </SecondLegNarrative>

              <SecuritiesAlternateId>
                <xsl:value-of select="''"/>

              </SecuritiesAlternateId>

              <RelatedReference>
                <xsl:value-of select="''"/>
              </RelatedReference>

              <CollateralReference1>
                <xsl:value-of select="''"/>
              </CollateralReference1>

              <CollateralReference2>
                <xsl:value-of select="''"/>
              </CollateralReference2>

              <CollateralReference3>
                <xsl:value-of select="''"/>
              </CollateralReference3>

              <CollateralReference4>
                <xsl:value-of select="''"/>
              </CollateralReference4>

              <CollateralReference5>
                <xsl:value-of select="''"/>
              </CollateralReference5>

              <CollateralReference6>
                <xsl:value-of select="''"/>
              </CollateralReference6>

              <CollateralReference7>
                <xsl:value-of select="''"/>
              </CollateralReference7>

              <CollateralReference8>
                <xsl:value-of select="''"/>
              </CollateralReference8>

              <CollateralReference9>
                <xsl:value-of select="''"/>
              </CollateralReference9>

              <CollateralReference10>
                <xsl:value-of select="''"/>
              </CollateralReference10>

              <SpecialInstructions1>
                <xsl:value-of select="''"/>
              </SpecialInstructions1>

              <SpecialInstructionsvalue1>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue1>

              <SpecialInstructions2>
                <xsl:value-of select="''"/>
              </SpecialInstructions2>

              <SpecialInstructionsvalue2>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue2>

              <SpecialInstructions3>
                <xsl:value-of select="''"/>
              </SpecialInstructions3>

              <SpecialInstructionsvalue3>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue3>

              <SpecialInstructions4>
                <xsl:value-of select="''"/>
              </SpecialInstructions4>

              <SpecialInstructionsvalue4>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue4>

              <SpecialInstructions5>
                <xsl:value-of select="''"/>

              </SpecialInstructions5>

              <SpecialInstructionsvalue5>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue5>


              <LoanDate>
                <xsl:value-of select="''"/>
              </LoanDate>

              <PledgeeCode>
                <xsl:value-of select="''"/>
              </PledgeeCode>

              <ABANumber>
                <xsl:value-of select="''"/>
              </ABANumber>

              <ABADescription>
                <xsl:value-of select="''"/>
              </ABADescription>

              <PledgeInstruction1>
                <xsl:value-of select="''"/>
              </PledgeInstruction1>

              <PledgeInstruction2>
                <xsl:value-of select="''"/>
              </PledgeInstruction2>

              <SegAccount>
                <xsl:value-of select="''"/>

              </SegAccount>

              <Margin>
                <xsl:value-of select="''"/>
              </Margin>


              <UnderlyingSecurityIdType>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityIdType>

              <UnderlyingSecurityId>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityId>

              <SecurityId3>
                <xsl:value-of select="SEDOL"/>
              </SecurityId3>

              <UnderlyingSecurityDescription>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityDescription>

              <OptionsType>
                <xsl:value-of select="''"/>
              </OptionsType>

              <OpenCloseIndicator>
                <xsl:value-of select="''"/>
              </OpenCloseIndicator>

              <OptionsStyle>
                <xsl:value-of select="''"/>
              </OptionsStyle>

              <NumberofContracts>
                <xsl:value-of select="''"/>
              </NumberofContracts>

              <ContractSize>
                <xsl:value-of select="''"/>
              </ContractSize>

              <ExercisePriceCurrency>
                <xsl:value-of select="''"/>
              </ExercisePriceCurrency>

              <ExercisePriceAmount>
                <xsl:value-of select="''"/>
              </ExercisePriceAmount>

              <OCCMemberCodeType>
                <xsl:value-of select="''"/>
              </OCCMemberCodeType>

              <OCCMemberIndicator>
                <xsl:value-of select="''"/>
              </OCCMemberIndicator>

              <OCCMemberCode>
                <xsl:value-of select="''"/>
              </OCCMemberCode>

              <OCCMemberName>
                <xsl:value-of select="''"/>
              </OCCMemberName>

              <OCCMemberAcatBroker>
                <xsl:value-of select="''"/>
              </OCCMemberAcatBroker>

              <OCCParticipantCodeType>
                <xsl:value-of select="''"/>

              </OCCParticipantCodeType>

              <OCCParticipantCode>
                <xsl:value-of select="''"/>
              </OCCParticipantCode>

              <OCCParticipantName>
                <xsl:value-of select="''"/>
              </OCCParticipantName>

              <OCCParticipantAcatBroker>
                <xsl:value-of select="''"/>
              </OCCParticipantAcatBroker>

              <ReasonCode>
                <xsl:value-of select="''"/>
              </ReasonCode>

              <ContractDate>
                <xsl:value-of select="''"/>
              </ContractDate>

              <RecordDate>
                <xsl:value-of select="''"/>
              </RecordDate>

              <PayDate>
                <xsl:value-of select="''"/>
              </PayDate>

              <OldPriceCurrency>
                <xsl:value-of select="''"/>
              </OldPriceCurrency>

              <OldPriceAmount>
                <xsl:value-of select="''"/>
              </OldPriceAmount>

              <NewPriceCurrency>
                <xsl:value-of select="''"/>
              </NewPriceCurrency>

              <NewPriceAmount>
                <xsl:value-of select="''"/>
              </NewPriceAmount>


              <BranchCode>
                <xsl:value-of select="''"/>
              </BranchCode>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

                <RowHeader>
                  <xsl:value-of select ="'False'"/>
                </RowHeader>

                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>

                <xsl:variable name ="varAllocationState">

                  <xsl:choose>
                    <xsl:when test ="TaxLotState = 'Allocated'">
                      <xsl:value-of  select="'New'"/>
                    </xsl:when>

                    <xsl:when test ="TaxLotState = 'Deleted'">
                      <xsl:value-of  select="'Cancel'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of  select="'SENT'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <TypeOfRequest>
                  <xsl:value-of select="'Cancel'"/>
                </TypeOfRequest>

                <OriginalCustomerReferenceNumber>
                  <xsl:value-of select="PBUniqueID"/>
                </OriginalCustomerReferenceNumber>

                <PreviousReferenceNumber>
                  <xsl:value-of select="''"/>
                </PreviousReferenceNumber>



                <SafekeepingAccount>
                  <xsl:value-of select="FundAccntNo"/>
                </SafekeepingAccount>

                <CustomerReferenceNumber>
                  <xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/>
                </CustomerReferenceNumber>

                <InstructionType>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy'">
                      <xsl:value-of select="'RVP'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell'">
                      <xsl:value-of select="'DVP'"/>
                    </xsl:when>

                  </xsl:choose>
                </InstructionType>

                <TransactionType>
                  <xsl:value-of select="'Standard'"/>
                </TransactionType>

                <SecurityIdType>
                  <xsl:value-of select="'ISIN'"/>
                </SecurityIdType>

                <SecurityId>
                  <xsl:value-of select="ISIN"/>
                </SecurityId>
<SecurityId2>
            <xsl:value-of select="SEDOL"/>
          </SecurityId2>

                <SecurityDescription>
                  <xsl:value-of select="CompanyName"/>
                </SecurityDescription>

                <ReceiveAccount>
                  <xsl:value-of select="''"/>
                </ReceiveAccount>


                <xsl:variable name="varTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <TradeDate>
                 <xsl:value-of select="$varTradeDate"/>
                </TradeDate>

                <xsl:variable name="varSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettlementDate>
                  <xsl:value-of select="$varSettlementDate"/>
                </SettlementDate>

                <xsl:variable name="PB_NAME">
                  <xsl:value-of select="'CITI'"/>
                </xsl:variable>
                <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
                </xsl:variable>

                <SettlementLocation>
                  <xsl:choose>

                    <xsl:when test="Exchange ='FRA'">
                      <xsl:value-of select="'DE'"/>
                    </xsl:when>
                    <xsl:when test="Exchange ='Euronext'">
                      <xsl:value-of select="'NL'"/>
                    </xsl:when>


                    <xsl:otherwise>
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementLocation>

                <PlaceofSafekeeping>
                  <xsl:value-of select="''"/>
                </PlaceofSafekeeping>

                <SettlementCurrency>
                  <xsl:value-of select="OldSettlCurrency"/>
                </SettlementCurrency>


                <SettlementQuantity>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </SettlementQuantity>

                <xsl:variable name="varOldsetNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>

                <SettlementAmount>
                
				   <xsl:value-of select="format-number($varOldsetNetAmount,'#.##')"/>
                </SettlementAmount>

                <Freetext>
                  <xsl:value-of select="''"/>

                </Freetext>

                <CashAccountNumber>
                  <xsl:value-of select="''"/>
                </CashAccountNumber>

                <CashAccountcurrency>
                  <xsl:value-of select="''"/>
                </CashAccountcurrency>

                <Shortform>
                  <xsl:value-of select="''"/>
                </Shortform>
                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
                </xsl:variable>
                <ClearingAgentCodeType>
                  <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                </ClearingAgentCodeType>




                <xsl:variable name="PRANA_COUNTERPARTY">
                  <xsl:value-of select="CounterParty"/>
                </xsl:variable>

                <xsl:variable name="PB_COUNTERPARTY">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name = $PB_NAME]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@DTCCode"/>
                </xsl:variable>

                <xsl:variable name="varDTCCode">
                  <xsl:choose>
                    <xsl:when test="$PB_COUNTERPARTY = ''">
                      <xsl:value-of select="$PRANA_COUNTERPARTY"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PB_COUNTERPARTY"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
                </xsl:variable>
                <ClearingAgentCode>
                  <xsl:choose>

                    <xsl:when test="Exchange ='FRA'">
                      <xsl:value-of select="'PARBDEFFXXX'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ClearingBrokerBIC != ' '">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ClearingAgentCode>

                <ClearingAgentName>
                  <xsl:value-of select="''"/>
                </ClearingAgentName>

                <Acatdepository>
                  <xsl:value-of select="''"/>
                </Acatdepository>

                <BuyerSellerCodeType>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'LOCAL'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="'BIC'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </BuyerSellerCodeType>


                <BuyerSellercode>
                  <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
                </BuyerSellercode>

                <BuyerSellerName>
                  <xsl:value-of select="CounterParty"/>
                </BuyerSellerName>

                <Acatcustodianclearingagt>
                 <xsl:value-of select="''"/>    
                </Acatcustodianclearingagt>

                <IntermediaryCustodianCodeType>
                  <xsl:value-of select="''"/>
                </IntermediaryCustodianCodeType>

                <IntermediaryCustodianCode>
                  <xsl:value-of select="''"/>
                </IntermediaryCustodianCode>

                <IntermediaryCustodianName>
                  <xsl:value-of select="''"/>
                </IntermediaryCustodianName>

                <Acatclearingagent>
                  <xsl:value-of select="''"/>
                </Acatclearingagent>

                <NationalityofInvestor>
                  <xsl:value-of select="''"/>
                </NationalityofInvestor>

                <PriceTypeCode>
                  <xsl:value-of select="''"/>
                </PriceTypeCode>

                <Pricecurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Pricecurrency>

                <Price>
				  <xsl:value-of select="format-number(OldAvgPrice,'#.######')"/>
                  
                </Price>

                <Dealamountcurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Dealamountcurrency>

				<xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <Dealamount>
				 <xsl:value-of select="format-number($varOldNetAmount,'#.##')"/>
                 
                </Dealamount>

                <Brokercommissioncurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Brokercommissioncurrency>

                <Brokercommission>
                 <xsl:value-of select="OldCommission + OldSoftCommission"/>
                </Brokercommission>

                <Taxcurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Taxcurrency>

                <Tax>
                  <xsl:value-of select="OldOtherBrokerFees"/>
                </Tax>

                <Stampdutycurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Stampdutycurrency>

                <Stampduty>
                  <xsl:value-of select="OldStampDuty"/>
                </Stampduty>

                <Amortisedamount>
                  <xsl:value-of select="''"/>
                </Amortisedamount>

                <Miscellaneousfeescurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </Miscellaneousfeescurrency>

                <Miscellaneousfees>
				<xsl:value-of select="format-number((OldClearingBrokerFee + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee  ),'0.######')"/>
<!-- 				 <xsl:value-of select="format-number(OldSecFee +OldTaxOnCommissions,'#.######')"/> -->
				              <!--   <xsl:value-of select="OldSecFee +OldTaxOnCommissions"/> -->
                  <!-- <xsl:value-of select="OldSecFee"/> -->
                </Miscellaneousfees>

                <PaymentLevytaxcurrency>
                  <xsl:value-of select="''"/>
                </PaymentLevytaxcurrency>

                <Paymentlevytax>
                  <xsl:value-of select="''"/>
                </Paymentlevytax>

                <Countrytaxcurrency>
                  <xsl:value-of select="''"/>
                </Countrytaxcurrency>

                <Countrytax>
                  <xsl:value-of select="''"/>

                </Countrytax>

                <Localtaxcurrency>
                  <xsl:value-of select="''"/>
                </Localtaxcurrency>



                <Localtax>
                  <xsl:value-of select="''"/>

                </Localtax>

                <LocalCountryTaxcurrency>
                  <xsl:value-of select="''"/>

                </LocalCountryTaxcurrency>


                <LocalCountryTax>
                  <xsl:value-of select="''"/>
                </LocalCountryTax>

                <Localbrokercommissioncurrency>
                  <xsl:value-of select="''"/>
                </Localbrokercommissioncurrency>

                <Localbrokercommission>
                  <xsl:value-of select="''"/>
                </Localbrokercommission>


                <Regulatoryamountcurrency>
                  <xsl:value-of select="''"/>
                </Regulatoryamountcurrency>

                <Regulatoryamount>
                  <xsl:value-of select="''"/>
                </Regulatoryamount>

                <Shippingamountcurrency>
                  <xsl:value-of select="''"/>
                </Shippingamountcurrency>

                <Shippingamount>
                  <xsl:value-of select="''"/>
                </Shippingamount>

                <Stockexchangetaxcurrency>
                  <xsl:value-of select="''"/>
                </Stockexchangetaxcurrency>

                <Stockexchangetax>
                  <xsl:value-of select="''"/>
                </Stockexchangetax>

                <Transfertaxcurrency>
                  <xsl:value-of select="''"/>
                </Transfertaxcurrency>

                <Transfertax>
                  <xsl:value-of select="''"/>
                </Transfertax>

                <Transactiontaxcurrency>
                  <xsl:value-of select="''"/>
                </Transactiontaxcurrency>

                <Transactiontax>
                  <xsl:value-of select="''"/>
                </Transactiontax>

                <WithholdingTaxcurrency>
                  <xsl:value-of select="''"/>
                </WithholdingTaxcurrency>

                <WithholdingTax>
                  <xsl:value-of select="''"/>
                </WithholdingTax>

                <ConsumptionTaxcurrency>
                  <xsl:value-of select="''"/>
                </ConsumptionTaxcurrency>

                <ConsumptionTax>
                  <xsl:value-of select="''"/>
                </ConsumptionTax>

                <Accruedinterestcurrency>
                  <xsl:value-of select="''"/>
                </Accruedinterestcurrency>

                <Accruedinterest>
                  <xsl:value-of select="''"/>
                </Accruedinterest>

                <Marginamountcurrency>
                  <xsl:value-of select="''"/>
                </Marginamountcurrency>

                <Marginamount>
                  <xsl:value-of select="''"/>
                </Marginamount>

                <NetGainLosscurrency>
                  <xsl:value-of select="''"/>
                </NetGainLosscurrency>

                <NetGainLoss>
                  <xsl:value-of select="''"/>
                </NetGainLoss>

                <Accruedcapitalizationcurrency>
                  <xsl:value-of select="''"/>
                </Accruedcapitalizationcurrency>

                <Accruedcapitalization>
                  <xsl:value-of select="''"/>
                </Accruedcapitalization>

                <IssueDisctAllowancecurrency>
                  <xsl:value-of select="''"/>
                </IssueDisctAllowancecurrency>


                <IssueDisctAllowance>
                  <xsl:value-of select="''"/>
                </IssueDisctAllowance>

                <ConcessionAmountcurrency>
                  <xsl:value-of select="''"/>
                </ConcessionAmountcurrency>

                <ConcessionAmount>
                  <xsl:value-of select="''"/>
                </ConcessionAmount>

                <ResultingAmtcurrency>
                  <xsl:value-of select="''"/>
                </ResultingAmtcurrency>

                <ResultingAmt>
                  <xsl:value-of select="''"/>
                </ResultingAmt>


                <ExchangeRate>
                  <xsl:value-of select="''"/>

                </ExchangeRate>

                <ExchangeRateFirstcurrency>
                  <xsl:value-of select="''"/>
                </ExchangeRateFirstcurrency>

                <ExchangeRateSecondcurrency>
                  <xsl:value-of select="''"/>
                </ExchangeRateSecondcurrency>

                <OriginalOrderedAmtcurrency>
                  <xsl:value-of select="''"/>
                </OriginalOrderedAmtcurrency>


                <OriginalOrderedAmt>
                  <xsl:value-of select="''"/>
                </OriginalOrderedAmt>


                <OtherAmtcurrency>
                  <xsl:value-of select="''"/>
                </OtherAmtcurrency>


                <OtherAmt>
                  <xsl:value-of select="''"/>
                </OtherAmt>

                <MaturityDate>
                  <xsl:value-of select="''"/>
                </MaturityDate>

                <InterestRate>
                  <xsl:value-of select="''"/>

                </InterestRate>

                <IssueDate>
                  <xsl:value-of select="''"/>

                </IssueDate>

                <ExpiryDate>
                  <xsl:value-of select="''"/>
                </ExpiryDate>

                <DatedDate>
                  <xsl:value-of select="''"/>
                </DatedDate>

                <CouponDate>
                  <xsl:value-of select="''"/>
                </CouponDate>

                <AccruedIntDays>
                  <xsl:value-of select="''"/>

                </AccruedIntDays>

                <AccountWithInstitutionCodeType>
                  <xsl:value-of select="''"/>
                </AccountWithInstitutionCodeType>

                <AccountWithInstitutionCode >
                  <xsl:value-of select="''"/>
                </AccountWithInstitutionCode>

                <AccountWithInstitutionName>
                  <xsl:value-of select="''"/>
                </AccountWithInstitutionName>

                <ACCWCashAccount>
                  <xsl:value-of select="''"/>

                </ACCWCashAccount>

                <CashBeneficiaryCodeType>
                  <xsl:value-of select="''"/>
                </CashBeneficiaryCodeType>

                <CashBeneficiaryCode>
                  <xsl:value-of select="''"/>
                </CashBeneficiaryCode>

                <CashBeneficiaryName>
                  <xsl:value-of select="''"/>
                </CashBeneficiaryName>

                <BENMCashAccount>
                  <xsl:value-of select="''"/>
                </BENMCashAccount>

                <PayeeCodeType>
                  <xsl:value-of select="''"/>
                </PayeeCodeType>

                <PayeeCode>
                  <xsl:value-of select="''"/>
                </PayeeCode>

                <PayeeName>
                  <xsl:value-of select="''"/>
                </PayeeName>

                <PAYECashAccount>
                  <xsl:value-of select="''"/>
                </PAYECashAccount>

                <TransactionReference>
                  <xsl:value-of select="''"/>
                </TransactionReference>


                <SecondLegReference>
                  <xsl:value-of select="''"/>
                </SecondLegReference>

                <ClosingDate>
                  <xsl:value-of select="''"/>
                </ClosingDate>

                <RepurchaseTypeIndicator>
                  <xsl:value-of select="''"/>
                </RepurchaseTypeIndicator>

                <TerminationCCY>
                  <xsl:value-of select="''"/>
                </TerminationCCY>


                <TerminationAmt>
                  <xsl:value-of select="''"/>
                </TerminationAmt>

                <TerminationAmtperPieceofCollateral>
                  <xsl:value-of select="''"/>
                </TerminationAmtperPieceofCollateral>

                <PremiumCCY>
                  <xsl:value-of select="''"/>
                </PremiumCCY>

                <PremiumAmt>
                  <xsl:value-of select="''"/>
                </PremiumAmt>

                <RepurchaseRate>
                  <xsl:value-of select="''"/>
                </RepurchaseRate>

                <RateType>
                  <xsl:value-of select="''"/>
                </RateType>

                <SpreadRate>
                  <xsl:value-of select="''"/>

                </SpreadRate>

                <CallDelay>
                  <xsl:value-of select="''"/>
                </CallDelay>

                <TotalCollateral>
                  <xsl:value-of select="''"/>
                </TotalCollateral>

                <InstructionSequenceNumber>
                  <xsl:value-of select="''"/>
                </InstructionSequenceNumber>

                <SecondLegNarrative>
                  <xsl:value-of select="''"/>
                </SecondLegNarrative>

                <SecuritiesAlternateId>
                  <xsl:value-of select="''"/>

                </SecuritiesAlternateId>

                <RelatedReference>
                  <xsl:value-of select="''"/>
                </RelatedReference>

                <CollateralReference1>
                  <xsl:value-of select="''"/>
                </CollateralReference1>

                <CollateralReference2>
                  <xsl:value-of select="''"/>
                </CollateralReference2>

                <CollateralReference3>
                  <xsl:value-of select="''"/>
                </CollateralReference3>

                <CollateralReference4>
                  <xsl:value-of select="''"/>
                </CollateralReference4>

                <CollateralReference5>
                  <xsl:value-of select="''"/>
                </CollateralReference5>

                <CollateralReference6>
                  <xsl:value-of select="''"/>
                </CollateralReference6>

                <CollateralReference7>
                  <xsl:value-of select="''"/>
                </CollateralReference7>

                <CollateralReference8>
                  <xsl:value-of select="''"/>
                </CollateralReference8>

                <CollateralReference9>
                  <xsl:value-of select="''"/>
                </CollateralReference9>

                <CollateralReference10>
                  <xsl:value-of select="''"/>
                </CollateralReference10>

                <SpecialInstructions1>
                  <xsl:value-of select="''"/>
                </SpecialInstructions1>

                <SpecialInstructionsvalue1>
                  <xsl:value-of select="''"/>
                </SpecialInstructionsvalue1>

                <SpecialInstructions2>
                  <xsl:value-of select="''"/>
                </SpecialInstructions2>

                <SpecialInstructionsvalue2>
                  <xsl:value-of select="''"/>
                </SpecialInstructionsvalue2>

                <SpecialInstructions3>
                  <xsl:value-of select="''"/>
                </SpecialInstructions3>

                <SpecialInstructionsvalue3>
                  <xsl:value-of select="''"/>
                </SpecialInstructionsvalue3>

                <SpecialInstructions4>
                  <xsl:value-of select="''"/>
                </SpecialInstructions4>

                <SpecialInstructionsvalue4>
                  <xsl:value-of select="''"/>
                </SpecialInstructionsvalue4>

                <SpecialInstructions5>
                  <xsl:value-of select="''"/>

                </SpecialInstructions5>

                <SpecialInstructionsvalue5>
                  <xsl:value-of select="''"/>
                </SpecialInstructionsvalue5>


                <LoanDate>
                  <xsl:value-of select="''"/>
                </LoanDate>

                <PledgeeCode>
                  <xsl:value-of select="''"/>
                </PledgeeCode>

                <ABANumber>
                  <xsl:value-of select="''"/>
                </ABANumber>

                <ABADescription>
                  <xsl:value-of select="''"/>
                </ABADescription>

                <PledgeInstruction1>
                  <xsl:value-of select="''"/>
                </PledgeInstruction1>

                <PledgeInstruction2>
                  <xsl:value-of select="''"/>
                </PledgeInstruction2>

                <SegAccount>
                  <xsl:value-of select="''"/>

                </SegAccount>

                <Margin>
                  <xsl:value-of select="''"/>
                </Margin>


                <UnderlyingSecurityIdType>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityIdType>

                <UnderlyingSecurityId>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityId>

<SecurityId3>
            <xsl:value-of select="SEDOL"/>
          </SecurityId3>
                <UnderlyingSecurityDescription>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityDescription>

                <OptionsType>
                  <xsl:value-of select="''"/>
                </OptionsType>

                <OpenCloseIndicator>
                  <xsl:value-of select="''"/>
                </OpenCloseIndicator>

                <OptionsStyle>
                  <xsl:value-of select="''"/>
                </OptionsStyle>

                <NumberofContracts>
                  <xsl:value-of select="''"/>
                </NumberofContracts>

                <ContractSize>
                  <xsl:value-of select="''"/>
                </ContractSize>

                <ExercisePriceCurrency>
                  <xsl:value-of select="''"/>
                </ExercisePriceCurrency>

                <ExercisePriceAmount>
                  <xsl:value-of select="''"/>
                </ExercisePriceAmount>

                <OCCMemberCodeType>
                  <xsl:value-of select="''"/>
                </OCCMemberCodeType>

                <OCCMemberIndicator>
                  <xsl:value-of select="''"/>
                </OCCMemberIndicator>

                <OCCMemberCode>
                  <xsl:value-of select="''"/>
                </OCCMemberCode>

                <OCCMemberName>
                  <xsl:value-of select="''"/>
                </OCCMemberName>

                <OCCMemberAcatBroker>
                  <xsl:value-of select="''"/>
                </OCCMemberAcatBroker>

                <OCCParticipantCodeType>
                  <xsl:value-of select="''"/>

                </OCCParticipantCodeType>

                <OCCParticipantCode>
                  <xsl:value-of select="''"/>
                </OCCParticipantCode>

                <OCCParticipantName>
                  <xsl:value-of select="''"/>
                </OCCParticipantName>

                <OCCParticipantAcatBroker>
                  <xsl:value-of select="''"/>
                </OCCParticipantAcatBroker>

                <ReasonCode>
                  <xsl:value-of select="''"/>
                </ReasonCode>

                <ContractDate>
                  <xsl:value-of select="''"/>
                </ContractDate>

                <RecordDate>
                  <xsl:value-of select="''"/>
                </RecordDate>

                <PayDate>
                  <xsl:value-of select="''"/>
                </PayDate>

                <OldPriceCurrency>
                  <xsl:value-of select="''"/>
                </OldPriceCurrency>

                <OldPriceAmount>
                  <xsl:value-of select="''"/>
                </OldPriceAmount>

                <NewPriceCurrency>
                  <xsl:value-of select="''"/>
                </NewPriceCurrency>

                <NewPriceAmount>
                  <xsl:value-of select="''"/>
                </NewPriceAmount>


                <BranchCode>
                  <xsl:value-of select="''"/>
                </BranchCode>


                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>

            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'False'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>

              <TypeOfRequest>
                <xsl:value-of  select="'New'"/>
              </TypeOfRequest>

              <OriginalCustomerReferenceNumber>
                <xsl:value-of select="''"/>
              </OriginalCustomerReferenceNumber>

              <PreviousReferenceNumber>
                <xsl:value-of select="''"/>
              </PreviousReferenceNumber>



              <SafekeepingAccount>
                <xsl:value-of select="FundAccntNo"/>
              </SafekeepingAccount>

              <CustomerReferenceNumber>
               <xsl:value-of select="concat(PBUniqueID,position())"/>
              </CustomerReferenceNumber>

              <InstructionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>

                </xsl:choose>
              </InstructionType>

              <TransactionType>
                <xsl:value-of select="'Standard'"/>
              </TransactionType>

              <SecurityIdType>
                <xsl:value-of select="'ISIN'"/>
              </SecurityIdType>

              <SecurityId>
                <xsl:value-of select="ISIN"/>
              </SecurityId>

              <SecurityId2>
                <xsl:value-of select="SEDOL"/>
              </SecurityId2>


              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <ReceiveAccount>
                <xsl:value-of select="''"/>
              </ReceiveAccount>


              <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate">
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <TradeDate>
            <xsl:value-of select="$varTradeDate"/>
          </TradeDate>


              <xsl:variable name="varSettlementDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate">
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <SettlementDate>
           <xsl:value-of select="$varSettlementDate"/>
          </SettlementDate>

               <xsl:variable name="PB_NAME">
            <xsl:value-of select="'CITI'"/>
          </xsl:variable>

          <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
          <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
          </xsl:variable>

          <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
          </xsl:variable>

          <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
          </xsl:variable>

          <SettlementLocation>
            <xsl:choose>

              <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'DE'"/>
              </xsl:when>
              <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'NL'"/>
              </xsl:when>


              <xsl:otherwise>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementLocation>


              <PlaceofSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceofSafekeeping>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>


              <SettlementQuantity>
                <xsl:value-of select="OrderQty"/>
              </SettlementQuantity>
<xsl:variable name="varSetNetamount2">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
              <SettlementAmount>
        
				<xsl:value-of select="format-number($varSetNetamount2,'#.##')"/>
              </SettlementAmount>

              <Freetext>
                <xsl:value-of select="''"/>

              </Freetext>

              <CashAccountNumber>
                <xsl:value-of select="''"/>
              </CashAccountNumber>

              <CashAccountcurrency>
                <xsl:value-of select="''"/>
              </CashAccountcurrency>

              <Shortform>
                <xsl:value-of select="''"/>
              </Shortform>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
          </xsl:variable>
          <ClearingAgentCodeType>
            <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
          </ClearingAgentCodeType>


              
              <xsl:variable name="PRANA_COUNTERPARTY">
                <xsl:value-of select="CounterParty"/>
              </xsl:variable>

              <xsl:variable name="PB_COUNTERPARTY">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name = $PB_NAME]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@DTCCode"/>
              </xsl:variable>

              <xsl:variable name="varDTCCode">
                <xsl:choose>
                  <xsl:when test="$PB_COUNTERPARTY = ''">
                    <xsl:value-of select="$PRANA_COUNTERPARTY"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_COUNTERPARTY"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
               <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
          </xsl:variable>
          <ClearingAgentCode>
            <xsl:choose>

              <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'PARBDEFFXXX'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ClearingBrokerBIC != ' '">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClearingAgentCode>


              <ClearingAgentName>
                <xsl:value-of select="''"/>
              </ClearingAgentName>

              <Acatdepository>
                <xsl:value-of select="''"/>
              </Acatdepository>

               <BuyerSellerCodeType>
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'LOCAL'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'BIC'"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuyerSellerCodeType>


              <BuyerSellercode>
            <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
          </BuyerSellercode>

              <BuyerSellerName>
                <xsl:value-of select="CounterParty"/>
              </BuyerSellerName>

             <Acatcustodianclearingagt>
            <xsl:choose>

              <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'6547070004'"/>
              </xsl:when>
              <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'ESES 611'"/>
              </xsl:when>


              <xsl:otherwise>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerLocalCode"/>
              </xsl:otherwise>
            </xsl:choose>
          </Acatcustodianclearingagt>

              <IntermediaryCustodianCodeType>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianCodeType>

              <IntermediaryCustodianCode>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianCode>

              <IntermediaryCustodianName>
                <xsl:value-of select="''"/>
              </IntermediaryCustodianName>

              <Acatclearingagent>
                <xsl:value-of select="''"/>
              </Acatclearingagent>

              <NationalityofInvestor>
                <xsl:value-of select="''"/>
              </NationalityofInvestor>

              <PriceTypeCode>
                <xsl:value-of select="''"/>
              </PriceTypeCode>

              <Pricecurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Pricecurrency>

              <Price>
			  <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                
              </Price>

              <Dealamountcurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Dealamountcurrency>

              <Dealamount>
			  <xsl:value-of select="format-number($varNetamount,'#.##')"/>
                
              </Dealamount>

              <Brokercommissioncurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Brokercommissioncurrency>

              <Brokercommission>
                 <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </Brokercommission>

              <Taxcurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Taxcurrency>

              <Tax>
                <xsl:value-of select="OtherBrokerFees"/>
              </Tax>

              <Stampdutycurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Stampdutycurrency>

              <Stampduty>
                <xsl:value-of select="StampDuty"/>
              </Stampduty>

              <Amortisedamount>
                <xsl:value-of select="''"/>
              </Amortisedamount>

              <Miscellaneousfeescurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </Miscellaneousfeescurrency>

              <Miscellaneousfees>
			  <xsl:value-of select="format-number((ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee  ),'0.######')"/>
                     <!-- <xsl:value-of select="format-number(SecFee +TaxOnCommissions,'#.######')"/> -->
              </Miscellaneousfees>

              <PaymentLevytaxcurrency>
                <xsl:value-of select="''"/>
              </PaymentLevytaxcurrency>

              <Paymentlevytax>
                <xsl:value-of select="''"/>
              </Paymentlevytax>

              <Countrytaxcurrency>
                <xsl:value-of select="''"/>
              </Countrytaxcurrency>

              <Countrytax>
                <xsl:value-of select="''"/>

              </Countrytax>

              <Localtaxcurrency>
                <xsl:value-of select="''"/>
              </Localtaxcurrency>



              <Localtax>
                <xsl:value-of select="''"/>

              </Localtax>

              <LocalCountryTaxcurrency>
                <xsl:value-of select="''"/>

              </LocalCountryTaxcurrency>


              <LocalCountryTax>
                <xsl:value-of select="''"/>
              </LocalCountryTax>

              <Localbrokercommissioncurrency>
                <xsl:value-of select="''"/>
              </Localbrokercommissioncurrency>

              <Localbrokercommission>
                <xsl:value-of select="''"/>
              </Localbrokercommission>


              <Regulatoryamountcurrency>
                <xsl:value-of select="''"/>
              </Regulatoryamountcurrency>

              <Regulatoryamount>
                <xsl:value-of select="''"/>
              </Regulatoryamount>

              <Shippingamountcurrency>
                <xsl:value-of select="''"/>
              </Shippingamountcurrency>

              <Shippingamount>
                <xsl:value-of select="''"/>
              </Shippingamount>

              <Stockexchangetaxcurrency>
                <xsl:value-of select="''"/>
              </Stockexchangetaxcurrency>

              <Stockexchangetax>
                <xsl:value-of select="''"/>
              </Stockexchangetax>

              <Transfertaxcurrency>
                <xsl:value-of select="''"/>
              </Transfertaxcurrency>

              <Transfertax>
                <xsl:value-of select="''"/>
              </Transfertax>

              <Transactiontaxcurrency>
                <xsl:value-of select="''"/>
              </Transactiontaxcurrency>

              <Transactiontax>
                <xsl:value-of select="''"/>
              </Transactiontax>

              <WithholdingTaxcurrency>
                <xsl:value-of select="''"/>
              </WithholdingTaxcurrency>

              <WithholdingTax>
                <xsl:value-of select="''"/>
              </WithholdingTax>

              <ConsumptionTaxcurrency>
                <xsl:value-of select="''"/>
              </ConsumptionTaxcurrency>

              <ConsumptionTax>
                <xsl:value-of select="''"/>
              </ConsumptionTax>

              <Accruedinterestcurrency>
                <xsl:value-of select="''"/>
              </Accruedinterestcurrency>

              <Accruedinterest>
                <xsl:value-of select="''"/>
              </Accruedinterest>

              <Marginamountcurrency>
                <xsl:value-of select="''"/>
              </Marginamountcurrency>

              <Marginamount>
                <xsl:value-of select="''"/>
              </Marginamount>

              <NetGainLosscurrency>
                <xsl:value-of select="''"/>
              </NetGainLosscurrency>

              <NetGainLoss>
                <xsl:value-of select="''"/>
              </NetGainLoss>

              <Accruedcapitalizationcurrency>
                <xsl:value-of select="''"/>
              </Accruedcapitalizationcurrency>

              <Accruedcapitalization>
                <xsl:value-of select="''"/>
              </Accruedcapitalization>

              <IssueDisctAllowancecurrency>
                <xsl:value-of select="''"/>
              </IssueDisctAllowancecurrency>


              <IssueDisctAllowance>
                <xsl:value-of select="''"/>
              </IssueDisctAllowance>

              <ConcessionAmountcurrency>
                <xsl:value-of select="''"/>
              </ConcessionAmountcurrency>

              <ConcessionAmount>
                <xsl:value-of select="''"/>
              </ConcessionAmount>

              <ResultingAmtcurrency>
                <xsl:value-of select="''"/>
              </ResultingAmtcurrency>

              <ResultingAmt>
                <xsl:value-of select="''"/>
              </ResultingAmt>


              <ExchangeRate>
                <xsl:value-of select="''"/>

              </ExchangeRate>

              <ExchangeRateFirstcurrency>
                <xsl:value-of select="''"/>
              </ExchangeRateFirstcurrency>

              <ExchangeRateSecondcurrency>
                <xsl:value-of select="''"/>
              </ExchangeRateSecondcurrency>

              <OriginalOrderedAmtcurrency>
                <xsl:value-of select="''"/>
              </OriginalOrderedAmtcurrency>


              <OriginalOrderedAmt>
                <xsl:value-of select="''"/>
              </OriginalOrderedAmt>


              <OtherAmtcurrency>
                <xsl:value-of select="''"/>
              </OtherAmtcurrency>


              <OtherAmt>
                <xsl:value-of select="''"/>
              </OtherAmt>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <InterestRate>
                <xsl:value-of select="''"/>

              </InterestRate>

              <IssueDate>
                <xsl:value-of select="''"/>

              </IssueDate>

              <ExpiryDate>
                <xsl:value-of select="''"/>
              </ExpiryDate>

              <DatedDate>
                <xsl:value-of select="''"/>
              </DatedDate>

              <CouponDate>
                <xsl:value-of select="''"/>
              </CouponDate>

              <AccruedIntDays>
                <xsl:value-of select="''"/>

              </AccruedIntDays>

              <AccountWithInstitutionCodeType>
                <xsl:value-of select="''"/>
              </AccountWithInstitutionCodeType>

              <AccountWithInstitutionCode >
                <xsl:value-of select="''"/>
              </AccountWithInstitutionCode>

              <AccountWithInstitutionName>
                <xsl:value-of select="''"/>
              </AccountWithInstitutionName>

              <ACCWCashAccount>
                <xsl:value-of select="''"/>

              </ACCWCashAccount>

              <CashBeneficiaryCodeType>
                <xsl:value-of select="''"/>
              </CashBeneficiaryCodeType>

              <CashBeneficiaryCode>
                <xsl:value-of select="''"/>
              </CashBeneficiaryCode>

              <CashBeneficiaryName>
                <xsl:value-of select="''"/>
              </CashBeneficiaryName>

              <BENMCashAccount>
                <xsl:value-of select="''"/>
              </BENMCashAccount>

              <PayeeCodeType>
                <xsl:value-of select="''"/>
              </PayeeCodeType>

              <PayeeCode>
                <xsl:value-of select="''"/>
              </PayeeCode>

              <PayeeName>
                <xsl:value-of select="''"/>
              </PayeeName>

              <PAYECashAccount>
                <xsl:value-of select="''"/>
              </PAYECashAccount>

              <TransactionReference>
                <xsl:value-of select="''"/>
              </TransactionReference>


              <SecondLegReference>
                <xsl:value-of select="''"/>
              </SecondLegReference>

              <ClosingDate>
                <xsl:value-of select="''"/>
              </ClosingDate>

              <RepurchaseTypeIndicator>
                <xsl:value-of select="''"/>
              </RepurchaseTypeIndicator>

              <TerminationCCY>
                <xsl:value-of select="''"/>
              </TerminationCCY>


              <TerminationAmt>
                <xsl:value-of select="''"/>
              </TerminationAmt>

              <TerminationAmtperPieceofCollateral>
                <xsl:value-of select="''"/>
              </TerminationAmtperPieceofCollateral>

              <PremiumCCY>
                <xsl:value-of select="''"/>
              </PremiumCCY>

              <PremiumAmt>
                <xsl:value-of select="''"/>
              </PremiumAmt>

              <RepurchaseRate>
                <xsl:value-of select="''"/>
              </RepurchaseRate>

              <RateType>
                <xsl:value-of select="''"/>
              </RateType>

              <SpreadRate>
                <xsl:value-of select="''"/>

              </SpreadRate>

              <CallDelay>
                <xsl:value-of select="''"/>
              </CallDelay>

              <TotalCollateral>
                <xsl:value-of select="''"/>
              </TotalCollateral>

              <InstructionSequenceNumber>
                <xsl:value-of select="''"/>
              </InstructionSequenceNumber>

              <SecondLegNarrative>
                <xsl:value-of select="''"/>
              </SecondLegNarrative>

              <SecuritiesAlternateId>
                <xsl:value-of select="''"/>

              </SecuritiesAlternateId>

              <RelatedReference>
                <xsl:value-of select="''"/>
              </RelatedReference>

              <CollateralReference1>
                <xsl:value-of select="''"/>
              </CollateralReference1>

              <CollateralReference2>
                <xsl:value-of select="''"/>
              </CollateralReference2>

              <CollateralReference3>
                <xsl:value-of select="''"/>
              </CollateralReference3>

              <CollateralReference4>
                <xsl:value-of select="''"/>
              </CollateralReference4>

              <CollateralReference5>
                <xsl:value-of select="''"/>
              </CollateralReference5>

              <CollateralReference6>
                <xsl:value-of select="''"/>
              </CollateralReference6>

              <CollateralReference7>
                <xsl:value-of select="''"/>
              </CollateralReference7>

              <CollateralReference8>
                <xsl:value-of select="''"/>
              </CollateralReference8>

              <CollateralReference9>
                <xsl:value-of select="''"/>
              </CollateralReference9>

              <CollateralReference10>
                <xsl:value-of select="''"/>
              </CollateralReference10>

              <SpecialInstructions1>
                <xsl:value-of select="''"/>
              </SpecialInstructions1>

              <SpecialInstructionsvalue1>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue1>

              <SpecialInstructions2>
                <xsl:value-of select="''"/>
              </SpecialInstructions2>

              <SpecialInstructionsvalue2>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue2>

              <SpecialInstructions3>
                <xsl:value-of select="''"/>
              </SpecialInstructions3>

              <SpecialInstructionsvalue3>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue3>

              <SpecialInstructions4>
                <xsl:value-of select="''"/>
              </SpecialInstructions4>

              <SpecialInstructionsvalue4>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue4>

              <SpecialInstructions5>
                <xsl:value-of select="''"/>

              </SpecialInstructions5>

              <SpecialInstructionsvalue5>
                <xsl:value-of select="''"/>
              </SpecialInstructionsvalue5>


              <LoanDate>
                <xsl:value-of select="''"/>
              </LoanDate>

              <PledgeeCode>
                <xsl:value-of select="''"/>
              </PledgeeCode>

              <ABANumber>
                <xsl:value-of select="''"/>
              </ABANumber>

              <ABADescription>
                <xsl:value-of select="''"/>
              </ABADescription>

              <PledgeInstruction1>
                <xsl:value-of select="''"/>
              </PledgeInstruction1>

              <PledgeInstruction2>
                <xsl:value-of select="''"/>
              </PledgeInstruction2>

              <SegAccount>
                <xsl:value-of select="''"/>

              </SegAccount>

              <Margin>
                <xsl:value-of select="''"/>
              </Margin>


              <UnderlyingSecurityIdType>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityIdType>

              <UnderlyingSecurityId>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityId>

              <SecurityId3>
                <xsl:value-of select="SEDOL"/>
              </SecurityId3>

              <UnderlyingSecurityDescription>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityDescription>

              <OptionsType>
                <xsl:value-of select="''"/>
              </OptionsType>

              <OpenCloseIndicator>
                <xsl:value-of select="''"/>
              </OpenCloseIndicator>

              <OptionsStyle>
                <xsl:value-of select="''"/>
              </OptionsStyle>

              <NumberofContracts>
                <xsl:value-of select="''"/>
              </NumberofContracts>

              <ContractSize>
                <xsl:value-of select="''"/>
              </ContractSize>

              <ExercisePriceCurrency>
                <xsl:value-of select="''"/>
              </ExercisePriceCurrency>

              <ExercisePriceAmount>
                <xsl:value-of select="''"/>
              </ExercisePriceAmount>

              <OCCMemberCodeType>
                <xsl:value-of select="''"/>
              </OCCMemberCodeType>

              <OCCMemberIndicator>
                <xsl:value-of select="''"/>
              </OCCMemberIndicator>

              <OCCMemberCode>
                <xsl:value-of select="''"/>
              </OCCMemberCode>

              <OCCMemberName>
                <xsl:value-of select="''"/>
              </OCCMemberName>

              <OCCMemberAcatBroker>
                <xsl:value-of select="''"/>
              </OCCMemberAcatBroker>

              <OCCParticipantCodeType>
                <xsl:value-of select="''"/>

              </OCCParticipantCodeType>

              <OCCParticipantCode>
                <xsl:value-of select="''"/>
              </OCCParticipantCode>

              <OCCParticipantName>
                <xsl:value-of select="''"/>
              </OCCParticipantName>

              <OCCParticipantAcatBroker>
                <xsl:value-of select="''"/>
              </OCCParticipantAcatBroker>

              <ReasonCode>
                <xsl:value-of select="''"/>
              </ReasonCode>

              <ContractDate>
                <xsl:value-of select="''"/>
              </ContractDate>

              <RecordDate>
                <xsl:value-of select="''"/>
              </RecordDate>

              <PayDate>
                <xsl:value-of select="''"/>
              </PayDate>

              <OldPriceCurrency>
                <xsl:value-of select="''"/>
              </OldPriceCurrency>

              <OldPriceAmount>
                <xsl:value-of select="''"/>
              </OldPriceAmount>

              <NewPriceCurrency>
                <xsl:value-of select="''"/>
              </NewPriceCurrency>

              <NewPriceAmount>
                <xsl:value-of select="''"/>
              </NewPriceAmount>


              <BranchCode>
                <xsl:value-of select="''"/>
              </BranchCode>


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
