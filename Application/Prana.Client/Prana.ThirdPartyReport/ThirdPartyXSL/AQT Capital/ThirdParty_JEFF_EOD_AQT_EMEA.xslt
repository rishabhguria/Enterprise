<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
	  
	  
      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='JEFF'][VenueID !='181' and VenueID !='182' and VenueID !='191' and VenueID !='192' and VenueID !='193']
					[CurrencySymbol = 'AED'   or CurrencySymbol = 'AMD'   or CurrencySymbol = 'AOA'   or CurrencySymbol = 'AZN'   or CurrencySymbol = 'BAM'   or CurrencySymbol = 'BGN'   or CurrencySymbol = 'BHD'   or CurrencySymbol = 'BIF'   or CurrencySymbol = 'BWP'   or CurrencySymbol = 'BYN'   or CurrencySymbol = 'CDF'   or CurrencySymbol = 'CHF'   or CurrencySymbol = 'CVE'   or CurrencySymbol = 'CZK'   or CurrencySymbol = 'DJF'   or CurrencySymbol = 'DKK'   or CurrencySymbol = 'DZD'   or CurrencySymbol = 'EGP'   or CurrencySymbol = 'ERN'   or CurrencySymbol = 'ETB'   or CurrencySymbol = 'EUR'   or CurrencySymbol = 'GBP'   or CurrencySymbol = 'GEL'   or CurrencySymbol = 'GHS'   or CurrencySymbol = 'GMD' 
					or CurrencySymbol = 'GNF'   or CurrencySymbol = 'HUF'   or CurrencySymbol = 'ILS'   or CurrencySymbol = 'IQD'   or CurrencySymbol = 'IRR'   or CurrencySymbol = 'ISK'   or CurrencySymbol = 'JOD'   or CurrencySymbol = 'KES'   or CurrencySymbol = 'KMF'   or CurrencySymbol = 'KWD'   or CurrencySymbol = 'KZT'   or CurrencySymbol = 'LBP'   or CurrencySymbol = 'LRD'   or CurrencySymbol = 'LSL'   or CurrencySymbol = 'LYD'   or CurrencySymbol = 'MAD'   or CurrencySymbol = 'MDL'   or CurrencySymbol = 'MGA'   or CurrencySymbol = 'MKD'   or CurrencySymbol = 'MRU'   or CurrencySymbol = 'MUR'   or CurrencySymbol = 'MWK'   or CurrencySymbol = 'MZN'   or CurrencySymbol = 'NAD'   or CurrencySymbol = 'NGN'   
					or CurrencySymbol = 'NOK'   or CurrencySymbol = 'OMR'   or CurrencySymbol = 'PLN'   or CurrencySymbol = 'QAR'   or CurrencySymbol = 'RON'   or CurrencySymbol = 'RSD'   or CurrencySymbol = 'RUB'   or CurrencySymbol = 'RWF'   or CurrencySymbol = 'SAR'   or CurrencySymbol = 'SCR'   or CurrencySymbol = 'SDG'   or CurrencySymbol = 'SEK'   or CurrencySymbol = 'SLE'   or CurrencySymbol = 'SOS'   or CurrencySymbol = 'SSP'   or CurrencySymbol = 'STN'   or CurrencySymbol = 'SYP'   or CurrencySymbol = 'SZL'   or CurrencySymbol = 'TND'   or CurrencySymbol = 'TRY'   or CurrencySymbol = 'TZS'   or CurrencySymbol = 'UAH'   or CurrencySymbol = 'UGX'   or CurrencySymbol = 'XAF'   or CurrencySymbol = 'XOF'   or CurrencySymbol = 'YER'   or CurrencySymbol = 'ZAR'   or CurrencySymbol = 'ZMW'   or CurrencySymbol = 'ZWL']">

        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <CancelIndicator>
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'N'"/>
                  </xsl:when>
                  
                  <xsl:when test="TaxLotState='Amemded'">
                    <xsl:value-of select ="''"/>
                  </xsl:when>

                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'X'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CancelIndicator>

              <JefferiesTradeId>
                <xsl:value-of select ="''"/>
              </JefferiesTradeId>

              <ClientTradeId>
                <xsl:value-of select ="EntityID"/>
              </ClientTradeId>

              <Moniker>
                <xsl:value-of select ="'P1754'"/>
              </Moniker>

              <!--Side Identifier-->

              <xsl:choose>
                <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                  <TransactionType>
                    <xsl:value-of select ="'BY'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'CS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'SL'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                  <TransactionType>
                    <xsl:value-of select ="'SS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:otherwise>
                  <TransactionType>
                    <xsl:value-of select="Side"/>
                  </TransactionType>
                </xsl:otherwise>
              </xsl:choose>

              <Quantity>
                <xsl:value-of select="OrderQty"/>
              </Quantity>

              <xsl:variable name ="varCheckSymbolUnderlying">
                <xsl:value-of select ="substring-before(Symbol,'-')"/>
              </xsl:variable>
              
              <xsl:choose>
                <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                  <InstrumentId>
                    <xsl:value-of select="SEDOL"/>
                  </InstrumentId>
                </xsl:when>
                
                <xsl:when test ="Asset ='EquityOption' ">
                  <InstrumentId>
                    <xsl:value-of select="OSIOptionSymbol"/>
                  </InstrumentId>
                </xsl:when>
                
                <xsl:otherwise>
                  <InstrumentId>
                    <xsl:value-of select="Symbol"/>
                  </InstrumentId>
                </xsl:otherwise>
              </xsl:choose>

              <Price>
                <xsl:value-of select="AvgPrice"/>
              </Price>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JEFF']/FundData[@PranaFund=$PB_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
              <AccountId>
				  <xsl:choose>
					  <xsl:when test ="AccountID ='1'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754UBS'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201636'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:when test ="AccountID ='2'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754MSIL'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201634'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
              </AccountId>

              <xsl:variable name ="varCounterParty">               
                      <xsl:value-of select="'JBUY'"/>                   
              </xsl:variable>

              <ExecutingBroker>
                <xsl:value-of select ="$varCounterParty"/>
              </ExecutingBroker>
             

              <TradeDate>
                <xsl:value-of select="substring-before(TradeDate,'T')"/>
              </TradeDate>

              <SettleDate>
                <xsl:value-of select ="substring-before(SettlementDate,'T')"/>
              </SettleDate>

              <CommissionType>
                <xsl:value-of select="'T'"/>
              </CommissionType>

              
              <Commission>
                <xsl:value-of select="CommissionCharged  + SoftCommissionCharged"/>
              </Commission>

              <SellingMethod>
                <xsl:value-of select ="''"/>
              </SellingMethod>

              <Vs_purchases_Date>
                <xsl:value-of select="''"/>
              </Vs_purchases_Date>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementExchangeRate>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' and SettlCurrency='USD'">
                    <xsl:value-of select="1"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SettlementExchangeRate>

              <Exchange>
                <xsl:value-of select="''"/>
              </Exchange>

              <OtherFee>
                <xsl:value-of select="OrfFee + SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFees + TaxOnCommissions"/>
              </OtherFee>

              <Strategy>
                <xsl:value-of select="''"/>
              </Strategy>

              <LotNumber>
                <xsl:value-of select="0"/>
              </LotNumber>

              <LotQuantity>
                <xsl:value-of select="0"/>
              </LotQuantity>

              <Trader>
                <xsl:value-of select="''"/>
              </Trader>

              <Interest>
                <xsl:value-of select="0"/>
              </Interest>

              <Custodian>
                <xsl:value-of select="''"/>
              </Custodian>

              <WhenIssued>
                <xsl:value-of select="'N'"/>
              </WhenIssued>

             
              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

                <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

				  <TaxLotState>
					  <xsl:value-of select="'Deleted'"/>
				  </TaxLotState>

                <CancelIndicator>
                  <xsl:value-of select="'X'"/>
                </CancelIndicator>

                <JefferiesTradeId>
                  <xsl:value-of select ="''"/>
                </JefferiesTradeId>

                <ClientTradeId>
                  <xsl:value-of select ="EntityID"/>
                </ClientTradeId>

                <Moniker>
                  <xsl:value-of select ="'P1754'"/>
                </Moniker>

            
                <xsl:choose>
                  <xsl:when test="OldSide='Buy to Open' or OldSide='Buy' ">
                    <TransactionType>
                      <xsl:value-of select ="'BY'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Buy to Cover' or OldSide='Buy to Close' ">
                    <TransactionType>
                      <xsl:value-of select ="'CS'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Sell' or OldSide='Sell to Close' ">
                    <TransactionType>
                      <xsl:value-of select ="'SL'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Sell short' or OldSide='Sell to Open' ">
                    <TransactionType>
                      <xsl:value-of select ="'SS'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:otherwise>
                    <TransactionType>
                      <xsl:value-of select="OldSide"/>
                    </TransactionType>
                  </xsl:otherwise>
                </xsl:choose>

                <Quantity>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </Quantity>
				
				
                <xsl:variable name ="varCheckSymbolUnderlying">
                  <xsl:value-of select ="substring-before(Symbol,'-')"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                    <InstrumentId>
                      <xsl:value-of select="SEDOL"/>
                    </InstrumentId>
                  </xsl:when>
                  <xsl:when test ="Asset ='EquityOption' ">
                    <InstrumentId>
                      <xsl:value-of select="OSIOptionSymbol"/>
                    </InstrumentId>

                  </xsl:when>
                  <xsl:otherwise>
                    <InstrumentId>
                      <xsl:value-of select="Symbol"/>
                    </InstrumentId>
                  </xsl:otherwise>
                </xsl:choose>

                <Price>
                  <xsl:value-of select="OldAvgPrice"/>
                </Price>

           <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JEFF']/FundData[@PranaFund=$PB_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
				  <AccountId>
				  <xsl:choose>
					  <xsl:when test ="AccountID ='1'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754UBS'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201636'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:when test ="AccountID ='2'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754MSIL'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201634'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
              </AccountId>

                <xsl:variable name ="varCounterParty">
                  <xsl:choose>
                    <xsl:when test="VenueID='1'">
                      <xsl:value-of select="'JBUY'"/>
                    </xsl:when>
					<xsl:when test="VenueID='237'">
                      <xsl:value-of select="'ROTH-M'"/>
                    </xsl:when>
					
                    <xsl:otherwise>
                      <xsl:value-of select="'JBUY'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <ExecutingBroker>
                  <xsl:value-of select ="$varCounterParty"/>
                </ExecutingBroker>
               

                <TradeDate>
                  <xsl:value-of select="substring-before(OldTradeDate,'T')"/>
                </TradeDate>

                <SettleDate>
                  <xsl:value-of select ="substring-before(OldSettlementDate,'T')"/>
                </SettleDate>

                <CommissionType>
                  <xsl:value-of select="'T'"/>
                </CommissionType>

             
                <Commission>
                  <xsl:value-of select="OldCommission  + OldSoftCommission"/>
                </Commission>

                <SellingMethod>
                  <xsl:value-of select ="''"/>
                </SellingMethod>

                <Vs_purchases_Date>
                  <xsl:value-of select="''"/>
                </Vs_purchases_Date>

                <SettlementCurrency>
                  <xsl:value-of select="OldSettlCurrency"/>
                </SettlementCurrency>

                <SettlementExchangeRate>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD' and OldSettlCurrency='USD'">
                      <xsl:value-of select="1"/>
                    </xsl:when>
                    <xsl:when test="OldSettlCurrency = CurrencySymbol">
                      <xsl:value-of select="''"/>
                    </xsl:when>
                    <xsl:when test="OldSettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                      <xsl:value-of select="FXRate_Taxlot"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </SettlementExchangeRate>

                <Exchange>
                  <xsl:value-of select="''"/>
                </Exchange>

                <OtherFee>
                  <xsl:value-of select="OldOrfFee + OldSecFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldMiscFees + OldOtherBrokerFees + OldTaxOnCommissions"/>
                </OtherFee>

                <Strategy>
                  <xsl:value-of select="''"/>
                </Strategy>

                <LotNumber>
                  <xsl:value-of select="0"/>
                </LotNumber>

                <LotQuantity>
                  <xsl:value-of select="0"/>
                </LotQuantity>

                <Trader>
                  <xsl:value-of select="''"/>
                </Trader>

                <Interest>
                  <xsl:value-of select="0"/>
                </Interest>

                <Custodian>
                  <xsl:value-of select="''"/>
                </Custodian>

                <WhenIssued>
                  <xsl:value-of select="'N'"/>
                </WhenIssued>

               
                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail> 
            </xsl:if>

            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

				<TaxLotState>
					<xsl:value-of select="'Allocated'"/>
				</TaxLotState>

              <CancelIndicator>
                <xsl:value-of select="'N'"/>
              </CancelIndicator>

              <JefferiesTradeId>
                <xsl:value-of select ="''"/>
              </JefferiesTradeId>

              <ClientTradeId>
                <xsl:value-of select ="EntityID"/>
              </ClientTradeId>

              <Moniker>
                <xsl:value-of select ="'P1754'"/>
              </Moniker>


              <xsl:choose>
                <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                  <TransactionType>
                    <xsl:value-of select ="'BY'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'CS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'SL'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                  <TransactionType>
                    <xsl:value-of select ="'SS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:otherwise>
                  <TransactionType>
                    <xsl:value-of select="Side"/>
                  </TransactionType>
                </xsl:otherwise>
              </xsl:choose>

              <Quantity>
                <xsl:value-of select="CumQty"/>
              </Quantity>

              <xsl:variable name ="varCheckSymbolUnderlying">
                <xsl:value-of select ="substring-before(Symbol,'-')"/>
              </xsl:variable>

              <xsl:choose>
                <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                  <InstrumentId>
                    <xsl:value-of select="SEDOL"/>
                  </InstrumentId>
                </xsl:when>

                <xsl:when test ="Asset ='EquityOption' ">
                  <InstrumentId>
                    <xsl:value-of select="OSIOptionSymbol"/>
                  </InstrumentId>
                </xsl:when>

                <xsl:otherwise>
                  <InstrumentId>
                    <xsl:value-of select="Symbol"/>
                  </InstrumentId>
                </xsl:otherwise>
              </xsl:choose>

              <Price>
                <xsl:value-of select="AvgPrice"/>
              </Price>

           <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JEFF']/FundData[@PranaFund=$PB_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
				 <AccountId>
				  <xsl:choose>
					  <xsl:when test ="AccountID ='1'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754UBS'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201636'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:when test ="AccountID ='2'">
						  <xsl:choose>
							  <xsl:when test ="Asset ='Equity' and IsSwapped='true'">
								  <xsl:value-of select ="'P1754MSIL'"/>
							  </xsl:when>

							  <xsl:when test ="Asset ='EquityOption' or Asset ='Equity'">
								  <xsl:value-of select ="'43201634'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
              </AccountId>

              <xsl:variable name ="varCounterParty">
                <xsl:choose>
                  <xsl:when test="VenueID='1'">
                      <xsl:value-of select="'JBUY'"/>
                    </xsl:when>
					<xsl:when test="VenueID='237'">
                      <xsl:value-of select="'ROTH-M'"/>
                    </xsl:when>
					
                    <xsl:otherwise>
                      <xsl:value-of select="'JBUY'"/>
                    </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <ExecutingBroker>
                <xsl:value-of select ="$varCounterParty"/>
              </ExecutingBroker>
             

              <TradeDate>
                <xsl:value-of select="substring-before(TradeDate,'T')"/>
              </TradeDate>

              <SettleDate>
                <xsl:value-of select ="substring-before(SettlementDate,'T')"/>
              </SettleDate>

              <CommissionType>
                <xsl:value-of select="'T'"/>
              </CommissionType>

              <Commission>
                <xsl:value-of select="CommissionCharged  + SoftCommissionCharged"/>
              </Commission>

              <SellingMethod>
                <xsl:value-of select ="''"/>
              </SellingMethod>

              <Vs_purchases_Date>
                <xsl:value-of select="''"/>
              </Vs_purchases_Date>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementExchangeRate>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' and SettlCurrency='USD'">
                    <xsl:value-of select="1"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SettlementExchangeRate>

              <Exchange>
                <xsl:value-of select="''"/>
              </Exchange>

              <OtherFee>
                <xsl:value-of select="OrfFee + SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFees + TaxOnCommissions"/>
              </OtherFee>

              <Strategy>
                <xsl:value-of select="''"/>
              </Strategy>

              <LotNumber>
                <xsl:value-of select="0"/>
              </LotNumber>

              <LotQuantity>
                <xsl:value-of select="0"/>
              </LotQuantity>

              <Trader>
                <xsl:value-of select="''"/>
              </Trader>

              <Interest>
                <xsl:value-of select="0"/>
              </Interest>

              <Custodian>
                <xsl:value-of select="''"/>
              </Custodian>

              <WhenIssued>
                <xsl:value-of select="'N'"/>
              </WhenIssued>

             
              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
            
           </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
