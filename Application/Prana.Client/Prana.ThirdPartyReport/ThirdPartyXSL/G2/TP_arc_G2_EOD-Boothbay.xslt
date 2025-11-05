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

				<ExternalID>
					<xsl:value-of select="'External ID'"/>
				</ExternalID>

				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>

				<TradeTime>
					<xsl:value-of select="'Trade Time'"/>
				</TradeTime>

				<SecurityType>
					<xsl:value-of select="'Security Type'"/>
				</SecurityType>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<Tax>
					<xsl:value-of select="'Tax'"/>
				</Tax>

				<SecurityCurrency>
					<xsl:value-of select="'Security Currency'"/>
				</SecurityCurrency>

				<Bundle>
					<xsl:value-of select="'Bundle'"/>
				</Bundle>

				<ExecutionBroker>
					<xsl:value-of select="'Execution Broker'"/>
				</ExecutionBroker>

				<Commissions>
					<xsl:value-of select="'Commissions'"/>
				</Commissions>

				<ClearingFee>
					<xsl:value-of select="'Clearing Fee'"/>
				</ClearingFee>

				<OperationCode>
					<xsl:value-of select="'Operation Code'"/>
				</OperationCode>

				<CustodianAccount>
					<xsl:value-of select="'Custodian Account'"/>
				</CustodianAccount>

				<SettleCurrency>
					<xsl:value-of select="'Settle Currency'"/>
				</SettleCurrency>
				
                <!-- <Currency> -->
                    <!-- <xsl:value-of select="'Currency'"/> -->
                <!-- </Currency> -->

                <FXRate>
                    <xsl:value-of select="'FX Rate'"/>
                </FXRate>
				
				<BloombergTicker>
					<xsl:value-of select="'Bloomberg Ticker'"/>					
				</BloombergTicker>

				<ExchangeFee>
					<xsl:value-of select="'Exchange Fee'"/>
				</ExchangeFee>

				<TradeQuantity>
					<xsl:value-of select="'Trade Quantity'"/>
				</TradeQuantity>

				<PrimeBroker>
					<xsl:value-of select="'Prime Broker'"/>
				</PrimeBroker>

				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<TradePrice>
					<xsl:value-of select="'Trade Price'"/>
				</TradePrice>

				<Book>
					<xsl:value-of select="'Book'"/>
				</Book>

				<NFAFee>
					<xsl:value-of select="'NFA Fee'"/>
				</NFAFee>

				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>
				<FinancingType>
						<xsl:value-of select="'Financing Type'"/>
					</FinancingType>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Boothbay Absolute Return Strategies' or AccountName='Boothbay Diversified Alpha Master Fund'][CounterParty!='CorpAction']">

				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<ExternalID>
						<xsl:value-of select="concat('G2_',EntityID)"/>
					</ExternalID>


					<Side>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="(Side='Buy to Cover' or Side='Buy to Close')">
								<xsl:value-of select="'BC'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<TradeTime>
						<xsl:value-of select="substring-before(substring-after(TradeDateTime,' '),' ')"/>
					</TradeTime>
					
					
					<xsl:variable name="varAsset">
						<xsl:choose>
							<xsl:when test="Exchange = 'BasketSwap'">
								<xsl:value-of select="'Index'"/>
							</xsl:when>
							<xsl:when test="Exchange = 'FX'">
								<xsl:value-of select="'Spot'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<SecurityType>
						<xsl:value-of select="$varAsset"/>
					</SecurityType>

					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<Tax>
						<xsl:value-of select="SecFee"/>
					</Tax>

					<SecurityCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SecurityCurrency>

					<Bundle>
						<xsl:value-of select="'G2_Default'"/>
					</Bundle>

					<ExecutionBroker>
						<xsl:value-of select ="CounterParty"/>
					</ExecutionBroker>
					
					
					<Commissions>
						<xsl:value-of select="CommissionCharged"/>
					</Commissions>

					<ClearingFee>
						<xsl:value-of select="''"/>
					</ClearingFee>
					
					<xsl:variable name="varTransactionAction">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<OperationCode>
						<xsl:value-of select="$varTransactionAction"/>
					</OperationCode>
					
                  <xsl:variable name="PB_NAME">
						<xsl:value-of select="''"/>
					</xsl:variable>

					<xsl:variable name="PB_FUND_NAME" select="AccountName"/>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFundName=$PB_FUND_NAME]/@PBFundName"/>
					</xsl:variable>
					<CustodianAccount>
						<!-- <xsl:choose> -->
							<!-- <xsl:when test ="$PRANA_FUND_NAME!=''"> -->
								<!-- <xsl:value-of select ="$PRANA_FUND_NAME"/> -->
							<!-- </xsl:when> -->

							<!-- <xsl:otherwise> -->
								<!-- <xsl:value-of select ="$PB_FUND_NAME"/> -->
							<!-- </xsl:otherwise> -->
						<!-- </xsl:choose> -->
						<xsl:choose>
              <xsl:when test ="AccountName= 'Boothbay Absolute Return Strategies'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select ="'06178G9X7'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                    <xsl:value-of select ="'038CAO3T1'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FX' or Asset='FXForward'">
                    <xsl:value-of select ="'0581CZX78'"/>
                  </xsl:when>
                </xsl:choose>              
              </xsl:when>
              
              <xsl:when test ="AccountName= 'Boothbay Diversified Alpha Master Fund'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select ="'06178G9W9'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                    <xsl:value-of select ="'038CAO3U8'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FX' or Asset='FXForward'">
                    <xsl:value-of select ="'0581CZX86'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
					</CustodianAccount>


		<xsl:variable name="varBuyCurrency">
			<xsl:choose>
				<xsl:when test="Side='Buy'">
					<xsl:value-of select="LeadCurrencyName"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="VsCurrencyName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="varSellCurrency">
			<xsl:choose>
				<xsl:when test="Side='Buy' ">
					<xsl:value-of select="VsCurrencyName"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="LeadCurrencyName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
					<SettleCurrency>						
						<xsl:choose>						
							 <xsl:when test="(Asset ='FX' or Asset ='FXForward')">
							   <xsl:value-of select="VsCurrencyName"/>
							 </xsl:when>
							 <xsl:otherwise>
								  <xsl:value-of select="SettlCurrency"/>
							 </xsl:otherwise>
						 </xsl:choose>
					</SettleCurrency>
					
                    <!-- <Currency> -->
                        <!-- <xsl:value-of select="''"/> -->
                    <!-- </Currency> -->

                     <FXRate>
					  <xsl:choose>						
							 <xsl:when test="(Asset !='FX' or Asset !='FXForward') and SettlCurrency = CurrencySymbol">
							   <xsl:value-of select="'1'"/>
							 </xsl:when>
							 <xsl:otherwise>
								  <xsl:value-of select="FXRate_Taxlot"/>
							 </xsl:otherwise>
						 </xsl:choose>                   
                     </FXRate>
					 
					<BloombergTicker>
						<xsl:value-of select="BBCode"/>
					</BloombergTicker>

					<ExchangeFee>
						<xsl:value-of select="''"/>
					</ExchangeFee>

					<TradeQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</TradeQuantity>

					<PrimeBroker>
						<xsl:value-of select ="'Morgan Stanley &amp; CO LLC'"/>
					</PrimeBroker>
					
					<SettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SettlementDate>					

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>

					<TradePrice>
						<xsl:value-of select="AveragePrice"/>
					</TradePrice>

					<Book>
						 <xsl:choose>
                   <xsl:when test ="AccountName= 'Boothbay Diversified Alpha Master Fund'">
                    <xsl:value-of select ="'BDAF_G2'"/>
                  </xsl:when>
                  <xsl:when test ="AccountName= 'Boothbay Absolute Return Strategies'">
                  	<xsl:value-of select="'BBARS_G2'"/>
                  </xsl:when>                 
                </xsl:choose>
					</Book>

					<NFAFee>
						<xsl:value-of select="''"/>
					</NFAFee>

					<TransactionType>
						<xsl:value-of select="'Trade'"/>
					</TransactionType>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<FinancingType>
					 <xsl:choose>
                   
                 <xsl:when test=" IsSwapped='true'">
                  	<xsl:value-of select="'TRS'"/>
                  </xsl:when> 
				<xsl:otherwise>
                <xsl:value-of select ="'NONE'"/>
              </xsl:otherwise>				  
					</xsl:choose>
					</FinancingType>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>