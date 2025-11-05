<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <Transactiontype>
          <xsl:value-of select="'Transaction type'"/>
        </Transactiontype>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <ClientrefNumber>
          <xsl:value-of select="'Client ref Number'"/>
        </ClientrefNumber>

        <Version>
          <xsl:value-of select="'Version'"/>
        </Version>

        <TransactionStatus>
          <xsl:value-of select="'Transaction Status'"/>
        </TransactionStatus>

        <SecurityIdentifierType>
          <xsl:value-of select="'Security Identifier Type'"/>
        </SecurityIdentifierType>


        <SecurityIdentifier>
          <xsl:value-of select="'Security Identifier'"/>
        </SecurityIdentifier>

        <ContractYear>
          <xsl:value-of select="'Contract Year'"/>
        </ContractYear>


        <ContractMonth>
          <xsl:value-of select="'Contract Month'"/>
        </ContractMonth>

        <ContractDay>
          <xsl:value-of select="'Contract Day'"/>
        </ContractDay>

        <ContractSecurityDescription>
          <xsl:value-of select="'Contract Security Description'"/>
        </ContractSecurityDescription>

        <MarkerExchange>
          <xsl:value-of select="'Marker/Exchange'"/>
        </MarkerExchange>

        <BuySellIndicator>
          <xsl:value-of select="'Buy/Sell Indicator'"/>
        </BuySellIndicator>

        <TradeType>
          <xsl:value-of select="'Trade Type'"/>
        </TradeType>

        <OrdertoCloseIndicator>
          <xsl:value-of select="'Order to Close Indicator'"/>
        </OrdertoCloseIndicator>

        <AveragePriceIndicator>
          <xsl:value-of select="'Average Price Indicator'"/>
        </AveragePriceIndicator>

        <SpreadTradeIndicator>
          <xsl:value-of select="'Spread Trade Indicator'"/>
        </SpreadTradeIndicator>

        <NightTradeIndicator>
          <xsl:value-of select="'Night Trade Indicator'"/>
        </NightTradeIndicator>

        <ExchangeforPhysicalIndicator>
          <xsl:value-of select="'Exchange for Physical Indicator'"/>
        </ExchangeforPhysicalIndicator>

        <BlocktradeIndicator>
          <xsl:value-of select="'Block trade Indicator'"/>
        </BlocktradeIndicator>

        <OffExchangeIndicator>
          <xsl:value-of select="'Off Exchange Indicator'"/>
        </OffExchangeIndicator>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <ExecutionTime>
          <xsl:value-of select="'Execution Time'"/>
        </ExecutionTime>


        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <CallPutIndicator>
          <xsl:value-of select="'Call/Put Indicator'"/>
        </CallPutIndicator>

        <StrikePrice>
          <xsl:value-of select="'Strike Price'"/>
        </StrikePrice>

        <ExecutingBroker>
          <xsl:value-of select="'Executing Broker'"/>
        </ExecutingBroker>

        <ClearingBroker>
          <xsl:value-of select="'Clearing Broker'"/>
        </ClearingBroker>

        <GiveUpReference>
          <xsl:value-of select="'Give Up Reference'"/>
        </GiveUpReference>

        <HearsayIndicator>
          <xsl:value-of select="'Hearsay Indicator'"/>
        </HearsayIndicator>

        <ExecutionFee>
          <xsl:value-of select="'Execution Fee'"/>
        </ExecutionFee>

        <ExecutionFeeCCY>
          <xsl:value-of select="'Execution Fee CCY'"/>
        </ExecutionFeeCCY>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <CommissionCCY>
          <xsl:value-of select="'Commission CCY'"/>
        </CommissionCCY>

        <ExchangeFee>
          <xsl:value-of select="'Exchange Fee'"/>
        </ExchangeFee>

        <ExchangeFeeCCY>
          <xsl:value-of select="'Exchange Fee CCY'"/>
        </ExchangeFeeCCY>

        <OrderId>
          <xsl:value-of select="'Order Id'"/>
        </OrderId>

        <DealId>
          <xsl:value-of select="'Deal Id'"/>
        </DealId>

        <Message>
          <xsl:value-of select="'Message'"/>
        </Message>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Future' or Asset='FutureOption']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <Transactiontype>
            <xsl:value-of select="'D01'"/>
          </Transactiontype>

          <Account>
            <xsl:value-of select="AccountNo"/>
          </Account>

          <xsl:variable name="i" select="position()" />
          <ClientrefNumber>
            <xsl:choose>
              <xsl:when test="$i &lt; 10">
                <xsl:value-of select="concat(PBUniqueID,'0000',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 100">
                <xsl:value-of select="concat(PBUniqueID,'000',$i)"/>
              </xsl:when>
              <xsl:when test="$i &lt; 1000">
                <xsl:value-of select="concat(PBUniqueID,'00',$i)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="concat(PBUniqueID,position())"/>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:value-of select="''"/>
          </ClientrefNumber>

          <Version>
            <xsl:value-of select="''"/>
          </Version>

          <TransactionStatus>
            <xsl:choose>
              <xsl:when test="TaxLotState = 'Allocated'">
                <xsl:value-of select="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Amended'">
                <xsl:value-of select="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Deleted'">
                <xsl:value-of select="'CAN'"/>
              </xsl:when>
            </xsl:choose>
          </TransactionStatus>

          <xsl:variable name="varKey">
            <xsl:call-template name="substring-after-last">
              <xsl:with-param name="string" select="BBCode"/>
              <xsl:with-param name="delimiter" select="' '"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varBBCode">
            <xsl:choose>
              <xsl:when test="($varKey = 'INDEX')">
                <xsl:value-of select="concat(substring-before(BBCode,' INDEX'),' Index')"/>
              </xsl:when>
              <xsl:when test="($varKey = 'COMDTY')">
                <xsl:value-of select="concat(substring-before(BBCode,' COMDTY'),' Comdty')"/>
              </xsl:when>
              <xsl:when test="($varKey = 'CURNCY')">
                <xsl:value-of select="concat(substring-before(BBCode,' CURNCY'),' Curncy')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="BBCode"/>
              </xsl:otherwise>            
            </xsl:choose>
          </xsl:variable>

          <SecurityIdentifierType>
            <xsl:choose>
              <xsl:when test="RICCode!='' ">
                <xsl:value-of select="'RIC'"/>
              </xsl:when>

              <xsl:when test="BBCode!='' ">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdentifierType>


          <SecurityIdentifier>
            <xsl:choose>
              <xsl:when test="RICCode!='' ">
                <xsl:value-of select="RICCode"/>
              </xsl:when>

              <xsl:when test="BBCode!='' ">
                <xsl:value-of select="$varBBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdentifier>

          <ContractYear>
            <xsl:value-of select="substring-after(substring-after(ExpirationDate,'/'),'/')"/>
          </ContractYear>


          <ContractMonth>
            <xsl:value-of select="substring-before(ExpirationDate,'/')"/>
          </ContractMonth>

          <ContractDay>
            <xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
          </ContractDay>

          <ContractSecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </ContractSecurityDescription>

          <MarkerExchange>
            <xsl:value-of select="Exchange"/>
          </MarkerExchange>

          <BuySellIndicator>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>

              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'S'"/>
              </xsl:when>

              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
              </xsl:when>

              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuySellIndicator>

          <TradeType>
            <xsl:value-of select="''"/>
          </TradeType>

          <OrdertoCloseIndicator>
            <xsl:value-of select="''"/>
          </OrdertoCloseIndicator>

          <AveragePriceIndicator>
            <xsl:value-of select="''"/>
          </AveragePriceIndicator>

          <SpreadTradeIndicator>
            <xsl:value-of select="''"/>
          </SpreadTradeIndicator>

          <NightTradeIndicator>
            <xsl:value-of select="''"/>
          </NightTradeIndicator>

          <ExchangeforPhysicalIndicator>
            <xsl:value-of select="''"/>
          </ExchangeforPhysicalIndicator>

          <BlocktradeIndicator>
            <xsl:value-of select="''"/>
          </BlocktradeIndicator>

          <OffExchangeIndicator>
            <xsl:value-of select="''"/>
          </OffExchangeIndicator>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <ExecutionTime>
            <xsl:value-of select="''"/>
          </ExecutionTime>


          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <!-- <xsl:value-of select ="format-number(AveragePrice,'##.####')"/> -->
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <CallPutIndicator>
            <xsl:choose>
              <xsl:when test="Asset='FutureOption'">
                <xsl:value-of select="substring(PutOrCall,1,1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CallPutIndicator>

          <StrikePrice>
            <xsl:choose>
              <xsl:when test="Asset='FutureOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </StrikePrice>

          <ExecutingBroker>
            <xsl:value-of select="CounterParty"/>
          </ExecutingBroker>

          <ClearingBroker>
            <xsl:value-of select="'MSCO'"/>
          </ClearingBroker>

          <GiveUpReference>
            <xsl:value-of select="''"/>
          </GiveUpReference>

          <HearsayIndicator>
            <xsl:value-of select="''"/>
          </HearsayIndicator>

          <ExecutionFee>
            <xsl:value-of select="''"/>
          </ExecutionFee>

          <ExecutionFeeCCY>
            <xsl:value-of select ="CurrencySymbol"/>
          </ExecutionFeeCCY>

          <Commission>
            <xsl:value-of select ="(CommissionCharged + SoftCommissionCharged)"/>
          </Commission>

          <CommissionCCY>
            <xsl:value-of select ="CurrencySymbol"/>
          </CommissionCCY>

          <ExchangeFee>
            <xsl:value-of select="OtherBrokerFee"/>
          </ExchangeFee>

          <ExchangeFeeCCY>
            <xsl:value-of select ="CurrencySymbol"/>
          </ExchangeFeeCCY>

          <OrderId>
            <xsl:value-of select="''"/>
          </OrderId>

          <DealId>
            <xsl:value-of select="''"/>
          </DealId>

          <Message>
            <xsl:value-of select="''"/>
          </Message>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>