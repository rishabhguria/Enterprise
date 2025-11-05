<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'/'),'/'),'/',substring-before($Date,'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
	

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption']">
        <ThirdPartyFlatFileDetail>
		 <!-- system inetrnal use-->
		<TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
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
            <xsl:value-of select="$varTradeDate"/>
          </TradeDate>

          <xsl:variable name="varSettleDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate"/>
            </xsl:call-template>
          </xsl:variable>
          <SettleDate>
            <xsl:value-of select="$varSettleDate"/>
          </SettleDate>

          <xsl:variable name ="varAccountNumber">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <AccountNumber>
            <xsl:value-of select="$varAccountNumber"/>
          </AccountNumber>

          <xsl:variable name ="varBroker">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <Broker>
            <xsl:value-of select="$varBroker"/>
          </Broker>

          <xsl:variable name ="varTransactionType">
            <xsl:value-of select="TransactionType"/>
          </xsl:variable>
          <TransactionType>
            <xsl:value-of select="$varTransactionType"/>
          </TransactionType>
          
          <xsl:variable name ="varQuantity">
            <xsl:value-of select="AllocatedQty"/>
          </xsl:variable>
          <Qunatity>
            <xsl:value-of select="$varQuantity"/>
          </Qunatity>

          <xsl:variable name ="varCurrency">
            <xsl:value-of select="CurrencySymbol"/>
          </xsl:variable>
          <TradeCCY>
            <xsl:value-of select="$varCurrency"/>
          </TradeCCY>

          <xsl:variable name ="varPrice">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>

          <xsl:variable name ="varSettleCurrency">
            <xsl:value-of select="SettlCurrency"/>
          </xsl:variable>
          <SettleCCY>
            <xsl:value-of select="$varSettleCurrency"/>
          </SettleCCY>

          <xsl:variable name ="varTradeFXRate">
            <xsl:value-of select="ForexRate_Trade"/>
          </xsl:variable>
          <TradeFXRate>
            <xsl:value-of select="$varTradeFXRate"/>
          </TradeFXRate>

          <xsl:variable name ="varBloomberg">
            <xsl:value-of select="BBCode"/>
          </xsl:variable>
          <Identifier>
            <xsl:value-of select="$varBloomberg"/>
          </Identifier>
               
          <IdentifierType>
            <xsl:value-of select="'Bloomberg Symbol'"/>
          </IdentifierType>

          <xsl:variable name ="varMultiplier">
            <xsl:value-of select="AssetMultiplier"/>
          </xsl:variable>
          <Multiplier>
            <xsl:value-of select="$varMultiplier"/>
          </Multiplier>

          <xsl:variable name ="varPutOrCall">
            <xsl:value-of select="PutOrCall"/>
          </xsl:variable>
          <OptionType>
            <xsl:value-of select="$varPutOrCall"/>
          </OptionType>

          <xsl:variable name ="varExpirationDate">
            <xsl:value-of select="ExpirationDate"/>
          </xsl:variable>
          <ExpirtDate>
            <xsl:value-of select="$varExpirationDate"/>
          </ExpirtDate>

          <xsl:variable name ="varStrikePrice">
            <xsl:value-of select="StrikePrice"/>
          </xsl:variable>
          <StrikePrice>
            <xsl:value-of select="$varStrikePrice"/>
          </StrikePrice>

          <ExerciseType>
            <xsl:value-of select="'American'"/>
          </ExerciseType>
          
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
