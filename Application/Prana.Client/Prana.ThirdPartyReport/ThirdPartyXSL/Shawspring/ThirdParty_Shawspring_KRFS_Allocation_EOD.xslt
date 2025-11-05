<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>     






      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'SSP Fund Cowen PHW006826']">  
        <ThirdPartyFlatFileDetail>


          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PB_NAME" select="'SS&amp;C'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          <TradeType>
		       <xsl:choose>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select ="'Buy to Cover'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </TradeType>
          
          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Amended'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Cancel'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Action>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </Action>
          
          <Portfolio>
            <xsl:value-of select="'SHA'"/>
          </Portfolio>

          <LocationAccount>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </LocationAccount>

          <Strategy>
            <xsl:value-of select="'NONE'"/>
          </Strategy>
		  
		    
			

          <Investment>
            <xsl:choose>
              <xsl:when test="CurrencySymbol='USD'">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring-before(BBCode,' Equity')"/>
              </xsl:otherwise>
            </xsl:choose>
          </Investment>

          <EventDate>
            <xsl:value-of select="TradeDate"/>
          </EventDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <ActualSettleDate>
            <xsl:value-of select="SettlementDate"/>
          </ActualSettleDate>

          <Comments>
            <xsl:value-of select="''"/>
          </Comments>

          <UserTranID1>
            <xsl:value-of select="EntityID"/>
          </UserTranID1>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>


          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <Price>
		   <xsl:choose>
              <xsl:when test="number($varPrice)">
                <xsl:value-of select="format-number($varPrice,'##.######')"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </Price>

          <NetCounterAmount>
            <xsl:value-of select="'CALC'"/>
          </NetCounterAmount>

          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="Commission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="varCommission">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Commission"/>
              </xsl:when>
			  
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="$Commission * $varFXRate"/>
              </xsl:when>
			  
				  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='D'">
                <xsl:value-of select="$Commission div $varFXRate"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
		  
          <TotCommission>
		  <xsl:choose>
              <xsl:when test="number($varCommission)">
                <xsl:value-of select="format-number($varCommission,'##.####')"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>		  
          </TotCommission>
		  
		     <xsl:variable name="Sec">
            <xsl:value-of select="number(SecFee)"/>
          </xsl:variable>
		  
		    <xsl:variable name="varSec">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Sec"/>
              </xsl:when>
			  
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="$Sec * $varFXRate"/>
              </xsl:when>
			  
				  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='D'">
                <xsl:value-of select="$Sec div $varFXRate"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SecFeeAmount>
		   <xsl:choose>
              <xsl:when test="number(SecFee)">
                <xsl:value-of select="format-number($varSec,'##.####')"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
					
          </SecFeeAmount>

          <PriceDenomination>
            <xsl:value-of select="SettlCurrency"/>
          </PriceDenomination>

          <CounterInvestment>
            <xsl:value-of select="CurrencySymbol"/>
          </CounterInvestment>

          <TradeFx>
		    <xsl:choose>
              <xsl:when test="FXRate_Taxlot='0'">
                <xsl:value-of select="''"/>
              </xsl:when>
			  
			   <xsl:when test="FXRate_Taxlot='1'">
                <xsl:value-of select="''"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
			
          </TradeFx>

		              <xsl:variable name = "PB_CounterParty">
							<xsl:value-of select="CounterParty"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BrokerCode">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PBBroker"/>
						</xsl:variable>

						
		  
          <Broker>
		  <xsl:choose>
								<xsl:when test="$PRANA_BrokerCode!=''">
									<xsl:value-of select="concat($PRANA_BrokerCode,'-ALGO-AMER')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($PB_CounterParty,'-ALGO-AMER')"/>
								</xsl:otherwise>
							</xsl:choose>
            
          </Broker>

          <ExpenseAmt>
            <xsl:value-of select="''"/>
          </ExpenseAmt>

          <ExpenseCode>
            <xsl:value-of select="''"/>
          </ExpenseCode>

          <Trader>
            <xsl:value-of select="'cps-shawspring1'"/>
          </Trader>

          <NetTradeAmount>
            <xsl:value-of select="'CALC'"/>
          </NetTradeAmount>

          <AssetType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select ="'EQT'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>                    
            </xsl:choose>          
          </AssetType>
		  
		   <xsl:variable name="NetAmount">
            <xsl:value-of select="NetAmount"/>
          </xsl:variable>
		  
		    <xsl:variable name="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$NetAmount"/>
              </xsl:when>
			  
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="$NetAmount * $varFXRate"/>
              </xsl:when>
			  
				  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='D'">
                <xsl:value-of select="$NetAmount div $varFXRate"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <USDSettlementAmount>
            <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
          </USDSettlementAmount>

          <Cusip>
            <xsl:value-of select="CUSIP"/>
          </Cusip>

          <Sedol>
            <xsl:value-of select="SEDOL"/>
          </Sedol>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>