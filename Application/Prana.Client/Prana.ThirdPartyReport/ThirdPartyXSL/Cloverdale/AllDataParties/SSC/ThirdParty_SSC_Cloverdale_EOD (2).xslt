<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <!--<ThirdPartyFlatFileDetail>

        --><!--for system internal use--><!--
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <FileHeader>
          <xsl:value-of select ="'true'"/>
        </FileHeader>

        <FileFooter>
          <xsl:value-of select ="'false'"/>
        </FileFooter>

        --><!--for system internal use--><!--
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        --><!--for system internal use--><!--
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <RecordAction>
          <xsl:value-of select="'RecordAction'"/>
        </RecordAction>

        <RecordType>
          <xsl:value-of select="'RecordType'"/>
        </RecordType>

        <Portfolio>
          <xsl:value-of select="'Portfolio'"/>
        </Portfolio>

        <Investment>
          <xsl:value-of select="'Investment'"/>
        </Investment>

        <LocationAccount>
          <xsl:value-of select="'LocationAccount'"/>
        </LocationAccount>

        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <EventDate>
          <xsl:value-of select="'EventDate'"/>
        </EventDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <ActualSettleDate>
          <xsl:value-of select="'ActualSettleDate'"/>
        </ActualSettleDate>

        <SecFeeAmount>
          <xsl:value-of select="'SecFeeAmount'"/>
        </SecFeeAmount>

        <NetCounterAmount>
          <xsl:value-of select="'NetCounterAmount'"/>
        </NetCounterAmount>

        <NetInvestmentAmount>
          <xsl:value-of select="'NetInvestmentAmount'"/>
        </NetInvestmentAmount>

        <TotCommission>
          <xsl:value-of select="'TotCommission'"/>
        </TotCommission>

        <UserTranId1>
          <xsl:value-of select="'UserTranId1'"/>
        </UserTranId1>

        <PriceDenomination>
          <xsl:value-of select="'PriceDenomination'"/>
        </PriceDenomination>

        <CounterInvestment>
          <xsl:value-of select="'CounterInvestment'"/>
        </CounterInvestment>

        <CounterFXDenomination>
          <xsl:value-of select="'CounterFXDenomination'"/>
        </CounterFXDenomination>

        <TradeFX>
          <xsl:value-of select="'TradeFX'"/>
        </TradeFX>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <SecurityType>
          <xsl:value-of select="'Security Type'"/>
        </SecurityType>

        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>

        --><!-- system use only--><!--
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>-->
      <xsl:for-each select="ThirdPartyFlatFileDetail[contains(AccountName,'CLVD')]">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <!--<IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>-->

          <FileHeader>
            <xsl:value-of select ="'false'"/>
          </FileHeader>
          
          <FileFooter>
            <xsl:value-of select ="'false'"/>
          </FileFooter>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="Prana_FundName">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name="PRANA_MasterFund_Name">
            <xsl:value-of select="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name= 'GS']/MasterFundData[@FundName=$Prana_FundName]/@MasterFundName"/>
          </xsl:variable>


          <RecordAction>
			  <xsl:choose>
				  <xsl:when test="TaxLotState='Allocated'">
					  <xsl:value-of select="'NEW'"/>
				  </xsl:when>
				  <xsl:when test="TaxLotState='Amended'">
					  <xsl:value-of select="'COR'"/>
				  </xsl:when>
				  <xsl:when test="TaxLotState='Deleted'">
					  <xsl:value-of select="'CAN'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
            
          </RecordAction>

          <RecordType>
			  <xsl:choose>
				  <xsl:when test ="Side = 'Buy to Cover'">
					  <xsl:value-of select ="'Buy to Close'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Side"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </RecordType>

          <Portfolio>
            <!--<xsl:value-of select="$PRANA_MasterFund_Name"/>-->
			  <!--<xsl:choose>
				  <xsl:when test ="contains(FundName,'GSEC')='true' or contains(FundName,'GSI')='true'">
					  <xsl:value-of select="'AFPK1209'"/>
				  </xsl:when>
				  <xsl:when test ="contains(FundName,'MS')='true'">
					  <xsl:value-of select="'038CDK7A3'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select ="''"/>
				  </xsl:otherwise>
			  </xsl:choose>-->
			  <!--<xsl:choose>
				  <xsl:when test="contains(FundName,'CLVD')">-->
					  <xsl:choose>
						  <xsl:when test="contains(AccountName,'CLVD - GS')">
							  <xsl:value-of select="'CLOVEG'"/>
						  </xsl:when>
						  <xsl:when test="contains(AccountName,'CLVD - MS')">
							  <xsl:value-of select="'CLOVET'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="substring-after(AccountName,'-')"/>
						  </xsl:otherwise>
					  </xsl:choose>
				
				  <!--</xsl:when>-->
				 
			  <!--</xsl:choose>-->
			 
          </Portfolio>

			<xsl:variable name="PB_SYMBOL_NAME">
				<xsl:value-of select="Symbol"/>
			</xsl:variable>

			<xsl:variable name="PRANA_SYMBOL_NAME">
				<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name='SSC']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			</xsl:variable>

			<xsl:variable name="SymbolMapping">
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
				
			</xsl:variable>


			<Investment>
			  <xsl:choose>

				  <xsl:when test="$SymbolMapping!=''">
					  <xsl:value-of select="$SymbolMapping"/>
				  </xsl:when>

				  <xsl:when test ="contains(Symbol, '-') != false">
					  <xsl:value-of select ="BBCode"/>
				  </xsl:when>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
				  </xsl:when>

				 

				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </Investment>

			<xsl:variable name ="varFundName">
				<xsl:choose>
					<xsl:when test ="AccountName = 'MS Main'">
						<xsl:value-of select ="'038CDFPK2'"/>
					</xsl:when>
					<xsl:when test ="AccountName = 'MS Swap'">
						<xsl:value-of select ="'06178F8Q5'"/>
					</xsl:when>
					<xsl:when test ="AccountName = 'MS FX Option'">
						<xsl:value-of select ="'058D17U04'"/>
					</xsl:when>
					<xsl:when test ="AccountName = 'MS FX Spot'">
						<xsl:value-of select ="'0581CB0P7'"/>
					</xsl:when>
					<xsl:when test ="AccountName = 'GS Main'">
						<xsl:value-of select ="'002506988'"/>
					</xsl:when>
					<xsl:when test ="AccountName = 'GS FX Option'">
						<xsl:value-of select ="'044455921'"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>

          <LocationAccount>
            <xsl:value-of select="concat($PRANA_MasterFund_Name, '_', $varFundName)"/>
          </LocationAccount>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

			<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

			<xsl:variable name="PB_BROKER_NAME">
				<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='SSC']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@MLPBroker"/>
			</xsl:variable>

			<xsl:variable name="varBROKER">
				<xsl:choose>
					<xsl:when test="$PB_BROKER_NAME != ''">
						<xsl:value-of select="$PB_BROKER_NAME"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PRANA_BROKER_NAME"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<Broker>
            <xsl:value-of select="$varBROKER"/>
          </Broker>

          <EventDate>
            <xsl:value-of select="TradeDate"/>
          </EventDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <ActualSettleDate>
            <xsl:value-of select="SettlementDate"/>
          </ActualSettleDate>

          <SecFeeAmount>
            <xsl:value-of select="StampDuty"/>
          </SecFeeAmount>

          <NetCounterAmount>
            <xsl:value-of select="''"/>
          </NetCounterAmount>

			<xsl:variable name="FXRate" >
				<xsl:choose>
					<xsl:when test="number(FXRate_Taxlot)">
						<xsl:value-of select="FXRate_Taxlot"/>
					</xsl:when>
					<xsl:when test="number(ForexRate)">
						<xsl:value-of select="ForexRate"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="1"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<BuyAmount>
				<xsl:choose>
					<xsl:when test="contains(Asset,'FX')">
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="AllocatedQty * AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
				
         	</BuyAmount>

			<SellAmount>
				<xsl:choose>
					<xsl:when test="contains(Asset,'FX')">
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="AllocatedQty * AveragePrice"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</SellAmount>

			<SpotRate>
				<xsl:value-of select="$FXRate"/>
				
			</SpotRate>

          <NetInvestmentAmount>
            <xsl:value-of select="NetAmount"/>
          </NetInvestmentAmount>

          <TotCommission>
            <xsl:value-of select="CommissionCharged"/>
          </TotCommission>

          <UserTranId1>
            <xsl:value-of select="concat('A',EntityID)"/>
          </UserTranId1>

			<BuyCurrency>
				<xsl:choose>
					<xsl:when test="contains(Asset,'FX')">
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>

				</xsl:choose>
				<!--<xsl:value-of select="CurrencySymbol"/>-->
			</BuyCurrency>

			<SellCurrency>
				<xsl:choose>
					<xsl:when test="contains(Asset,'FX')">
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>

				</xsl:choose>
				<!--<xsl:value-of select="CurrencySymbol"/>-->
			</SellCurrency>

					<CounterFXDenomination>
            <xsl:value-of select="CurrencySymbol"/>
          </CounterFXDenomination>

          <TradeFX>
            <xsl:value-of select="ForexRate"/>
          </TradeFX>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

			<SecurityType>
				<xsl:value-of select="Asset"/>
			</SecurityType>

			<SecurityDescription>
				<xsl:value-of select="FullSecurityName"/>
			</SecurityDescription>

			<xsl:variable name="PRANA_EXCHANGE" select="Exchange"/>

			<xsl:variable name="PB_EXCHANGE">
				<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name='SSC']/ExchangeData[@PranaExchange=$PRANA_EXCHANGE]/@PBExchangeName"/>
			</xsl:variable>

			<xsl:variable name="varEXCODE">
				<xsl:choose>
					<xsl:when test ="$PB_EXCHANGE!=''">
						<xsl:value-of select="$PB_EXCHANGE"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="Exchange"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<Currency>
				<xsl:value-of select="CurrencySymbol"/>
			</Currency>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
