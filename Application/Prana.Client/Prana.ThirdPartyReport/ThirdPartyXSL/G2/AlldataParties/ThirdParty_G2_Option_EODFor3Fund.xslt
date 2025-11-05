<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:template name="DateFormat">
<xsl:param name="Date"/>
<xsl:value-of select="concat(substring-before(substring-after($Date,'/'),'/'),'/',substring-before($Date,'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
</xsl:template>
<xsl:template match="/ThirdPartyFlatFileDetailCollection">
<ThirdPartyFlatFileDetailCollection>
  <ThirdPartyFlatFileDetail>
    
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>
    <!--  system inetrnal use -->
<TaxLotState>
<xsl:value-of select="TaxLotState"/>
</TaxLotState>
<EntityID>
<xsl:value-of select="EntityID"/>
</EntityID>
    <TradeDate>
      <xsl:value-of select="'TradeDate'"/>
    </TradeDate>
    <SettleDate>
      <xsl:value-of select="'SettleDate'"/>
    </SettleDate>
    <AccountNumber>
      <xsl:value-of select="'AccountNumber'"/>
    </AccountNumber>
    <Broker>
      <xsl:value-of select="'Broker'"/>
    </Broker>
    <TransactionType>
      <xsl:value-of select="'TransactionType'"/>
    </TransactionType>
    <Qunatity>
      <xsl:value-of select="'Quantity'"/>
    </Qunatity>
    <TradeCCY>
      <xsl:value-of select="'TradeCCY'"/>
    </TradeCCY>
    <Price>
      <xsl:value-of select="'Price'"/>
    </Price>
    <SettleCCY>
      <xsl:value-of select="'SettleCCY'"/>
    </SettleCCY>
    <TradeFXRate>
      <xsl:value-of select="'TradeFXRate'"/>
    </TradeFXRate>
    <Identifier>
      <xsl:value-of select="'Identifier'"/>
    </Identifier>
    <IdentifierType>
      <xsl:value-of select="'IdentifierType'"/>
    </IdentifierType>
    <Multiplier>
      <xsl:value-of select="'Multiplier'"/>
    </Multiplier>
    <OptionType>
      <xsl:value-of select="'OptionType'"/>
    </OptionType>
    <ExpirtDate>
      <xsl:value-of select="'ExpirtDate'"/>
    </ExpirtDate>
    <StrikePrice>
      <xsl:value-of select="'StrikePrice'"/>
    </StrikePrice>
    <ExerciseType>
      <xsl:value-of select="'ExerciseType'"/>
    </ExerciseType>

    <BarrierType>
      <xsl:value-of select="'BarrierType'"/>
    </BarrierType>

    <BarrierLowerLevel>
      <xsl:value-of select="'BarrierLowerLevel'"/>
    </BarrierLowerLevel>

    <BarrierUpperLevel>
      <xsl:value-of select="'BarrierUpperLevel'"/>
    </BarrierUpperLevel>

    <TriggerDate>
      <xsl:value-of select="'TriggerDate'"/>
    </TriggerDate>

    <BarrierEffectiveDate>
      <xsl:value-of select="'BarrierEffectiveDate'"/>
    </BarrierEffectiveDate>

    <BarrierTerminationDate>
      <xsl:value-of select="'BarrierTerminationDate'"/>
    </BarrierTerminationDate>

    <BookPath>
      <xsl:value-of select="'BookPath'"/>
    </BookPath>
    <OtherPayments1>
      <xsl:value-of select="'OtherPayments1'"/>
    </OtherPayments1>

    <OtherPayments1Type>
      <xsl:value-of select="'OtherPayments1Type'"/>
    </OtherPayments1Type>

    <OtherPayments2>
      <xsl:value-of select="'OtherPayments2'"/>
    </OtherPayments2>

    <OtherPayments2Type>
      <xsl:value-of select="'OtherPayments2Type'"/>
    </OtherPayments2Type>

    <OtherPayments3>
      <xsl:value-of select="'OtherPayments3'"/>
    </OtherPayments3>

    <OtherPayments3Type>
      <xsl:value-of select="'OtherPayments3Type'"/>
    </OtherPayments3Type>

    <OtherPayments4>
      <xsl:value-of select="'OtherPayments4'"/>
    </OtherPayments4>

    <OtherPayments4Type>
      <xsl:value-of select="'OtherPayments4Type'"/>
    </OtherPayments4Type>

    <TraderName>
      <xsl:value-of select="'TraderName'"/>
    </TraderName>

    <TraderType>
      <xsl:value-of select="'TraderType'"/>
    </TraderType>

    <TRSId>
      <xsl:value-of select="'TRSId'"/>
    </TRSId>

    <BookId>
      <xsl:value-of select="'BookId'"/>
    </BookId>

    <ExternalReference>
      <xsl:value-of select="'ExternalReference'"/>
    </ExternalReference>

    <Notes>
      <xsl:value-of select="'Notes'"/>
    </Notes>

    <BookingStatus>
      <xsl:value-of select="'BookingStatus'"/>
    </BookingStatus>

    <TradeStatus>
      <xsl:value-of select="'TradeStatus'"/>
    </TradeStatus>
    
    <SettlementInstructionId>
      <xsl:value-of select="'SettlementInstructionId'"/>
    </SettlementInstructionId>
  </ThirdPartyFlatFileDetail>
