<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

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

       

        <AccountNumber>
          <xsl:value-of select="'Account Number'"/>
        </AccountNumber>

        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Side>
          <xsl:value-of select="'Buy/Sell/SellShort/CoverShort'"/>
        </Side>

        <Ticker>
          <xsl:value-of select="'Ticker'"/>
        </Ticker>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <Issuer>
          <xsl:value-of select="'Issuer'"/>
        </Issuer>

        <OtherDescription>
          <xsl:value-of select="'Other Description'"/>
        </OtherDescription>

        <Coupon>
          <xsl:value-of select="'Coupon'"/>
        </Coupon>

        <Maturity>
          <xsl:value-of select="'Maturity'"/>
        </Maturity>

        <OriginalFace>
          <xsl:value-of select ="'Original Face'"/>
        </OriginalFace>

        <TradeQuantity>
          <xsl:value-of select ="'Current Face/Trade Quantity'"/>
        </TradeQuantity>


        <Factor>
          <xsl:value-of select ="'Factor'"/>
        </Factor>

        <Price>
          <xsl:value-of select ="'Price'"/>
        </Price>

        <TradeFXRate>
          <xsl:value-of select ="'Trade FX Rate'"/>
        </TradeFXRate>

        <TradeCurrency>
          <xsl:value-of select ="'Trade Currency'"/>
        </TradeCurrency>

        <SettleCurrency>
          <xsl:value-of select ="'Settle Currency'"/>
        </SettleCurrency>

        <EffectiveYield>
          <xsl:value-of select ="'Effective Yield'"/>
        </EffectiveYield>

        <Commission>
          <xsl:value-of select ="'Commission'"/>
        </Commission>

        <SECFee>
          <xsl:value-of select ="'SEC Fee'"/>
        </SECFee>

        <OtherFees>
          <xsl:value-of select ="'Other Fees'"/>
        </OtherFees>

        <AccruedInterest>
          <xsl:value-of select ="'Accrued Interest'"/>
        </AccruedInterest>

        <NetTradeCash>
          <xsl:value-of select ="'Net Trade Cash'"/>
        </NetTradeCash>

        <Counterparty>
          <xsl:value-of select ="'Counterparty Broker'"/>
        </Counterparty>

		  <AssetType>
			  <xsl:value-of select ="'Asset Type'"/>
		  </AssetType>


        <!-- system inetrnal use-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name ="varExpirationDate">
            <xsl:if test ="Asset = 'EquityOption' or Asset = 'FutureOption' or Asset = 'Future' or Asset = 'FixedIncome'">
              <xsl:value-of select ="ExpirationDate"/>
            </xsl:if>
          </xsl:variable>

          <AccountNumber>
            <xsl:value-of select="FundName"/>
          </AccountNumber>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Side>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:choose>
						  <xsl:when test="Side='Buy to Open' or Side='Buy'">
							  <xsl:value-of select="'BUY'"/>
						  </xsl:when>

						  <xsl:when test="Side='Sell to Open'">
							  <xsl:value-of select="'Sell Short'"/>
						  </xsl:when>


						  <xsl:when test="Side='Sell to Close' or Side='Sell'">
							  <xsl:value-of select="'SELL'"/>
						  </xsl:when>

						  <xsl:when test="Side='Buy to Close'">
							  <xsl:value-of select="'COVER'"/>
						  </xsl:when>
						  
					  </xsl:choose>
				  </xsl:when>

				  <xsl:otherwise>
					  <xsl:choose>
						  <xsl:when test="Side='Buy'">
							  <xsl:value-of select="'BUY'"/>
						  </xsl:when>

						  <xsl:when test="Side='Sell'">
							  <xsl:value-of select="'SELL'"/>
						  </xsl:when>

						  <xsl:when test="Side='Buy to Close'">
							  <xsl:value-of select="'COVER'"/>
						  </xsl:when>

						  <xsl:when test="Side='Sell short'">
							  <xsl:value-of select="'Sell Short'"/>
						  </xsl:when>
					  </xsl:choose>
				 
					  
				  </xsl:otherwise>
			  </xsl:choose>
			  
		  </Side>

          <Ticker>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="OSIOptionSymbol"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </Ticker>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <Issuer>
            <xsl:value-of select="''"/>
          </Issuer>

          <OtherDescription>
            <xsl:value-of select="FullSecurityName"/>
          </OtherDescription>

          <Coupon>
            <xsl:value-of select="Coupon"/>
          </Coupon>

          <Maturity>
            <xsl:value-of select="$varExpirationDate"/>
          </Maturity>

          <OriginalFace>
            <xsl:value-of select ="''"/>
          </OriginalFace>

          <TradeQuantity>
            <xsl:value-of select ="AllocatedQty"/>
          </TradeQuantity>


          <Factor>
            <xsl:value-of select ="AssetMultiplier"/>
          </Factor>

          <Price>
            <xsl:value-of select ="AveragePrice"/>
          </Price>

          <TradeFXRate>
            <xsl:value-of select ="ForexRate"/>
          </TradeFXRate>

          <TradeCurrency>
            <xsl:value-of select ="CurrencySymbol"/>
          </TradeCurrency>

          <SettleCurrency>
            <xsl:value-of select ="'USD'"/>
          </SettleCurrency>

          <EffectiveYield>
            <xsl:value-of select ="''"/>
          </EffectiveYield>

          <Commission>
            <xsl:value-of select ="CommissionCharged"/>
          </Commission>

          <SECFee>
            <xsl:value-of select ="StampDuty"/>
          </SECFee>

          <OtherFees>
            <xsl:value-of select ="OtherBrokerFee"/>
          </OtherFees>

          <AccruedInterest>
            <xsl:value-of select ="AccruedInterest"/>
            </AccruedInterest>

          <NetTradeCash>
            <xsl:value-of select ="NetAmount"/>
          </NetTradeCash>

          <Counterparty>
            <xsl:value-of select ="CounterParty"/>
          </Counterparty>
			<AssetType>
				<xsl:value-of select ="Asset"/>
			</AssetType>


					<!-- system inetrnal use-->
            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>
          </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
