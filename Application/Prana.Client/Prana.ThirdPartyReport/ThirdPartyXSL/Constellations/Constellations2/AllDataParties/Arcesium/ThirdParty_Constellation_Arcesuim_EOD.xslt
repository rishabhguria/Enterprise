<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>


        <ArcesiumTradeSecurityLoader>
          <xsl:value-of select="'ArcesiumTradeSecurityLoader'"/>

        </ArcesiumTradeSecurityLoader>

        <OperationCode>
          <xsl:value-of select="'OperationCode'"/>
        </OperationCode>

        <TransactionType>
          <xsl:value-of select="'TransactionType'"/>
        </TransactionType>


        <ExternalID>
          <xsl:value-of select="'ExternalID'"/>
        </ExternalID>

        <Book>
          <xsl:value-of select="'Book'"/>
        </Book>

        <PrimeBroker>
          <xsl:value-of select="'PrimeBroker'"/>
        </PrimeBroker>

        <ExecutionBroker>
          <xsl:value-of select="'ExecutionBroker'"/>
        </ExecutionBroker>

        <Bundle>
          <xsl:value-of select="'Bundle'"/>
        </Bundle>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <TradeQuantity>
          <xsl:value-of select="'Trade Quantity'"/>
        </TradeQuantity>

        <TradePrice>
          <xsl:value-of select="'TradePrice'"/>
        </TradePrice>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <TradeTime>
          <xsl:value-of select="'TradeTime'"/>
        </TradeTime>

        <SettlementDate>
          <xsl:value-of select="'SettlementDate'"/>
        </SettlementDate>

        <SettleCurrency>
          <xsl:value-of select="'SettleCurrency'"/>
        </SettleCurrency>

        <FXRate>
          <xsl:value-of select="'FXRate'"/>
        </FXRate>


        <CustodianAccount>
          <xsl:value-of select="'CustodianAccount'"/>
        </CustodianAccount>


        <TradeDateFXrate>
          <xsl:value-of select="'TradeDateFXrate'"/>
        </TradeDateFXrate>

        <SettleDateFXRate>
          <xsl:value-of select="'SettleDateFXRate'"/>
        </SettleDateFXRate>

        <ActualSettleDate>
          <xsl:value-of select="'ActualSettleDate'"/>
        </ActualSettleDate>

        <FinancingType>
          <xsl:value-of select="'FinancingType'"/>
        </FinancingType>

        <FixingDate>
          <xsl:value-of select="'FixingDate'"/>
        </FixingDate>

        <Commissions>
          <xsl:value-of select="'Commissions'"/>
        </Commissions>

        <ClearingFee>
          <xsl:value-of select="'ClearingFee'"/>
        </ClearingFee>

        <ExchangeFee>
          <xsl:value-of select="'ExchangeFee'"/>
        </ExchangeFee>

        <Tax>
          <xsl:value-of select="'Tax'"/>
        </Tax>

        <TicketCharges>
          <xsl:value-of select="'TicketCharges'"/>
        </TicketCharges>

        <NFAFee>
          <xsl:value-of select="'NFAFee'"/>
        </NFAFee>

        <SecurityDescription>
          <xsl:value-of select="'SecurityDescription'"/>
        </SecurityDescription>

        <SecurityType>
          <xsl:value-of select="'SecurityType'"/>
        </SecurityType>

        <SecuritySubType>
          <xsl:value-of select="'SecuritySubType'"/>
        </SecuritySubType>

        <DeathDate>
          <xsl:value-of select="'DeathDate'"/>
        </DeathDate>

        <SecurityCurrency>
          <xsl:value-of select="'SecurityCurrency'"/>
        </SecurityCurrency>

        <TradingFactor>
          <xsl:value-of select="'TradingFactor'"/>
        </TradingFactor>

        <ExternalSecurityID>
          <xsl:value-of select="'ExternalSecurityID'"/>
        </ExternalSecurityID>


        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>


        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>


        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <RIC>
          <xsl:value-of select="'RIC'"/>
        </RIC>

        <BloombergTicker>
          <xsl:value-of select="'BloombergTicker'"/>
        </BloombergTicker>

        <BloombergUniqueID>
          <xsl:value-of select="'BloombergUniqueID'"/>
        </BloombergUniqueID>

        <BloombergFigi>
          <xsl:value-of select="'BloombergFigi'"/>
        </BloombergFigi>

        <WSOFacilityUniqueID>
          <xsl:value-of select="'WSOFacilityUniqueID'"/>
        </WSOFacilityUniqueID>

        <LoanxID>
          <xsl:value-of select="'LoanxID'"/>
        </LoanxID>

        <LinID>
          <xsl:value-of select="'LinID'"/>
        </LinID>

        <IDSource>
          <xsl:value-of select="'IDSource'"/>
        </IDSource>

        <SecurityID>
          <xsl:value-of select="'SecurityID'"/>
        </SecurityID>

        <RepoType>
          <xsl:value-of select="'RepoType'"/>
        </RepoType>

        <RepoCategory>
          <xsl:value-of select="'RepoCategory'"/>
        </RepoCategory>

        <CollateralSecurityType>
          <xsl:value-of select="'CollateralSecurityType'"/>
        </CollateralSecurityType>

        <CollateralISIN>
          <xsl:value-of select="'CollateralISIN'"/>
        </CollateralISIN>

        <CollateralCUSIP>
          <xsl:value-of select="'CollateralCUSIP'"/>
        </CollateralCUSIP>

        <RepoCounterparty>
          <xsl:value-of select="'RepoCounterparty'"/>
        </RepoCounterparty>

        <RepoCustodianAccount>
          <xsl:value-of select="'RepoCustodianAccount'"/>
        </RepoCustodianAccount>

        <OpenDate>
          <xsl:value-of select="'OpenDate'"/>
        </OpenDate>

        <CloseDate>
          <xsl:value-of select="'CloseDate'"/>
        </CloseDate>

        <OnMoney>
          <xsl:value-of select="'OnMoney'"/>
        </OnMoney>

        <RepoLoanAmount>
          <xsl:value-of select="'RepoLoanAmount'"/>
        </RepoLoanAmount>

        <HairCut>
          <xsl:value-of select="'HairCut'"/>
        </HairCut>

        <RepoRate>
          <xsl:value-of select="'RepoRate'"/>
        </RepoRate>

        <AccrueDate>
          <xsl:value-of select="'AccrueDate'"/>
        </AccrueDate>

        <BusinessDayConvention>
          <xsl:value-of select="'BusinessDayConvention'"/>
        </BusinessDayConvention>

        <ClearingMethod>
          <xsl:value-of select="'ClearingMethod'"/>
        </ClearingMethod>

        <RollConvention>
          <xsl:value-of select="'RollConvention'"/>
        </RollConvention>

        <SwapLevel>
          <xsl:value-of select="'SwapLevel'"/>
        </SwapLevel>

        <DayCountType>
          <xsl:value-of select="'DayCountType'"/>
        </DayCountType>

        <FirstCoupon>
          <xsl:value-of select="'FirstCoupon'"/>
        </FirstCoupon>

        <SwapFrequency>
          <xsl:value-of select="'SwapFrequency'"/>
        </SwapFrequency>

        <ResetFrequency>
          <xsl:value-of select="'ResetFrequency'"/>
        </ResetFrequency>

        <DayCountType2>
          <xsl:value-of select="'DayCountType2'"/>
        </DayCountType2>

        <Ratesource>
          <xsl:value-of select="'Ratesource'"/>
        </Ratesource>

        <Direction>
          <xsl:value-of select="'Direction'"/>
        </Direction>

        <Direction2>
          <xsl:value-of select="'Direction2'"/>
        </Direction2>

        <FirstCoupon2>
          <xsl:value-of select="'FirstCoupon2'"/>
        </FirstCoupon2>

        <Ratesource2>
          <xsl:value-of select="'Ratesource2'"/>
        </Ratesource2>

        <SwapFrequency2>
          <xsl:value-of select="'SwapFrequency2'"/>
        </SwapFrequency2>

        <ResetFrequency2>
          <xsl:value-of select="'ResetFrequency2'"/>
        </ResetFrequency2>

        <Spread>
          <xsl:value-of select="'Spread'"/>
        </Spread>

        <Spread2>
          <xsl:value-of select="'Spread2'"/>
        </Spread2>

        <NotionalExchangeType	>
          <xsl:value-of select="'NotionalExchangeType'"/>
        </NotionalExchangeType>

        <SecurityCurrency2>
          <xsl:value-of select="'SecurityCurrency2'"/>
        </SecurityCurrency2>

        <Notional2>
          <xsl:value-of select="'Notional2'"/>
        </Notional2>

        <IsIMM>
          <xsl:value-of select="'IsIMM'"/>
        </IsIMM>

        <ReferenceObligation>
          <xsl:value-of select="'ReferenceObligation'"/>
        </ReferenceObligation>

        <RedCode>
          <xsl:value-of select="'RedCode'"/>

        </RedCode>

        <RestructuringType>
          <xsl:value-of select="'RestructuringType'"/>
        </RestructuringType>

        <DefaultRecovery>
          <xsl:value-of select="'DefaultRecovery'"/>
        </DefaultRecovery>

        <DebtType>
          <xsl:value-of select="'DebtType'"/>
        </DebtType>

        <HolidayAdjustCouponAmount>
          <xsl:value-of select="'HolidayAdjustCouponAmount'"/>
        </HolidayAdjustCouponAmount>

        <UpfrontMarking>
          <xsl:value-of select="'UpfrontMarking'"/>
        </UpfrontMarking>

        <AmericanEuropeanIndicator>
          <xsl:value-of select="'AmericanEuropeanIndicator'"/>
        </AmericanEuropeanIndicator>

        <StrikePrice>
          <xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>

        <PutcallIndicator>
          <xsl:value-of select="'PutcallIndicator'"/>
        </PutcallIndicator>

        <ContractSize>
          <xsl:value-of select="'ContractSize'"/>
        </ContractSize>

        <CashorPhysical>
          <xsl:value-of select="'CashorPhysical'"/>
        </CashorPhysical>

        <WindowStart>
          <xsl:value-of select="'WindowStart'"/>
        </WindowStart>

        <WindowEnd>
          <xsl:value-of select="'WindowEnd'"/>
        </WindowEnd>

        <UpperBarrier>
          <xsl:value-of select="'UpperBarrier'"/>
        </UpperBarrier>

        <LowerBarrier>
          <xsl:value-of select="'LowerBarrier'"/>
        </LowerBarrier>

        <KnockinType>
          <xsl:value-of select="'KnockinType'"/>
        </KnockinType>

        <KnockOutType>
          <xsl:value-of select="'KnockOutType'"/>
        </KnockOutType>

        <OptionCutoffTime>
          <xsl:value-of select="'OptionCutoffTime'"/>
        </OptionCutoffTime>

        <OptionCutoffTimeZone>
          <xsl:value-of select="'OptionCutoffTimeZone'"/>
        </OptionCutoffTimeZone>

        <SecondUpperBarrier>
          <xsl:value-of select="'SecondUpperBarrier'"/>
        </SecondUpperBarrier>

        <SecondLowerBarrier>
          <xsl:value-of select="'SecondLowerBarrier'"/>
        </SecondLowerBarrier>

        <BarrierTouchCode>
          <xsl:value-of select="'BarrierTouchCode'"/>
        </BarrierTouchCode>

        <BarrierCode>
          <xsl:value-of select="'BarrierCode'"/>
        </BarrierCode>

        <ForwardBeginDate>
          <xsl:value-of select="'ForwardBeginDate'"/>
        </ForwardBeginDate>

        <ForwardEndDate>
          <xsl:value-of select="'ForwardEndDate'"/>
        </ForwardEndDate>

        <ForwardSettleDate>
          <xsl:value-of select="'ForwardSettleDate'"/>
        </ForwardSettleDate>

        <CommodityType>
          <xsl:value-of select="'CommodityType'"/>
        </CommodityType>

        <GMIProductCode>
          <xsl:value-of select="'GMIProductCode'"/>
        </GMIProductCode>

        <GMIExchangeCode>
          <xsl:value-of select="'GMIExchangeCode'"/>
        </GMIExchangeCode>

        <ExchangeProductCode>
          <xsl:value-of select="'ExchangeProductCode'"/>
        </ExchangeProductCode>

        <PeakType>
          <xsl:value-of select="'PeakType'"/>
        </PeakType>

        <EnergyUnits>
          <xsl:value-of select="'EnergyUnits'"/>
        </EnergyUnits>

        <U1.SecurityDescription>
          <xsl:value-of select="'U1.SecurityDescription'"/>
        </U1.SecurityDescription>

        <U1.SecurityType>
          <xsl:value-of select="'U1.SecurityType'"/>
        </U1.SecurityType>

        <U1.SecuritySubType>
          <xsl:value-of select="'U1.SecuritySubType'"/>
        </U1.SecuritySubType>

        <U1.DeathDate>
          <xsl:value-of select="'U1.DeathDate'"/>
        </U1.DeathDate>

        <U1.SecurityCurrency>
          <xsl:value-of select="'U1.SecurityCurrency'"/>
        </U1.SecurityCurrency>

        <U1.TradingFactor>
          <xsl:value-of select="'U1.TradingFactor'"/>
        </U1.TradingFactor>

        <U1.ExternalSecurityID>
          <xsl:value-of select="'U1.ExternalSecurityID'"/>
        </U1.ExternalSecurityID>

        <U1.CUSIP>
          <xsl:value-of select="'U1.CUSIP'"/>
        </U1.CUSIP>

        <U1.ISIN>
          <xsl:value-of select="'U1.ISIN'"/>
        </U1.ISIN>

        <U1.SEDOL>
          <xsl:value-of select="'U1.SEDOL'"/>
        </U1.SEDOL>


        <U1.RIC>
          <xsl:value-of select="'U1.RIC'"/>
        </U1.RIC>
        <U1.BloombergTicker>
          <xsl:value-of select="'U1.BloombergTicker'"/>
        </U1.BloombergTicker>

        <U1.BloombergUniqueID>
          <xsl:value-of select="'U1.BloombergUniqueID'"/>
        </U1.BloombergUniqueID>


        <U1.BloombergFigi>
          <xsl:value-of select="'U1.BloombergFigi'"/>
        </U1.BloombergFigi>

        <U1.AccrueDate>
          <xsl:value-of select="'U1.AccrueDate'"/>
        </U1.AccrueDate>

        <U1.BusinessDayConvention>
          <xsl:value-of select="'U1.BusinessDayConvention'"/>
        </U1.BusinessDayConvention>

        <U1.ClearingMethod>
          <xsl:value-of select="'U1.ClearingMethod'"/>
        </U1.ClearingMethod>

        <U1.RollConvention>
          <xsl:value-of select="'U1.RollConvention'"/>
        </U1.RollConvention>

        <U1.SwapLevel>
          <xsl:value-of select="'U1.SwapLevel'"/>
        </U1.SwapLevel>

        <U1.DayCountType>
          <xsl:value-of select="'U1.DayCountType'"/>
        </U1.DayCountType>

        <U1.Direction>
          <xsl:value-of select="'U1.Direction'"/>
        </U1.Direction>

        <U1.FirstCoupon>
          <xsl:value-of select="'U1.FirstCoupon'"/>
        </U1.FirstCoupon>


        <U1.RateSource>
          <xsl:value-of select="'U1.RateSource'"/>
        </U1.RateSource>

        <U1.SwapFrequency>
          <xsl:value-of select="'U1.SwapFrequency'"/>
        </U1.SwapFrequency>

        <U1.ResetFrequency>
          <xsl:value-of select="'U1.ResetFrequency'"/>
        </U1.ResetFrequency>

        <U1.DayCountType2>
          <xsl:value-of select="'U1.DayCountType2'"/>
        </U1.DayCountType2>

        <U1.Direction2>
          <xsl:value-of select="'U1.Direction2'"/>
        </U1.Direction2>

        <U1.FirstCoupon2>
          <xsl:value-of select="'U1.FirstCoupon2'"/>
        </U1.FirstCoupon2>

        <U1.Ratesource2>
          <xsl:value-of select="'U1.Ratesource2'"/>
        </U1.Ratesource2>

        <U1.SwapFrequency2>
          <xsl:value-of select="'U1.SwapFrequency2'"/>
        </U1.SwapFrequency2>

        <U1.ResetFrequency2>
          <xsl:value-of select="'U1.ResetFrequency2'"/>
        </U1.ResetFrequency2>

        <U1.Spread>
          <xsl:value-of select="'U1.Spread'"/>
        </U1.Spread>

        <U1.NotionalExchangeType>
          <xsl:value-of select="'U1.NotionalExchangeType'"/>
        </U1.NotionalExchangeType>

        <U1.ReferenceObligation>
          <xsl:value-of select="'U1.ReferenceObligation'"/>
        </U1.ReferenceObligation>

        <U1.RestructuringType>
          <xsl:value-of select="'U1.RestructuringType'"/>
        </U1.RestructuringType>

        <U1.DefaultRecovery>
          <xsl:value-of select="'U1.DefaultRecovery'"/>
        </U1.DefaultRecovery>

        <U1.RedCode>
          <xsl:value-of select="'U1.RedCode'"/>
        </U1.RedCode>

        <U1.DebtType>
          <xsl:value-of select="'U1.DebtType'"/>
        </U1.DebtType>

        <U1.HolidayAdjustCouponAmount>
          <xsl:value-of select="'U1.HolidayAdjustCouponAmount'"/>
        </U1.HolidayAdjustCouponAmount>


        <U1.UpfrontMarking>
          <xsl:value-of select="'U1.UpfrontMarking'"/>
        </U1.UpfrontMarking>

        <TradeConfirmationStatus>
          <xsl:value-of select="'TradeConfirmationStatus'"/>
        </TradeConfirmationStatus>

        <TradeNotes>
          <xsl:value-of select="'TradeNotes'"/>
        </TradeNotes>

        <TradeType>
          <xsl:value-of select="'TradeType'"/>
        </TradeType>

        <TransferType>
          <xsl:value-of select="'TransferType'"/>
        </TransferType>

        <Trader>
          <xsl:value-of select="'Trader'"/>
        </Trader>

        <ReceiveDatePrice>
          <xsl:value-of select="'ReceiveDatePrice'"/>
        </ReceiveDatePrice>

        <TaxLotDate>
          <xsl:value-of select="'TaxLotDate'"/>
        </TaxLotDate>


        <TaxLotDateTime>
          <xsl:value-of select="'TaxLotDateTime'"/>
        </TaxLotDateTime>

        <TaxLotSettleDate>
          <xsl:value-of select="'TaxLotSettleDate'"/>
        </TaxLotSettleDate>

        <TaxLotSettleDateTime>
          <xsl:value-of select="'TaxLotSettleDateTime'"/>
        </TaxLotSettleDateTime>

        <TaxLotActualSettleDate>
          <xsl:value-of select="'TaxLotActualSettleDate'"/>
        </TaxLotActualSettleDate>

        <TaxLotActualSettleDateTime>
          <xsl:value-of select="'TaxLotActualSettleDateTime'"/>
        </TaxLotActualSettleDateTime>

        <TaxLotCost>
          <xsl:value-of select="'TaxLotCost'"/>
        </TaxLotCost>

        <TaxLotPriceTax>
          <xsl:value-of select="'TaxLotPriceTax'"/>
        </TaxLotPriceTax>

        <LotFXRate>
          <xsl:value-of select="'LotFXRate'"/>
        </LotFXRate>


        <DestinationBundle>
          <xsl:value-of select="'DestinationBundle'"/>
        </DestinationBundle>

        <DestinationPB>
          <xsl:value-of select="'DestinationPB'"/>
        </DestinationPB>

        <DestinationCA>
          <xsl:value-of select="'DestinationCA'"/>
        </DestinationCA>

        <IndependentAmount>
          <xsl:value-of select="'IndependentAmount'"/>
        </IndependentAmount>

        <IAType>
          <xsl:value-of select="'IAType'"/>
        </IAType>

        <IACurrency>
          <xsl:value-of select="'IACurrency'"/>
        </IACurrency>

        <IAEffectiveDate>
          <xsl:value-of select="'IAEffectiveDate'"/>
        </IAEffectiveDate>

        <ImpliedCommissionFlag>
          <xsl:value-of select="'ImpliedCommissionFlag'"/>
        </ImpliedCommissionFlag>

        <ChargesinSecurityCurrency>
          <xsl:value-of select="'ChargesinSecurityCurrency'"/>
        </ChargesinSecurityCurrency>

        <NetConsideration>
          <xsl:value-of select="'NetConsideration'"/>
        </NetConsideration>

        <OMSTradingFactor>
          <xsl:value-of select="'OMSTradingFactor'"/>
        </OMSTradingFactor>

        <AccruedInterest>
          <xsl:value-of select="'AccruedInterest'"/>
        </AccruedInterest>

        <AIOverride>
          <xsl:value-of select="'AIOverride'"/>
        </AIOverride>

        <HardDollars>
          <xsl:value-of select="'HardDollars'"/>
        </HardDollars>

        <SoftDollars>
          <xsl:value-of select="'SoftDollars'"/>
        </SoftDollars>



        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Boothbay Swap-UBS' or AccountName='Boothbay - UBS' or AccountName='Boothbay - Wells Fargo']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          

          <ArcesiumTradeSecurityLoader>
            <xsl:choose>
              <xsl:when test="AccountName='Boothbay Swap-UBS'">
                <xsl:value-of select="'EquitySwap'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </ArcesiumTradeSecurityLoader>

          <OperationCode>
            <xsl:choose>
              <xsl:when test ="TaxLotState = 'Allocated'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Amended'">
                <xsl:value-of select="'A'"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Deleted'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
            </xsl:choose>
          </OperationCode>

          <TransactionType>
            <xsl:value-of select="'TRADE'"/>
          </TransactionType>


          <ExternalID>
            <xsl:value-of select="''"/>
          </ExternalID>

          <xsl:variable name="PB_NAME" select="'Arcesium'"/>

          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="Broker">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Book>
            <xsl:value-of select="''"/>
          </Book>

          <PrimeBroker>
            <xsl:value-of select="$Broker"/>
          </PrimeBroker>

          <ExecutionBroker>
            <xsl:value-of select="$Broker"/>
          </ExecutionBroker>

          <Bundle>
            <xsl:value-of select="Strategy"/>
          </Bundle>

          <Side>
            <xsl:choose>
              <xsl:when test ="Asset = 'EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'BTO'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'STC'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'STO'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BTC'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test ="Asset = 'Equity' or Asset = 'FixedIncome' or Asset = 'EquitySwap'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BC'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test ="Asset = 'FX' or Asset='Future'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <TradeQuantity>
            <xsl:value-of select="AllocatedQty"/>
          </TradeQuantity>
          
          <TradePrice>
            <xsl:value-of select="AveragePrice"/>
          </TradePrice>

          <TradeDate>
            <xsl:value-of select="concat(substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'),substring-after(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <TradeTime>
            <xsl:value-of select="substring-before(substring-after(TradeDateTime,' '),' ')"/>
          </TradeTime>

          <SettlementDate>
            <xsl:value-of select="concat(substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'),substring-after(substring-after(SettlementDate,'/'),'/'))"/>
          </SettlementDate>

          <SettleCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCurrency>

          <FXRate>            
            <xsl:value-of select="ForexRate"/>
          </FXRate>


          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <CustodianAccount>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
          </CustodianAccount>
          

          <TradeDateFXrate>
            <xsl:value-of select="FXRate_Taxlot"/>
          </TradeDateFXrate>

          <SettleDateFXRate>
            <xsl:value-of select="''"/>
          </SettleDateFXRate>

          <ActualSettleDate>
            <xsl:value-of select="''"/>
          </ActualSettleDate>

          <FinancingType>
            <xsl:value-of select="''"/>
          </FinancingType>

          <FixingDate>
            <xsl:value-of select="''"/>
          </FixingDate>

          <Commissions>
            <xsl:choose>
              <xsl:when test="number(CommissionCharged)">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Commissions>

          <ClearingFee>
            <xsl:value-of select="ClearingFee"/>
          </ClearingFee>

          <ExchangeFee>
            <xsl:value-of select="''"/>
          </ExchangeFee>

          <Tax>
            <xsl:value-of select="SecFee"/>
          </Tax>

          <TicketCharges>
            <xsl:value-of select="(ClearingFee + TaxOnCommissions + MiscFees + OrfFee + TransactionLevy + StampDuty)"/>
          </TicketCharges>

          <NFAFee>
            <xsl:value-of select="''"/>
          </NFAFee>

          <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>

          <SecurityType>
            <xsl:choose>
              <xsl:when test="AccountName='Boothbay Swap-UBS'">
                <xsl:value-of select="'EquitySwap'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityType>

          <SecuritySubType>
            <xsl:value-of select="''"/>
          </SecuritySubType>

          <DeathDate>
            <xsl:value-of select="''"/>
          </DeathDate>

          <SecurityCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </SecurityCurrency>

          <TradingFactor>
            <xsl:value-of select="''"/>
          </TradingFactor>

          <ExternalSecurityID>
            <xsl:value-of select="''"/>
          </ExternalSecurityID>



          <xsl:variable name = "PRANA_CUSIP" >
            <xsl:value-of select="Symbol"/>
          </xsl:variable>

          <xsl:variable name="PB_CUSIP_MAPPING">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SecurityID_Mapping.xml')/SecurityMapping/PB[@Name=$PB_NAME]/SymbolData[@PranaSymbol = $PRANA_CUSIP]/@ThirdPartyCUSIP"/>
          </xsl:variable>
          <CUSIP>
            <xsl:choose>
              <!-- <xsl:when test="AccountName='Boothbay Swap-UBS'"> -->
			  <xsl:when test="$PB_CUSIP_MAPPING !='' or $PB_CUSIP_MAPPING !='*'">
                <xsl:value-of select="$PB_CUSIP_MAPPING"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CUSIP"/>
              </xsl:otherwise>
            </xsl:choose>
          </CUSIP>



          <xsl:variable name = "PRANA_ISIN" >
            <xsl:value-of select="Symbol"/>
          </xsl:variable>

          <xsl:variable name="PB_ISIN_MAPPING">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SecurityID_Mapping.xml')/SecurityMapping/PB[@Name=$PB_NAME]/SymbolData[@PranaSymbol = $PRANA_ISIN]/@ThirdPartyISIN"/>
          </xsl:variable>
          <ISIN>
            <xsl:choose>
              <xsl:when test="$PB_ISIN_MAPPING !='' or $PB_ISIN_MAPPING !='*'">
                <xsl:value-of select="$PB_ISIN_MAPPING"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ISIN"/>
              </xsl:otherwise>
            </xsl:choose>
          </ISIN>

          <xsl:variable name = "PRANA_SEDOL" >
            <xsl:value-of select="Symbol"/>
          </xsl:variable>

          <xsl:variable name="PB_SEDOL_MAPPING">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SecurityID_Mapping.xml')/SecurityMapping/PB[@Name=$PB_NAME]/SymbolData[@PranaSymbol = $PRANA_SEDOL]/@ThirdPartySEDOL"/>
          </xsl:variable>

          <SEDOL>
            <xsl:choose>
              <xsl:when test="$PB_SEDOL_MAPPING !='' or $PB_SEDOL_MAPPING !='*'">
                <xsl:value-of select="$PB_SEDOL_MAPPING"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="SEDOL"/>
              </xsl:otherwise>
            </xsl:choose>
          </SEDOL>

          <RIC>
            <xsl:value-of select="RIC"/>
          </RIC>

          <BloombergTicker>
            <xsl:value-of select="BBCode"/>
          </BloombergTicker>

          <BloombergUniqueID>
            <xsl:value-of select="''"/>
          </BloombergUniqueID>

          <BloombergFigi>
            <xsl:value-of select="''"/>
          </BloombergFigi>

          <WSOFacilityUniqueID>
            <xsl:value-of select="''"/>
          </WSOFacilityUniqueID>

          <LoanxID>
            <xsl:value-of select="''"/>
          </LoanxID>

          <LinID>
            <xsl:value-of select="''"/>
          </LinID>

          <IDSource>
            <xsl:value-of select="''"/>
          </IDSource>

          <SecurityID>
            <xsl:value-of select="''"/>
          </SecurityID>

          <RepoType>
            <xsl:value-of select="''"/>
          </RepoType>

          <RepoCategory>
            <xsl:value-of select="''"/>
          </RepoCategory>

          <CollateralSecurityType>
            <xsl:value-of select="''"/>
          </CollateralSecurityType>

          <CollateralISIN>
            <xsl:value-of select="''"/>
          </CollateralISIN>

          <CollateralCUSIP>
            <xsl:value-of select="''"/>
          </CollateralCUSIP>

          <RepoCounterparty>
            <xsl:value-of select="''"/>
          </RepoCounterparty>

          <RepoCustodianAccount>
            <xsl:value-of select="''"/>
          </RepoCustodianAccount>

          <OpenDate>
            <xsl:value-of select="''"/>
          </OpenDate>

          <CloseDate>
            <xsl:value-of select="''"/>
          </CloseDate>

          <OnMoney>
            <xsl:value-of select="''"/>
          </OnMoney>

          <RepoLoanAmount>
            <xsl:value-of select="''"/>
          </RepoLoanAmount>

          <HairCut>
            <xsl:value-of select="''"/>
          </HairCut>

          <RepoRate>
            <xsl:value-of select="''"/>
          </RepoRate>

          <AccrueDate>
            <xsl:value-of select="''"/>
          </AccrueDate>

          <BusinessDayConvention>
            <xsl:value-of select="''"/>
          </BusinessDayConvention>

          <ClearingMethod>
            <xsl:value-of select="''"/>
          </ClearingMethod>

          <RollConvention>
            <xsl:value-of select="''"/>
          </RollConvention>

          <SwapLevel>
            <xsl:value-of select="''"/>
          </SwapLevel>

          <DayCountType>
            <xsl:value-of select="''"/>
          </DayCountType>

          <FirstCoupon>
            <xsl:value-of select="''"/>
          </FirstCoupon>

          <SwapFrequency>
            <xsl:value-of select="''"/>
          </SwapFrequency>

          <ResetFrequency>
            <xsl:value-of select="''"/>
          </ResetFrequency>

          <DayCountType2>
            <xsl:value-of select="''"/>
          </DayCountType2>

          <Ratesource>
            <xsl:value-of select="''"/>
          </Ratesource>

          <Direction>
            <xsl:value-of select="''"/>
          </Direction>

          <Direction2>
            <xsl:value-of select="''"/>
          </Direction2>

          <FirstCoupon2>
            <xsl:value-of select="''"/>
          </FirstCoupon2>

          <Ratesource2>
            <xsl:value-of select="''"/>
          </Ratesource2>

          <SwapFrequency2>
            <xsl:value-of select="''"/>
          </SwapFrequency2>

          <ResetFrequency2>
            <xsl:value-of select="''"/>
          </ResetFrequency2>

          <Spread>
            <xsl:value-of select="''"/>
          </Spread>

          <Spread2>
            <xsl:value-of select="''"/>
          </Spread2>

          <NotionalExchangeType	>
            <xsl:value-of select="''"/>
          </NotionalExchangeType>

          <SecurityCurrency2>
            <xsl:value-of select="''"/>
          </SecurityCurrency2>

          <Notional2>
            <xsl:value-of select="''"/>
          </Notional2>

          <IsIMM>
            <xsl:value-of select="''"/>
          </IsIMM>

          <ReferenceObligation>
            <xsl:value-of select="''"/>
          </ReferenceObligation>

          <RedCode>
            <xsl:value-of select="''"/>
            
          </RedCode>

          <RestructuringType>
            <xsl:value-of select="''"/>
          </RestructuringType>

          <DefaultRecovery>
            <xsl:value-of select="''"/>
          </DefaultRecovery>

          <DebtType>
            <xsl:value-of select="''"/>
          </DebtType>

          <HolidayAdjustCouponAmount>
            <xsl:value-of select="''"/>
          </HolidayAdjustCouponAmount>

          <UpfrontMarking>
            <xsl:value-of select="''"/>
          </UpfrontMarking>

          <AmericanEuropeanIndicator>
            <xsl:value-of select="''"/>
          </AmericanEuropeanIndicator>

          <StrikePrice>
            <xsl:value-of select="''"/>
          </StrikePrice>

          <PutcallIndicator>
            <xsl:value-of select="''"/>
          </PutcallIndicator>

          <ContractSize>
            <xsl:value-of select="''"/>
          </ContractSize>

          <CashorPhysical>
            <xsl:value-of select="''"/>
          </CashorPhysical>

          <WindowStart>
            <xsl:value-of select="''"/>
          </WindowStart>

          <WindowEnd>
            <xsl:value-of select="''"/>
          </WindowEnd>

          <UpperBarrier>
            <xsl:value-of select="''"/>
          </UpperBarrier>

          <LowerBarrier>
            <xsl:value-of select="''"/>
          </LowerBarrier>

          <KnockinType>
            <xsl:value-of select="''"/>
          </KnockinType>

          <KnockOutType>
            <xsl:value-of select="''"/>
          </KnockOutType>

          <OptionCutoffTime>
            <xsl:value-of select="''"/>
          </OptionCutoffTime>

          <OptionCutoffTimeZone>
            <xsl:value-of select="''"/>
          </OptionCutoffTimeZone>

          <SecondUpperBarrier>
            <xsl:value-of select="''"/>
          </SecondUpperBarrier>

          <SecondLowerBarrier>
            <xsl:value-of select="''"/>
          </SecondLowerBarrier>

          <BarrierTouchCode>
            <xsl:value-of select="''"/>
          </BarrierTouchCode>

          <BarrierCode>
            <xsl:value-of select="''"/>
          </BarrierCode>

          <ForwardBeginDate>
            <xsl:value-of select="''"/>
          </ForwardBeginDate>

          <ForwardEndDate>
            <xsl:value-of select="''"/>
          </ForwardEndDate>

          <ForwardSettleDate>
            <xsl:value-of select="''"/>
          </ForwardSettleDate>

          <CommodityType>
            <xsl:value-of select="''"/>
          </CommodityType>

          <GMIProductCode>
            <xsl:value-of select="''"/>
          </GMIProductCode>

          <GMIExchangeCode>
            <xsl:value-of select="''"/>
          </GMIExchangeCode>

          <ExchangeProductCode>
            <xsl:value-of select="''"/>
          </ExchangeProductCode>

          <PeakType>
            <xsl:value-of select="''"/>
          </PeakType>

          <EnergyUnits>
            <xsl:value-of select="''"/>
          </EnergyUnits>

          <U1.SecurityDescription>
            <xsl:value-of select="''"/>
          </U1.SecurityDescription>

          <U1.SecurityType>
            <xsl:value-of select="''"/>
          </U1.SecurityType>

          <U1.SecuritySubType>
            <xsl:value-of select="''"/>
          </U1.SecuritySubType>

          <U1.DeathDate>
            <xsl:value-of select="''"/>
          </U1.DeathDate>

          <U1.SecurityCurrency>
            <xsl:value-of select="''"/>
          </U1.SecurityCurrency>

          <U1.TradingFactor>
            <xsl:value-of select="''"/>
          </U1.TradingFactor>

          <U1.ExternalSecurityID>
            <xsl:value-of select="''"/>
          </U1.ExternalSecurityID>

          <U1.CUSIP>
            <xsl:value-of select="''"/>
          </U1.CUSIP>

          <U1.ISIN>
            <xsl:value-of select="''"/>
          </U1.ISIN>

          <U1.SEDOL>
            <xsl:value-of select="''"/>
          </U1.SEDOL>


          <U1.RIC>
            <xsl:value-of select="''"/>
          </U1.RIC>
          <U1.BloombergTicker>
            <xsl:value-of select="''"/>
          </U1.BloombergTicker>

          <U1.BloombergUniqueID>
            <xsl:value-of select="''"/>
          </U1.BloombergUniqueID>


          <U1.BloombergFigi>
            <xsl:value-of select="''"/>
          </U1.BloombergFigi>

          <U1.AccrueDate>
            <xsl:value-of select="''"/>
          </U1.AccrueDate>

          <U1.BusinessDayConvention>
            <xsl:value-of select="''"/>
          </U1.BusinessDayConvention>

          <U1.ClearingMethod>
            <xsl:value-of select="''"/>
          </U1.ClearingMethod>

          <U1.RollConvention>
            <xsl:value-of select="''"/>
          </U1.RollConvention>

          <U1.SwapLevel>
            <xsl:value-of select="''"/>
          </U1.SwapLevel>

          <U1.DayCountType>
            <xsl:value-of select="''"/>
          </U1.DayCountType>

          <U1.Direction>
            <xsl:value-of select="''"/>
          </U1.Direction>

          <U1.FirstCoupon>
            <xsl:value-of select="''"/>
          </U1.FirstCoupon>


          <U1.RateSource>
            <xsl:value-of select="''"/>
          </U1.RateSource>

          <U1.SwapFrequency>
            <xsl:value-of select="''"/>
          </U1.SwapFrequency>

          <U1.ResetFrequency>
            <xsl:value-of select="''"/>
          </U1.ResetFrequency>

          <U1.DayCountType2>
            <xsl:value-of select="''"/>
          </U1.DayCountType2>

          <U1.Direction2>
            <xsl:value-of select="''"/>
          </U1.Direction2>

          <U1.FirstCoupon2>
            <xsl:value-of select="''"/>
          </U1.FirstCoupon2>

          <U1.Ratesource2>
            <xsl:value-of select="''"/>
          </U1.Ratesource2>

          <U1.SwapFrequency2>
            <xsl:value-of select="''"/>
          </U1.SwapFrequency2>

          <U1.ResetFrequency2>
            <xsl:value-of select="''"/>
          </U1.ResetFrequency2>

          <U1.Spread>
            <xsl:value-of select="''"/>
          </U1.Spread>

          <U1.NotionalExchangeType>
            <xsl:value-of select="''"/>
          </U1.NotionalExchangeType>

          <U1.ReferenceObligation>
            <xsl:value-of select="''"/>
          </U1.ReferenceObligation>

          <U1.RestructuringType>
            <xsl:value-of select="''"/>
          </U1.RestructuringType>
            
            <U1.DefaultRecovery>
              <xsl:value-of select="''"/>
            </U1.DefaultRecovery>

          <U1.RedCode>
            <xsl:value-of select="''"/>
          </U1.RedCode>

          <U1.DebtType>
            <xsl:value-of select="''"/>
          </U1.DebtType>

          <U1.HolidayAdjustCouponAmount>
            <xsl:value-of select="''"/>
          </U1.HolidayAdjustCouponAmount>


          <U1.UpfrontMarking>
            <xsl:value-of select="''"/>
          </U1.UpfrontMarking>

          <TradeConfirmationStatus>
            <xsl:value-of select="''"/>
          </TradeConfirmationStatus>

          <TradeNotes>
            <xsl:value-of select="''"/>
          </TradeNotes>

          <TradeType>
            <xsl:value-of select="''"/>
          </TradeType>

          <TransferType>
            <xsl:value-of select="''"/>
          </TransferType>

          <Trader>
            <xsl:value-of select="''"/>
          </Trader>

          <ReceiveDatePrice>
            <xsl:value-of select="''"/>
          </ReceiveDatePrice>

          <TaxLotDate>
            <xsl:value-of select="''"/>
          </TaxLotDate>


          <TaxLotDateTime>
            <xsl:value-of select="''"/>
          </TaxLotDateTime>

          <TaxLotSettleDate>
            <xsl:value-of select="''"/>
          </TaxLotSettleDate>

          <TaxLotSettleDateTime>
            <xsl:value-of select="''"/>
          </TaxLotSettleDateTime>

          <TaxLotActualSettleDate>
            <xsl:value-of select="''"/>
          </TaxLotActualSettleDate>

          <TaxLotActualSettleDateTime>
            <xsl:value-of select="''"/>
          </TaxLotActualSettleDateTime>

          <TaxLotCost>
            <xsl:value-of select="''"/>
          </TaxLotCost>

          <TaxLotPriceTax>
            <xsl:value-of select="''"/>
          </TaxLotPriceTax>

          <LotFXRate>
            <xsl:value-of select="''"/>
          </LotFXRate>


          <DestinationBundle>
            <xsl:value-of select="''"/>
          </DestinationBundle>

          <DestinationPB>
            <xsl:value-of select="''"/>
          </DestinationPB>

          <DestinationCA>
            <xsl:value-of select="''"/>
          </DestinationCA>

          <IndependentAmount>
            <xsl:value-of select="''"/>
          </IndependentAmount>

          <IAType>
            <xsl:value-of select="''"/>
          </IAType>

          <IACurrency>
            <xsl:value-of select="''"/>
          </IACurrency>

          <IAEffectiveDate>
            <xsl:value-of select="''"/>
          </IAEffectiveDate>

          <ImpliedCommissionFlag>
            <xsl:value-of select="''"/>
          </ImpliedCommissionFlag>

          <ChargesinSecurityCurrency>
            <xsl:value-of select="''"/>
          </ChargesinSecurityCurrency>

          <NetConsideration>
            <xsl:value-of select="NetAmount"/>
          </NetConsideration>

          <OMSTradingFactor>
            <xsl:value-of select="''"/>
          </OMSTradingFactor>

          <AccruedInterest>
            <xsl:value-of select="''"/>
          </AccruedInterest>

          <AIOverride>
            <xsl:value-of select="''"/>
          </AIOverride>

          <HardDollars>
            <xsl:value-of select="''"/>
          </HardDollars>

          <SoftDollars>
            <xsl:value-of select="''"/>
          </SoftDollars>



          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>