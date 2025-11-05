<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

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
        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <!--<InstrumentSubType>
					<xsl:value-of select="'Instrument Sub Type'"/>
				</InstrumentSubType>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<LifeCycle>
					<xsl:value-of select="'Life Cycle'"/>
				</LifeCycle>-->

        <Confirmed>
          <xsl:value-of select ="'Confirmed'"/>
        </Confirmed>

        <Sent>
          <xsl:value-of select="'Sent'"/>
        </Sent>

        <SeqNumber>
          <xsl:value-of select="'Seq Number'"/>
        </SeqNumber>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>

        <AccountNumber>
          <xsl:value-of select="'Account Number'"/>
        </AccountNumber>

        <Future_Option>
          <xsl:value-of select="'Future/Option'"/>
        </Future_Option>

        <CitcoCode>
          <xsl:value-of select="'Citco Code'"/>
        </CitcoCode>

        <Commodity>
          <xsl:value-of select="'Commodity'"/>
        </Commodity>

        <ExchangeTicker>
          <xsl:value-of select="'Exchange Ticker'"/>
        </ExchangeTicker>

        <ExchangeTickerAdmin>
          <xsl:value-of select="'Exchange Ticker (Admin)'"/>
        </ExchangeTickerAdmin>

        <COMEX>
          <xsl:value-of select="'COMEX'"/>
        </COMEX>

        <Buy_Sell>
        <xsl:value-of select="'Buy/Sell'"/>
        </Buy_Sell>

        <Carry>
          <xsl:value-of select="'Carry'"/>
        </Carry>

        <Lots>
          <xsl:value-of select="'Lots'"/>
        </Lots>

        <PromptDate>
          <xsl:value-of select="'Prompt Date'"/>
        </PromptDate>

        <Put_Call>
          <xsl:value-of select="'Put/Call'"/>
        </Put_Call>

        <Strike>
          <xsl:value-of select="'Strike'"/>
        </Strike>

        <NettPrice>
          <xsl:value-of select="'Nett Price'"/>
        </NettPrice>

        <ExecutingBroker>
          <xsl:value-of select="'Executing Broker'"/>
        </ExecutingBroker>

        <ClearingBroker>
          <xsl:value-of select="'Clearing Broker'"/>
        </ClearingBroker>

        <Comments>
          <xsl:value-of select="'Comments'"/>
        </Comments>

        <AdminTicker>
          <xsl:value-of select="'Admin Ticker'"/>
        </AdminTicker>

        <BloombergTicker>
          <xsl:value-of select="'Bloomberg Ticker'"/>
        </BloombergTicker>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <TradeDate1>
          <xsl:value-of select="'Trade Date '"/>
        </TradeDate1>

        <SettleDate1>
          <xsl:value-of select="'Settle Date '"/>
        </SettleDate1>

        <PromptDate1>
          <xsl:value-of select="'Prompt Date '"/>
        </PromptDate1>

        <LastTradeDate>
          <xsl:value-of select="'Last Trade Date'"/>
        </LastTradeDate>

        <DeliveryDate>
          <xsl:value-of select="'Delivery Date'"/>
        </DeliveryDate>

        <ExpiryDate>
          <xsl:value-of select="'Expiry Date'"/>
        </ExpiryDate>


        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <xsl:if test ="AccountName = '0VQ-954118' or (AccountName != '0VQ-954118' and CounterParty = 'Jefferies Bache')">
			  <ThirdPartyFlatFileDetail>
				  <!--for system internal use-->
				  <RowHeader>
					  <xsl:value-of select ="true"/>
				  </RowHeader>

				  <!--for system use only-->
				  <IsCaptionChangeRequired>
					  <xsl:value-of select ="true"/>
				  </IsCaptionChangeRequired>

				  <!--for system internal use-->
				  <TaxLotState>
					  <xsl:value-of select ="TaxLotState"/>
				  </TaxLotState>

				  <xsl:variable name="varKey">
					  <xsl:call-template name="substring-after-last">
						  <xsl:with-param name="string" select="BBCode"/>
						  <xsl:with-param name="delimiter" select="' '"/>
					  </xsl:call-template>
				  </xsl:variable>

				  <Confirmed>
					  <xsl:value-of select ="''"/>
				  </Confirmed>

				  <Sent>
					  <xsl:value-of select="''"/>
				  </Sent>

				  <SeqNumber>
					  <xsl:value-of select="EntityID"/>
				  </SeqNumber>

				  <TradeDate>
					  <xsl:value-of select="TradeDate"/>
				  </TradeDate>

				  <SettleDate>
					  <xsl:value-of select="SettlementDate"/>
				  </SettleDate>

				  <Action>
					  <xsl:choose>
						  <xsl:when test="TaxLotState = 'Allocated'">
							  <xsl:value-of select="'New'"/>
						  </xsl:when>
						  <xsl:when test="TaxLotState = 'Amemded'">
							  <xsl:value-of select="'Cor'"/>
						  </xsl:when>
						  <xsl:when test="TaxLotState = 'Deleted'">
							  <xsl:value-of select="'Del'"/>
						  </xsl:when>
					  </xsl:choose>
				  </Action>

				  <AccountNumber>
					  <xsl:value-of select="AccountName"/>
				  </AccountNumber>

				  <Future_Option>
					  <xsl:choose>
						  <xsl:when test="Asset = 'FutureOption'">
							  <xsl:value-of select="'Option'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="Asset"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Future_Option>



				  <CitcoCode>
					  <xsl:choose>
						  <xsl:when test="($varKey = 'Index' or $varKey = 'INDEX') and Asset = 'Future'">
							  <xsl:value-of select="'IDXFUT'"/>
						  </xsl:when>
						  <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'Future' and Exchange != 'LME'">
							  <xsl:value-of select="'CMDFUT'"/>
						  </xsl:when>
						  <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'FutureOption'">
							  <xsl:value-of select="'CMDFUTOPT'"/>
						  </xsl:when>
						  <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY') and Exchange = 'LME' ">
							  <xsl:value-of select="'MTLFWD'"/>
						  </xsl:when>
					  </xsl:choose>
				  </CitcoCode>

				  <Commodity>
					  <xsl:value-of select="UDASectorName"/>
				  </Commodity>

				  <ExchangeTicker>
					  <xsl:value-of select="substring-before(UnderlyingSymbol,' ')"/>
				  </ExchangeTicker>

				  <ExchangeTickerAdmin>
					  <xsl:choose>
						  <xsl:when test="Exchange = 'LME' or Exchange = 'LME-FO'">
							  <xsl:value-of select="concat(substring-before(UnderlyingSymbol,' '),'P')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="substring-before(UnderlyingSymbol,' ')"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </ExchangeTickerAdmin>

				  <COMEX>
					  <xsl:choose>
						  <xsl:when test ="Exchange = 'LME-FO'">
							  <xsl:value-of select ="'LME'"/>
						  </xsl:when>
						  <xsl:when test ="Exchange = 'COMX'">
							  <xsl:value-of select ="'Comex'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="Exchange"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </COMEX>

				  <Buy_Sell>
					  <xsl:value-of select="substring(Side,1,1)"/>
				  </Buy_Sell>

				  <Carry>
					  <xsl:choose>
						  <xsl:when test ="TradeAttribute1 = ''">
							  <xsl:value-of select="LotId"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="TradeAttribute1"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Carry>

				  <Lots>
					  <xsl:choose>
						  <xsl:when test ="substring(Side,1,1) = 'S'">
							  <xsl:value-of select ="AllocatedQty*(-1)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="AllocatedQty"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Lots>

				  <xsl:variable name="varPromptDate">
					  <xsl:value-of select="substring(substring-after(Symbol, ' '),1,2)"/>
				  </xsl:variable>

				  <PromptDate>
					  <xsl:choose>
						  <xsl:when test ="(Asset = 'Future' and Exchange ='LME') or (Asset = 'Future' and Exchange ='CME') or (Asset = 'FutureOption' and Exchange ='LME-FO') ">
							  <xsl:value-of select ="concat(substring($varPromptDate,2,1),'1',substring($varPromptDate,1,1))"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:choose>
								  <xsl:when test="number(substring($varPromptDate,1,1))">
									  <xsl:value-of select="concat('1',$varPromptDate)"/>
								  </xsl:when>
								  <xsl:when test="number(substring($varPromptDate,2,1))">
									  <xsl:value-of select="concat(substring($varPromptDate,1,1),'1',substring($varPromptDate,2,1))"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="$varPromptDate"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </xsl:otherwise>
					  </xsl:choose>
				  </PromptDate>

				  <Put_Call>
					  <xsl:value-of select="substring(PutOrCall,1,1)"/>
				  </Put_Call>

				  <Strike>
					  <xsl:choose>
						  <xsl:when test="number(StrikePrice)">
							  <xsl:value-of select="StrikePrice"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Strike>

				  <NettPrice>
					  <xsl:value-of select="AveragePrice"/>
				  </NettPrice>

				  <ExecutingBroker>
					  <xsl:value-of select="CounterParty"/>
				  </ExecutingBroker>

				  <ClearingBroker>
					  <xsl:choose>
						  <xsl:when test ="FundName = '0VQ-954118'">
							  <xsl:value-of select ="'Jefferies'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="'JPMC'"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </ClearingBroker>

				  <Comments>
					  <xsl:value-of select="Description"/>
				  </Comments>

				  <AdminTicker>
					  <xsl:value-of select="translate(normalize-space(substring-before(BBCode,$varKey)),$varSmall,$varCapital)"/>
				  </AdminTicker>

				  <BloombergTicker>
					  <xsl:value-of select="translate(BBCode,$varSmall,$varCapital)"/>
				  </BloombergTicker>

				  <Currency>
					  <xsl:value-of select="CurrencySymbol"/>
				  </Currency>

				  <TradeDate1>
					  <xsl:value-of select="concat(substring-before(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-after(substring-after(TradeDate,'/'),'/'))"/>
				  </TradeDate1>

				  <SettleDate1>
					  <xsl:value-of select="concat(substring-before(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-after(substring-after(SettlementDate,'/'),'/'))"/>
				  </SettleDate1>

				  <PromptDate1>
					  <xsl:value-of select="concat(substring-before(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-after(substring-after(ExpirationDate,'/'),'/'))"/>
				  </PromptDate1>

				  <LastTradeDate>
					  <xsl:value-of select="concat(substring-before(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-after(substring-after(ExpirationDate,'/'),'/'))"/>
				  </LastTradeDate>

				  <DeliveryDate>
					  <xsl:choose>
						  <xsl:when test ="Asset = 'FutureOption'">
							  <xsl:value-of select ="''"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="concat(substring-before(substring-after(DeliveryDate,'/'),'/'),substring-before(DeliveryDate,'/'),substring-before(substring-after(substring-after(DeliveryDate,'/'),'/'),' '))"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </DeliveryDate>

				  <ExpiryDate>
					  <xsl:choose>
						  <xsl:when test ="Asset = 'FutureOption'">
							  <xsl:value-of select="concat(substring-before(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-after(substring-after(ExpirationDate,'/'),'/'))"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </ExpiryDate>

				  <!-- system use only-->
				  <EntityID>
					  <xsl:value-of select="EntityID"/>
				  </EntityID>

			  </ThirdPartyFlatFileDetail>
		  </xsl:if>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
