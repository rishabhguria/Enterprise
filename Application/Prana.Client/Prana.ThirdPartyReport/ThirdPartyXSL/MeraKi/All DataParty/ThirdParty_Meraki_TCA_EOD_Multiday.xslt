<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
 

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <IsAnOrder>
            <xsl:value-of select="IsAnOrder"/>
          </IsAnOrder>
		  
		  <MessageType>
            <xsl:value-of select="MessageType"/>
          </MessageType>

			<OrderRef>
            <xsl:value-of select="ParentClOrderID"/>
          </OrderRef>
		  
          <ParentOrderRef>
            <xsl:value-of select="ParentOrderRef"/>
          </ParentOrderRef>

          <DecisionTime>
			<xsl:choose>
              <xsl:when test="MessageType = 'SO' or MessageType = 'F'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="DecisionTime"/>
              </xsl:otherwise>
            </xsl:choose>
          </DecisionTime>
		  
		  <ArrivalTime_QuoteTime>
			<xsl:choose>
              <xsl:when test="MessageType = 'F'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ArrivalTime_QuoteTime"/>
              </xsl:otherwise>
            </xsl:choose>
          </ArrivalTime_QuoteTime>

          <FirstFillTime_TradeTime>
            <xsl:value-of select="FirstFillTime_TradeTime"/>
          </FirstFillTime_TradeTime>

          <LastFillTime>
            <xsl:value-of select="LastFillTime"/>
          </LastFillTime>
          <ExecutionType>
            <xsl:value-of select="ExecutionType"/>
          </ExecutionType>
          <LimitPrice>
			<xsl:value-of select="LimitPrice"/>
          </LimitPrice>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Currency>
            <xsl:value-of select="Currency"/>
          </Currency>

          <Venue>
            <xsl:value-of select="Venue"/>
          </Venue>

          <PMID>
            <xsl:value-of select="PMID"/>
          </PMID>

          <ParticipantCode>
            <xsl:value-of select="Broker"/>
          </ParticipantCode>

          <TraderID>
            <xsl:value-of select="TraderID"/>
          </TraderID>

          <CounterPartyCode>
            <xsl:value-of select="CounterPartyCode"/>
          </CounterPartyCode>


          <Price>
            <xsl:value-of select="Price"/>
          </Price>

          <Quantity>
            <xsl:value-of select="Quantity"/>
          </Quantity>

          <Side>
            <xsl:value-of select="Side"/>
          </Side>

          <TradeFlag>
            <!-- <xsl:value-of select="TradeFlag"/> -->
          </TradeFlag>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <PublicTradeID>
            <xsl:value-of select="PublicTradeID"/>
          </PublicTradeID>

          <UserDefinedFilter>
            <xsl:value-of select="UserDefinedFilter"/>
          </UserDefinedFilter>

          <TradingNetworkID>
            <xsl:value-of select="TradingNetworkID"/>
          </TradingNetworkID>

          <SettlementPeriod>
            <xsl:value-of select="SettlementPeriod"/>
          </SettlementPeriod>

          <MarketOrderId>
            <xsl:value-of select="MarketOrderId"/>
          </MarketOrderId>

          <ParticipationRate>
            <xsl:value-of select="ParticipationRate"/>
          </ParticipationRate>

          <BenchmarkVenues>
            <xsl:value-of select="BenchmarkVenues"/>
          </BenchmarkVenues>

          <BenchmarkType>
            <xsl:value-of select="BenchmarkType"/>
          </BenchmarkType>

          <FlowType>
            <!-- <xsl:value-of select="FlowType"/> -->
          </FlowType>

          <BasketID>
            <xsl:value-of select="BasketID"/>
          </BasketID>

          <Note>
            <xsl:value-of select="Note"/>
          </Note>

          <Urgency>
            <xsl:value-of select="Urgency"/>
          </Urgency>

          <AlgoName>
            <xsl:value-of select="AlgoName"/>
          </AlgoName>

          <AlgoParams>
            <xsl:value-of select="AlgoParams"/>
          </AlgoParams>

          <Index>
            <xsl:value-of select="Index"/>
          </Index>

          <Sector>
            <xsl:value-of select="Sector"/>
          </Sector>

          <FeeBasis1> 
		   <xsl:choose>
              <xsl:when test="FeeBasis1 = '*'">
                <xsl:value-of select="'0'"/>
              </xsl:when>
              <xsl:otherwise>
               <xsl:value-of select="FeeBasis1"/>  
              </xsl:otherwise>
            </xsl:choose> 
          </FeeBasis1>

          <FeeAmount1>
            <!-- <xsl:value-of select="FeeAmount1"/> -->
          </FeeAmount1>

          <FeeBasis2>
            <!-- <xsl:value-of select="FeeBasis2"/> -->
          </FeeBasis2>

          <FeeAmount2>
            <!-- <xsl:value-of select="FeeAmount2"/> -->
          </FeeAmount2>

          <PreTradeImpactEstimate>
            <xsl:value-of select="PreTradeImpactEstimate"/>
          </PreTradeImpactEstimate>

          <PreTradeRiskEstimate>
            <xsl:value-of select="PreTradeRiskEstimate"/>
          </PreTradeRiskEstimate>

          <UserBenchmarks>
            <xsl:value-of select="UserBenchmarks"/>
          </UserBenchmarks>

          <AssetType>
            <xsl:value-of select="AssetType"/>
          </AssetType>

          <AssetSubType>
            <xsl:value-of select="AssetSubType"/>
          </AssetSubType>

          <ActionType>
            <xsl:value-of select="ActionType"/>
          </ActionType>

          <ActionDateTime>
            <!-- <xsl:value-of select="ActionDateTime"/> -->
          </ActionDateTime>

          <DirectedFlow>
            <xsl:value-of select="DirectedFlow"/>
          </DirectedFlow>

          <PrimaryExchangeLine>
            <xsl:value-of select="PrimaryExchangeLine"/>
          </PrimaryExchangeLine>

          <TargetCurrency>
            <xsl:value-of select="TargetCurrency"/>
          </TargetCurrency>

          <CFICode>
            <xsl:value-of select="CFICode"/>
          </CFICode>

          <CounterpartyInteractionType>
            <xsl:value-of select="CounterpartyInteractionType"/>
          </CounterpartyInteractionType>

          <LastCapacity>
            <xsl:value-of select="LastCapacity"/>
          </LastCapacity>

          <TimeInForce>
            <xsl:value-of select="TimeInForce"/>
          </TimeInForce>

          <ClientCategory>
            <!-- <xsl:value-of select="ClientCategory"/> -->
          </ClientCategory>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>