<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption' and (AccountName='MS Investment Partners LP' or AccountName='MS Investment Partners QP' or AccountName='Quantum Partners LP')]">
<ThirdPartyFlatFileDetail>
  
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>
<!--  system inetrnal use -->
<TaxLotState>
<xsl:value-of select="TaxLotState"/>
</TaxLotState>
<EntityID>
<xsl:value-of select="EntityID"/>
</EntityID>
<xsl:variable name="varTradeDate">
<xsl:call-template name="DateFormat">
<xsl:with-param name="Date" select="TradeDate"/>
</xsl:call-template>
</xsl:variable>
<TradeDate>
<xsl:value-of select="TradeDate"/>
</TradeDate>
<xsl:variable name="varSettleDate">
<xsl:call-template name="DateFormat">
<xsl:with-param name="Date" select="SettlementDate"/>
</xsl:call-template>
</xsl:variable>
<SettleDate>
<xsl:value-of select="SettlementDate"/>
</SettleDate>
<xsl:variable name="varAccountNumber">
<xsl:value-of select="AccountName"/>
</xsl:variable>
<AccountNumber>
<xsl:value-of select="$varAccountNumber"/>
</AccountNumber>
<xsl:variable name="varBroker">
<xsl:value-of select="CounterParty"/>
</xsl:variable>
<Broker>
<xsl:value-of select="$varBroker"/>
</Broker>
<xsl:variable name="varTransactionType">
<xsl:value-of select="TransactionType"/>
</xsl:variable>
<TransactionType>
<xsl:value-of select="$varTransactionType"/>
</TransactionType>
<xsl:variable name="varQuantity">
<xsl:value-of select="AllocatedQty"/>
</xsl:variable>
<Qunatity>
<xsl:value-of select="$varQuantity"/>
</Qunatity>
<xsl:variable name="varCurrency">
<xsl:value-of select="CurrencySymbol"/>
</xsl:variable>
<TradeCCY>
<xsl:value-of select="$varCurrency"/>
</TradeCCY>
<xsl:variable name="varPrice">
<xsl:value-of select="AveragePrice"/>
</xsl:variable>
<Price>
<xsl:value-of select="$varPrice"/>
</Price>
<xsl:variable name="varSettleCurrency">
<xsl:value-of select="SettlCurrency"/>
</xsl:variable>
<SettleCCY>
<xsl:value-of select="$varSettleCurrency"/>
</SettleCCY>
<xsl:variable name="varTradeFXRate">
<xsl:value-of select="ForexRate_Trade"/>
</xsl:variable>
<TradeFXRate>
<xsl:value-of select="$varTradeFXRate"/>
</TradeFXRate>
<xsl:variable name="varBloomberg">
<xsl:value-of select="OSIOptionSymbol"/>
</xsl:variable>
<Identifier>
<xsl:value-of select="$varBloomberg"/>
</Identifier>
<IdentifierType>
<xsl:value-of select="'OSIOptionSymbol'"/>
</IdentifierType>
<xsl:variable name="varMultiplier">
<xsl:value-of select="AssetMultiplier"/>
</xsl:variable>
<Multiplier>
<xsl:value-of select="$varMultiplier"/>
</Multiplier>
<xsl:variable name="varPutOrCall">
<xsl:value-of select="PutOrCall"/>
</xsl:variable>
<OptionType>
<xsl:value-of select="$varPutOrCall"/>
</OptionType>
<xsl:variable name="varExpirationDate">
<xsl:value-of select="ExpirationDate"/>
</xsl:variable>
<ExpirtDate>
<xsl:value-of select="$varExpirationDate"/>
</ExpirtDate>
<xsl:variable name="varStrikePrice">
<xsl:value-of select="StrikePrice"/>
</xsl:variable>
<StrikePrice>
<xsl:value-of select="$varStrikePrice"/>
</StrikePrice>
<ExerciseType>
<xsl:value-of select="'American'"/>
</ExerciseType>

