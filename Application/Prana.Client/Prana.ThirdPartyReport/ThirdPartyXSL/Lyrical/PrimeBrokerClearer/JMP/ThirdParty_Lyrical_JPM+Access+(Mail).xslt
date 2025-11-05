<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
   public int RoundOff(double Qty)
   {

   return (int)Math.Round(Qty,3);
   }
 </msxsl:script>
  
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

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

        <AccountNumber>
          <xsl:value-of select="'AccountNumber'"/>
        </AccountNumber>

        <TransactionType>
          <xsl:value-of select="'TransactionType'"/>
        </TransactionType>

        <NetAmount>
          <xsl:value-of select="'NetAmount (Amount)'"/>
        </NetAmount>

        <AssetType>
          <xsl:value-of select="'AssetType'"/>
        </AssetType>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'SettlementDate'"/>
        </SettlementDate>

        <SecurityType>
          <xsl:value-of select="'SecurityType'"/>
        </SecurityType>

        <SecurityID>
          <xsl:value-of select="'SecurityID'"/>
        </SecurityID>

        <SecurityDescription>
          <xsl:value-of select="'SecurityDescription'"/>
        </SecurityDescription>

        <TradeBroker>
          <xsl:value-of select="'TradeBroker'"/>
        </TradeBroker>

        <Filler1>
          <xsl:value-of select="'Filler'"/>
        </Filler1>

        <ClearingBroker>
          <xsl:value-of select="'ClearingBroker'"/>
        </ClearingBroker>

        <Filler2>
          <xsl:value-of select="'Filler'"/>
        </Filler2>

        <Quanity>
          <xsl:value-of select="'Quantity'"/>
        </Quanity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

		  <AccruedInterest>
          <xsl:value-of select="'AccruedInterest'"/>
        </AccruedInterest>

        <ClientReferenceNumber>
          <xsl:value-of select="'ClientReferenceNumber/Reference2'"/>
        </ClientReferenceNumber>

        <SpecialInstructions>
          <xsl:value-of select="'SpecialInstructions'"/>
        </SpecialInstructions>

        <Reference>
          <xsl:value-of select="'Reference (Reference 1)'"/>
        </Reference>

        <DTCControl>
          <xsl:value-of select="'DTCControl'"/>
        </DTCControl>

        <Authorization>
          <xsl:value-of select="'Authorization'"/>
        </Authorization>

        <FortyEightHourUpdate>
          <xsl:value-of select="'FortyEightHourUpdate'"/>
        </FortyEightHourUpdate>

        <Registration>
          <xsl:value-of select="'Registration'"/>
        </Registration>

        <TransferToAccount>
          <xsl:value-of select="'TransferToAccount'"/>
        </TransferToAccount>

        <InventoryValue>
          <xsl:value-of select="'InventoryValue'"/>
        </InventoryValue>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Fees>
          <xsl:value-of select="'Fees'"/>
        </Fees>

        <InstructionToFollow>
          <xsl:value-of select="'InstructionToFollow'"/>
        </InstructionToFollow>

        <DirectMail>
          <xsl:value-of select="'DirectMail'"/>
        </DirectMail>

        <ShippingName>
          <xsl:value-of select="'ShippingName'"/>
        </ShippingName>

        <ShippingAddress1>
          <xsl:value-of select="'ShippingAddress1'"/>
        </ShippingAddress1>

        <TransfereeName>
          <xsl:value-of select="'TransfereeName'"/>
        </TransfereeName>
		  
		  <TransfereeAddress1>
			  <xsl:value-of select="'TransfereeAddress1'"/>
		  </TransfereeAddress1>
		  
		  <NonResidentAlien>
			  <xsl:value-of select="'Non-ResidentAlien'"/>
		  </NonResidentAlien>

		  <TaxpayerID>
			  <xsl:value-of select="'TaxpayerID'"/>
		  </TaxpayerID>
		  <CreditReportDescription>
			  <xsl:value-of select="'CreditReportDescription'"/>
		  </CreditReportDescription>
		  <DebitReportDescription>
			  <xsl:value-of select="'DebitReportDescription'"/>
		  </DebitReportDescription>

		  <TaxClass>
			  <xsl:value-of select="'TaxClass'"/>
        </TaxClass>
        
        <ReportDescription>
          <xsl:value-of select="'ReportDescription'"/>
        </ReportDescription>
        <SendingAcctNo>
          <xsl:value-of select="'SendingAcctNo.'"/>
        </SendingAcctNo>
        <SendingAccountName>
          <xsl:value-of select="'SendingAccountName'"/>
        </SendingAccountName>
        <SendingBankABANo>
          <xsl:value-of select="'SendingBankABANo'"/>
        </SendingBankABANo>
        <OtherBeneficiaryInformation>
          <xsl:value-of select="'OtherBeneficiaryInformation'"/>
        </OtherBeneficiaryInformation>
        
        <ReceivingAcctNo>
          <xsl:value-of select="'ReceivingAcctNo.'"/>
        </ReceivingAcctNo>
        <ReceivingAcctName>
          <xsl:value-of select="'ReceivingAcctName'"/>
        </ReceivingAcctName>
        <ReceivingBankABANo>
          <xsl:value-of select="'ReceivingBankABANo.'"/>
        </ReceivingBankABANo>
        <BeneficiaryAcctNo>
          <xsl:value-of select="'BeneficiaryAcctNo.'"/>
        </BeneficiaryAcctNo>
        <BeneficiaryAcctName>
          <xsl:value-of select="'BeneficiaryAcctName'"/>
        </BeneficiaryAcctName>
        <Settlementlocation>
          <xsl:value-of select="'Settlementlocation'"/>
        </Settlementlocation>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>
        <FaceValue>
          <xsl:value-of select="'Face Value'"/>
        </FaceValue>
        <GrossAmount>
          <xsl:value-of select="'Gross Amount'"/>
        </GrossAmount>
        
        <EXCUM>
          <xsl:value-of select="'EX/CUM'"/>
        </EXCUM>

        <BrokerReference>
          <xsl:value-of select="'BrokerReference'"/>
        </BrokerReference>
        <DoNotRegister>
          <xsl:value-of select="'DoNotRegister'"/>
        </DoNotRegister>
        <TaxAmount>
          <xsl:value-of select="'Tax Amount'"/>
        </TaxAmount>
        <ShipTransfer>
          <xsl:value-of select="'ShipTransfer'"/>
        </ShipTransfer>
        <Charges>
          <xsl:value-of select="'Charges'"/>
        </Charges>
        
        <DepositoryPrincipalBrokerIDType>
          <xsl:value-of select="'Depository  / Principal Broker ID Type'"/>
        </DepositoryPrincipalBrokerIDType>

        <ClearingAt>
          <xsl:value-of select="'ClearingAt'"/>
        </ClearingAt>
        <Participant>
          <xsl:value-of select="'Participant'"/>
        </Participant>
        <UltimateBeneficiary>
          <xsl:value-of select="'UltimateBeneficiary'"/>
        </UltimateBeneficiary>
        <UltimateBeneficiaryName>
          <xsl:value-of select="'UltimateBeneficiaryName'"/>
        </UltimateBeneficiaryName>
        <BeneficiaryBankName>
          <xsl:value-of select="'BeneficiaryBankName'"/>
        </BeneficiaryBankName>
        <Filler3>
          <xsl:value-of select="'Filler'"/>
        </Filler3>
        
        <OrderingPartyDeliveryAgent>
          <xsl:value-of select="'OrderingParty/Delivery Agent'"/>
        </OrderingPartyDeliveryAgent>

        <IntermediaryName>
          <xsl:value-of select="'IntermediaryName'"/>
        </IntermediaryName>
        <Filler4>
          <xsl:value-of select="'Filler'"/>
        </Filler4>
        
        <BuyCurrency>
          <xsl:value-of select="'Buy Currency'"/>
        </BuyCurrency>
        <BuyAmount>
          <xsl:value-of select="'Buy Amount'"/>
        </BuyAmount>
        <SellCurrency>
          <xsl:value-of select="'Sell Currency'"/>
        </SellCurrency>

        <SellAmount>
          <xsl:value-of select="'SellAmount'"/>
        </SellAmount>
        <Filler5>
          <xsl:value-of select="'Filler'"/>
        </Filler5>
        <RequestedValueDate>
          <xsl:value-of select="'RequestedValueDate'"/>
        </RequestedValueDate>
        <Filler6>
          <xsl:value-of select="'Filler'"/>
        </Filler6>
        <Filler7>
          <xsl:value-of select="'Filler'"/>
        </Filler7>
        <Filler8>
          <xsl:value-of select="'Filler'"/>
        </Filler8>
        <Filler9>
          <xsl:value-of select="'Filler'"/>
        </Filler9>
        <Rate1>
          <xsl:value-of select="'Rate'"/>
        </Rate1>
        <Filler10>
          <xsl:value-of select="'Filler'"/>
        </Filler10>
        <AdditionalDetails>
          <xsl:value-of select="'AdditionalDetails'"/>
        </AdditionalDetails>
        <Filler11>
          <xsl:value-of select="'Filler'"/>
        </Filler11>
        <Filler12>
          <xsl:value-of select="'Filler'"/>
        </Filler12>
        <Filler13>
          <xsl:value-of select="'Filler'"/>
        </Filler13>
        <Filler14>
          <xsl:value-of select="'Filler'"/>
        </Filler14>
        <DTCControlAffirm>
          <xsl:value-of select="'DTCControlAffirm'"/>
        </DTCControlAffirm>
        <ReferenceNo>
          <xsl:value-of select="'ReferenceNo'"/>
        </ReferenceNo>
        <ReferenceDescription>
          <xsl:value-of select="'ReferenceDescription'"/>
        </ReferenceDescription>
        <UltimateBeneficiaryBIC>
          <xsl:value-of select="'UltimateBeneficiaryBIC'"/>
        </UltimateBeneficiaryBIC>
        <BeneficiaryBankBIC>
          <xsl:value-of select="'BeneficiaryBankBIC'"/>
        </BeneficiaryBankBIC>
        <IntermediaryBIC>
          <xsl:value-of select="'IntermediaryBIC'"/>
        </IntermediaryBIC>
        <ClientLoanReferenceNumber>
          <xsl:value-of select="'ClientLoanReferenceNumber'"/>
        </ClientLoanReferenceNumber>
        <OriginalClientLoanReferenceNumber>
          <xsl:value-of select="'OriginalClientLoanReferenceNumber'"/>
        </OriginalClientLoanReferenceNumber>
        <Skeletal_Security_Type>
          <xsl:value-of select="'Skeletal_Security_Type'"/>
        </Skeletal_Security_Type>
        <SubType>
          <xsl:value-of select="'SubType'"/>
        </SubType>
        <InterestType>
          <xsl:value-of select="'InterestType'"/>
        </InterestType>
        <IssueDate>
          <xsl:value-of select="'IssueDate'"/>
        </IssueDate>
        <MaturityDate>
          <xsl:value-of select="'MaturityDate'"/>
        </MaturityDate>
        <InterestRate>
          <xsl:value-of select="'InterestRate'"/>
        </InterestRate>
        <PoolNumber>
          <xsl:value-of select="'PoolNumber'"/>
        </PoolNumber>
        <InterestPrincipalIndicator>
          <xsl:value-of select="'InterestPrincipalIndicator'"/>
        </InterestPrincipalIndicator>
        <NoteCertificateNum>
          <xsl:value-of select="'NoteCertificateNum'"/>
        </NoteCertificateNum>
        <Issuer>
          <xsl:value-of select="'Issuer'"/>
        </Issuer>
        <IssuerName>
          <xsl:value-of select="'IssuerName'"/>
        </IssuerName>
        <PaperType>
          <xsl:value-of select="'PaperType'"/>
        </PaperType>
        <DTCPaper>
          <xsl:value-of select="'DTCPaper'"/>
        </DTCPaper>
        <Instruction_Confirmation>
          <xsl:value-of select="'Instruction_Confirmation'"/>
        </Instruction_Confirmation>
        <Custody_NoCustody>
          <xsl:value-of select="'Custody_NoCustody'"/>
        </Custody_NoCustody>
        <Amendment>
          <xsl:value-of select="'Amendment'"/>
        </Amendment>
        <NationalClearingSystemCode>
          <xsl:value-of select="'NationalClearingSystemCode'"/>
        </NationalClearingSystemCode>
        <UltimateBeneficiaryAccount>
          <xsl:value-of select="'UltimateBeneficiaryAccount'"/>
        </UltimateBeneficiaryAccount>
        <AustraclearCode>
          <xsl:value-of select="'AustraclearCode'"/>
        </AustraclearCode>
        <BuySellIndicator>
          <xsl:value-of select="'BuySellIndicator'"/>
        </BuySellIndicator>
        <FinancialIndicator>
          <xsl:value-of select="'FinancialIndicator'"/>
        </FinancialIndicator>
        
        <OrderedCurrency>
          <xsl:value-of select="'Ordered Currency'"/>
        </OrderedCurrency>
        <CounterCurrency>
          <xsl:value-of select="'Counter Currency'"/>
        </CounterCurrency>
        <OrderedAmount>
          <xsl:value-of select="'Ordered Amount'"/>
        </OrderedAmount>
        <OrderingPartyDeliveryAgentBIC>
          <xsl:value-of select="'Ordering PartyDelivery Agent BIC'"/>
        </OrderingPartyDeliveryAgentBIC>

        <ShortSaleCode>
          <xsl:value-of select="'ShortSaleCode'"/>
        </ShortSaleCode>
        
        <RepurchaseType>
          <xsl:value-of select="'Repurchase Type'"/>
        </RepurchaseType>

        <RepoReference>
          <xsl:value-of select="'Repo Reference'"/>
        </RepoReference>
        
        <SettlementType>
          <xsl:value-of select="'Settlement Type'"/>
        </SettlementType>
        <RepoRate>
          <xsl:value-of select="'Repo Rate'"/>
        </RepoRate>
        <ClearingBrokerType>
          <xsl:value-of select="'Clearing Broker Type'"/>
        </ClearingBrokerType>
        <ExpirationDate>
          <xsl:value-of select="'Expiration Date'"/>
        </ExpirationDate>
        <ContractSize>
          <xsl:value-of select="'Contract Size'"/>
        </ContractSize>
        <OptionFutureSide>
          <xsl:value-of select="'Option Future Side'"/>
        </OptionFutureSide>
        <OptionFutureType>
          <xsl:value-of select="'Option Future Type'"/>
        </OptionFutureType>
        <FundType>
          <xsl:value-of select="'Fund Type'"/>
        </FundType>
        <StrikePrice>
          <xsl:value-of select="'Strike Price'"/>
        </StrikePrice>
        <FirstCouponDate>
          <xsl:value-of select="'First Coupon Date'"/>
        </FirstCouponDate>
        <PaymentFrequency>
          <xsl:value-of select="'Payment Frequency'"/>
        </PaymentFrequency>
        <ForfeitAmount>
          <xsl:value-of select="'Forfeit Amount'"/>
        </ForfeitAmount>
        <PremiumAmount>
          <xsl:value-of select="'Premium Amount'"/>
        </PremiumAmount>
        <TerminationAmount>
          <xsl:value-of select="'Termination Amount'"/>
        </TerminationAmount>
        <VariableRateSupport>
          <xsl:value-of select="'Variable Rate Support'"/>
        </VariableRateSupport>
        <MethodOfInterestComputation>
          <xsl:value-of select="'Method Of Interest Computation'"/>
        </MethodOfInterestComputation>
        <RateType>
          <xsl:value-of select="'Rate Type'"/>
        </RateType>

        <Rate2>
          <xsl:value-of select="'Rate'"/>
        </Rate2>
        
        <RateValue>
          <xsl:value-of select="'Rate Value'"/>
        </RateValue>
        <OpenTermIndicator>
          <xsl:value-of select="'Open Term Indicator'"/>
        </OpenTermIndicator>
        <ClosingSettlementDate>
          <xsl:value-of select="'Closing Settlement Date'"/>
        </ClosingSettlementDate>
        <PayChargesIndicator>
          <xsl:value-of select="'Pay Charges Indicator'"/>
        </PayChargesIndicator>

        <TradeDepositoryIndicator>
          <xsl:value-of select="'TradeDepositoryIndicator'"/>
        </TradeDepositoryIndicator>
        
        <StockLoanMarginRate>
          <xsl:value-of select="'Stock Loan Margin Rate'"/>
        </StockLoanMarginRate>
        <TermDate>
          <xsl:value-of select="'Term Date'"/>
        </TermDate>
        <PairOffIndicator>
          <xsl:value-of select="'Pair Off Indicator'"/>
        </PairOffIndicator>
        <PairOffTransactionNumber>
          <xsl:value-of select="'Pair Off Transaction Number'"/>
        </PairOffTransactionNumber>
        <CallDelay>
          <xsl:value-of select="'Call Delay'"/>
        </CallDelay>
        <OrderingInstitution>
          <xsl:value-of select="'Ordering Institution'"/>
        </OrderingInstitution>
        <IntraCompanyPayment>
          <xsl:value-of select="'Intra-Company Payment'"/>
        </IntraCompanyPayment>
        <FundManagerBIC>
          <xsl:value-of select="'Fund Manager BIC'"/>
        </FundManagerBIC>
        <FXCounterpartyBIC>
          <xsl:value-of select="'FX Counterparty BIC'"/>
        </FXCounterpartyBIC>
        <CLSSettlementMemberBIC>
          <xsl:value-of select="'CLS Settlement Member BIC'"/>
        </CLSSettlementMemberBIC>
        <BrokerSetupIndicator>
          <xsl:value-of select="'Broker Setup Indicator'"/>
        </BrokerSetupIndicator>
        <BrokerABANumber>
          <xsl:value-of select="'Broker ABA Number'"/>
        </BrokerABANumber>
        <BrokerThirdPartyMnemonic>
          <xsl:value-of select="'Broker Third Party Mnemonic'"/>
        </BrokerThirdPartyMnemonic>
        <PlaceofSettlementBIC>
          <xsl:value-of select="'Place of Settlement BIC'"/>
        </PlaceofSettlementBIC>

        <StampKeyword>
          <xsl:value-of select="'StampKeyword'"/>
        </StampKeyword>
        <BeneficiaryDeclaration>
          <xsl:value-of select="'BeneficiaryDeclaration'"/>
        </BeneficiaryDeclaration>
        <StampableAmount>
          <xsl:value-of select="'StampableAmount'"/>
        </StampableAmount>
        <StampAmountIndicator>
          <xsl:value-of select="'StampAmountIndicator'"/>
        </StampAmountIndicator>
        <PrincipalBrokerSafeAccount>
          <xsl:value-of select="'PrincipalBrokerSafeAccount'"/>
        </PrincipalBrokerSafeAccount>
        <ClearingBrokerSafeAccount>
          <xsl:value-of select="'ClearingBrokerSafeAccount'"/>
        </ClearingBrokerSafeAccount>
        <SafekeepingAccount>
          <xsl:value-of select="'SafekeepingAccount'"/>
        </SafekeepingAccount>
        <ExpiryDate>
          <xsl:value-of select="'ExpiryDate'"/>
        </ExpiryDate>
        <PaymentType>
          <xsl:value-of select="'PaymentType'"/>
        </PaymentType>
        <OptionType>
          <xsl:value-of select="'OptionType'"/>
        </OptionType>
        <OptionInstructionType>
          <xsl:value-of select="'OptionInstructionType'"/>
        </OptionInstructionType>
        <DealPrice>
          <xsl:value-of select="'DealPrice'"/>
        </DealPrice>
        <ExercisePrice>
          <xsl:value-of select="'ExercisePrice'"/>
        </ExercisePrice>
        <FuturesInstructionType>
          <xsl:value-of select="'FuturesInstructionType'"/>
        </FuturesInstructionType>
        <AccountingOptions>
          <xsl:value-of select="'AccountingOptions'"/>
        </AccountingOptions>
        <PlaceOfTrade>
          <xsl:value-of select="'PlaceOfTrade'"/>
        </PlaceOfTrade>
        <ContractSizePrice>
          <xsl:value-of select="'ContractSizePrice'"/>
        </ContractSizePrice>
        <ReasonCode>
          <xsl:value-of select="'ReasonCode'"/>
        </ReasonCode>
        <SupplementaryInformation>
          <xsl:value-of select="'SupplementaryInformation'"/>
        </SupplementaryInformation>
        
        <PaymentIntermediaryNationalClearingSystemCode>
          <xsl:value-of select="'Payment Intermediary National Clearing System Code'"/>
        </PaymentIntermediaryNationalClearingSystemCode>
        <PaymentIntermediaryBankBIC>
          <xsl:value-of select="'Payment Intermediary Bank BIC'"/>
        </PaymentIntermediaryBankBIC>
        <PaymentIntermediaryBankNameAddress>
          <xsl:value-of select="'Payment Intermediary Bank Name Address'"/>
        </PaymentIntermediaryBankNameAddress>
        <PaymentIntermediaryBeneficiaryBankAcctNo>
          <xsl:value-of select="'Payment Intermediary Beneficiary Bank Acct No'"/>
        </PaymentIntermediaryBeneficiaryBankAcctNo>

        <TransfereeAddress2>
          <xsl:value-of select="'TransfereeAddress2'"/>
        </TransfereeAddress2>
        <TransfereeAddress3>
          <xsl:value-of select="'TransfereeAddress3'"/>
        </TransfereeAddress3>
        <TransfereeAddress4>
          <xsl:value-of select="'TransfereeAddress4'"/>
        </TransfereeAddress4>
        <ShippingAddress2>
          <xsl:value-of select="'ShippingAddress2'"/>
        </ShippingAddress2>
        <ShippingAddress3>
          <xsl:value-of select="'ShippingAddress3'"/>
        </ShippingAddress3>
        <ShippingAddress5>
          <xsl:value-of select="'ShippingAddress5'"/>
        </ShippingAddress5>
        <CollateralStatutoryAsset>
          <xsl:value-of select="'CollateralStatutoryAsset'"/>
        </CollateralStatutoryAsset>
        <StateCommonwealthTerritory>
          <xsl:value-of select="'StateCommonwealthTerritory'"/>
        </StateCommonwealthTerritory>
        <ServiceablilityType>
          <xsl:value-of select="'ServiceablilityType'"/>
        </ServiceablilityType>
        <FxOptions>
          <xsl:value-of select="'FxOptions'"/>
        </FxOptions>
        <LinkedForeignExchange>
          <xsl:value-of select="'LinkedForeignExchange'"/>
        </LinkedForeignExchange>
        <BaseCurrency>
          <xsl:value-of select="'BaseCurrency'"/>
        </BaseCurrency>
        
        <DebitCashAccountNumber>
          <xsl:value-of select="'Debit Cash Account Number'"/>
        </DebitCashAccountNumber>
        <CreditCashAccountNumber>
          <xsl:value-of select="'Credit Cash Account Number'"/>
        </CreditCashAccountNumber>


      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">


        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

		  <xsl:variable name = "PRANA_FUND_NAME">
			<xsl:value-of select="AccountName"/>
		</xsl:variable>

		<xsl:variable name ="PB_FUND_CODE">
			<xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
		</xsl:variable>
		
		<xsl:variable name="varFees">
      <xsl:choose>
        <xsl:when test="number(StampDuty)">
          <xsl:value-of select="format-number(StampDuty,'#.##')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="0"/>
        </xsl:otherwise>
      </xsl:choose>            
       </xsl:variable>
		  
		  <xsl:variable name="varPrice">
            <xsl:value-of select="format-number(AveragePrice,'#.####')"/>
          </xsl:variable>
		
		  
          <AccountNumber>
            <xsl:value-of select="$PB_FUND_CODE"/>
          </AccountNumber>

          <TransactionType>
            <xsl:choose>
              <xsl:when test="TransactionType='Sell'">
                <xsl:value-of select="'DVP'"/>
              </xsl:when>
              <xsl:when test="TransactionType='Buy'">
                <xsl:value-of select="'RVP'"/>
              </xsl:when>
            </xsl:choose>
          </TransactionType>

          <xsl:variable name="varCommission">
            <xsl:choose>
              <xsl:when test="number(CommissionCharged)">
                <xsl:value-of select="format-number(CommissionCharged,'#.##')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          
          <NetAmount>
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select ="format-number(((AllocatedQty * $varPrice) + $varCommission + $varFees),'#.##')"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select ="format-number(((AllocatedQty * $varPrice) - $varCommission - $varFees),'#.##')"/>
              </xsl:when>
            </xsl:choose>
          </NetAmount>

          <AssetType>
            <xsl:value-of select="'EQU'"/>
          </AssetType>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <SecurityType>
            <xsl:value-of select="'CUSIP'"/>
          </SecurityType>

          <SecurityID>
            <xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
          </SecurityID>

          <SecurityDescription>
            <xsl:value-of select="''"/>
          </SecurityDescription>

          <TradeBroker>
              <xsl:choose>
                <xsl:when test="CounterParty='JPM'">
                  <xsl:value-of select="'352'"/>
                </xsl:when>
                <xsl:when test="CounterParty='BGCE'">
                  <xsl:value-of select="'161'"/>
                </xsl:when>
                <xsl:when test="CounterParty='SMHI'">
                  <xsl:value-of select="'443'"/>
                </xsl:when>
                <xsl:when test="CounterParty='SMBC'">
                  <xsl:value-of select="'443'"/>
                </xsl:when>
                <xsl:when test="CounterParty='RJET'">
                  <xsl:value-of select="'725'"/>
                </xsl:when>
				  <xsl:when test="CounterParty='GS'">
					  <xsl:value-of select="'DTC5'"/>
				  </xsl:when>

				  <xsl:when test="CounterParty='BARC'">
					  <xsl:value-of select="'229'"/>
				  </xsl:when>
				  <xsl:when test="CounterParty='MISL'">
              						<xsl:value-of select="'161'"/>
              						</xsl:when>
              </xsl:choose>
          </TradeBroker>

          <Filler1>
            <xsl:value-of select="''"/>
          </Filler1>

          <ClearingBroker>
            <xsl:value-of select="''"/>
          </ClearingBroker>

          <Filler2>
            <xsl:value-of select="''"/>
          </Filler2>

          <Quanity>
            <xsl:value-of select="AllocatedQty"/>
          </Quanity>

          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>
		 
          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <AccuredInterest>
            <xsl:value-of select="''"/>
          </AccuredInterest>

          <ClientReferenceNumber>
            <xsl:value-of select="''"/>
          </ClientReferenceNumber>

          <SpecialInstructions>
            <xsl:value-of select="''"/>
          </SpecialInstructions>

          <Reference>
            <xsl:value-of select="''"/>
          </Reference>

          <DTCControl>
            <xsl:value-of select="''"/>
          </DTCControl>

          <Authorization>
            <xsl:value-of select="''"/>
          </Authorization>

          <FortyEightHourUpdate>
            <xsl:value-of select="''"/>
          </FortyEightHourUpdate>

          <Registration>
            <xsl:value-of select="''"/>
          </Registration>

          <TransferToAccount>
            <xsl:value-of select="''"/>
          </TransferToAccount>

          <InventoryValue>
            <xsl:value-of select="''"/>
          </InventoryValue>

          <Principal>
            <xsl:value-of select="round((AllocatedQty * $varPrice) * 100) div 100"/>
          </Principal>

		  
          <Fees>
            <xsl:value-of select="$varFees"/>
          </Fees>

          <InstructionToFollow>
            <xsl:value-of select="''"/>
          </InstructionToFollow>

          <DirectMail>
            <xsl:value-of select="''"/>
          </DirectMail>

          <ShippingName>
            <xsl:value-of select="''"/>
          </ShippingName>

          <ShippingAddress1>
            <xsl:value-of select="''"/>
          </ShippingAddress1>

          <TransfereeName>
            <xsl:value-of select="''"/>
          </TransfereeName>

          <NonResidentAlien>
            <xsl:value-of select="''"/>
          </NonResidentAlien>
          
          <TaxpayerID>
            <xsl:value-of select="''"/>
          </TaxpayerID>
          <CreditReportDescription>
            <xsl:value-of select="''"/>
          </CreditReportDescription>
          <DebitReportDescription>
            <xsl:value-of select="''"/>
          </DebitReportDescription>
          <TaxClass>
            <xsl:value-of select="''"/>
          </TaxClass>
          <TransfereeAddress1>
            <xsl:value-of select="''"/>
          </TransfereeAddress1>
          <ReportDescription>
            <xsl:value-of select="''"/>
          </ReportDescription>
          <SendingAcctNo>
            <xsl:value-of select="''"/>
          </SendingAcctNo>
          <SendingAccountName>
            <xsl:value-of select="''"/>
          </SendingAccountName>
          <SendingBankABANo>
            <xsl:value-of select="''"/>
          </SendingBankABANo>
          <OtherBeneficiaryInformation>
            <xsl:value-of select="''"/>
          </OtherBeneficiaryInformation>
          <ReceivingAcctNo.>
            <xsl:value-of select="''"/>
          </ReceivingAcctNo.>
          <ReceivingAcctName>
            <xsl:value-of select="''"/>
          </ReceivingAcctName>
          <ReceivingBankABANo>
            <xsl:value-of select="''"/>
          </ReceivingBankABANo>
          <BeneficiaryAcctNo>
            <xsl:value-of select="''"/>
          </BeneficiaryAcctNo>
          <BeneficiaryAcctName>
            <xsl:value-of select="''"/>
          </BeneficiaryAcctName>
          <Settlementlocation>
            <xsl:value-of select="''"/>
          </Settlementlocation>
          <Currency>
            <xsl:value-of select="''"/>
          </Currency>
          <FaceValue>
            <xsl:value-of select="''"/>
          </FaceValue>
          <GrossAmount>
            <xsl:value-of select="''"/>
          </GrossAmount>
          <EXCUM>
            <xsl:value-of select="''"/>
          </EXCUM>
          <BrokerReference>
            <xsl:value-of select="''"/>
          </BrokerReference>
          <DoNotRegister>
            <xsl:value-of select="''"/>
          </DoNotRegister>
          <TaxAmount>
            <xsl:value-of select="''"/>
          </TaxAmount>
          <ShipTransfer>
            <xsl:value-of select="''"/>
          </ShipTransfer>
          <Charges>
            <xsl:value-of select="''"/>
          </Charges>
          <DepositoryPrincipalBrokerIDType>
            <xsl:value-of select="''"/>
          </DepositoryPrincipalBrokerIDType>
          <ClearingAt>
            <xsl:value-of select="''"/>
          </ClearingAt>
          <Participant>
            <xsl:value-of select="''"/>
          </Participant>
          <UltimateBeneficiary>
            <xsl:value-of select="''"/>
          </UltimateBeneficiary>
          <UltimateBeneficiaryName>
            <xsl:value-of select="''"/>
          </UltimateBeneficiaryName>
          <BeneficiaryBankName>
            <xsl:value-of select="''"/>
          </BeneficiaryBankName>
          <Filler3>
            <xsl:value-of select="''"/>
          </Filler3>
          <OrderingPartyDeliveryAgent>
            <xsl:value-of select="''"/>
          </OrderingPartyDeliveryAgent>
          <IntermediaryName>
            <xsl:value-of select="''"/>
          </IntermediaryName>
          <Filler4>
            <xsl:value-of select="''"/>
          </Filler4>
          <BuyCurrency>
            <xsl:value-of select="''"/>
          </BuyCurrency>
          <BuyAmount>
            <xsl:value-of select="''"/>
          </BuyAmount>
          <SellCurrency>
            <xsl:value-of select="''"/>
          </SellCurrency>
          <SellAmount>
            <xsl:value-of select="''"/>
          </SellAmount>
          <Filler5>
            <xsl:value-of select="''"/>
          </Filler5>
          <RequestedValueDate>
            <xsl:value-of select="''"/>
          </RequestedValueDate>
          <Filler6>
            <xsl:value-of select="''"/>
          </Filler6>
          <Filler7>
            <xsl:value-of select="''"/>
          </Filler7>
          <Filler8>
            <xsl:value-of select="''"/>
          </Filler8>
          <Filler9>
            <xsl:value-of select="''"/>
          </Filler9>
          <Rate1>
            <xsl:value-of select="''"/>
          </Rate1>
          <Filler10>
            <xsl:value-of select="''"/>
          </Filler10>
          <AdditionalDetails>
            <xsl:value-of select="''"/>
          </AdditionalDetails>
          <Filler11>
            <xsl:value-of select="''"/>
          </Filler11>
          <Filler12>
            <xsl:value-of select="''"/>
          </Filler12>
          <Filler13>
            <xsl:value-of select="''"/>
          </Filler13>
          <Filler14>
            <xsl:value-of select="''"/>
          </Filler14>
          <DTCControlAffirm>
            <xsl:value-of select="''"/>
          </DTCControlAffirm>
          <ReferenceNo>
            <xsl:value-of select="''"/>
          </ReferenceNo>
          <ReferenceDescription>
            <xsl:value-of select="''"/>
          </ReferenceDescription>
          <UltimateBeneficiaryBIC>
            <xsl:value-of select="''"/>
          </UltimateBeneficiaryBIC>
          <BeneficiaryBankBIC>
            <xsl:value-of select="''"/>
          </BeneficiaryBankBIC>
          <IntermediaryBIC>
            <xsl:value-of select="''"/>
          </IntermediaryBIC>
          <ClientLoanReferenceNumber>
            <xsl:value-of select="''"/>
          </ClientLoanReferenceNumber>
          <OriginalClientLoanReferenceNumber>
            <xsl:value-of select="''"/>
          </OriginalClientLoanReferenceNumber>
          <Skeletal_Security_Type>
            <xsl:value-of select="''"/>
          </Skeletal_Security_Type>
          <SubType>
            <xsl:value-of select="''"/>
          </SubType>
          <InterestType>
            <xsl:value-of select="''"/>
          </InterestType>
          <IssueDate>
            <xsl:value-of select="''"/>
          </IssueDate>
          <MaturityDate>
            <xsl:value-of select="''"/>
          </MaturityDate>
          <InterestRate>
            <xsl:value-of select="''"/>
          </InterestRate>
          <PoolNumber>
            <xsl:value-of select="''"/>
          </PoolNumber>
          <InterestPrincipalIndicator>
            <xsl:value-of select="''"/>
          </InterestPrincipalIndicator>
          <NoteCertificateNum>
            <xsl:value-of select="''"/>
          </NoteCertificateNum>
          <Issuer>
            <xsl:value-of select="''"/>
          </Issuer>
          <IssuerName>
            <xsl:value-of select="''"/>
          </IssuerName>
          <PaperType>
            <xsl:value-of select="''"/>
          </PaperType>
          <DTCPaper>
            <xsl:value-of select="''"/>
          </DTCPaper>
          <Instruction_Confirmation>
            <xsl:value-of select="''"/>
          </Instruction_Confirmation>
          <Custody_NoCustody>
            <xsl:value-of select="''"/>
          </Custody_NoCustody>
          <Amendment>
            <xsl:value-of select="''"/>
          </Amendment>
          <NationalClearingSystemCode>
            <xsl:value-of select="''"/>
          </NationalClearingSystemCode>
          <UltimateBeneficiaryAccount>
            <xsl:value-of select="''"/>
          </UltimateBeneficiaryAccount>
          <AustraclearCode>
            <xsl:value-of select="''"/>
          </AustraclearCode>
          <BuySellIndicator>
            <xsl:value-of select="''"/>
          </BuySellIndicator>
          <FinancialIndicator>
            <xsl:value-of select="''"/>
          </FinancialIndicator>
          <OrderedCurrency>
            <xsl:value-of select="''"/>
          </OrderedCurrency>
          <CounterCurrency>
            <xsl:value-of select="''"/>
          </CounterCurrency>
          <OrderedAmount>
            <xsl:value-of select="''"/>
          </OrderedAmount>
          <OrderingPartyDeliveryAgentBIC>
            <xsl:value-of select="''"/>
          </OrderingPartyDeliveryAgentBIC>
          <ShortSaleCode>
            <xsl:value-of select="''"/>
          </ShortSaleCode>
          <RepurchaseType>
            <xsl:value-of select="''"/>
          </RepurchaseType>
          <RepoReference>
            <xsl:value-of select="''"/>
          </RepoReference>
          <SettlementType>
            <xsl:value-of select="''"/>
          </SettlementType>
          <RepoRate>
            <xsl:value-of select="''"/>
          </RepoRate>
          <ClearingBrokerType>
            <xsl:value-of select="''"/>
          </ClearingBrokerType>
          <ExpirationDate>
            <xsl:value-of select="''"/>
          </ExpirationDate>
          <ContractSize>
            <xsl:value-of select="''"/>
          </ContractSize>
          <OptionFutureSide>
            <xsl:value-of select="''"/>
          </OptionFutureSide>
          <OptionFutureType>
            <xsl:value-of select="''"/>
          </OptionFutureType>
          <FundType>
            <xsl:value-of select="''"/>
          </FundType>
          <StrikePrice>
            <xsl:value-of select="''"/>
          </StrikePrice>
          <FirstCouponDate>
            <xsl:value-of select="''"/>
          </FirstCouponDate>
          <PaymentFrequency>
            <xsl:value-of select="''"/>
          </PaymentFrequency>
          <ForfeitAmount>
            <xsl:value-of select="''"/>
          </ForfeitAmount>
          <PremiumAmount>
            <xsl:value-of select="''"/>
          </PremiumAmount>
          <TerminationAmount>
            <xsl:value-of select="''"/>
          </TerminationAmount>
          <VariableRateSupport>
            <xsl:value-of select="''"/>
          </VariableRateSupport>
          <MethodOfInterestComputation>
            <xsl:value-of select="''"/>
          </MethodOfInterestComputation>
          <RateType>
            <xsl:value-of select="''"/>
          </RateType>
          <Rate2>
            <xsl:value-of select="''"/>
          </Rate2>
          <RateValue>
            <xsl:value-of select="''"/>
          </RateValue>
          <OpenTermIndicator>
            <xsl:value-of select="''"/>
          </OpenTermIndicator>
          <ClosingSettlementDate>
            <xsl:value-of select="''"/>
          </ClosingSettlementDate>
          <PayChargesIndicator>
            <xsl:value-of select="''"/>
          </PayChargesIndicator>
          <TradeDepositoryIndicator>
            <xsl:value-of select="''"/>
          </TradeDepositoryIndicator>
          <StockLoanMarginRate>
            <xsl:value-of select="''"/>
          </StockLoanMarginRate>
          <TermDate>
            <xsl:value-of select="''"/>
          </TermDate>
          <PairOffIndicator>
            <xsl:value-of select="''"/>
          </PairOffIndicator>
          <PairOffTransactionNumber>
            <xsl:value-of select="''"/>
          </PairOffTransactionNumber>
          <CallDelay>
            <xsl:value-of select="''"/>
          </CallDelay>
          <OrderingInstitution>
            <xsl:value-of select="''"/>
          </OrderingInstitution>
          <IntraCompanyPayment>
            <xsl:value-of select="''"/>
          </IntraCompanyPayment>
          <FundManagerBIC>
            <xsl:value-of select="''"/>
          </FundManagerBIC>
          <FXCounterpartyBIC>
            <xsl:value-of select="''"/>
          </FXCounterpartyBIC>
          <CLSSettlementMemberBIC>
            <xsl:value-of select="''"/>
          </CLSSettlementMemberBIC>
          <BrokerSetupIndicator>
            <xsl:value-of select="''"/>
          </BrokerSetupIndicator>
          <BrokerABANumber>
            <xsl:value-of select="''"/>
          </BrokerABANumber>
          <BrokerThirdPartyMnemonic>
            <xsl:value-of select="''"/>
          </BrokerThirdPartyMnemonic>
          <PlaceofSettlementBIC>
            <xsl:value-of select="''"/>
          </PlaceofSettlementBIC>
          <StampKeyword>
            <xsl:value-of select="''"/>
          </StampKeyword>
          <BeneficiaryDeclaration>
            <xsl:value-of select="''"/>
          </BeneficiaryDeclaration>
          <StampableAmount>
            <xsl:value-of select="''"/>
          </StampableAmount>
          <StampAmountIndicator>
            <xsl:value-of select="''"/>
          </StampAmountIndicator>
          <PrincipalBrokerSafeAccount>
            <xsl:value-of select="''"/>
          </PrincipalBrokerSafeAccount>
          <ClearingBrokerSafeAccount>
            <xsl:value-of select="''"/>
          </ClearingBrokerSafeAccount>
          <SafekeepingAccount>
            <xsl:value-of select="''"/>
          </SafekeepingAccount>
          <ExpiryDate>
            <xsl:value-of select="''"/>
          </ExpiryDate>
          <PaymentType>
            <xsl:value-of select="''"/>
          </PaymentType>
          <OptionType>
            <xsl:value-of select="''"/>
          </OptionType>
          <OptionInstructionType>
            <xsl:value-of select="''"/>
          </OptionInstructionType>
          <DealPrice>
            <xsl:value-of select="''"/>
          </DealPrice>
          <ExercisePrice>
            <xsl:value-of select="''"/>
          </ExercisePrice>
          <FuturesInstructionType>
            <xsl:value-of select="''"/>
          </FuturesInstructionType>
          <AccountingOptions>
            <xsl:value-of select="''"/>
          </AccountingOptions>
          <PlaceOfTrade>
            <xsl:value-of select="''"/>
          </PlaceOfTrade>
          <ContractSizePrice>
            <xsl:value-of select="''"/>
          </ContractSizePrice>
          <ReasonCode>
            <xsl:value-of select="''"/>
          </ReasonCode>
          <SupplementaryInformation>
            <xsl:value-of select="''"/>
          </SupplementaryInformation>
          <PaymentIntermediaryNationalClearingSystemCode>
            <xsl:value-of select="''"/>
          </PaymentIntermediaryNationalClearingSystemCode>
          <PaymentIntermediaryBankBIC>
            <xsl:value-of select="''"/>
          </PaymentIntermediaryBankBIC>
          <PaymentIntermediaryBankNameAddress>
            <xsl:value-of select="''"/>
          </PaymentIntermediaryBankNameAddress>
          <PaymentIntermediaryBeneficiaryBankAcctNo>
            <xsl:value-of select="''"/>
          </PaymentIntermediaryBeneficiaryBankAcctNo>
          <TransfereeAddress2>
            <xsl:value-of select="''"/>
          </TransfereeAddress2>
          <TransfereeAddress3>
            <xsl:value-of select="''"/>
          </TransfereeAddress3>
          <TransfereeAddress4>
            <xsl:value-of select="''"/>
          </TransfereeAddress4>
          <ShippingAddress2>
            <xsl:value-of select="''"/>
          </ShippingAddress2>
          <ShippingAddress3>
            <xsl:value-of select="''"/>
          </ShippingAddress3>
          <ShippingAddress5>
            <xsl:value-of select="''"/>
          </ShippingAddress5>
          <CollateralStatutoryAsset>
            <xsl:value-of select="''"/>
          </CollateralStatutoryAsset>
          <StateCommonwealthTerritory>
            <xsl:value-of select="''"/>
          </StateCommonwealthTerritory>
          <ServiceablilityType>
            <xsl:value-of select="''"/>
          </ServiceablilityType>
          <FxOptions>
            <xsl:value-of select="''"/>
          </FxOptions>
          <LinkedForeignExchange>
            <xsl:value-of select="''"/>
          </LinkedForeignExchange>
          <BaseCurrency>
            <xsl:value-of select="''"/>
          </BaseCurrency>
          <DebitCashAccountNumber>
            <xsl:value-of select="''"/>
          </DebitCashAccountNumber>
          <CreditCashAccountNumber>
            <xsl:value-of select="''"/>
          </CreditCashAccountNumber>
        
      </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>