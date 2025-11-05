<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

 <xsl:template name="MonthName">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month=01">
        <xsl:value-of select="'Jan'"/>
      </xsl:when>
      <xsl:when test="$Month=02">
        <xsl:value-of select="'Feb'"/>
      </xsl:when>
      <xsl:when test="$Month=03">
        <xsl:value-of select="'Mar'"/>
      </xsl:when>
      <xsl:when test="$Month=04">
        <xsl:value-of select="'Apr'"/>
      </xsl:when>
      <xsl:when test="$Month=05">
        <xsl:value-of select="'May'"/>
      </xsl:when>
      <xsl:when test="$Month=06">
        <xsl:value-of select="'Jun'"/>
      </xsl:when>
      <xsl:when test="$Month=07">
        <xsl:value-of select="'Jul'"/>
      </xsl:when>
      <xsl:when test="$Month=08">
        <xsl:value-of select="'Aug'"/>
      </xsl:when>
      <xsl:when test="$Month=09">
        <xsl:value-of select="'Sep'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'Oct'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'Nov'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'Dec'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <FileHeader>
            <xsl:value-of select="'true'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select="'true'"/>
          </FileFooter>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

       <xsl:variable name="varPosition" select="position()" />
          <LOCALREF>
		  <xsl:choose>
              <xsl:when test="$varPosition &lt; 10">                
				 <!-- <xsl:value-of select="concat('BYC',LOCALREF,'0',$varPosition)"/> -->
				 <xsl:value-of select="concat('BYC','0',$varPosition)"/>
              </xsl:when>

              <xsl:otherwise>
                <!-- <xsl:value-of select="concat('BYC',LOCALREF,$varPosition)"/> -->
				<xsl:value-of select="concat('BYC',$varPosition)"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </LOCALREF>

          <CFID>
            <xsl:choose>
              <xsl:when test="$varPosition &lt; 10">                
				 <!-- <xsl:value-of select="concat('BYC',LOCALREF,'0',$varPosition)"/> -->
				 <xsl:value-of select="concat('BYC','0',$varPosition)"/>
              </xsl:when>

              <xsl:otherwise>
                <!-- <xsl:value-of select="concat('BYC',LOCALREF,$varPosition)"/> -->
				<xsl:value-of select="concat('BYC',$varPosition)"/>
              </xsl:otherwise>
            </xsl:choose>
          </CFID>

          <ROUTECD>
            <xsl:value-of select="'PSHG'"/>
          </ROUTECD>

              <!--<TIRORDERID>            
             <xsl:value-of select="concat(substring(translate(TradeDate,'-',''),1,4),substring(translate(TradeDate,'-',''),6,1),
                          substring(Symbol_PK,string-length(Symbol_PK)-1,2),BrokerID)"/>
          </TIRORDERID>-->

          <TIRORDERID>
            <xsl:value-of select="UniqueID"/>
          </TIRORDERID>
	
          <TIRPIECE>
            <xsl:value-of select="''"/>
          </TIRPIECE>

          <TIRSEQ>
            <xsl:value-of select="''"/>
          </TIRSEQ>

          <SECIDTYPE>
                <xsl:choose>
                  <xsl:when test="TradeCurrency = 'USD'">
                    <xsl:choose>
                       <xsl:when test="contains(Asset,'Option') and OSIOptionSymbol != '' ">
                        <xsl:value-of select="'O'"/>
                      </xsl:when>
                        <xsl:when test="contains(Asset,'Option') and OSIOptionSymbol = '' ">
                        <xsl:value-of select="'S'"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity' and Symbol != ''">
                        <xsl:value-of select="'S'"/>
                        </xsl:when>
                       <xsl:when test="Asset='FixedIncome' and CUSIP != ''">
                        <xsl:value-of select="'C'"/>                      
                      </xsl:when>                                        
                      <xsl:otherwise>
                        <xsl:value-of select="'S'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>                     
                      <xsl:when test="Asset = 'Equity' and ISIN !=''">
                        <xsl:value-of select="'N'"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity' and SEDOL != ''">
                        <xsl:value-of select = "'D'"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity'">
                        <xsl:value-of select="'S'"/>
                      </xsl:when>
                     <xsl:otherwise>
                        <xsl:value-of select="'S'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </SECIDTYPE>

              <SECURITYID>
                <xsl:choose>
                  <xsl:when test="TradeCurrency = 'USD'">
                    <xsl:choose>
                       <xsl:when test="contains(Asset,'Option') and OSIOptionSymbol != '' ">
                        <xsl:value-of select="OSIOptionSymbol"/>
                      </xsl:when>
                        <xsl:when test="contains(Asset,'Option') and OSIOptionSymbol = '' ">
                        <xsl:value-of select="Symbol"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity' and Symbol != ''">
                        <xsl:value-of select="Symbol"/>
                        </xsl:when>
                       <xsl:when test="Asset='FixedIncome' and CUSIP != ''">
                        <xsl:value-of select="CUSIP"/>                      
                      </xsl:when>
                      <xsl:when test="Asset='FixedIncome' and CUSIP = ''">
                        <xsl:value-of select="Symbol"/>                      
                      </xsl:when>     
                      <xsl:otherwise>
                        <xsl:value-of select="Symbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>                     
                      <xsl:when test="Asset = 'Equity' and ISIN != ''">
                        <xsl:value-of select="ISIN"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity' and SEDOL != ''">
                        <xsl:value-of select = "SEDOL"/>
                      </xsl:when>
                      <xsl:when test="Asset='Equity'">
                        <xsl:value-of select="Symbol"/>
                      </xsl:when>
                       <xsl:when test="Asset='FixedIncome' and CUSIP != ''">
                        <xsl:value-of select="CUSIP"/>                      
                      </xsl:when>
                      <xsl:when test="Asset='FixedIncome' and CUSIP = ''">
                        <xsl:value-of select="Symbol"/>                      
                      </xsl:when>     
                     <xsl:otherwise>
                        <xsl:value-of select="Symbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </SECURITYID>

          <xsl:variable name="varCounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <DESCRIPTION1>
            <xsl:value-of select="concat('PB',' ',$varCounterParty)"/>
          </DESCRIPTION1>

          <DESCRIPTION2>
            <xsl:value-of select="''"/>
          </DESCRIPTION2>

          <DESCRIPTION3>            
                <xsl:value-of select="''"/>              
          </DESCRIPTION3>

          <DESCRIPTION4>
            <xsl:value-of select="''"/>
          </DESCRIPTION4>


          <xsl:variable name="varMonthCode">
            <xsl:call-template name="MonthName">
              <xsl:with-param name="Month" select="substring-before(TradeDate,'-')"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varSMonthCode">
            <xsl:call-template name="MonthName">
              <xsl:with-param name="Month" select="substring-before(SettlementDate,'-')"/>
            </xsl:call-template>
          </xsl:variable>

          <TRADEDATE>
            <xsl:value-of select="concat(substring-before(substring-after(TradeDate,'-'),'-'),'-',$varMonthCode,'-',substring-after(substring-after(TradeDate,'-'),'-'))"/>
          </TRADEDATE>


          <SETLDATE>
            <xsl:value-of select="concat(substring-before(substring-after(SettlementDate,'-'),'-'),'-',$varSMonthCode,'-',substring-after(substring-after(SettlementDate,'-'),'-'))"/>
          </SETLDATE>

          <QUANTITY>
            <xsl:value-of select="Quantity"/>
          </QUANTITY>

          <QUANTITYDESC>
            <xsl:value-of select="''"/>
          </QUANTITYDESC>

          <NETMONEY>
            <xsl:value-of select="''"/>
          </NETMONEY>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Pershing']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          
          <xsl:variable name="varAccountName">
            <xsl:choose>              
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Sell'">
                    <xsl:value-of select="concat($THIRDPARTY_FUND_CODE,'2')"/>
                  </xsl:when>

                  <xsl:when test="Side='Buy to Open' or Side='Sell to Close'">
                    <xsl:value-of select="concat($THIRDPARTY_FUND_CODE,'2')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat($THIRDPARTY_FUND_CODE,'3')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>          
       
          </xsl:variable>		  
		  
            <CASHACCOUNT>
               <!-- <xsl:choose> -->
              <!-- <xsl:when test="CustomOrdering='1'"> -->
                <!-- <xsl:choose> -->
                  <!-- <xsl:when test="Side='Buy' or Side='Sell'"> -->
                    <!-- <xsl:value-of select="concat('3DT829174','2')"/> -->
                  <!-- </xsl:when> -->

                  <!-- <xsl:when test="Side='Buy to Open' or Side='Sell to Close'"> -->
                    <!-- <xsl:value-of select="concat('3DT829174','2')"/> -->
                  <!-- </xsl:when> -->
                  <!-- <xsl:otherwise>                    -->
                    <!-- <xsl:value-of select="concat('3DT829174','3')"/> -->
                  <!-- </xsl:otherwise> -->
                <!-- </xsl:choose> -->
                
              <!-- </xsl:when> -->
              <!-- <xsl:otherwise> -->
                <!-- <xsl:choose> -->
                  <!-- <xsl:when test="CustomOrdering='2' and $varAccountName != ''"> -->
                    <!-- <xsl:value-of select="$varAccountName"/> -->
                  <!-- </xsl:when> -->
                  <!-- <xsl:otherwise> -->
                    <!-- <xsl:value-of select="AccountName"/> -->
                  <!-- </xsl:otherwise> -->
                <!-- </xsl:choose> -->
              <!-- </xsl:otherwise> -->
            <!-- </xsl:choose> -->
			<xsl:value-of select="'XVL8966812'"/>
             </CASHACCOUNT>

           

        <SECACCOUNT>
            <xsl:choose>
              <xsl:when test="CustomOrdering='1'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Sell'">
                    <xsl:value-of select="concat('3DT829174','0')"/>
                  </xsl:when>

                  <xsl:when test="Side='Buy to Open' or Side='Sell to Close'">
                    <xsl:value-of select="concat('3DT829174','0')"/>
                  </xsl:when>
                  <xsl:otherwise>                   
                    <xsl:value-of select="concat('3DT829174','0')"/>
                  </xsl:otherwise>
                </xsl:choose>
                
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="CustomOrdering='2' and $varAccountName != ''">
                    <xsl:value-of select="$varAccountName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AccountName"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </SECACCOUNT>


          <TRADECURRID>
            <xsl:value-of select="TradeCurrency"/>
          </TRADECURRID>

          <SETLCURRID>
            <xsl:value-of select="SettleCurrency"/>
          </SETLCURRID>

          <BSIND>
            <xsl:choose>
              <xsl:when test="CustomOrdering='1'">
                <xsl:choose>
                  <xsl:when test="contains(Side,'Buy')">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:when test="contains(Side,'Sell')">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="contains(Side,'Buy')">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="contains(Side,'Sell')">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
            
          </BSIND>


          <INSTTYP>
            <xsl:value-of select ="'N'"/>
          </INSTTYP>


          <xsl:variable name="varSettFxAmt">
                <xsl:choose>
                  <xsl:when test="SettleCurrency != TradeCurrency">
                    <xsl:choose>
                      <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                        <xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:choose>
                  <xsl:when test="SettleCurrency = TradeCurrency">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varSettFxAmt"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <PRICE>
                <xsl:value-of select="format-number($varPrice,'0.#######')"/>
              </PRICE>


          <xsl:variable name="varFXRate">
                <xsl:choose>
                  <xsl:when test="SettleCurrency != TradeCurrency">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varCommission">
                <xsl:value-of select="Commission"/>
              </xsl:variable>

              <xsl:variable name="varCommission1">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varCommission"/>
                  </xsl:when>
                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
                    <xsl:value-of select="$varCommission * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
                    <xsl:value-of select="$varCommission div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <COMMISSION>
                <xsl:value-of select="format-number($varCommission1,'0.##')"/>
              </COMMISSION>

              <STAMPTAX>
                <xsl:value-of select="StampDuty"/>
              </STAMPTAX>

              <xsl:variable name="varOtherFees">
                <xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>
              </xsl:variable>

            <xsl:variable name="varOtherFees1">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varOtherFees"/>
                  </xsl:when>
                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
                    <xsl:value-of select="$varOtherFees * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
                    <xsl:value-of select="$varOtherFees div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
			  
          <LOCALCHGS>
            <xsl:value-of select="''"/>
          </LOCALCHGS>

          <INTEREST>
            <xsl:value-of select="''"/>
          </INTEREST>

          <PRINCIPAL>
            <xsl:value-of select="''"/>
          </PRINCIPAL>

          <SECFEE>
                <xsl:value-of select="''"/>
          </SECFEE>


          <EXECBROKER>
               <xsl:choose>
                  <xsl:when test="contains(Asset,'Option')">
                    <xsl:value-of select="CounterParty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
          </EXECBROKER>

          <BROKEROS>
            <xsl:value-of select="''"/>
          </BROKEROS>

          <TRAILERCD1>
            <xsl:value-of select="''"/>
          </TRAILERCD1>

          <TRAILERCD2>
            <xsl:value-of select="''"/>
          </TRAILERCD2>

            <TRAILERCD3>
                 <xsl:choose>
                  <xsl:when test=" Side='Sell short'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Buy to Close'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                </xsl:choose>
              </TRAILERCD3>

		    <BLOTTERCD>
                <xsl:choose>
                  <xsl:when test=" TradeCurrency ='USD'">
                    <xsl:value-of select="'49'"/>
                  </xsl:when>
                 <xsl:otherwise>
                    <xsl:value-of select="'40'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BLOTTERCD>

          <CLRNGHSE>
            <xsl:value-of select="'Y'"/>
          </CLRNGHSE>


          <CLRAGNTCD>
            <xsl:value-of select="CounterParty"/>
          </CLRAGNTCD>

          <CLRAGNT1>
            <xsl:value-of select="''"/>
          </CLRAGNT1>

          <CLRAGNT2>
            <xsl:value-of select="''"/>
          </CLRAGNT2>

          <CLRAGNT3>
            <xsl:value-of select="''"/>
          </CLRAGNT3>

          <CLRAGNT4>
            <xsl:value-of select="''"/>
          </CLRAGNT4>

          <CNTRPRTYCD>
            <xsl:value-of select="''"/>
          </CNTRPRTYCD>


          <CNTRPTY1>
            <xsl:value-of select="''"/>
          </CNTRPTY1>

          <CNTRPTY2>
            <xsl:value-of select="''"/>
          </CNTRPTY2>

          <CNTRPTY3>
            <xsl:value-of select="''"/>
          </CNTRPTY3>

          <CNTRPTY4>
            <xsl:value-of select="''"/>
          </CNTRPTY4>

          <INSTRUCT>
            <xsl:value-of select="''"/>
          </INSTRUCT>

          <CEDELAKV>
            <xsl:value-of select="''"/>
          </CEDELAKV>


          <ORIGLOCALREF>
            <xsl:choose>
              <xsl:when test="$varPosition &lt; 10">                
				 <xsl:value-of select="concat('BYC','0',$varPosition)"/>
				 <!-- <xsl:value-of select="concat('BYC',LOCALREF,'0',$varPosition)"/> -->
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="concat('BYC',$varPosition)"/>
				<!-- <xsl:value-of select="concat('BYC',LOCALREF,$varPosition)"/> -->
              </xsl:otherwise>
            </xsl:choose>
          </ORIGLOCALREF>

          <NOTES>
            <xsl:value-of select="''"/>
          </NOTES>

          <FILLER>
            <xsl:value-of select="''"/>
          </FILLER>

          <FILLER1>
            <xsl:value-of select="''"/>
          </FILLER1>

          <RR>
            <xsl:value-of select="''"/>
          </RR>

          <SETLCOUNTRYCD>
            <xsl:value-of select="substring(SettleCurrency,1,2)"/>
          </SETLCOUNTRYCD>

          <INSTRUMENTTYPE>
            <xsl:value-of select="''"/>
          </INSTRUMENTTYPE>


          <COMMISSIONRATE>
            <xsl:value-of select="''"/>
          </COMMISSIONRATE>

          <COMPANYNO>
            <xsl:value-of select="''"/>
          </COMPANYNO>

          <Filler2>
            <xsl:value-of select="''"/>
          </Filler2>

          <Filler3>
            <xsl:value-of select="''"/>
          </Filler3>

          <Filler4>
            <xsl:value-of select="''"/>
          </Filler4>

          <Filler5>
            <xsl:value-of select="''"/>
          </Filler5>

          <Filler6>
            <xsl:value-of select="''"/>
          </Filler6>


          <GPF2IDCode>
            <xsl:value-of select="''"/>
          </GPF2IDCode>


          <GPF2Amount>
            <xsl:value-of select="''"/>
          </GPF2Amount>

          <GPF2CurrencyCode>
            <xsl:value-of select="''"/>
          </GPF2CurrencyCode>

          <GPF2AddSubtract>
            <xsl:value-of select="''"/>
          </GPF2AddSubtract>

          <GPF3IDCode>
            <xsl:value-of select="''"/>
          </GPF3IDCode>

          <GPF3Amount>
            <xsl:value-of select="''"/>
          </GPF3Amount>

          <GPF3CurrencyCode>
            <xsl:value-of select="''"/>
          </GPF3CurrencyCode>

          <GPF3AddSubtract>
            <xsl:value-of select="''"/>
          </GPF3AddSubtract>

          <GPF4IDCode>
            <xsl:value-of select="''"/>
          </GPF4IDCode>

          <GPF4Amount>
            <xsl:value-of select="''"/>
          </GPF4Amount>

          <GPF4CurrencyCode>
            <xsl:value-of select="''"/>
          </GPF4CurrencyCode>

          <GPF4AddSubtract>
            <xsl:value-of select="''"/>
          </GPF4AddSubtract>

          <GPF5IDCode>
            <xsl:value-of select="''"/>
          </GPF5IDCode>

          <GPF5Amount>
            <xsl:value-of select="''"/>
          </GPF5Amount>

          <GPF5CurrencyCode>
            <xsl:value-of select="''"/>
          </GPF5CurrencyCode>

          <GPF5AddSubtract>
            <xsl:value-of select="''"/>
          </GPF5AddSubtract>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>