<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>


        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <AccountID>
          <xsl:value-of select ="'Account ID'"/>
        </AccountID>

        <Transactiontype>
          <xsl:value-of select ="'Transaction type'"/>
        </Transactiontype>

        <Tradedate>
          <xsl:value-of select ="'Trade date'"/>
        </Tradedate>

        <Settlementdate>
          <xsl:value-of select ="'Settlement date'"/>
        </Settlementdate>

        <Cusip>
          <xsl:value-of select ="'Cusip'"/>
        </Cusip>

        <NameofSecurity>
          <xsl:value-of select ="'Name of Security'"/>
        </NameofSecurity>

        <ParValue>
          <xsl:value-of select ="'Par Value'"/>
        </ParValue>

        <Tradeprice>
          <xsl:value-of select ="'Trade price'"/>
        </Tradeprice>

        <Principle>
          <xsl:value-of select ="'Principle'"/>
        </Principle>

        <Commission>
          <xsl:value-of select ="'Commission'"/>
        </Commission>
		
        <Interest>
          <xsl:value-of select ="'Interest'"/>
        </Interest>

        <Othercharges>
          <xsl:value-of select ="'Other charges'"/>
        </Othercharges>

        <Settlementamount>
          <xsl:value-of select ="'Settlement amount'"/>
        </Settlementamount>

        <!-- <ExecutingBrokerinstructions> -->
          <!-- <xsl:value-of select ="'Executing Broker instructions'"/> -->
        <!-- </ExecutingBrokerinstructions> -->

        <!-- <ClearingBrokerinstructions> -->
          <!-- <xsl:value-of select ="'Clearing Broker instructions'"/> -->
        <!-- </ClearingBrokerinstructions> -->

        <ExecutingBrokername>
          <xsl:value-of select ="'Executing Broker name'"/>
        </ExecutingBrokername>

        <ExecutingBrokerAccount>
          <xsl:value-of select ="'Executing Broker Account'"/>
        </ExecutingBrokerAccount>

        <ClearingBrokername>
          <xsl:value-of select ="'Clearing Broker name'"/>
        </ClearingBrokername>

        <ClearingBrokerAccount>
          <xsl:value-of select ="'Clearing Broker Account'"/>
        </ClearingBrokerAccount>

        <Tradetype>
          <xsl:value-of select ="'Trade type'"/>
        </Tradetype>

		<FXinstruction>
		<xsl:value-of select ="'FX instruction'"/>
		</FXinstruction>
		
		 <Netamount>
		  <xsl:value-of select ="'Net amount'"/>
		  </Netamount>
		  
		  <TradeCurrency>
		  <xsl:value-of select ="'Trade Currency'"/>
		   </TradeCurrency>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>


          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'NT'"/>
          </xsl:variable>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <AccountID>
            <xsl:choose>
              <xsl:when test ="$PB_FUND_NAME != ''">
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountID>
          <xsl:variable name="Side1">
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                <xsl:value-of select ="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                <xsl:value-of select ="'SELL SHORT'"/>
              </xsl:when>
              
              <xsl:otherwise>

                <xsl:value-of select="Side"/>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Transactiontype>
            <xsl:value-of select ="$Side1"/>
          </Transactiontype>
          <xsl:variable name="Trade_Day" select="substring(TradeDate,4,2)"/>
          <xsl:variable name="Trade_Month" select="substring(TradeDate,1,2)"/>
          <xsl:variable name="Trade_Year" select="substring(TradeDate,7,4)"/>
          
          <Tradedate>
            <xsl:value-of select="concat($Trade_Year,$Trade_Month,$Trade_Day)"/>
          </Tradedate>

          <xsl:variable name="STrade_Day" select="substring(SettlementDate,4,2)"/>
          <xsl:variable name="STrade_Month" select="substring(SettlementDate,1,2)"/>
          <xsl:variable name="STrade_Year" select="substring(SettlementDate,7,4)"/>
          <Settlementdate>
            <xsl:value-of select="concat($STrade_Year,$STrade_Month,$STrade_Day)"/>
          </Settlementdate> 
		  
		  <xsl:variable name="varCusipSedol">
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD' ">
                <xsl:value-of select ="CUSIP"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="SEDOL"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <Cusip>
            <xsl:value-of select ="$varCusipSedol"/>
          </Cusip>
          
          <NameofSecurity>
            <xsl:value-of select ="FullSecurityName"/>
          </NameofSecurity>
          
          <ParValue>
            <xsl:value-of select ="AllocatedQty"/>
          </ParValue>
          
          <Tradeprice>
            <xsl:value-of select ="AveragePrice"/>
          </Tradeprice>
          
          <Principle>
            <xsl:value-of select ="GrossAmount"/>
          </Principle>
          
          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'0.###')"/>
          </Commission>		
		            
          <Interest>
            <xsl:value-of select ="''"/>
          </Interest>
          
          <Othercharges>
            <xsl:value-of select ="OtherBrokerFee"/>
          </Othercharges>
          
          <Settlementamount>
            <xsl:value-of select ="NetAmount"/>
          </Settlementamount>
          
          <!-- <ExecutingBrokerinstructions> -->
            <!-- <xsl:value-of select ="''"/> -->
          <!-- </ExecutingBrokerinstructions> -->
          
          <!-- <ClearingBrokerinstructions> -->
            <!-- <xsl:value-of select ="''"/> -->
          <!-- </ClearingBrokerinstructions> -->

           <xsl:variable name="PRANA_EXEC_CURRENCY_NAME" select="CurrencySymbol"/> 
              <xsl:variable name="THIRDPARTY_EXEC_CURRENCY">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyMapping.xml')/ExecutingMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PRANA_EXEC_CURRENCY_NAME]/@ExecutingBrokerCurrency"/>
              </xsl:variable>
		  
          <ExecutingBrokername>
            <xsl:value-of select="$THIRDPARTY_EXEC_CURRENCY"/>
          </ExecutingBrokername>
		  
		  <xsl:variable name="varExecutingBrokerAccount">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyMapping.xml')/ExecutingMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PRANA_EXEC_CURRENCY_NAME]/@ExecutingBrokerAccount"/>
              </xsl:variable>
          
          <ExecutingBrokerAccount>
            <xsl:value-of select ="$varExecutingBrokerAccount"/>
          </ExecutingBrokerAccount>
          
		      <xsl:variable name="PRANA_CLEAR_CURRENCY_NAME" select="CurrencySymbol"/> 
              <xsl:variable name="THIRDPARTY_CLEAR_CURRENCY">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyMapping.xml')/ExecutingMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PRANA_CLEAR_CURRENCY_NAME]/@ClearingBrokerCurrency"/>
              </xsl:variable>
			  
			  
		  
          <ClearingBrokername>
            <xsl:value-of select="$THIRDPARTY_CLEAR_CURRENCY"/>
          </ClearingBrokername>
		  
		  <xsl:variable name="varClearingBrokerAccount">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyMapping.xml')/ExecutingMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PRANA_EXEC_CURRENCY_NAME]/@ClearingBrokerAccount"/>
              </xsl:variable>
          
          <ClearingBrokerAccount>
            <xsl:value-of select ="$varClearingBrokerAccount"/>
          </ClearingBrokerAccount>
          
          <Tradetype>
            <xsl:value-of select ="Asset"/>
          </Tradetype>

		  
		  <xsl:variable name="varFXinstruction">
            <xsl:choose>
              <xsl:when test="CurrencySymbol !='USD' ">
                <xsl:value-of select ="'NT to execute FX vs USD'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
		  
		  <FXinstruction>
		    <xsl:value-of select ="$varFXinstruction"/>
		  </FXinstruction>
		  
		  <Netamount>
		  <xsl:value-of select ="NetAmount"/>
		  </Netamount>
		  
		  <TradeCurrency>
		   <xsl:value-of select ="CurrencySymbol"/>
		   </TradeCurrency>
		  

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

         </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>