<BarrierType>
<xsl:value-of select="''"/>
</BarrierType>
  
<BarrierLowerLevel>
<xsl:value-of select="''"/>
</BarrierLowerLevel>

<BarrierUpperLevel>
<xsl:value-of select="''"/>
</BarrierUpperLevel>
  
<TriggerDate>
<xsl:value-of select="''"/>
</TriggerDate>
  
<BarrierEffectiveDate>
<xsl:value-of select="''"/>
</BarrierEffectiveDate>

<BarrierTerminationDate>
<xsl:value-of select="''"/>
</BarrierTerminationDate>
  
<BookPath>
<xsl:value-of select="''"/>
</BookPath>
    <xsl:variable name="varCommission">
        <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
      </xsl:variable>
      <OtherPayments1>
            <xsl:value-of select="$varCommission"/>
          </OtherPayments1>
          
          <OtherPayments1Type>
            <xsl:value-of select="'Flat Commission'"/>
          </OtherPayments1Type>

      <xsl:variable name="varFees">
        <xsl:value-of select="SecFees + OtherBrokerFee + ClearingBrokerFee + MiscFees + SecFee + OccFee + OrfFee + ClearingFee + TaxOnCommissions + StampDuty + TransactionLevy"/>
      </xsl:variable>
  
          <OtherPayments2>
            <xsl:value-of select="$varFees"/>
          </OtherPayments2>
  
          <OtherPayments2Type>
            <xsl:value-of select="'Other Fees'"/>
          </OtherPayments2Type>

          <OtherPayments3>
            <xsl:value-of select="''"/>
          </OtherPayments3>

          <OtherPayments3Type>
            <xsl:value-of select="''"/>
          </OtherPayments3Type>

          <OtherPayments4>
            <xsl:value-of select="''"/>
          </OtherPayments4>

          <OtherPayments4Type>
            <xsl:value-of select="''"/>
          </OtherPayments4Type>

          <TraderName>
            <xsl:value-of select="''"/>
          </TraderName>

          <TraderType>
            <xsl:value-of select="''"/>
          </TraderType>

          <TRSId>
            <xsl:value-of select="''"/>
          </TRSId>

          <BookId>
            <xsl:value-of select="''"/>
          </BookId>

          <ExternalReference>
            <xsl:value-of select="''"/>
          </ExternalReference>

          <Notes>
            <xsl:value-of select="''"/>
          </Notes>
  
         <BookingStatus>
            <xsl:value-of select="''"/>
          </BookingStatus>
  
            <TradeStatus>
              <xsl:choose>
                <xsl:when test="TaxLotStateID=0">
                  <xsl:value-of select="'New'"/>
                </xsl:when>
                <xsl:when test="TaxLotStateID=1">
                  <xsl:value-of select="'Send'"/>
                </xsl:when>
                <xsl:when test="TaxLotStateID=2">
                  <xsl:value-of select="'Correction'"/>
                </xsl:when>
                <xsl:when test="TaxLotStateID=3">
                  <xsl:value-of select="'Cancel'"/>
                </xsl:when>
                <xsl:when test="TaxLotStateID=4">
                  <xsl:value-of select="'Ignore'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TradeStatus>
          <SettlementInstructionId>
            <xsl:value-of select="''"/>
          </SettlementInstructionId>

</ThirdPartyFlatFileDetail>
</xsl:for-each>
</ThirdPartyFlatFileDetailCollection>
</xsl:template>
</xsl:stylesheet>