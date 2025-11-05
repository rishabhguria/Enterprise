<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <Portfolio>
          <xsl:value-of select ="'Portfolio'"/>
        </Portfolio>
        
        <SEDOL>
          <xsl:value-of select ="'SEDOL'"/>
        </SEDOL>
        
        <SecurityName>
          <xsl:value-of select ="'Security Name'"/>
        </SecurityName>
        
        <DealType>
          <xsl:value-of select ="'Deal Type'"/>
        </DealType>
        
        <TickerField>
          <xsl:value-of select ="'Ticker Field'"/>
        </TickerField>
        
        <TradeDate>
          <xsl:value-of select ="'Trade Date'"/>
        </TradeDate>
        
        <TransType>
          <xsl:value-of select ="'Trans Type'"/>
        </TransType>
        
        <Currency>
          <xsl:value-of select ="'Currency'"/>
        </Currency>
        
        <MIC>
          <xsl:value-of select ="'MIC'"/>
        </MIC>
        
        <ExchRate>
          <xsl:value-of select ="'Exch Rate'"/>
        </ExchRate>
        
        <Broker>
          <xsl:value-of select ="'Broker'"/>
        </Broker>
        
        <SettleDate>
          <xsl:value-of select ="'Settle Date'"/>
        </SettleDate>
        
        <ExternalTradeID>
          <xsl:value-of select ="'External Trade ID'"/>
        </ExternalTradeID>
        
        <Fund>
          <xsl:value-of select ="'Fund'"/>
        </Fund>
        
        <Units>
          <xsl:value-of select ="'Units'"/>
        </Units>
        
        <PriceNative>
          <xsl:value-of select ="'Price Native'"/>
        </PriceNative>
        
        <GrossNative>
          <xsl:value-of select ="'Gross Native'"/>
        </GrossNative>
        
        <BrokerageNative>
          <xsl:value-of select ="'Brokerage Native'"/>
        </BrokerageNative>
        
        <StDutyNative>
          <xsl:value-of select ="'St Duty Native'"/>
        </StDutyNative>
        
        <OtherChargeNative>
          <xsl:value-of select ="'Other Charge Native'"/>
        </OtherChargeNative>
        
        <NetAmountNative>
          <xsl:value-of select ="'Net Amount Native'"/>
        </NetAmountNative>
        
        <GSTPaidAUD>
          <xsl:value-of select ="'GST Paid AUD'"/>
        </GSTPaidAUD>
        
        <FXCounterCurrency>
          <xsl:value-of select ="'FX Counter Currency'"/>
        </FXCounterCurrency>
        
        <FXCounterAmt>
          <xsl:value-of select ="'FX Counter Amt'"/>
        </FXCounterAmt>
        
        <AccruedInterest>
          <xsl:value-of select ="'Accrued Interest'"/>
        </AccruedInterest>
        
        <ISIN>
          <xsl:value-of select ="'ISIN'"/>
        </ISIN>
        
        <TradeStatus>
          <xsl:value-of select ="'Trade Status'"/>
        </TradeStatus>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail [CompanyAccountID='8']">
              <!--<xsl:for-each select="ThirdPartyFlatFileDetail [CompanyAccountID='8']">-->
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          <Portfolio>
            <xsl:value-of select ="'JMMNZS'"/>
          </Portfolio>

          <SEDOL>
            <xsl:choose>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward') and CurrencySymbol !='USD'">
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:when>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward') and CurrencySymbol ='USD'">
                <xsl:value-of select="SettlCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="SEDOL"/>
              </xsl:otherwise>
            </xsl:choose>
          </SEDOL>
          <SecurityName>
             <xsl:value-of select="FullSecurityName"/>         
          </SecurityName>

          <DealType>
           <xsl:choose>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward')">
                <xsl:value-of select="'S'"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </DealType>

          <TickerField>
           <xsl:choose>
              <xsl:when test="contains(BBCode, 'Equity') and CurrencySymbol !='USD' ">
                 <xsl:value-of select="substring-before(BBCode,' Equity')"/>
              </xsl:when>
            
              <xsl:when test="contains(BBCode, 'EQUITY') and CurrencySymbol !='USD'">
                  <xsl:value-of select="substring-before(BBCode,' EQUITY')"/>
              </xsl:when>
              <xsl:when test="Asset = 'Equity'">
                 <xsl:value-of select="Symbol"/>
              </xsl:when>
               <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </TickerField>
		  
						<xsl:variable name="varDate">
							<xsl:value-of select="substring(TradeDate,4,2)"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(TradeDate,1,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(TradeDate,7,4)"/>
						</xsl:variable>
						
						<xsl:variable name="varTradeDate">
							<xsl:value-of select="concat($varDate,'/', $varMonth, '/', $varYear)"/>
						</xsl:variable>
						

          <TradeDate>
            <xsl:value-of select="$varTradeDate"/>
          </TradeDate>
          
          <xsl:variable name="varSide1">
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                <xsl:value-of select ="'S'"/>
              </xsl:when>              
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TransType>
            <xsl:value-of select="$varSide1"/>
          </TransType>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>
		  
		  
		   <xsl:variable name="PB_NAME">
            <xsl:value-of select="'JANA EOD'"/>
          </xsl:variable>
		  
		  <xsl:variable name="PRANA_MIC_NAME" select="Exchange"/>

					<xsl:variable name="THIRDPARTY_MIC_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdPartyMIC_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@MIC=$PRANA_MIC_NAME]/@PranaMIC"/>
					</xsl:variable>

					<xsl:variable name="MIC">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_MIC_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_MIC_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

          <MIC>
           <!-- <xsl:choose> -->
              <!-- <xsl:when test="Exchange = 'NASDAQ'"> -->
                <!-- <xsl:value-of select="'XNGS'"/> -->
              <!-- </xsl:when> -->
              
              <!-- <xsl:otherwise> -->
                <!-- <xsl:value-of select="''"/> -->
              <!-- </xsl:otherwise> -->
            <!-- </xsl:choose> -->
			<xsl:value-of select="$MIC"/>
          </MIC>

          <ExchRate>
            <xsl:choose>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward') and CurrencySymbol !='USD'">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExchRate>
          
        
          <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = $PB_NAME]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = ''">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Broker>
            <xsl:value-of select="$varCounterParty"/>
          </Broker>
			<xsl:variable name="varSDate">
							<xsl:value-of select="substring(SettlementDate,4,2)"/>
						</xsl:variable>

						<xsl:variable name="varSMonth">
							<xsl:value-of select="substring(SettlementDate,1,2)"/>
						</xsl:variable>

						<xsl:variable name="varSYear">
							<xsl:value-of select="substring(SettlementDate,7,4)"/>
						</xsl:variable>
						
						<xsl:variable name="varSettlementDate">
							<xsl:value-of select="concat($varSDate,'/', $varSMonth, '/', $varSYear)"/>
						</xsl:variable>
          <SettleDate>
            <xsl:value-of select="$varSettlementDate"/> 
          </SettleDate>

          <ExternalTradeID>
            <xsl:value-of select="PBUniqueID"/>
          </ExternalTradeID>

          <Fund>
            <xsl:value-of select="'CH'"/>
          </Fund>

          <Units>
            <xsl:value-of select="AllocatedQty"/>
          </Units>

          <PriceNative>
            <xsl:value-of select="AveragePrice"/>
          </PriceNative>

          <GrossNative>
            <xsl:value-of select="GrossAmount"/>
          </GrossNative>

          <BrokerageNative>
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </BrokerageNative>

          <StDutyNative>
            <xsl:value-of select="StampDuty"/>
          </StDutyNative>

          <OtherChargeNative>
            <xsl:value-of select="OtherBrokerFee"/>
          </OtherChargeNative>

          <NetAmountNative>
            <xsl:value-of select="NetAmount"/>
          </NetAmountNative>

          <GSTPaidAUD>
            <xsl:value-of select="''"/>
          </GSTPaidAUD>

          <FXCounterCurrency>  
			<xsl:choose>		  
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="SettlCurrency"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXCounterCurrency>

          <FXCounterAmt>
            <xsl:choose>
              <xsl:when test="Asset = 'FX' or  Asset = 'FXForward'">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXCounterAmt>

          <AccruedInterest>
            <xsl:value-of select="''"/>
          </AccruedInterest>

          <ISIN>
            <xsl:choose>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward') and CurrencySymbol !='USD'">
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:when>
              <xsl:when test="(Asset = 'FX' or  Asset = 'FXForward') and CurrencySymbol ='USD'">
                <xsl:value-of select="SettlCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ISIN"/>
              </xsl:otherwise>
            </xsl:choose>
          </ISIN>
          
          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'AMEND'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CAN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <TradeStatus>
            <xsl:value-of select="$varTaxlotState"/>
          </TradeStatus>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>