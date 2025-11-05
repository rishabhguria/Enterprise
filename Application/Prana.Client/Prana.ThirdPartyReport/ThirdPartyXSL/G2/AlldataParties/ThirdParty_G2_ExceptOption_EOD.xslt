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

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='EquityOption']">
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

        <xsl:variable name="varOriginalPurchaseDate">
          <xsl:call-template name="DateFormat">
            <xsl:with-param name="Date" select="OriginalPurchaseDate"/>
          </xsl:call-template>
        </xsl:variable>
        <OriginalPurchaseDate>
          <xsl:value-of select="$varOriginalPurchaseDate"/>
        </OriginalPurchaseDate>

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

        <xsl:variable name ="varISIN">
          <xsl:value-of select="ISIN"/>
        </xsl:variable>
        <Identifier>
          <xsl:value-of select="$varISIN"/>
        </Identifier>

        <IdentifierType>
          <xsl:value-of select="'ISIN'"/>
        </IdentifierType>

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

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>
</xsl:stylesheet>
